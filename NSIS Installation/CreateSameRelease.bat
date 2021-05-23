SETLOCAL ENABLEDELAYEDEXPANSION
@ECHO OFF

TITLE Creating WeekNumber release with same version...
CLS

CALL CreateNewRelease.bat SKIP_VERSION_UPDATE
EXIT
