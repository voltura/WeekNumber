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
                //                if (context != null)
                {
                    context?.Dispose();
                }
                Mutex.ReleaseMutex();
            }
        }

        #endregion Application starting point

        internal static void SetGCSettings()
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GCSettings.LatencyMode = GCLatencyMode.Batch;
        }

    }
}