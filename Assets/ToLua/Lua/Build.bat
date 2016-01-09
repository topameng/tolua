cd /d %~dp0

mkdir jit
xcopy /Y /D ..\..\..\Luajit\jit jit

mkdir out
mkdir out\system
mkdir out\math
mkdir out\u3d
mkdir out\protobuf

for %%i in (*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (system\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (math\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (u3d\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (protobuf\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes

xcopy /Y /D /S out ..\..\StreamingAssets\Lua
rd /s/q jit
rd /s/q out

