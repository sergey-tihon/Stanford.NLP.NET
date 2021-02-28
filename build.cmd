@echo off
cls

dotnet tool restore

cd data
dotnet paket restore
cd ..

dotnet paket restore
dotnet fake run build.fsx %*
