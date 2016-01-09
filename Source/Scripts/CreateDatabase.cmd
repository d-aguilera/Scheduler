@echo off

if "%~1"=="" goto usage

set schedulerdb=Scheduler

echo.
echo This script will create the [%schedulerdb%] database and schema objects.
echo.
echo Database files will be created in folder: %~1
echo.
echo Press ENTER to proceed, or Ctrl-C to abort.

pause > nul

SQLCMD.EXE -E -v varDbName="%schedulerdb%" -v varDataPath="%~1" -i CreateDatabase.sql

echo Database created successfully.
echo.

goto :eof

:usage
echo.
echo Missing parameters.
echo.
echo Usage: CreateDatabase ^<PATH_TO_SQLSERVER_DATA_FOLDER^>
echo.
echo.  Where the data path may be for instance "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA"
echo.