#region Using statements

using System.Drawing;
using System.Windows.Forms;

#endregion

namespace WeekNumber.Controls
{
    /// <summary>
    ///     Customize menu item selected colors
    /// </summary>
    public class CustomColorsRenderer : ToolStripProfessionalRenderer
    {
        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        public CustomColorsRenderer() : base(new CustomColors())
        {
        }

        #endregion

        #region Private class

        private class CustomColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected => Color.DeepSkyBlue;

            public override Color MenuItemSelectedGradientBegin => Color.DeepSkyBlue;

            public override Color MenuItemSelectedGradientEnd => Color.DeepSkyBlue;
        }

        #endregion
    }
}