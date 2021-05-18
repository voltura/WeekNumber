#region Using statements

using System;
using System.Runtime.InteropServices;

#endregion Using statements

namespace WeekNumber
{
    internal static class NativeMethods
    {
        #region Internal constants

        internal const int NULL = 0;
        internal const int HWND_BROADCAST = 0xffff;
        internal const int WM_SETTINGCHANGE = 0x001a;

        #endregion Internal constants

        #region External user32.dll function to free GDI+ icon from memory

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyIcon(IntPtr handle);

        #endregion External user32.dll function to free GDI+ icon from memory

        #region External user32.dll function to inform Windows about changed taskbar setting

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam);

        #endregion External user32.dll function to inform Windows about changed taskbar setting
    }
}