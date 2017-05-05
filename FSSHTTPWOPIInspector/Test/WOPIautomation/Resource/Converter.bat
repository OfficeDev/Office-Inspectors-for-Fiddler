@echo on
set arg1=%1
set arg2=%2
set arg3=%3
set arg4=%4

if exist C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz del C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz
cmd /c call %arg1%\ExecAction.exe stop
timeout /t 5
cmd /c call %arg1%\ExecAction.exe dump
set target=C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz
call:checkFile
cmd /c call %arg1%\ExecAction.exe quit
if not exist C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz timeout /t 10
if not exist %arg3% mkdir %arg3%
if exist %arg3%\%arg4%.saz del %arg3%\%arg4%.saz
copy C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz %arg3%
set target=%arg3%\dump.saz
call:checkFile
ren %arg3%\dump.saz %arg4%.saz

set target=C:\Users\%arg2%\Documents\Fiddler2\Captures\dump.saz
call:checkFile

::--------------------------------------------------------
::-- Function section starts below here
::--------------------------------------------------------
:checkFile    - passing a variable by reference
if exist %target% goto foundIt
rem if not exist wait for 10 seconds
timeout /t 10 >null
goto:checkFile

:foundId
echo found %target%
goto:eof

