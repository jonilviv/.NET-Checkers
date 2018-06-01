@echo off

for /f "tokens=*" %%C in ('dir /a:d /s /b bin, obj, TestResults, packages, out') DO (rd /S /Q "%%C")
@if errorlevel 1 if not [%1] == [-i] pause