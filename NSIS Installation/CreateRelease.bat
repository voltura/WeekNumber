SETLOCAL ENABLEDELAYEDEXPANSION
@ECHO OFF

TITLE Creating WeekNumber release...
CLS

SET "VERSION="
SET SEVEN_ZIP_FULLPATH=C:\Program Files\7-Zip\7z.exe
SET MSBUILD_FULLPATH=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe
SET FART=..\Tools\fart.exe
CALL :UPDATE_VERSION
CALL :COMPILE_RELEASE

START /WAIT CompileInstaller.bat %VERSION%
SET RESULT=%ERRORLEVEL%
IF %RESULT%==0 (
	@ECHO Installer created successfully.
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.exe
	@CALL :COMPRESS_INSTALLER
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.7z
	@CALL :COMPRESS_WEEKNUMBER_ZIP
	@CALL :GENERATE_MD5 WeekNumber.zip
	@CALL :GENERATE_VERSION_INFO %VERSION% WeekNumber_%VERSION%_Installer.exe
	@CALL :COPY_RELEASE
	DEL /F /Q WeekNumber_%VERSION%_Installer.log
) ELSE (
	@ECHO Installer not created, result = %RESULT%.
	@PAUSE
)
EXIT

:GENERATE_MD5
SET "MD5="
for /F "skip=1" %%G in ('CertUtil -hashfile %1 MD5') do (
	@SET "MD5=%%G"
	@GOTO :GOT_MD5 %1
)
GOTO :EOF

:GOT_MD5
ECHO %MD5%  %1> "%1.MD5"
ECHO.>> "%1.MD5"
GOTO :EOF

:COMPRESS_INSTALLER
IF NOT EXIST "%SEVEN_ZIP_FULLPATH%" (
	@ECHO 7-zip not found, cannot compress installer.
	@GOTO :EOF
)
"%SEVEN_ZIP_FULLPATH%" a -t7z -y WeekNumber_%VERSION%_Installer.7z WeekNumber_%VERSION%_Installer.exe WeekNumber_%VERSION%_Installer.exe.MD5
GOTO :EOF

:COMPRESS_WEEKNUMBER_ZIP
IF NOT EXIST "%SEVEN_ZIP_FULLPATH%" (
	@ECHO 7-zip not found, cannot compress installer.
	@GOTO :EOF
)
"%SEVEN_ZIP_FULLPATH%" a -tzip -y WeekNumber.zip WeekNumber_%VERSION%_Installer.exe WeekNumber_%VERSION%_Installer.exe.MD5
GOTO :EOF

:GENERATE_VERSION_INFO
ECHO %1 %2>VERSION.TXT
GOTO :EOF

:COPY_RELEASE
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
%FART% -q ..\Properties\AssemblyInfo.cs %CurrentAssemblyFileVersion% %NewAssemblyFileVersion%
SET VERSION=%NewAssemblyFileVersion%
GOTO :EOF

:COMPILE_RELEASE
PUSHD ..
"%MSBUILD_FULLPATH%" WeekNumber.sln /p:Platform=x86 /property:Configuration=Release -m
POPD
GOTO :EOF
