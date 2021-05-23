#region Using statements

using Microsoft.Win32;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

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

        #endregion Internal static methods

        #region Private method that creates the application settings file if needed

        private static void CreateSettings()
        {
            string settingsFile = Application.ExecutablePath + ".config";
            if (!File.Exists(settingsFile))
            {
                Log.LogCaller();
                CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;
                System.DayOfWeek firstDay = currentCultureInfo.DateTimeFormat.FirstDayOfWeek;
                CalendarWeekRule calendarWeekRule = currentCultureInfo.DateTimeFormat.CalendarWeekRule;
                string xml = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <appSettings>
    <add key=""DayOfWeek"" value=""{firstDay}""/>
    <add key=""CalendarWeekRule"" value=""{calendarWeekRule}"" />
    <add key=""BackgroundR"" value=""0""/>
    <add key=""BackgroundG"" value=""0""/>
    <add key=""BackgroundB"" value=""0""/>
    <add key=""ForegroundR"" value=""255""/>
    <add key=""ForegroundG"" value=""255""/>
    <add key=""ForegroundB"" value=""255""/>
    <add key=""ForceRedraw"" value=""False""/>
    <add key=""IconResolution"" value=""""/>
    <add key=""StartWithWindows"" value=""False""/>
    <add key=""logFileSizeMB"" value=""10""/>
    <add key=""UseApplicationLog"" value=""False""/>
  </appSettings>
</configuration>";
                File.WriteAllText(settingsFile, xml, System.Text.Encoding.UTF8);
                Log.Info = $"Created '{settingsFile}'";
            }
        }

        #endregion Private method that creates the application settings file if needed
    }
}