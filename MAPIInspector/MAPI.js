public static BindUIColumn("MAPI")
function CalcMethodCol(oS : Session){
    var isMAPI: boolean = false;
    if (oS.RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http"))
    {
        isMAPI = true; 
    }
    else if (oS.ResponseHeaders.ExistsAndEquals("X-ResponseCode", "0") && oS.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http"))
    {
        isMAPI = true; 
    }
    else if (oS.ResponseHeaders.Exists("X-ResponseCode") && !oS.ResponseHeaders.ExistsAndEquals("X-ResponseCode", "0") && oS.ResponseHeaders.ExistsAndContains("Content-Type", "text/html"))
    {
        isMAPI = true;
    }
    else
    {
        isMAPI = false; 
    }

    if(isMAPI && oS.RequestHeaders.ExistsAndContains("X-RequestType", "Execute"))
    {
        return "MS-OXCROPS";  
    }
    else if (isMAPI)
    {
        return "MS-OXCMAPIHTTP";
    }
    else 
    {
        return "";
    }
}