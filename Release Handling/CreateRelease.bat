SETLOCAL ENABLEDELAYEDEXPANSION
@ECHO OFF

TITLE Creating WeekNumber release...
COLOR 1E
CLS

:: ==========================
:: Optional input parameter
:: ==========================
:: Parameter values:
::  <no value given> = do not update version and do not publish
::  U                = update version but do not publish
::  P                = do not update version only publish
::  UP               = update version and publish
SET SCRIPT_PARAMETER=%1

:: ==========================
:: Global script variables
:: ==========================
SET "VERSION="
SET /A SCRIPT_RESULT=0
SET PUBLISH_REL=FALSE
SET UPDATE_VER=FALSE

IF "%SCRIPT_PARAMETER%" EQU "U" SET UPDATE_VER=TRUE
IF "%SCRIPT_PARAMETER%" EQU "P" SET PUBLISH_REL=TRUE
IF "%SCRIPT_PARAMETER%" EQU "UP" SET UPDATE_VER=TRUE && SET PUBLISH_REL=TRUE
SET SCRIPT_DIR=%CD%

:: ==========================
:: GitHub release API variables
:: ==========================
:: Get secret GitHub access token from external file into variable 'GITHUB_ACCESS_TOKEN'
CALL GITHUB_ACCESS_TOKEN.bat
SET REPO_OWNER=voltura
SET REPO_NAME=WeekNumber
:: v%VERSION%
SET "TAG_NAME=" 
:: BRANCH (master)
SET "TARGET_COMMITISH=master"
:: WeekNumber %VERSION%
SET "NAME="
::"Release of version %VERSION%"
SET "BODY="
SET DRAFT=false
SET PRERELEASE=false
SET "UPLOAD_URL="
SET "CURL_RESULT="
SET "UPLOAD_URL="

:: ==========================
:: Tools
:: ==========================
SET SEVEN_ZIP_FULLPATH=C:\Program Files\7-Zip\7z.exe
SET MSBUILD_FULLPATH=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe
SET FART=..\Tools\fart.exe
SET CURL=C:\Program Files\curl-7.76.1-win64-mingw\bin\curl.exe

:: ==========================
:: Logic
:: ==========================
CALL :UPDATE_VERSION
CALL :COMPILE_RELEASE
CALL :CREATE_INSTALLER_AND_FILES_FOR_RELEASE
CALL :PUBLISH_RELEASE
CD /D %SCRIPT_DIR%
START ReleaseManager.bat %SCRIPT_RESULT%
EXIT

:: ==========================
:: Functions
:: ==========================
:CREATE_INSTALLER_AND_FILES_FOR_RELEASE
ECHO.
CD "..\NSIS Installation"
START "Compile installer" /MIN /WAIT CompileInstaller.bat %VERSION%
SET RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%RESULT%
IF %RESULT%==0 (
	@COLOR 1E
	@ECHO Installer created successfully.
	@COLOR 1E
	@TIMEOUT /T 5 /NOBREAK >NUL	
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.exe
	@CALL :COMPRESS_INSTALLER
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.7z
	@CALL :COMPRESS_WEEKNUMBER_ZIP
	@CALL :GENERATE_MD5 WeekNumber.zip
	@CALL :GENERATE_VERSION_INFO %VERSION% WeekNumber_%VERSION%_Installer.exe
	@CALL :COPY_RELEASE
	DEL /F /Q WeekNumber_%VERSION%_Installer.log
) ELSE (
	@COLOR 1C
	@ECHO Installer not created, result = %RESULT%.
	@COLOR 1E
	@TIMEOUT /T 5 /NOBREAK >NUL
	NOTEPAD "%CD%\WeekNumber_%VERSION%_Installer.log"
	@START ReleaseManager.bat %SCRIPT_RESULT%
	@EXIT
)
GOTO :EOF

:GENERATE_MD5
COLOR 1E
ECHO.
ECHO Generating MD5 for '%1'...
SET "MD5="
for /F "skip=1" %%G in ('CertUtil -hashfile %1 MD5') do (
	@SET "MD5=%%G"
	@GOTO :GOT_MD5 %1
)
COLOR 1C
ECHO.
ECHO Failed to generate MD5 for '%1'.
TIMEOUT /T 5 /NOBREAK >NUL
COLOR 1E
GOTO :EOF

:GOT_MD5
COLOR 1A
ECHO.
ECHO %MD5%  %1> "%1.MD5"
COLOR 1E
ECHO.>> "%1.MD5"
GOTO :EOF

:COMPRESS_INSTALLER
ECHO.
IF NOT EXIST "%SEVEN_ZIP_FULLPATH%" (
	@COLOR 1C
	@ECHO 7-zip not found, cannot compress installer.
	@COLOR 1E
	@TIMEOUT /T 5 /NOBREAK >NUL
	@GOTO :EOF
)
"%SEVEN_ZIP_FULLPATH%" a -t7z -y WeekNumber_%VERSION%_Installer.7z WeekNumber_%VERSION%_Installer.exe WeekNumber_%VERSION%_Installer.exe.MD5
SET SEVEN_ZIP_RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%SEVEN_ZIP_RESULT%
IF "%SEVEN_ZIP_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO 7-zip failed to compress installer. Result = %SEVEN_ZIP_RESULT%
	@COLOR 1E
	@TIMEOUT /T 5 /NOBREAK >NUL
)
GOTO :EOF

