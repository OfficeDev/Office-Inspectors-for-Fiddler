@echo on
::\Resource\Converter.bat "\"{0}\" \"{1}\" \"{2}\" \"{3}\"", fiddlerPath, userName, outputPath, newName
::{0} fiddlerPath:C:\Users\wopiuser\AppData\Local\Programs\Fiddler\
::{1} userName:wopiuser
::{2} outputPath:C:\v-fanfan\Update-FSSHTTP_FSSHTTPB-Inspectors-for-Fiddler\FSSHTTPWOPIInspector\Test\WOPIautomation\TestResults\
::{3} newName:CoautherWithConflict
::%arg1%=fiddlerPath
set arg1=%1
::%arg2%=userName
set arg2=%2
::%arg3%=outputPath
set arg3=%3
::%arg4%=newName
set arg4=%4

echo "if exist C:\Users\"%arg2%"\Documents\Fiddler2\Captures\dump.saz del C:\Users\"%arg2%"\Documents\Fiddler2\Captures\dump.saz"
if exist C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz del C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz
echo "cmd /c call "%arg1%"\ExecAction.exe stop"
cmd /c call %arg1%\ExecAction.exe stop
timeout /t 5
echo "cmd /c call "%arg1%"\ExecAction.exe dump"
cmd /c call %arg1%\ExecAction.exe dump
echo "set target=C:\Users\"%arg2%"\Documents\Fiddler2\Captures\dump.saz"
set target=C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz
echo "call:checkFile"
call:checkFile
echo "cmd /c call "%arg1%"\ExecAction.exe quit"
cmd /c call %arg1%\ExecAction.exe quit
echo "if not exist C:\Users\"%arg2%"\Documents\Fiddler2\Captures\dump.saz timeout /t 10"
if not exist C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz timeout /t 10
echo "if not exist "%arg3%" mkdir %arg3%"
if not exist %arg3% mkdir %arg3%
echo "if exist "%arg3%"\"%arg4%".saz del "%arg3%"\"%arg4%".saz"
if exist %arg3%\%arg4%.saz del %arg3%\%arg4%.saz
echo "copy C:\Users\"%arg2%"\Documents\Fiddler2\Captures\dump.saz "%arg3%
copy C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz %arg3%
echo "set target="%arg3%"\dump.saz"
set target=%arg3%\dump.saz
echo "call:checkFile"
call:checkFile
echo "ren "%arg3%"\dump.saz %arg4%.saz"
ren %arg3%\dump.saz %arg4%.saz
echo "set target=C:\Users\"%arg2%"\Documents\Fiddler2\Captures\dump.saz"

set target=C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz
echo "checkFile"
call:checkFile

::--------------------------------------------------------
::-- Function section starts below here
::--------------------------------------------------------
:checkFile    - passing a variable by reference
echo "if exist "%target%" goto foundId"
if exist %target% goto foundId
rem if not exist wait for 10 seconds
timeout /t 10 >null
echo "checkFile"
goto:checkFile

:foundId
echo found %target%
goto:eof

pause

