# .NET Framework 4.6.1 Developer Pack installer
$DotNetInstallerUrl = "https://go.microsoft.com/fwlink/?linkid=2099470"
$DotNetInstallerPath = "$env:USERPROFILE\Downloads\NDP461-DevPack-KB3105179-ENU.exe"

# Download the .NET Framework 4.6.1 Developer Pack
Write-Host "Downloading .NET Framework 4.6.1 Developer Pack installer from $DotNetInstallerUrl..."
Invoke-WebRequest -Uri $DotNetInstallerUrl -OutFile $DotNetInstallerPath

# Check if the download was successful
if (Test-Path $DotNetInstallerPath) {
    Write-Host ".NET Framework 4.6.1 Developer Pack installer downloaded successfully to $DotNetInstallerPath."
} else {
    Write-Host "Failed to download .NET Framework 4.6.1 Developer Pack installer."
    exit 1
}

# Install the .NET Framework 4.6.1 Developer Pack silently
Write-Host "Installing .NET Framework 4.6.1 Developer Pack silently..."
Start-Process -FilePath $DotNetInstallerPath -ArgumentList "/q" -Wait
Write-Host ".NET Framework 4.6.1 Developer Pack installation completed."

# Fiddler installer (check for the latest version on the Telerik website)
$FiddlerInstallerUrl = "https://telerik-fiddler.s3.amazonaws.com/fiddler/FiddlerSetup.exe"
$FiddlerInstallerPath = "$env:USERPROFILE\Downloads\FiddlerSetup.exe"

# Download Fiddler installer
Write-Host "Downloading Fiddler installer from $FiddlerInstallerUrl..."
Invoke-WebRequest -Uri $FiddlerInstallerUrl -OutFile $FiddlerInstallerPath

# Check if the download was successful
if (Test-Path $FiddlerInstallerPath) {
    Write-Host "Fiddler installer downloaded successfully to $FiddlerInstallerPath."
} else {
    Write-Host "Failed to download Fiddler installer."
    exit 1
}

# Install Fiddler silently
Write-Host "Installing Fiddler silently..."
Start-Process -FilePath $FiddlerInstallerPath -ArgumentList "/S" -Wait
Write-Host "Fiddler installation completed."