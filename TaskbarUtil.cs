#region Using statements

using Microsoft.Win32;
using System;

#endregion Using statements

namespace WeekNumber
{
    internal static class TaskbarUtil
    {
        #region Private registry constants

        private const string EXPLORER_ADVANCED_REG_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string TASKBAR_SMALL_ICONS_REG_NAME = "TaskbarSmallIcons";

        #endregion Private registry constants

        #region Internal taskbar method

        internal static void ToogleTaskbarIconSize()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(EXPLORER_ADVANCED_REG_KEY, true))
                {
                    if (key is null)
                    {
                        Message.Show(Resources.FailedToUpdateRegistry);
                        return;
                    }
                    int newValue = Math.Abs(((int)key?.GetValue(TASKBAR_SMALL_ICONS_REG_NAME, 0, RegistryValueOptions.None)) - 1);
                    key?.SetValue(TASKBAR_SMALL_ICONS_REG_NAME, newValue, RegistryValueKind.DWord);
                    Log.Info = $"{TASKBAR_SMALL_ICONS_REG_NAME}={(newValue == 0 ? "false" : "true")}";
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToUpdateRegistry, ex);
                return;
            }
            try
            {
                if (!NativeMethods.SendNotifyMessage((IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SETTINGCHANGE, UIntPtr.Zero, "TraySettings")) //NOTE: Will only work for English Windows UI locale
                {
                    Message.Show(Resources.FailedToNotifyWindowsOfTaskbarIconSizeChange);
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToNotifyWindowsOfTaskbarIconSizeChange, ex);
            }
        }

        #endregion Internal taskbar method

        #region Internal taskbar function

        internal static bool UsingSmallTaskbarButtons()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(EXPLORER_ADVANCED_REG_KEY, false))
                {
                    return (int)key?.GetValue(TASKBAR_SMALL_ICONS_REG_NAME, 0, RegistryValueOptions.None) != 0;
                }
            }
            catch (Exception ex)
            {
                Log.LogCaller();
                Log.Error = ex;
            }
            return true;
        }

        #endregion Internal taskbar function
    }
}
