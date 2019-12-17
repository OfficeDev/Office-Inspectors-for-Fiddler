namespace MapiInspector
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Be.Windows.Forms;
    using Fiddler;
    using global::MAPIInspector.Parsers;

    /// <summary>
    /// MAPIInspector Class
    /// </summary>
    public abstract class MAPIInspector : Inspector2
    {
        /// <summary>
        /// Gets or sets the parsing session in fiddler.
        /// </summary>
        public static Session ParsingSession;

        /// <summary>
        /// Record all sessions in Fiddler.
        /// </summary>
        public static Session[] AllSessions;

        /// <summary>
        /// The targetHandle is used to record the session id and its object handle before a loop parsing for context session
        /// </summary>
        public static Stack<Dictionary<ushort, Dictionary<int, uint>>> TargetHandle = new Stack<Dictionary<ushort, Dictionary<int, uint>>>();

        /// <summary>
        /// The ContextInformationCollection is used to record current session for all of the context information results.
        /// </summary>
        public static List<ContextInformation> ContextInformationCollection = new List<ContextInformation>();

        /// <summary>
        /// Indicate whether the current parsing session is in the loop of the context session parsing
        /// </summary>
        public static bool IsLooperCall = false;

        /// <summary>
        /// Indicate whether the current parsing session is need to parse crops layer
        /// </summary>
        public static bool NeedToParseCROPSLayer = false;

        /// <summary>
        /// Indicate whether this session is only for getting the server object handle
        /// </summary>
        public static bool IsOnlyGetServerHandle = false;

        /// <summary>
        /// Used to record all MAPIHTTP buffers in one session is compressed one or not
        /// </summary>
        public static List<bool> BuffersIsCompressed = new List<bool>();

        /// <summary>
        /// Used to record fasterTransfer stream property type in RopGetBuffer partial
        /// </summary>
        public static ushort PartialGetType;

        /// <summary>
        /// Used to record fasterTransfer stream property Id in RopGetBuffer partial
        /// </summary>
        public static ushort PartialGetId;

        /// <summary>
        /// Used to record fasterTransfer stream property remain size in RopGetBuffer partial
        /// </summary>
        public static int PartialGetRemainSize = -1;

        /// <summary>
        /// Used to record fasterTransfer stream property sub remain size in RopGetBuffer partial
        /// </summary>
        public static int PartialGetSubRemainSize = -1;

        /// <summary>
        /// Used to indicates if this ROP is about fasterTransfer stream RopGetBuffer partial
        /// </summary>
        public static bool IsGet;

        /// <summary>
        /// Used to record serverUrl of the session which contains a RopGetBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialGetServerUrl;

        /// <summary>
        /// Used to record processName of the session which contains a RopGetBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialGetProcessName;

        /// <summary>
        /// Used to record clientInfo of the session which contains a RopGetBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialGetClientInfo;

        /// <summary>
        /// Used to record session for RopGetBuffer partial
        /// </summary>
        public static Session PartialGetSession;

        /// <summary>
        /// Used to record fasterTransfer stream property type in RopPutBuffer partial
        /// </summary>
        public static ushort PartialPutType;

        /// <summary>
        /// Used to record fasterTransfer stream property Id in RopPutBuffer partial
        /// </summary>
        public static ushort PartialPutId;

        /// <summary>
        /// Used to record fasterTransfer stream property remain size in RopPutBuffer partial
        /// </summary>
        public static int PartialPutRemainSize = -1;

        /// <summary>
        /// Used to record fasterTransfer stream property sub remain size in RopPutBuffer partial
        /// </summary>
        public static int PartialPutSubRemainSize = -1;

        /// <summary>
        /// Used to indicates if this ROP is about fasterTransfer stream RopPutBuffer partial
        /// </summary>
        public static bool IsPut;

        /// <summary>
        /// Used to record serverUrl of the session which contains a RopPutBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutServerUrl;

        /// <summary>
        /// Used to record processName of the session which contains a RopPutBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutProcessName;

        /// <summary>
        /// Used to record clientInfo of the session which contains a RopPutBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutClientInfo;

        /// <summary>
        /// Used to record session for RopPutBuffer partial
        /// </summary>
        public static Session PartialPutSession;

        /// <summary>
        /// Used to record fasterTransfer stream property type in putExtendBuffer partial
        /// </summary>
        public static ushort PartialPutExtendType;

        /// <summary>
        /// Used to record fasterTransfer stream property Id in putExtendBuffer partial
        /// </summary>
        public static ushort PartialPutExtendId;

        /// <summary>
        /// Used to record fasterTransfer stream property remain size in putExtendBuffer partial
        /// </summary>
        public static int PartialPutExtendRemainSize = -1;

        /// <summary>
        /// Used to record fasterTransfer stream property sub remain size in putExtendBuffer partial
        /// </summary>
        public static int PartialPutExtendSubRemainSize = -1;

        /// <summary>
        /// Used to indicates if this ROP is about fasterTransfer stream putExtendBuffer partial
        /// </summary>
        public static bool IsPutExtend;

        /// <summary>
        /// Used to record serverUrl of the session which contains a RopPutExtendBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutExtendServerUrl;

        /// <summary>
        /// Used to record processName of the session which contains a RopPutExtendBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutExtendProcessName;

        /// <summary>
        /// Used to record clientInfo of the session which contains a RopPutExtendBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutExtendClientInfo;

        /// <summary>
        /// Used to record session for putExtendbuffer partial
        /// </summary>
        public static Session PartialPutExtendSession;

        /// <summary>
        /// Used to indicates if there is one byte need to be read before parsing fasterTransfer element
        /// </summary>
        public static bool IsOneMoreByteToRead = false;

        /// <summary>
        /// Record the map in handle, sessionId and PartialContextInformation for RopGetBuffer
        /// </summary>
        public static Dictionary<uint, SortedDictionary<int, PartialContextInformation>> HandleWithSessionGetContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();

        /// <summary>
        /// Record the map in handle, sessionId and PartialContextInformation for RopPutBuffer
        /// </summary>
        public static Dictionary<uint, SortedDictionary<int, PartialContextInformation>> HandleWithSessionPutContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();

        /// <summary>
        /// Record the map in handle, sessionId and PartialContextInformation for RopPutExtendedBuffer
        /// </summary>
        public static Dictionary<uint, SortedDictionary<int, PartialContextInformation>> HandleWithSessionPutExtendContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();

        /// <summary>
        /// The requestDic is used to save the session id and its parsed execute request.
        /// </summary>
        private Dictionary<int, object> requestDic = new Dictionary<int, object>();

        /// <summary>
        /// The responseDic is used to save the session id and its parsed execute response.
        /// </summary>
        private Dictionary<int, object> responseDic = new Dictionary<int, object>();

        /// <summary>
        /// The handleGetDic is used to save the session id and its response handle for RopGetBuffer.
        /// </summary>
        private Dictionary<int, List<uint>> handleGetDic = new Dictionary<int, List<uint>>();

        /// <summary>
        /// The handlePutDic is used to save the session id and its request handle for RopPutBuffer.
        /// </summary>
        private Dictionary<int, List<uint>> handlePutDic = new Dictionary<int, List<uint>>();

        /// <summary>
        /// The requestBytesForHexview is used to save the session id and its parsed request bytes provided for MAPIHexBox.
        /// </summary>
        private Dictionary<int, byte[]> requestBytesForHexview = new Dictionary<int, byte[]>();

        /// <summary>
        /// The responseBytesForHexview is used to save the session id and its parsed response bytes provided for MAPIHexBox.
        /// </summary>
        private Dictionary<int, byte[]> responseBytesForHexview = new Dictionary<int, byte[]>();

        /// <summary>
        /// The JsonResult is used to save the Json string which converted by parse result
        /// </summary>
        public static string JsonResult = string.Empty;

        /// <summary>
        /// The JsonFile is used to set a file name to save JsonResult
        /// </summary>
        public static string JsonFile = "Json.txt";

        /// <summary>
        /// The JsonFile is used to set a file name to save error messages when automation test
        /// </summary>
        public static string ErrorFile = "Error.txt";

        /// <summary>
        /// The AllRopsList is used to save all Rop messages when automation test.
        /// </summary>
        public static List<string> AllRopsList = new List<string>();

        /// <summary>
        /// Enum for traffic direction
        /// </summary>
        public enum TrafficDirection
        {
            /// <summary>
            /// Indicates request
            /// </summary>
            In,

            /// <summary>
            /// Indicates response
            /// </summary>
            Out
        }

        /// <summary>
        /// Gets or sets the ROPInputBuffer payload for compressed or XOR
        /// </summary>
        public static List<byte[]> InputPayLoadCompressedXOR { get; set; }

        /// <summary>
        /// Gets or sets the ROPOutputBuffer payload for compressed or XOR
        /// </summary>
        public static List<byte[]> OutputPayLoadCompressedXOR { get; set; }

        /// <summary>
        /// Gets or sets the AuxiliaryBufferPayload payload for compressed or XOR
        /// </summary>
        public static byte[] AuxPayLoadCompressedXOR { get; set; }

        /// <summary>
        /// Gets or sets the Tree View control where displayed the MAPI message.
        /// </summary>
        public TreeView MAPIViewControl { get; set; }

        /// <summary>
        /// Gets or sets the control collection where displayed the MAPI parsed message and corresponding hex data.
        /// </summary>
        public MAPIControl MAPIControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame has been changed.
        /// </summary>
        public bool bDirty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame is read-only.
        /// </summary>
        public bool bReadOnly { get; set; }

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
        /// Gets or sets the base HTTP headers assigned by the request or response
        /// </summary>
        public HTTPHeaders BaseHeaders { get; set; }

        /// <summary>
        /// Gets a value indicating whether the message is MAPI protocol message.
        /// </summary>
        public bool IsMapihttp
        {
            get
            {
                if (this.session != null)
                {
                    if (this is IRequestInspector2)
                    {
                        return this.session.RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                    }
                    else if ((this is IResponseInspector2) && this.session.id != 0)
                    {
                        if ((this is IResponseInspector2) && this.session.ResponseHeaders.Exists("X-ResponseCode"))
                        {
                            string responseCode = this.session.ResponseHeaders["X-ResponseCode"];
                            if (responseCode == "0")
                            {
                                return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                            }
                            else if (responseCode != string.Empty)
                            {
                                return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "text/html");
                            }
                        }
                    }
                    else if ((this is IResponseInspector2) && this.session["X-ResponseCode"] != null)
                    {
                        string responseCode = this.session["X-ResponseCode"];
                        if (responseCode == "0")
                        {
                            return this.session["Content-Type"] != null && this.session["Content-Type"] == "application/mapi-http";
                        }
                        else if (responseCode != string.Empty)
                        {
                            return this.session["Content-Type"] != null && this.session["Content-Type"] == "text/html";
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the Session object to pull frame data from Fiddler.
        /// </summary>
        internal Session session { get; set; }

        /// <summary>
        /// Gets or sets the raw bytes from the frame
        /// </summary>
        private byte[] rawBody { get; set; }

        /// <summary>
        /// Parse special session's response message to MAPI layer only
        /// </summary>
        /// <param name="currentSession">The session to parse</param>
        /// <param name="outputHandleIndex">The handle index need to get</param>
        /// <returns>The object handle table</returns>
        public static uint ParseResponseMessageSimplely(Session currentSession, int outputHandleIndex)
        {
            uint handle_InResponse = 0;
            if (IsMapihttpSession(currentSession, TrafficDirection.Out))
            {
                byte[] bytesForHexView;
                object mapiResponse;
                mapiResponse = ParseHTTPExecuteResponsePayload(currentSession.ResponseHeaders, currentSession, currentSession.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                int rgbOutputBufferCount = (mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Length;

                for (int i = 0; i < rgbOutputBufferCount; i++)
                {
                    handle_InResponse = ((mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[i].Payload as ROPOutputBuffer_WithoutCROPS).ServerObjectHandleTable[outputHandleIndex];
                }
            }

            return handle_InResponse;
        }

        /// <summary>
        /// Parse special session's response message to MS-OXCROPS layer
        /// </summary>
        /// <param name="headers">The header of this parsing session .</param>
        /// <param name="currentSession">The parsing session.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <param name="bytes">The bytes provided for MAPI view layer.</param>
        /// <returns>The object parsed result</returns>
        public static object ParseHTTPExecuteResponsePayload(HTTPHeaders headers, Session currentSession, byte[] bytesFromHTTP, TrafficDirection direction, out byte[] bytes)
        {
            object objectOut = null;
            byte[] emptyByte = new byte[0];
            bytes = emptyByte;
            string requestType = string.Empty;
            if (!MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
            {
                if (bytesFromHTTP == null || bytesFromHTTP.Length == 0 || headers == null || !headers.Exists("X-RequestType"))
                {
                    return null;
                }

                requestType = headers["X-RequestType"];

                if (requestType == null)
                {
                    return null;
                }
            }
            else
            {
                if (bytesFromHTTP == null || bytesFromHTTP.Length == 0 || currentSession.RequestHeaders == null || !currentSession.RequestHeaders.Exists("X-RequestType"))
                {
                    return null;
                }

                requestType = currentSession.RequestHeaders["X-RequestType"];

                if (requestType == null)
                {
                    return null;
                }
            }

            try
            {
                if (direction == TrafficDirection.Out && MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                {
                    if (currentSession["Transfer-Encoding"] != null && currentSession["Transfer-Encoding"] == "chunked")
                    {
                        bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                        bytes = bytesFromHTTP;
                    }
                }
                else if (direction == TrafficDirection.Out && headers.Exists("Transfer-Encoding") && headers["Transfer-Encoding"] == "chunked")
                {
                    bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                    bytes = bytesFromHTTP;
                }
                else
                {
                    bytes = bytesFromHTTP;
                }

                Stream stream = new MemoryStream(bytesFromHTTP);
                MAPIInspector.ParsingSession = currentSession;

                if (direction == TrafficDirection.Out && requestType == "Execute")
                {
                    ExecuteResponseBody executeResponse = new ExecuteResponseBody();
                    executeResponse.Parse(stream);
                    objectOut = executeResponse;
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
        /// Method to judge whether a session is MAPIHTTP message or not
        /// </summary>
        /// <param name="currentSession">The session to be judged</param>
        /// <param name="direction">Traffic direction</param>
        /// <returns>Boole value indicates whether this session is MAPIHTTP layer message</returns>
        public static bool IsMapihttpSession(Session currentSession, TrafficDirection direction)
        {
            if (currentSession != null)
            {
                if (direction == TrafficDirection.In)
                {
                    return currentSession.RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                }
                else if (direction == TrafficDirection.Out && !MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                {
                    if (currentSession.ResponseHeaders.Exists("X-ResponseCode"))
                    {
                        string responseCode = currentSession.ResponseHeaders["X-ResponseCode"];

                        if (responseCode == "0")
                        {
                            return currentSession.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                        }
                        else if (responseCode != string.Empty)
                        {
                            return currentSession.ResponseHeaders.ExistsAndContains("Content-Type", "text/html");
                        }
                    }
                }
                else if (direction == TrafficDirection.Out && currentSession["X-ResponseCode"] != null)
                {
                    string responseCode = currentSession["X-ResponseCode"];
                    if (responseCode == "0")
                    {
                        return currentSession["Content-Type"] != null && currentSession["Content-Type"] == "application/mapi-http";
                    }
                    else if (responseCode != string.Empty)
                    {
                        return currentSession["Content-Type"] != null && currentSession["Content-Type"] == "text/html";
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Called by Fiddler to add the MAPI inspector tab
        /// </summary>
        /// <param name="o">The tab control for the inspector</param>
        public override void AddToTab(TabPage o)
        {
            o.Text = "MAPI";
            this.MAPIControl = new MAPIControl();
            o.Controls.Add(this.MAPIControl);
            this.MAPIControl.Size = o.Size;
            this.MAPIControl.Dock = DockStyle.Fill;
            this.MAPIViewControl = this.MAPIControl.MAPITreeView;
            this.MAPIControl.MAPIHexBox.VScrollBarVisible = true;
            this.MAPIViewControl.AfterSelect -= this.TreeView_AfterSelect;
            this.MAPIViewControl.AfterSelect += this.TreeView_AfterSelect;
            DecodingContext dc = new DecodingContext();
        }

        /// <summary>
        /// Represents the method, which is used to handle the AfterSelect event of a TreeView.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">A System.Windows.Forms.TreeViewEventArgs that contains the event data.</param>
        public void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int request = sender.ToString().LastIndexOf("Request");
            int response = sender.ToString().LastIndexOf("Response");

            if (e.Node.Tag == null)
            {
                this.MAPIControl.MAPIHexBox.Select(0, 0);
                this.MAPIControl.CROPSHexBox.Select(0, 0);
            }
            else
            {
                if (((BaseStructure.Position)e.Node.Tag).IsCompressedXOR)
                {
                    if (((BaseStructure.Position)e.Node.Tag).IsAuxiliayPayload)
                    {
                        this.MAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(AuxPayLoadCompressedXOR);
                    }
                    else
                    {
                        if (request > response)
                        {
                            this.MAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(InputPayLoadCompressedXOR[((BaseStructure.Position)e.Node.Tag).BufferIndex]);
                        }
                        else
                        {
                            this.MAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(OutputPayLoadCompressedXOR[((BaseStructure.Position)e.Node.Tag).BufferIndex]);
                        }
                    }

                    this.MAPIControl.CROPSHexBox.Select(((BaseStructure.Position)e.Node.Tag).StartIndex, ((BaseStructure.Position)e.Node.Tag).Offset);
                    this.MAPIControl.MAPIHexBox.Select(0, 0);
                    this.MAPIControl.CROPSHexBox.Visible = true;
                    ToolTip toolTip = new ToolTip();
                    toolTip.SetToolTip(this.MAPIControl.CROPSHexBox, "This is decompressed payload data.");
                    this.MAPIControl.SplitContainer.Panel2Collapsed = false;
                }
                else
                {
                    this.MAPIControl.MAPIHexBox.Select(((BaseStructure.Position)e.Node.Tag).StartIndex, ((BaseStructure.Position)e.Node.Tag).Offset);
                    this.MAPIControl.CROPSHexBox.Visible = false;
                    this.MAPIControl.SplitContainer.Panel2Collapsed = true;
                }
            }
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
            this.MAPIViewControl.Nodes.Clear();
            this.MAPIControl.MAPIRichTextBox.Visible = false;
            this.MAPIControl.MAPIRichTextBox.Clear();
            this.MAPIControl.CROPSHexBox.Visible = false;
            byte[] empty = new byte[0];
            this.MAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(empty);
            this.MAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
            this.MAPIControl.SplitContainer.Panel2Collapsed = true;
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

            if (this.IsMapihttp)
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
        /// This method is used to parse the sessions in advance, which is designed for the related context information ROPs.
        /// </summary>
        /// <param name="sourceRopID">The ROP ID missing context information</param>
        /// <param name="obj">The target object containing the context information</param>
        /// <param name="bytes">The target byte array provided to HexView</param>
        /// <param name="parameters">The missing context information ROP related parameters</param>
        public void HandleContextInformation(ushort sourceRopID, out object obj, out byte[] bytes, uint[] parameters = null)
        {
            byte[] bytesForHexView;
            object mapiRequest = new object();
            object mapiResponse = new object();
            string savedResult = string.Empty;
            object result = new object();
            Session thisSession = MAPIInspector.ParsingSession;
            int thisSessionID = thisSession.id;
            if (MapiInspector.MAPIInspector.IsFromFiddlerCore(thisSession))
            {
                thisSessionID = int.Parse(thisSession["VirtualID"]);
            }
            if ((RopIdType)sourceRopID == RopIdType.RopLogon)
            {
                this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                obj = this.ParseResponseMessage(thisSession, out bytesForHexView, true);
                bytes = bytesForHexView;
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopSetMessageReadFlag)
            {
                Session currentSession = AllSessions[Convert.ToInt32(thisSession["Number"]) - 1];
                string serverurl = thisSession.RequestHeaders.RequestPath;
                string processName = thisSession.LocalProcess;
                string clientInfo = thisSession.RequestHeaders["X-ClientInfo"];

                if (parameters != null && parameters.Length > 0)
                {
                    // parsing the previous sessions until DecodingContext.LogonFlagMapLogId contains the LogOn Id in this RopSetMessageReadFlag ROP. 
                    Dictionary<int, uint> dic = new Dictionary<int, uint>();
                    Dictionary<ushort, Dictionary<int, uint>> targetDic = new Dictionary<ushort, Dictionary<int, uint>>();
                    dic.Add(thisSessionID, parameters[0]);
                    targetDic.Add(sourceRopID, dic);
                    TargetHandle.Push(targetDic);

                    do
                    {
                        if (currentSession.RequestHeaders.RequestPath == serverurl && currentSession.LocalProcess == processName && currentSession.RequestHeaders["X-ClientInfo"] == clientInfo &&
                            IsMapihttpSession(currentSession, TrafficDirection.In) && currentSession.RequestHeaders["X-RequestType"] == "Execute")
                        {
                            this.ParseRequestMessage(currentSession, out bytesForHexView, true);
                        }

                        if (Convert.ToInt32(currentSession["Number"]) == 1)
                        {
                            break;
                        }
                        else
                        {
                            currentSession = AllSessions[Convert.ToInt32(currentSession["Number"]) - 1];
                        }
                    }
                    while (DecodingContext.LogonFlagMapLogId.Count == 0 || !(DecodingContext.LogonFlagMapLogId.ContainsKey(serverurl) && DecodingContext.LogonFlagMapLogId[serverurl].ContainsKey(processName) && DecodingContext.LogonFlagMapLogId[serverurl][processName].ContainsKey(clientInfo) && DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo].ContainsKey((byte)parameters[0])));

                    if (DecodingContext.LogonFlagMapLogId.ContainsKey(serverurl) && DecodingContext.LogonFlagMapLogId[serverurl].ContainsKey(processName) && DecodingContext.LogonFlagMapLogId[serverurl][processName].ContainsKey(clientInfo) && DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo].ContainsKey((byte)parameters[0]))
                    {
                        result = DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo][(byte)parameters[0]];
                    }
                    else
                    {
                        result = string.Format("{0} cannot be parsed successfully due to missing the LogOn information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[0]);
                    }

                    if (TargetHandle.Count == 1)
                    {
                        ContextInformation information = new ContextInformation();
                        information.RopID = (RopIdType)sourceRopID;
                        information.Handle = parameters[0];
                        information.RelatedInformation = result;
                        ContextInformationCollection.Add(information);

                        if (!this.OverwriteOriginalInformation(thisSessionID, serverurl, processName, clientInfo, out savedResult))
                        {
                            obj = savedResult;
                            bytes = new byte[0];
                            return;
                        }
                    }

                    TargetHandle.Pop();
                }

                if (DecodingContext.LogonFlagMapLogId.ContainsKey(serverurl) && DecodingContext.LogonFlagMapLogId[serverurl].ContainsKey(processName) && DecodingContext.LogonFlagMapLogId[serverurl][processName].ContainsKey(clientInfo) && DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo].ContainsKey((byte)parameters[0]))
                {
                    // Add this session id(RopSetMessageReadFlag Rop)in DecodingContext.SessionLogonFlagMapLogId.
                    if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(thisSessionID)))
                    {
                        DecodingContext.SessionLogonFlagMapLogId.Add(thisSessionID, DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo]);
                    }

                    // Parsing the request structure of this session.
                    obj = this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                    bytes = bytesForHexView;
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the LogOn information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[0]);
                    bytes = new byte[0];
                }
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopGetPropertiesSpecific)
            {
                this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                obj = this.ParseResponseMessage(thisSession, out bytesForHexView, true);
                bytes = bytesForHexView;
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopWritePerUserInformation)
            {
                Session currentSession = AllSessions[Convert.ToInt32(thisSession["Number"]) - 1];
                string serverurl = thisSession.RequestHeaders.RequestPath;
                string processName = thisSession.LocalProcess;
                string clientInfo = thisSession.RequestHeaders["X-ClientInfo"];

                if (parameters != null && parameters.Length > 0)
                {
                    Dictionary<int, uint> dic = new Dictionary<int, uint>();
                    Dictionary<ushort, Dictionary<int, uint>> targetDic = new Dictionary<ushort, Dictionary<int, uint>>();
                    dic.Add(thisSessionID, parameters[0]);
                    targetDic.Add(sourceRopID, dic);

                    // Parsing the previous sessions until DecodingContext.LogonFlagMapLogId contains the LogOn Id in this RopWritePerUserInformation ROP. 
                    TargetHandle.Push(targetDic);

                    do
                    {
                        if (currentSession.RequestHeaders.RequestPath == serverurl && currentSession["LocalProcess"] == processName && currentSession.RequestHeaders["X-ClientInfo"] == clientInfo &&
                            IsMapihttpSession(currentSession, TrafficDirection.In) && currentSession.RequestHeaders["X-RequestType"] == "Execute")
                        {
                            this.ParseRequestMessage(currentSession, out bytesForHexView);
                        }

                        if (Convert.ToInt32(currentSession["Number"]) == 1)
                        {
                            break;
                        }
                        else
                        {
                            currentSession = AllSessions[Convert.ToInt32(currentSession["Number"]) - 1];
                        }
                    }
                    while (DecodingContext.LogonFlagMapLogId.Count == 0 || !(DecodingContext.LogonFlagMapLogId.ContainsKey(serverurl) && DecodingContext.LogonFlagMapLogId[serverurl].ContainsKey(processName) && DecodingContext.LogonFlagMapLogId[serverurl][processName].ContainsKey(clientInfo) && DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo].ContainsKey((byte)parameters[0])));

                    if (DecodingContext.LogonFlagMapLogId.ContainsKey(serverurl) && DecodingContext.LogonFlagMapLogId[serverurl].ContainsKey(processName) && DecodingContext.LogonFlagMapLogId[serverurl][processName].ContainsKey(clientInfo) && DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo].ContainsKey((byte)parameters[0]))
                    {
                        result = DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo][(byte)parameters[0]];
                    }
                    else
                    {
                        result = string.Format("{0} cannot be parsed successfully due to missing the LogOn information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[0]);
                    }

                    if (TargetHandle.Count == 1)
                    {
                        ContextInformation information = new ContextInformation();
                        information.RopID = (RopIdType)sourceRopID;
                        information.Handle = parameters[0];
                        information.RelatedInformation = result;
                        ContextInformationCollection.Add(information);

                        if (!this.OverwriteOriginalInformation(thisSessionID, serverurl, processName, clientInfo, out savedResult))
                        {
                            obj = savedResult;
                            bytes = new byte[0];
                            return;
                        }
                    }

                    TargetHandle.Pop();
                }

                if (DecodingContext.LogonFlagMapLogId.ContainsKey(serverurl) && DecodingContext.LogonFlagMapLogId[serverurl].ContainsKey(processName) && DecodingContext.LogonFlagMapLogId[serverurl][processName].ContainsKey(clientInfo) && DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo].ContainsKey((byte)parameters[0]))
                {
                    // Add this session id in DecodingContext.SessionLogonFlagsInLogonRop.
                    if (!(DecodingContext.SessionLogonFlagMapLogId != null && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(thisSessionID)))
                    {
                        DecodingContext.SessionLogonFlagMapLogId.Add(thisSessionID, DecodingContext.LogonFlagMapLogId[serverurl][processName][clientInfo]);
                    }

                    // Parsing the request structure of this session.
                    obj = this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                    bytes = bytesForHexView;
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the LogOn information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[0]);
                    bytes = new byte[0];
                }
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopQueryRows || (RopIdType)sourceRopID == RopIdType.RopFindRow || (RopIdType)sourceRopID == RopIdType.RopExpandRow)
            {
                Session currentSession = thisSession;
                int currentSessionID = currentSession.id;
                if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                {
                    currentSessionID = int.Parse(currentSession["VirtualID"]);
                }
                Dictionary<int, uint> dic_QueryRows = new Dictionary<int, uint>();
                Dictionary<ushort, Dictionary<int, uint>> targetDic = new Dictionary<ushort, Dictionary<int, uint>>();
                dic_QueryRows.Add(thisSessionID, parameters[1]);
                targetDic.Add(sourceRopID, dic_QueryRows);
                TargetHandle.Push(targetDic);
                string serverurl = thisSession.RequestHeaders.RequestPath;
                string processName = thisSession.LocalProcess;
                string clientInfo = thisSession.RequestHeaders["X-ClientInfo"];

                if (parameters != null && parameters.Length > 1)
                {
                    // SetColumn_InputHandles_InResponse is only set in this session(and RopSetColumns) response parse, so if SetColumn_InputHandles_InResponse contains this rops outputhandle means that setcolumn and this rop is in the same session.
                    if (DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 && DecodingContext.SetColumn_InputHandles_InResponse.Contains(parameters[1]))
                    {
                        this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                    }
                    else
                    {
                        currentSession = AllSessions[Convert.ToInt32(thisSession["Number"]) - 1];
                        if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                        {
                            currentSessionID = int.Parse(currentSession["VirtualID"]);
                        }
                        else
                        {
                            currentSessionID = currentSession.id;
                        }
                        while (currentSessionID >= 1 && currentSessionID < thisSessionID)
                        {
                            string currentServerPath = currentSession.RequestHeaders.RequestPath;
                            string currentProcessName = currentSession.LocalProcess;
                            string currentClientInfo = currentSession.RequestHeaders["X-ClientInfo"];

                            if (currentServerPath == serverurl && currentProcessName == processName && currentClientInfo == clientInfo && IsMapihttpSession(currentSession, TrafficDirection.In) && currentSession.RequestHeaders["X-RequestType"] == "Execute")
                            {
                                this.ParseRequestMessage(currentSession, out bytesForHexView, true);
                            }

                            if (Convert.ToInt32(currentSession["Number"]) == 1)
                            {
                                break;
                            }
                            else if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(parameters[1]) && DecodingContext.RowRops_handlePropertyTags[parameters[1]].ContainsKey(currentSessionID) && DecodingContext.RowRops_handlePropertyTags[parameters[1]][currentSessionID].Item1 == serverurl && DecodingContext.RowRops_handlePropertyTags[parameters[1]][currentSessionID].Item2 == processName
                                && DecodingContext.RowRops_handlePropertyTags[parameters[1]][currentSessionID].Item3 == clientInfo)
                            {
                                break;
                            }
                            else
                            {
                                if (Convert.ToInt32(currentSession["Number"]) == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentSession = AllSessions[Convert.ToInt32(currentSession["Number"]) - 1];
                                    if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                                    {
                                        currentSessionID = int.Parse(currentSession["VirtualID"]);
                                    }
                                    else
                                    {
                                        currentSessionID = currentSession.id;
                                    }
                                }
                            }
                        }
                    }

                    if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(parameters[1]) && DecodingContext.RowRops_handlePropertyTags[parameters[1]].ContainsKey(currentSessionID))
                    {
                        result = DecodingContext.RowRops_handlePropertyTags[parameters[1]][currentSessionID].Item4;
                    }
                    else
                    {
                        result = string.Format("{0} cannot be parsed successfully due to missing the PropertyTags for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                    }
                }

                if (TargetHandle.Count == 1)
                {
                    ContextInformation information = new ContextInformation();
                    information.RopID = (RopIdType)sourceRopID;
                    information.Handle = parameters[1];
                    information.RelatedInformation = result;
                    ContextInformationCollection.Add(information);

                    if (!this.OverwriteOriginalInformation(thisSessionID, serverurl, processName, clientInfo, out savedResult))
                    {
                        obj = savedResult;
                        bytes = new byte[0];
                        return;
                    }
                }

                TargetHandle.Pop();

                if (result is PropertyTag[])
                {
                    if (currentSession != thisSession)
                    {
                        Dictionary<int, Tuple<string, string, string, PropertyTag[]>> sessionTagMap = new Dictionary<int, Tuple<string, string, string, PropertyTag[]>>();
                        sessionTagMap = DecodingContext.RowRops_handlePropertyTags[parameters[1]];
                        DecodingContext.RowRops_handlePropertyTags.Remove(parameters[1]);
                        Tuple<string, string, string, PropertyTag[]> tupleValue = sessionTagMap[currentSessionID];
                        sessionTagMap.Remove(currentSessionID);
                        sessionTagMap.Add(thisSessionID, tupleValue);
                        DecodingContext.RowRops_handlePropertyTags.Add(parameters[1], sessionTagMap);
                    }

                    obj = this.ParseResponseMessage(thisSession, out bytesForHexView, true);
                    bytes = bytesForHexView;
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the PropertyTags for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                    bytes = new byte[0];
                }
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopNotify)
            {
                Session currentSession = thisSession;
                int currentSessionID = currentSession.id;
                if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                {
                    currentSessionID = int.Parse(currentSession["VirtualID"]);
                }
                uint targetSessionID = 0;
                Dictionary<int, uint> dic_Notify = new Dictionary<int, uint>();
                Dictionary<ushort, Dictionary<int, uint>> targetDic = new Dictionary<ushort, Dictionary<int, uint>>();
                dic_Notify.Add(thisSessionID, parameters[1]);
                targetDic.Add(sourceRopID, dic_Notify);
                TargetHandle.Push(targetDic);
                uint resultTableSessionId = 0;
                string serverurl = thisSession.RequestHeaders.RequestPath;
                string processName = thisSession.LocalProcess;
                string clientInfo = thisSession.RequestHeaders["X-ClientInfo"];

                if (parameters != null && parameters.Length > 1)
                {
                    // SetColumn_InputHandles_InResponse is only set in this session(and RopSetColumns) response parse, so if SetColumn_InputHandles_InResponse contains this ROP's output handle means the RopSetColumns and this ROP is in the same session.
                    if (DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 && DecodingContext.SetColumn_InputHandles_InResponse.Contains(parameters[1]))
                    {
                        this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                    }
                    else
                    {
                        currentSession = AllSessions[Convert.ToInt32(thisSession["Number"]) - 1];
                        if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                        {
                            currentSessionID = int.Parse(currentSession["VirtualID"]);
                        }
                        else
                        {
                            currentSessionID = currentSession.id;
                        }

                        // isFound used to specify whether the setColumns for this notify has found.
                        bool isFound = false;

                        while (currentSessionID >= 1)
                        {
                            string currentServerPath = currentSession.RequestHeaders.RequestPath;
                            string currentProcessName = currentSession.LocalProcess;
                            string currentClientInfo = currentSession.RequestHeaders["X-ClientInfo"];

                            if (currentServerPath == serverurl && currentProcessName == processName && currentClientInfo == clientInfo && IsMapihttpSession(currentSession, TrafficDirection.Out) && currentSession.RequestHeaders["X-RequestType"] == "Execute")
                            {
                                IsOnlyGetServerHandle = true;
                                object resResult = this.ParseResponseMessage(currentSession, out bytesForHexView, false);
                                IsOnlyGetServerHandle = false;

                                if (resResult != null && (resResult as ExecuteResponseBody).RopBuffer != null && (resResult as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Count() != 0)
                                {
                                    List<uint> tableHandles = ((ROPOutputBuffer_WithoutCROPS)(resResult as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[0].Payload).ServerObjectHandleTable.ToList();

                                    if (tableHandles.Contains(parameters[1]) && currentServerPath == serverurl && currentProcessName == processName && currentClientInfo == clientInfo)
                                    {
                                        int handleIndex = tableHandles.IndexOf(parameters[1]);
                                        object requestResult = this.ParseRequestMessage(currentSession, out bytesForHexView, true);

                                        if (requestResult != null)
                                        {
                                            if ((requestResult as ExecuteRequestBody).RopBuffer != null && (requestResult as ExecuteRequestBody).RopBuffer.Buffers.Count() != 0)
                                            {
                                                foreach (ExtendedBuffer_Input input in (requestResult as ExecuteRequestBody).RopBuffer.Buffers)
                                                {
                                                    if (input.Payload is ROPInputBuffer)
                                                    {
                                                        object[] rops = (input.Payload as ROPInputBuffer).RopsList;

                                                        foreach (var rop in rops)
                                                        {
                                                            if ((rop is RopGetRulesTableRequest && ((rop as RopGetRulesTableRequest).OutputHandleIndex == handleIndex)) ||
                                                            (rop is RopGetAttachmentTableRequest && ((rop as RopGetAttachmentTableRequest).OutputHandleIndex == handleIndex)) ||
                                                            (rop is RopGetPermissionsTableRequest && ((rop as RopGetAttachmentTableRequest).OutputHandleIndex == handleIndex)) ||
                                                            (rop is RopGetContentsTableRequest && ((rop as RopGetContentsTableRequest).OutputHandleIndex == handleIndex)) ||
                                                            (rop is RopGetHierarchyTableRequest && ((rop as RopGetHierarchyTableRequest).OutputHandleIndex == handleIndex)))
                                                            {
                                                                // Update the fourth parameter of Notify_handlePropertyTags
                                                                if (DecodingContext.Notify_handlePropertyTags.Count > 0)
                                                                {
                                                                    List<int> sessions = DecodingContext.Notify_handlePropertyTags[parameters[1]].Keys.ToList();
                                                                    foreach (int sessionID in sessions)
                                                                    {
                                                                        if (sessionID <= thisSessionID && sessionID >= currentSessionID)
                                                                        {
                                                                            Tuple<string, string, string, PropertyTag[], string> originalTuple = DecodingContext.Notify_handlePropertyTags[parameters[1]][sessionID];
                                                                            if (originalTuple.Item5 == string.Empty)
                                                                            {
                                                                                Tuple<string, string, string, PropertyTag[], string> updatedTuple = new Tuple<string, string, string, PropertyTag[], string>(originalTuple.Item1, originalTuple.Item2, originalTuple.Item3, originalTuple.Item4, rop.GetType().Name);
                                                                                DecodingContext.Notify_handlePropertyTags[parameters[1]][sessionID] = updatedTuple;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            if ((parameters[0] != 0 && rop is RopGetContentsTableRequest &&
                                                                (rop as RopGetContentsTableRequest).OutputHandleIndex == handleIndex) ||
                                                                (parameters[0] == 0 && rop is RopGetHierarchyTableRequest &&
                                                                (rop as RopGetHierarchyTableRequest).OutputHandleIndex == handleIndex))
                                                            {
                                                                // Break the looper
                                                                isFound = true;
                                                                resultTableSessionId = (uint)currentSessionID;
                                                                break;
                                                            }
                                                        }

                                                        if (isFound == true)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (isFound == true)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (Convert.ToInt32(currentSession["Number"]) == 1)
                            {
                                break;
                            }
                            else
                            {
                                currentSession = AllSessions[Convert.ToInt32(currentSession["Number"]) - 1];
                                if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                                {
                                    currentSessionID = int.Parse(currentSession["VirtualID"]);
                                }
                                else
                                {
                                    currentSessionID = currentSession.id;
                                }
                            }
                        }
                    }
                }

                if (DecodingContext.Notify_handlePropertyTags.ContainsKey(parameters[1]))
                {
                    // Just get a biggest value for the distance for initial                                    
                    string searchkey = parameters[0] == 0 ? "RopGetHierarchyTableRequest" : "RopGetContentsTableRequest";

                    foreach (uint sessionID in DecodingContext.Notify_handlePropertyTags[parameters[1]].Keys)
                    {
                        Tuple<string, string, string, PropertyTag[], string> currentTuple = DecodingContext.Notify_handlePropertyTags[parameters[1]][(int)sessionID];
                        if ((sessionID >= resultTableSessionId && sessionID <= thisSessionID))
                        {
                            if (currentTuple.Item1 == serverurl && currentTuple.Item2 == processName && currentTuple.Item3 == clientInfo)
                            {
                                if (currentTuple.Item5.Contains(searchkey) && targetSessionID < sessionID)
                                {
                                    targetSessionID = sessionID;
                                }
                            }
                        }
                    }
                    if (targetSessionID != 0)
                    {
                        result = DecodingContext.Notify_handlePropertyTags[parameters[1]][(int)targetSessionID].Item4;
                    }
                    else
                    {
                        result = string.Format("RopNotify cannot be parsed successfully due to missing the PropertyTags for handle {0}, check whether the trace is complete.", parameters[1]);
                    }
                }
                else
                {
                    result = string.Format("RopNotify cannot be parsed successfully due to missing the PropertyTags for handle {0}, check whether the trace is complete.", parameters[1]);
                }

                if (TargetHandle.Count == 1)
                {
                    ContextInformation information = new ContextInformation();
                    information.RopID = (RopIdType)sourceRopID;
                    information.Handle = parameters[1];
                    information.RelatedInformation = result;
                    ContextInformationCollection.Add(information);

                    if (!this.OverwriteOriginalInformation(thisSessionID, serverurl, processName, clientInfo, out savedResult))
                    {
                        obj = savedResult;
                        bytes = new byte[0];
                        return;
                    }
                }

                TargetHandle.Pop();

                if (result is PropertyTag[])
                {
                    if (currentSession != thisSession)
                    {
                        Dictionary<int, Tuple<string, string, string, PropertyTag[], string>> sessionTagMap = new Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>();
                        sessionTagMap = DecodingContext.Notify_handlePropertyTags[parameters[1]];
                        DecodingContext.Notify_handlePropertyTags.Remove(parameters[1]);
                        Tuple<string, string, string, PropertyTag[], string> tupleValue = sessionTagMap[(int)targetSessionID];
                        sessionTagMap.Remove((int)targetSessionID);
                        sessionTagMap.Add(thisSessionID, tupleValue);
                        DecodingContext.Notify_handlePropertyTags.Add(parameters[1], sessionTagMap);
                    }

                    obj = this.ParseResponseMessage(thisSession, out bytesForHexView, true);
                    bytes = bytesForHexView;
                }
                else
                {
                    obj = string.Format("RopNotify cannot be parsed successfully due to missing the PropertyTags for handle {0}, check whether the trace is complete.", parameters[1]);
                    bytes = new byte[0];
                }
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopBufferTooSmall)
            {
                if (DecodingContext.SessionRequestRemainSize.Count > 0 && DecodingContext.SessionRequestRemainSize.ContainsKey(thisSessionID))
                {
                    obj = this.responseDic[thisSessionID];
                    bytes = this.responseBytesForHexview[thisSessionID];
                }
                else
                {
                    this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                    obj = this.ParseResponseMessage(thisSession, out bytesForHexView, true);
                    bytes = bytesForHexView;
                }
            }
            else
            {
                obj = null;
                bytes = new byte[0];
            }
        }

        /// <summary>
        /// Restore the covered related context information during loop call
        /// </summary>
        /// <param name="sessionID">The session ID</param>
        /// <param name="serverurl">The server URL for this session</param>
        /// <param name="processName">The process name for this session</param>
        /// <param name="clientInfo">The clientInfo for this session</param>
        /// <param name="result">The result for missing related information </param>
        /// <returns>The result for overwriting.</returns>
        public bool OverwriteOriginalInformation(int sessionID, string serverurl, string processName, string clientInfo, out string result)
        {
            bool checkResult = true;
            result = string.Empty;

            if (ContextInformationCollection.Count > 0)
            {
                foreach (ContextInformation infor in ContextInformationCollection)
                {
                    if (infor.RelatedInformation.GetType() != typeof(string))
                    {
                        switch (infor.RopID)
                        {
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                break;
                            case RopIdType.RopQueryRows:
                            case RopIdType.RopFindRow:
                            case RopIdType.RopExpandRow:
                                if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(infor.Handle) && DecodingContext.RowRops_handlePropertyTags[infor.Handle].ContainsKey(sessionID)
                                    && DecodingContext.RowRops_handlePropertyTags[infor.Handle][sessionID].Item1 == serverurl && DecodingContext.RowRops_handlePropertyTags[infor.Handle][sessionID].Item2 == processName)
                                {
                                    if (DecodingContext.RowRops_handlePropertyTags[infor.Handle][sessionID].Item4 != (PropertyTag[])infor.RelatedInformation)
                                    {
                                        Tuple<string, string, string, PropertyTag[]> value = new Tuple<string, string, string, PropertyTag[]>(serverurl, processName, clientInfo, (PropertyTag[])infor.RelatedInformation);
                                        DecodingContext.RowRops_handlePropertyTags[infor.Handle][sessionID] = value;
                                    }
                                }

                                break;
                            case RopIdType.RopNotify:
                                if (DecodingContext.Notify_handlePropertyTags.ContainsKey(infor.Handle) && DecodingContext.Notify_handlePropertyTags[infor.Handle].ContainsKey(sessionID)
                                    && DecodingContext.Notify_handlePropertyTags[infor.Handle][sessionID].Item1 == serverurl && DecodingContext.Notify_handlePropertyTags[infor.Handle][sessionID].Item2 == processName)
                                {
                                    if (DecodingContext.Notify_handlePropertyTags[infor.Handle][sessionID].Item4 != (PropertyTag[])infor.RelatedInformation)
                                    {
                                        Tuple<string, string, string, PropertyTag[], string> value = new Tuple<string, string, string, PropertyTag[], string>(serverurl, processName, clientInfo, (PropertyTag[])infor.RelatedInformation, string.Empty);
                                        DecodingContext.Notify_handlePropertyTags[infor.Handle][sessionID] = value;
                                    }
                                }

                                break;
                            case RopIdType.RopSetMessageReadFlag:
                            case RopIdType.RopWritePerUserInformation:
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                            case RopIdType.RopFastTransferDestinationPutBufferExtended:
                                break;
                        }
                    }
                    else
                    {
                        result = infor.RelatedInformation.ToString();
                        return false;
                    }
                }
            }

            return checkResult;
        }

        /// <summary>
        /// Parse sessions from start to this session to find informations for RopGetBuffer partial status
        /// </summary>
        /// <param name="ropID">The ROP id related with partial</param>
        /// <param name="parameters">The handle information</param>
        /// <param name="bytes">The output bytes returned</param>
        /// <returns>The parsed result for current session</returns>
        public object Partial(RopIdType ropID, uint parameters, out byte[] bytes)
        {
            byte[] bytesForHexView = new byte[0];
            object obj = new object();
            bytes = bytesForHexView;
            Session thisSession = MAPIInspector.ParsingSession;
            int thisSessionID = thisSession.id;
            if (MapiInspector.MAPIInspector.IsFromFiddlerCore(thisSession))
            {
                thisSessionID = int.Parse(thisSession["VirtualID"]);
            }

            if (ropID == RopIdType.RopFastTransferSourceGetBuffer)
            {
                if (this.responseDic.ContainsKey(thisSessionID))
                {
                    obj = this.responseDic[thisSessionID];
                    bytes = this.responseBytesForHexview[thisSessionID];

                    if (HandleWithSessionGetContextInformation.ContainsKey(parameters) && HandleWithSessionGetContextInformation[parameters].ContainsKey(thisSessionID))
                    {
                        PartialGetId = HandleWithSessionGetContextInformation[parameters][thisSessionID].ID;
                        PartialGetType = HandleWithSessionGetContextInformation[parameters][thisSessionID].Type;
                        PartialGetRemainSize = HandleWithSessionGetContextInformation[parameters][thisSessionID].RemainSize;
                        PartialGetSubRemainSize = HandleWithSessionGetContextInformation[parameters][thisSessionID].SubRemainSize;
                        IsGet = HandleWithSessionGetContextInformation[parameters][thisSessionID].IsGet;
                        OutputPayLoadCompressedXOR = HandleWithSessionGetContextInformation[parameters][thisSessionID].PayLoadCompresssedXOR;
                        PartialGetServerUrl = thisSession.RequestHeaders.RequestPath;
                        PartialGetProcessName = thisSession.LocalProcess;
                        PartialGetClientInfo = thisSession.RequestHeaders["X-ClientInfo"];
                    }
                }
                else
                {
                    Session currentSession = AllSessions[1];
                    int currentSessionID = currentSession.id;
                    if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                    {
                        currentSessionID = int.Parse(currentSession["VirtualID"]);
                    }
                    int sessionGetContextCount = HandleWithSessionGetContextInformation.Count;

                    if (sessionGetContextCount > 0 && HandleWithSessionGetContextInformation.ContainsKey(parameters))
                    {
                        int lastSavedSessionID = HandleWithSessionGetContextInformation[parameters].Keys.Last();

                        if (lastSavedSessionID == thisSessionID)
                        {
                            HandleWithSessionGetContextInformation[parameters].Remove(lastSavedSessionID);

                            if (HandleWithSessionGetContextInformation[parameters].Count > 0)
                            {
                                lastSavedSessionID = HandleWithSessionGetContextInformation[parameters].Keys.Last();
                            }
                            else
                            {
                                lastSavedSessionID = currentSessionID;
                            }
                        }

                        if (lastSavedSessionID != 1)
                        {
                            PartialGetId = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].ID;
                            PartialGetType = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].Type;
                            PartialGetRemainSize = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].RemainSize;
                            PartialGetSubRemainSize = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].SubRemainSize;
                            IsGet = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].IsGet;
                            OutputPayLoadCompressedXOR = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].PayLoadCompresssedXOR;
                            PartialGetSession = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].Session;
                            PartialGetServerUrl = PartialGetSession.RequestHeaders.RequestPath;
                            PartialGetProcessName = PartialGetSession.LocalProcess;
                            PartialGetClientInfo = PartialGetSession.RequestHeaders["X-ClientInfo"];

                            currentSession = AllSessions[Convert.ToInt32(PartialGetSession["Number"]) + 1];
                        }
                        if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                        {
                            currentSessionID = int.Parse(currentSession["VirtualID"]);
                        }
                        else
                        {
                            currentSessionID = currentSession.id;
                        }
                    }

                    string serverurl = thisSession.RequestHeaders.RequestPath;
                    string processName = thisSession.LocalProcess;
                    string clientInfo = thisSession.RequestHeaders["X-ClientInfo"];

                    while (currentSessionID < thisSessionID)
                    {
                        if (currentSession.RequestHeaders.RequestPath == serverurl && currentSession.LocalProcess == processName && currentSession.RequestHeaders["X-ClientInfo"] == clientInfo && IsMapihttpSession(currentSession, TrafficDirection.Out) && currentSession.RequestHeaders["X-RequestType"] == "Execute")
                        {
                            List<uint> tableHandles = new List<uint>();

                            if (this.handleGetDic.ContainsKey(currentSessionID))
                            {
                                tableHandles = this.handleGetDic[currentSessionID];
                            }
                            else
                            {
                                try
                                {
                                    IsOnlyGetServerHandle = true;
                                    object mapiResponse = this.ParseResponseMessage(currentSession, out bytesForHexView, false);

                                    if (mapiResponse != null && (mapiResponse as ExecuteResponseBody).RopBuffer != null && (mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Count() != 0)
                                    {
                                        tableHandles = ((ROPOutputBuffer_WithoutCROPS)(mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[0].Payload).ServerObjectHandleTable.ToList();
                                    }
                                }
                                finally
                                {
                                    IsOnlyGetServerHandle = false;
                                }
                            }

                            if (tableHandles.Contains(parameters))
                            {
                                this.ParseResponseMessage(currentSession, out bytesForHexView, true);
                            }
                        }

                        currentSession = AllSessions[Convert.ToInt32(currentSession["Number"]) + 1];
                        if (currentSessionID == currentSession.id ||
                            (currentSession["VirtualID"] != null && currentSessionID == int.Parse(currentSession["VirtualID"]))) break;
                        if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                        {
                            currentSessionID = int.Parse(currentSession["VirtualID"]);
                        }
                        else
                        {
                            currentSessionID = currentSession.id;
                        }
                    }

                    if (!DecodingContext.PartialInformationReady.ContainsKey(thisSessionID))
                    {
                        DecodingContext.PartialInformationReady.Add(thisSessionID, true);
                    }

                    obj = this.ParseResponseMessage(thisSession, out bytesForHexView, true);
                    DecodingContext.PartialInformationReady = new Dictionary<int, bool>();
                    bytes = bytesForHexView;
                }
            }
            else if (ropID == RopIdType.RopFastTransferDestinationPutBuffer || ropID == RopIdType.RopFastTransferDestinationPutBufferExtended)
            {
                if (this.requestDic.ContainsKey(thisSessionID))
                {
                    obj = this.requestDic[thisSessionID];
                    bytes = this.requestBytesForHexview[thisSessionID];

                    if ((RopIdType)ropID == RopIdType.RopFastTransferDestinationPutBuffer)
                    {
                        if (HandleWithSessionPutContextInformation.ContainsKey(parameters) && HandleWithSessionPutContextInformation[parameters].ContainsKey(thisSessionID))
                        {
                            PartialPutId = HandleWithSessionPutContextInformation[parameters][thisSessionID].ID;
                            PartialPutType = HandleWithSessionPutContextInformation[parameters][thisSessionID].Type;
                            PartialPutRemainSize = HandleWithSessionPutContextInformation[parameters][thisSessionID].RemainSize;
                            PartialPutSubRemainSize = HandleWithSessionPutContextInformation[parameters][thisSessionID].SubRemainSize;
                            IsPut = true;
                            InputPayLoadCompressedXOR = HandleWithSessionPutContextInformation[parameters][thisSessionID].PayLoadCompresssedXOR;
                            PartialPutServerUrl = thisSession.RequestHeaders.RequestPath;
                            PartialPutProcessName = thisSession.LocalProcess;
                            PartialPutClientInfo = thisSession.RequestHeaders["X-ClientInfo"];
                        }
                    }
                    else
                    {
                        if (HandleWithSessionPutExtendContextInformation.ContainsKey(parameters) && HandleWithSessionPutExtendContextInformation[parameters].ContainsKey(thisSessionID))
                        {
                            PartialPutExtendId = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].ID;
                            PartialPutExtendType = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].Type;
                            PartialPutExtendRemainSize = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].RemainSize;
                            PartialPutExtendSubRemainSize = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].SubRemainSize;
                            IsPutExtend = true;
                            InputPayLoadCompressedXOR = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].PayLoadCompresssedXOR;
                            PartialPutExtendServerUrl = thisSession.RequestHeaders.RequestPath;
                            PartialPutExtendProcessName = thisSession.LocalProcess;
                            PartialPutExtendClientInfo = thisSession.RequestHeaders["X-ClientInfo"];
                        }
                    }
                }
                else
                {
                    Session currentSession = AllSessions[1];
                    int currentSessionID = currentSession.id;
                    if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                    {
                        currentSessionID = int.Parse(currentSession["VirtualID"]);
                    }
                    if (ropID == RopIdType.RopFastTransferDestinationPutBuffer)
                    {
                        int sessionPutContextCount = HandleWithSessionPutContextInformation.Count;

                        if (sessionPutContextCount > 0 && HandleWithSessionPutContextInformation.ContainsKey(parameters))
                        {
                            int lastSavedSessionID = HandleWithSessionPutContextInformation[parameters].Keys.Last();

                            if (lastSavedSessionID == thisSessionID)
                            {
                                HandleWithSessionPutContextInformation[parameters].Remove(lastSavedSessionID);

                                if (HandleWithSessionPutContextInformation[parameters].Count > 0)
                                {
                                    lastSavedSessionID = HandleWithSessionPutContextInformation[parameters].Keys.Last();
                                }
                                else
                                {
                                    lastSavedSessionID = currentSessionID;
                                }
                            }

                            if (lastSavedSessionID != 1)
                            {
                                PartialPutId = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].ID;
                                PartialPutType = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].Type;
                                PartialPutRemainSize = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].RemainSize;
                                PartialPutSubRemainSize = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].SubRemainSize;
                                IsPut = true;
                                InputPayLoadCompressedXOR = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].PayLoadCompresssedXOR;
                                PartialPutSession = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].Session;
                                PartialPutServerUrl = PartialPutSession.RequestHeaders.RequestPath;
                                PartialPutProcessName = PartialPutSession.LocalProcess;
                                PartialPutClientInfo = PartialPutSession.RequestHeaders["X-ClientInfo"];
                                currentSession = AllSessions[Convert.ToInt32(PartialPutSession["Number"]) + 1];
                            }

                            if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                            {
                                currentSessionID = int.Parse(currentSession["VirtualID"]);
                            }
                            else
                            {
                                currentSessionID = currentSession.id;
                            }
                        }
                    }
                    else
                    {
                        int sessionPutExtendContextCount = HandleWithSessionPutExtendContextInformation.Count;

                        if (sessionPutExtendContextCount > 0 && HandleWithSessionPutExtendContextInformation.ContainsKey(parameters))
                        {
                            int lastSavedSessionID = HandleWithSessionPutExtendContextInformation[parameters].Keys.Last();

                            if (lastSavedSessionID == thisSessionID)
                            {
                                HandleWithSessionPutExtendContextInformation[parameters].Remove(lastSavedSessionID);

                                if (HandleWithSessionPutExtendContextInformation[parameters].Count > 0)
                                {
                                    lastSavedSessionID = HandleWithSessionPutExtendContextInformation[parameters].Keys.Last();
                                }
                                else
                                {
                                    lastSavedSessionID = currentSessionID;
                                }
                            }

                            if (lastSavedSessionID != 1)
                            {
                                PartialPutExtendId = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].ID;
                                PartialPutExtendType = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].Type;
                                PartialPutExtendRemainSize = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].RemainSize;
                                PartialPutExtendSubRemainSize = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].SubRemainSize;
                                IsPutExtend = true;
                                InputPayLoadCompressedXOR = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].PayLoadCompresssedXOR;
                                PartialPutExtendSession = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].Session;
                                PartialPutExtendServerUrl = PartialPutExtendSession.RequestHeaders.RequestPath;
                                PartialPutExtendProcessName = PartialPutExtendSession.LocalProcess;
                                PartialPutExtendClientInfo = PartialPutExtendSession.RequestHeaders["X-ClientInfo"];
                                currentSession = AllSessions[Convert.ToInt32(PartialPutExtendSession["Number"]) + 1];
                            }

                            if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                            {
                                currentSessionID = int.Parse(currentSession["VirtualID"]);
                            }
                            else
                            {
                                currentSessionID = currentSession.id;
                            }
                        }
                    }

                    while (currentSessionID < thisSessionID)
                    {
                        string serverurl = thisSession.RequestHeaders.RequestPath;
                        string processName = thisSession.LocalProcess;
                        string clientInfo = thisSession.RequestHeaders["X-ClientInfo"];

                        if (currentSession.RequestHeaders.RequestPath == serverurl && currentSession.LocalProcess == processName && currentSession.RequestHeaders["X-ClientInfo"] == clientInfo && IsMapihttpSession(currentSession, TrafficDirection.In) && currentSession.ResponseHeaders["X-RequestType"] == "Execute")
                        {
                            List<uint> tableHandles = new List<uint>();

                            if (this.handlePutDic.ContainsKey(currentSessionID))
                            {
                                tableHandles = this.handlePutDic[currentSessionID];
                            }
                            else
                            {
                                try
                                {
                                    IsOnlyGetServerHandle = true;
                                    object mapiRequest = this.ParseRequestMessage(currentSession, out bytesForHexView, false);

                                    if (mapiRequest != null && (mapiRequest as ExecuteRequestBody).RopBuffer != null && (mapiRequest as ExecuteRequestBody).RopBuffer.Buffers.Count() != 0)
                                    {
                                        tableHandles = ((ROPInputBuffer_WithoutCROPS)(mapiRequest as ExecuteRequestBody).RopBuffer.Buffers[0].Payload).ServerObjectHandleTable.ToList();
                                    }
                                }
                                finally
                                {
                                    IsOnlyGetServerHandle = false;
                                }
                            }

                            if (tableHandles.Contains(parameters))
                            {
                                this.ParseRequestMessage(currentSession, out bytesForHexView, true);
                            }
                            else if (tableHandles.Contains(0xffffffff))
                            {
                                List<uint> tablehandleResList = new List<uint>();

                                try
                                {
                                    IsOnlyGetServerHandle = true;
                                    object mapiResponse = this.ParseResponseMessage(currentSession, out bytesForHexView, false);

                                    if (mapiResponse != null && (mapiResponse as ExecuteResponseBody).RopBuffer != null && (mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Count() != 0)
                                    {
                                        tableHandles = ((ROPOutputBuffer_WithoutCROPS)(mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[0].Payload).ServerObjectHandleTable.ToList();
                                    }
                                }
                                finally
                                {
                                    IsOnlyGetServerHandle = false;
                                }

                                if (tableHandles.Contains(parameters))
                                {
                                    this.ParseRequestMessage(currentSession, out bytesForHexView, true);
                                }
                            }
                        }

                        currentSession = AllSessions[Convert.ToInt32(currentSession["Number"]) + 1];
                        if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                        {
                            currentSessionID = int.Parse(currentSession["VirtualID"]);
                        }
                        else
                        {
                            currentSessionID = currentSession.id;
                        }
                    }

                    if (!DecodingContext.PartialInformationReady.ContainsKey(thisSessionID))
                    {
                        DecodingContext.PartialInformationReady.Add(thisSessionID, true);
                    }

                    obj = this.ParseRequestMessage(thisSession, out bytesForHexView, true);
                    DecodingContext.PartialInformationReady = new Dictionary<int, bool>();
                    bytes = bytesForHexView;
                }
            }

            return obj;
        }

        /// <summary>
        /// Parse special session's request message
        /// </summary>
        /// <param name="parsingSession">The session to parse</param>
        /// <param name="hexViewBytes">Byte array for display in RopHexView</param>
        /// <param name="isLooper">A boolean value indicates if this session is in a loop for parsing context sessions</param>
        /// <returns>MAPI request object</returns>
        public object ParseRequestMessage(Session parsingSession, out byte[] hexViewBytes, bool isLooper = false)
        {
            object mapiRequest = null;
            hexViewBytes = new byte[0];

            if (IsMapihttpSession(parsingSession, TrafficDirection.In))
            {
                NeedToParseCROPSLayer = isLooper;
                byte[] bytesForHexView;
                mapiRequest = this.ParseHTTPPayload(parsingSession.RequestHeaders, parsingSession, parsingSession.requestBodyBytes, TrafficDirection.In, out bytesForHexView);
                hexViewBytes = bytesForHexView;
                int parsingSessionID = parsingSession.id;
                if (MapiInspector.MAPIInspector.IsFromFiddlerCore(parsingSession))
                {
                    parsingSessionID = int.Parse(parsingSession["VirtualID"]);
                }

                if (mapiRequest != null)
                {
                    if (parsingSession.requestBodyBytes.Length != 0 && mapiRequest.GetType().Name == "ExecuteRequestBody" && this.requestDic != null && !this.requestDic.ContainsKey(parsingSessionID))
                    {
                        if (!IsOnlyGetServerHandle)
                        {
                            this.requestDic.Add(parsingSessionID, mapiRequest);
                            this.requestBytesForHexview.Add(parsingSessionID, bytesForHexView);
                        }
                        else
                        {
                            if (!this.handlePutDic.ContainsKey(parsingSessionID))
                            {
                                if ((mapiRequest as ExecuteRequestBody).RopBuffer != null && (mapiRequest as ExecuteRequestBody).RopBuffer.Buffers.Count() != 0)
                                {
                                    this.handlePutDic.Add(parsingSessionID, ((ROPInputBuffer_WithoutCROPS)(mapiRequest as ExecuteRequestBody).RopBuffer.Buffers[0].Payload).ServerObjectHandleTable.ToList());
                                }
                            }
                        }
                    }
                    else if (parsingSession.requestBodyBytes.Length != 0 && mapiRequest.GetType().Name == "ExecuteRequestBody" && this.requestDic != null && this.requestDic.ContainsKey(parsingSessionID) && !IsOnlyGetServerHandle)
                    {
                        this.requestDic.Remove(parsingSessionID);
                        this.requestDic.Add(parsingSessionID, mapiRequest);
                    }
                }
            }

            if (NeedToParseCROPSLayer)
            {
                NeedToParseCROPSLayer = false;
            }

            return mapiRequest;
        }

        /// <summary>
        /// Parse special session's response message
        /// </summary>
        /// <param name="currentSession">The session to parse</param>
        /// <param name="hexViewBytes">Byte array for display in RopHexView</param>
        /// <param name="isLooper">A boolean value indicates if this session is in a loop for parsing context sessions</param>
        /// <returns>MAPI response object</returns>
        public object ParseResponseMessage(Session currentSession, out byte[] hexViewBytes, bool isLooper = false)
        {
            object mapiResponse = null;
            hexViewBytes = new byte[0];
            if (!MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
            {
                if (IsMapihttpSession(currentSession, TrafficDirection.Out) && currentSession.ResponseHeaders["X-ResponseCode"] == "0")
                {
                    NeedToParseCROPSLayer = isLooper;
                    byte[] bytesForHexView;
                    mapiResponse = this.ParseHTTPPayload(currentSession.ResponseHeaders, currentSession, currentSession.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                    hexViewBytes = bytesForHexView;
                    int parsingSessionID = currentSession.id;
                    if (MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                    {
                        parsingSessionID = int.Parse(currentSession["VirtualID"]);
                    }
                    if (mapiResponse != null)
                    {
                        if (currentSession.responseBodyBytes.Length != 0 && mapiResponse.GetType().Name == "ExecuteResponseBody" && this.responseDic != null && !this.responseDic.ContainsKey(parsingSessionID))
                        {
                            if (!IsOnlyGetServerHandle)
                            {
                                this.responseDic.Add(parsingSessionID, mapiResponse);
                                this.responseBytesForHexview.Add(parsingSessionID, bytesForHexView);
                            }
                            else
                            {
                                if (!this.handleGetDic.ContainsKey(parsingSessionID))
                                {
                                    if ((mapiResponse as ExecuteResponseBody).RopBuffer != null && (mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Count() != 0)
                                    {
                                        this.handleGetDic.Add(parsingSessionID, ((ROPOutputBuffer_WithoutCROPS)(mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[0].Payload).ServerObjectHandleTable.ToList());
                                    }
                                }
                            }
                        }
                        else if (currentSession.responseBodyBytes.Length != 0 && mapiResponse.GetType().Name == "ExecuteResponseBody" && this.responseDic != null && this.responseDic.ContainsKey(parsingSessionID) && !IsOnlyGetServerHandle)
                        {
                            this.responseDic.Remove(parsingSessionID);
                            this.responseDic.Add(parsingSessionID, mapiResponse);
                        }
                    }
                }
            }
            else
            {
                if (IsMapihttpSession(currentSession, TrafficDirection.Out) && currentSession["X-ResponseCode"] == "0")
                {
                    NeedToParseCROPSLayer = isLooper;
                    byte[] bytesForHexView;
                    mapiResponse = this.ParseHTTPPayload(currentSession.ResponseHeaders, currentSession, currentSession.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                    hexViewBytes = bytesForHexView;
                    int parsingSessionID = currentSession.id;
                    if (currentSession.id == 0)
                    {
                        parsingSessionID = int.Parse(currentSession["VirtualID"]);
                    }
                    if (mapiResponse != null)
                    {
                        if (currentSession.responseBodyBytes.Length != 0 && mapiResponse.GetType().Name == "ExecuteResponseBody" && this.responseDic != null && !this.responseDic.ContainsKey(parsingSessionID))
                        {
                            if (!IsOnlyGetServerHandle)
                            {
                                this.responseDic.Add(parsingSessionID, mapiResponse);
                                this.responseBytesForHexview.Add(parsingSessionID, bytesForHexView);
                            }
                            else
                            {
                                if (!this.handleGetDic.ContainsKey(parsingSessionID))
                                {
                                    if ((mapiResponse as ExecuteResponseBody).RopBuffer != null && (mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Count() != 0)
                                    {
                                        this.handleGetDic.Add(parsingSessionID, ((ROPOutputBuffer_WithoutCROPS)(mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[0].Payload).ServerObjectHandleTable.ToList());
                                    }
                                }
                            }
                        }
                        else if (currentSession.responseBodyBytes.Length != 0 && mapiResponse.GetType().Name == "ExecuteResponseBody" && this.responseDic != null && this.responseDic.ContainsKey(parsingSessionID) && !IsOnlyGetServerHandle)
                        {
                            this.responseDic.Remove(parsingSessionID);
                            this.responseDic.Add(parsingSessionID, mapiResponse);
                        }
                    }
                }
            }


            if (isLooper)
            {
                NeedToParseCROPSLayer = false;
            }

            return mapiResponse;
        }

        /// <summary>
        /// Parse the HTTP payload to MAPI message.
        /// </summary>
        /// <param name="headers">The HTTP header.</param>
        /// <param name="currentSession">the current session.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <param name="bytes">The bytes provided for MAPI view layer.</param>
        /// <returns>The object parsed result</returns>
        public object ParseHTTPPayload(HTTPHeaders headers, Session currentSession, byte[] bytesFromHTTP, TrafficDirection direction, out byte[] bytes)
        {
            object objectOut = null;
            byte[] emptyByte = new byte[0];
            bytes = emptyByte;
            string requestType = string.Empty;

            if (!MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
            {
                if (bytesFromHTTP == null || bytesFromHTTP.Length == 0)
                {
                    return "Payload length from HTTP layer is 0";
                }
                else if (headers == null || !headers.Exists("X-RequestType"))
                {
                    return "X-RequestType header does not exist.";
                }

                requestType = headers["X-RequestType"];

                if (requestType == null)
                {
                    return "Request type is null";
                }
            }
            else
            {
                if (bytesFromHTTP == null || bytesFromHTTP.Length == 0)
                {
                    return "Payload length from HTTP layer is 0";
                }
                else if (headers == null || !currentSession.RequestHeaders.Exists("X-RequestType"))
                {
                    return "X-RequestType header does not exist.";
                }

                requestType = currentSession.RequestHeaders["X-RequestType"];

                if (requestType == null)
                {
                    return "Request type is null";
                }
            }

            try
            {
                if (direction == TrafficDirection.Out && MapiInspector.MAPIInspector.IsFromFiddlerCore(currentSession))
                {
                    if (currentSession["Transfer-Encoding"] != null && currentSession["Transfer-Encoding"] == "chunked")
                    {
                        bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                        bytes = bytesFromHTTP;
                    }
                }
                else if (direction == TrafficDirection.Out && headers.Exists("Transfer-Encoding") && headers["Transfer-Encoding"] == "chunked")
                {
                    bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                    bytes = bytesFromHTTP;
                }
                else
                {
                    bytes = bytesFromHTTP;
                }

                Stream stream = new MemoryStream(bytesFromHTTP);
                MAPIInspector.ParsingSession = currentSession;

                if (direction == TrafficDirection.In)
                {
                    switch (requestType)
                    {
                        case "Connect":
                            {
                                ConnectRequestBody connectRequest = new ConnectRequestBody();
                                connectRequest.Parse(stream);
                                objectOut = connectRequest;
                                break;
                            }

                        case "Execute":
                            {
                                ExecuteRequestBody executeRequest = new ExecuteRequestBody();
                                executeRequest.Parse(stream);
                                objectOut = executeRequest;
                                break;
                            }

                        case "Disconnect":
                            {
                                DisconnectRequestBody disconnectRequest = new DisconnectRequestBody();
                                disconnectRequest.Parse(stream);
                                objectOut = disconnectRequest;
                                break;
                            }

                        case "NotificationWait":
                            {
                                NotificationWaitRequestBody notificationWaitRequest = new NotificationWaitRequestBody();
                                notificationWaitRequest.Parse(stream);
                                objectOut = notificationWaitRequest;
                                break;
                            }

                        case "Bind":
                            {
                                BindRequest bindRequest = new BindRequest();
                                bindRequest.Parse(stream);
                                objectOut = bindRequest;
                                break;
                            }

                        case "Unbind":
                            {
                                UnbindRequest unbindRequest = new UnbindRequest();
                                unbindRequest.Parse(stream);
                                objectOut = unbindRequest;
                                break;
                            }

                        case "CompareMIds":
                            {
                                CompareMinIdsRequest compareMinIdsRequest = new CompareMinIdsRequest();
                                compareMinIdsRequest.Parse(stream);
                                objectOut = compareMinIdsRequest;
                                break;
                            }

                        case "DNToMId":
                            {
                                DnToMinIdRequest dnToMinIdRequest = new DnToMinIdRequest();
                                dnToMinIdRequest.Parse(stream);
                                objectOut = dnToMinIdRequest;
                                break;
                            }

                        case "GetMatches":
                            {
                                GetMatchesRequest getMatchesRequest = new GetMatchesRequest();
                                getMatchesRequest.Parse(stream);
                                objectOut = getMatchesRequest;
                                break;
                            }

                        case "GetPropList":
                            {
                                GetPropListRequest getPropListRequest = new GetPropListRequest();
                                getPropListRequest.Parse(stream);
                                objectOut = getPropListRequest;
                                break;
                            }

                        case "GetProps":
                            {
                                GetPropsRequest getPropsRequest = new GetPropsRequest();
                                getPropsRequest.Parse(stream);
                                objectOut = getPropsRequest;
                                break;
                            }

                        case "GetSpecialTable":
                            {
                                GetSpecialTableRequest getSpecialTableRequest = new GetSpecialTableRequest();
                                getSpecialTableRequest.Parse(stream);
                                objectOut = getSpecialTableRequest;
                                break;
                            }

                        case "GetTemplateInfo":
                            {
                                GetTemplateInfoRequest getTemplateInfoRequest = new GetTemplateInfoRequest();
                                getTemplateInfoRequest.Parse(stream);
                                objectOut = getTemplateInfoRequest;
                                break;
                            }

                        case "ModLinkAtt":
                            {
                                ModLinkAttRequest modLinkAttRequest = new ModLinkAttRequest();
                                modLinkAttRequest.Parse(stream);
                                objectOut = modLinkAttRequest;
                                break;
                            }

                        case "ModProps":
                            {
                                ModPropsRequest modPropsRequest = new ModPropsRequest();
                                modPropsRequest.Parse(stream);
                                objectOut = modPropsRequest;
                                break;
                            }

                        case "QueryRows":
                            {
                                QueryRowsRequest queryRowsRequest = new QueryRowsRequest();
                                queryRowsRequest.Parse(stream);
                                objectOut = queryRowsRequest;
                                break;
                            }

                        case "QueryColumns":
                            {
                                QueryColumnsRequest queryColumnsRequest = new QueryColumnsRequest();
                                queryColumnsRequest.Parse(stream);
                                objectOut = queryColumnsRequest;
                                break;
                            }

                        case "ResolveNames":
                            {
                                ResolveNamesRequest resolveNamesRequest = new ResolveNamesRequest();
                                resolveNamesRequest.Parse(stream);
                                objectOut = resolveNamesRequest;
                                break;
                            }

                        case "ResortRestriction":
                            {
                                ResortRestrictionRequest resortRestrictionRequest = new ResortRestrictionRequest();
                                resortRestrictionRequest.Parse(stream);
                                objectOut = resortRestrictionRequest;
                                break;
                            }

                        case "SeekEntries":
                            {
                                SeekEntriesRequest seekEntriesRequest = new SeekEntriesRequest();
                                seekEntriesRequest.Parse(stream);
                                objectOut = seekEntriesRequest;
                                break;
                            }

                        case "UpdateStat":
                            {
                                UpdateStatRequest updateStatRequest = new UpdateStatRequest();
                                updateStatRequest.Parse(stream);
                                objectOut = updateStatRequest;
                                break;
                            }

                        case "GetMailboxUrl":
                            {
                                GetMailboxUrlRequest getMailboxUrlRequest = new GetMailboxUrlRequest();
                                getMailboxUrlRequest.Parse(stream);
                                objectOut = getMailboxUrlRequest;
                                break;
                            }

                        case "GetAddressBookUrl":
                            {
                                GetAddressBookUrlRequest getAddressBookUrlRequest = new GetAddressBookUrlRequest();
                                getAddressBookUrlRequest.Parse(stream);
                                objectOut = getAddressBookUrlRequest;
                                break;
                            }

                        default:
                            {
                                objectOut = "Unavailable Request Type";
                                break;
                            }
                    }
                    if (AllRopsList.Count <= 0 || !AllRopsList.Contains(requestType + "Request"))
                    {
                        AllRopsList.Add(requestType + "Request");
                    }
                }
                else
                {
                    switch (requestType)
                    {
                        case "Connect":
                            {
                                ConnectResponseBody connectResponse = new ConnectResponseBody();
                                connectResponse.Parse(stream);
                                objectOut = connectResponse;
                                break;
                            }

                        case "Execute":
                            {
                                ExecuteResponseBody executeResponse = new ExecuteResponseBody();
                                executeResponse.Parse(stream);
                                objectOut = executeResponse;
                                break;
                            }

                        case "Disconnect":
                            {
                                DisconnectResponseBody disconnectResponse = new DisconnectResponseBody();
                                disconnectResponse.Parse(stream);
                                objectOut = disconnectResponse;
                                break;
                            }

                        case "NotificationWait":
                            {
                                NotificationWaitResponseBody notificationWaitResponse = new NotificationWaitResponseBody();
                                notificationWaitResponse.Parse(stream);
                                objectOut = notificationWaitResponse;
                                break;
                            }

                        case "Bind":
                            {
                                BindResponse bindResponse = new BindResponse();
                                bindResponse.Parse(stream);
                                objectOut = bindResponse;
                                break;
                            }

                        case "Unbind":
                            {
                                UnbindResponse unbindResponse = new UnbindResponse();
                                unbindResponse.Parse(stream);
                                objectOut = unbindResponse;
                                break;
                            }

                        case "CompareMIds":
                            {
                                CompareMinIdsResponse compareMinIdsResponse = new CompareMinIdsResponse();
                                compareMinIdsResponse.Parse(stream);
                                objectOut = compareMinIdsResponse;
                                break;
                            }

                        case "DNToMId":
                            {
                                DnToMinIdResponse dnToMinIdResponse = new DnToMinIdResponse();
                                dnToMinIdResponse.Parse(stream);
                                objectOut = dnToMinIdResponse;
                                break;
                            }

                        case "GetMatches":
                            {
                                GetMatchesResponse getMatchesResponse = new GetMatchesResponse();
                                getMatchesResponse.Parse(stream);
                                objectOut = getMatchesResponse;
                                break;
                            }

                        case "GetPropList":
                            {
                                GetPropListResponse getPropListResponse = new GetPropListResponse();
                                getPropListResponse.Parse(stream);
                                objectOut = getPropListResponse;
                                break;
                            }

                        case "GetProps":
                            {
                                GetPropsResponse getPropsResponse = new GetPropsResponse();
                                getPropsResponse.Parse(stream);
                                objectOut = getPropsResponse;
                                break;
                            }

                        case "GetSpecialTable":
                            {
                                GetSpecialTableResponse getSpecialTableResponse = new GetSpecialTableResponse();
                                getSpecialTableResponse.Parse(stream);
                                objectOut = getSpecialTableResponse;
                                break;
                            }

                        case "GetTemplateInfo":
                            {
                                GetTemplateInfoResponse getTemplateInfoResponse = new GetTemplateInfoResponse();
                                getTemplateInfoResponse.Parse(stream);
                                objectOut = getTemplateInfoResponse;
                                break;
                            }

                        case "ModLinkAtt":
                            {
                                ModLinkAttResponse modLinkAttResponse = new ModLinkAttResponse();
                                modLinkAttResponse.Parse(stream);
                                objectOut = modLinkAttResponse;
                                break;
                            }

                        case "ModProps":
                            {
                                ModPropsResponse modPropsResponse = new ModPropsResponse();
                                modPropsResponse.Parse(stream);
                                objectOut = modPropsResponse;
                                break;
                            }

                        case "QueryRows":
                            {
                                QueryRowsResponse queryRowsResponse = new QueryRowsResponse();
                                queryRowsResponse.Parse(stream);
                                objectOut = queryRowsResponse;
                                break;
                            }

                        case "QueryColumns":
                            {
                                QueryColumnsResponse queryColumnsResponse = new QueryColumnsResponse();
                                queryColumnsResponse.Parse(stream);
                                objectOut = queryColumnsResponse;
                                break;
                            }

                        case "ResolveNames":
                            {
                                ResolveNamesResponse resolveNamesResponse = new ResolveNamesResponse();
                                resolveNamesResponse.Parse(stream);
                                objectOut = resolveNamesResponse;
                                break;
                            }

                        case "ResortRestriction":
                            {
                                ResortRestrictionResponse resortRestrictionResponse = new ResortRestrictionResponse();
                                resortRestrictionResponse.Parse(stream);
                                objectOut = resortRestrictionResponse;
                                break;
                            }

                        case "SeekEntries":
                            {
                                SeekEntriesResponse seekEntriesResponse = new SeekEntriesResponse();
                                seekEntriesResponse.Parse(stream);
                                objectOut = seekEntriesResponse;
                                break;
                            }

                        case "UpdateStat":
                            {
                                UpdateStatResponse updateStatResponse = new UpdateStatResponse();
                                updateStatResponse.Parse(stream);
                                objectOut = updateStatResponse;
                                break;
                            }

                        case "GetMailboxUrl":
                            {
                                GetMailboxUrlResponse getMailboxUrlResponse = new GetMailboxUrlResponse();
                                getMailboxUrlResponse.Parse(stream);
                                objectOut = getMailboxUrlResponse;
                                break;
                            }

                        case "GetAddressBookUrl":
                            {
                                GetAddressBookUrlResponse getAddressBookUrlResponse = new GetAddressBookUrlResponse();
                                getAddressBookUrlResponse.Parse(stream);
                                objectOut = getAddressBookUrlResponse;
                                break;
                            }

                        default:
                            {
                                objectOut = "Unavailable Response Type";
                                break;
                            }
                    }
                    if (AllRopsList.Count <= 0 || !AllRopsList.Contains(requestType + "Response"))
                    {
                        AllRopsList.Add(requestType + "Response");
                    }
                }

                return objectOut;
            }
            catch (MissingInformationException missingException)
            {
                DecodingContext.LogonFlagMapLogId = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>>();
                this.HandleContextInformation(missingException.RopID, out objectOut, out bytes, missingException.Parameters);
                return objectOut;
            }
            catch (MissingPartialInformationException missingPartialException)
            {
                objectOut = this.Partial(missingPartialException.RopID, missingPartialException.Parameter, out bytes);
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
        /// <param name="bytesForHexview">The byte array provided for HexView</param>
        public void DisplayObject(object obj, byte[] bytesForHexview)
        {
            if (obj == null)
            {
                return;
            }
            else if (obj.GetType().Name == "String")
            {
                this.MAPIViewControl.BeginUpdate();
                this.MAPIControl.MAPIRichTextBox.Visible = true;
                this.MAPIControl.MAPIRichTextBox.Text = obj.ToString();
                this.MAPIViewControl.EndUpdate();
            }
            else
            {
                this.MAPIViewControl.BeginUpdate();
                int result = 0;

                try
                {
                    TreeNode topNode = BaseStructure.AddNodesForTree(obj, 0, out result);
                    this.MAPIViewControl.Nodes.Add(topNode);
                    topNode.ExpandAll();
                    this.MAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(bytesForHexview);
                    this.MAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
                    if (this.MAPIViewControl.Nodes.Count != 0)
                    {
                        this.MAPIViewControl.Nodes[0].EnsureVisible();
                    }
                }
                catch (Exception e)
                {
                    this.MAPIControl.MAPIRichTextBox.Visible = true;
                    this.MAPIControl.MAPIRichTextBox.Text = e.Message;
                }
                finally
                {
                    this.MAPIViewControl.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Clean parsed session related dictionaries
        /// </summary>
        public void ResetHanleInformation()
        {
            this.requestDic = new Dictionary<int, object>();
            this.responseDic = new Dictionary<int, object>();
            this.handleGetDic = new Dictionary<int, List<uint>>();
            this.handlePutDic = new Dictionary<int, List<uint>>();
        }

        /// <summary>
        /// Clean partial fast transfer stream related dictionaries
        /// </summary>
        public void ResetPartialContextInformation()
        {
            HandleWithSessionGetContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();
            HandleWithSessionPutContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();
            HandleWithSessionPutExtendContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();
        }

        /// <summary>
        /// Empty the partial related parameters information
        /// </summary>
        public void ResetPartialParameters()
        {
            // Empty the partial parameters of RopGetBuffer
            PartialGetType = 0;
            PartialGetId = 0;
            PartialGetRemainSize = -1;
            PartialGetSubRemainSize = -1;
            IsGet = false;

            // Empty the partial parameters of RopPutBuffer
            PartialPutType = 0;
            PartialPutId = 0;
            PartialPutRemainSize = -1;
            PartialPutSubRemainSize = -1;
            IsPut = false;

            // Empty the partial parameters of RopPutExtendedBuffer
            PartialPutExtendType = 0;
            PartialPutExtendId = 0;
            PartialPutExtendRemainSize = -1;
            PartialPutExtendSubRemainSize = -1;
            IsPutExtend = false;
        }

        /// <summary>
        /// Reset public parameters.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        public void AfterCallDoImport(object sender, EventArgs e)
        {
            this.ResetHanleInformation();
            this.ResetPartialContextInformation();
            this.ResetPartialParameters();
        }

        /// <summary>
        /// Add a index feature for session
        /// </summary>
        public void SetIndexForContextRelatedMethods()
        {
            for (int i = 0; i < AllSessions.Length; i++)
            {
                AllSessions[i]["Number"] = i.ToString();
            }
        }

        /// <summary>
        /// Update the view with parsed and diagnosed data
        /// </summary>
        private void UpdateView()
        {
            this.Clear();
            byte[] bytesForHexView;
            object parserResult;
            IsLooperCall = false;
            TargetHandle = new Stack<Dictionary<ushort, Dictionary<int, uint>>>();
            ContextInformationCollection = new List<ContextInformation>();
            this.ResetPartialParameters();

            if (this.IsMapihttp)
            {
                List<Session> allSessionsList = new List<Session>();
                Session session0 = new Session(new byte[0], new byte[0]);
                Session[] sessionsInFiddler = FiddlerApplication.UI.GetAllSessions();
                allSessionsList.AddRange(sessionsInFiddler);
                FiddlerApplication.OnLoadSAZ += this.AfterCallDoImport;
                allSessionsList.Sort(delegate (Session p1, Session p2)
                {
                    return p1.id.CompareTo(p2.id);
                });
                allSessionsList.Insert(0, session0);
                AllSessions = allSessionsList.ToArray();
                int allSessionLength = AllSessions.Length;

                if (allSessionLength > 0 && AllSessions[allSessionLength - 1]["Number"] == null)
                {
                    this.SetIndexForContextRelatedMethods();
                }

                try
                {
                    if (this.Direction == TrafficDirection.In)
                    {
                        parserResult = this.ParseHTTPPayload(this.BaseHeaders, this.session, this.session.requestBodyBytes, TrafficDirection.In, out bytesForHexView);
                    }
                    else
                    {
                        // An X-ResponseCode of 0 (zero) means success from the perspective of the protocol transport, and the client SHOULD parse the response body based on the request that was issued.
                        if (this.BaseHeaders["X-ResponseCode"] != "0")
                        {
                            return;
                        }

                        parserResult = this.ParseHTTPPayload(this.BaseHeaders, this.session, this.session.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                    }

                    this.DisplayObject(parserResult, bytesForHexView);
                }
                catch (Exception e)
                {
                    parserResult = e.ToString();
                }
                finally
                {
                    DecodingContext.Notify_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>>();
                    DecodingContext.RowRops_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>>();
                    TargetHandle = new Stack<Dictionary<ushort, Dictionary<int, uint>>>();
                    ContextInformationCollection = new List<ContextInformation>();
                    IsLooperCall = true;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Method to judge whether a session is from FiddlerCore or FiddlerExe
        /// </summary>
        /// <param name="currentSession">The session to be judged</param>
        /// <returns>Boole value indicates whether this session is from FiddlerCore or not</returns>
        public static bool IsFromFiddlerCore(Session session)
        {
            bool result = false;
            if (session.id == 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method to judge whether a session is MAPIHTTP message or not when Automation Test
        /// </summary>        
        /// <returns>Boole value indicates whether this session is MAPIHTTP layer message</returns>
        public bool IsMapihttpWithoutUI()
        {
            if (MapiInspector.MAPIInspector.ParsingSession != null)
            {
                return MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
            }
            if (MapiInspector.MAPIInspector.ParsingSession["X-ResponseCode"] != null)
            {
                string a = MapiInspector.MAPIInspector.ParsingSession["X-ResponseCode"];
                if (a == "0")
                {
                    return MapiInspector.MAPIInspector.ParsingSession["Content-Type"] != null && MapiInspector.MAPIInspector.ParsingSession["Content-Type"] == "application/mapi-http";
                }
                if (a != "")
                {
                    return MapiInspector.MAPIInspector.ParsingSession["Content-Type"] != null && MapiInspector.MAPIInspector.ParsingSession["Content-Type"] == "text/html";
                }
            }
            return false;
        }

        /// <summary>
        /// Parse the sessions from capture file using the MAPI Inspector
        /// </summary>
        /// <param name="sessionsFromCore">The sessions which from FiddlerCore to parse</param>
        /// <param name="pathName">The path for save result file</param>
        /// <param name="autoCaseName">The test case name to parse</param>
        /// <param name="allRops">All ROPs contained in list</param>
        /// <returns>Parse result, true means success</returns>
        public bool ParseCaptureFile(Fiddler.Session[] sessionsFromCore, string pathName, string autoCaseName, out List<string> allRops)
        {
            var errorStringList = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            AllSessions = sessionsFromCore;
            DecodingContext decodingContext = new DecodingContext();
            ResetPartialParameters();
            ResetPartialContextInformation();
            ResetHanleInformation();
            for (int i = 0; i < AllSessions.Length; i++)
            {
                var session = AllSessions[i];
                Session val = AllSessions[i];
                if (AllSessions[i]["VirtualID"] != null)
                {
                    MapiInspector.MAPIInspector.ParsingSession = val;
                }
                if (AllSessions.Length > 0 && AllSessions[AllSessions.Length - 1]["Number"] == null)
                {
                    SetIndexForContextRelatedMethods();
                }
                if (IsMapihttpWithoutUI())
                {
                    try
                    {
                        MapiInspector.MAPIInspector.IsLooperCall = false;
                        ResetPartialParameters();
                        BaseHeaders = val.RequestHeaders;
                        byte[] bytes;
                        object obj = ParseHTTPPayload(BaseHeaders, val, val.requestBodyBytes, TrafficDirection.In, out bytes);
                        JsonResult += Utilities.ConvertCSharpToJson(i, isRequest: true, obj);
                        if (val["X-ResponseCode"] == "0")
                        {
                            object obj2 = ParseHTTPPayload(BaseHeaders, val, val.responseBodyBytes, TrafficDirection.Out, out bytes);
                            JsonResult += Utilities.ConvertCSharpToJson(i, isRequest: false, obj2);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorStringList.Add(string.Format("{0}. Error: Frame#{1} Error Message:{2}", errorStringList.Count + 1, val["VirtualID"], ex.Message));
                    }
                    finally
                    {
                        DecodingContext.Notify_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>>();
                        DecodingContext.RowRops_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>>();
                        TargetHandle = new Stack<Dictionary<ushort, Dictionary<int, uint>>>();
                        ContextInformationCollection = new List<ContextInformation>();
                        MapiInspector.MAPIInspector.IsLooperCall = true;
                    }
                }
                if (i % 10 == 0 && JsonResult.Length != 0)
                {
                    string path = pathName + Path.DirectorySeparatorChar.ToString() + autoCaseName + "-" + JsonFile;
                    if (!File.Exists(path))
                    {
                        using (StreamWriter streamWriter = File.CreateText(path))
                        {
                            streamWriter.WriteLine(JsonResult);
                        }
                    }
                    else
                    {
                        using (StreamWriter streamWriter2 = File.AppendText(path))
                        {
                            streamWriter2.WriteLine(JsonResult);
                        }
                    }
                    JsonResult = string.Empty;
                }
            }
            allRops = AllRopsList;
            foreach (string errorString in errorStringList)
            {
                stringBuilder.AppendLine(errorString);
            }
            if (stringBuilder.Length != 0)
            {
                string path2 = pathName + Path.DirectorySeparatorChar.ToString() + autoCaseName + "-" + ErrorFile;
                File.WriteAllText(path2, stringBuilder.ToString());
                return false;
            }
            return true;
        }
    }
}