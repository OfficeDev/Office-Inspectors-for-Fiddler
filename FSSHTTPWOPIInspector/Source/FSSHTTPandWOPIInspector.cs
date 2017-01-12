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
            byte[] empty = new byte[0];
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.ByteProvider = new StaticByteProvider(empty);
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.ByteProvider.ApplyChanges();
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPISplitContainer.Panel2Collapsed = true;
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
                    string soapbody = SOAPRegex.Match(text).Value;

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
                            RequestEnvelope requestEnvelop = (RequestEnvelope)serializer.Deserialize(ms);
                            objectOut = requestEnvelop.Body;

                            if (requestEnvelop.Body.RequestCollection != null)
                            {
                                TryParseFSSHTTPBRequestMessage(requestEnvelop.Body.RequestCollection.Request, bytesFromHTTP);
                            }
                            break;
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
                                string res;
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
                                ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(res ?? ""));
                                XmlSerializer serializer = new XmlSerializer(typeof(ResponseEnvelopeBody));
                                ResponseEnvelopeBody body = (ResponseEnvelopeBody)serializer.Deserialize(ms);
                                objectOut = body;

                                // if SubResponseData has fsshttpb messages do parser.
                                if (body.ResponseCollection != null)
                                {
                                    TryParseFSSHTTPBResponseMessage(body.ResponseCollection.Response, bytesFromHTTP);
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
                                string binaryOctets = GetOctetsBinaryForXOP(bytesFromHTTP);
                                byte[] FSSHTTPBIncludeBytes = GetFSSHTTPBBytesForXOP(binaryOctets, bytesFromHTTP);

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

            foreach (Response res in Responses)
            {
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
                            string binaryOctets = GetOctetsBinaryForXOP(bytesFromHTTP);
                            byte[] FSSHTTPBIncludeBytes = GetFSSHTTPBBytesForXOP(binaryOctets, bytesFromHTTP);

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
            if (obj == null)
            {
                return;
            }
            else if (obj.GetType().Name == "String")
            {
                this.FSSHTTPandWOPIViewControl.BeginUpdate();
                this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Visible = true;
                this.FSSHTTPandWOPIControl.FSSHTTPandWOPIRichTextBox.Text = obj.ToString();
                this.FSSHTTPandWOPIViewControl.EndUpdate();
            }
            else
            {
                this.FSSHTTPandWOPIViewControl.BeginUpdate();
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
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Visible = true;
                    this.FSSHTTPandWOPIControl.FSSHTTPandWOPISplitContainer.Panel2Collapsed = false;
                }
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
        /// <returns>string of the octets binary, the string value is equal to the Hex value of binary</returns>
        public string GetOctetsBinaryForXOP(byte[] bytesFromHTTP)
        {
            Regex contentInBoundaryRegex = new Regex(@"(?<=2d2d)[\s\S]*(?=2d2d)"); // Accroding to [XOP10] "--" indicate the --MIME_boundary, and for each XOP package, there are 4 MIME_boundary, so this regex is used to get content between the first and the last MIME_boundary. 
            Regex secondBoundaryRegex = new Regex(@"2d2d[\s\S]*(?=2d2d)"); //This regex is used to get content behind the second boundary.
            Regex binaryOctetsRegex = new Regex(@"2d2d[\s\S]*0d0a0d0a"); // This regex is used to get the binary octets.
            string binaryOctets = string.Empty;
            string HexString = BytearrayToString(bytesFromHTTP);

            if (contentInBoundaryRegex.Match(HexString).Success)
            {
                string contentInBoundary = contentInBoundaryRegex.Match(HexString).Value;
                if (secondBoundaryRegex.Match(contentInBoundary).Success)
                {
                    string contentBehindsecondBoundary = secondBoundaryRegex.Match(contentInBoundary).Value;
                    if (binaryOctetsRegex.Match(contentBehindsecondBoundary).Success)
                    {
                        binaryOctets = binaryOctetsRegex.Replace(contentBehindsecondBoundary, string.Empty);
                    }
                    else
                    {
                        throw new Exception("Can't find FSSHTTPB resource.");
                    }
                }
                else
                {
                    throw new Exception("Can't find FSSHTTPB resource.");
                }
            }
            else
            {
                throw new Exception("Can't find FSSHTTPB resource.");
            }
            return binaryOctets;
        }

        /// <summary>
        /// Get the byte array of the octets binary in XOP package
        /// </summary>
        /// <param name="OctetsBinaryString">string of the octets binary</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer</param>
        /// <returns>the byte array of the octets binary in XOP package, also is the FSSHTTPB bytes</returns>
        public byte[] GetFSSHTTPBBytesForXOP(string OctetsBinaryString, byte[] bytesFromHTTP)
        {
            string HexString = BytearrayToString(bytesFromHTTP);
            byte[] FSSHTTPBBytes = new byte[OctetsBinaryString.Length / 2];

            for (int i = 0; i < OctetsBinaryString.Length / 2; i++)
            {
                FSSHTTPBBytes[i] = (byte)bytesFromHTTP[i + HexString.IndexOf(OctetsBinaryString) / 2];
            }
            return FSSHTTPBBytes;
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

        /// <summary>
        /// Enable the WOPIAndHTTPHexView
        /// </summary>
        /// <param name="FSSHTTPBBytes">The raw data for FSSHTTPB protocol </param>
        public void EnalbeWOPIAndHTTPHexView(byte[] FSSHTTPBBytes)
        {
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.Visible = true;
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPIHexBox.ByteProvider = new StaticByteProvider(FSSHTTPBBytes);
            this.FSSHTTPandWOPIControl.FSSHTTPandWOPISplitContainer.Panel2Collapsed = false;
        }
        #endregion
    }
}