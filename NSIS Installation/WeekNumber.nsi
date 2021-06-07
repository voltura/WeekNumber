!include "MUI2.nsh" ;Include Modern UI
!include nsDialogs.nsh
!include LogicLib.nsh

;--------------------------------
;General
Unicode True

!ifndef VERSION
  !define VERSION "1.0.0.0"
!endif

;LoadLanguageFile "${NSISDIR}\Contrib\Language files\English.nlf"
;LoadLanguageFile "${NSISDIR}\Contrib\Language files\Swedish.nlf"

!define PRODUCT_NAME "$(ProductNameLanguageSpecific)"
!define PRODUCT_VERSION ${VERSION}
!define PRODUCT_GROUP "WeekNumber"
!define PRODUCT_PUBLISHER "Voltura AB"
!define PRODUCT_WEB_SITE "https://voltura.github.io/WeekNumber"
!define PRODUCT_ID "{550adc75-8afb-4813-ac91-8c8c6cb681ae}"

;Name and file
Name "WeekNumber"
OutFile "WeekNumber_${VERSION}_Installer.exe"
BrandingText `${PRODUCT_NAME} Installer`
Caption "${PRODUCT_NAME} ${VERSION} Installer"
VIProductVersion ${VERSION}
VIAddVersionKey ProductName "${PRODUCT_NAME} Installer"
VIAddVersionKey Comments "An installer for ${PRODUCT_NAME}"
VIAddVersionKey CompanyName "${PRODUCT_PUBLISHER}"
VIAddVersionKey LegalCopyright "Copyright © ${PRODUCT_PUBLISHER} 2018-2021"
VIAddVersionKey FileDescription "${PRODUCT_NAME} Installer"
VIAddVersionKey FileVersion ${VERSION}
VIAddVersionKey ProductVersion ${VERSION}
VIAddVersionKey InternalName "${PRODUCT_NAME} Installer"
VIAddVersionKey OriginalFilename "WeekNumber_${VERSION}_Installer.exe"
VIAddVersionKey PrivateBuild "${VERSION}"
VIAddVersionKey SpecialBuild "${VERSION}"
;Default installation folder
InstallDir "$LOCALAPPDATA\Voltura AB\WeekNumber"
;Get installation folder from registry if available
InstallDirRegKey HKCU "Software\WeekNumber" ""
;Request application privileges for Windows
RequestExecutionLevel user

;--------------------------------
;Variables
Var StartMenuFolder
Var KillResult

;--------------------------------
;Macros
!macro AdjustConfigValue ConfigFile Key Value
   DetailPrint "Config: adding '${Key}'='${Value}' to ${ConfigFile}"
   nsisXML::create
   nsisXML::load ${ConfigFile}
   nsisXML::select "/configuration/appSettings/add[@key='${Key}']"
   nsisXML::setAttribute "value" ${Value}
   nsisXML::save ${ConfigFile}
!macroend


;--------------------------------
;Interface Settings
;Show all languages, despite user's codepage
!define MUI_LANGDLL_ALLLANGUAGES
!define MUI_ABORTWARNING
!define MUI_ICON "..\Resources\weekicon.ico"
!define MUI_UNICON "..\Resources\weekicon.ico"
!define MUI_HEADERIMAGE_BITMAP "..\Resources\WeekNumber.bmp"
!define MUI_HEADERIMAGE_BITMAP_STRETCH AspectFitHeight
!define MUI_HEADERIMAGE_UNBITMAP "..\Resources\WeekNumber.bmp"
!define MUI_HEADERIMAGE_UNBITMAP_STRETCH AspectFitHeight
!define MUI_CUSTOMFUNCTION_GUIINIT onGUIInit

;--------------------------------
;Language Selection Dialog Settings
;Remember the installer language
!define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
!define MUI_LANGDLL_REGISTRY_KEY "Software\WeekNumber" 
!define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language" 
 
;--------------------------------
;Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "$(LICENSETXT)"
!insertmacro MUI_PAGE_DIRECTORY
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\WeekNumber" 
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN "$INSTDIR\WeekNumber.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Run WeekNumber" 
!define MUI_FINISHPAGE_SHOWREADME ""
!define MUI_FINISHPAGE_SHOWREADME_TEXT "$(StartWithWindows)"
!define MUI_FINISHPAGE_SHOWREADME_FUNCTION configureStartWithWindows
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages
!insertmacro MUI_LANGUAGE "English" ; The first language is the default language
!insertmacro MUI_LANGUAGE "Swedish"

;Language strings
LicenseLangString LICENSETXT ${LANG_ENGLISH} "License.en-US.txt"

LicenseLangString LICENSETXT ${LANG_SWEDISH} "License.sv-SE.txt"

LangString WeekNumberIsRunning ${LANG_ENGLISH} "WeekNumber is running, close it and run installer again."

LangString WeekNumberIsRunning ${LANG_SWEDISH} "WeekNumber startad, avsluta programmet installera igen."

