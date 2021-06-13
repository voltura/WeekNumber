#region Using statements

using Microsoft.Win32;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#endregion Using statements

namespace WeekNumber
{
    internal static class Settings
    {
        #region Internal static property that updates registry for application to start when Windows start

        internal static bool StartWithWindows
        {
            get
            {
                bool startWithWindows = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run\", Application.ProductName, null) != null;
                return startWithWindows;
            }
            set
            {
                using (RegistryKey registryKey = Registry.CurrentUser)
                using (RegistryKey regRun = registryKey?.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\", true))
                {
                    if (value)
                    {
                        regRun?.SetValue(Application.ProductName, Application.ExecutablePath);
                        UpdateSetting(Resources.StartWithWindows, true.ToString());
                    }
                    else
                    {
                        if (regRun?.GetValue(Application.ProductName) != null)
                            regRun?.DeleteValue(Application.ProductName);
                        UpdateSetting(Resources.StartWithWindows, false.ToString());
                    }
                    registryKey?.Flush();
                }
            }
        }

        #endregion Internal static property that updates registry for application to start when Windows start

        #region Internal static methods

        internal static bool SettingIsValue(string setting, string value)
        {
            CreateSettings();
            return ConfigurationManager.AppSettings.Get(setting) == value;
        }

        internal static string GetSetting(string setting)
        {
            CreateSettings();
            return ConfigurationManager.AppSettings.Get(setting);
        }

        internal static int GetIntSetting(string setting, int defaultValue = 0)
        {
            if (int.TryParse(GetSetting(setting), out int settingInt))
            {
                return settingInt;
            }
            return defaultValue;
        }

        internal static void UpdateSetting(string setting, string value)
        {
            CreateSettings();
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
            settings[setting].Value = value;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            Log.Info = $"'{setting}' set to '{value}'";
        }

        /// <summary>
        /// Creates a backup of the applications current settings file
        /// </summary>
        internal static bool BackupSettings(string fileName = "")
        {
            Log.LogCaller();
            string settingsFile = Application.ExecutablePath + ".config";
            string settingsBackupDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");
            string settingsFileBackup = Path.Combine(settingsBackupDir, Application.ExecutablePath + ".config.bak");
            if (fileName != string.Empty)
            {
                settingsBackupDir = Path.GetDirectoryName(fileName);
                settingsFileBackup = fileName;
            }
            try
            {
                if (settingsFile.Equals(settingsFileBackup, StringComparison.InvariantCultureIgnoreCase))
                {
                    Log.ErrorString = $"Cannot create copy of settings file since it has same fullpath as original: '{settingsFileBackup}'";
                    return false;
                }
                if (File.Exists(settingsFile))
                {
                    if (!Directory.Exists(settingsBackupDir)) Directory.CreateDirectory(settingsBackupDir);
                    File.Copy(settingsFile, settingsFileBackup, true);
                    Log.Info = $"Copy of settings file created: '{settingsFileBackup}'.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error = ex;
            }
            Log.ErrorString = $"Failed to create copy of settings file: '{settingsFileBackup}'";
            return false;
        }

        internal static void RestoreBackupSettings()
        {
            Log.LogCaller();
            string settingsFile = Application.ExecutablePath + ".config";
            string settingsBackupDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");
            string settingsFileBackup = Path.Combine(settingsBackupDir, Application.ExecutablePath + ".config.bak");
            try
            {
                if (!File.Exists(settingsFileBackup)) return;
                if (!File.Exists(settingsFile)) CreateSettings();
                XmlDocument doc = new XmlDocument();
                doc.Load(settingsFileBackup);
                XmlNodeList nodeList = doc.SelectNodes("/configuration/appSettings");
                foreach (XmlNode node in nodeList)
                    foreach (XmlNode child in node.ChildNodes)
                        if (child.Name == "add")
                        {
                            XmlAttributeCollection attribs = child.Attributes;
                            if (attribs.Count == 2)
                            {
                                string settingsName = attribs[0].Value;
                                string settingsValue = attribs[1].Value;
                                try
                                {
                                    UpdateSetting(settingsName, settingsValue);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error = ex;
                                }
                            }
                        }
                File.Delete(settingsFileBackup);
                Log.Info = "Removed backup settings file after restore.";
            }
            catch (Exception ex)
            {
                Log.Error = ex;
            }
        }

        /// <summary>
        /// Import settings from file
        /// </summary>
        /// <param name="fileToImport"></param>
        /// <returns></returns>
        internal static bool ImportSettings(string fileToImport)
        {
            Log.LogCaller();
            string settingsFile = Application.ExecutablePath + ".config";
            try
            {
                if (!File.Exists(fileToImport))
                {
                    Log.ErrorString = $"Settings file '{fileToImport}' not found, no import made.";
                    return false;
                }
                if (!File.Exists(settingsFile)) CreateSettings();
                XmlDocument doc = new XmlDocument();
                doc.Load(fileToImport);
                XmlNodeList nodeList = doc.SelectNodes("/configuration/appSettings");
                foreach (XmlNode node in nodeList)
                    foreach (XmlNode child in node.ChildNodes)
                        if (child.Name == "add")
                        {
                            XmlAttributeCollection attribs = child.Attributes;
                            if (attribs.Count == 2)
                            {
                                string settingsName = attribs[0].Value;
                                string settingsValue = attribs[1].Value;
                                try
                                {
                                    UpdateSetting(settingsName, settingsValue);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error = ex;
                                }
                            }
                        }
            }
            catch (Exception ex)
            {
                Log.ErrorString = $"Failed to import settings file '{fileToImport}'.";
                Log.Error = ex;
                return false;
            }
            Log.Info = $"Imported settings from '{fileToImport}'.";
            return true;
        }

        /// <summary>
        /// Set application culture info
        /// </summary>
        internal static void SetCultureInfoFromSystemOrSettings()
        {
            Log.LogCaller();
            try
            {
                string language = GetSetting(Resources.Language);
                if (string.IsNullOrEmpty(language)) // First run there is no setting for language
                {
                    if (CultureInfo.CurrentCulture.Name == Resources.Swedish)
                    {
                        language = Resources.Swedish;
                    }
                    else
                    {
                        language = Resources.English;
                    }
                    UpdateSetting(Resources.Language, language);
                }
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(language, false);
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(language, false);
                Log.Info = $"Set application Culture to '{language}'";
            }
            catch (Exception ex)
            {
                Log.Error = ex;
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(Resources.English, false);
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Resources.English, false);
                Log.Info = $"Set application Culture to '{Resources.English}'";
            }
        }


        #endregion Internal static methods

        #region Private method that creates the application settings file if needed

        private static void CreateSettings()
        {
            string settingsFile = Application.ExecutablePath + ".config";
            if (!File.Exists(settingsFile))
            {
                Log.LogCaller();
                CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;
                DayOfWeek firstDay = currentCultureInfo.DateTimeFormat.FirstDayOfWeek;
                CalendarWeekRule calendarWeekRule = currentCultureInfo.DateTimeFormat.CalendarWeekRule;
                string xml = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <appSettings>
    <add key=""DayOfWeek"" value=""{firstDay}""/>
    <add key=""CalendarWeekRule"" value=""{calendarWeekRule}""/>
    <add key=""IconBackgroundRed"" value=""0""/>
    <add key=""IconBackgroundGreen"" value=""0""/>
    <add key=""IconBackgroundBlue"" value=""0""/>
    <add key=""IconForegroundRed"" value=""255""/>
    <add key=""IconForegroundGreen"" value=""255""/>
    <add key=""IconForegroundBlue"" value=""255""/>
    <add key=""IconResolution"" value=""""/>
    <add key=""StartWithWindows"" value=""False""/>
    <add key=""MaxLogFileSizeInMB"" value=""10""/>
    <add key=""UseApplicationLog"" value=""False""/>
    <add key=""AutoUpdate"" value=""False""/>
    <add key=""DisplayStartupNotification"" value=""True""/>
    <add key=""DisplayWeekChangedNotification"" value=""True""/>
    <add key=""UseSilentNotifications"" value=""True""/>
    <add key=""Language"" value=""""/>
    <add key=""SmoothingMode"" value=""2""/>
    <add key=""CompositingQuality"" value=""2""/>
    <add key=""InterpolationMode"" value=""2""/>
    <add key=""TextContrast"" value=""1""/>
  </appSettings>
</configuration>";
                File.WriteAllText(settingsFile, xml, System.Text.Encoding.UTF8);
                Log.Info = $"Created '{settingsFile}'";
            }
        }

        #endregion Private method that creates the application settings file if needed
    }
}