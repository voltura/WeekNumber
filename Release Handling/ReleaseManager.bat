SETLOCAL ENABLEDELAYEDEXPANSION
ECHO OFF

SET Result=%1
SET ScriptVersion=1.4
SET SCRIPT_DIR=%~dp0

TITLE WeekNumber Release Manager v%ScriptVersion%
MODE 60,34
CLS

IF "%Result%" EQU "" GOTO :MENU

IF "%Result%" NEQ "0" (
	ECHO  Last operation failed. Return code = %Result%
	ECHO  =======================================
)
IF "%Result%" EQU "0" (
	ECHO  Last operation completed successfully!
	ECHO  =======================================
)

:MENU
CD /D "%SCRIPT_DIR%"
COLOR 1F
ECHO    __________       .__
ECHO    \______   \ ____ ^|  ^|   ____ _____    ______ ____
ECHO     ^|       _// __ \^|  ^| _/ __ \\__  \  /  ___// __ \
ECHO     ^|    ^|   \  ___/^|  ^|_\  ___/ / __ \_\___ \\  ___/
ECHO     ^|____^|_  /\___  ^>____/\___  ^>____  /____  ^>\___  ^>
ECHO            \/     \/          \/     \/     \/     \/
ECHO      _____                                             
ECHO     /     \ _____    ____ _____     ____   ___________
ECHO    /  \ /  \\__  \  /    \\__  \   / ___\_/ __ \_  __ \
ECHO   /    Y    \/ __ \^|   ^|  \/ __ \_/ /_/  ^>  ___/^|  ^| \/
ECHO   \____^|__  (____  /___^|  (____  /\___  / \___  ^>__^|
ECHO           \/     \/     \/     \//_____/      \/
ECHO                                                      v%ScriptVersion%
ECHO  1) Build solution and create release
ECHO.
ECHO  2) Update version, sync sources, 
ECHO     build solution and create release
ECHO.
ECHO  3) Build solution, create and publish release
ECHO.
ECHO  4) Build solution, create and publish pre-release
ECHO.
ECHO  5) Update version, sync sources, build
ECHO     solution, create and publish release
ECHO.
ECHO  6) Update version, sync sources, build
ECHO     solution, create and publish pre-release
ECHO.
ECHO  7) Sync sources 8) Show releases X) Exit
ECHO.
CHOICE /C 12345678X /N /M "Select action:"
SET USER_SELECTION=%ERRORLEVEL%
IF %USER_SELECTION%==1 START /MAX CreateRelease.bat A
IF %USER_SELECTION%==2 START /MAX CreateRelease.bat U
IF %USER_SELECTION%==3 START /MAX CreateRelease.bat P
IF %USER_SELECTION%==4 START /MAX CreateRelease.bat P P
IF %USER_SELECTION%==5 START /MAX CreateRelease.bat UP
IF %USER_SELECTION%==6 START /MAX CreateRelease.bat UP P
IF %USER_SELECTION%==7 CALL :SYNC_SOURCE
IF %USER_SELECTION%==8 CALL :LAUNCH_CMD
IF %USER_SELECTION%==9 EXIT
IF %USER_SELECTION% LEQ 7 EXIT
CLS
GOTO :MENU

:SYNC_SOURCE
CLS
CALL :DISP_MSG "Syncing sources, please wait..." 0 0
GIT pull -q >NUL 2>&1
CALL :DISP_MSG " - Get changes from git" %ERRORLEVEL% 0
GIT add --all >NUL 2>&1
CALL :DISP_MSG " - Add all changes" %ERRORLEVEL% 0
GIT commit -a -m "Auto update via Release Manager %ScriptVersion%" >NUL 2>&1
CALL :DISP_MSG " - Commit all changes" %ERRORLEVEL% 0
GIT push --all >NUL 2>&1
CALL :DISP_MSG " - Push all changes" %ERRORLEVEL% 0
CALL :DISP_MSG "Sync complete." 0 1
GOTO :EOF

:DISP_MSG
SET MSG=%1
SET MSG=%MSG:~1,-1%
SET CODE=%2
SET /A DELAY_SEC=%3+0
IF "%CODE%" NEQ "0" COLOR 4F
ECHO.
ECHO   %MSG%
IF "%CODE%" NEQ "0" (
	ECHO   ==========================
	ECHO   Result: %CODE%
	ECHO   ==========================
	ECHO   Press any key to return to menu...
	PAUSE >NUL
	GOTO :MENU
) ELSE (
	TIMEOUT /T %DELAY_SEC% /NOBREAK >NUL
)
GOTO :EOF

:LAUNCH_CMD
PUSHD "%SCRIPT_DIR%\..\Releases"
START "WeekNumber Release Manager v%ScriptVersion% Command line" /I CMD.EXE /K DIR /O-D /4 /S
POPD
GOTO :EOF
