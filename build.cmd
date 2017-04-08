@echo off
cls

cd data

..\.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

cd ..

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

IF NOT EXIST build.fsx (
  .paket\paket.exe update
)
packages\FAKE\tools\FAKE.exe build.fsx %*
