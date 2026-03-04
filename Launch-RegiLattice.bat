@echo off
:: Launch-RegiLattice.bat — Elevated launcher for RegiLattice.
:: Prefers PowerShell 7 (pwsh.exe), falls back to Windows PowerShell 5.1.

setlocal
set "psScript=%~dp0RegiLatticeMenu.ps1"

:: Locate the best available PowerShell
set "pwsh="
where pwsh.exe >nul 2>&1 && set "pwsh=pwsh.exe" && goto :launch
if exist "%ProgramFiles%\PowerShell\7\pwsh.exe" (
    set "pwsh=%ProgramFiles%\PowerShell\7\pwsh.exe"
    goto :launch
)
:: Fall back to built-in Windows PowerShell
set "pwsh=powershell.exe"

:launch
echo Starting RegiLattice with %pwsh% ...
"%pwsh%" -ExecutionPolicy Bypass -Command ^
    "Start-Process '%pwsh%' -ArgumentList '-ExecutionPolicy','Bypass','-NoExit','-File','\"%psScript%\"' -Verb RunAs"
endlocal
