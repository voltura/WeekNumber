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
SET "PUBLISH_REL=FALSE"
SET "UPDATE_VER=FALSE"

IF "%SCRIPT_PARAMETER%" EQU "U" SET "UPDATE_VER=TRUE"
IF "%SCRIPT_PARAMETER%" EQU "P" SET "PUBLISH_REL=TRUE"
IF "%SCRIPT_PARAMETER%" EQU "UP" SET "UPDATE_VER=TRUE"
IF "%SCRIPT_PARAMETER%" EQU "UP" SET "PUBLISH_REL=TRUE"

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
CD /D %SCRIPT_DIR%
CD "..\NSIS Installation"
START "Compile installer" /MIN /WAIT CompileInstaller.bat %VERSION%
SET RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%RESULT%
IF %RESULT%==0 (
	@COLOR 1E
	@ECHO Installer created successfully.
	@COLOR 1E
	@TIMEOUT /T 2 /NOBREAK >NUL	
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.exe
	@CALL :COMPRESS_INSTALLER
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.7z
	@CALL :COMPRESS_WEEKNUMBER_ZIP
	@CALL :GENERATE_MD5 WeekNumber.zip
	@CALL :GENERATE_VERSION_INFO %VERSION% WeekNumber_%VERSION%_Installer.exe
	@CALL :COPY_RELEASE
	@CD /D %SCRIPT_DIR%
	@DEL /F /Q "..\NSIS Installation\WeekNumber_%VERSION%_Installer.log"
) ELSE (
	@COLOR 1C
	@ECHO Installer not created, result = %RESULT%.
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@CD /D %SCRIPT_DIR%
	NOTEPAD "..\NSIS Installation\WeekNumber_%VERSION%_Installer.log"
	@START ReleaseManager.bat %SCRIPT_RESULT%
	@EXIT
)
GOTO :EOF

:GENERATE_MD5
COLOR 1E
ECHO.
ECHO Generating MD5 for '%1'...
SET "MD5="
FOR /F "skip=1" %%G IN ('CertUtil -hashfile %1 MD5') DO (
	@SET "MD5=%%G"
	@GOTO :GOT_MD5 %1
)
COLOR 1C
ECHO.
ECHO Failed to generate MD5 for '%1'.
TIMEOUT /T 10 /NOBREAK >NUL
CD /D %SCRIPT_DIR%
COLOR 1E
START ReleaseManager.bat 11
EXIT
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
	@TIMEOUT /T 10 /NOBREAK >NUL
	@CD /D %SCRIPT_DIR%
	@START ReleaseManager.exe 10
	@EXIT
	@GOTO :EOF
)
"%SEVEN_ZIP_FULLPATH%" a -t7z -y WeekNumber_%VERSION%_Installer.7z WeekNumber_%VERSION%_Installer.exe WeekNumber_%VERSION%_Installer.exe.MD5
SET SEVEN_ZIP_RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%SEVEN_ZIP_RESULT%
IF "%SEVEN_ZIP_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO 7-zip failed to compress installer. Result = %SEVEN_ZIP_RESULT%
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@CD /D %SCRIPT_DIR%
	@START ReleaseManager.exe %SEVEN_ZIP_RESULT%
	@EXIT
)
GOTO :EOF

