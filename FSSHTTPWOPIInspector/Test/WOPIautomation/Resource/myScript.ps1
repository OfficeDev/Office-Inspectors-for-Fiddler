		$path = "c:\Wenbo"
		$captureFileName = "aa.cap"
		$fullPath = $path + "\" + $captureFileName;
		if (!(test-path $path))
		{
			new-item -ItemType directory -force -path $path
		}
		nmcap /network * /capture tcp /File $fullPath /stopwhen /frame "(ipv4.address == ipconfig.localipv4address) AND (Ipv4.DestinationAddress == 1.2.3.4)" 
