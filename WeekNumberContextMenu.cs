#region Using statements

using System;
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
            Log.LogCaller();
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
                Log.LogCaller();
                MenuItem mi = (MenuItem)o;
                TaskbarUtil.ToogleTaskbarIconSize();
                mi.Text = TaskbarUtil.UsingSmallTaskbarButtons() ? Resources.SwitchToLargeTaskbarButtonsMenu : Resources.SwitchToSmallTaskbarButtonsMenu;
                SettingsChangedHandler?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
        }

        private static void ExitMenuClick(object o, EventArgs e)
        {
            Log.LogCaller();
            Application.Exit();
        }

        private void FirstDayOfWeekClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
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
                Log.LogCaller();
                MenuItem mi = (MenuItem)o;
                mi.Enabled = false;
                if (mi.Name == Resources.ResetColors)
                {
                    Settings.UpdateSetting(Resources.IconForegroundRed, 255.ToString());
                    Settings.UpdateSetting(Resources.IconForegroundBlue, 255.ToString());
                    Settings.UpdateSetting(Resources.IconForegroundGreen, 255.ToString());
                    Settings.UpdateSetting(Resources.IconBackgroundRed, 0.ToString());
                    Settings.UpdateSetting(Resources.IconBackgroundGreen, 0.ToString());
                    Settings.UpdateSetting(Resources.IconBackgroundBlue, 0.ToString());
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
                            Settings.GetIntSetting(mi.Name + Resources.Red),
                            Settings.GetIntSetting(mi.Name + Resources.Green),
                            Settings.GetIntSetting(mi.Name + Resources.Blue))
                    })
                    {
                        cd.FullOpen = true;
                        cd.AnyColor = true;
                        cd.AllowFullOpen = true;
                        cd.ShowDialog();
                        Settings.UpdateSetting(mi.Name + Resources.Red, cd.Color.R.ToString());
                        Settings.UpdateSetting(mi.Name + Resources.Green, cd.Color.G.ToString());
                        Settings.UpdateSetting(mi.Name + Resources.Blue, cd.Color.B.ToString());
                    }
                }
                EnableMenuItem(mi);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.FailedToUpdateColor, ex);
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
                Log.LogCaller();
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
            }
        }

        private void CalendarWeekRuleClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
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
            }
        }

        private void UseApplicationLogClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
                MenuItem mi = (MenuItem)o;
                if (mi != null)
                {
                    mi.Enabled = false;
                    mi.Checked = !mi.Checked;
                    Settings.UpdateSetting(Resources.UseApplicationLog, mi.Checked ? "True" : "False");
                    Log.Init();
                    EnableMenuItem(mi);
                    SettingsChangedHandler?.Invoke(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
        }

        private void DisplayNotificationClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
                MenuItem mi = (MenuItem)o;
                if (mi != null)
                {
                    mi.Enabled = false;
                    mi.Checked = !mi.Checked;
                    Settings.UpdateSetting(mi.Name, mi.Checked ? "True" : "False");
                    foreach (MenuItem m in mi.Parent?.MenuItems)
                    {
                        if (m.Name == Resources.UseSilentNotifications)
                        {
                            m.Enabled = mi.Checked;
                        }
                    }
                    EnableMenuItem(mi);
                    SettingsChangedHandler?.Invoke(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
        }

        private void UseSilentNotificationsClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
                MenuItem mi = (MenuItem)o;
                if (mi != null)
                {
                    mi.Enabled = false;
                    mi.Checked = !mi.Checked;
                    Settings.UpdateSetting(Resources.UseSilentNotifications, mi.Checked ? "True" : "False");
                    EnableMenuItem(mi);
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
        }

        private void ShowApplicationLogClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
                MenuItem mi = (MenuItem)o;
                if (mi != null)
                {
                    mi.Enabled = false;
                    if (Settings.SettingIsValue(Resources.UseApplicationLog, "True"))
                    {
                        Log.Show();
                        mi.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
        }

        private void StartWithWindowsClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
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
            }
        }

        private void AutoUpdateClick(object o, EventArgs e)
        {
            try
            {
                Log.LogCaller();
                MenuItem mi = (MenuItem)o;
                if (mi != null)
                {
                    mi.Enabled = false;
                    mi.Checked = !mi.Checked;
                    Settings.UpdateSetting(Resources.AutoUpdate, mi.Checked ? "True" : "False");
                    EnableMenuItem(mi);
                    SettingsChangedHandler?.Invoke(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
        }

        private void AboutClick(object o, EventArgs e)
        {
            Log.LogCaller();
            MenuItem mi = (MenuItem)o;
            try
            {
                mi.Enabled = false;
                Forms.MessageForm.LogAndDisplayLinkMessage(Resources.About, UpdateHandler.APPLICATION_URL);
            }
            finally
            {
                EnableMenuItem(mi);
            }
        }

        private static void SaveIconClick(object o, EventArgs e)
        {
            Log.LogCaller();
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
        private void CheckWeekForDateClick(object o, EventArgs e)
        {
            Log.LogCaller();
            MenuItem mi = (MenuItem)o;
            try
            {
                mi.Enabled = false;
                Forms.DateForm.Display();
            }
            finally
            {
                EnableMenuItem(mi);
            }
        }

        #endregion Private event handling

        #region Private method for context menu creation

        internal void CreateContextMenu()
        {
            Log.LogCaller();
            ContextMenu = new ContextMenu(new MenuItem[7]
            {
                new MenuItem(Resources.AboutMenu, AboutClick)
                {
                    DefaultItem = true
                },
                new MenuItem(Resources.CheckForNewVersionMenu, UpdateHandler.UpdateClick),
                new MenuItem(Resources.OpenApplicationWebPageMenu, UpdateHandler.OpenApplicationWebPageClick),
                SettingsMenu(),
                new MenuItem(Resources.CheckWeekForDateMenu, CheckWeekForDateClick),
                new MenuItem(Resources.SeparatorMenu),
                new MenuItem(Resources.ExitMenu, ExitMenuClick)
            });
        }

        #endregion Private method for context menu creation

        #region Private helper methods for menu items

        private MenuItem SettingsMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.SettingsMenu, new MenuItem[7]
                {
                    new MenuItem(Resources.StartWithWindowsMenu, StartWithWindowsClick)
                    {
                        Checked = Settings.StartWithWindows
                    },
                    new MenuItem(Resources.AutoUpdateMenu, AutoUpdateClick)
                    {
                        Checked = Settings.SettingIsValue(Resources.AutoUpdate, "True")
                    },
                    ApplicationLogMenu(),
                    NotificationsMenu(),
                    new MenuItem(TaskbarUtil.UsingSmallTaskbarButtons() ?
                        Resources.SwitchToLargeTaskbarButtonsMenu : Resources.SwitchToSmallTaskbarButtonsMenu,
                        ToogleTaskbarIconSizeMenuClick),
                    CalendarSettingsMenu(),
                    IconMenu()
                });
        }

        private MenuItem IconMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.IconMenu, new MenuItem[3]
                {
                    ColorsMenu(),
                    IconResolutionMenu(),
                    new MenuItem(Resources.SaveIconMenu, SaveIconClick)
                });
        }

        private MenuItem CalendarSettingsMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.CalendarSettingsMenu, new MenuItem[2]
                {
                    FirstDayOfWeekMenu(),
                    CalendarWeekRuleMenu()
                });
        }

        private MenuItem NotificationsMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.NotificationsMenu, new MenuItem[3]
                    {
                        new MenuItem(Resources.DisplayStartupNotificationMenu, DisplayNotificationClick)
                        {
                            Name = Resources.DisplayStartupNotification,
                            Checked = Settings.SettingIsValue(Resources.DisplayStartupNotification, "True")
                        },
                        new MenuItem(Resources.DisplayWeekChangedNotificationMenu, DisplayNotificationClick)
                        {
                            Name = Resources.DisplayWeekChangedNotification,
                            Checked = Settings.SettingIsValue(Resources.DisplayWeekChangedNotification, "True")
                        },
                        new MenuItem(Resources.UseSilentNotificationsMenu, UseSilentNotificationsClick)
                        {
                            Name = Resources.UseSilentNotifications,
                            Enabled = Settings.SettingIsValue(Resources.DisplayStartupNotification, "True"),
                            Checked = Settings.SettingIsValue(Resources.UseSilentNotifications, "True")
                        }
                    });
        }

        private MenuItem ApplicationLogMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.ApplicationLogMenu, new MenuItem[2]
                    {
                        new MenuItem(Resources.UseApplicationLogMenu, UseApplicationLogClick)
                        {
                            Checked = Settings.SettingIsValue(Resources.UseApplicationLog, "True")
                        },
                        new MenuItem(Resources.ShowLogMenu, ShowApplicationLogClick)
                        {
                            Name = Resources.ShowLog,
                            Enabled = Settings.SettingIsValue(Resources.UseApplicationLog, "True")
                        }
                    })
            {
                Name = Resources.ApplicationLog
            };
        }

        private MenuItem ColorsMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.ColorsMenu, new MenuItem[3]
            {
                new MenuItem(Resources.IconForegroundMenu, ColorMenuClick)
                {
                    Name = Resources.IconForeground
                },
                new MenuItem(Resources.IconBackgroundMenu, ColorMenuClick)
                {
                    Name = Resources.IconBackground
                },
                new MenuItem(Resources.ResetColorsMenu, ColorMenuClick)
                {
                    Name = Resources.ResetColors
                }
            });
        }

        private MenuItem IconResolutionMenu()
        {
            Log.LogCaller();
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

        private MenuItem CalendarWeekRuleMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.CalendarWeekRuleMenu, new MenuItem[3]
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
            Log.LogCaller();
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
            Log.LogCaller();
            foreach (MenuItem m in mi.Parent?.MenuItems)
            {
                m.Checked = false;
            }
            mi.Checked = true;
            mi.Enabled = true;
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
            Log.LogCaller();
            ContextMenu?.Dispose();
            ContextMenu = null;
        }

        #endregion IDisposable methods
    }
}