:COMPRESS_WEEKNUMBER_ZIP
ECHO.
IF NOT EXIST "%SEVEN_ZIP_FULLPATH%" (
	@COLOR 1C
	@ECHO 7-zip not found, cannot compress installer.
	@COLOR 1E
	@TIMEOUT /T 5 /NOBREAK >NUL
	@GOTO :EOF
)
"%SEVEN_ZIP_FULLPATH%" a -tzip -y WeekNumber.zip WeekNumber_%VERSION%_Installer.exe WeekNumber_%VERSION%_Installer.exe.MD5
SET SEVEN_ZIP_RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%SEVEN_ZIP_RESULT%
IF "%SEVEN_ZIP_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO 7-zip failed to generate WeekNumber.zip. Result = %SEVEN_ZIP_RESULT%
	@COLOR 1E
	@TIMEOUT /T 5 /NOBREAK >NUL
)
GOTO :EOF

:GENERATE_VERSION_INFO
COLOR 1E
ECHO.
ECHO Generating VERSION.TXT...
ECHO %1 %2>VERSION.TXT
GOTO :EOF

:COPY_RELEASE
COLOR 1E
ECHO.
ECHO Copying release files to release folder...
MD ..\Releases\%VERSION%
MOVE /Y WeekNumber.zip ..\Releases\%VERSION%\
MOVE /Y WeekNumber.zip.MD5 ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.7z ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.7z.MD5 ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.exe ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.exe.MD5 ..\Releases\%VERSION%\
MOVE /Y VERSION.TXT ..\Releases\%VERSION%\
GOTO :EOF

:UPDATE_VERSION
COLOR 1E
ECHO.
ECHO Getting current version...
TYPE ..\Properties\AssemblyInfo.cs|FINDSTR AssemblyFileVersion >VERSION_REPLACE.TXT
SET /P AssemblyFileVersion=<VERSION_REPLACE.TXT
DEL /F /Q VERSION_REPLACE.TXT
SET CurrentAssemblyFileVersion=%AssemblyFileVersion:~32,-3%
SET "MAJOR="
SET "MINOR="
SET "BUILD="
SET "REVISION="

FOR /F "tokens=1,2,3,4 delims=." %%G IN ("%CurrentAssemblyFileVersion%") DO (
	SET /A MAJOR=%%G
	SET /A MINOR=%%H
	SET /A BUILD=%%I
	SET /A REVISION=%%J
)
IF "%UPDATE_VER%" EQU "TRUE" GOTO :UPDATE_REVISION

:SKIP_VERSION_UPDATE
SET VERSION=%MAJOR%.%MINOR%.%BUILD%.%REVISION%
COLOR 1E
ECHO.
ECHO Current version = %VERSION%, will not update the version.
TIMEOUT /T 5 /NOBREAK >NUL
GOTO :EOF

:UPDATE_REVISION
IF %REVISION% GEQ 9 GOTO :UPDATE_BUILD
SET /A REVISION=%REVISION%+1
SET NewAssemblyFileVersion=%MAJOR%.%MINOR%.%BUILD%.%REVISION%
GOTO :DO_VER_UPDATE

:UPDATE_BUILD
IF %BUILD% GEQ 9 GOTO :UPDATE_MINOR
SET /A BUILD=%BUILD%+1
SET NewAssemblyFileVersion=%MAJOR%.%MINOR%.%BUILD%.0
GOTO :DO_VER_UPDATE

:UPDATE_MINOR
IF %MINOR% GEQ 9 GOTO :UPDATE_MAJOR
SET /A MINOR=%MINOR%+1
SET NewAssemblyFileVersion=%MAJOR%.%MINOR%.0.0
GOTO :DO_VER_UPDATE

:UPDATE_MAJOR
SET /A MAJOR=%MAJOR%+1
SET NewAssemblyFileVersion=%MAJOR%.0.0.0

:DO_VER_UPDATE
COLOR 1E
ECHO.
ECHO Updating version from %CurrentAssemblyFileVersion% to %NewAssemblyFileVersion%...
%FART% -q ..\Properties\AssemblyInfo.cs %CurrentAssemblyFileVersion% %NewAssemblyFileVersion%
SET FART_RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%FART_RESULT%
IF "%FART_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO Failed to update version.
	@COLOR 1E
	@TIMEOUT /T 5 /NOBREAK >NUL
)
SET VERSION=%NewAssemblyFileVersion%
IF "%FART_RESULT%" NEQ "0" SET VERSION=%CurrentAssemblyFileVersion%
ECHO Current version set to %VERSION% (previous version was %CurrentAssemblyFileVersion%).
TIMEOUT /T 5 /NOBREAK >NUL
GOTO :EOF

