#region Using statements

using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion Using statements

namespace WeekNumber
{
    internal class WeekNumberContextMenu : IDisposable
    {
        #region Internal context menu

        internal ContextMenu ContextMenu { get; private set; }

        #endregion Internal context menu

        #region Private variables

        private readonly UpdateHandler _updateHandler;

        #endregion Private variables

        #region Internal contructor

        internal WeekNumberContextMenu()
        {
            Log.LogCaller();
            _updateHandler = UpdateHandler.Instance;
            CreateContextMenu();
        }

        #endregion Internal constructor

        #region Internal event handling (used for icon update)

        internal event EventHandler SettingsChangedHandler;

        #endregion Internal event handling (used for icon update)

        #region Private event handling

        private void NumbericOptionClick(object o, EventArgs e)
        {
            MenuItem mi = null;
            try
            {
                Log.LogCaller();
                mi = (MenuItem)o;
                mi.Enabled = false;
                Settings.UpdateSetting(mi.Parent.Name, mi.Tag.ToString());
                CheckMenuItemUncheckSiblings(mi);
                SettingsChangedHandler?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
            finally
            {
                EnableMenuItem(mi);
            }
        }

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

        private void LanguageClick(object o, EventArgs e)
        {
            Log.LogCaller();
            MenuItem mi = (MenuItem)o;
            try
            {
                if (mi != null)
                {
                    mi.Enabled = false;
                    Settings.UpdateSetting(Resources.Language, mi.Name);
                    CheckMenuItemUncheckSiblings(mi);
                    Settings.SetCultureInfoFromSystemOrSettings();
                    SettingsChangedHandler?.Invoke(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.Show(Resources.UnhandledException, ex);
            }
            finally
            {
                EnableMenuItem(mi);
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

        private void ImportSettingsClick(object o, EventArgs e)
        {
            Log.LogCaller();
            MenuItem mi = (MenuItem)o;
            try
            {
                mi.Enabled = false;
                using (var ofd = new OpenFileDialog())
                {
                    ofd.InitialDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}";
                    ofd.FileName = $"{System.IO.Path.GetFileName(Application.ExecutablePath)}.Config";
                    ofd.Title = Resources.ImportSettingsMenu;
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Log.Info = $"Settings file '{ofd.FileName}' selected by user to import";
                        bool importOK = Settings.ImportSettings(ofd.FileName);
                        Message.Show("\r\n\r\n" + (importOK ? Resources.SettingsImported : Resources.FailedToImportSettings));
                        if (importOK) SettingsChangedHandler?.Invoke(null, null);
                    }
                }
            }
            finally
            {
                EnableMenuItem(mi);
            }
        }

        private void ExportSettingsClick(object o, EventArgs e)
        {
            Log.LogCaller();
            MenuItem mi = (MenuItem)o;
            try
            {
                mi.Enabled = false;
                using (var ofd = new SaveFileDialog())
                {
                    ofd.InitialDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}";
                    ofd.FileName = $"{System.IO.Path.GetFileName(Application.ExecutablePath)}.Config";
                    ofd.Title = Resources.ExportSettingsMenu;
                    ofd.CheckFileExists = false;
                    ofd.CheckPathExists = true;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Log.Info = $"File '{ofd.FileName}' specified by user to export application settings to.";
                        bool exportOK = Settings.BackupSettings(ofd.FileName);
                        Message.Show("\r\n\r\n" + (exportOK ? Resources.SettingsExported : Resources.FailedToExportSettings));
                    }
                }
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
                new MenuItem(Resources.CheckForNewVersionMenu, _updateHandler.UpdateClick),
                new MenuItem(Resources.OpenApplicationWebPageMenu, _updateHandler.OpenApplicationWebPageClick),
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
            return new MenuItem(Resources.SettingsMenu, new MenuItem[10]
                {
                    new MenuItem(Resources.StartWithWindowsMenu, StartWithWindowsClick)
                    {
                        Checked = Settings.StartWithWindows
                    },
                    new MenuItem(Resources.AutoUpdateMenu, AutoUpdateClick)
                    {
                        Checked = Settings.SettingIsValue(Resources.AutoUpdate, "True")
                    },
                    LanguageMenu(),
                    ApplicationLogMenu(),
                    NotificationsMenu(),
                    new MenuItem(TaskbarUtil.UsingSmallTaskbarButtons() ?
                        Resources.SwitchToLargeTaskbarButtonsMenu : Resources.SwitchToSmallTaskbarButtonsMenu,
                        ToogleTaskbarIconSizeMenuClick),
                    new MenuItem(Resources.ExportSettingsMenu, ExportSettingsClick),
                    new MenuItem(Resources.ImportSettingsMenu, ImportSettingsClick),
                    CalendarSettingsMenu(),
                    IconMenu()
                });
        }

        private MenuItem IconMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.IconMenu, new MenuItem[4]
                {
                    ColorsMenu(),
                    IconResolutionMenu(),
                    GraphicsSettingsMenu(),
                    new MenuItem(Resources.SaveIconMenu, SaveIconClick)
                });
        }

