cd /d %~dp0

mkdir jit
xcopy /Y /D ..\..\..\Luajit\jit jit

mkdir out
mkdir out\System
mkdir out\System\Reflection
mkdir out\socket
mkdir out\UnityEngine
mkdir out\protobuf
mkdir out\misc
mkdir out\cjson

for %%i in (*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (System\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (System\Reflection\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (socket\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (UnityEngine\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (protobuf\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (misc\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes
for %%i in (cjson\*.lua) do ..\..\..\Luajit\luajit.exe -b -g %%i out\%%i.bytes

xcopy /Y /D /S out ..\..\StreamingAssets\Lua
rd /s/q jit
rd /s/q out

