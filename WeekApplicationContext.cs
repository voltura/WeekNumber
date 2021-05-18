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
                Application.ApplicationExit += OnApplicationExit;
                SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
                _currentWeek = Week.Current();
                _lastIconRes = GetIconResolution();
                Gui = new TaskbarGui(_currentWeek, _lastIconRes);
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

        private static int GetIconResolution(bool forceUpdate = false)
        {
            int iconResolution = Settings.GetIntSetting(Resources.IconResolution, -1);
            if (forceUpdate || iconResolution == -1)
            {
                // guess what icon resolution to use based on system
                bool usesSmallTaskbarButtons = TaskbarUtil.UsingSmallTaskbarButtons();
                double number = System.Windows.SystemParameters.SmallIconHeight;
                number *= GetWindowsZoom() * 2;
                number *= usesSmallTaskbarButtons ? 1 : 1.5;
                // find closes match to existing configs
                List<int> list = new List<int> { 20, 24, 32, 40, 48, 64, 128, 256, 512 };
                int closest = list.Aggregate((x, y) => Math.Abs(x - number) < Math.Abs(y - number) ? x : y);
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

        private void OnUserPreferenceChanged(object sender, EventArgs e)
        {
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
                _currentWeek = Week.Current();
                int iconResolution = Settings.GetIntSetting(Resources.IconResolution, (int)IconSize.Icon256);
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
                _timer.Interval = (1440 - ((DateTime.Now.Hour * 60) + DateTime.Now.Minute)) * 60000 + (DateTime.Now.Second * 1000);
                _timer.Start();
            }
        }

        #endregion Private event handlers

        #region Private methods

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

        private static double GetWindowsZoom()
        {
            return (double)(Screen.PrimaryScreen.WorkingArea.Width / System.Windows.SystemParameters.PrimaryScreenWidth);
        }

        #endregion Private methods
    }
}