:COMPRESS_WEEKNUMBER_ZIP
ECHO.
IF NOT EXIST "%SEVEN_ZIP_FULLPATH%" (
	@COLOR 1C
	@ECHO 7-zip not found, cannot compress installer.
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@CD /D %SCRIPT_DIR%
	@START ReleaseManager.exe 22
	@GOTO :EOF
)
"%SEVEN_ZIP_FULLPATH%" a -tzip -y WeekNumber.zip WeekNumber_%VERSION%_Installer.exe WeekNumber_%VERSION%_Installer.exe.MD5
SET SEVEN_ZIP_RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%SEVEN_ZIP_RESULT%
IF "%SEVEN_ZIP_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO 7-zip failed to generate WeekNumber.zip. Result = %SEVEN_ZIP_RESULT%
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@CD /D %SCRIPT_DIR%
	@START ReleaseManager.exe %SEVEN_ZIP_RESULT%
	@EXIT
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
CD /D %SCRIPT_DIR%
MD ..\Releases\%VERSION% >NUL
IF NOT EXIST "..\NSIS Installation\WeekNumber.zip" GOTO :FAILED_COPY_RELEASE
MOVE /Y "..\NSIS Installation\WeekNumber.zip" ..\Releases\%VERSION%\
IF "%ERRORLEVEL%" NEQ "0" GOTO :FAILED_COPY_RELEASE
IF NOT EXIST "..\NSIS Installation\WeekNumber.zip.MD5" GOTO :FAILED_COPY_RELEASE
MOVE /Y "..\NSIS Installation\WeekNumber.zip.MD5" ..\Releases\%VERSION%\
IF "%ERRORLEVEL%" NEQ "0" GOTO :FAILED_COPY_RELEASE
IF NOT EXIST "..\NSIS Installation\WeekNumber_%VERSION%_Installer.7z" GOTO :FAILED_COPY_RELEASE
MOVE /Y "..\NSIS Installation\WeekNumber_%VERSION%_Installer.7z" ..\Releases\%VERSION%\
IF "%ERRORLEVEL%" NEQ "0" GOTO :FAILED_COPY_RELEASE
IF NOT EXIST "..\NSIS Installation\WeekNumber_%VERSION%_Installer.7z.MD5" GOTO :FAILED_COPY_RELEASE
MOVE /Y "..\NSIS Installation\WeekNumber_%VERSION%_Installer.7z.MD5" ..\Releases\%VERSION%\
IF "%ERRORLEVEL%" NEQ "0" GOTO :FAILED_COPY_RELEASE
IF NOT EXIST "..\NSIS Installation\WeekNumber_%VERSION%_Installer.exe" GOTO :FAILED_COPY_RELEASE
MOVE /Y "..\NSIS Installation\WeekNumber_%VERSION%_Installer.exe" ..\Releases\%VERSION%\
IF "%ERRORLEVEL%" NEQ "0" GOTO :FAILED_COPY_RELEASE
IF NOT EXIST "..\NSIS Installation\WeekNumber_%VERSION%_Installer.exe.MD5" GOTO :FAILED_COPY_RELEASE
MOVE /Y "..\NSIS Installation\WeekNumber_%VERSION%_Installer.exe.MD5" ..\Releases\%VERSION%\
IF "%ERRORLEVEL%" NEQ "0" GOTO :FAILED_COPY_RELEASE
IF NOT EXIST "..\NSIS Installation\VERSION.TXT" GOTO :FAILED_COPY_RELEASE
MOVE /Y "..\NSIS Installation\VERSION.TXT" ..\Releases\%VERSION%\
IF "%ERRORLEVEL%" NEQ "0" GOTO :FAILED_COPY_RELEASE
GOTO :EOF

:FAILED_COPY_RELEASE
ECHO.
COLOR 1C
ECHO Failed to copy release files, aborting...
COLOR 1E
CD /D %SCRIPT_DIR%
TIMEOUT /T 10 /NOBREAK >NUL
START ReleaseManager.exe 55
EXIT
GOTO :EOF

:UPDATE_VERSION
COLOR 1E
ECHO.
ECHO Getting current version...
CD /D %SCRIPT_DIR%
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
SET VERSION=%MAJOR%.%MINOR%.%BUILD%.%REVISION%
IF "%UPDATE_VER%" EQU "TRUE" CALL :UPDATE_REVISION
IF "%UPDATE_VER%" EQU "FALSE" CALL :SKIP_VERSION_UPDATE
GOTO :EOF

:SKIP_VERSION_UPDATE
COLOR 1E
ECHO.
ECHO Current version = %VERSION%, will not update the version.
TIMEOUT /T 2 /NOBREAK >NUL
GOTO :EOF

:UPDATE_REVISION
IF %REVISION% GEQ 9 CALL :UPDATE_BUILD
IF %REVISION% GEQ 9 GOTO :EOF
ECHO Updating revision number
SET /A REVISION=%REVISION%+1
SET NewAssemblyFileVersion=%MAJOR%.%MINOR%.%BUILD%.%REVISION%
CALL :DO_VER_UPDATE
GOTO :EOF

