@echo off
echo "Cleaning soltion start"
echo "Get-ChildItem .\ -include bin,obj,tmp,publish -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }"
start "Clean_Solution" /d "%cd%" powershell -NoExit -Command "Get-ChildItem .\ -include bin,obj,tmp,publish -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }"
pause