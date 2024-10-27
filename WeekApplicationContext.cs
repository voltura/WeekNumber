#region Using statements

using Microsoft.Win32;
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
        private int _currentWeek;
        private int _lastIconRes;

        #endregion Private variables

        #region Constructor

        internal WeekApplicationContext()
        {
            try
            {
                Log.LogCaller();
                Settings.StartWithWindows = Settings.SettingIsValue(Resources.StartWithWindows, true.ToString());
                Application.ApplicationExit += OnApplicationExit;
                SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
                _currentWeek = Week.Current();
                _lastIconRes = WeekIcon.GetIconResolution();
                Gui = new TaskbarGui(_currentWeek, _lastIconRes);
                Gui.UpdateRequest += GuiUpdateRequestHandler;
                _timer = GetTimer;
            }
            catch (Exception ex)
            {
                _timer?.Stop();
                Log.LogCaller();
                Message.Show(Resources.UnhandledException, ex);
                Application.Exit();
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
                //int calculatedInterval = 86400000 - ((DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000));
                int calculatedInterval = 10000;  // workaround for blurry icon due to Windows icon rendering bug, update icon every 10 seconds instead of when week change occurs only
                Timer timer = new Timer
                {
                    Interval = calculatedInterval,
                    Enabled = true
                };
                Log.Info = $"Timer interval={calculatedInterval / 1000}s";
                timer.Tick += OnTimerTick;
                return timer;
            }
        }

        #endregion Private Timer property

        #region Private event handlers

        private void GuiUpdateRequestHandler(object sender, EventArgs e)
        {
            Log.LogCaller();
            UpdateIcon(true, true);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Log.LogCaller();
            Cleanup(false);
        }

        private void OnUserPreferenceChanged(object sender, EventArgs e)
        {
            Log.LogCaller();
            int iconRes = WeekIcon.GetIconResolution(true);
            if (iconRes != _lastIconRes)
            {
                UpdateIcon(true, true);
                _lastIconRes = iconRes;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            UpdateIcon();
        }

        private void UpdateIcon(bool force = false, bool redrawContextMenu = false)
        {
            if (_currentWeek == Week.Current() && force == false)
            {
                return;
            }
            _timer?.Stop();
            Application.DoEvents();
            try
            {
                Log.LogCaller();
                _currentWeek = Week.Current();
                int iconResolution = Settings.GetIntSetting(Resources.IconResolution, (int)IconSize.Icon256);
                Log.Info = $"Update icon with week number {_currentWeek} using resolution {iconResolution}x{iconResolution}, redraw context menu={redrawContextMenu}, forced update={force}";
                Gui?.UpdateIcon(_currentWeek, iconResolution, redrawContextMenu);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToSetIcon, ex);
                Cleanup();
                throw;
            }
            if (_timer != null)
            {
                int calculatedInterval = 86400000 - ((DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000));
                _timer.Interval = calculatedInterval;
                Log.Info = $"Timer interval={calculatedInterval / 1000}s";
                _timer.Start();
            }
        }

        #endregion Private event handlers

        #region Private methods

        private void Cleanup(bool forceExit = true)
        {
            Log.LogCaller();
            _timer?.Stop();
            _timer?.Dispose();
            Gui?.Dispose();
            Gui = null;
            if (forceExit)
            {
                Application.Exit();
            }
        }

        #endregion Private methods
    }
}