:COMPILE_RELEASE
PUSHD ..
COLOR 1E
ECHO.
ECHO Updating Compiling a new release build...
CALL "%MSBUILD_FULLPATH%" WeekNumber.sln /p:Platform=x86 /property:Configuration=Release -m
SET BUILD_RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%BUILD_RESULT%
IF "%BUILD_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO.
	@ECHO BUILD FAILED. Cannot create release.
	@TIMEOUT /T 5 /NOBREAK >NUL
	@COLOR 1E
	@START ReleaseManager.bat %SCRIPT_RESULT%
	@EXIT
)
POPD
GOTO :EOF

:PUBLISH_RELEASE
IF "%PUBLISH_REL%" NEQ "TRUE" GOTO :EOF
COLOR 1E
ECHO.
ECHO Publishing release to Github...
SET TAG_NAME=v%VERSION%
SET NAME=WeekNumber %VERSION%
SET BODY=Release of version %VERSION%
"%CURL%" -s -H "Accept: application/vnd.github.v3+json" -H "Authorization: token %GITHUB_ACCESS_TOKEN%" -H "Content-Type:application/json" "https://api.github.com/repos/%REPO_OWNER%/%REPO_NAME%/releases" -d "{ \"tag_name\": \"%TAG_NAME%\", \"target_commitish\": \"%TARGET_COMMITISH%\",\"name\": \"%NAME%\",\"body\": \"%BODY%\",\"draft\": false, \"prerelease\": true}" >release_info.txt
SET CURL_RESULT=%ERRORLEVEL%
ECHO RESULT = %CURL_RESULT%
IF "%CURL_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO Failed to publish release
	@TIMEOUT /T 10 /NOBREAK >NUL
	@START ReleaseManager.bat %CURL_RESULT%
	@EXIT
)
ECHO.
ECHO Successfully published release.
TIMEOUT /T 10 /NOBREAK >NUL
CALL :PARSE_RELEASE_INFO
CALL :UPLOAD_RELEASE_ASSETS
GOTO :EOF

:PARSE_RELEASE_INFO
ECHO.
ECHO Parsing release info...
TYPE release_info.txt|FINDSTR upload_url >UPLOAD_URL.TXT
DEL /F /Q release_info.txt >NUL
SET /P UPLOAD_URL=<UPLOAD_URL.TXT
DEL /F /Q UPLOAD_URL.TXT >NUL
SET UPLOAD_URL=%UPLOAD_URL:~17,-15%
ECHO UPLOAD_URL=%UPLOAD_URL%
ECHO.
ECHO Successfully parsed received release info.
TIMEOUT /T 2 /NOBREAK >NUL
GOTO :EOF

:UPLOAD_RELEASE_ASSETS
ECHO.
ECHO Uploading release assets...
PUSHD ..\Releases\%VERSION%
CALL :UPLOAD_FILE WeekNumber.zip
CALL :UPLOAD_FILE WeekNumber.zip.MD5
CALL :UPLOAD_FILE WeekNumber_%VERSION%_Installer.7z zip
CALL :UPLOAD_FILE "WeekNumber_%VERSION%_Installer.7z.MD5"
CALL :UPLOAD_FILE "WeekNumber_%VERSION%_Installer.exe"
CALL :UPLOAD_FILE "WeekNumber_%VERSION%_Installer.exe.MD5"
CALL :UPLOAD_FILE VERSION.TXT
POPD
ECHO.
ECHO Upload completed.
GOTO :EOF

:UPLOAD_FILE
SET FILE_TO_UPLOAD=%1
CALL :CHECK_IF_MISSING_FILE %FILE_TO_UPLOAD%
ECHO.
ECHO Uploading %FILE_TO_UPLOAD%...
ECHO.
"%CURL%" -s -H "Accept: application/vnd.github.v3+json" -H "Authorization: token %GITHUB_ACCESS_TOKEN%" -H "Content-Type: application/octet-stream" --data-binary @%FILE_TO_UPLOAD% "%UPLOAD_URL%?name=%FILE_TO_UPLOAD%&label=%FILE_TO_UPLOAD%"
SET CURL_RESULT=%ERRORLEVEL%
:: Note: curl result can be 0 but file not uploaded, need to parse received json to validate success
ECHO RESULT = %CURL_RESULT%
IF "%CURL_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO Failed to upload %FILE_TO_UPLOAD%
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@START ReleaseManager.bat %CURL_RESULT%
	@EXIT
)
ECHO.
ECHO Successfully uploaded %FILE_TO_UPLOAD%.
TIMEOUT /T 5 /NOBREAK >NUL
GOTO :EOF

:CHECK_IF_MISSING_FILE
SET FILE_TO_CHECK=%1
ECHO.
IF NOT EXIST "%FILE_TO_CHECK%" (
	@COLOR 1C
	@ECHO Missing "%FILE_TO_CHECK%", cannot publish file.
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@START ReleaseManager.bat %CURL_RESULT%
	@EXIT
)
GOTO :EOF
