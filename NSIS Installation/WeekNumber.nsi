;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "WeekNumber ${VERSION}"
  OutFile "WeekNumberInstaller - ${VERSION}.exe"

  ;Default installation folder
  InstallDir "$LOCALAPPDATA\WeekNumber"

  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\WeekNumber" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel user

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
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
  
  # uninstall old version
  ExecWait '"$INSTDIR\Uninstall.exe" /S _?=$INSTDIR'

  SetOutPath "$INSTDIR"

  ;ADD YOUR OWN FILES HERE...
  File "..\bin\x86\Release\WeekNumber.exe"
  File "..\bin\x86\Release\WeekNumber.exe.Config"
  ;Store installation folder
  WriteRegStr HKCU "Software\WeekNumber" "" $INSTDIR

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecWeekNumber ${LANG_ENGLISH} "WeekNumber application"

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecWeekNumber} $(DESC_SecWeekNumber)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...

  Delete "$INSTDIR\WeekNumber.exe"
  Delete "$INSTDIR\WeekNumber.exe.Config"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\WeekNumber"

SectionEnd
