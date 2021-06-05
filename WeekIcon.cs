#region Using statements

using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System;

#endregion Using statements

namespace WeekNumber
{
    internal static class WeekIcon
    {
        #region Icon Size

        private const int _defaultSize = (int)IconSize.Icon256;
        private const int _saveSize = (int)IconSize.Icon256;

        #endregion Icon Size

        #region Internal static functions

        internal static int GetIconResolution(bool forceUpdate = false)
        {
            int iconResolution = Settings.GetIntSetting(Resources.IconResolution, -1);
            double myDbl = 1.0d;
            if (forceUpdate || iconResolution == -1)
            {
                // guess what icon resolution to use based on system
                double winZoomLvl = GetWindowsZoom();
                bool usingSmallTaskbarIcons = TaskbarUtil.UsingSmallTaskbarButtons();
                myDbl = (double)System.Windows.SystemParameters.SmallIconHeight * winZoomLvl;
                if (!usingSmallTaskbarIcons) myDbl *= 1.5d;
                if (System.Windows.SystemParameters.PrimaryScreenWidth > 1600) myDbl *= 2.0d;
                Log.Info = $"SmallIconHeight={System.Windows.SystemParameters.SmallIconHeight}";
                Log.Info = $"WindowsZoomLevel={winZoomLvl * 100}";
                Log.Info = $"UsingSmallTaskbarButtons={usingSmallTaskbarIcons}";
                Log.Info = $"Guessed icon resolution={myDbl}x{myDbl}";

                // find closes match to existing configs (do not allow 16,20,24)
                List<int> list = new List<int> { 32, 40, 48, 64, 128, 256, 512 };
                int closest = list.Aggregate((x, y) => Math.Abs(x - myDbl) < Math.Abs(y - myDbl) ? x : y);

                Log.Info = $"Closest icon resolution={closest}x{closest}";

                Settings.UpdateSetting(Resources.IconResolution, closest.ToString());
                iconResolution = closest;
            }

            return iconResolution;
        }

        internal static Bitmap GetImage(int weekNumber)
        {
            Log.LogCaller();
            int size = 256;
            using (Bitmap bitmap = new Bitmap(size, size))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                try
                {
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.TextContrast = 1;
                    DrawBackgroundOnGraphics(graphics, size);
                    DrawWeekNumberOnGraphics(weekNumber, graphics, size);
                    return bitmap.Clone(new RectangleF(0, 0, size, size), System.Drawing.Imaging.PixelFormat.DontCare);
                }
                finally
                {
                    graphics.Dispose();
                    bitmap?.Dispose();
                }
            }
        }

        internal static Icon GetIcon(int weekNumber, int size = 0)
        {
            Log.LogCaller();
            if (size == 0)
            {
                Log.Info = $"Using default icon size {_defaultSize}x{_defaultSize}";
                size = _defaultSize;
            }
            Icon icon = null;
            using (Bitmap bitmap = new Bitmap(size, size))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.TextContrast = 1;
                DrawBackgroundOnGraphics(graphics, size);
                DrawWeekNumberOnGraphics(weekNumber, graphics, size);
                IntPtr bHicon = bitmap.GetHicon();
                Icon newIcon = Icon.FromHandle(bHicon);
                icon = new Icon(newIcon, size, size);
                CleanupIcon(ref newIcon);
            }
            return icon;
        }

        internal static void CleanupIcon(ref Icon icon)
        {
            if (icon is null)
            {
                return;
            }
            NativeMethods.DestroyIcon(icon.Handle);
            icon.Dispose();
        }

        internal static bool SaveIcon(int weekNumber, string fullPath)
        {
            bool result = true;
            Icon icon = null;

            try
            {
                Log.LogCaller();
                icon = GetIcon(weekNumber, _saveSize);
                using (FileStream fs = new FileStream(fullPath, FileMode.Create,
                    FileAccess.Write, FileShare.None))
                {
                    icon.Save(fs);
                    Log.Info = $"Save app icon as '{fullPath}'";
                }
            }
            catch (System.Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
                result = false;
            }
            finally
            {
                CleanupIcon(ref icon);
            }
            return result;
        }

        #endregion Internal static functions

        #region Privare static helper methods

        private static void DrawBackgroundOnGraphics(Graphics graphics, int size = 0)
        {
            if (size == 0)
            {
                size = _defaultSize;
            }
            Color backgroundColor = Color.FromArgb(
                Settings.GetIntSetting(Resources.BackgroundR),
                Settings.GetIntSetting(Resources.BackgroundG),
                Settings.GetIntSetting(Resources.BackgroundB));
            Color foregroundColor = Color.FromArgb(
                Settings.GetIntSetting(Resources.ForegroundR, 255),
                Settings.GetIntSetting(Resources.ForegroundG, 255),
                Settings.GetIntSetting(Resources.ForegroundB, 255));
            using (SolidBrush foregroundBrush = new SolidBrush(foregroundColor))
            using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
            {
                float inset = (float)System.Math.Abs(size * .03125);
                graphics?.FillRectangle(backgroundBrush, inset, inset, size - inset, size - inset);
                using (Pen pen = new Pen(foregroundColor, inset * 2))
                {
                    graphics?.DrawRectangle(pen, inset, inset, size - inset * 2, size - inset * 2);
                }
                float leftInset = (float)System.Math.Abs(size * .15625);
                graphics?.FillRectangle(foregroundBrush, leftInset, inset / 2, inset * 3, inset * 5);
                float rightInset = (float)System.Math.Abs(size * .75);
                graphics?.FillRectangle(foregroundBrush, rightInset, inset / 2, inset * 3, inset * 5);
            }
        }

        private static void DrawWeekNumberOnGraphics(int weekNumber, Graphics graphics, int size = 0)
        {
            if (size == 0) size = _defaultSize;
            float fontSize = (float)System.Math.Abs(size * .78125);
            float insetX = (float)-(size > (int)IconSize.Icon16 ? System.Math.Abs(fontSize * .12) : System.Math.Abs(fontSize * .07));
            float insetY = (float)(size > (int)IconSize.Icon16 ? System.Math.Abs(fontSize * .2) : System.Math.Abs(fontSize * .08));
            Color foregroundColor = Color.FromArgb(
                Settings.GetIntSetting(Resources.ForegroundR),
                Settings.GetIntSetting(Resources.ForegroundG),
                Settings.GetIntSetting(Resources.ForegroundB));

            using (Font font = new Font(FontFamily.GenericMonospace, fontSize, FontStyle.Bold,
                GraphicsUnit.Pixel, 0, false))
            using (Brush brush = new SolidBrush(foregroundColor))
                graphics?.DrawString(weekNumber.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), font, brush, insetX, insetY);
        }

        private static double GetWindowsZoom()
        {
            return (double)(Screen.PrimaryScreen.WorkingArea.Width / System.Windows.SystemParameters.PrimaryScreenWidth);
        }

        #endregion Private static helper methods
    }
}