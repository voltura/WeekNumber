#region Using statements

using System;
using System.Globalization;
using System.Windows.Forms;

#endregion Using statements

namespace WeekNumber
{
    internal class WeekNumberContextMenu : IDisposable
    {
        #region Internal context menu

        internal ContextMenu ContextMenu { get; private set; }

        #endregion Internal context menu

        #region Internal contructor

        internal WeekNumberContextMenu()
        {
            CreateContextMenu();
        }

        #endregion Internal constructor

        #region Internal event handling (used for icon update)

        internal event EventHandler SettingsChangedHandler;

        #endregion Internal event handling (used for icon update)

        #region Private event handling

        private void ToogleTaskbarIconSizeMenuClick(object o, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)o;
                TaskbarUtil.ToogleTaskbarIconSize();
                mi.Text = TaskbarUtil.UsingSmallTaskbarButtons() ? Resources.SwitchToLargeTaskbarButtonsMenu : Resources.SwitchToSmallTaskbarButtonsMenu;
                SettingsChangedHandler?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.SomethingWentWrong, ex);
            }
        }

        private static void ExitMenuClick(object o, EventArgs e)
        {
            Application.Exit();
        }

        private void FirstDayOfWeekClick(object o, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)o;
                mi.Enabled = false;
                CheckMenuItemUncheckSiblings(mi);
                Settings.UpdateSetting(Week.DayOfWeekString, mi.Name);
                EnableMenuItem(mi);
                SettingsChangedHandler?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToUpdateDayOfWeek, ex);
                throw;
            }
        }

        private void ColorMenuClick(object o, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)o;
                mi.Enabled = false;
                if (mi.Name == Resources.ResetColors)
                {
                    Settings.UpdateSetting(Resources.ForegroundR, 255.ToString());
                    Settings.UpdateSetting(Resources.ForegroundB, 255.ToString());
                    Settings.UpdateSetting(Resources.ForegroundG, 255.ToString());
                    Settings.UpdateSetting(Resources.BackgroundR, 0.ToString());
                    Settings.UpdateSetting(Resources.BackgroundG, 0.ToString());
                    Settings.UpdateSetting(Resources.BackgroundB, 0.ToString());
                }
                else
                {
                    using (ColorDialog cd = new ColorDialog
                    {
                        AllowFullOpen = false,
                        FullOpen = false,
                        SolidColorOnly = false,
                        ShowHelp = false,
                        Color = System.Drawing.Color.FromArgb(
                            Settings.GetIntSetting(mi.Name + Resources.R),
                            Settings.GetIntSetting(mi.Name + Resources.G),
                            Settings.GetIntSetting(mi.Name + Resources.B))
                    })
                    {
                        cd.FullOpen = true;
                        cd.AnyColor = true;
                        cd.AllowFullOpen = true;
                        cd.ShowDialog();
                        Settings.UpdateSetting(mi.Name + Resources.R, cd.Color.R.ToString());
                        Settings.UpdateSetting(mi.Name + Resources.G, cd.Color.G.ToString());
                        Settings.UpdateSetting(mi.Name + Resources.B, cd.Color.B.ToString());
                    }
                }

                Settings.UpdateSetting(Resources.ForceRedraw, true.ToString(CultureInfo.InvariantCulture));
                EnableMenuItem(mi);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToUpdateColor, ex);
                throw;
            }
            finally
            {
                SettingsChangedHandler?.Invoke(null, null);
            }
        }

        private void IconResolutionMenuClick(object o, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)o;
                mi.Enabled = false;
                int iconSizeSelected = (int)IconSize.Icon256;
                int.TryParse(mi.Name.Replace(Resources.Icon, string.Empty), out iconSizeSelected);
                Settings.UpdateSetting(Resources.IconResolution, iconSizeSelected.ToString());
                CheckMenuItemUncheckSiblings(mi);
                SettingsChangedHandler?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToChangeIconResolution, ex);
                throw;
            }
        }

        private void CalendarWeekRuleClick(object o, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)o;
                mi.Enabled = false;
                CheckMenuItemUncheckSiblings(mi);
                string calendarWeekRuleSetting = mi.Name;
                Settings.UpdateSetting(Week.CalendarWeekRuleString, calendarWeekRuleSetting);
                EnableMenuItem(mi);
                SettingsChangedHandler?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToUpdateCalendarWeekRule, ex);
                throw;
            }
        }

        private void StartWithWindowsClick(object o, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)o;
                if (mi != null)
                {
                    mi.Enabled = false;
                    mi.Checked = !mi.Checked;
                    Settings.StartWithWindows = mi.Checked;
                    EnableMenuItem(mi);
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToUpdateRegistry, ex);
                throw;
            }
        }

        private void AboutClick(object o, EventArgs e)
        {
            MenuItem mi = (MenuItem)o;
            mi.Enabled = false;
            Message.Show(Resources.About);
            EnableMenuItem(mi);
        }

        private static void SaveIconClick(object o, EventArgs e)
        {
            MenuItem mi = (MenuItem)o;
            mi.Enabled = false;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = Resources.SaveIconMenu,
                AddExtension = true,
                DefaultExt = ".ico",
                FileName = Application.ProductName + ".ico",
                SupportMultiDottedExtensions = false,
                OverwritePrompt = true,
                CheckPathExists = true
            })
            {
                if (DialogResult.OK == saveFileDialog.ShowDialog())
                {
                    WeekIcon.SaveIcon(Week.Current(), saveFileDialog.FileName);
                }
            }
            EnableMenuItem(mi);
        }

        #endregion Private event handling

        #region Private method for context menu creation

        internal void CreateContextMenu()
        {
            ContextMenu = new ContextMenu(new MenuItem[4]
            {
                new MenuItem(Resources.AboutMenu, AboutClick)
                {
                    DefaultItem = true
                },
                new MenuItem(Resources.SettingsMenu, new MenuItem[7]
                {
                    new MenuItem(Resources.StartWithWindowsMenu, StartWithWindowsClick)
                    {
                        Checked = Settings.StartWithWindows
                    },
                    new MenuItem(TaskbarUtil.UsingSmallTaskbarButtons() ? 
                        Resources.SwitchToLargeTaskbarButtonsMenu : Resources.SwitchToSmallTaskbarButtonsMenu, 
                        ToogleTaskbarIconSizeMenuClick),
                    FirstDayOfWeekMenu(),
                    CalendarRuleMenu(),
                    ColorsMenu(),
                    new MenuItem(Resources.SaveIconMenu, SaveIconClick),
                    IconResolutionMenu()
                }),
                new MenuItem(Resources.SeparatorMenu),
                new MenuItem(Resources.ExitMenu, ExitMenuClick)
            });
        }

        #endregion Private method for context menu creation

        #region Private helper methods for menu items

        private MenuItem ColorsMenu()
        {
            return new MenuItem(Resources.ColorsMenu, new MenuItem[3]
            {
                new MenuItem(Resources.ForegroundMenu, ColorMenuClick)
                {
                    Name = Resources.Foreground
                },
                new MenuItem(Resources.BackgroundMenu, ColorMenuClick)
                {
                    Name = Resources.Background
                },
                new MenuItem(Resources.ResetColorsMenu, ColorMenuClick)
                {
                    Name = Resources.ResetColors
                }
            });
        }

        private MenuItem IconResolutionMenu()
        {
            int iconResolutionSetting = Settings.GetIntSetting(Resources.IconResolution, (int)IconSize.Icon256);
            string selectedIconResolutionName = $"Icon{iconResolutionSetting}";
            return new MenuItem(Resources.IconResolutionMenu, new MenuItem[10]
            {
                new MenuItem(Resources.Icon16, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon16),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon16),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon16)
                },
                new MenuItem(Resources.Icon20, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon20),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon20),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon20)
                },
                new MenuItem(Resources.Icon24, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon24),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon24),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon24)
                },
                new MenuItem(Resources.Icon32, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon32),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon32),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon32)
                },
                new MenuItem(Resources.Icon40, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon40),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon40),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon40)
                },
                new MenuItem(Resources.Icon48, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon48),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon48),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon48)
                },
                new MenuItem(Resources.Icon64, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon64),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon64),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon64)
                },
                new MenuItem(Resources.Icon128, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon128),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon128),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon128)
                },
                new MenuItem(Resources.Icon256, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon256),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon256),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon256)
                },
                new MenuItem(Resources.Icon512, IconResolutionMenuClick)
                {
                    Name = nameof(Resources.Icon512),
                    Checked = selectedIconResolutionName == nameof(Resources.Icon512),
                    Enabled = selectedIconResolutionName != nameof(Resources.Icon512)
                }
            });
        }

        private MenuItem CalendarRuleMenu()
        {
            return new MenuItem(Resources.CalendarRuleMenu, new MenuItem[3]
            {
                new MenuItem(Resources.FirstDayMenu, CalendarWeekRuleClick)
                {
                    Name = Week.FirstDay,
                    Checked = Settings.SettingIsValue(Week.CalendarWeekRuleString, Week.FirstDay)
                },
                new MenuItem(Resources.FirstFourDayWeekMenu, CalendarWeekRuleClick)
                {
                    Name = Week.FirstFourDayWeek,
                    Checked = Settings.SettingIsValue(Week.CalendarWeekRuleString, Week.FirstFourDayWeek)
                },
                new MenuItem(Resources.FirstFullWeekMenu, CalendarWeekRuleClick)
                {
                    Name = Week.FirstFullWeek,
                    Checked = Settings.SettingIsValue(Week.CalendarWeekRuleString, Week.FirstFullWeek)
                }
            });
        }

        private MenuItem FirstDayOfWeekMenu()
        {
            return new MenuItem(Resources.FirstDayOfWeekMenu, new MenuItem[7]
            {
                new MenuItem(Resources.MondayMenu, FirstDayOfWeekClick)
                {
                    Name = Week.Monday,
                    Checked = Settings.SettingIsValue(Week.DayOfWeekString, Week.Monday)
                },
                new MenuItem(Resources.TuesdayMenu, FirstDayOfWeekClick)
                {
                    Name = Week.Tuesday,
                    Checked = Settings.SettingIsValue(Week.DayOfWeekString, Week.Tuesday)
                },
                new MenuItem(Resources.WednesdayMenu, FirstDayOfWeekClick)
                {
                    Name = Week.Wednesday,
                    Checked = Settings.SettingIsValue(Week.DayOfWeekString, Week.Wednesday)
                },
                new MenuItem(Resources.ThursdayMenu, FirstDayOfWeekClick)
                {
                    Name = Week.Thursday,
                    Checked = Settings.SettingIsValue(Week.DayOfWeekString, Week.Thursday)
                },
                new MenuItem(Resources.FridayMenu, FirstDayOfWeekClick)
                {
                    Name = Week.Friday,
                    Checked = Settings.SettingIsValue(Week.DayOfWeekString, Week.Friday)
                },
                new MenuItem(Resources.SaturdayMenu, FirstDayOfWeekClick)
                {
                    Name = Week.Saturday,
                    Checked = Settings.SettingIsValue(Week.DayOfWeekString, Week.Saturday)
                },
                new MenuItem(Resources.SundayMenu, FirstDayOfWeekClick)
                {
                    Name = Week.Sunday,
                    Checked = Settings.SettingIsValue(Week.DayOfWeekString, Week.Sunday)
                }
            });
        }

        private static void EnableMenuItem(MenuItem mi)
        {
            if (mi != null)
            {
                mi.Enabled = true;
            }
        }

        private static void CheckMenuItemUncheckSiblings(MenuItem mi)
        {
            if (mi is null)
            {
                return;
            }
            foreach (MenuItem m in mi.Parent?.MenuItems)
            {
                m.Checked = false;
                m.Enabled = true;
            }
            mi.Checked = true;
            mi.Enabled = false;
        }

        #endregion Private helper methods for menu items

        #region IDisposable methods

        /// <summary>
        /// Disposes the context menu
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
            CleanupContextMenu();
        }

        private void CleanupContextMenu()
        {
            ContextMenu?.Dispose();
            ContextMenu = null;
        }

        #endregion IDisposable methods
    }
}