#region Using statements

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

#endregion
namespace WeekNumber
{
    internal static class Log
    {
        #region Static variables

        private static readonly string logFile = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
        private static readonly string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
            Application.CompanyName, Application.ProductName);
        private static readonly string logFileFullPath = Path.Combine(appDataFolder, logFile);
        private static readonly int DEFAULT_LOGFILE_SIZE_IN_MEGABYTES = 10;

        #endregion

        #region Internal static constructor

        /// <summary>
        ///     Constructor - Init log
        /// </summary>
        static Log()
        {
            Init();
        }

        #endregion

        #region Internal methods

        /// <summary>
        ///     Init log
        /// </summary>
        internal static void Init()
        {
            try
            {
                if (!Directory.Exists(appDataFolder))
                {
                    Directory.CreateDirectory(appDataFolder);
                }
                Trace.Listeners.Clear();
                if (!Settings.SettingIsValue(Resources.UseApplicationLog, "True"))
                {
                    Trace.Listeners.Clear();
                    Trace.Flush();
                    Trace.Close();
                    if (File.Exists(logFileFullPath)) File.Delete(logFileFullPath);
                    return;
                }
                FileStream fs = new FileStream(logFileFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete, 1024, FileOptions.WriteThrough);
                TextWriterTraceListener traceListener = new TextWriterTraceListener(fs);
                Trace.Listeners.Add(traceListener);
                Trace.AutoFlush = true;
                Trace.UseGlobalLock = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        internal static void Close(string info)
        {
            Info = info;
            Close();
        }

        internal static void Close()
        {
            Truncate();
            Trace.Flush();
            Trace.Close();
        }

        /// <summary>
        ///     Truncate log
        /// </summary>
        internal static void Truncate()
        {
            try
            {
                Trace.Flush();
                Trace.Close();
                Trace.Listeners.Clear();
                FileInfo fi = new FileInfo(logFileFullPath);
                if (fi.Exists)
                {
                    int trimSize = Settings.GetIntSetting(Resources.logFileSizeMB, DEFAULT_LOGFILE_SIZE_IN_MEGABYTES) * 1024 * 1024;
                    if (fi.Length > trimSize)
                    {
                        using (MemoryStream ms = new MemoryStream(trimSize))
                        using (FileStream s = new FileStream(logFileFullPath, FileMode.Open, FileAccess.ReadWrite))
                        {
                            s.Seek(-trimSize, SeekOrigin.End);
                            byte[] bytes = new byte[trimSize];
                            s.Read(bytes, 0, trimSize);
                            ms.Write(bytes, 0, trimSize);
                            ms.Position = 0;
                            s.SetLength(trimSize);
                            s.Position = 0;
                            ms.CopyTo(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            Init();
        }

        internal static void Show(string info)
        {
            Info = info;
            Show();
        }

        internal static void Show()
        {
            try
            {
                if (File.Exists(logFileFullPath))
                {
                    using (Process process = new Process() { StartInfo = new ProcessStartInfo(logFileFullPath) { UseShellExecute = true } })
                    {
                        process.Start();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Error = ex;
            }
        }

        internal static void LogCaller()
        {
            StackTrace stackTrace = new StackTrace();
            string method = stackTrace.GetFrame(1).GetMethod().Name;
            string methodClass = stackTrace.GetFrame(1).GetMethod().DeclaringType.FullName;
            string calleeMethod = stackTrace.GetFrame(2).GetMethod().Name;
            string calleeClass = stackTrace.GetFrame(2).GetMethod().DeclaringType.FullName;
            string infoText = $"{methodClass}::{(method == ".ctor" ? "constructor" : method)} called from {calleeClass}::{(calleeMethod == ".ctor" ? "constructor" : calleeMethod)}";
            Info = infoText;
        }

        #endregion

        #region Internal static log properties

        /// <summary>
        ///     Log info
        /// </summary>
        internal static string Info
        {
            private get => string.Empty;
            set
            {
                try
                {
                    string formattedValue = value.Replace('\r', ' ').Replace('\n', ' ').Trim();
                    Trace.TraceInformation($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff", CultureInfo.InvariantCulture)} {formattedValue}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        ///     Log error
        /// </summary>
        internal static Exception Error
        {
            private get => new ArgumentNullException(logFile);
            set
            {
                try
                {
                    string formattedValue = value.ToString().Replace('\r', ' ').Replace('\n', ' ').Trim();
                    Trace.TraceError($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff", CultureInfo.InvariantCulture)} {formattedValue}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        ///     Log an error string
        /// </summary>
        internal static string ErrorString
        {
            private get => string.Empty;
            set
            {
                try
                {
                    string formattedValue = value.Replace('\r', ' ').Replace('\n', ' ').Trim();
                    Trace.TraceError($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff", CultureInfo.InvariantCulture)} {formattedValue}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        #endregion
    }
}
