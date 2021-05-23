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
        private static readonly string APPLICATION_URL = "https://voltura.github.io/WeekNumber";

        #endregion Private constants

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
            if (!NativeMethods.IsConnectedToInternet())
            {
                if (silent)
                {
                    Log.ErrorString = Resources.FailedToCheckUpdateNoInternet;
                }
                else
                {
                    Message.Show(Resources.FailedToCheckUpdateNoInternet, isError: true);
                }

                return;
            }
            string runningVersion = Application.ProductVersion;
            string versionInfoFromInternet;

            using (WebClient client = new WebClient())
            {
                try
                {
                    versionInfoFromInternet = client.DownloadString(VERSION_CHECK_URL);
                    Log.Info = $"versionInfoFromInternet='{versionInfoFromInternet}'";
                }
                catch (WebException we)
                {
                    //     The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.-or-
                    //     An error occurred while downloading the resource.
                    if (silent)
                    {
                        Log.ErrorString = Resources.FailedToPerformVersionCheck;
                    }
                    else
                    {
                        Message.Show($@"{Resources.FailedToPerformVersionCheck} 

{Resources.CheckBrowserNavigation}
{VERSION_CHECK_BASE_URL}", we);
                    }
                    return;
                }
                catch (NotSupportedException nse)
                {
                    //     The method has been called simultaneously on multiple threads.
                    if (silent)
                    {
                        Log.ErrorString = Resources.FailedToPerformVersionCheck;
                    }
                    else
                    {
                        Message.Show($@"{Resources.FailedToPerformVersionCheck}

{Resources.CheckForNewVersionHere}
{VERSION_CHECK_BASE_URL}", nse);
                    }
                    return;
                }
                string[] versionInfo = versionInfoFromInternet.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (versionInfo.Length != 2)
                {
                    string unableToParse = $"Unable to parse '{VERSION_CHECK_URL}' information.";
                    if (silent)
                    {
                        Log.ErrorString = Resources.FailedToPerformVersionCheck;
                    }
                    else
                    {
                        Message.Show($@"{Resources.FailedToPerformVersionCheck}

{unableToParse}", isError: true);
                    }
                    return;
                }
                string internetVersion = versionInfo[0];
                string latestInstallerFileName = versionInfo[1];

                if (!IsNewerVersion(runningVersion, internetVersion))
                {
                    if (silent)
                    {
                        Log.Info = Resources.LatestVersionInstalled;
                    }
                    else
                    {
                        Message.Show(Resources.LatestVersionInstalled);
                    }
                    return;
                }
                if (silent || Message.UserAcceptedQuestion($@"{Resources.NewVersionAvailable}

{Resources.InstalledVersion} {runningVersion}
{Resources.AvailableVersion} {internetVersion}

{Resources.DownloadAndInstallQuestion}"))
                {
                    try
                    {
                        string destinationFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", latestInstallerFileName);
                        client.DownloadFile(VERSION_CHECK_BASE_URL + latestInstallerFileName, destinationFullPath);
                        //Start installer + close current app
                        try
                        {
                            if (File.Exists(destinationFullPath))
                            {
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
                                if (silent)
                                {
                                    Log.ErrorString = Resources.FailedAutoInstall;
                                }
                                else
                                {
                                    Message.Show($@"{Resources.FailedAutoInstall}

{Resources.CheckForNewVersionHere} { VERSION_CHECK_BASE_URL}", isError: true);
                                }
                                return;
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            if (silent)
                            {
                                Log.ErrorString = Resources.FailedToStartInstaller;
                            }
                            else
                            {
                                Message.Show($@"{Resources.FailedToStartInstaller}

Close this application and manually run {destinationFullPath} to update the application.", ex);
                            }
                            return;
                        }
                    }
                    catch (WebException we)
                    {
                        //     The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.-or-
                        //     An error occurred while downloading the resource.
                        if (silent)
                        {
                            Log.ErrorString = Resources.FailedToDownloadNewVersion;
                        }
                        else
                        {
                            Message.Show($@"{Resources.FailedToDownloadNewVersion}

Check if you can download it maually via a web browser from this address:

{VERSION_CHECK_BASE_URL}", we);
                        }
                    }
                    catch (NotSupportedException nse)
                    {
                        //     The method has been called simultaneously on multiple threads.
                        if (silent)
                        {
                            Log.ErrorString = Resources.FailedToDownloadNewVersion;
                        }
                        else
                        {
                            Message.Show($@"{Resources.FailedToDownloadNewVersion}

Try manually downloading it via a web browser from this address:

{VERSION_CHECK_BASE_URL}", nse);
                        }
                    }
                }
            }
        }

        #endregion Internal methods

        #region Private methods

        /// <summary>
        /// Check if a newer version exist
        /// </summary>
        /// <param name="existingVersion"></param>
        /// <param name="internetVersion"></param>
        /// <returns></returns>
        private static bool IsNewerVersion(string existingVersion, string internetVersion)
        {
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

        #endregion Private methods
    }
}
