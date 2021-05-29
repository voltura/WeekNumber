#region Using statements

using System.Drawing;
using System.Globalization;
using System.IO;

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
                DrawBackgroundOnGraphics(graphics, size);
                DrawWeekNumberOnGraphics(weekNumber, graphics, size);
                System.IntPtr bHicon = bitmap.GetHicon();
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
            if (size == 0)
            {
                size = _defaultSize;
            }
            float fontSize = (float)System.Math.Abs(size * .78125);
            float insetX = (float)-(size > (int)IconSize.Icon16 ? System.Math.Abs(fontSize * .14) : System.Math.Abs(fontSize * .07));
            float insetY = (float) (size > (int)IconSize.Icon16 ? System.Math.Abs(fontSize * .2) : System.Math.Abs(fontSize * .08));
            Color foregroundColor = Color.FromArgb(
                Settings.GetIntSetting(Resources.ForegroundR),
                Settings.GetIntSetting(Resources.ForegroundG),
                Settings.GetIntSetting(Resources.ForegroundB));

            using (Font font = new Font(FontFamily.GenericMonospace, fontSize, FontStyle.Bold,
                GraphicsUnit.Pixel, 0, false))
            using (Brush brush = new SolidBrush(foregroundColor))
            {
                graphics?.DrawString(weekNumber.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), font, brush, insetX, insetY);
            }
        }

        #endregion Private static helper methods
    }
}