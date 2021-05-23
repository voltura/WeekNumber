!include "MUI2.nsh" ;Include Modern UI
!include nsDialogs.nsh
!include LogicLib.nsh

;--------------------------------
;General
Unicode True

!ifndef VERSION
  !define VERSION "1.6.2.1"
!endif

!define PRODUCT_NAME "WeekNumber by Voltura AB"
!define PRODUCT_VERSION ${VERSION}
!define PRODUCT_GROUP "WeekNumber"
!define PRODUCT_PUBLISHER "Voltura AB"
!define PRODUCT_WEB_SITE "https://github.com/voltura"
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
;Request application privileges for Windows Vista
RequestExecutionLevel user

;--------------------------------
;Variables
Var StartMenuFolder
;Var Dialog
;Var Label
;Var Checkbox
;Var Checkbox_State
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
!define MUI_ABORTWARNING
!define MUI_ICON "..\Resources\weekicon.ico"
!define MUI_UNICON "..\Resources\weekicon.ico"
!define MUI_HEADERIMAGE_BITMAP "..\Resources\WeekNumber.bmp"
!define MUI_HEADERIMAGE_BITMAP_STRETCH AspectFitHeight
!define MUI_HEADERIMAGE_UNBITMAP "..\Resources\WeekNumber.bmp"
!define MUI_HEADERIMAGE_UNBITMAP_STRETCH AspectFitHeight
!define MUI_CUSTOMFUNCTION_GUIINIT onGUIInit
 
;--------------------------------
;Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "License.txt"
!insertmacro MUI_PAGE_DIRECTORY
;Start Menu Folder Page Configuration
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\WeekNumber" 
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
;Page custom nsDialogsSettingsPage nsDialogsSettingsPageLeave
!define MUI_FINISHPAGE_RUN "$INSTDIR\WeekNumber.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Run WeekNumber" 

!define MUI_FINISHPAGE_SHOWREADME ""
!define MUI_FINISHPAGE_SHOWREADME_TEXT "Start WeekNumber with Windows"
!define MUI_FINISHPAGE_SHOWREADME_FUNCTION configureStartWithWindows
;!define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED

!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages
!insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections
Section "WeekNumber application" SecWeekNumber
  Call weekNumberExeRunning
  ${If} $KillResult == "NOK"
    IfSilent +2
    Messagebox MB_OK|MB_ICONSTOP "WeekNumber is running, close it and run installer again."
    Abort
  ${EndIf}
  ;Uninstall old version
  ExecWait '"$INSTDIR\Uninstall WeekNumber.exe" /S _?=$INSTDIR'
  SetOutPath "$INSTDIR"
  File "..\bin\x86\Release\WeekNumber.exe"
  File "..\bin\x86\Release\WeekNumber.exe.Config"
  ;Store installation folder
  WriteRegStr HKCU "Software\WeekNumber" "" $INSTDIR
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall WeekNumber.exe"
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\WeekNumber.lnk" "$INSTDIR\WeekNumber.exe"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\Uninstall WeekNumber.lnk" "$INSTDIR\Uninstall WeekNumber.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

;--------------------------------
;Descriptions
;Language strings
LangString DESC_SecWeekNumber ${LANG_ENGLISH} "WeekNumber application"

;--------------------------------
;Uninstaller Section
Section "Uninstall"
  Delete "$INSTDIR\WeekNumber.exe"
  Delete "$INSTDIR\WeekNumber.exe.log"
  Delete "$INSTDIR\WeekNumber.exe.Config"
  Delete "$INSTDIR\Uninstall WeekNumber.exe"
  RMDir "$INSTDIR"
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  Delete "$SMPROGRAMS\$StartMenuFolder\WeekNumber.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall WeekNumber.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  DeleteRegKey HKCU "Software\WeekNumber"
  IfSilent +2
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Run\WeekNumber"
  
SectionEnd

;--------------------------------
; Functions
;Function nsDialogsSettingsPage
;  DetailPrint "Displaying Setting Page"
;  nsDialogs::Create 1018
;  Pop $Dialog
;  ${If} $Dialog == error
;  	Abort
;  ${EndIf}
;  ${NSD_CreateLabel} 0 0 100% 12u "Tick the checkbox if you want WeekNumber to start with Windows"
;  Pop $Label
;  ${NSD_CreateCheckbox} 0 30u 100% 10u "&Start with Windows"
;  Pop $Checkbox
;  ${NSD_SetState} $Checkbox_State ${BST_CHECKED}
;  nsDialogs::Show
;FunctionEnd

Function configureStartWithWindows
    ;Add Start with Windows setting
    DetailPrint "Configuring WeekNumber to start with Windows..."
    !insertmacro AdjustConfigValue "$INSTDIR\WeekNumber.exe.config" "StartWithWindows" "True"	
FunctionEnd

;Function nsDialogsSettingsPageLeave
;  ${NSD_GetState} $Checkbox $Checkbox_State
;  ${If} $Checkbox_State == ${BST_CHECKED}
;    ;Add Start with Windows setting
;    DetailPrint "Configuring WeekNumber to start with Windows..."
;    !insertmacro AdjustConfigValue "$INSTDIR\WeekNumber.exe.config" "StartWithWindows" "True"	
;  ${EndIf}
;FunctionEnd

Function onGUIInit
  Aero::Apply
FunctionEnd

Function weekNumberExeRunning
  StrCpy $R0 2
  DetailPrint "Checking if WeekNumber is running..."
  System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "550adc75-8afb-4813-ac91-8c8c6cb681ae") i .R0'
  IntCmp $R0 0 notRunning
  System::Call 'kernel32::CloseHandle(i $R0)'
  DetailPrint "WeekNumber is running, trying to stop it..."
  LockedList::CloseProcess /kill WeekNumber.exe
  Sleep 2000
  System::Call 'kernel32::OpenMutex(i 0x100000, b 0, t "550adc75-8afb-4813-ac91-8c8c6cb681ae") i .R0'
  IntCmp $R0 0 notRunning
  DetailPrint "Installation aborted - WeekNumber is running"
  IfSilent +2
  MessageBox MB_OK|MB_ICONEXCLAMATION "WeekNumber #1 is running. Please close before installing." /SD IDOK
  StrCpy $KillResult "NOK"
  Abort
  notRunning:
  StrCpy $KillResult "OK"
  DetailPrint "WeekNumber is not running"
FunctionEnd

Function .onInstSuccess

IfSilent 0 +2
Exec '"$INSTDIR\WeekNumber.exe"'

FunctionEnd