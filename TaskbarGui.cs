#region Using statements

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
            _contextMenu = new WeekNumberContextMenu();
            _notifyIcon = GetNotifyIcon(_contextMenu.ContextMenu);
            UpdateIcon(week, ref _notifyIcon, iconResolution);
            _contextMenu.SettingsChangedHandler += OnSettingsChange;
        }

        #endregion Constructor

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
        public void UpdateIcon(int weekNumber, int iconResolution = (int)IconSize.Icon256)
        {
            UpdateIcon(weekNumber, ref _notifyIcon, iconResolution);
        }

        #endregion Public UpdateIcon method

        #region Private static UpdateIcon method

        private static void UpdateIcon(int weekNumber, ref NotifyIcon notifyIcon, int iconResolution)
        {
            notifyIcon.Text = Resources.Week + weekNumber;
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