SETLOCAL ENABLEDELAYEDEXPANSION
@ECHO OFF
SET Result=%1
SET ScriptVersion=1.1
TITLE WeekNumber Release Manager %ScriptVersion%
CLS
IF "%Result%" EQU "" GOTO :MENU
IF "%Result%" NEQ "0" (
	@COLOR 0C
	@ECHO ======================================
	@ECHO Last operation failed!
	@ECHO ======================================
	@ECHO Return code = %Result%
)
IF "%Result%" EQU "0" (
	@COLOR 0A
	@ECHO ======================================
	@ECHO Last operation completed successfully!
	@ECHO ======================================
)

:MENU
ECHO.
COLOR 1E
ECHO ======================================
ECHO WeekNumber Release Manager version %ScriptVersion%
ECHO ======================================
ECHO.
ECHO A: Build release
ECHO B: Update version and build release
ECHO C: Build release and publish it
ECHO D: Update version, build release and publish it
ECHO E: Exit
ECHO.
CHOICE /C ABCDE /M "Select an option"
SET USER_SELECTION=%ERRORLEVEL%
IF %USER_SELECTION%==1 START CreateRelease.bat
IF %USER_SELECTION%==2 START CreateRelease.bat U
IF %USER_SELECTION%==3 START CreateRelease.bat P
IF %USER_SELECTION%==4 START CreateRelease.bat UP
EXIT
