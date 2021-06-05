#region Using statements

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace WeekNumber.Forms
{
    /// <summary>
    /// Message form
    /// </summary>
    public partial class DateForm : Form
    {
        #region Public methods

        /// <summary>
        ///     Set message to display in form
        /// </summary>
        /// <param name="messageText">Message text</param>
        public void SetMessage(string messageText)
        {
            lblInformation.Text = messageText;
        }

        #endregion

        #region Protected class properties

        /// <summary>
        /// Mouse location offset used form form movement
        /// </summary>
        protected Point Offset { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Settings form constructor
        /// </summary>
        /// <param name="messageText">Message text</param>
        public DateForm(string messageText)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            SetControlTexts();
            SetMessage(messageText);
        }

        /// <summary>
        ///     Settings form constructor
        /// </summary>
        public DateForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            SetControlTexts();
        }

        #endregion

        #region Public static methods

        /// <summary>
        ///     Display a message
        /// </summary>
        /// <param name="messageText"></param>
        public static void DisplayMessage(string messageText)
        {
            using (DateForm message = new DateForm(messageText))
            {
                message.ShowDialog();
            }
        }

        /// <summary>
        ///     Logs and displays message
        /// </summary>
        /// <param name="messageText"></param>
        public static void LogAndDisplayMessage(string messageText)
        {
            Log.Info = messageText;
            using (DateForm message = new DateForm(messageText))
            {
                message.ShowDialog();
            }
        }

        #endregion

        #region Events handling

        private void OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsTitle_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateOffset(e);
        }

        private void SettingsTitle_MouseMove(object sender, MouseEventArgs e)
        {
            MoveForm(e);
        }

        private void MinimizePanel_MouseEnter(object sender, EventArgs e)
        {
            FocusMinimizeIcon();
        }

        private void MinimizePanel_MouseLeave(object sender, EventArgs e)
        {
            UnfocusMinimizeIcon();
        }

        private void MinimizePanel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MinimizePanelFrame_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SelectedYearChanged(object sender, EventArgs e)
        {
            UpdateDaysDropdown();
        }

        private void SelectedMonthChanged(object sender, EventArgs e)
        {
            UpdateDaysDropdown();
        }

        #endregion

        #region Private methods

        private void UpdateDaysDropdown()
        {

            if (int.TryParse(ccbYear.SelectedItem?.ToString(), out int year) == true &&
                int.TryParse(ccbMonth.SelectedItem?.ToString(), out int month) == true)
            {
                bool gotPreviousSelectedDay = int.TryParse(ccbDay.SelectedItem?.ToString(), out int previousSelectedDay);
                if (ccbDay.Items.Count != DateTime.DaysInMonth(year, month))
                {
                    ccbDay.Items.Clear();
                    for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
                    {
                        ccbDay.Items.Add(i.ToString());
                    }
                    if (gotPreviousSelectedDay == true && previousSelectedDay <= DateTime.DaysInMonth(year, month))
                    {
                        ccbDay.Text = previousSelectedDay.ToString();
                    }
                    else
                    {
                        ccbDay.Text = DateTime.Now.Day.ToString();
                    }
                }
            }
        }

        private void SetControlTexts()
        {
            btnClose.Text = Resources.Close;
            lblDateFormTitle.Text = Message.CAPTION;
            Text = Message.CAPTION;
            lblYear.Text = Resources.Year;
            lblMonth.Text = Resources.Month;
            lblDay.Text = Resources.Day;
            ccbYear.Items.Clear();
            for (int i = DateTime.Now.Year - 10; i < DateTime.Now.Year + 10; i++)
            {
                ccbYear.Items.Add(i.ToString());
            }
            ccbYear.Text = DateTime.Now.Year.ToString();
            ccbMonth.Items.Clear();
            for (int i = 1; i < 13; i++)
            {
                ccbMonth.Items.Add(i.ToString());
            }
            ccbMonth.Text = DateTime.Now.Month.ToString();
            ccbDay.Items.Clear();
            for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                ccbDay.Items.Add(i.ToString());
            }
            ccbDay.Text = DateTime.Now.Day.ToString();
        }

        private void FocusMinimizeIcon()
        {
            minimizePanel.BackColor = Color.LightGray;
        }

        private void MoveForm(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Top = Cursor.Position.Y - Offset.Y;
            Left = Cursor.Position.X - Offset.X;
        }

        private void UpdateOffset(MouseEventArgs e)
        {
            Offset = new Point(e.X, e.Y);
        }

        private void UnfocusMinimizeIcon()
        {
            minimizePanel.BackColor = Color.White;
        }

        #endregion
    }
}