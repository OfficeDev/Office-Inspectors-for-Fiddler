using System.IO;
using System.Windows.Forms;
using Fiddler;
using MAPIInspector.Parsers;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;


namespace MapiInspector
{
    public abstract class MAPIInspector : Inspector2
    {
        /// <summary>
        /// Gets or sets the Tree View control where displayed the MAPI message.
        /// </summary>
        public TreeView oMAPIViewControl { get; set; }

        /// <summary>
        /// Gets or sets the control collection where displayed the MAPI parsed message and corresponding hex data.
        /// </summary>
        public MAPIControl oMAPIControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame has been changed.
        /// </summary>
        public bool bDirty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame is read-only.
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
        /// Gets or sets the ROPInputBuffer or ROPOutputBuffer payload for compressed or xor
        /// </summary>
        public static byte[] payLoadCompresssedXOR { get; set; }

        /// <summary>
        /// Gets or sets the AuxiliaryBufferPayload payload for compressed or xor
        /// </summary>
        public static byte[] auxPayLoadCompresssedXOR { get; set; }

        /// <summary>
        /// Gets or sets the current selected session in fiddler.
        /// </summary>
        public static Session selectedSession;

        /// <summary>
        /// Gets or sets the parsing session in fiddler.
        /// </summary>
        public static Session parsingSession;
        
        /// <summary>
        /// Record all sessions in Fiddler.
        /// </summary>
        public static Session[] allSessions;

        /// <summary>
        /// Record the lastContextSession
        /// </summary>
        public static int? theLastContextSession = null;

        /// <summary>
        /// The requestDic is used to save the session id and its parsed execute request.
        /// </summary>
        private Dictionary<int, object> requestDic = new Dictionary<int, object>();

        /// <summary>
        /// The responseDic is used to save the session id and its parsed execute response.
        /// </summary>
        private Dictionary<int, object> responseDic = new Dictionary<int, object>();

        /// <summary>
        /// The requestBytesForHexview is used to save the session id and its parsed request bytes provided for MAPIHexBox.
        /// </summary>
        private Dictionary<int, byte[]> requestBytesForHexview = new Dictionary<int, byte[]>();

        /// <summary>
        /// The responseBytesForHexview is used to save the session id and its parsed response bytes provided for MAPIHexBox.
        /// </summary>
        private Dictionary<int, byte[]> responseBytesForHexview = new Dictionary<int, byte[]>();

        /// <summary>
        /// The decompressedRequestForHexview is used to save the session id and its parsed request bytes provided for CROPSHexBox.
        /// </summary>
        private Dictionary<int, byte[]> decompressedRequestForHexview = new Dictionary<int, byte[]>();

        /// <summary>
        /// The decompressedRequestForHexview is used to save the session id and its parsed response bytes provided for CROPSHexBox.
        /// </summary>
        private Dictionary<int, byte[]> decompressedResponseForHexview = new Dictionary<int, byte[]>();

        /// <summary>
        /// The targetHandle is used to record the session id and its object handle before a loopr parsing for context session
        /// </summary>
        public static Stack<Dictionary<int, uint>> targetHandle = new Stack<Dictionary<int, uint>>();
        
        /// <summary>
        /// The ContextInformationCollection is used to record current session for all of the context information results.
        /// </summary>
        public static List<ContextInformation> ContextInformationCollection = new List<ContextInformation>();

        /// <summary>
        /// Indicate whether the current parsing session is in the looper of the context session parsing
        /// </summary>
        public static bool isLooperCall = false;

        /// <summary>
        /// Indicate whether the current parsing session is need to parse crops layer
        /// </summary>
        public static bool needToParseCROPSLayer = false;

