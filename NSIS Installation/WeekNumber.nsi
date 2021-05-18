;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  !define PRODUCT_NAME "WeekNumber by Voltura AB"
  !define PRODUCT_VERSION ${VERSION}
  !define PRODUCT_GROUP "WeekNumber"
  !define PRODUCT_PUBLISHER "Voltura AB"
  !define PRODUCT_WEB_SITE "https://github.com/voltura"
  !define PRODUCT_ID "{550adc75-8afb-4813-ac91-8c8c6cb681ae}"

  ;Name and file
  Name "WeekNumber ${VERSION}"
  OutFile "WeekNumber_${VERSION}_Installer.exe"

  Caption "${PRODUCT_NAME} Installer"
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
  ;LegalTrademarks

  ;Default installation folder
  InstallDir "$LOCALAPPDATA\Voltura AB\WeekNumber"

  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\WeekNumber" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel user

;--------------------------------
;Variables

  Var StartMenuFolder
  
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING
  !define MUI_ICON "..\Resources\weekicon.ico"
  !define MUI_UNICON "..\Resources\weekicon.ico"
  !define MUI_HEADERIMAGE_BITMAP "..\Resources\WeekNumber.bmp"
  !define MUI_HEADERIMAGE_BITMAP_STRETCH AspectFitHeight
  !define MUI_HEADERIMAGE_UNBITMAP "..\Resources\WeekNumber.bmp"
  !define MUI_HEADERIMAGE_UNBITMAP_STRETCH AspectFitHeight
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

  !define MUI_FINISHPAGE_RUN "$INSTDIR\WeekNumber.exe"
  !define MUI_FINISHPAGE_RUN_TEXT "Run WeekNumber" 

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

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecWeekNumber} $(DESC_SecWeekNumber)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\WeekNumber.exe"
  Delete "$INSTDIR\WeekNumber.exe.Config"
  Delete "$INSTDIR\Uninstall WeekNumber.exe"
  RMDir "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
    
  Delete "$SMPROGRAMS\$StartMenuFolder\WeekNumber.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall WeekNumber.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  
  DeleteRegKey /ifempty HKCU "Software\WeekNumber"
  
SectionEnd
