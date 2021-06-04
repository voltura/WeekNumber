﻿#region Using statements

using Microsoft.Win32;
using System;
using System.Windows.Forms;

#endregion Using statements

namespace WeekNumber
{
    internal class TaskbarGui : IDisposable, IGui
    {
        #region Public event handler (update icon request)

        public event EventHandler UpdateRequest;

        #endregion Public event handler (update icon request)

        #region Private variables

        private NotifyIcon _notifyIcon;
        private readonly WeekNumberContextMenu _contextMenu;

        #endregion Private variables

        #region Constructor

        internal TaskbarGui(int week, int iconResolution = (int)IconSize.Icon256)
        {
            Log.LogCaller();
            _contextMenu = new WeekNumberContextMenu();
            _notifyIcon = GetNotifyIcon(_contextMenu.ContextMenu);
            UpdateIcon(week, ref _notifyIcon, iconResolution);
            DisplayUserInfoBalloonTipSilently();
            _contextMenu.SettingsChangedHandler += OnSettingsChange;
        }

        #endregion Constructor

        #region Display NotifyIcon BalloonTip silently

        private void DisplayUserInfoBalloonTipSilently()
        {
            if (Settings.SettingIsValue(Resources.DisplayStartupMessage, "False")) return;
            Log.LogCaller();
            bool siletMsg = Settings.SettingIsValue(Resources.SilentStartupMessage, "True");
            object currentSound = null;
            if (siletMsg)
            {
                using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings", true))
                {
                    currentSound = regKey.GetValue("NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND");
                    regKey.SetValue("NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", 0);
                    regKey.Flush();
                    regKey.Close();
                }
            }
            _notifyIcon.ShowBalloonTip(10000, Message.CAPTION, $"{_notifyIcon.Text}\r\n{Resources.StartupMessageText}", ToolTipIcon.None);
            if (siletMsg)
            {
                using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings", true))
                {
                    System.Threading.Thread.Sleep(1000);
                    if (currentSound is null)
                    {
                        regKey.DeleteValue("NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND");
                    }
                    else
                    {
                        regKey.SetValue("NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", currentSound);
                    }
                    regKey.Flush();
                    regKey.Close();
                }
            }
        }

        #endregion

        #region Private event handler

        private void OnSettingsChange(object sender, EventArgs e)
        {
            UpdateRequest?.Invoke(null, null);
        }

        #endregion Private event handler

        #region Public UpdateIcon method

        /// <summary>
        /// Updates icon on GUI with given week number
        /// </summary>
        /// <param name="weekNumber">The week number to display on icon</param>
        /// <param name="iconResolution">The width and height of the icon</param>
        /// <param name="redrawContextMenu">Redraw context menu</param>
        public void UpdateIcon(int weekNumber, int iconResolution = (int)IconSize.Icon256, bool redrawContextMenu = false)
        {
            UpdateIcon(weekNumber, ref _notifyIcon, iconResolution);
            if (redrawContextMenu)
            {
                _contextMenu.CreateContextMenu();
                _notifyIcon.ContextMenu = _contextMenu.ContextMenu;
            }
        }

        #endregion Public UpdateIcon method

        #region Private static UpdateIcon method

        private static void UpdateIcon(int weekNumber, ref NotifyIcon notifyIcon, int iconResolution)
        {
            Log.LogCaller();
            notifyIcon.Text = $"{DateTime.Now.ToLongDateString()}\r\n{Resources.Week} {weekNumber}";
            System.Drawing.Icon prevIcon = notifyIcon.Icon;
            notifyIcon.Icon = WeekIcon.GetIcon(weekNumber, iconResolution);
            WeekIcon.CleanupIcon(ref prevIcon);
        }

        #endregion Private static UpdateIcon method

        #region Private helper property to create NotifyIcon

        private static NotifyIcon GetNotifyIcon(ContextMenu contextMenu)
        {
            return new NotifyIcon { Visible = true, ContextMenu = contextMenu };
        }

        #endregion Private helper property to create NotifyIcon

        #region IDisposable methods

        /// <summary>
        /// Disposes the GUI resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            CleanupNotifyIcon();
            _contextMenu.Dispose();
        }

        private void CleanupNotifyIcon()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                if (_notifyIcon.Icon != null)
                {
                    NativeMethods.DestroyIcon(_notifyIcon.Icon.Handle);
                    _notifyIcon.Icon?.Dispose();
                }
                _notifyIcon.ContextMenu?.MenuItems.Clear();
                _notifyIcon.ContextMenu?.Dispose();
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }

        #endregion IDisposable methods
    }
}