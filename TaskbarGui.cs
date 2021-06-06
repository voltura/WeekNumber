#region Using statements

using Microsoft.Win32;
using System;
using System.Windows.Forms;

#region Test code
/*
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
*/
#endregion Test code

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
        private int _latestWeek;

        #endregion Private variables

        #region Constructor

        internal TaskbarGui(int week, int iconResolution = (int)IconSize.Icon256)
        {
            Log.LogCaller();
            _latestWeek = week;
            _contextMenu = new WeekNumberContextMenu();
            _notifyIcon = GetNotifyIcon(_contextMenu.ContextMenu);
            UpdateIcon(week, ref _notifyIcon, iconResolution);
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            #region Test code
            /* test code
            Rectangle rect =NotifyIconHelper.GetIconRect(_notifyIcon);*/
            #endregion Test code

            if (Settings.SettingIsValue(Resources.DisplayStartupNotification, "True"))
            {
                DisplayUserInfoBalloonTip($"{_notifyIcon.Text}\r\n{Resources.StartupMessageText}");
            }
            _contextMenu.SettingsChangedHandler += OnSettingsChange;
        }

        #endregion Constructor

        #region Display NotifyIcon BalloonTip

        private void DisplayUserInfoBalloonTip(string message)
        {
            Log.LogCaller();
            bool siletMsg = Settings.SettingIsValue(Resources.UseSilentNotifications, "True");
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
            _notifyIcon.ShowBalloonTip(10000, Message.CAPTION, message, ToolTipIcon.None);
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

        #endregion Display NotifyIcon BalloonTip

        #region Private event handlers

        private void OnSettingsChange(object sender, EventArgs e)
        {
            UpdateRequest?.Invoke(null, null);
        }
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Forms.DateForm.Display();
        }

        #endregion Private event handlers

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

        #region Private UpdateIcon method

        private void UpdateIcon(int weekNumber, ref NotifyIcon notifyIcon, int iconResolution)
        {
            Log.LogCaller();
            try
            {
                notifyIcon.Text = $"{DateTime.Now.ToLongDateString()}\r\n{Resources.Week} {weekNumber}";
                System.Drawing.Icon prevIcon = notifyIcon.Icon;
                notifyIcon.Icon = WeekIcon.GetIcon(weekNumber, iconResolution);
                WeekIcon.CleanupIcon(ref prevIcon);
            }
            finally
            {
                if (_latestWeek != weekNumber)
                {
                    if (Settings.SettingIsValue(Resources.DisplayWeekChangedNotification, "True"))
                    {
                        DisplayUserInfoBalloonTip(notifyIcon.Text);
                    }
                    _latestWeek = weekNumber;
                }
            }
        }

        #endregion Private UpdateIcon method

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

    /* Use this to clear icon area instead, inspiration code only, need modification, but with area of icon then maybe move mouse over it programmatically instead of current more complex solution




    sealed class NotifyIconHelper
    {

        public static Rectangle GetIconRect(NotifyIcon icon)
        {
            RECT rect = new RECT();
            NOTIFYICONIDENTIFIER notifyIcon = new NOTIFYICONIDENTIFIER();

            notifyIcon.cbSize = Marshal.SizeOf(notifyIcon);
            //use hWnd and id of NotifyIcon instead of guid is needed
            notifyIcon.hWnd = GetHandle(icon);
            notifyIcon.uID = GetId(icon);

            int hresult = Shell_NotifyIconGetRect(ref notifyIcon, out rect);
            //rect now has the position and size of icon

            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

        private static int GetIconID(NotifyIcon icon)
        {
            RECT rect = new RECT();
            NOTIFYICONIDENTIFIER notifyIcon = new NOTIFYICONIDENTIFIER();

            notifyIcon.cbSize = Marshal.SizeOf(notifyIcon);
            //use hWnd and id of NotifyIcon instead of guid is needed
            notifyIcon.hWnd = GetHandle(icon);
            notifyIcon.uID = GetId(icon);

            int hresult = Shell_NotifyIconGetRect(ref notifyIcon, out rect);
            //rect now has the position and size of icon

            return notifyIcon.uID;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NOTIFYICONIDENTIFIER
        {
            public Int32 cbSize;
            public IntPtr hWnd;
            public Int32 uID;
            public Guid guidItem;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern int Shell_NotifyIconGetRect([In] ref NOTIFYICONIDENTIFIER identifier, [Out] out RECT iconLocation);

        private static FieldInfo windowField = typeof(NotifyIcon).GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        private static IntPtr GetHandle(NotifyIcon icon)
        {
            if (windowField == null) throw new InvalidOperationException("[Useful error message]");
            NativeWindow window = windowField.GetValue(icon) as NativeWindow;

            if (window == null) throw new InvalidOperationException("[Useful error message]");  // should not happen?
            return window.Handle;
        }

        private static FieldInfo idField = typeof(NotifyIcon).GetField("id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        private static int GetId(NotifyIcon icon)
        {
            if (idField == null) throw new InvalidOperationException("[Useful error message]");
            return (int)idField.GetValue(icon);
        }

    }

*/
}