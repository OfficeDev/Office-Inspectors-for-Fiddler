param (
   [string]$OutlookVersion,
   [string]$IsEnable
)

$path = ""

if ($OutlookVersion -eq "Outlook2010")
{
	$path = "HKCU:\SOFTWARE\Policies\Microsoft\Office\14.0\Outlook\Cached Mode"
}

if ($OutlookVersion -eq "Outlook2013")
{
	$path = "HKCU:\SOFTWARE\Policies\Microsoft\Office\15.0\Outlook\Cached Mode"
}

if ($OutlookVersion -eq "Outlook2016")
{
	$path = "HKCU:\SOFTWARE\Policies\Microsoft\Office\16.0\Outlook\Cached Mode"
}

if (!(Test-RegistryValue -path $path -name "enable") -eq "true")
{
	New-Item -Path $path -Force
}

if ($IsEnable -eq "true")
{
	New-ItemProperty -Path $path -Name "Enable" -Value 1 -PropertyType "DWORD" -Force
}
else
{
	New-ItemProperty -Path $path -Name "Enable" -Value 0 -PropertyType "DWORD" -Force
}


Function Test-RegistryValue {
    param(
        [Alias("PSPath")]
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [String]$Path
        ,
        [Parameter(Position = 1, Mandatory = $true)]
        [String]$Name
        ,
        [Switch]$PassThru
    ) 

    process {
        if (Test-Path $Path) {
            $Key = Get-Item -LiteralPath $Path
            if ($Key.GetValue($Name, $null) -ne $null) {
                if ($PassThru) {
                    Get-ItemProperty $Path $Name
                } else {
                    $true
                }
            } else {
                $false
            }
        } else {
            $false
        }
    }
}