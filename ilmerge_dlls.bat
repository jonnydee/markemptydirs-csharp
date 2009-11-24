@echo off

set ProjectDir=%1
set BinPath=%ProjectDir%bin\Debug\
set OutputPath=%2

echo Building '%OutputPath%MarkEmptyDirs.exe'...
ilmerge /t:exe /out:%OutputPath%MarkEmptyDirs.exe %BinPath%MarkEmptyDirs.exe %BinPath%DR.Util.dll
echo ...FINISHED!