LangString unWeekNumberIsRunning ${LANG_ENGLISH} "WeekNumber is running, close it and run uninstaller again."

LangString unWeekNumberIsRunning ${LANG_SWEDISH} "WeekNumber startad, avsluta programmet och avinstallera igen."

LangString ProductNameLanguageSpecific ${LANG_ENGLISH} "WeekNumber by Voltura AB"

LangString ProductNameLanguageSpecific ${LANG_SWEDISH} "WeekNumber av Voltura AB"

LangString StartWithWindows ${LANG_ENGLISH} "Start WeekNumber with Windows"

LangString StartWithWindows ${LANG_SWEDISH} "Starta WeekNumber med Windows"

LangString CheckIfWeekNumberIsRunning ${LANG_ENGLISH} "Checking if WeekNumber is running..."

LangString CheckIfWeekNumberIsRunning ${LANG_SWEDISH} "Kontrollerar om WeekNumber körs..."

LangString ConfigStartWithWindows ${LANG_ENGLISH} "Configuring WeekNumber to start with Windows..."

LangString ConfigStartWithWindows ${LANG_SWEDISH} "Konfigurerar WeekNumber att starta med Windows..."

LangString WeekNumberRunningTryStop ${LANG_ENGLISH} "WeekNumber is running, trying to stop it..."

LangString WeekNumberRunningTryStop ${LANG_SWEDISH} "WeekNumber körs, försöker stoppa..."

LangString WeekNumberRunningAbortInstall ${LANG_ENGLISH} "Installation aborted - WeekNumber is running"

LangString WeekNumberRunningAbortInstall ${LANG_SWEDISH} "Installation avbruten - WeekNumber körs"

LangString WeekNumberRunningAbortUninstall ${LANG_ENGLISH} "Uninstall aborted - WeekNumber is running"

LangString WeekNumberRunningAbortUninstall ${LANG_SWEDISH} "Avinstallation avbruten - WeekNumber körs"

LangString WeekNumberRunningCloseBeforeInstall ${LANG_ENGLISH} "WeekNumber is running. Please close before install."

LangString WeekNumberRunningCloseBeforeInstall ${LANG_SWEDISH} "WeekNumber körs. Vänligen stäng innan installation."

LangString WeekNumberRunningCloseBeforeUninstall ${LANG_ENGLISH} "WeekNumber is running. Please close before uninstall."

LangString WeekNumberRunningCloseBeforeUninstall ${LANG_SWEDISH} "WeekNumber körs. Vänligen stäng innan avinstallation."

LangString WeekNumberNotRunning ${LANG_ENGLISH} "WeekNumber is not running"

LangString WeekNumberNotRunning ${LANG_SWEDISH} "WeekNumber körs ej"

;--------------------------------
;Reserve Files
  !insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Installer Sections
Section "WeekNumber application" SecWeekNumber
  Call weekNumberExeRunning
  ${If} $KillResult == "NOK"
    IfSilent +2
    Messagebox MB_OK|MB_ICONSTOP "$(WeekNumberIsRunning)"
    Abort
  ${EndIf}
  ;Uninstall old version
  ExecWait '"$INSTDIR\Uninstall WeekNumber.exe" /S _?=$INSTDIR'
  SetOutPath "$INSTDIR"
  File "..\bin\x86\Release\WeekNumber.exe"
  File "..\bin\x86\Release\WeekNumber.exe"
  SetOutPath "$INSTDIR\\sv-SE"
  SetOverwrite try
  File "..\bin\x86\Release\sv-SE\WeekNumber.resources.dll"
  SetOutPath "$INSTDIR\\en-US"
  File "..\bin\x86\Release\en-US\WeekNumber.resources.dll"
  ;Store installation folder
  WriteRegStr HKCU "Software\WeekNumber" "" $INSTDIR
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall WeekNumber.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "DisplayName" "WeekNumber by Voltura AB"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "UninstallString" "$\"$INSTDIR\Uninstall WeekNumber.exe$\""
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "QuietUninstallString" "$\"$INSTDIR\Uninstall WeekNumber.exe$\" /S"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "InstallLocation" "$\"$INSTDIR$\""
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "Publisher" "Voltura AB"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "Readme" "https://voltura.github.io/WeekNumber"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "URLUpdateInfo" "https://github.com/voltura/weeknumber/releases/latest/download/VERSION.TXT"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "URLInfoAbout" "https://voltura.github.io/WeekNumber"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "HelpLink" "https://voltura.github.io/WeekNumber"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "DisplayVersion" "${VERSION}"
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "NoModify" "1"
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "NoRepair" "1"
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "EstimatedSize" "512"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "Comments" "System tray app - shows current week number"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber" "DisplayIcon" "$\"$INSTDIR\WeekNumber.exe$\""
  
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\WeekNumber.lnk" "$INSTDIR\WeekNumber.exe"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\Uninstall WeekNumber.lnk" "$INSTDIR\Uninstall WeekNumber.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

