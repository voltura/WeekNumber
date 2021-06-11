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
        #region Private variables

        private Image _img;
        private bool _initComplete = false;

        #endregion Private variables

        #region Protected property

        /// <summary>
        /// Mouse location offset used form form movement
        /// </summary>
        protected Point Offset { get; set; }

        #endregion

        #region Private constructor

        private DateForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            SetControlTexts();
        }

        #endregion

        #region Public static method

        /// <summary>
        /// Displays Date form
        /// </summary>
        public static void Display()
        {
            using (DateForm dateForm = new DateForm())
            {
                dateForm.ShowDialog();
            }
        }

        #endregion

        #region Events handling

        private void Close_Click(object sender, EventArgs e)
        {
            DisposeImage();
            Close();
        }

        private void DisposeImage()
        {
            _img?.Dispose();
            _img = pictureBoxWeek.Image;
            _img?.Dispose();
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
            DisposeImage();
            Close();
        }

        private void MinimizePanelFrame_Click(object sender, EventArgs e)
        {
            DisposeImage();
            Close();
        }

        private void SelectedYearChanged(object sender, EventArgs e)
        {
            UpdateDaysDropdown();
            UpdateWeekInfo();
        }

        private void SelectedMonthChanged(object sender, EventArgs e)
        {
            UpdateDaysDropdown();
            UpdateWeekInfo();
        }

        private void SelectedDayChanged(object sender, EventArgs e)
        {
            UpdateWeekInfo();
        }

        private void DateForm_Shown(object sender, EventArgs e)
        {
            _initComplete = true;
        }

        #endregion

        #region Private methods

        private void UpdateWeekInfo(bool initial = false)
        {
            if (!_initComplete && !initial) return;
            string weekDayPrefix = string.Empty;
            if (int.TryParse(ccbYear.SelectedItem?.ToString(), out int year) == true &&
                int.TryParse(ccbMonth.SelectedItem?.ToString(), out int month) == true &&
                int.TryParse(ccbDay.SelectedItem?.ToString(), out int day) == true)
            {
                DateTime selectedDate = new DateTime(year, month, day);
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == Resources.Swedish)
                {
                    weekDayPrefix = Message.SWEDISH_DAY_OF_WEEK_PREFIX[(int)selectedDate.DayOfWeek];
                }
                int selectedWeek = Week.GetWeekNumber(selectedDate);
                lblInformation.Text = $"{Resources.Week} {selectedWeek}\r\n{weekDayPrefix}{selectedDate.ToLongDateString()}";
                _img = pictureBoxWeek.Image;
                pictureBoxWeek.Image = WeekIcon.GetImage(selectedWeek);
                _img?.Dispose();
            }
            else if (initial)
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == Resources.Swedish)
                {
                    weekDayPrefix = Message.SWEDISH_DAY_OF_WEEK_PREFIX[(int)DateTime.Now.DayOfWeek];
                }
                lblInformation.Text = $"{Resources.Week} {Week.Current()}\r\n{weekDayPrefix}{DateTime.Now.ToLongDateString()}";
                pictureBoxWeek.Image = WeekIcon.GetImage(Week.Current());
            }
            else
            {
                lblInformation.Text = Resources.SelectDate;
                pictureBoxWeek.Image = null;
            }
        }

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
            PopulateYearDropdown();
            PopulateMonthDropdown();
            PopulateDayDropdown();
            UpdateWeekInfo(true);
        }

        private void PopulateDayDropdown()
        {
            ccbDay.Items.Clear();
            for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                ccbDay.Items.Add(i.ToString());
            }
            ccbDay.Text = DateTime.Now.Day.ToString();
        }

        private void PopulateMonthDropdown()
        {
            ccbMonth.Items.Clear();
            for (int i = 1; i < 13; i++)
            {
                ccbMonth.Items.Add(i.ToString());
            }
            ccbMonth.Text = DateTime.Now.Month.ToString();
        }

        private void PopulateYearDropdown()
        {
            ccbYear.Items.Clear();
            for (int i = DateTime.Now.Year - 10; i < DateTime.Now.Year + 10; i++)
            {
                ccbYear.Items.Add(i.ToString());
            }
            ccbYear.Text = DateTime.Now.Year.ToString();
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