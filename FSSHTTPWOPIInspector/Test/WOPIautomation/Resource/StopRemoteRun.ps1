param ([string]$username, [string]$password, [string]$RemoteCapturePath, [string]$LocalCapturePath, [string]$NewName)
	$Username = $username
	$Password = $password
	$LocalCapturePath = $LocalCapturePath
	$pass = ConvertTo-SecureString -AsPlainText $Password -Force
	$Cred = New-Object System.Management.Automation.PSCredential -ArgumentList $Username,$pass

	if (Test-Connection -ComputerName sut04 -Quiet)
	{
		Invoke-Command -ComputerName sut04 -Credential $Cred  -ScriptBlock {ping 1.2.3.4}
	}
        if (!(test-path $LocalCapturePath))
	{
		new-item -ItemType directory -force -path $LocalCapturePath
	}
	copy-item $RemoteCapturePath $LocalCapturePath -Force
        
	$LocalCapturePath = $LocalCapturePath + "\" + "aa.cap"
	$NewName = $NewName + ".cap"
        rename-item $LocalCapturePath $NewName

