param ([string]$username, [string]$password)
	$Username = $username
	$Password = $password
	$pass = ConvertTo-SecureString -AsPlainText $Password -Force
	$Cred = New-Object System.Management.Automation.PSCredential -ArgumentList $Username,$pass

	if (Test-Connection -ComputerName sut02 -Quiet)
	{
		#Invoke-Command -ComputerName sut02 -Credential $Cred -FilePath "..\..\..\Resource\myScript.ps1" 
        Invoke-Command -ComputerName sut02 -Credential $Cred -FilePath "C:\myScript.ps1" 
	}

