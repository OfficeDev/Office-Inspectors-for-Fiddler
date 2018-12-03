param ([string]$username, [string]$password)
	$Username = $username
	$Password = $password
	$pass = ConvertTo-SecureString -AsPlainText $Password -Force
	$Cred = New-Object System.Management.Automation.PSCredential -ArgumentList $Username,$pass

	if (Test-Connection -ComputerName sut03 -Quiet)
	{
		#Invoke-Command -ComputerName sut03 -Credential $Cred -FilePath "..\..\..\Resource\myScript.ps1" 
        Invoke-Command -ComputerName sut03 -Credential $Cred -FilePath $PSScriptRoot"\myScript.ps1" 
	}