        /// <summary>
        /// Indicate whether this session is only for getting the server object handle
        /// </summary>
        public static bool isOnlyGetServerHandle = false;

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
        /// Gets whether the message is MAPI protocol message.
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
                    else if ((this is IResponseInspector2) && this.session.ResponseHeaders.Exists("X-ResponseCode"))
                    {
                        string xResponseCode = this.session.ResponseHeaders["X-ResponseCode"];
                        if (xResponseCode == "0")
                        {
                            return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                        }
                        else if (xResponseCode != "")
                        {
                            return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "text/html");
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Called by Fiddler to add the MAPI inspector tab
        /// </summary>
        /// <param name="o">The tab control for the inspector</param>
        public override void AddToTab(TabPage o)
        {
            o.Text = "MAPI";
            this.oMAPIControl = new MAPIControl();
            o.Controls.Add(this.oMAPIControl);
            this.oMAPIControl.Size = o.Size;
            this.oMAPIControl.Dock = DockStyle.Fill;
            this.oMAPIViewControl = this.oMAPIControl.MAPITreeView;
            this.oMAPIControl.MAPIHexBox.VScrollBarVisible = true;
            this.oMAPIViewControl.AfterSelect -= TreeView_AfterSelect;
            this.oMAPIViewControl.AfterSelect += TreeView_AfterSelect;
            DecodingContext dc = new DecodingContext();
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
                this.oMAPIControl.MAPIHexBox.Select(0, 0);
                this.oMAPIControl.CROPSHexBox.Select(0, 0);
            }
            else
            {
                if (((BaseStructure.Position)e.Node.Tag).IsCompressedXOR)
                {
                    if (((BaseStructure.Position)e.Node.Tag).IsAuxiliayPayload)
                    {
                        this.oMAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(auxPayLoadCompresssedXOR);
                    }
                    else
                    {
                        this.oMAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(payLoadCompresssedXOR);
                    }
                    this.oMAPIControl.CROPSHexBox.Select(((BaseStructure.Position)e.Node.Tag).StartIndex, ((BaseStructure.Position)e.Node.Tag).Offset);
                    this.oMAPIControl.MAPIHexBox.Select(0, 0);
                    this.oMAPIControl.CROPSHexBox.Visible = true;
                    ToolTip ToolTip = new ToolTip();
                    ToolTip.SetToolTip(this.oMAPIControl.CROPSHexBox, "This is decompressed payload data.");
                    this.oMAPIControl.SplitContainer.Panel2Collapsed = false;
                }
                else
                {
                    this.oMAPIControl.MAPIHexBox.Select(((BaseStructure.Position)e.Node.Tag).StartIndex, ((BaseStructure.Position)e.Node.Tag).Offset);
                    this.oMAPIControl.CROPSHexBox.Visible = false;
                    this.oMAPIControl.SplitContainer.Panel2Collapsed = true;
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
            this.oMAPIViewControl.Nodes.Clear();
            this.oMAPIControl.MAPIRichTextBox.Visible = false;
            this.oMAPIControl.MAPIRichTextBox.Clear();
            this.oMAPIControl.CROPSHexBox.Visible = false;
            byte[] empty = new byte[0];
            this.oMAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(empty);
            this.oMAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
            this.oMAPIControl.SplitContainer.Panel2Collapsed = true;
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
        /// <param name="parameters">The missing context information ROP related parameters</param>
        /// <param name="obj">The target object containing the context information</param>
        /// <param name="bytes">The target byte array provided to Hexview</param>
        public void HandleContextInformation(ushort sourceRopID, out object obj, out byte[] bytes, uint[] parameters = null)
        {
            List<Session> AllSessionsList = new List<Session>();
            Session session0 = new Session(new byte[0], new byte[0]);
            AllSessionsList.AddRange(FiddlerApplication.UI.GetAllSessions());
            AllSessionsList.Sort(delegate (Session p1, Session p2)
            {
                return p1.id.CompareTo(p2.id);
            });
            AllSessionsList.Insert(0, session0);
            Session[] allSessions = AllSessionsList.ToArray();
            byte[] bytesForHexView;
            object MAPIRequest = new object();
            object MAPIResponse = new object();
            string savedResult = string.Empty;
            object result = new object();
            Session ThisSession = MAPIInspector.parsingSession;

            if ((RopIdType)sourceRopID == RopIdType.RopLogon)
            {
                ParseRequestMessage(ThisSession, true);
                obj = ParseResponseMessage(ThisSession, out bytesForHexView, true);
                bytes = bytesForHexView;
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopSetMessageReadFlag)
            {
                Session currentSession = allSessions[AllSessionsList.IndexOf(ThisSession) - 1];
                if (parameters != null && parameters.Length > 0)
                {
                    // parsing the previous sessions until DecodingContext.LogonFlagMapLogId contains the Logon Id in this RopSetMessageReadFlag rop. 
                    Dictionary<int, uint> dic = new Dictionary<int, uint>();
                    dic.Add(ThisSession.id, parameters[0]);
                    targetHandle.Push(dic);
                    do
                    {
                        if (IsMapihttpSession(currentSession, TrafficDirection.In))
                        {
                            ParseRequestMessage(currentSession, true);
                        }
                        if (AllSessionsList.IndexOf(currentSession) == 0)
                        {
                            break;
                        }
                        else
                        {
                            currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                        }
                    }
                    while (DecodingContext.LogonFlagMapLogId.Count == 0 || !DecodingContext.LogonFlagMapLogId.ContainsKey((byte)parameters[0]));
                    
                    if (DecodingContext.LogonFlagMapLogId.ContainsKey((byte)parameters[0]))
                    {
                        result = DecodingContext.LogonFlagMapLogId[(byte)parameters[0]];
                    }
                    else
                    {
                        result = string.Format("{0} cannot be parsed successfully due to missing the Logon information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[0]);
                    }

                    if (targetHandle.Count == 1)
                    {
                        ContextInformation information = new ContextInformation();
                        information.RopID = (RopIdType)sourceRopID;
                        information.Handle = parameters[0];
                        information.RelatedInformation = result;
                        ContextInformationCollection.Add(information);
                        if (!OverwriteOriginalInformation(ThisSession.id, out savedResult))
                        {
                            obj = savedResult;
                            bytes = new byte[0];
                            return;
                        }                      
                    }
                    targetHandle.Pop();
                }

                // Add this session id(RopSetMessageReadFlag Rop)in DecodingContext.SessionLogonFlagMapLogId.
                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(ThisSession.id)))
                {
                    DecodingContext.SessionLogonFlagMapLogId.Add(ThisSession.id, DecodingContext.LogonFlagMapLogId);
                }

                if (DecodingContext.LogonFlagMapLogId.ContainsKey((byte)parameters[0]))
                {
                    // Parsing the request structure of this session.
                    ParseRequestMessage(ThisSession, true);
                    obj = requestDic[ThisSession.id];
                    bytes = requestBytesForHexview[ThisSession.id];
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the Logon information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[0]);
                    bytes = new byte[0];
                }
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopFastTransferSourceGetBuffer)
            {
                Session currentSession = allSessions[AllSessionsList.IndexOf(ThisSession) - 1];
                DecodingContext.StreamType_Getbuffer = 0;
                Dictionary<int, uint> dic_Getbuffer = new Dictionary<int, uint>();
                dic_Getbuffer.Add((int)ThisSession.id, parameters[1]);
                targetHandle.Push(dic_Getbuffer);
                if (parameters != null && parameters.Length > 1)
                {
                    if ((DecodingContext.CopyTo_OutputHandles.Count > 0 && DecodingContext.CopyTo_OutputHandles.Contains(parameters[1])) ||
                       (DecodingContext.CopyProperties_OutputHandles.Count > 0 && DecodingContext.CopyProperties_OutputHandles.Contains(parameters[1])) ||
                       (DecodingContext.SyncConfigure_OutputHandles.Count > 0 && DecodingContext.SyncConfigure_OutputHandles.Contains(parameters[1])) ||
                       (DecodingContext.CopyFolder_OutputHandles.Count > 0 && DecodingContext.CopyFolder_OutputHandles.Contains(parameters[1])) ||
                       (DecodingContext.CopyMessage_OutputHandles.Count > 0 && DecodingContext.CopyMessage_OutputHandles.Contains(parameters[1])) ||
                       (DecodingContext.SyncGetTransferState_OutputHandles.Count > 0 && DecodingContext.SyncGetTransferState_OutputHandles.Contains(parameters[1])))
                    {
                        if (DecodingContext.CopyTo_OutputHandles.Count > 0 && DecodingContext.CopyTo_OutputHandles.Contains(parameters[1]))
                        {
                            // If CopyTo output handle is equal to the GetBuffer input handle, need to do further parse for CopyTo request.
                            ParseRequestMessage(ThisSession, true);
                            int CopyToRopNum = DecodingContext.CopyTo_OutputHandles.IndexOf(parameters[1]);
                            if (DecodingContext.FasttransterMid_InputIndexAndHandles.ContainsKey(0xffffffff))
                            {
                                ObjectHandlesType ObjectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                switch (ObjectHandleType)
                                {
                                    case ObjectHandlesType.FolderHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                        break;
                                    case ObjectHandlesType.MessageHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                        break;
                                    case ObjectHandlesType.AttachmentHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                        break;
                                    default:
                                        throw new Exception("The ObjectHandlesType is not right.");
                                }
                            }
                            else if (!DecodingContext.ObjectHandles.ContainsKey(DecodingContext.CopyTo_InputHandles[CopyToRopNum]) || DecodingContext.StreamType_Getbuffer == 0)
                            {
                                Dictionary<int, uint> dic_CopyTo = new Dictionary<int, uint>();
                                dic_CopyTo.Add((int)ThisSession.id, DecodingContext.CopyTo_InputHandles[CopyToRopNum]);
                                targetHandle.Push(dic_CopyTo);
                                uint searchCopyToHandle_SameSession = DecodingContext.CopyTo_InputHandles[CopyToRopNum];
                                do
                                {
                                    ParseResponseMessage(currentSession, out bytesForHexView);
                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                }
                                while (!DecodingContext.ObjectHandles.ContainsKey(searchCopyToHandle_SameSession));
                                targetHandle.Pop();

                                ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchCopyToHandle_SameSession];
                                switch (ObjectHandleType)
                                {
                                    case ObjectHandlesType.FolderHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                        break;
                                    case ObjectHandlesType.MessageHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                        break;
                                    case ObjectHandlesType.AttachmentHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                        break;
                                    default:
                                        throw new Exception("The ObjectHandlesType is not right.");
                                }
                            }
                        }
                        else if (DecodingContext.CopyProperties_OutputHandles.Count > 0 && DecodingContext.CopyProperties_OutputHandles.Contains(parameters[1]))
                        {
                            // If CopyProperties output handle is equal to the GetBuffer input handle, need to do further parse for CopyProperties request.
                            ParseRequestMessage(ThisSession, true);
                            int CopyPropertiesRopNum = DecodingContext.CopyProperties_OutputHandles.IndexOf(parameters[1]);

                            // when ObjectHandles contains object handle in copyProperties rop, the FastTransferStream type can be determined by the ObjectHandlesType.
                            if (DecodingContext.FasttransterMid_InputIndexAndHandles.ContainsKey(0xffffffff))
                            {
                                ObjectHandlesType ObjectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                switch (ObjectHandleType)
                                {
                                    case ObjectHandlesType.FolderHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                        break;
                                    case ObjectHandlesType.MessageHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                        break;
                                    case ObjectHandlesType.AttachmentHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                        break;
                                    default:
                                        throw new Exception("The ObjectHandlesType is not right.");
                                }
                            }
                            else if (!DecodingContext.ObjectHandles.ContainsKey(DecodingContext.CopyProperties_InputHandles[CopyPropertiesRopNum]) || DecodingContext.StreamType_Getbuffer == 0)
                            {
                                Dictionary<int, uint> dic_copyProperties = new Dictionary<int, uint>();
                                dic_copyProperties.Add((int)ThisSession.id, DecodingContext.CopyProperties_InputHandles[CopyPropertiesRopNum]);
                                targetHandle.Push(dic_copyProperties);
                                uint searchCopyPropertiesHandle_SameSession = DecodingContext.CopyProperties_InputHandles[CopyPropertiesRopNum];
                                do
                                {
                                    ParseResponseMessage(currentSession, out bytesForHexView);
                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                }
                                while (!DecodingContext.ObjectHandles.ContainsKey(searchCopyPropertiesHandle_SameSession));
                                targetHandle.Pop();

                                ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchCopyPropertiesHandle_SameSession];
                                switch (ObjectHandleType)
                                {
                                    case ObjectHandlesType.FolderHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                        break;
                                    case ObjectHandlesType.MessageHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                        break;
                                    case ObjectHandlesType.AttachmentHandles:
                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                        break;
                                    default:
                                        throw new Exception("The ObjectHandlesType is not right.");
                                }
                            }
                        }
                        else if (DecodingContext.SyncConfigure_OutputHandles.Count > 0 && DecodingContext.SyncConfigure_OutputHandles.Contains(parameters[1]))
                        {
                            // If SyncConfigure output handle is equal to the GetBuffer input handle, need to do further parse for CopyProperties request.
                            ParseRequestMessage(ThisSession, true);
                            obj = requestDic[ThisSession.id];

                            int SyncConfigureRopNum = DecodingContext.SyncConfigure_OutputHandles.IndexOf(parameters[1]);
                            int SyncConfigureRopNum_Current = 0;
                            int RopBufferCount = (obj as ExecuteRequestBody).RopBuffer.buffers.Length;
                            for (int i = 0; i < RopBufferCount; i++)
                            {
                                foreach (var Rop in (obj as ExecuteRequestBody).RopBuffer.buffers[i].Payload.RopsList)
                                {
                                    if (Rop is RopSynchronizationConfigureRequest)
                                    {
                                        if (SyncConfigureRopNum == SyncConfigureRopNum_Current)
                                        {
                                            if ((Rop as RopSynchronizationConfigureRequest).SynchronizationType == SynchronizationType.Contents)
                                            {
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.contentsSync;
                                                break;
                                            }
                                            else
                                            {
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.hierarchySync;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            SyncConfigureRopNum_Current++;
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        else if (DecodingContext.CopyFolder_OutputHandles.Count > 0 && DecodingContext.CopyFolder_OutputHandles.Contains(parameters[1]))
                        {
                            DecodingContext.StreamType_Getbuffer = FastTransferStreamType.TopFolder;
                        }
                        else if (DecodingContext.CopyMessage_OutputHandles != null && DecodingContext.CopyMessage_OutputHandles.Contains(parameters[1]))
                        {
                            DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageList;
                        }
                        else if (DecodingContext.SyncGetTransferState_OutputHandles != null && DecodingContext.SyncGetTransferState_OutputHandles.Contains(parameters[1]))
                        {
                            DecodingContext.StreamType_Getbuffer = FastTransferStreamType.state;
                        }
                    }
                    else
                    {
                        // parsing the previous sessions until DecodingContext.StreamType_Getbuffer has value. 
                        do
                        {
                            if (IsMapihttpSession(currentSession, TrafficDirection.Out))
                            {
                                // If currentSessionID is not contained in responseDic, do parse this response structure, else not. 

                                obj = ParseResponseMessage(currentSession, out bytesForHexView, true);
                                if (DecodingContext.CopyTo_OutputHandles != null && DecodingContext.CopyTo_OutputHandles.Contains(parameters[1]))
                                {
                                    // If CopyTo output handle is equal to the GetBuffer input handle, need to do further parse for CopyTo request.
                                    ParseRequestMessage(currentSession, true);
                                    int CopyToRopNum = DecodingContext.CopyTo_OutputHandles.IndexOf(parameters[1]);
                                    if (DecodingContext.FasttransterMid_InputIndexAndHandles.ContainsKey(0xffffffff))
                                    {
                                        ObjectHandlesType ObjectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                        switch (ObjectHandleType)
                                        {
                                            case ObjectHandlesType.FolderHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                                break;
                                            case ObjectHandlesType.MessageHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                                break;
                                            case ObjectHandlesType.AttachmentHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                                break;
                                            default:
                                                throw new Exception("The ObjectHandlesType is not right.");
                                        }
                                    }
                                    else if (!DecodingContext.ObjectHandles.ContainsKey(DecodingContext.CopyTo_InputHandles[CopyToRopNum]))
                                    {
                                        Dictionary<int, uint> dic = new Dictionary<int, uint>();
                                        dic.Add(currentSession.id, DecodingContext.CopyTo_InputHandles[CopyToRopNum]);
                                        targetHandle.Push(dic);
                                        uint searchCopyToHandle_DifferentSession = DecodingContext.CopyTo_InputHandles[CopyToRopNum];
                                        do
                                        {
                                            ParseResponseMessage(currentSession, out bytesForHexView, true);
                                            currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                        }
                                        while (!DecodingContext.ObjectHandles.ContainsKey(searchCopyToHandle_DifferentSession));
                                        targetHandle.Pop();

                                        ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchCopyToHandle_DifferentSession];
                                        switch (ObjectHandleType)
                                        {
                                            case ObjectHandlesType.FolderHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                                break;
                                            case ObjectHandlesType.MessageHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                                break;
                                            case ObjectHandlesType.AttachmentHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                                break;
                                            default:
                                                throw new Exception("The ObjectHandlesType is not right.");
                                        }
                                    }
                                }
                                else if (DecodingContext.CopyProperties_OutputHandles != null && DecodingContext.CopyProperties_OutputHandles.Contains(parameters[1]))
                                {
                                    ParseRequestMessage(currentSession, true);
                                    int CopyPropertiesRopNum = DecodingContext.CopyProperties_OutputHandles.IndexOf(parameters[1]);

                                    // when ObjectHandles contains object handle in copyProperties rop, the FastTransferStream type can be determined by the ObjectHandlesType.
                                    if (DecodingContext.FasttransterMid_InputIndexAndHandles.ContainsKey(0xffffffff))
                                    {
                                        ObjectHandlesType ObjectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                        switch (ObjectHandleType)
                                        {
                                            case ObjectHandlesType.FolderHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                                break;
                                            case ObjectHandlesType.MessageHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                                break;
                                            case ObjectHandlesType.AttachmentHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                                break;
                                            default:
                                                throw new Exception("The ObjectHandlesType is not right.");
                                        }
                                    }
                                    else if (!DecodingContext.ObjectHandles.ContainsKey(DecodingContext.CopyProperties_InputHandles[CopyPropertiesRopNum]))
                                    {
                                        Dictionary<int, uint> dic = new Dictionary<int, uint>();
                                        dic.Add(currentSession.id, DecodingContext.CopyProperties_InputHandles[CopyPropertiesRopNum]);
                                        targetHandle.Push(dic);
                                        uint searchCopyPropertiesHandle_DifferentSession = DecodingContext.CopyProperties_InputHandles[CopyPropertiesRopNum];
                                        do
                                        {
                                            ParseResponseMessage(currentSession, out bytesForHexView, true);
                                            currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                        }
                                        while (!DecodingContext.ObjectHandles.ContainsKey(searchCopyPropertiesHandle_DifferentSession));
                                        targetHandle.Pop();

                                        ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchCopyPropertiesHandle_DifferentSession];
                                        switch (ObjectHandleType)
                                        {
                                            case ObjectHandlesType.FolderHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.folderContent;
                                                break;
                                            case ObjectHandlesType.MessageHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageContent;
                                                break;
                                            case ObjectHandlesType.AttachmentHandles:
                                                DecodingContext.StreamType_Getbuffer = FastTransferStreamType.attachmentContent;
                                                break;
                                            default:
                                                throw new Exception("The ObjectHandlesType is not right.");
                                        }
                                    }
                                }
                                else if (DecodingContext.SyncConfigure_OutputHandles != null && DecodingContext.SyncConfigure_OutputHandles.Contains(parameters[1]))
                                {
                                    ParseRequestMessage(currentSession, true);
                                    obj = requestDic[currentSession.id];

                                    int SyncConfigureRopNum = DecodingContext.SyncConfigure_OutputHandles.IndexOf(parameters[1]);
                                    int SyncConfigureRopNum_Current = 0;
                                    int RopBufferCount = (obj as ExecuteRequestBody).RopBuffer.buffers.Length;
                                    for (int i = 0; i < RopBufferCount; i++)
                                    {
                                        foreach (var Rop in (obj as ExecuteRequestBody).RopBuffer.buffers[i].Payload.RopsList)
                                        {
                                            if (Rop is RopSynchronizationConfigureRequest)
                                            {
                                                if (SyncConfigureRopNum == SyncConfigureRopNum_Current)
                                                {
                                                    if ((Rop as RopSynchronizationConfigureRequest).SynchronizationType == SynchronizationType.Contents)
                                                    {
                                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.contentsSync;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        DecodingContext.StreamType_Getbuffer = FastTransferStreamType.hierarchySync;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    SyncConfigureRopNum_Current++;
                                                    continue;
                                                }
                                            }
                                        }
                                    }

                                }
                                else if (DecodingContext.CopyFolder_OutputHandles != null && DecodingContext.CopyFolder_OutputHandles.Contains(parameters[1]))
                                {
                                    DecodingContext.StreamType_Getbuffer = FastTransferStreamType.TopFolder;
                                }
                                else if (DecodingContext.CopyMessage_OutputHandles != null && DecodingContext.CopyMessage_OutputHandles.Contains(parameters[1]))
                                {
                                    DecodingContext.StreamType_Getbuffer = FastTransferStreamType.MessageList;
                                }
                                else if (DecodingContext.SyncGetTransferState_OutputHandles != null && DecodingContext.SyncGetTransferState_OutputHandles.Contains(parameters[1]))
                                {
                                    DecodingContext.StreamType_Getbuffer = FastTransferStreamType.state;
                                }
                                else
                                {
                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                    continue;
                                }
                            }
                            if (AllSessionsList.IndexOf(currentSession) == 0)
                            {
                                break;
                            }
                            else
                            {
                                currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                            }
                        }
                        while (DecodingContext.StreamType_Getbuffer == 0);
                    }
                }


                if (DecodingContext.StreamType_Getbuffer != 0)
                {
                    result = DecodingContext.StreamType_Getbuffer;
                }
                else
                {
                    result = string.Format("{0} cannot be parsed successfully due to missing the stream type information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                }

                if (targetHandle.Count == 1)
                {
                    ContextInformation information = new ContextInformation();
                    information.RopID = (RopIdType)sourceRopID;
                    information.Handle = parameters[1];
                    information.RelatedInformation = result;
                    ContextInformationCollection.Add(information);
                    if (!OverwriteOriginalInformation(ThisSession.id, out savedResult))
                    {
                        obj = savedResult;
                        bytes = new byte[0];
                        return;
                    }
                }
                targetHandle.Pop();

                // Add this session id(GetBuffer Rop) in DecodingContext.SessionFastTransferStreamType.
                if (DecodingContext.SessionFastTransferStreamType.Count != 0 && DecodingContext.SessionFastTransferStreamType.ContainsKey(ThisSession.id))
                {
                    DecodingContext.SessionFastTransferStreamType[ThisSession.id] = DecodingContext.StreamType_Getbuffer;
                }
                DecodingContext.SessionFastTransferStreamType.Add(ThisSession.id, DecodingContext.StreamType_Getbuffer);

                if (DecodingContext.StreamType_Getbuffer != 0)
                {
                    // After get StreamType for this session id (GetBuffer Rop). Do parse for response structure of this session.
                    obj = ParseResponseMessage(ThisSession, out bytesForHexView, true);
                    bytes = bytesForHexView;
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the stream type information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                    bytes = new byte[0];
                }
                DecodingContext.StreamType_Getbuffer = 0;
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopFastTransferDestinationPutBuffer)
            {
                Session currentSession = allSessions[AllSessionsList.IndexOf(ThisSession) - 1];
                DecodingContext.StreamType_Putbuffer = 0;
                Dictionary<int, uint> dic_Putbuffer = new Dictionary<int, uint>();
                dic_Putbuffer.Add((int)ThisSession.id, parameters[1]);
                targetHandle.Push(dic_Putbuffer);

                if (parameters != null && parameters.Length > 1)
                {
                    if (parameters[1] == 0xffffffff)
                    {
                        //Save the parameter in current session
                        Dictionary<uint, SourceOperation> putbufferSourceInCurrentSession = new Dictionary<uint, SourceOperation>();
                        putbufferSourceInCurrentSession = DecodingContext.PutBuffer_sourceOperation;

                        // ObjectHandle value is 0xffffffff means the putbuffer rop and destinationConfiure rop are in the same session. so parse this session response to get putBuffer input handle and destinationConfigure output handle
                        ParseResponseMessage(ThisSession, out bytesForHexView, true);
                        obj = responseDic[ThisSession.id];
                        int rgbOutputBufferCount = (obj as ExecuteResponseBody).RopBuffer.rgbOutputBuffers.Length;
                        for (int i = 0; i < rgbOutputBufferCount; i++)
                        {
                            uint putBufferHandle_InResponse = ((obj as ExecuteResponseBody).RopBuffer.rgbOutputBuffers[i].Payload as ROPOutputBuffer).ServerObjectHandleTable[parameters[0]];
                            if (DecodingContext.DestinationConfigure_OutputHandles.Contains(putBufferHandle_InResponse))
                            {
                                SourceOperation sourceOperationType = putbufferSourceInCurrentSession[putBufferHandle_InResponse];
                                if (sourceOperationType == SourceOperation.CopyFolder)
                                {
                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.TopFolder;
                                }
                                else if (sourceOperationType == SourceOperation.CopyMessages)
                                {
                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.MessageList;
                                }
                                else
                                {
                                    // segment1: get the rop num of RopFastTransferDestinationConfigure in this session
                                    uint destinationConfigureRopNum = 0;
                                    foreach (var Rop in ((obj as ExecuteResponseBody).RopBuffer.rgbOutputBuffers[i].Payload as ROPOutputBuffer).RopsList)
                                    {
                                        if (Rop is RopFastTransferDestinationConfigureResponse)
                                        {
                                            if ((Rop as RopFastTransferDestinationConfigureResponse).OutputHandleIndex != (byte)parameters[0])
                                            {
                                                destinationConfigureRopNum++;
                                                continue;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    //segment1
                                    if (DecodingContext.DestinationConfigure_InputHandles.ToArray()[destinationConfigureRopNum] == 0xffffffff)
                                    {
                                        ObjectHandlesType ObjectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                        switch (ObjectHandleType)
                                        {
                                            case ObjectHandlesType.FolderHandles:
                                                DecodingContext.StreamType_Putbuffer = FastTransferStreamType.folderContent;
                                                break;
                                            case ObjectHandlesType.MessageHandles:
                                                DecodingContext.StreamType_Putbuffer = FastTransferStreamType.MessageContent;
                                                break;
                                            case ObjectHandlesType.AttachmentHandles:
                                                DecodingContext.StreamType_Putbuffer = FastTransferStreamType.attachmentContent;
                                                break;
                                            default:
                                                throw new Exception("The ObjectHandlesType is not right.");
                                        }
                                    }
                                    else
                                    {
                                        uint searchDestinationConfigure_DifferentSession = DecodingContext.DestinationConfigure_InputHandles.ToArray()[destinationConfigureRopNum];
                                        if (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession))
                                        {
                                            Dictionary<int, uint> dic = new Dictionary<int, uint>();

                                            dic.Add((int)ThisSession.id, searchDestinationConfigure_DifferentSession);
                                            targetHandle.Push(dic);
                                            do
                                            {
                                                if (IsMapihttpSession(currentSession, TrafficDirection.Out))
                                                {
                                                    ParseResponseMessage(currentSession, out bytesForHexView, true);
                                                }
                                                currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                            }
                                            while (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession));
                                            targetHandle.Pop();
                                        }

                                        ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchDestinationConfigure_DifferentSession];
                                        switch (ObjectHandleType)
                                        {
                                            case ObjectHandlesType.FolderHandles:
                                                DecodingContext.StreamType_Putbuffer = FastTransferStreamType.folderContent;
                                                break;
                                            case ObjectHandlesType.MessageHandles:
                                                DecodingContext.StreamType_Putbuffer = FastTransferStreamType.MessageContent;
                                                break;
                                            case ObjectHandlesType.AttachmentHandles:
                                                DecodingContext.StreamType_Putbuffer = FastTransferStreamType.attachmentContent;
                                                break;
                                            default:
                                                throw new Exception("The ObjectHandlesType is not right.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Parsing the previous sessions until DecodingContext.StreamType_PutBuffer has value. 
                        do
                        {
                            if (IsMapihttpSession(currentSession, TrafficDirection.Out))
                            {
                                ParseResponseMessage(currentSession, out bytesForHexView, true);
                                if (DecodingContext.DestinationConfigure_OutputHandles != null && DecodingContext.DestinationConfigure_OutputHandles.Contains(parameters[1]))
                                {
                                    ParseRequestMessage(currentSession, true);
                                    obj = requestDic[currentSession.id];
                                    int destinationConfigureRopNum = DecodingContext.DestinationConfigure_OutputHandles.IndexOf(parameters[1]);
                                    int destinationConfigureRopNum_Current = 0;
                                    int RopBufferCount = (obj as ExecuteRequestBody).RopBuffer.buffers.Length;
                                    for (int i = 0; i < RopBufferCount; i++)
                                    {
                                        // If DestinationConfigure output handle is equal to the PutBuffer input handle and DestinationConfigure request has parsed, will get the stream type according to the SourceOperation field in RopSynchronizationConfigureRequest.
                                        foreach (var Rop in (obj as ExecuteRequestBody).RopBuffer.buffers[i].Payload.RopsList)
                                        {
                                            if (Rop is RopFastTransferDestinationConfigureRequest)
                                            {
                                                if (destinationConfigureRopNum == destinationConfigureRopNum_Current)
                                                {
                                                    if ((Rop as RopFastTransferDestinationConfigureRequest).SourceOperation == SourceOperation.CopyFolder)
                                                    {
                                                        DecodingContext.StreamType_Putbuffer = FastTransferStreamType.TopFolder;
                                                        break;
                                                    }
                                                    else if ((Rop as RopFastTransferDestinationConfigureRequest).SourceOperation == SourceOperation.CopyMessages)
                                                    {
                                                        DecodingContext.StreamType_Putbuffer = FastTransferStreamType.MessageList;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        if (DecodingContext.DestinationConfigure_InputHandles.ToArray()[destinationConfigureRopNum] == 0xffffffff)
                                                        {
                                                            ObjectHandlesType ObjectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                                            switch (ObjectHandleType)
                                                            {
                                                                case ObjectHandlesType.FolderHandles:
                                                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.folderContent;
                                                                    break;
                                                                case ObjectHandlesType.MessageHandles:
                                                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.MessageContent;
                                                                    break;
                                                                case ObjectHandlesType.AttachmentHandles:
                                                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.attachmentContent;
                                                                    break;
                                                                default:
                                                                    throw new Exception("The ObjectHandlesType is not right.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            uint searchDestinationConfigure_DifferentSession = DecodingContext.DestinationConfigure_InputHandles.ToArray()[destinationConfigureRopNum];
                                                            if (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession))
                                                            {
                                                                Dictionary<int, uint> dic = new Dictionary<int, uint>();

                                                                dic.Add(currentSession.id, searchDestinationConfigure_DifferentSession);
                                                                targetHandle.Push(dic);
                                                                do
                                                                {
                                                                    ParseResponseMessage(currentSession, out bytesForHexView);
                                                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                                                }
                                                                while (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession));
                                                                targetHandle.Pop();
                                                            }
                                                            ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchDestinationConfigure_DifferentSession];
                                                            switch (ObjectHandleType)
                                                            {
                                                                case ObjectHandlesType.FolderHandles:
                                                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.folderContent;
                                                                    break;
                                                                case ObjectHandlesType.MessageHandles:
                                                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.MessageContent;
                                                                    break;
                                                                case ObjectHandlesType.AttachmentHandles:
                                                                    DecodingContext.StreamType_Putbuffer = FastTransferStreamType.attachmentContent;
                                                                    break;
                                                                default:
                                                                    throw new Exception("The ObjectHandlesType is not right.");
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    destinationConfigureRopNum_Current++;
                                                    continue;
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                    continue;
                                }
                            }

                            if (AllSessionsList.IndexOf(currentSession) == 0)
                            {
                                break;
                            }
                            else
                            {
                                currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                            }
                        }
                        while (DecodingContext.StreamType_Putbuffer == 0);
                    }
                }

                if (DecodingContext.StreamType_Putbuffer != 0)
                {
                    result = DecodingContext.StreamType_Putbuffer;
                }
                else
                {
                    result = string.Format("{0} cannot be parsed successfully due to missing the stream type information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                }

                if (targetHandle.Count == 1)
                {
                    ContextInformation information = new ContextInformation();
                    information.RopID = (RopIdType)sourceRopID;
                    information.Handle = parameters[1];
                    information.RelatedInformation = result;
                    ContextInformationCollection.Add(information);
                    if (!OverwriteOriginalInformation(ThisSession.id, out savedResult))
                    {
                        obj = savedResult;
                        bytes = new byte[0];
                        return;
                    }
                }
                targetHandle.Pop();

                // Add this session id in DecodingContext.SessionFastTransferStreamType.
                if (DecodingContext.SessionFastTransferStreamType.Count != 0 && DecodingContext.SessionFastTransferStreamType.ContainsKey(ThisSession.id))
                {
                    DecodingContext.SessionFastTransferStreamType[ThisSession.id] = DecodingContext.StreamType_Putbuffer;
                }
                DecodingContext.SessionFastTransferStreamType.Add(ThisSession.id, DecodingContext.StreamType_Putbuffer);

                if (DecodingContext.StreamType_Putbuffer != 0)
                {
                    // After get StreamType. Do parse this session request message.
                    ParseRequestMessage(ThisSession, true);
                    obj = requestDic[ThisSession.id];
                    bytes = requestBytesForHexview[ThisSession.id];
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the stream type information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                    bytes = new byte[0];
                }
                DecodingContext.StreamType_Putbuffer = 0;
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopFastTransferDestinationPutBufferExtended)
            {
                Session currentSession = allSessions[AllSessionsList.IndexOf(ThisSession) - 1];
                DecodingContext.StreamType_PutbufferExtended = 0;
                Dictionary<int, uint> dic_PutbufferExtended = new Dictionary<int, uint>();
                dic_PutbufferExtended.Add((int)ThisSession.id, parameters[1]);
                targetHandle.Push(dic_PutbufferExtended);

                if (parameters != null && parameters.Length > 1)
                {
                    // If the parameter contains 0xffffffff, it means the configure and PutBufferExtended in the same frame
                    if (parameters[1] == 0xffffffff)
                    {
                        ParseResponseMessage(ThisSession, out bytesForHexView, true);
                        obj = responseDic[ThisSession.id];
                        if (obj is ExecuteResponseBody)
                        {
                            int rgbOutputBufferCount = (obj as ExecuteResponseBody).RopBuffer.rgbOutputBuffers.Length;
                            for (int i = 0; i < rgbOutputBufferCount; i++)
                            {

                                uint OutputHandle_InResponse = ((obj as ExecuteResponseBody).RopBuffer.rgbOutputBuffers[i].Payload as ROPOutputBuffer).ServerObjectHandleTable[parameters[0]];
                                if (DecodingContext.DestinationConfigure_OutputHandles.Contains(OutputHandle_InResponse))
                                {
                                    SourceOperation operation = DecodingContext.PutBufferExtended_sourceOperation[OutputHandle_InResponse];
                                    if (operation == SourceOperation.CopyFolder)
                                    {
                                        DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.TopFolder;
                                    }
                                    else if (operation == SourceOperation.CopyMessages)
                                    {
                                        DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.MessageList;
                                    }
                                    else
                                    {
                                        uint numberInRopList = 0;
                                        foreach (var rop in ((obj as ExecuteResponseBody).RopBuffer.rgbOutputBuffers[i].Payload as ROPOutputBuffer).RopsList)
                                        {
                                            if (rop is RopFastTransferDestinationConfigureResponse)
                                            {
                                                if ((rop as RopFastTransferDestinationConfigureResponse).OutputHandleIndex != (byte)parameters[0])
                                                {
                                                    numberInRopList++;
                                                    continue;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        // if the the inputhandle is 0xffffffff, it means the operation and the configure in the same session. 
                                        if (DecodingContext.DestinationConfigure_InputHandles.ToArray()[numberInRopList] == 0xffffffff)
                                        {
                                            ObjectHandlesType objectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                            switch (objectHandleType)
                                            {
                                                case ObjectHandlesType.AttachmentHandles:
                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.attachmentContent;
                                                    break;
                                                case ObjectHandlesType.FolderHandles:
                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.folderContent;
                                                    break;
                                                case ObjectHandlesType.MessageHandles:
                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.MessageContent;
                                                    break;
                                                default:
                                                    throw new Exception("The ObjectHandlesType is not right.");
                                            }
                                        }
                                        else
                                        {
                                            uint searchDestinationConfigure_DifferentSession = DecodingContext.DestinationConfigure_InputHandles.ToArray()[numberInRopList];
                                            if (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession))
                                            {
                                                Dictionary<int, uint> dic = new Dictionary<int, uint>();
                                                dic.Add(ThisSession.id, searchDestinationConfigure_DifferentSession);
                                                targetHandle.Push(dic);
                                                do
                                                {
                                                    if (IsMapihttpSession(currentSession, TrafficDirection.Out))
                                                    {
                                                        ParseResponseMessage(currentSession, out bytesForHexView, true);
                                                    }
                                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                                }
                                                while (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession));
                                                targetHandle.Pop();
                                            }

                                            ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchDestinationConfigure_DifferentSession];
                                            switch (ObjectHandleType)
                                            {
                                                case ObjectHandlesType.FolderHandles:
                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.folderContent;
                                                    break;
                                                case ObjectHandlesType.MessageHandles:
                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.MessageContent;
                                                    break;
                                                case ObjectHandlesType.AttachmentHandles:
                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.attachmentContent;
                                                    break;
                                                default:
                                                    throw new Exception("The ObjectHandlesType is not right.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Parsing the previous sessions until DecodingContext.StreamType_PutBufferExtended has value. 
                        do
                        {
                            if (IsMapihttpSession(currentSession, TrafficDirection.Out))
                            {
                                ParseResponseMessage(currentSession, out bytesForHexView, true);
                                if (DecodingContext.DestinationConfigure_OutputHandles != null && DecodingContext.DestinationConfigure_OutputHandles.Contains(parameters[1]))
                                {
                                    ParseRequestMessage(currentSession, true);
                                    obj = requestDic[currentSession.id];
                                    int destinationConfigureRopNum = DecodingContext.DestinationConfigure_OutputHandles.IndexOf(parameters[1]);
                                    int destinationConfigureRopNum_Current = 0;
                                    int RopBufferCount = (obj as ExecuteRequestBody).RopBuffer.buffers.Length;
                                    for (int i = 0; i < RopBufferCount; i++)
                                    {
                                        // If DestinationConfigure output handle is equal to the PutBuffer input handle and DestinationConfigure request has parsed, will get the stream type according to the SourceOperation field in RopSynchronizationConfigureRequest.
                                        foreach (var Rop in (obj as ExecuteRequestBody).RopBuffer.buffers[i].Payload.RopsList)
                                        {
                                            if (Rop is RopFastTransferDestinationConfigureRequest)
                                            {
                                                if (destinationConfigureRopNum == destinationConfigureRopNum_Current)
                                                {
                                                    if ((Rop as RopFastTransferDestinationConfigureRequest).SourceOperation == SourceOperation.CopyFolder)
                                                    {
                                                        DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.TopFolder;
                                                        break;
                                                    }
                                                    else if ((Rop as RopFastTransferDestinationConfigureRequest).SourceOperation == SourceOperation.CopyMessages)
                                                    {
                                                        DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.MessageList;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        if (DecodingContext.DestinationConfigure_InputHandles.ToArray()[destinationConfigureRopNum] == 0xffffffff)
                                                        {
                                                            ObjectHandlesType ObjectHandleType = DecodingContext.ObjectIndex[DecodingContext.FasttransterMid_InputIndexAndHandles[0xffffffff]];
                                                            switch (ObjectHandleType)
                                                            {
                                                                case ObjectHandlesType.FolderHandles:
                                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.folderContent;
                                                                    break;
                                                                case ObjectHandlesType.MessageHandles:
                                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.MessageContent;
                                                                    break;
                                                                case ObjectHandlesType.AttachmentHandles:
                                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.attachmentContent;
                                                                    break;
                                                                default:
                                                                    throw new Exception("The ObjectHandlesType is not right.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            uint searchDestinationConfigure_DifferentSession = DecodingContext.DestinationConfigure_InputHandles.ToArray()[destinationConfigureRopNum];
                                                            if (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession))
                                                            {
                                                                Dictionary<int, uint> dic = new Dictionary<int, uint>();
                                                                dic.Add(currentSession.id, searchDestinationConfigure_DifferentSession);
                                                                targetHandle.Push(dic);
                                                                do
                                                                {
                                                                    ParseResponseMessage(currentSession, out bytesForHexView);
                                                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                                                }
                                                                while (!DecodingContext.ObjectHandles.ContainsKey(searchDestinationConfigure_DifferentSession));
                                                                targetHandle.Pop();
                                                            }
                                                            ObjectHandlesType ObjectHandleType = DecodingContext.ObjectHandles[searchDestinationConfigure_DifferentSession];
                                                            switch (ObjectHandleType)
                                                            {
                                                                case ObjectHandlesType.FolderHandles:
                                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.folderContent;
                                                                    break;
                                                                case ObjectHandlesType.MessageHandles:
                                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.MessageContent;
                                                                    break;
                                                                case ObjectHandlesType.AttachmentHandles:
                                                                    DecodingContext.StreamType_PutbufferExtended = FastTransferStreamType.attachmentContent;
                                                                    break;
                                                                default:
                                                                    throw new Exception("The ObjectHandlesType is not right.");
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    destinationConfigureRopNum_Current++;
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                                    continue;
                                }
                            }

                            if (AllSessionsList.IndexOf(currentSession) == 0)
                            {
                                break;
                            }
                            else
                            {
                                currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                            }
                        }
                        while (DecodingContext.StreamType_PutbufferExtended == 0);
                    }
                }

                if (DecodingContext.StreamType_PutbufferExtended != 0)
                {
                    result = DecodingContext.StreamType_PutbufferExtended;
                }
                else
                {
                    result = string.Format("{0} cannot be parsed successfully due to missing the stream type information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                }

                if (targetHandle.Count == 1)
                {
                    ContextInformation information = new ContextInformation();
                    information.RopID = (RopIdType)sourceRopID;
                    information.Handle = parameters[1];
                    information.RelatedInformation = result;
                    ContextInformationCollection.Add(information);
                    if (!OverwriteOriginalInformation(ThisSession.id, out savedResult))
                    {
                        obj = savedResult;
                        bytes = new byte[0];
                        return;
                    }
                }
                targetHandle.Pop();

                // Add this session id in DecodingContext.SessionFastTransferStreamType.
                if (DecodingContext.SessionFastTransferStreamType.Count != 0 && DecodingContext.SessionFastTransferStreamType.ContainsKey(ThisSession.id))
                {
                    DecodingContext.SessionFastTransferStreamType[ThisSession.id] = DecodingContext.StreamType_PutbufferExtended;
                }
                DecodingContext.SessionFastTransferStreamType.Add(ThisSession.id, DecodingContext.StreamType_PutbufferExtended);


                if (DecodingContext.StreamType_PutbufferExtended != 0)
                {
                    // After get StreamType. Do parse this session request message.
                    ParseRequestMessage(ThisSession, true);
                    obj = requestDic[ThisSession.id];
                    bytes = requestBytesForHexview[ThisSession.id];
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the stream type information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                    bytes = new byte[0];
                }
                DecodingContext.StreamType_PutbufferExtended = 0;
            } 
            else if ((RopIdType)sourceRopID == RopIdType.RopGetPropertiesSpecific)
            {
                ParseRequestMessage(ThisSession,true);
                obj = ParseResponseMessage(ThisSession, out bytesForHexView, true);
                bytes = bytesForHexView;
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopWritePerUserInformation)
            {
                Session currentSession = allSessions[AllSessionsList.IndexOf(ThisSession) - 1];
                if (parameters != null && parameters.Length > 0)
                {
                    Dictionary<int, uint> dic = new Dictionary<int, uint>();
                    dic.Add(ThisSession.id, parameters[0]);

                    // Parsing the previous sessions until DecodingContext.LogonFlagMapLogId contains the Logon Id in this RopWritePerUserInformation rop. 
                    targetHandle.Push(dic);
                    do
                    {
                        if (IsMapihttpSession(currentSession, TrafficDirection.In))
                        {
                            ParseRequestMessage(currentSession);
                        }
                        if (AllSessionsList.IndexOf(currentSession) == 0)
                        {
                            break;
                        }
                        else
                        {
                            currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                        }
                    }
                    while (DecodingContext.LogonFlagMapLogId.Count == 0 || !DecodingContext.LogonFlagMapLogId.ContainsKey((byte)parameters[0]));

                    if (DecodingContext.LogonFlagMapLogId.ContainsKey((byte)parameters[0]))
                    {
                        result = DecodingContext.LogonFlagMapLogId[(byte)parameters[0]];
                    }
                    else
                    {
                        result = string.Format("{0} cannot be parsed successfully due to missing the Logon information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                    }

                    if (targetHandle.Count == 1)
                    {
                        ContextInformation information = new ContextInformation();
                        information.RopID = (RopIdType)sourceRopID;
                        information.Handle = parameters[0];
                        information.RelatedInformation = result;
                        ContextInformationCollection.Add(information);
                        if (!OverwriteOriginalInformation(ThisSession.id, out savedResult))
                        {
                            obj = savedResult;
                            bytes = new byte[0];
                            return;
                        }  
                    }
                    targetHandle.Pop();
                }

                // Add this session id in DecodingContext.SessionLogonFlagsInLogonRop.
                if (!(DecodingContext.SessionLogonFlagMapLogId != null && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(ThisSession.id)))
                {
                    DecodingContext.SessionLogonFlagMapLogId.Add(ThisSession.id, DecodingContext.LogonFlagMapLogId);
                }

                if (DecodingContext.LogonFlagMapLogId.ContainsKey((byte)parameters[0]))
                {
                    // Parsing the request structure of this session.
                    ParseRequestMessage(ThisSession,true);
                    obj = requestDic[ThisSession.id];
                    bytes = requestBytesForHexview[ThisSession.id];
                }
                else
                {
                    obj = string.Format("{0} cannot be parsed successfully due to missing the Logon information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                    bytes = new byte[0];
                }
            }
            else if ((RopIdType)sourceRopID == RopIdType.RopQueryRows || (RopIdType)sourceRopID == RopIdType.RopFindRow || (RopIdType)sourceRopID == RopIdType.RopExpandRow) // MSOXCTABL rop
            {
                Session currentSession = allSessions[AllSessionsList.IndexOf(ThisSession) - 1];
                Dictionary<int, uint> dic_QueryRows = new Dictionary<int, uint>();
                dic_QueryRows.Add((int)ThisSession.id, parameters[1]);
                targetHandle.Push(dic_QueryRows);

                if (parameters != null && parameters.Length > 1)
                {

                    // SetColumn_InputHandles_InResponse is only set in this session(and setcolumn) response parse, so if SetColumn_InputHandles_InResponse contians this rops outputhandle means that setcolumn and this rop is in the same session.
                    if (DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 && (DecodingContext.SetColumn_InputHandles_InResponse).Contains(parameters[1]))
                    {
                        ParseRequestMessage(ThisSession, true);
                    }
                    else
                    {
                        Dictionary<int, uint> dic = new Dictionary<int, uint>();
                        dic.Add(ThisSession.id, parameters[1]);
                        targetHandle.Push(dic);
                        do
                        {
                            if (IsMapihttpSession(currentSession, TrafficDirection.In))
                            {
                                ParseRequestMessage(currentSession, true);
                            }
                            
                            if (AllSessionsList.IndexOf(currentSession) == 0)
                            {
                                break;
                            }
                            else
                            {
                                currentSession = allSessions[AllSessionsList.IndexOf(currentSession) - 1];
                            }
                        }
                        while (DecodingContext.SetColumnProTagMap_Handle.Count == 0 || !DecodingContext.SetColumnProTagMap_Handle.ContainsKey(parameters[1]));
                        targetHandle.Pop();
                    }

                    if (DecodingContext.SetColumnProTagMap_Handle.Count > 0)
                    {
                        if (DecodingContext.RowRops_propertyTags.ContainsKey(ThisSession.id))
                        {
                            DecodingContext.RowRops_propertyTags.Remove(ThisSession.id);
                            DecodingContext.RowRops_propertyTags.Add(ThisSession.id, DecodingContext.SetColumnProTagMap_Handle);
                        }
                        else
                        {
                            DecodingContext.RowRops_propertyTags.Add(ThisSession.id, DecodingContext.SetColumnProTagMap_Handle);
                        }
                    }
                }

                if (DecodingContext.SetColumnProTagMap_Handle.ContainsKey(parameters[1]))
                {
                    result = DecodingContext.SetColumnProTagMap_Handle[parameters[1]];
                }
                else
                {
                    result = string.Format("{0} cannot be parsed successfully due to missing the Logon information for handle {1}, check whether the trace is complete.", (RopIdType)sourceRopID, parameters[1]);
                }

                if (targetHandle.Count == 1)
                {
                    ContextInformation information = new ContextInformation();
                    information.RopID = (RopIdType)sourceRopID;
                    information.Handle = parameters[1];
                    information.RelatedInformation = result;
                    ContextInformationCollection.Add(information);
                    if (!OverwriteOriginalInformation(ThisSession.id, out savedResult))
                    {
                        obj = savedResult;
                        bytes = new byte[0];
                        return;
                    }  
                }
                targetHandle.Pop();

                if (DecodingContext.SetColumnProTagMap_Handle.ContainsKey(parameters[1]))
                {
                    obj = ParseResponseMessage(ThisSession, out bytesForHexView, true);
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
                Session currentSession = allSessions[1];
                Dictionary<int, uint> dic_Notify = new Dictionary<int, uint>();
                dic_Notify.Add(ThisSession.id, parameters[1]);
                targetHandle.Push(dic_Notify);

                if (parameters != null && parameters.Length > 1)
                {

                    // SetColumn_InputHandles_InResponse is only set in this session(and setcolumn) response parse, so if SetColumn_InputHandles_InResponse contians this rops outputhandle means that setcolumn and this rop is in the same session.
                    if (DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 && (DecodingContext.SetColumn_InputHandles_InResponse).Contains(parameters[1]))
                    {
                        ParseRequestMessage(ThisSession,true);
                    }
                    else
                    {
                        do
                        {
                            if (IsMapihttpSession(currentSession, TrafficDirection.Out))
                            {
                                ParseRequestMessage(currentSession, true);
                            }
                            if (currentSession.id >= AllSessionsList.IndexOf(ThisSession))
                            {
                                break;
                            }
                            else
                            {
                                currentSession = allSessions[AllSessionsList.IndexOf(currentSession) + 1];
                            }
                        }
                        while ( !(DecodingContext.PropertyTagsForNotify.Count > 0 && DecodingContext.PropertyTagsForNotify.ContainsKey(parameters[1])));
                    }
                }

                if (DecodingContext.PropertyTagsForNotify.ContainsKey(parameters[1]))
                {
                    result = DecodingContext.PropertyTagsForNotify[parameters[1]];
                }
                else
                {
                    result = string.Format("RopNotify cannot be parsed successfully due to missing the PropertyTags for handle {0}, check whether the trace is complete.", parameters[1]);
                }

                if (targetHandle.Count == 1)
                {
                    ContextInformation information = new ContextInformation();
                    information.RopID = (RopIdType)sourceRopID;
                    information.Handle = parameters[1];
                    information.RelatedInformation = result;
                    ContextInformationCollection.Add(information);
                    if (!OverwriteOriginalInformation(ThisSession.id, out savedResult))
                    {
                        obj = savedResult;
                        bytes = new byte[0];
                        return;
                    }  
                }
                targetHandle.Pop();

                if (DecodingContext.PropertyTagsForNotify.ContainsKey(parameters[1]))
                {
                    obj = ParseResponseMessage(ThisSession, out bytesForHexView, true);
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
                if (DecodingContext.SessionRequestRemainSize.Count > 0 && DecodingContext.SessionRequestRemainSize.ContainsKey(ThisSession.id))
                {
                    obj = responseDic[ThisSession.id];
                    bytes = responseBytesForHexview[ThisSession.id];
                }
                else
                {
                    ParseRequestMessage(ThisSession,  true);
                    obj = ParseResponseMessage(ThisSession, out bytesForHexView, true);
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
        /// Restore the covered related context information during looper call
        /// </summary>
        /// <param name="sessionID">The session ID</param>
        /// <param name="result">The result for missing related information </param>
        /// <returns>The result for overwriting.</returns>
        public bool OverwriteOriginalInformation(int sessionID, out string result)
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
                                //DecodingContext.SessionFastTransferStreamType.Add(ThisSessionID, (FastTransferStreamType)infor.RelatedInformation);
                                break;
                            case RopIdType.RopQueryRows:
                            case RopIdType.RopFindRow:
                            case RopIdType.RopExpandRow:
                                Dictionary<uint, PropertyTag[]> propTags = new Dictionary<uint, PropertyTag[]>();
                                propTags.Add(infor.Handle, (PropertyTag[])infor.RelatedInformation);
                                if (DecodingContext.RowRops_propertyTags.ContainsKey(sessionID))
                                {
                                    if (DecodingContext.RowRops_propertyTags[sessionID].ContainsKey(infor.Handle))
                                    {
                                        if (DecodingContext.RowRops_propertyTags[sessionID][infor.Handle] != (PropertyTag[])infor.RelatedInformation)
                                        {
                                            DecodingContext.RowRops_propertyTags[sessionID][infor.Handle] = (PropertyTag[])infor.RelatedInformation;
                                        }
                                    }
                                }
                                break;
                            case RopIdType.RopNotify:
                                if (DecodingContext.PropertyTagsForNotify.ContainsKey(infor.Handle))
                                {
                                    if ((PropertyTag[])infor.RelatedInformation != DecodingContext.PropertyTagsForNotify[infor.Handle])
                                    {
                                        DecodingContext.PropertyTagsForNotify[infor.Handle] = (PropertyTag[])infor.RelatedInformation;
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
        /// Parse special session's request message
        /// </summary>
        /// <param name="parsingSession">The session to parse</param>
        /// <param name="isLooper">A bool value indicates if this session is in a loop for parsing context sessions</param>
        public void ParseRequestMessage(Session parsingSession, bool isLooper = false)
        {
            if (IsMapihttpSession(parsingSession, TrafficDirection.In))
            {
                needToParseCROPSLayer = isLooper;
                byte[] bytesForHexView;
                object MAPIRequest = ParseHTTPPayload(parsingSession.RequestHeaders, parsingSession, parsingSession.requestBodyBytes, TrafficDirection.In, out bytesForHexView);
                if (MAPIRequest != null)
                {
                    if (parsingSession.requestBodyBytes.Length != 0 && MAPIRequest.GetType().Name == "ExecuteRequestBody" && requestDic != null && !requestDic.ContainsKey(parsingSession.id))
                    {
                        requestDic.Add(parsingSession.id, MAPIRequest);
                        requestBytesForHexview.Add(parsingSession.id, bytesForHexView);
                    }
                    else if (parsingSession.requestBodyBytes.Length != 0 && MAPIRequest.GetType().Name == "ExecuteRequestBody" && requestDic != null && requestDic.ContainsKey(parsingSession.id))
                    {
                        requestDic.Remove(parsingSession.id);
                        requestDic.Add(parsingSession.id, MAPIRequest);
                    }
                }
            }
            if (needToParseCROPSLayer)
            {
                needToParseCROPSLayer = false;
            }
        }

        /// <summary>
        /// Parse special session's response message
        /// </summary>
        /// <param name="session">The session to parse</param>
        /// <param name="HexViewBytes">Byte array for display in RopHexView</param>
        /// <param name="isLooper">A bool value indicates if this session is in a loop for parsing context sessions</param>
        public object ParseResponseMessage(Session paringSession, out byte[] HexViewBytes, bool isLooper = false)
        {
            object MAPIResponse = null;
            HexViewBytes = new byte[0];
            if (IsMapihttpSession(paringSession, TrafficDirection.Out))
            {
                needToParseCROPSLayer = isLooper;
                byte[] bytesForHexView;
                Session tempSession = this.session;
                this.session = paringSession;
                try
                {
                    MAPIResponse = ParseHTTPPayload(paringSession.ResponseHeaders, paringSession, paringSession.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                    HexViewBytes = bytesForHexView;
                    if (paringSession.responseBodyBytes.Length != 0 && MAPIResponse.GetType().Name == "ExecuteResponseBody" && responseDic != null && !responseDic.ContainsKey(paringSession.id))
                    {
                        responseDic.Add(paringSession.id, MAPIResponse);
                        responseBytesForHexview.Add(paringSession.id, bytesForHexView);
                    }
                    else if (paringSession.responseBodyBytes.Length != 0 && MAPIResponse.GetType().Name == "ExecuteResponseBody" && responseDic != null && responseDic.ContainsKey(paringSession.id))
                    {
                        responseDic.Remove(paringSession.id);
                        responseDic.Add(paringSession.id, MAPIResponse);
                    }
                }
                catch (Exception e)
                {
                    if (ContextInformationCollection.Count > 0)
                    {
                        foreach (ContextInformation infor in ContextInformationCollection)
                        {
                            if (infor.RelatedInformation.GetType() == typeof(string))
                            {
                                MAPIResponse = infor.RelatedInformation.ToString();
                            }
                        }
                    }
                }
                finally
                {
                    this.session = tempSession;
                }
            }
            
            if (isLooper)
            {
                needToParseCROPSLayer = false;
            }
            return MAPIResponse;
        }

        public static uint ParseResponseMessageSimplely(Session currentSession, int outputHandleIndex)
        {
            uint handle_InResponse = 0;
            if (IsMapihttpSession(currentSession, TrafficDirection.Out))
            {
                byte[] bytesForHexView;
                object MAPIResponse = ParseHTTPExecuteResponsePayload(currentSession.ResponseHeaders, currentSession, currentSession.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                int rgbOutputBufferCount = (MAPIResponse as ExecuteResponseBody).RopBuffer.rgbOutputBuffers.Length;
                for (int i = 0; i < rgbOutputBufferCount; i++)
                {
                    handle_InResponse = ((MAPIResponse as ExecuteResponseBody).RopBuffer.rgbOutputBuffers[i].Payload as ROPOutputBuffer_WithoutCROPS).ServerObjectHandleTable[outputHandleIndex];
                }
            }
            return handle_InResponse;
        }

        /// <summary>
        /// Parse the HTTP payload to MAPI message.
        /// </summary>
        /// <param name="headers">The HTTP header.</param>
        /// <param name="currentSessionID">the current session ID.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <param name="bytes">The bytes provided for MAPI view layer.</param>
        /// <returns>The object parsed result</returns>
        public object ParseHTTPPayload(HTTPHeaders headers, Session currentSession, byte[] bytesFromHTTP, TrafficDirection direction, out byte[] bytes)
        {
            object objectOut = null;
            byte[] emptyByte = new byte[0];
            bytes = emptyByte;

            if (bytesFromHTTP == null || bytesFromHTTP.Length == 0 || headers == null || !headers.Exists("X-RequestType"))
            {
                return null;
            }

            string requestType = headers["X-RequestType"];

            if (requestType == null)
            {
                return null;
            }
            try
            {
                if (direction == TrafficDirection.Out && headers.Exists("Transfer-Encoding") && headers["Transfer-Encoding"] == "chunked")
                {
                    bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                    bytes = bytesFromHTTP;
                }
                else
                {
                    bytes = bytesFromHTTP;
                }

                Stream stream = new MemoryStream(bytesFromHTTP);
                MAPIInspector.selectedSession = this.session;
                MAPIInspector.parsingSession = currentSession;
                if (direction == TrafficDirection.In)
                {
                    switch (requestType)
                    {
                        case "Connect":
                            {
                                ConnectRequestBody ConnectRequest = new ConnectRequestBody();
                                ConnectRequest.Parse(stream);
                                objectOut = ConnectRequest;
                                break;
                            }
                        case "Execute":
                            {
                                ExecuteRequestBody ExecuteRequest = new ExecuteRequestBody();
                                ExecuteRequest.Parse(stream);
                                objectOut = ExecuteRequest;
                                break;
                            }
                        case "Disconnect":
                            {
                                DisconnectRequestBody DisconnectRequest = new DisconnectRequestBody();
                                DisconnectRequest.Parse(stream);
                                objectOut = DisconnectRequest;
                                break;
                            }
                        case "NotificationWait":
                            {
                                NotificationWaitRequestBody NotificationWaitRequest = new NotificationWaitRequestBody();
                                NotificationWaitRequest.Parse(stream);
                                objectOut = NotificationWaitRequest;
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
                                objectOut = "Unavailable Response Type";
                                break;
                            }
                    }
                }
                else
                {
                    switch (requestType)
                    {
                        case "Connect":
                            {
                                ConnectResponseBody ConnectResponse = new ConnectResponseBody();
                                ConnectResponse.Parse(stream);
                                objectOut = ConnectResponse;
                                break;
                            }
                        case "Execute":
                            {
                                ExecuteResponseBody ExecuteResponse = new ExecuteResponseBody();
                                ExecuteResponse.Parse(stream);
                                objectOut = ExecuteResponse;
                                break;
                            }
                        case "Disconnect":
                            {

                                DisconnectResponseBody DisconnectResponse = new DisconnectResponseBody();
                                DisconnectResponse.Parse(stream);
                                objectOut = DisconnectResponse;
                                break;
                            }
                        case "NotificationWait":
                            {

                                NotificationWaitResponseBody NotificationWaitResponse = new NotificationWaitResponseBody();
                                NotificationWaitResponse.Parse(stream);
                                objectOut = NotificationWaitResponse;
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
                }
                return objectOut;
            }
            catch (MissingInformationException mException)
            {
                DecodingContext.LogonFlagMapLogId = new Dictionary<byte, LogonFlags>();
                DecodingContext.DestinationConfigure_OutputHandles = new List<uint>();

                HandleContextInformation(mException.RopID, out objectOut, out bytes, mException.Parameters);
                return objectOut;
            }
            catch (Exception ex)
            {
                objectOut = ex.ToString();
                return objectOut;
            }
        }
		
		 /// <param name="currentSessionID">the current session ID.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <param name="bytes">The bytes provided for MAPI view layer.</param>
        /// <returns>The object parsed result</returns>
        public static object ParseHTTPExecuteResponsePayload(HTTPHeaders headers, Session currentSession, byte[] bytesFromHTTP, TrafficDirection direction, out byte[] bytes)
        {
            object objectOut = null;
            byte[] emptyByte = new byte[0];
            bytes = emptyByte;

            if (bytesFromHTTP == null || bytesFromHTTP.Length == 0 || headers == null || !headers.Exists("X-RequestType"))
            {
                return null;
            }

            string requestType = headers["X-RequestType"];

            if (requestType == null)
            {
                return null;
            }
            try
            {
                if (direction == TrafficDirection.Out && headers.Exists("Transfer-Encoding") && headers["Transfer-Encoding"] == "chunked")
                {
                    bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                    bytes = bytesFromHTTP;
                }
                else
                {
                    bytes = bytesFromHTTP;
                }

                Stream stream = new MemoryStream(bytesFromHTTP);
                MAPIInspector.parsingSession = currentSession;
                if (direction == TrafficDirection.Out && requestType == "Execute")
                {
                    ExecuteResponseBody ExecuteResponse = new ExecuteResponseBody();
                    ExecuteResponse.Parse(stream);
                    objectOut = ExecuteResponse;
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
        /// <param name="bytesForHexview">The byte array provided for Hexview</param>
        public void DisplayObject(object obj, byte[] bytesForHexview)
        {
            if (obj == null)
            {
                return;
            }
            else if (obj.GetType().Name == "String")
            {
                this.oMAPIViewControl.BeginUpdate();
                this.oMAPIControl.MAPIRichTextBox.Visible = true;
                this.oMAPIControl.MAPIRichTextBox.Text = obj.ToString();
                this.oMAPIViewControl.EndUpdate();
            }
            else
            {
                this.oMAPIViewControl.BeginUpdate();
                int result = 0;
                try
                {
                    TreeNode topNode = BaseStructure.AddNodesForTree(obj, 0, out result);
                    this.oMAPIViewControl.Nodes.Add(topNode);
                    topNode.ExpandAll();
                    this.oMAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(bytesForHexview);
                    this.oMAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
                }
                catch (Exception e)
                {
                    this.oMAPIControl.MAPIRichTextBox.Visible = true;
                    this.oMAPIControl.MAPIRichTextBox.Text = e.Message;
                }
                finally
                {
                    this.oMAPIViewControl.EndUpdate();
                }
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
            isLooperCall = false;
            targetHandle = new Stack<Dictionary<int, uint>>();
            ContextInformationCollection = new List<ContextInformation>();


            if (this.IsMapihttp)
            {
                try
                {
                    if (this.Direction == TrafficDirection.In)
                    {
                        parserResult = this.ParseHTTPPayload(this.BaseHeaders, this.session, this.session.requestBodyBytes, TrafficDirection.In, out bytesForHexView);
                    }
                    else
                    {
                        //An X-ResponseCode of 0 (zero) means success from the perspective of the protocol transport, and the client SHOULD parse the response body based on the request that was issued.
                        if (this.BaseHeaders["X-ResponseCode"] != "0")
                        {
                            return;
                        }
                        parserResult = this.ParseHTTPPayload(this.BaseHeaders, this.session, this.session.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                    }
                    DisplayObject(parserResult, bytesForHexView);
                }
                finally
                {
                    DecodingContext.PropertyTagsForNotify = new Dictionary<uint, PropertyTag[]>();
                    DecodingContext.SetColumnProTagMap_Handle = new Dictionary<uint, PropertyTag[]>();
                    DecodingContext.ObjectHandles = new Dictionary<uint, ObjectHandlesType>();
                    targetHandle = new Stack<Dictionary<int, uint>>();
                    ContextInformationCollection = new List<ContextInformation>();
                    isLooperCall = true;
                }

            }
            else
            {
                return;
            }
        }
        public static bool IsMapihttpSession(Session currentSession, TrafficDirection direction)
        {

            if (currentSession != null)
            {
                if (direction == TrafficDirection.In)
                {
                    return currentSession.RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                }
                else if (direction == TrafficDirection.Out && currentSession.ResponseHeaders.Exists("X-ResponseCode"))
                {
                    string xResponseCode = currentSession.ResponseHeaders["X-ResponseCode"];
                    if (xResponseCode == "0")
                    {
                        return currentSession.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                    }
                    else if (xResponseCode != "")
                    {
                        return currentSession.ResponseHeaders.ExistsAndContains("Content-Type", "text/html");
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Enum for traffic direction
        /// </summary>
        public enum TrafficDirection
        {
            In,
            Out
        }
    }
}