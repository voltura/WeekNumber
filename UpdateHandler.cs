#region Using statements

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

#endregion Using statements

namespace WeekNumber
{
    internal static class UpdateHandler
    {
        #region Private constants

        private static readonly string VERSION_CHECK_BASE_URL = "http://github.com/voltura/weeknumber/releases/latest/download/";
        private static readonly string VERSION_CHECK_URL = $"{VERSION_CHECK_BASE_URL}VERSION.TXT";
        internal static readonly string APPLICATION_URL = "https://voltura.github.io/WeekNumber/";

        #endregion Private constants

        #region Internal struct

        internal struct VersionInfo
        {
            public bool Error;
            public string Version;
            public string Installer;
        }

        #endregion Internal struct

        #region Internal methods

        internal static void UpdateClick(object sender, EventArgs e)
        {
            Log.LogCaller();
            PerformUpdateCheck();
        }

        internal static void OpenApplicationWebPageClick(object sender, EventArgs e)
        {
            Log.LogCaller();
            try
            {
                using (Process process = new Process() { StartInfo = new ProcessStartInfo(APPLICATION_URL) { UseShellExecute = true } })
                {
                    process.Start();
                }
            }
            catch (InvalidOperationException ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
        }

        internal static void PerformUpdateCheck(bool silent = false)
        {
            Log.LogCaller();
            string runningVersion = Application.ProductVersion;
            VersionInfo internetVersionInfo = GetInternetVersion(silent);
            if (internetVersionInfo.Error) return;
            if (!IsNewerVersion(runningVersion, internetVersionInfo.Version))
            {
                LogAndShow($"\r\n\r\n{Resources.LatestVersionInstalled}", silent);
                return;
            }
            if (silent || Message.UserAcceptedQuestion($@"{Resources.NewVersionAvailable}

{Resources.InstalledVersion} {runningVersion}
{Resources.AvailableVersion} {internetVersionInfo.Version}

{Resources.DownloadAndInstallQuestion}"))
            {
                try
                {
                    string destinationFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp",
                        internetVersionInfo.Installer);
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(VERSION_CHECK_BASE_URL + internetVersionInfo.Installer + ".MD5", destinationFullPath + ".MD5");
                        client.DownloadFile(VERSION_CHECK_BASE_URL + internetVersionInfo.Installer, destinationFullPath);
                    }
                    try
                    {
                        if (File.Exists(destinationFullPath) &&
                            File.Exists(destinationFullPath + ".MD5"))
                        {
                            //remove smartscreen filter (alternative data stream Zone.Identifier) on downloaded installer executable file
                            UnblockFile(destinationFullPath);
                            //validate installer checksum
                            string installerMD5 = CalculateMD5(destinationFullPath);
                            string installerInternetMD5 = File.ReadAllText(destinationFullPath + ".MD5").PadRight(32).Substring(0, 32);
                            if (installerMD5 != installerInternetMD5)
                            {
                                LogAndShow($@"{Resources.FailedAutoInstall}
{Resources.InvalidChecksumCouldNotAutoInstall}
{Resources.CheckForNewVersionHere} {VERSION_CHECK_BASE_URL}", silent, new Exception($"{Resources.FailedAutoInstall} {Resources.InvalidChecksumCouldNotAutoInstall}"));
                                return;
                            }
                            Settings.BackupSettings();
                            //Start installer + close current app
                            using (Process process = new Process()
                            {
                                StartInfo = new ProcessStartInfo(destinationFullPath)
                                {
                                    UseShellExecute = true,
                                    CreateNoWindow = silent,
                                    WindowStyle = silent ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                                    Arguments = silent ? "/S" : string.Empty
                                }
                            })
                            {
                                process.Start();
                            }
                            Application.Exit();
                        }
                        else
                        {
                            LogAndShow($@"{Resources.FailedAutoInstall}

{Resources.CheckForNewVersionHere} {VERSION_CHECK_BASE_URL}", silent, new Exception(Resources.FailedAutoInstall));
                            return;
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        LogAndShow($@"{Resources.FailedToStartInstaller}

{Resources.CloseAppAndManuallyRun} {destinationFullPath} {Resources.ToUpdateTheApplication}", silent, ex);
                        return;
                    }
                }
                catch (WebException we)
                {
                    // The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.-or-
                    // An error occurred while downloading the resource.
                    LogAndShow($@"{Resources.FailedToDownloadNewVersion}

{Resources.TryDownloadingManuallyFromThisAddress}

{VERSION_CHECK_BASE_URL}", silent, we);
                    return;
                }
                catch (NotSupportedException nse)
                {
                    // The method has been called simultaneously on multiple threads.
                    LogAndShow($@"{Resources.FailedToDownloadNewVersion}

{Resources.TryDownloadingManuallyFromThisAddress}

{VERSION_CHECK_BASE_URL}", silent, nse);
                }
            }
        }

        #endregion Internal methods

        #region Private methods

        private static VersionInfo GetInternetVersion(bool silent)
        {
            Log.LogCaller();
            VersionInfo vi = new VersionInfo
            {
                Version = "0.0.0.0",
                Error = true
            };
            if (!NativeMethods.IsConnectedToInternet())
            {
                LogAndShow(Resources.FailedToCheckUpdateNoInternet, silent, new Exception(Resources.FailedToCheckUpdateNoInternet));
                return vi;
            }
            string versionInfoFromInternet;

            try
            {
                using (WebClient client = new WebClient())
                {
                    versionInfoFromInternet = client.DownloadString(VERSION_CHECK_URL).Replace('\r', ' ').Replace('\n', ' ').TrimEnd();
                }
                Log.Info = $"versionInfoFromInternet='{versionInfoFromInternet}'";
            }
            catch (WebException we)
            {
                // The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.-or-
                // An error occurred while downloading the resource.
                LogAndShow($@"{Resources.FailedToPerformVersionCheck} 

{Resources.CheckBrowserNavigation}
{VERSION_CHECK_BASE_URL}", silent, we);
                return vi;
            }
            catch (NotSupportedException nse)
            {
                // The method has been called simultaneously on multiple threads.
                LogAndShow($@"{Resources.FailedToPerformVersionCheck}

{Resources.CheckForNewVersionHere}
{VERSION_CHECK_BASE_URL}", silent, nse);
                return vi;
            }
            string[] versionInfo = versionInfoFromInternet.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (versionInfo.Length != 2)
            {
                LogAndShow($@"{Resources.FailedToPerformVersionCheck}

{Resources.UnableToParse} '{VERSION_CHECK_URL}' {Resources.Information}.", silent, new InvalidDataException(Resources.FailedToPerformVersionCheck));
                return vi;
            }
            vi.Error = false;
            vi.Version = versionInfo[0];
            vi.Installer = versionInfo[1];
            return vi;
        }

        /// <summary>
        /// Check if a newer version exist
        /// </summary>
        /// <param name="existingVersion"></param>
        /// <param name="internetVersion"></param>
        /// <returns></returns>
        private static bool IsNewerVersion(string existingVersion, string internetVersion)
        {
            Log.LogCaller();
            bool result = existingVersion != internetVersion;
            char[] dotSeparator = new char[] { '.' };
            string[] existingVersionParts = existingVersion.Split(dotSeparator);
            string[] internetVersionParts = internetVersion.Split(dotSeparator);

            bool parseVer = Int32.TryParse(existingVersionParts[0], out int eMajor);
            if (!parseVer) return result;
            parseVer = Int32.TryParse(internetVersionParts[0], out int iMajor);
            if (!parseVer) return result;
            if (iMajor > eMajor) return true;
            parseVer = Int32.TryParse(existingVersionParts[1], out int eMinor);
            if (!parseVer) return result;
            parseVer = Int32.TryParse(internetVersionParts[1], out int iMinor);
            if (!parseVer) return result;
            if (iMinor > eMinor) return true;
            parseVer = Int32.TryParse(existingVersionParts[2], out int eBuild);
            if (!parseVer) return result;
            parseVer = Int32.TryParse(internetVersionParts[2], out int iBuild);
            if (!parseVer) return result;
            if (iBuild > eBuild) return true;
            parseVer = Int32.TryParse(existingVersionParts[3], out int eRevision);
            if (!parseVer) return result;
            parseVer = Int32.TryParse(internetVersionParts[3], out int iRevision);
            if (!parseVer) return result;
            if (iRevision > eRevision) return true; else return false;
        }

        private static void LogAndShow(string msg, bool silent, Exception ex = null)
        {
            if (ex is null)
            {
                Log.Info = msg;
                if (!silent) Message.Show(msg);
            }
            else
            {
                Log.ErrorString = msg;
                Log.Error = ex;
                if (!silent) Message.Show($"\r\n\r\n{msg}", isError: ex != null);
            }
        }

        private static string CalculateMD5(string filename)
        {
            Log.LogCaller();
            string fileMD5Hash = string.Empty;
            try
            {
                if (File.Exists(filename))
                {
                    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                    {
                        using (FileStream stream = File.OpenRead(filename))
                        {
                            byte[] hash = md5.ComputeHash(stream);
                            fileMD5Hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                            Log.Info = $"Calculated MD5 hash for file '{filename}' to '{fileMD5Hash}'";
                        }
                    }
                }
                else
                {
                    Log.ErrorString = $"Cannot calculate MD5 hash for file '{filename}' since the file does not exist.";
                }
            }
            catch (Exception ex)
            {
                Log.Error = ex;
            }
            return fileMD5Hash;
        }

        /// <summary>
        /// Removes Zone Identification tagging that are using Alternate Data Streams (Zone.Identifier) created on file 
        /// when downloaded from internet, also called 'Mark of the Web'
        /// Allows to execute file without Microsoft Defender SmartScreen interferance.
        /// See https://www.winhelponline.com/blog/bulk-unblock-files-downloaded-internet/ for more info
        /// </summary>
        /// <param name="fullPath"></param>
        private static bool UnblockFile(string fullPath)
        {
            Log.LogCaller();
            bool result = false;
            string parameters = @"-Command ""& {Unblock-File -Path """ + fullPath + @""" }""";
            try
            {
                if (!File.Exists(fullPath))
                {
                    Log.ErrorString = $"File '{fullPath}' not found.";
                    return result;
                }
                using (Process p = new Process()
                {
                    StartInfo = new ProcessStartInfo("powershell.exe", parameters)
                    {
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true
                    }
                })
                {
                    Log.Info = $"About to unblock file '{fullPath}'...";
                    if (p.Start())
                    {
                        result = p.WaitForExit(10000);
                    }
                    else
                    {
                        Log.ErrorString = $"Failed to start 'powershell.exe {parameters}'";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error = ex;
            }
            finally
            {
                if (result)
                {
                    Log.Info = $"File '{fullPath}' unblocked successfully.";
                }
                else
                {
                    Log.ErrorString = $"Failed unblock file '{fullPath}'.";
                }
            }
            return result;
        }

        #endregion Private methods
    }
}
