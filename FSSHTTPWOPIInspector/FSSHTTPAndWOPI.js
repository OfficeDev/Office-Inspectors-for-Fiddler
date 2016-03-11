public static BindUIColumn("MS Protocol")
    function CalcMethodCol(oS : Session) {
        var isWOPI: boolean = false;
        var isFSSHTTP: boolean = false;
        if (oS != null && (oS.fullUrl.ToLower().Contains("/_vti_bin/wopi.ashx")) || oS.fullUrl.ToLower().EndsWith("hosting/discovery")) {
            isWOPI = true; 
        } else if (oS != null && oS.RequestHeaders["SOAPAction"] == "\"http://schemas.microsoft.com/sharepoint/soap/ICellStorages/ExecuteCellStorageRequest\"") {
            isFSSHTTP = true; 
        } else {
            return "";
        }
        var op: String = null;
         op = GetWOPIOperationName(oS);
        
        if(isWOPI)
        { return "MS-WOPI" +":" + op;
            }
        else if(isFSSHTTP)
        {
            return "MS-FSSHTTP";
        }
    }
 
        
    static function GetWOPIOperationName(sParams: Session):String {
        var url:String  =  sParams.fullUrl.ToLower();
            
        if (url.EndsWith("hosting/discovery"))
        {
            return "Discovery";
        }
        url = url.match(/[\s\S]*(?=\?)/);

        if (sParams.RequestHeaders.Exists("X-WOPI-Override"))
        {
            switch (sParams.RequestHeaders["X-WOPI-Override"])
                {
            case "PUT_RELATIVE":
            return "PutRelativeFile";
            case "UNLOCK":
            return "Unlock";
            case "REFRESH_LOCK":
            return "RefreshLock";
            case "DELETE":
            return"DeleteFile";
            case "READ_SECURE_STORE":
            return "ReadSecureStore"
            case "GET_RESTRICTED_LINK":
            return "GetRestrictedLink";
            case "REVOKE_RESTRICTED_LINK":
            return "RevokeRestrictedLink";
            case "PUT":
            return "PutFile";
            case "LOCK":
            if (sParams.RequestHeaders.Exists("X-WOPI-OldLock"))
            {
                return "UnlockAndRelock";
            }
            else
            {
                return "Lock";
            }
            case "COBALT":
            if (sParams.RequestHeaders.Exists("X-WOPI-RelativeTarget"))
            {
                return "ExecuteCellStorageRelative";
            }
            else
            {
                return "ExecuteCellStorage";
            }
            default:
            return "";
        }
    }

    if (url.EndsWith("/contents"))
    {
    return "GetFile";
    }

    if (url.EndsWith("/children"))
    {
    return "EnumerateChildren";
    }

    if (url.Contains("/files/"))
    {
    return "CheckFileInfo";
    }

    if (url.Contains("/folders/"))
    {
    return "CheckFolderInfo";
    }

    return "";
    }