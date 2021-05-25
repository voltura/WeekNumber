#region Using statements

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Settings.StartWithWindows = Settings.SettingIsValue(Resources.StartWithWindows, true.ToString());
                Application.ApplicationExit += OnApplicationExit;
                SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
                _currentWeek = Week.Current();
                _lastIconRes = GetIconResolution();
                Gui = new TaskbarGui(_currentWeek, _lastIconRes);
                Gui.UpdateRequest += GuiUpdateRequestHandler;
                _timer = GetTimer;
                _timer.Interval = 10000; // set first interval to 10s to trigger update check
            }
            catch (Exception ex)
            {
                _timer?.Stop();
                Log.LogCaller();
                Message.Show(Resources.UnhandledException, ex);
                Application.Exit();
            }
        }

        private static int GetIconResolution(bool forceUpdate = false)
        {
            int iconResolution = Settings.GetIntSetting(Resources.IconResolution, -1);
            if (forceUpdate || iconResolution == -1)
            {
                // guess what icon resolution to use based on system
                double winZoomLvl = GetWindowsZoom();
                bool usingSmallTaskbarIcons = TaskbarUtil.UsingSmallTaskbarButtons();

                Log.Info = $"SmallIconHeight={System.Windows.SystemParameters.SmallIconHeight}";
                Log.Info = $"WindowsZoomLevel={winZoomLvl * 100}";
                Log.Info = $"UsingSmallTaskbarButtons={usingSmallTaskbarIcons}";

                double number = System.Windows.SystemParameters.SmallIconHeight *
                                winZoomLvl * (usingSmallTaskbarIcons ? 1 : 1.5);

                Log.Info = $"Guessed icon resolution={number}x{number}";

                // find closes match to existing configs (do not allow 16,20,24)
                List<int> list = new List<int> { 32, 40, 48, 64, 128, 256, 512 };
                int closest = list.Aggregate((x, y) => Math.Abs(x - number) < Math.Abs(y - number) ? x : y);

                Log.Info = $"Closest icon resolution={closest}x{closest}";

                Settings.UpdateSetting(Resources.IconResolution, closest.ToString());
                iconResolution = closest;
            }

            return iconResolution;
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
                int calculatedInterval = (1440 - ((DateTime.Now.Hour * 60) + DateTime.Now.Minute)) * 60000 + (DateTime.Now.Second * 1000);
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
            AutoUpdateCheck();
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
            AutoUpdateCheck();
            int iconRes = GetIconResolution(true);
            if (iconRes != _lastIconRes)
            {
                UpdateIcon(true, true);
                _lastIconRes = iconRes;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            UpdateIcon();
            AutoUpdateCheck();
        }

        private void AutoUpdateCheck()
        {
            Log.LogCaller();
            if (!Settings.SettingIsValue(Resources.AutoUpdate, "True"))
            {
                return;
            }
            UpdateHandler.PerformUpdateCheck(silent: true);
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
                int calculatedInterval = (1440 - ((DateTime.Now.Hour * 60) + DateTime.Now.Minute)) * 60000 + (DateTime.Now.Second * 1000);
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

        private static double GetWindowsZoom()
        {
            return (double)(Screen.PrimaryScreen.WorkingArea.Width / System.Windows.SystemParameters.PrimaryScreenWidth);
        }

        #endregion Private methods
    }
}