;--------------------------------
;Uninstaller Section
Section "Uninstall"
  Call un.weekNumberExeRunning
  ${If} $KillResult == "NOK"
    IfSilent +2
    Messagebox MB_OK|MB_ICONSTOP "$(UnWeekNumberIsRunning)"
    Abort
  ${EndIf}
  Delete "$INSTDIR\WeekNumber.exe"
  Delete "$INSTDIR\sv-SE\WeekNumber.resources.dll"
  Delete "$INSTDIR\en-US\WeekNumber.resources.dll"
  
  ;do not delete old application log files if silently uninstalling, 
  ;normal scenario for silent uninstall is auto-update
  IfSilent +2
  Delete "$INSTDIR\WeekNumber.exe.log"
  
  Delete "$INSTDIR\WeekNumber.exe.Config"
  Delete "$INSTDIR\Uninstall WeekNumber.exe"
  
  ;do not delete old application log files if silently uninstalling, 
  ;normal scenario for silent uninstall is auto-update
  IfSilent +5
  Delete "$LOCALAPPDATA\WeekNumber.exe.log"
  Delete "$LOCALAPPDATA\WeekNumber\WeekNumber.exe.log"
  Delete "$LOCALAPPDATA\Voltura AB\WeekNumber.exe.log"
  Delete "$LOCALAPPDATA\Voltura AB\WeekNumber\WeekNumber.exe.log"
  
  Delete "$LOCALAPPDATA\Temp\WeekNumberCleanup.exe"
  Delete "$LOCALAPPDATA\Temp\WeekNumber_*_Installer.exe"
  Delete "$LOCALAPPDATA\Temp\WeekNumber_*_Installer.exe.MD5"
  
  ;do not remove install folder if silent uninstall (normally happens during auto-update), 
  ;we want to keep log file
  IfSilent +2
  RMDir "$INSTDIR"
  
  RMDir "$INSTDIR\sv-SE"
  RMDir "$INSTDIR\en-US"
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  Delete "$SMPROGRAMS\$StartMenuFolder\WeekNumber.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall WeekNumber.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"

  ;do not remove local appdata folder if silent uninstall (normally happens during auto-update), 
  ;we want to keep log file
  IfSilent +2
  RMDir "$LOCALAPPDATA\WeekNumber"

  DeleteRegKey HKCU "Software\WeekNumber"
  IfSilent +3
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Run\WeekNumber"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\WeekNumber"
  
SectionEnd

;--------------------------------
; Functions
Function configureStartWithWindows
    ;Add Start with Windows setting
    DetailPrint "$(ConfigStartWithWindows)"
    !insertmacro AdjustConfigValue "$INSTDIR\WeekNumber.exe.config" "StartWithWindows" "True"	
FunctionEnd

Function onGUIInit
  Aero::Apply
FunctionEnd

Function weekNumberExeRunning
  StrCpy $R0 2
  DetailPrint "$(CheckIfWeekNumberIsRunning)"
  System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "550adc75-8afb-4813-ac91-8c8c6cb681ae") i .R0'
  IntCmp $R0 0 notRunning
  System::Call 'kernel32::CloseHandle(i $R0)'
  DetailPrint "$(WeekNumberRunningTryStop)"
  LockedList::CloseProcess /kill WeekNumber.exe
  Sleep 2000
  System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "550adc75-8afb-4813-ac91-8c8c6cb681ae") i .R0'
  IntCmp $R0 0 notRunning
  DetailPrint "$(WeekNumberRunningAbortInstall)"
  IfSilent +2
  MessageBox MB_OK|MB_ICONEXCLAMATION "$(WeekNumberRunningCloseBeforeInstall)" /SD IDOK
  StrCpy $KillResult "NOK"
  Abort
  notRunning:
  StrCpy $KillResult "OK"
  DetailPrint "$(WeekNumberNotRunning)"
FunctionEnd

Function un.weekNumberExeRunning
  StrCpy $R0 2
  DetailPrint "$(CheckIfWeekNumberIsRunning)"
  System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "550adc75-8afb-4813-ac91-8c8c6cb681ae") i .R0'
  IntCmp $R0 0 notRunning
  System::Call 'kernel32::CloseHandle(i $R0)'
  DetailPrint "$(WeekNumberRunningTryStop)"
  LockedList::CloseProcess /kill WeekNumber.exe
  Sleep 2000
  System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "550adc75-8afb-4813-ac91-8c8c6cb681ae") i .R0'
  IntCmp $R0 0 notRunning
  DetailPrint "$(WeekNumberRunningAbortUninstall)"
  IfSilent +2
  MessageBox MB_OK|MB_ICONEXCLAMATION "$(WeekNumberRunningCloseBeforeUninstall)" /SD IDOK
  StrCpy $KillResult "NOK"
  Abort
  notRunning:
  StrCpy $KillResult "OK"
  DetailPrint "$(WeekNumberNotRunning)"
FunctionEnd

Function .onInstSuccess
  IfSilent 0 +2
  Exec '"$INSTDIR\WeekNumber.exe"'

FunctionEnd

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd