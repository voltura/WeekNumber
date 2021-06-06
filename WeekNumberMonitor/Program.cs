#region Using statements

using System;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;

#endregion Using statements

/// <summary>
/// This program is used by WeekNumber to refresh taskbar notification area if WeekNumber.exe is process is terminated 
/// and its notification area icon is still displayed, even though the WeekNumber application is not running.
/// This projects compiled executable file contents has been manually converted into a base64 encoded string which is utilized from 
/// WeekNumber::MonitorProcess class which writes the exe to %LOCALAPPDATA%\temp and starts the program when WeekNumber.exe
/// is started.
/// </summary>
internal class Program
{
    #region Application starting point

    [STAThread]
    private static void Main(string[] args)
    {
        try
        {
            int id = int.Parse(args[0]);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GCSettings.LatencyMode = GCLatencyMode.Batch;
            using (Process p = Process.GetProcessById(id))
            {
                p.WaitForExit();
            }
        }
        catch (Exception)
        {
        }
        RefreshTrayArea();
    }

    #endregion Application starting point

    #region Logic for refreshing taskbar notification area

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
        string lpszWindow);

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

    private static void RefreshTrayArea()
    {
        IntPtr systemTrayContainerHandle = FindWindow("Shell_TrayWnd", null);
        IntPtr systemTrayHandle = FindWindowEx(systemTrayContainerHandle, IntPtr.Zero, "TrayNotifyWnd", null);
        IntPtr sysPagerHandle = FindWindowEx(systemTrayHandle, IntPtr.Zero, "SysPager", null);
        IntPtr notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", "Notification Area");
        if (notificationAreaHandle == IntPtr.Zero)
        {
            notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32",
                "User Promoted Notification Area");
            IntPtr notifyIconOverflowWindowHandle = FindWindow("NotifyIconOverflowWindow", null);
            IntPtr overflowNotificationAreaHandle = FindWindowEx(notifyIconOverflowWindowHandle, IntPtr.Zero,
                "ToolbarWindow32", "Overflow Notification Area");
            RefreshTrayArea(overflowNotificationAreaHandle);
        }
        RefreshTrayArea(notificationAreaHandle);
    }

    private static void RefreshTrayArea(IntPtr windowHandle)
    {
        const uint wmMousemove = 0x0200;
        GetClientRect(windowHandle, out RECT rect);
        for (var x = 0; x < rect.right; x += 5)
            for (var y = 0; y < rect.bottom; y += 5)
                SendMessage(windowHandle, wmMousemove, 0, (y << 16) + x);
    }

    #endregion Logic for refreshing taskbar notification area
}
