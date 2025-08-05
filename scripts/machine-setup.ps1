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

# Fiddler installer (try multiple sources)
$FiddlerInstallerUrls = @(
    "https://api.getfiddler.com/fc/latest",
    "https://downloads.getfiddler.com/fiddler-classic/FiddlerSetup.5.0.20253.3311-latest.exe",
    "https://telerik-fiddler.s3.amazonaws.com/fiddler/FiddlerSetup.exe"
)
$FiddlerInstallerPath = "$env:USERPROFILE\Downloads\FiddlerSetup.exe"
$FiddlerDownloadSuccess = $false

# Try downloading Fiddler installer from multiple sources
foreach ($url in $FiddlerInstallerUrls) {
    try {
        Write-Host "Attempting to download Fiddler installer from $url..."
        
        # Download to a temporary path first, then rename to consistent filename
        $tempPath = "$env:USERPROFILE\Downloads\FiddlerSetup_temp.exe"
        Invoke-WebRequest -Uri $url -OutFile $tempPath -ErrorAction Stop
        
        if (Test-Path $tempPath) {
            # Rename to consistent filename
            if (Test-Path $FiddlerInstallerPath) {
                Remove-Item $FiddlerInstallerPath -Force
            }
            Move-Item $tempPath $FiddlerInstallerPath
            
            Write-Host "Fiddler installer downloaded successfully from $url and renamed to $FiddlerInstallerPath."
            $FiddlerDownloadSuccess = $true
            break
        }
    }
    catch {
        Write-Warning "Failed to download from $url`: $($_.Exception.Message)"
        # Clean up any partial downloads
        if (Test-Path $tempPath) {
            Remove-Item $tempPath -Force -ErrorAction SilentlyContinue
        }
        if (Test-Path $FiddlerInstallerPath) {
            Remove-Item $FiddlerInstallerPath -Force -ErrorAction SilentlyContinue
        }
    }
}

if (!$FiddlerDownloadSuccess) {
    Write-Host "Failed to download Fiddler installer from any source."
    exit 1
}

# Install Fiddler silently
Write-Host "Installing Fiddler silently..."
Start-Process -FilePath $FiddlerInstallerPath -ArgumentList "/S" -Wait
Write-Host "Fiddler installation completed."

Write-Host "✅ All tools installed successfully"