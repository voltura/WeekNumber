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
            if (!NativeMethods.IsConnectedToInternet())
            {
                Message.Show(Resources.FailedToCheckUpdateNoInternet, isError: true);
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
                    Message.Show($@"Failed to perform version check. 

Check if you can navigate via a web browser to this address:
{VERSION_CHECK_BASE_URL}", we);
                    return;
                }
                catch (NotSupportedException nse)
                {
                    //     The method has been called simultaneously on multiple threads.
                    Message.Show($@"Failed to perform version check.

Manually check for newer version here:
{VERSION_CHECK_BASE_URL}", nse);
                    return;
                }
                string[] versionInfo = versionInfoFromInternet.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (versionInfo.Length != 2)
                {
                    string unableToParse = $"Unable to parse '{VERSION_CHECK_URL}' information.";
                    Message.Show($@"Failed to perform version check.

{unableToParse}", isError: true);
                    return;
                }
                string internetVersion = versionInfo[0];
                string latestInstallerFileName = versionInfo[1];

                if (!IsNewerVersion(runningVersion, internetVersion))
                {
                    Message.Show($@"You have the latest version!");
                    return;
                }
                if (Message.UserAcceptedQuestion($@"There is a new version available!

Your version: {runningVersion}
New version: {internetVersion}

Download and install new version now?"))
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
                                using (Process process = new Process() { StartInfo = new ProcessStartInfo(destinationFullPath) { UseShellExecute = true } })
                                {
                                    process.Start();
                                }
                                Application.Exit();
                            }
                            else
                            {
                                Message.Show($@"Failed to download and run the installer automatically.

Manually check for newer version here { VERSION_CHECK_BASE_URL}", isError: true);
                                return;
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            Message.Show($@"Failed to start the installer automatically.

Close this application and manually run {destinationFullPath} to update the application.", ex);
                            return;
                        }
                    }
                    catch (WebException we)
                    {
                        //     The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.-or-
                        //     An error occurred while downloading the resource.
                        Message.Show($@"Failed to download new version.

Check if you can download it maually via a web browser from this address:

{VERSION_CHECK_BASE_URL}", we);
                    }
                    catch (NotSupportedException nse)
                    {
                        //     The method has been called simultaneously on multiple threads.
                        Message.Show($@"Failed to download new version.

Try manually downloading it via a web browser from this address:

{VERSION_CHECK_BASE_URL}", nse);
                    }
                }
            }
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

        #endregion Internal methods

        #region Private methods

        /// <summary>
        /// Ugly code to check if a newer version exist, need refactoring
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
