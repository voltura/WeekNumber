#region Using statements

using System;
using System.Runtime;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion Using statements

namespace WeekNumber
{
    internal class Program
    {
        #region Private variable to allow only one instance of application

        private static readonly Mutex Mutex = new Mutex(true, "550adc75-8afb-4813-ac91-8c8c6cb681ae");

        #endregion Private variable to allow only one instance of application

        #region Application starting point

        [STAThread]
        private static void Main()
        {
            if (!Mutex.WaitOne(TimeSpan.Zero, true))
            {
                return;
            }
            WeekApplicationContext context = null;
            try
            {
                Log.Info = "=== Application started ===";
                Log.Info = Application.ProductName + " version " + Application.ProductVersion;
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
                Settings.RestoreBackupSettings();
                NativeMethods.RefreshTrayArea();
                SetGCSettings();
                Application.EnableVisualStyles();
                Application.VisualStyleState = VisualStyleState.ClientAndNonClientAreasEnabled;
                context = new WeekApplicationContext();
                if (context?.Gui != null)
                {
                    Application.Run(context);
                }
            }
            finally
            {
                Log.Close("=== Application ended ===");
                context?.Dispose();
                Mutex.ReleaseMutex();
            }
        }

        #endregion Application starting point

        #region Private method that configures garbarge collection settings

        private static void SetGCSettings()
        {
            Log.LogCaller();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GCSettings.LatencyMode = GCLatencyMode.Batch;
        }

        #endregion Private method that configures garbarge collection settings

        #region Global unhandled Exception trap

        /// <summary>
        /// Catches all unhandled exceptions for the application
        /// Writes the exception to the application log file
        /// Terminates the application with exit code -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Log.Error = ex;
            Message.Show(Resources.UnhandledException, ex);
            Log.Close("=== Application ended ===");
            Environment.Exit(1);
        }

        #endregion Global unhandled Exception trap
    }
}