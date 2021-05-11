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

        private const int _size = 256;

        #endregion Icon Size

        #region Internal static functions

        internal static Icon GetIcon(int weekNumber)
        {
            Icon icon = null;
            using (Bitmap bitmap = new Bitmap(_size, _size))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                DrawBackgroundOnGraphics(graphics);
                DrawWeekNumberOnGraphics(weekNumber, graphics);
                System.IntPtr bHicon = bitmap.GetHicon();
                Icon newIcon = Icon.FromHandle(bHicon);
                icon = new Icon(newIcon, _size, _size);
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
                icon = GetIcon(weekNumber);
                using (FileStream fs = new FileStream(fullPath, FileMode.Create,
                    FileAccess.Write, FileShare.None))
                {
                    icon.Save(fs);
                }
            }
            catch (System.Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
                result = false;
                throw;
            }
            finally
            {
                CleanupIcon(ref icon);
            }
            return result;
        }

        #endregion Internal static functions

        #region Privare static helper methods

        private static void DrawBackgroundOnGraphics(Graphics graphics)
        {
            Color backgroundColor = Color.FromName(Settings.GetSetting(Resources.Background));
            Color foregroundColor = Color.FromName(Settings.GetSetting(Resources.Foreground));
            using (SolidBrush foregroundBrush = new SolidBrush(foregroundColor))
            using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
            {
                float inset = (float)System.Math.Abs(_size * .03125);
                graphics?.FillRectangle(backgroundBrush, inset, inset, _size - inset, _size - inset);
                using (Pen pen = new Pen(foregroundColor, inset * 2))
                {
                    graphics?.DrawRectangle(pen, inset, inset, _size - inset * 2, _size - inset * 2);
                }
                float leftInset = (float)System.Math.Abs(_size * .15625);
                graphics?.FillRectangle(foregroundBrush, leftInset, inset / 2, inset * 3, inset * 5);
                float rightInset = (float)System.Math.Abs(_size * .75);
                graphics?.FillRectangle(foregroundBrush, rightInset, inset / 2, inset * 3, inset * 5);
            }
        }

        private static void DrawWeekNumberOnGraphics(int weekNumber, Graphics graphics)
        {
            float fontSize = (float)System.Math.Abs(_size * .78125);
            float insetX = (float)-System.Math.Abs(fontSize * .14);
            float insetY = (float)System.Math.Abs(fontSize * .2);
            Color foregroundColor = Color.FromName(Settings.GetSetting(Resources.Foreground));

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