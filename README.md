# WeekNumber
Windows 10 taskbar application that displays the current week number

<img src="https://user-images.githubusercontent.com/2292809/120221521-d6a79580-c23e-11eb-99d1-be6210b43fcf.png" data-canonical-src="https://user-images.githubusercontent.com/2292809/120221521-d6a79580-c23e-11eb-99d1-be6210b43fcf.png" alt="WeekNumber" width="150" height="150" />

[![Latest release ZIP](https://img.shields.io/github/v/release/voltura/WeekNumber?label=download%20latest%20release&style=for-the-badge)](https://github.com/voltura/weeknumber/releases/latest/download/WeekNumber.zip)

*- Completely Free and Open Source! Donations are highly appriciated!*

[![Donate](https://img.shields.io/badge/donate_via-paypal_or_card-blue)](https://www.paypal.com/donate?hosted_button_id=7PN65YXN64DBG) _**<-- Press here to donate!**_

[![Github All Releases](https://img.shields.io/github/downloads/voltura/WeekNumber/total.svg)]()
[![License](https://img.shields.io/badge/licence-MIT-green)]()

## Features

Options to start with Windows, auto update, customize colors, set icon resolution and calendar rules via right-click on the application icon.
The application is tiny and its memory consumption low. For details see _Help section_ below.

## Screenshots
#### Taskbar icon with default colors
![Screenshot #1](https://user-images.githubusercontent.com/2292809/120904782-473f1f80-c64e-11eb-9256-c3d0ddab2124.png)
#### Context menu - accessable via right-click on week number icon
![Screenshot #2](https://user-images.githubusercontent.com/2292809/120904846-af8e0100-c64e-11eb-9fff-6725bbd4fce2.png)
#### Get week number for date form
![Screenshot #3](https://user-images.githubusercontent.com/2292809/120904877-e49a5380-c64e-11eb-80b5-c44d6f71fab6.png)
#### Settings menu
![Screenshot #4](https://user-images.githubusercontent.com/2292809/120904919-101d3e00-c64f-11eb-8c9d-323a2cd33807.png)
#### Taskbar icon with customized colors
![Screenshot #5](https://user-images.githubusercontent.com/2292809/118048718-f4d74f80-b37c-11eb-8b36-211250ff25c5.png)
#### Icon color picker, possible to define and use custom colors (both icon text and background can be customized)
![Screenshot #6](https://user-images.githubusercontent.com/2292809/118050315-4e407e00-b37f-11eb-8ac9-17cc1a08aa08.png)
#### More context menu options
![Screenshot #7](https://user-images.githubusercontent.com/2292809/120905023-a5203700-c64f-11eb-8785-7d4557f2339e.png)

## Installation
Download installer and run to install (option to start when installer finishes), to remove the application run the uninstaller or use Windows *Add or remove programs* feature or via the Control Panels *Programs And Features* section. If you downloaded WeekNumber.zip via *Download latest release* first unzip the archive, then run the installer.

### Installation notes

Choose to keep the file downloaded if prompted (it will look similar to this, depends on your web browser and settings)

![Keep file if prompted #1](https://user-images.githubusercontent.com/2292809/120716901-fa7d0c80-c4c6-11eb-9232-f279f959f0a6.png)

![Keep file if prompted #2](https://user-images.githubusercontent.com/2292809/118524536-8c9eba00-b73e-11eb-9c6c-bc8defde0caa.png)

Use below actions to allow the installer to run on your PC when Microsoft SmartScreen is active:

![smartscreen_0](https://user-images.githubusercontent.com/2292809/120404034-c201f500-c345-11eb-9abd-670e927c4a36.png)

![smartscreen_1](https://user-images.githubusercontent.com/2292809/120404043-c4fce580-c345-11eb-945d-a5931bb5b721.png)


#### Distributed / Silent install / uninstall options

To silently install call the installer with /S parameter.
> WeekNumber_{VERSION}_Installer.exe /S

To silently uninstall call the uninstaller with /S parameter.
> Uninstall WeekNumber.exe /S

## Help section

All application features are accessable via right mouse click on the application icon residing in the Windows taskbar.
If the application icon is not visible then press the ^ symbol on the taskbar, click and hold on the application icon and then drag it to the visible taskbar area to pin it there.

### Context menu options:

- **About WeekNumber** - _Displays version information_
- **Check for updated version** - _Checks if newer version is available, option to update if that is the case_
- **Open application web page** - _Opens the default browser pointing to the application web page_
- **Update automatically** - _If ticked the application will update itself automatically without user interaction_
- **Settings** - _Additional application settings displayed in sub-menus_
   - **Start with Windows** - _Application starts automatically when user login to Windows if ticked_
   - **Application log** - _Application log settings in sub-menus_
      - **Use application log** - _Application writes to a log file if ticked (technical)_
      - **Show application log** - _If above is ticked, opens the application log file in Notepad (technical)_
   - **Startup message** - _Control application startup message in sub-menus_
      - **Display startup message** - _If ticked application will show a notification when started_
      - **Silent message** - _If ticked, startup message does not cause a system sound_
   - **Use small / large taskbar buttons** - _Toogles Windows taskbar size and also adjusts the application icon resolution to match. Having trouble seeing the week number? Try using large taskbar buttons_
   - **First Day Of Week** - _Calendar rule, select what day a week starts on (per default the application uses the systems regional settings to figure this out, but it can be manually overridden here)_
   - **Calendar week rule** - _Additional calendar rule that tells what rule is used for the first week of a year (per default the application uses the systems regional settings to figure this out, but it can be manually overridden here)_
   - **Icon colors** - _Allows user to change icon background and text (foreground) color or reset colors used back to default in sub-menus_
   - **Save icon...** - _Lets user save the current WeekNumber icon displayed to a .ico file_
   - **Icon resolution** - _Possibility to tweak the WeekNumber icon resolution, if the icon is fuzzy setting a higher or lower resolution can help with apperance_
- **Get week number for date...** - _Get week number for specified date (shows form where date can be entered)_
- **Exit WeekNumber** - _Closes the application. Start it again from the Windows Start Menu_

## Statistics

[![Open issues](https://img.shields.io/github/issues/voltura/WeekNumber)](https://github.com/voltura/WeekNumber/issues)
[![Code Quality](https://img.shields.io/github/workflow/status/voltura/WeekNumber/CodeQL)]()
[![Website Status](https://img.shields.io/website?url=https%3A%2F%2Fvoltura.github.io%2FWeekNumber%2F)]()

[![Number of programming langauges](https://img.shields.io/github/languages/count/voltura/WeekNumber)]()
[![Top programming language](https://img.shields.io/github/languages/top/voltura/WeekNumber)]()
[![Code size](https://img.shields.io/github/languages/code-size/voltura/WeekNumber)]()
[![Number of repo forks](https://img.shields.io/github/forks/voltura/WeekNumber)]()
[![Number of repo stars](https://img.shields.io/github/stars/voltura/WeekNumber)]()
[![goto counter](https://img.shields.io/github/search/voltura/WeekNumber/goto)]()
[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fvoltura%2FWeekNumber%2Fhit-counter&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)]()
![GitHub last commit](https://img.shields.io/github/last-commit/voltura/WeekNumber?color=red)

![Visitors](https://estruyf-github.azurewebsites.net/api/VisitorHit?user=volturaf&repo=WeekNumber&countColorcountColor&countColor=%235690f2)