:UPDATE_BUILD
IF %BUILD% GEQ 9 CALL :UPDATE_MINOR
IF %BUILD% GEQ 9 GOTO :EOF
ECHO Updating build number
SET /A BUILD=%BUILD%+1
SET NewAssemblyFileVersion=%MAJOR%.%MINOR%.%BUILD%.0
CALL :DO_VER_UPDATE
GOTO :EOF

:UPDATE_MINOR
IF %MINOR% GEQ 9 CALL :UPDATE_MAJOR
IF %MINOR% GEQ 9 GOTO :EOF
ECHO Updating minor number
SET /A MINOR=%MINOR%+1
SET NewAssemblyFileVersion=%MAJOR%.%MINOR%.0.0
CALL :DO_VER_UPDATE
GOTO :EOF

:UPDATE_MAJOR
SET /A MAJOR=%MAJOR%+1
ECHO Updating major number
SET NewAssemblyFileVersion=%MAJOR%.0.0.0
CALL :DO_VER_UPDATE
GOTO :EOF

:DO_VER_UPDATE
COLOR 1E
ECHO.
ECHO Updating version from %CurrentAssemblyFileVersion% to %NewAssemblyFileVersion%...
CD /D %SCRIPT_DIR%
%FART% -q ..\Properties\AssemblyInfo.cs %CurrentAssemblyFileVersion% %NewAssemblyFileVersion%
SET FART_RESULT=%ERRORLEVEL%
IF "%FART_RESULT%" NEQ "1" (
	@COLOR 1C
	@ECHO Failed to update version.
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@CD /D %SCRIPT_DIR%
	@START ReleaseManager.bat 66
	@EXIT
)
SET VERSION=%NewAssemblyFileVersion%
IF "%FART_RESULT%" NEQ "1" SET VERSION=%CurrentAssemblyFileVersion%
ECHO Current version set to %VERSION% (previous version was %CurrentAssemblyFileVersion%).
TIMEOUT /T 2 /NOBREAK >NUL
GOTO :EOF

:COMPILE_RELEASE
CD /D %SCRIPT_DIR%
PUSHD ..
COLOR 1E
ECHO.
ECHO Updating Compiling a new release build...
CALL "%MSBUILD_FULLPATH%" WeekNumber.sln /p:Platform=x86 /t:Rebuild /property:Configuration=Release -m
SET BUILD_RESULT=%ERRORLEVEL%
SET /A SCRIPT_RESULT=%SCRIPT_RESULT%+%BUILD_RESULT%
IF "%BUILD_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO.
	@ECHO BUILD FAILED. Cannot create release.
	@TIMEOUT /T 10 /NOBREAK >NUL
	@COLOR 1E
	@CD /D %SCRIPT_DIR%
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
"%CURL%" -s -H "Accept: application/vnd.github.v3+json" -H "Authorization: token %GITHUB_ACCESS_TOKEN%" -H "Content-Type:application/json" "https://api.github.com/repos/%REPO_OWNER%/%REPO_NAME%/releases" -d "{ \"tag_name\": \"%TAG_NAME%\", \"target_commitish\": \"%TARGET_COMMITISH%\",\"name\": \"%NAME%\",\"body\": \"%BODY%\",\"draft\": %DRAFT%, \"prerelease\": %PRERELEASE%}" >release_info.txt
SET CURL_RESULT=%ERRORLEVEL%
ECHO RESULT = %CURL_RESULT%
IF "%CURL_RESULT%" NEQ "0" (
	@COLOR 1C
	@ECHO Failed to publish release
	@CD /D %SCRIPT_DIR%
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
CD /D %SCRIPT_DIR%
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
	@CD /D %SCRIPT_DIR%
	@START ReleaseManager.bat %CURL_RESULT%
	@EXIT
)
ECHO.
ECHO Successfully uploaded %FILE_TO_UPLOAD%.
TIMEOUT /T 2 /NOBREAK >NUL
GOTO :EOF

:CHECK_IF_MISSING_FILE
SET FILE_TO_CHECK=%1
ECHO.
IF NOT EXIST "%FILE_TO_CHECK%" (
	@COLOR 1C
	@ECHO Missing "%FILE_TO_CHECK%", cannot publish file.
	@COLOR 1E
	@TIMEOUT /T 10 /NOBREAK >NUL
	@CD /D %SCRIPT_DIR%
	@START ReleaseManager.bat %CURL_RESULT%
	@EXIT
)
GOTO :EOF
