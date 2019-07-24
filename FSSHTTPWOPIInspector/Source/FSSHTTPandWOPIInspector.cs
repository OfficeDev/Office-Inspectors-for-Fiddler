using System.IO;
using System.Windows.Forms;
using Fiddler;
using FSSHTTPandWOPIInspector.Parsers;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace FSSHTTPandWOPIInspector
{
    public abstract class FSSHTTPandWOPIInspector : Inspector2
    {
        /// <summary>
        /// Gets or sets the Tree View control where displayed the FSSHTTPandWOPI message.
        /// </summary>
        public TreeView FSSHTTPandWOPIViewControl { get; set; }

        /// <summary>
        /// Gets or sets the control collection where displayed the FSSHTTPandWOPI parsed message and corresponding hex data.
        /// </summary>
        public FSSHTTPandWOPIControl FSSHTTPandWOPIControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame has been changed
        /// </summary>
        public bool bDirty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame is read-only
        /// </summary>
        public bool bReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the Session object to pull frame data from Fiddler.
        /// </summary>
        internal Session session { get; set; }

        /// <summary>
        /// Gets or sets the raw bytes from the frame
        /// </summary>
        private byte[] rawBody { get; set; }

        /// <summary>
        /// Gets or sets the base HTTP headers assigned by the request or response
        /// </summary>
        public HTTPHeaders BaseHeaders { get; set; }

        /// <summary>
        /// Gets or sets the FSSHTTPBytes.
        /// </summary>
        public List<byte[]> FSSHTTPBBytes { get; set; }

        /// <summary>
        /// Gets whether the message is ONESTORE protocol message
        /// </summary>
        public static bool IsOneStore;

        /// <summary>
        /// Encrypted Object Group ID or Object ID List in ONESTORE protocol message
        /// </summary>
        public static List<ExtendedGUID> encryptedObjectGroupIDList = new List<ExtendedGUID>();

        /// <summary>
        /// Bool value indicate wether errorCode in FSSHTTP response is duplicate
        /// </summary>
        public bool isErrorCodeDuplicate;

        /// <summary>
        /// Boolean value to check whether next frame is editors table element
        /// </summary>
        public static bool isNextEditorTable;

        /// <summary>
        /// Gets the direction of the traffic
        /// </summary>
        public TrafficDirection Direction
        {
            get
            {
                if (this is IRequestInspector2)
                {
                    return TrafficDirection.In;
                }
                else
                {
                    return TrafficDirection.Out;
                }
            }
        }

        /// <summary>
        /// Gets whether the message is FSSHTTP protocol message.
        /// </summary>
        public bool IsFSSHTTP
        {
            get
            {
                if (this.session != null && this.session.RequestHeaders["SOAPAction"] == "\"http://schemas.microsoft.com/sharepoint/soap/ICellStorages/ExecuteCellStorageRequest\"") // /microsoft-server-activesync
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets whether the message is WOPI protocol message.
        /// </summary>
        public bool IsWOPI
        {
            get
            {
                if (this.session != null && (this.session.fullUrl.ToLower().Contains("/_vti_bin/wopi.ashx")) || this.session.fullUrl.ToLower().EndsWith("hosting/discovery")) // /microsoft-server-activesync
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Called by Fiddler to add the FSSHTTPandWOPI inspector tab
        /// </summary>
        /// <param name="o">The tab control for the inspector</param>
        public override void AddToTab(TabPage o)
        {
            o.Text = "FSSHTTPandWOPI";
            this.FSSHTTPandWOPIControl = new FSSHTTPandWOPIControl();
            o.Controls.Add(this.FSSHTTPandWOPIControl);
            this.FSSHTTPandWOPIControl.Size = o.Size;
            this.FSSHTTPandWOPIControl.Dock = DockStyle.Fill;
            this.FSSHTTPandWOPIViewControl = this.FSSHTTPandWOPIControl.FSSHTTPandWOPITreeView;
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.VScrollBarVisible = true;
            this.FSSHTTPandWOPIViewControl.AfterSelect -= TreeView_AfterSelect;
            this.FSSHTTPandWOPIViewControl.AfterSelect += TreeView_AfterSelect;
        }

        /// <summary>
        /// Method that returns a sorting hint
        /// </summary>
        /// <returns>An integer indicating where we should order ourselves</returns>
        public override int GetOrder()
        {
            return 0;
        }

        /// <summary>
        /// Method Fiddler calls to clear the display
        /// </summary>
        public void Clear()
        {
            this.FSSHTTPandWOPIViewControl.Nodes.Clear();
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Visible = false;
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Clear();
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Visible = false;
            this.FSSHTTPBBytes = new List<byte[]>();
            this.isErrorCodeDuplicate = false;
            byte[] empty = new byte[0];
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.ByteProvider = new StaticByteProvider(empty);
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.ByteProvider.ApplyChanges();
            //this.FSSHTTPandWOPIControl.FSSHTTPandWOPISplitContainer.Panel2Collapsed = true;
            //this.FSSHTTPandWOPIControl.FSSHTTPandWOPISplit.Visible = false;
        }

        /// <summary>
        /// Called by Fiddler to determine how confident this inspector is that it can
        /// decode the data.  This is only called when the user hits enter or double-
        /// clicks a session.  
        /// If we score the highest out of the other inspectors, Fiddler will open this
        /// inspector's tab and then call AssignSession.
        /// </summary>
        /// <param name="oS">the session object passed by Fiddler</param>
        /// <returns>Int between 0-100 with 100 being the most confident</returns>
        public override int ScoreForSession(Session oS)
        {
            if (null == this.session)
            {
                this.session = oS;
            }

            if (null == this.BaseHeaders)
            {
                if (this is IRequestInspector2)
                {
                    this.BaseHeaders = this.session.oRequest.headers;
                }
                else
                {
                    this.BaseHeaders = this.session.oResponse.headers;
                }
            }

            if (this.IsFSSHTTP || this.IsWOPI)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This is called every time this inspector is shown
        /// </summary>
        /// <param name="oS">Session object passed by Fiddler</param>
        public override void AssignSession(Session oS)
        {
            this.session = oS;
            base.AssignSession(oS);
        }

        /// <summary>
        /// Gets or sets the body byte[], called by Fiddler with session byte[]
        /// </summary>
        public byte[] body
        {
            get
            {
                return this.rawBody;
            }
            set
            {
                this.rawBody = value;
                this.UpdateView();
            }
        }

        /// <summary>
        /// Parse the HTTP payload to FSSHTTP and WOPI message.
        /// </summary>
        /// <param name="responseHeaders">The HTTP response header.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <returns>The object parsed result</returns>
        public object ParseHTTPPayloadForFSSHTTP(HTTPHeaders responseHeaders, byte[] bytesFromHTTP, TrafficDirection direction)
        {
            object objectOut = null;
            string soapbody = "";
            byte[] emptyByte = new byte[0];

            if (bytesFromHTTP == null || bytesFromHTTP.Length == 0)
            {
                return null;
            }

            try
            {
                if (direction == TrafficDirection.Out && responseHeaders.Exists("Transfer-Encoding") && responseHeaders["Transfer-Encoding"] == "chunked")
                {
                    bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                }

                Stream stream = new MemoryStream(bytesFromHTTP);
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();

                Regex SOAPRegex = new Regex(@"\<s:Envelop.*\<\/s:Envelope\>"); // extract envelop from http payload.
                if (SOAPRegex.Match(text).Success)
                {
                    XmlDocument doc = new XmlDocument();
                    soapbody = SOAPRegex.Match(text).Value;

                    if (direction == TrafficDirection.In)
                    {
                        Regex FSSHTTPRequestRegex = new Regex("xsi:type=\"\\w*\"\\s"); // remove xsi:type in xml message. this xsi:type is used for inherit in xmlSerializer. 
                        string FSSHTTPRequest = FSSHTTPRequestRegex.Replace(soapbody, string.Empty);
                        MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(FSSHTTPRequest ?? ""));
                        XmlSerializer serializer = new XmlSerializer(typeof(RequestEnvelope));
                        RequestEnvelope requestEnvelop = (RequestEnvelope)serializer.Deserialize(ms);
                        objectOut = requestEnvelop.Body;                        

                        // if SubRequestData has fsshttpb messages do parser.
                        if (requestEnvelop.Body.RequestCollection != null)
                        {
                            TryParseFSSHTTPBRequestMessage(requestEnvelop.Body.RequestCollection.Request, bytesFromHTTP);
                        }
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(soapbody ?? ""));
                        XmlSerializer serializer = new XmlSerializer(typeof(ResponseEnvelope));
                        ResponseEnvelope responseEnvelop = (ResponseEnvelope)serializer.Deserialize(ms);
                        objectOut = responseEnvelop.Body;

                        // if SubResponseData has fsshttpb messages do parser.
                        if (responseEnvelop.Body.ResponseCollection != null)
                        {
                            TryParseFSSHTTPBResponseMessage(responseEnvelop.Body.ResponseCollection.Response, bytesFromHTTP);
                        }
                    }
                }
                return objectOut;
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException.Message.Contains("ErrorCode") && e.InnerException.StackTrace.Contains("AttributeDuplCheck"))
                {
                    objectOut = soapbody;
                    isErrorCodeDuplicate = true;
                }
                else
                {
                    objectOut = e.ToString();
                }
                return objectOut;
            }
            catch (Exception ex)
            {
                objectOut = ex.ToString();
                return objectOut;
            }
        }

        /// <summary>
        /// Parse the HTTP payload to WOPI message.
        /// </summary>
        /// <param name="requestHeaders">The HTTP request header.</param>
        /// <param name="responseHeaders">The HTTP response header.</param>
        /// <param name="url">url for a HTTP message.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <returns>The object parsed result</returns>
        public object ParseHTTPPayloadForWOPI(HTTPHeaders requestHeaders, HTTPHeaders responseHeaders, string url, byte[] bytesFromHTTP, out string binaryStructureRopName, TrafficDirection direction)
        {
            object objectOut = null;
            string res = "";
            binaryStructureRopName = string.Empty;
            try
            {
                if (direction == TrafficDirection.Out && responseHeaders.Exists("Transfer-Encoding") && responseHeaders["Transfer-Encoding"] == "chunked")
                {
                    bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                }

                Stream stream = new MemoryStream(bytesFromHTTP);
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
                WOPIOperations operation = GetWOPIOperationName(requestHeaders, url);
                if (direction == TrafficDirection.In)
                {
                    switch (operation)
                    {
                        case WOPIOperations.PutRelativeFile:
                            objectOut = bytesFromHTTP;
                            binaryStructureRopName = "PutRelativeFile";
                            break;
                        case WOPIOperations.PutFile:
                            objectOut = bytesFromHTTP;
                            binaryStructureRopName = "PutFile";
                            break;
                        case WOPIOperations.ExecuteCellStorageRelativeRequest:
                        case WOPIOperations.ExecuteCellStorageRequest:
                            byte[] cellreq = bytesFromHTTP;
                            MemoryStream ms;
                            string req;
                            if (text.Contains("<s:Envelope"))
                            {
                                if (requestHeaders.Exists("Content-Encoding") && requestHeaders["Content-Encoding"] == "gzip")
                                {
                                    cellreq = Fiddler.Utilities.GzipExpand(cellreq);
                                    ms = new MemoryStream(cellreq);
                                }
                                else
                                {
                                    ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text ?? ""));
                                }
                                XmlSerializer serializer = new XmlSerializer(typeof(RequestEnvelope));
                                RequestEnvelope Envelop = (RequestEnvelope)serializer.Deserialize(ms);
                                objectOut = Envelop.Body;

                                if (Envelop.Body.RequestCollection != null)
                                {
                                    TryParseFSSHTTPBRequestMessage(Envelop.Body.RequestCollection.Request, bytesFromHTTP);
                                }
                                break;
                            }
                            else
                            {
                                if (requestHeaders.Exists("Content-Encoding") && requestHeaders["Content-Encoding"] == "gzip")
                                {
                                    cellreq = Fiddler.Utilities.GzipExpand(cellreq);
                                    string req_sub = System.Text.Encoding.UTF8.GetString(cellreq);
                                    req = string.Format("{0}{1}{2}", @"<Body>", req_sub, "</Body>");
                                }
                                else
                                {
                                    req = string.Format("{0}{1}{2}", @"<Body>", text, "</Body>");
                                }
                                ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(req ?? ""));
                                XmlSerializer serializer = new XmlSerializer(typeof(RequestEnvelopeBody));
                                RequestEnvelopeBody body = (RequestEnvelopeBody)serializer.Deserialize(ms);
                                objectOut = body;

                                if (body.RequestCollection != null)
                                {
                                    TryParseFSSHTTPBRequestMessage(body.RequestCollection.Request, bytesFromHTTP);
                                }
                                break;
                            }

                        case WOPIOperations.PutUserInfo:
                            objectOut = text;
                            break;
                        case WOPIOperations.Discovery:
                        case WOPIOperations.CheckFileInfo:
                        case WOPIOperations.Lock:
                        case WOPIOperations.RefreshLock:
                        case WOPIOperations.RevokeRestrictedLink:
                        case WOPIOperations.Unlock:
                        case WOPIOperations.UnlockAndRelock:
                        case WOPIOperations.GetLock:
                        case WOPIOperations.DeleteFile:
                        case WOPIOperations.ReadSecureStore:
                        case WOPIOperations.RenameFile:
                        case WOPIOperations.GetRestrictedLink:
                        case WOPIOperations.CheckFolderInfo:
                        case WOPIOperations.GetFile:
                        case WOPIOperations.EnumerateChildren:
                            objectOut = string.Format("{0} operation's request body is null", operation.ToString());
                            break;
                        default:
                            throw new Exception("The WOPI operations type is not right.");
                    }
                }
                else
                {
                    string status = this.session.ResponseHeaders.HTTPResponseStatus.Replace(" " + this.session.ResponseHeaders.StatusDescription, string.Empty);
                    if (Convert.ToUInt32(status) != 200)// the status is not success
                        return null;

                    ResponseBodyBase responseBody = new ResponseBodyBase();
                    switch (operation)
                    {
                        case WOPIOperations.Discovery:
                            MemoryStream discoveryms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text ?? ""));
                            XmlSerializer discoverySerializer = new XmlSerializer(typeof(wopidiscovery));
                            wopidiscovery discoveryres = (wopidiscovery)discoverySerializer.Deserialize(discoveryms);
                            objectOut = discoveryres;
                            break;
                        case WOPIOperations.CheckFileInfo:
                            objectOut = WOPISerilizer.JsonToObject<CheckFileInfo>(text);
                            break;
                        case WOPIOperations.CheckFolderInfo:
                            objectOut = WOPISerilizer.JsonToObject<CheckFolderInfo>(text);
                            break;
                        case WOPIOperations.PutRelativeFile:
                            objectOut = WOPISerilizer.JsonToObject<PutRelativeFile>(text);
                            break;
                        case WOPIOperations.ReadSecureStore:
                            objectOut = WOPISerilizer.JsonToObject<ReadSecureStore>(text);
                            break;
                        case WOPIOperations.EnumerateChildren:
                            objectOut = WOPISerilizer.JsonToObject<EnumerateChildren>(text);
                            break;
                        case WOPIOperations.RenameFile:
                            objectOut = WOPISerilizer.JsonToObject<RenameFile>(text);
                            break;
                        case WOPIOperations.ExecuteCellStorageRelativeRequest:
                        case WOPIOperations.ExecuteCellStorageRequest:
                            {
                                byte[] cellres = bytesFromHTTP;
                                MemoryStream ms;
                                if (responseHeaders.Exists("Content-Encoding") && responseHeaders["Content-Encoding"] == "gzip")
                                {
                                    cellres = Fiddler.Utilities.GzipExpand(cellres);
                                    string res_sub = System.Text.Encoding.UTF8.GetString(cellres);
                                    res = string.Format("{0}{1}{2}", @"<Body>", res_sub, "</Body>");
                                }
                                else
                                {
                                    res = string.Format("{0}{1}{2}", @"<Body>", text, "</Body>");
                                }

                                try
                                {
                                    ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(res ?? ""));
                                    XmlSerializer serializer = new XmlSerializer(typeof(ResponseEnvelopeBody));
                                    ResponseEnvelopeBody body = (ResponseEnvelopeBody)serializer.Deserialize(ms);
                                    objectOut = body;

                                    // if SubResponseData has fsshttpb messages do parser.
                                    if (body.ResponseCollection != null)
                                    {
                                        TryParseFSSHTTPBResponseMessage(body.ResponseCollection.Response, bytesFromHTTP);
                                    }
                                }
                                catch
                                {
                                    Regex SOAPRegex = new Regex(@"\<s:Envelop.*\<\/s:Envelope\>"); // extract envelop from http payload.
                                    if (SOAPRegex.Match(res).Success)
                                    {
                                        XmlDocument doc = new XmlDocument();
                                        string soapbody = SOAPRegex.Match(res).Value;

                                        MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(soapbody ?? ""));
                                        XmlSerializer serializer = new XmlSerializer(typeof(ResponseEnvelope));
                                        ResponseEnvelope responseEnvelop = (ResponseEnvelope)serializer.Deserialize(memoryStream);
                                        objectOut = responseEnvelop.Body;

                                        // if SubResponseData has fsshttpb messages do parser.
                                        if (responseEnvelop.Body.ResponseCollection != null)
                                        {
                                            TryParseFSSHTTPBResponseMessage(responseEnvelop.Body.ResponseCollection.Response, bytesFromHTTP);
                                        }
                                    }
                                }
                                break;
                            }
                        case WOPIOperations.GetFile:
                            objectOut = bytesFromHTTP;
                            binaryStructureRopName = "GetFile";
                            break;
                        case WOPIOperations.DeleteFile:
                        case WOPIOperations.Lock:
                        case WOPIOperations.GetRestrictedLink:
                        case WOPIOperations.PutFile:
                        case WOPIOperations.RefreshLock:
                        case WOPIOperations.RevokeRestrictedLink:
                        case WOPIOperations.Unlock:
                        case WOPIOperations.UnlockAndRelock:
                        case WOPIOperations.GetLock:
                        case WOPIOperations.PutUserInfo:
                            objectOut = string.Format("{0} operation's response body is null", operation.ToString());
                            break;
                        default:
                            throw new Exception("The WOPI operations type is not right.");
                    }
                }
                return objectOut;
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException.Message.Contains("ErrorCode") && e.InnerException.StackTrace.Contains("AttributeDuplCheck"))
                {
                    objectOut = res;
                    isErrorCodeDuplicate = true;
                }
                else
                {
                    objectOut = e.ToString();
                }
                return objectOut;
            }
            catch (Exception ex)
            {
                objectOut = ex.ToString();
                return objectOut;
            }
        }

        /// <summary>
        /// Parse the HTTP payload to FSSHTTPB Request message.
        /// </summary>
        /// <param name="Requests">Array of Request that is part of a cell storage service request.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        public void TryParseFSSHTTPBRequestMessage(Request[] Requests, byte[] bytesFromHTTP)
        {
            if (Requests == null)
                return;

            byte[][] includeTexts = GetOctetsBinaryForXOP(bytesFromHTTP, true).ToArray();
            int index = 0;

            foreach (Request req in Requests)
            {
                if (req.SubRequest != null && req.SubRequest.Length > 0)
                {
                    foreach (SubRequestElementGenericType subreq in req.SubRequest)
                    {
                        if (subreq.SubRequestData != null)
                        {
                            if (subreq.SubRequestData.Text != null && subreq.SubRequestData.Text.Length > 0)
                            {
                                string textValue = subreq.SubRequestData.Text[0];
                                byte[] FSSHTTPBTextBytes = Convert.FromBase64String(textValue);

                                if (!IsFSSHTTPBStart(FSSHTTPBTextBytes))
                                    return;

                                FsshttpbRequest Fsshttpbreq = (FsshttpbRequest)ParseFSSHTTPBBytes(FSSHTTPBTextBytes, TrafficDirection.In);
                                subreq.SubRequestData.TextObject = Fsshttpbreq;
                                FSSHTTPBBytes.Add(FSSHTTPBTextBytes);
                            }

                            if (subreq.SubRequestData.Include != null)
                            {
                                byte[] FSSHTTPBIncludeBytes = includeTexts[index++];
                                FsshttpbRequest Fsshttpbreq = (FsshttpbRequest)ParseFSSHTTPBBytes(FSSHTTPBIncludeBytes, TrafficDirection.In);
                                subreq.SubRequestData.IncludeObject = Fsshttpbreq;
                                FSSHTTPBBytes.Add(FSSHTTPBIncludeBytes);
                            }                        
						}
                    }
                }
            }
        }

        /// <summary>
        /// Parse the HTTP payload to FSSHTTPB Response message.
        /// </summary>
        /// <param name="Responses">Array of Response element that is part of a cell storage service response.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        public void TryParseFSSHTTPBResponseMessage(Response[] Responses, byte[] bytesFromHTTP)
        {
            if (Responses == null)
                return;

            byte[][] includeTexts = GetOctetsBinaryForXOP(bytesFromHTTP, false).ToArray();
            int index = 0;

            foreach (Response res in Responses)
            {
                // If response is for ONESTORE,set FSSHTTPandWOPIInspector.IsOneStore ture.
                if (res.Url.EndsWith(".one") || res.Url.EndsWith(".onetoc2"))
                {
                    FSSHTTPandWOPIInspector.IsOneStore = true;
                }

                if (res.SubResponse != null && res.SubResponse.Length > 0)
                {
                    foreach (SubResponseElementGenericType subres in res.SubResponse)
                    {
                        if (subres.SubResponseData == null)
                            continue;

                        if (subres.SubResponseData.Text != null && subres.SubResponseData.Text.Length > 0)
                        {
                            string textValue = subres.SubResponseData.Text[0];
                            byte[] FSSHTTPBTextBytes = Convert.FromBase64String(textValue);
                            FsshttpbResponse Fsshttpbres = (FsshttpbResponse)ParseFSSHTTPBBytes(FSSHTTPBTextBytes, TrafficDirection.Out);
                            subres.SubResponseData.TextObject = Fsshttpbres;
                            FSSHTTPBBytes.Add(FSSHTTPBTextBytes);
                        }

                        if (subres.SubResponseData.Include != null)
                        {
                            byte[] FSSHTTPBIncludeBytes = includeTexts[index++];
                            FsshttpbResponse Fsshttpbres = (FsshttpbResponse)ParseFSSHTTPBBytes(FSSHTTPBIncludeBytes, TrafficDirection.Out);
                            subres.SubResponseData.IncludeObject = Fsshttpbres;
                            FSSHTTPBBytes.Add(FSSHTTPBIncludeBytes);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parse the FSSHTTPB Bytes.
        /// </summary>
        /// <param name="FSSHTTPBbytes">The raw date contains FSSHTTPB message.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <returns>The object parsed result</returns>
        public object ParseFSSHTTPBBytes(byte[] FSSHTTPBbytes, TrafficDirection direction)
        {
            object objectOut = null;
            byte[] emptyByte = new byte[0];
            if (FSSHTTPBbytes == null || FSSHTTPBbytes.Length == 0)
            {
                return null;
            }

            try
            {
                if (direction == TrafficDirection.In)
                {
                    FsshttpbRequest FsshttpbReq = new FsshttpbRequest();
                    MemoryStream s = new MemoryStream(FSSHTTPBbytes);
                    FsshttpbReq.Parse(s);
                    objectOut = FsshttpbReq;
                }
                else
                {
                    FsshttpbResponse FsshttpbRes = new FsshttpbResponse();
                    MemoryStream s = new MemoryStream(FSSHTTPBbytes);
                    FsshttpbRes.Parse(s);
                    objectOut = FsshttpbRes;
                }
                
                return objectOut;

            }
            catch (Exception ex)
            {
                objectOut = ex.ToString();
                return objectOut;
            }
        }

        /// <summary>
        /// Display the object in Tree View
        /// </summary>
        /// <param name="obj">The object to display</param>
        /// <param name="RopNameforBinaryStructure">The string value used to as the name of tree node for object which is bianry value</param>
        public void DisplayObject(object obj, string RopNameforBinaryStructure)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }
                else if (obj.GetType().Name == "String")
                {
                    this.FSSHTTPandWOPIViewControl.BeginUpdate();
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Visible = true;

                    if (isErrorCodeDuplicate)
                    {
                        string[] xmlFragments = obj.ToString().Split(new[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
                        int startNum = 0;
                        int endNum = 0;
                        for (int i = 0; i < xmlFragments.Length; i++)
                        {
                            xmlFragments[i] += ">";
                            if (xmlFragments[i].Contains("</"))
                            {
                                endNum += 1;
                            }
                            int n = startNum - endNum;
                            while (n > 0)
                            {
                                this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.AppendText("    ");
                                n--;
                            }
                            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.AppendText(xmlFragments[i]);
                            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.AppendText("\n");
                            if (!xmlFragments[i].Contains("</"))
                            {
                                startNum += 1;
                            }

                            if (xmlFragments[i].Contains("/>"))
                            {
                                endNum += 1;
                            }
                        }
                    }
                    else
                    {
                        this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Text = obj.ToString();
                    }

                    this.FSSHTTPandWOPIViewControl.EndUpdate();
                }
                else
                {
                    this.FSSHTTPandWOPIViewControl.BeginUpdate();
                    TreeAndHexViewAdjust(false);
                    TreeNode topNode = BaseStructure.ObjectToTreeNode(obj, RopNameforBinaryStructure);
                    topNode = BaseStructure.RemoveAnySpecifiedTreeNode(topNode);
                    int index = 1;
                    topNode = BaseStructure.AddserialNumForFSSHTTPBTreeNode(topNode, ref index);
                    this.FSSHTTPandWOPIViewControl.Nodes.Add(topNode);
                    this.FSSHTTPandWOPIViewControl.Nodes[this.FSSHTTPandWOPIViewControl.Nodes.Count - 1].EnsureVisible();
                    topNode.ExpandAll();
                    this.FSSHTTPandWOPIViewControl.EndUpdate();
                }
            }
            catch (Exception e)
            {
                this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Visible = true;
                this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Text = e.Message;
                this.FSSHTTPandWOPIViewControl.EndUpdate();
            }
        }

        /// <summary>
        /// Update the view with parsed and diagnosed data
        /// </summary>
        private void UpdateView()
        {
            this.Clear();
            object parserResult;

            if (this.IsFSSHTTP)
            {
                if (this.Direction == TrafficDirection.In)
                {
                    if (this.session.requestBodyBytes.Length == 0)
                        return;
                    parserResult = this.ParseHTTPPayloadForFSSHTTP(this.session.ResponseHeaders, this.session.requestBodyBytes, TrafficDirection.In);
                }
                else
                {
                    if (this.session.responseBodyBytes.Length == 0)
                        return;
                    parserResult = this.ParseHTTPPayloadForFSSHTTP(this.session.ResponseHeaders, this.session.responseBodyBytes, TrafficDirection.Out);
                }
                DisplayObject(parserResult, string.Empty);
            }
            else if (this.IsWOPI)
            {
                string ropNameforBinaryStructure;
                string Url = this.session.fullUrl.ToLower();
                Regex UrlAbs = new Regex(@"[\s\S]*(?=\?)");
                if (UrlAbs.Match(Url).Success)
                {
                    Url = UrlAbs.Match(Url).Value;
                }
                if (this.Direction == TrafficDirection.In)
                {
                    parserResult = this.ParseHTTPPayloadForWOPI(this.session.RequestHeaders, this.session.ResponseHeaders, Url, this.session.requestBodyBytes, out ropNameforBinaryStructure, TrafficDirection.In);
                }
                else
                {
                    parserResult = this.ParseHTTPPayloadForWOPI(this.session.RequestHeaders, this.session.ResponseHeaders, Url, this.session.responseBodyBytes, out ropNameforBinaryStructure, TrafficDirection.Out);
                }
                DisplayObject(parserResult, ropNameforBinaryStructure);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Enum for traffic direction
        /// </summary>
        public enum TrafficDirection
        {
            In,
            Out
        }

        /// <summary>
        /// Get the WOPI operation
        /// </summary>
        /// <param name="headers">Http headers</param>
        /// <param name="url">url for a session</param>
        /// <returns>The operation name</returns>
        public WOPIOperations GetWOPIOperationName(HTTPHeaders headers, string url)
        {
            if (url.EndsWith("hosting/discovery"))
                return WOPIOperations.Discovery;

            if (headers.Exists("X-WOPI-Override"))
            {
                switch (headers["X-WOPI-Override"] as string)
                {
                    case "PUT_RELATIVE":
                        return WOPIOperations.PutRelativeFile;
                    case "UNLOCK":
                        return WOPIOperations.Unlock;
                    case "REFRESH_LOCK":
                        return WOPIOperations.RefreshLock;
                    case "DELETE":
                        return WOPIOperations.DeleteFile;
                    case "READ_SECURE_STORE":
                        return WOPIOperations.ReadSecureStore;
                    case "GET_RESTRICTED_LINK":
                        return WOPIOperations.GetRestrictedLink;
                    case "REVOKE_RESTRICTED_LINK":
                        return WOPIOperations.RevokeRestrictedLink;
                    case "PUT":
                        return WOPIOperations.PutFile;
                    case "RENAME_FILE":
                        return WOPIOperations.RenameFile;
                    case "LOCK":
                        if (headers.Exists("X-WOPI-OldLock"))
                        {
                            return WOPIOperations.UnlockAndRelock;
                        }
                        else
                        {
                            return WOPIOperations.Lock;
                        }
                    case "COBALT":
                        if (headers.Exists("X-WOPI-RelativeTarget"))
                        {
                            return WOPIOperations.ExecuteCellStorageRelativeRequest;
                        }
                        else
                        {
                            return WOPIOperations.ExecuteCellStorageRequest;
                        }
                    default:
                        return WOPIOperations.Unknown;
                }
            }

            if (url.EndsWith("/contents"))
            {
                return WOPIOperations.GetFile;
            }

            if (url.EndsWith("/children"))
            {
                return WOPIOperations.EnumerateChildren;
            }

            if (url.Contains("/files/"))
            {
                return WOPIOperations.CheckFileInfo;
            }

            if (url.Contains("/folders/"))
            {
                return WOPIOperations.CheckFolderInfo;
            }

            return WOPIOperations.Unknown;
        }

        /// <summary>
        /// Represents the method, which is used to handle the AfterSelect event of a TreeView.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">A System.Windows.Forms.TreeViewEventArgs that contains the event data.</param>
        void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                if (this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Visible == true)
                {
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Select(0, 0);
                }
                return;
            }
            else
            {
                if (((BaseStructure.Position)e.Node.Tag).Num != 0)
                {
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.ByteProvider = new StaticByteProvider(FSSHTTPBBytes[((BaseStructure.Position)e.Node.Tag).Num - 1]); ;
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Select(((BaseStructure.Position)e.Node.Tag).StartIndex, ((BaseStructure.Position)e.Node.Tag).Offset);

                    TreeAndHexViewAdjust(true, this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Visible);
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Visible = true;
                }
            }
        }

        /// <summary>
        /// The method is used to adjust two views size
        /// </summary>
        /// <param name="showHexView">A bool indicates whether hexview is appear.</param>
        void TreeAndHexViewAdjust(bool showHexView, bool Visible = false)
        {
            if (showHexView)
            {
                if (!Visible)
                {
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Size = new System.Drawing.Size(350, 475);
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPITreeView.Size = new System.Drawing.Size(622, 475);
                }
            }
            else
            {
                this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Size = new System.Drawing.Size(2, 475);
                this.FSSHTTPandWOPIControl.FSSHTTPandWOPITreeView.Size = new System.Drawing.Size(1700, 475);
            }
        }

        #region Help methods
        /// <summary>
        /// Concert byte array to hex string
        /// </summary>
        /// <param name="ba">The byte array used to convert</param>
        /// <returns>Hex string value</returns>
        public string BytearrayToString(byte[] ba)
        {
            StringBuilder st = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                st.AppendFormat("{0:x2}", b);
            }
            return st.ToString();
        }

        /// <summary>
        /// Get the string of octets binary in XOP package
        /// </summary>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer</param>
        /// <param name="IncludeTextLength">Out the length of octets binary</param>
        /// <returns>A int value indicate the position of the octets binary in the bytesFromHTTP</returns>
        public List<byte[]> GetOctetsBinaryForXOP(byte[] bytesFromHTTP, bool isRequest)
        {
            string HexString = BytearrayToString(bytesFromHTTP);
            Regex MIMEBoundaryRegex;
            Regex octetsBinaryRegex;
            bool bounaryContainUrn = true;
            if (isRequest)
            {
                MIMEBoundaryRegex = new Regex(@"2d2d75726e3a75756964");// MIME bounary is --urn:uuid
                octetsBinaryRegex = new Regex(@"2d2d75726e3a75756964([\s\S]*?)(?=2d2d75726e3a75756964)");// This regex is used to get substring between two --urn:uuid.
                if (MIMEBoundaryRegex.Matches(HexString).Count == 0)
                {
                    MIMEBoundaryRegex = new Regex(@"2d2d75756964");// MIME bounary is --uuid
                    octetsBinaryRegex = new Regex(@"2d2d75756964([\s\S]*?)(?=2d2d75756964)");// This regex is used to get substring between two --uuid.
                    bounaryContainUrn = false;
                }
            }
            else
            {
                MIMEBoundaryRegex = new Regex(@"2d2d75756964");// MIME bounary is --uuid
                octetsBinaryRegex = new Regex(@"2d2d75756964([\s\S]*?)(?=2d2d75756964)");// This regex is used to get substring between two --uuid.
            }

            Regex IncludeRegex = new Regex(@"2d2d[\s\S]*0d0a0d0a"); // This regex is used to get the Include text(octets Binary minus include Header)
            List<byte[]> IncludeTexts = new List<byte[]>();
            if (MIMEBoundaryRegex.Matches(HexString).Count >= 3)
            {
                // remove first MIME bounary from HexString
                string firstMIMEBoundary = MIMEBoundaryRegex.Match(HexString).Value;
                HexString = MIMEBoundaryRegex.Replace(HexString, string.Empty, 1);
                int HistoryPosition = 0;
                if (isRequest && bounaryContainUrn)
                {
                    HistoryPosition = 10; // 10 is the length of first --urn:uuid in bytesFromHTTP, it has been removed in HexString
                }
                else
                {
                    HistoryPosition = 6; // 6 is the length of first --uuid in bytesFromHTTP, it has been removed in HexString
                }

                int MIMEBoundaryEaroIndex = 0;
                int LastIncludeLength = 0;
                // Get all include text binary
                while (octetsBinaryRegex.Matches(HexString).Count > 0)
                {
                    string octetsBinary = octetsBinaryRegex.Match(HexString).Value;
                    string includeHeader = IncludeRegex.Match(octetsBinary).Value;
                    string includeText = IncludeRegex.Replace(octetsBinary, string.Empty, 1);
                    int ThisIncludePosition = HexString.IndexOf(includeText) / 2;// One char in byte array as a string 
                    int ThisIncludeLength = includeText.Length / 2 - 2; // (-2) because behinde every include text there is a 0D0A

                    byte[] includeByte = new byte[ThisIncludeLength];
                    if (MIMEBoundaryEaroIndex == 0)
                    {
                        HistoryPosition += ThisIncludePosition;

                    }
                    else
                    {
                        HistoryPosition += (LastIncludeLength + includeHeader.Length / 2 + 2);// (+2) because behinde every include text there is a 0D0A
                    }
                    LastIncludeLength = ThisIncludeLength;
                    Array.Copy(bytesFromHTTP, HistoryPosition, includeByte, 0, ThisIncludeLength);
                    IncludeTexts.Add(includeByte);
                    HexString = octetsBinaryRegex.Replace(HexString, string.Empty, 1);
                    MIMEBoundaryEaroIndex++;
                }
                return IncludeTexts;
            }
            return IncludeTexts;
        }

        /// <summary>
        /// Check if the start point is FSSHTTPB bits
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>bool value indicate if the payload is fsshttpb or not</returns>
        public bool IsFSSHTTPBStart(byte[] payload)
        {
            if (payload == null || payload.Length < 4)
                return false;

            if ((payload[0] == 0x0C && payload[1] == 0x00 && payload[2] == 0x0B && payload[3] == 0x00)
                || (payload[0] == 0x0D && payload[1] == 0x00 && payload[2] == 0x0B && payload[3] == 0x00)
                || (payload[0] == 0x0E && payload[1] == 0x00 && payload[2] == 0x0B && payload[3] == 0x00))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}