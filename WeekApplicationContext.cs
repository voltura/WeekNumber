#region Using statements

using System;
using System.Windows.Forms;

#endregion Using statements

namespace WeekNumber
{
    internal class WeekApplicationContext : ApplicationContext
    {
        #region Internal Taskbar GUI

        internal IGui Gui;

        #endregion Internal Taskbar GUI

        #region Private variables

        private readonly Timer _timer;
        private int currentWeek;

        #endregion Private variables

        #region Constructor

        internal WeekApplicationContext()
        {
            try
            {
                Application.ApplicationExit += OnApplicationExit;
                currentWeek = Week.Current();
                int iconResolution = Settings.GetIntSetting(Resources.IconResolution, (int)IconSize.Icon256);
                Gui = new TaskbarGui(currentWeek, iconResolution);
                Gui.UpdateRequest += GuiUpdateRequestHandler;
                _timer = GetTimer;
            }
            catch (Exception ex)
            {
                _timer?.Stop();
                Message.Show(Resources.UnhandledException, ex);
                Application.Exit();
                throw;
            }
        }

        #endregion Constructor

        #region Private Timer property

        private Timer GetTimer
        {
            get
            {
                if (_timer != null)
                {
                    return _timer;
                }
                Timer timer = new Timer
                {
                    Interval = (1440 - ((DateTime.Now.Hour * 60) + DateTime.Now.Minute)) * 60000 + (DateTime.Now.Second * 1000),
                    Enabled = true
                };
                timer.Tick += OnTimerTick;
                return timer;
            }
        }

        #endregion Private Timer property

        #region Private event handlers

        private void GuiUpdateRequestHandler(object sender, EventArgs e)
        {
            UpdateIcon(true);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Cleanup(false);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            UpdateIcon();
        }

        private void UpdateIcon(bool force = false)
        {
            if (currentWeek == Week.Current() && force == false)
            {
                return;
            }
            _timer?.Stop();
            Application.DoEvents();
            try
            {
                currentWeek = Week.Current();
                int iconResolution = Settings.GetIntSetting(Resources.IconResolution, (int)IconSize.Icon256);
                Gui?.UpdateIcon(currentWeek, iconResolution);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToSetIcon, ex);
                Cleanup();
                throw;
            }
            if (_timer != null)
            {
                _timer.Interval = (1440 - ((DateTime.Now.Hour * 60) + DateTime.Now.Minute)) * 60000 + (DateTime.Now.Second * 1000);
                _timer.Start();
            }
        }

        #endregion Private event handlers

        #region Private Cleanup method

        private void Cleanup(bool forceExit = true)
        {
            _timer?.Stop();
            _timer?.Dispose();
            Gui?.Dispose();
            Gui = null;
            if (forceExit)
            {
                Application.Exit();
            }
        }

        #endregion Private Cleanup method
    }
}