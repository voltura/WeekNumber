SETLOCAL ENABLEDELAYEDEXPANSION
@ECHO OFF

SET Result=%1
SET ScriptVersion=1.2
SET SCRIPT_DIR=%~dp0

TITLE WeekNumber Release Manager %ScriptVersion%
MODE 56,20
CLS

IF "%Result%" EQU "" GOTO :MENU

IF "%Result%" NEQ "0" (
	@COLOR 1C
	@ECHO ======================================
	@ECHO Last operation failed!
	@ECHO ======================================
	@ECHO Return code = %Result%
)
IF "%Result%" EQU "0" (
	@COLOR 1A
	@ECHO ======================================
	@ECHO Last operation completed successfully!
	@ECHO ======================================
)

:MENU
CD /D "%SCRIPT_DIR%"
ECHO.
COLOR 1E
ECHO ======================================
ECHO WeekNumber Release Manager version %ScriptVersion%
ECHO ======================================
ECHO.
ECHO A: Build release
ECHO B: Update version and build release
ECHO C: Build release and publish
ECHO D: Update version, build release and publish
ECHO X: Exit
ECHO.
CHOICE /C ABCDX /M "Select action"
SET USER_SELECTION=%ERRORLEVEL%

IF %USER_SELECTION%==1 START /MAX CreateRelease.bat
IF %USER_SELECTION%==2 START /MAX CreateRelease.bat U
IF %USER_SELECTION%==3 START /MAX CreateRelease.bat P
IF %USER_SELECTION%==4 START /MAX CreateRelease.bat UP

EXIT
