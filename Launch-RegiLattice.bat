@echo off
:: Launch-RegiLattice.bat — Starts RegiLattice via Python.
:: Prefers Python on PATH; falls back to common install locations.

setlocal

:: Locate Python
set "PY="
where python >nul 2>&1 && set "PY=python" && goto :launch
if exist "%ProgramFiles%\Python314\python.exe" (
    set "PY=%ProgramFiles%\Python314\python.exe"
    goto :launch
)
if exist "%ProgramFiles%\Python312\python.exe" (
    set "PY=%ProgramFiles%\Python312\python.exe"
    goto :launch
)
if exist "%LocalAppData%\Programs\Python\Python312\python.exe" (
    set "PY=%LocalAppData%\Programs\Python\Python312\python.exe"
    goto :launch
)

echo ERROR: Python not found. Install Python 3.10+ from https://python.org
pause
exit /b 1

:launch
echo Starting RegiLattice with %PY% ...

:: Pass CLI arguments through: --gui, --list, apply, remove, etc.
"%PY%" -m regilattice %*

if errorlevel 1 (
    echo.
    echo RegiLattice exited with errors. Check the log for details.
    pause
)
endlocal
