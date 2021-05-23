@ECHO OFF

TITLE Running script to create WeekNumber installer...
CLS

SET VERSION=1.6.2.1
SET SEVEN_ZIP_FULLPATH=C:\Program Files\7-Zip\7z.exe

START /WAIT CompileInstaller.bat %VERSION%
SET RESULT=%ERRORLEVEL%
IF %RESULT%==0 (
	@ECHO Installer created successfully.
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.exe
	@CALL :COMPRESS_INSTALLER
	@CALL :GENERATE_MD5 WeekNumber_%VERSION%_Installer.7z
	@CALL :COMPRESS_WEEKNUMBER_ZIP
	@CALL :GENERATE_MD5 WeekNumber.zip
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

:COPY_RELEASE
MD ..\Releases\%VERSION%
MOVE /Y WeekNumber.zip ..\Releases\%VERSION%\
MOVE /Y WeekNumber.zip.MD5 ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.7z ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.7z.MD5 ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.exe ..\Releases\%VERSION%\
MOVE /Y WeekNumber_%VERSION%_Installer.exe.MD5 ..\Releases\%VERSION%\
GOTO :EOF