        private MenuItem GraphicsSettingsMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.GraphicsSettingsMenu, new MenuItem[4]
                {
                    SmoothingModeMenu(),
                    CompositingQualityMenu(),
                    InterpolationModeMenu(),
                    TextContrastMenu()
                });
        }

        private MenuItem TextContrastMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.TextContrastMenu, new MenuItem[5]
                    {
                        new MenuItem(Resources.TextContrast1Menu, NumbericOptionClick)
                        {
                            Tag = 1,
                            Checked = Settings.GetIntSetting(Resources.TextContrast, 1) == 1
                        },
                        new MenuItem(Resources.TextContrast2Menu, NumbericOptionClick)
                        {
                            Tag = 2,
                            Checked = Settings.GetIntSetting(Resources.TextContrast, 1) == 2
                        },
                        new MenuItem(Resources.TextContrast3Menu, NumbericOptionClick)
                        {
                            Tag = 3,
                            Checked = Settings.GetIntSetting(Resources.TextContrast, 1) == 3
                        },
                        new MenuItem(Resources.TextContrast4Menu, NumbericOptionClick)
                        {
                            Tag = 4,
                            Checked = Settings.GetIntSetting(Resources.TextContrast, 1) == 4
                        },
                        new MenuItem(Resources.TextContrast5Menu, NumbericOptionClick)
                        {
                            Tag = 5,
                            Checked = Settings.GetIntSetting(Resources.TextContrast, 1) == 5
                        }
                    })
            {
                Name = Resources.TextContrast
            };
        }

        private MenuItem InterpolationModeMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.InterpolationModeMenu, new MenuItem[8]
                    {
                        new MenuItem(Resources.InterpolationModeDefaultMenu, NumbericOptionClick)
                        {
                            Tag = 0,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 0
                        },
                        new MenuItem(Resources.InterpolationModeLowMenu, NumbericOptionClick)
                        {
                            Tag = 1,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 1
                        },
                        new MenuItem(Resources.InterpolationModeHighMenu, NumbericOptionClick)
                        {
                            Tag = 2,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 2
                        },
                        new MenuItem(Resources.InterpolationModeBilinearMenu, NumbericOptionClick)
                        {
                            Tag = 3,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 3
                        },
                        new MenuItem(Resources.InterpolationModeBicubicMenu, NumbericOptionClick)
                        {
                            Tag = 4,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 4
                        },
                        new MenuItem(Resources.InterpolationModeNearestNeighborMenu, NumbericOptionClick)
                        {
                            Tag = 5,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 5
                        },
                        new MenuItem(Resources.InterpolationModeHighQualityBilinearMenu, NumbericOptionClick)
                        {
                            Tag = 6,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 6
                        },
                        new MenuItem(Resources.InterpolationModeHighQualityBicubicMenu, NumbericOptionClick)
                        {
                            Tag = 7,
                            Checked = Settings.GetIntSetting(Resources.InterpolationMode, 0) == 7
                        }
                    })
            {
                Name = Resources.InterpolationMode
            };
        }

        private MenuItem CompositingQualityMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.CompositingQualityMenu, new MenuItem[5]
                    {
                        new MenuItem(Resources.CompositingQualityDefaultMenu, NumbericOptionClick)
                        {
                            Tag = 0,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 0
                        },
                        new MenuItem(Resources.CompositingQualityHighSpeedMenu, NumbericOptionClick)
                        {
                            Tag = 1,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 1
                        },
                        new MenuItem(Resources.CompositingQualityHighQualityMenu, NumbericOptionClick)
                        {
                            Tag = 2,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 2
                        },
                        new MenuItem(Resources.CompositingQualityGammaCorrectedMenu, NumbericOptionClick)
                        {
                            Tag = 3,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 3
                        },
                        new MenuItem(Resources.CompositingQualityAssumeLinearMenu, NumbericOptionClick)
                        {
                            Tag = 4,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 4
                        }
                    })
            {
                Name = Resources.CompositingQuality
            };
        }

        private MenuItem SmoothingModeMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.SmoothingModeMenu, new MenuItem[5]
                    {
                        new MenuItem(Resources.SmoothingModeDefaultMenu, NumbericOptionClick)
                        {
                            Tag = 0,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 0
                        },
                        new MenuItem(Resources.SmoothingModeHighSpeedMenu, NumbericOptionClick)
                        {
                            Tag = 1,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 1
                        },
                        new MenuItem(Resources.SmoothingModeHighQualityMenu, NumbericOptionClick)
                        {
                            Tag = 2,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 2
                        },
                        new MenuItem(Resources.SmoothingModeNoneMenu, NumbericOptionClick)
                        {
                            Tag = 3,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 3
                        },
                        new MenuItem(Resources.SmoothingAntiAliasMenu, NumbericOptionClick)
                        {
                            Tag = 4,
                            Checked = Settings.GetIntSetting(Resources.SmoothingMode, 0) == 4
                        }
                    })
            { 
                Name = Resources.SmoothingMode 
            };
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
        private MenuItem LanguageMenu()
        {
            Log.LogCaller();
            return new MenuItem(Resources.LanguageMenu, new MenuItem[2]
                    {
                        new MenuItem(Resources.EnglishMenu, LanguageClick)
                        {
                            Name = Resources.English,
                            Checked = Settings.SettingIsValue(Resources.Language, Resources.English)
                        },
                        new MenuItem(Resources.SwedishMenu, LanguageClick)
                        {
                            Name = Resources.Swedish,
                            Checked = Settings.SettingIsValue(Resources.Language, Resources.Swedish)
                        }
                    })
            {
                Name = Resources.Language
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