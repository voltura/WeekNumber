@ECHO OFF
TITLE Running script to create WeekNumber installer...
SET VERSION=1.6.2.0
START /WAIT CompileInstaller.bat %VERSION%
SET RESULT=%ERRORLEVEL%
IF %RESULT%==0 (
	@ECHO Installer created successfully.
) ELSE (
	@ECHO Installer not created, result = %RESULT%.
)
PAUSE