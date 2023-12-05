set PathToBinaries=%1
set FiddlerPath=%2

xcopy "%PathToBinaries%*.*" %FiddlerPath)%\Inspectors\ /Q /Y
xcopy "%PathToBinaries%*.*" %FiddlerPath%\ImportExport\ /Q /Y
exit