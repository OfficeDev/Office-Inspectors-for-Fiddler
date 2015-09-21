public static BindUIColumn("MS Protocol")
function CalcMethodCol(oS : Session) {
    var isMAPI: boolean = false;
    if (oS.RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http")) {
        isMAPI = true; 
    } else if (oS.ResponseHeaders.ExistsAndEquals("X-ResponseCode", "0") && oS.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http")) {
        isMAPI = true; 
    } else if (oS.ResponseHeaders.Exists("X-ResponseCode") && !oS.ResponseHeaders.ExistsAndEquals("X-ResponseCode", "0") && oS.ResponseHeaders.ExistsAndContains("Content-Type", "text/html")) {
        isMAPI = true;
    } else {
        return "";
    }
    var sRequestType = oS.RequestHeaders["X-RequestType"];
    switch (sRequestType) {
        case "Connect":
            return "MS-OXCMAPIHTTP";
        case "Execute":
            return "MS-OXCROPS";  
        case "Disconnect":
            return "MS-OXCMAPIHTTP";
        case "NotificationWait":
            return "MS-OXCMAPIHTTP";
        case "Bind":
            return "MS-OXNSPI";
        default:
            return "";
    }
}