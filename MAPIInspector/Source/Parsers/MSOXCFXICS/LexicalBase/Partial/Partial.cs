using BlockParser;
using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAPIInspector.Parsers
{
    internal class Partial
    {
        /// <summary>
        /// Used to record fasterTransfer stream property type in RopGetBuffer partial
        /// </summary>
        public static PropertyDataType PartialGetType { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property Id in RopGetBuffer partial
        /// </summary>
        public static PidTagPropertyEnum PartialGetId { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property remain size in RopGetBuffer partial
        /// </summary>
        public static int PartialGetRemainSize { get; set; } = -1;

        /// <summary>
        /// Used to record fasterTransfer stream property sub remain size in RopGetBuffer partial
        /// </summary>
        public static int PartialGetSubRemainSize { get; set; } = -1;

        /// <summary>
        /// Used to indicates if this ROP is about fasterTransfer stream RopGetBuffer partial
        /// </summary>
        public static bool IsGet { get; set; }

        /// <summary>
        /// Used to record serverUrl of the session which contains a RopGetBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialGetServerUrl { get; set; }

        /// <summary>
        /// Used to record processName of the session which contains a RopGetBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialGetProcessName { get; set; }

        /// <summary>
        /// Used to record clientInfo of the session which contains a RopGetBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialGetClientInfo { get; set; }

        /// <summary>
        /// Used to record session for RopGetBuffer partial
        /// </summary>
        public static Session PartialGetSession { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property type in RopPutBuffer partial
        /// </summary>
        public static PropertyDataType PartialPutType { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property Id in RopPutBuffer partial
        /// </summary>
        public static PidTagPropertyEnum PartialPutId { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property remain size in RopPutBuffer partial
        /// </summary>
        public static int PartialPutRemainSize { get; set; } = -1;

        /// <summary>
        /// Used to record fasterTransfer stream property sub remain size in RopPutBuffer partial
        /// </summary>
        public static int PartialPutSubRemainSize { get; set; } = -1;

        /// <summary>
        /// Used to indicates if this ROP is about fasterTransfer stream RopPutBuffer partial
        /// </summary>
        public static bool IsPut { get; set; }

        /// <summary>
        /// Used to record serverUrl of the session which contains a RopPutBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutServerUrl { get; set; }

        /// <summary>
        /// Used to record processName of the session which contains a RopPutBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutProcessName { get; set; }

        /// <summary>
        /// Used to record clientInfo of the session which contains a RopPutBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutClientInfo { get; set; }

        /// <summary>
        /// Used to record session for RopPutBuffer partial
        /// </summary>
        public static Session PartialPutSession { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property type in putExtendBuffer partial
        /// </summary>
        public static PropertyDataType PartialPutExtendType { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property Id in putExtendBuffer partial
        /// </summary>
        public static PidTagPropertyEnum PartialPutExtendId { get; set; }

        /// <summary>
        /// Used to record fasterTransfer stream property remain size in putExtendBuffer partial
        /// </summary>
        public static int PartialPutExtendRemainSize { get; set; } = -1;

        /// <summary>
        /// Used to record fasterTransfer stream property sub remain size in putExtendBuffer partial
        /// </summary>
        public static int PartialPutExtendSubRemainSize { get; set; } = -1;

        /// <summary>
        /// Used to indicates if this ROP is about fasterTransfer stream putExtendBuffer partial
        /// </summary>
        public static bool IsPutExtend { get; set; }

        /// <summary>
        /// Used to record serverUrl of the session which contains a RopPutExtendBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutExtendServerUrl { get; set; }

        /// <summary>
        /// Used to record processName of the session which contains a RopPutExtendBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutExtendProcessName { get; set; }

        /// <summary>
        /// Used to record clientInfo of the session which contains a RopPutExtendBuffer partial fasterTransfer stream
        /// </summary>
        public static string PartialPutExtendClientInfo { get; set; }

        /// <summary>
        /// Used to record session for putExtendbuffer partial
        /// </summary>
        public static Session PartialPutExtendSession { get; set; }

        /// <summary>
        /// Used to indicates if there is one byte need to be read before parsing fasterTransfer element
        /// </summary>
        public static bool IsOneMoreByteToRead { get; set; } = false;

        /// <summary>
        /// Record the map in handle, sessionId and PartialContextInformation for RopGetBuffer
        /// </summary>
        public static Dictionary<uint, SortedDictionary<int, PartialContextInformation>> HandleWithSessionGetContextInformation { get; set; } = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();

        /// <summary>
        /// Record the map in handle, sessionId and PartialContextInformation for RopPutBuffer
        /// </summary>
        public static Dictionary<uint, SortedDictionary<int, PartialContextInformation>> HandleWithSessionPutContextInformation { get; set; } = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();

        /// <summary>
        /// Record the map in handle, sessionId and PartialContextInformation for RopPutExtendedBuffer
        /// </summary>
        public static Dictionary<uint, SortedDictionary<int, PartialContextInformation>> HandleWithSessionPutExtendContextInformation { get; set; } = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();

        /// <summary>
        /// Parse sessions from start to this session to find informations for RopGetBuffer partial status
        /// </summary>
        /// <param name="ropID">The ROP id related with partial</param>
        /// <param name="parameters">The handle information</param>
        /// <param name="bytes">The output bytes returned</param>
        /// <returns>The parsed result for current session</returns>
        public static object FindPartialInformation(RopIdType ropID, uint parameters, out byte[] bytes)
        {
            byte[] bytesForHexView = new byte[0];
            object obj = new object();
            bytes = bytesForHexView;
            Session thisSession = MapiInspector.MAPIParser.ParsingSession;
            int thisSessionID = thisSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(thisSession))
            {
                thisSessionID = int.Parse(thisSession["VirtualID"]);
            }

            if (ropID == RopIdType.RopFastTransferSourceGetBuffer)
            {
                if (MapiInspector.MAPIParser.responseDic.ContainsKey(thisSessionID))
                {
                    obj = MapiInspector.MAPIParser.responseDic[thisSessionID];
                    bytes = MapiInspector.MAPIParser.responseBytesForHexview[thisSessionID];

                    if (HandleWithSessionGetContextInformation.ContainsKey(parameters) &&
                        HandleWithSessionGetContextInformation[parameters].ContainsKey(thisSessionID))
                    {
                        PartialGetId = HandleWithSessionGetContextInformation[parameters][thisSessionID].ID;
                        PartialGetType = HandleWithSessionGetContextInformation[parameters][thisSessionID].Type;
                        PartialGetRemainSize = HandleWithSessionGetContextInformation[parameters][thisSessionID].RemainSize;
                        PartialGetSubRemainSize = HandleWithSessionGetContextInformation[parameters][thisSessionID].SubRemainSize;
                        IsGet = HandleWithSessionGetContextInformation[parameters][thisSessionID].IsGet;
                        MapiInspector.MAPIParser.OutputPayLoadCompressedXOR = HandleWithSessionGetContextInformation[parameters][thisSessionID].PayLoadCompresssedXOR;
                        PartialGetServerUrl = thisSession.RequestHeaders.RequestPath;
                        PartialGetProcessName = thisSession.LocalProcess;
                        PartialGetClientInfo = thisSession.RequestHeaders["X-ClientInfo"];
                    }
                }
                else
                {
                    Session currentSession = MapiInspector.MAPIParser.AllSessions[1];
                    int currentSessionID = currentSession.id;
                    if (MapiInspector.MAPIParser.IsFromFiddlerCore(currentSession))
                    {
                        currentSessionID = int.Parse(currentSession["VirtualID"]);
                    }
                    int sessionGetContextCount = HandleWithSessionGetContextInformation.Count;

                    if (sessionGetContextCount > 0 &&
                        HandleWithSessionGetContextInformation.ContainsKey(parameters))
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
                            MapiInspector.MAPIParser.OutputPayLoadCompressedXOR = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].PayLoadCompresssedXOR;
                            PartialGetSession = HandleWithSessionGetContextInformation[parameters][lastSavedSessionID].Session;
                            PartialGetServerUrl = PartialGetSession.RequestHeaders.RequestPath;
                            PartialGetProcessName = PartialGetSession.LocalProcess;
                            PartialGetClientInfo = PartialGetSession.RequestHeaders["X-ClientInfo"];

                            currentSession = MapiInspector.MAPIParser.AllSessions[Convert.ToInt32(PartialGetSession["Number"]) + 1];
                        }
                        if (MapiInspector.MAPIParser.IsFromFiddlerCore(currentSession))
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
                        if (currentSession.RequestHeaders.RequestPath == serverurl &&
                            currentSession.LocalProcess == processName &&
                            currentSession.RequestHeaders["X-ClientInfo"] == clientInfo &&
                            MapiInspector.MAPIParser.IsMapihttpSession(currentSession, MapiInspector.MAPIParser.TrafficDirection.Out) &&
                            currentSession.RequestHeaders["X-RequestType"] == "Execute")
                        {
                            List<uint> tableHandles = new List<uint>();

                            if (MapiInspector.MAPIParser.handleGetDic.ContainsKey(currentSessionID))
                            {
                                tableHandles = MapiInspector.MAPIParser.handleGetDic[currentSessionID];
                            }
                            else
                            {
                                try
                                {
                                    MapiInspector.MAPIParser.IsOnlyGetServerHandle = true;
                                    object mapiResponse = MapiInspector.MAPIParser.ParseResponseMessage(currentSession, out bytesForHexView, false);

                                    if (mapiResponse != null &&
                                        (mapiResponse as ExecuteResponseBody).RopBuffer != null &&
                                        (mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Count() != 0)
                                    {
                                        tableHandles = ((ROPOutputBuffer_WithoutCROPS)(mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[0].Payload).ServerObjectHandleTable.ToList();
                                    }
                                }
                                finally
                                {
                                    MapiInspector.MAPIParser.IsOnlyGetServerHandle = false;
                                }
                            }

                            if (tableHandles.Contains(parameters))
                            {
                                MapiInspector.MAPIParser.ParseResponseMessage(currentSession, out bytesForHexView, true);
                            }
                        }

                        var nextSessionNumber = Convert.ToInt32(currentSession["Number"]) + 1;
                        foreach (var session in MapiInspector.MAPIParser.AllSessions)
                        {
                            if (Convert.ToInt32(session["Number"]) == nextSessionNumber)
                            {
                                currentSession = session;
                                break;
                            }
                        }
                        if (currentSessionID == currentSession.id ||
                            (currentSession["VirtualID"] != null &&
                            currentSessionID == int.Parse(currentSession["VirtualID"]))) break;
                        if (MapiInspector.MAPIParser.IsFromFiddlerCore(currentSession))
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

                    obj = MapiInspector.MAPIParser.ParseResponseMessage(thisSession, out bytesForHexView, true);
                    DecodingContext.PartialInformationReady = new Dictionary<int, bool>();
                    bytes = bytesForHexView;
                }
            }
            else if (ropID == RopIdType.RopFastTransferDestinationPutBuffer ||
                ropID == RopIdType.RopFastTransferDestinationPutBufferExtended)
            {
                if (MapiInspector.MAPIParser.requestDic.ContainsKey(thisSessionID))
                {
                    obj = MapiInspector.MAPIParser.requestDic[thisSessionID];
                    bytes = MapiInspector.MAPIParser.requestBytesForHexview[thisSessionID];

                    if (ropID == RopIdType.RopFastTransferDestinationPutBuffer)
                    {
                        if (HandleWithSessionPutContextInformation.ContainsKey(parameters) &&
                            HandleWithSessionPutContextInformation[parameters].ContainsKey(thisSessionID))
                        {
                            PartialPutId = HandleWithSessionPutContextInformation[parameters][thisSessionID].ID;
                            PartialPutType = HandleWithSessionPutContextInformation[parameters][thisSessionID].Type;
                            PartialPutRemainSize = HandleWithSessionPutContextInformation[parameters][thisSessionID].RemainSize;
                            PartialPutSubRemainSize = HandleWithSessionPutContextInformation[parameters][thisSessionID].SubRemainSize;
                            IsPut = true;
                            MapiInspector.MAPIParser.InputPayLoadCompressedXOR = HandleWithSessionPutContextInformation[parameters][thisSessionID].PayLoadCompresssedXOR;
                            PartialPutServerUrl = thisSession.RequestHeaders.RequestPath;
                            PartialPutProcessName = thisSession.LocalProcess;
                            PartialPutClientInfo = thisSession.RequestHeaders["X-ClientInfo"];
                        }
                    }
                    else
                    {
                        if (HandleWithSessionPutExtendContextInformation.ContainsKey(parameters) &&
                            HandleWithSessionPutExtendContextInformation[parameters].ContainsKey(thisSessionID))
                        {
                            PartialPutExtendId = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].ID;
                            PartialPutExtendType = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].Type;
                            PartialPutExtendRemainSize = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].RemainSize;
                            PartialPutExtendSubRemainSize = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].SubRemainSize;
                            IsPutExtend = true;
                            MapiInspector.MAPIParser.InputPayLoadCompressedXOR = HandleWithSessionPutExtendContextInformation[parameters][thisSessionID].PayLoadCompresssedXOR;
                            PartialPutExtendServerUrl = thisSession.RequestHeaders.RequestPath;
                            PartialPutExtendProcessName = thisSession.LocalProcess;
                            PartialPutExtendClientInfo = thisSession.RequestHeaders["X-ClientInfo"];
                        }
                    }
                }
                else
                {
                    Session currentSession = MapiInspector.MAPIParser.AllSessions[1];
                    int currentSessionID = currentSession.id;
                    if (MapiInspector.MAPIParser.IsFromFiddlerCore(currentSession))
                    {
                        currentSessionID = int.Parse(currentSession["VirtualID"]);
                    }
                    if (ropID == RopIdType.RopFastTransferDestinationPutBuffer)
                    {
                        int sessionPutContextCount = HandleWithSessionPutContextInformation.Count;

                        if (sessionPutContextCount > 0 &&
                            HandleWithSessionPutContextInformation.ContainsKey(parameters))
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
                                MapiInspector.MAPIParser.InputPayLoadCompressedXOR = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].PayLoadCompresssedXOR;
                                PartialPutSession = HandleWithSessionPutContextInformation[parameters][lastSavedSessionID].Session;
                                PartialPutServerUrl = PartialPutSession.RequestHeaders.RequestPath;
                                PartialPutProcessName = PartialPutSession.LocalProcess;
                                PartialPutClientInfo = PartialPutSession.RequestHeaders["X-ClientInfo"];
                                currentSession = MapiInspector.MAPIParser.AllSessions[Convert.ToInt32(PartialPutSession["Number"]) + 1];
                            }

                            if (MapiInspector.MAPIParser.IsFromFiddlerCore(currentSession))
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

                        if (sessionPutExtendContextCount > 0 &&
                            HandleWithSessionPutExtendContextInformation.ContainsKey(parameters))
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
                                MapiInspector.MAPIParser.InputPayLoadCompressedXOR = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].PayLoadCompresssedXOR;
                                PartialPutExtendSession = HandleWithSessionPutExtendContextInformation[parameters][lastSavedSessionID].Session;
                                PartialPutExtendServerUrl = PartialPutExtendSession.RequestHeaders.RequestPath;
                                PartialPutExtendProcessName = PartialPutExtendSession.LocalProcess;
                                PartialPutExtendClientInfo = PartialPutExtendSession.RequestHeaders["X-ClientInfo"];
                                currentSession = MapiInspector.MAPIParser.AllSessions[Convert.ToInt32(PartialPutExtendSession["Number"]) + 1];
                            }

                            if (MapiInspector.MAPIParser.IsFromFiddlerCore(currentSession))
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

                        if (currentSession.RequestHeaders.RequestPath == serverurl &&
                            currentSession.LocalProcess == processName &&
                            currentSession.RequestHeaders["X-ClientInfo"] == clientInfo &&
                            MapiInspector.MAPIParser.IsMapihttpSession(currentSession, MapiInspector.MAPIParser.TrafficDirection.In) &&
                            currentSession.ResponseHeaders["X-RequestType"] == "Execute")
                        {
                            List<uint> tableHandles = new List<uint>();

                            if (MapiInspector.MAPIParser.handlePutDic.ContainsKey(currentSessionID))
                            {
                                tableHandles = MapiInspector.MAPIParser.handlePutDic[currentSessionID];
                            }
                            else
                            {
                                try
                                {
                                    MapiInspector.MAPIParser.IsOnlyGetServerHandle = true;
                                    object mapiRequest = MapiInspector.MAPIParser.ParseRequestMessage(currentSession, out bytesForHexView, false);

                                    if (mapiRequest != null &&
                                        (mapiRequest as ExecuteRequestBody).RopBuffer != null &&
                                        (mapiRequest as ExecuteRequestBody).RopBuffer.Buffers.Count() != 0)
                                    {
                                        tableHandles = ((ROPInputBuffer_WithoutCROPS)(mapiRequest as ExecuteRequestBody).RopBuffer.Buffers[0].Payload).ServerObjectHandleTable.ToList();
                                    }
                                }
                                finally
                                {
                                    MapiInspector.MAPIParser.IsOnlyGetServerHandle = false;
                                }
                            }

                            if (tableHandles.Contains(parameters))
                            {
                                MapiInspector.MAPIParser.ParseRequestMessage(currentSession, out bytesForHexView, true);
                            }
                            else if (tableHandles.Contains(0xffffffff))
                            {
                                List<uint> tablehandleResList = new List<uint>();

                                try
                                {
                                    MapiInspector.MAPIParser.IsOnlyGetServerHandle = true;
                                    object mapiResponse = MapiInspector.MAPIParser.ParseResponseMessage(currentSession, out bytesForHexView, false);

                                    if (mapiResponse != null &&
                                        (mapiResponse as ExecuteResponseBody).RopBuffer != null &&
                                        (mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers.Count() != 0)
                                    {
                                        tableHandles = ((ROPOutputBuffer_WithoutCROPS)(mapiResponse as ExecuteResponseBody).RopBuffer.RgbOutputBuffers[0].Payload).ServerObjectHandleTable.ToList();
                                    }
                                }
                                finally
                                {
                                    MapiInspector.MAPIParser.IsOnlyGetServerHandle = false;
                                }

                                if (tableHandles.Contains(parameters))
                                {
                                    MapiInspector.MAPIParser.ParseRequestMessage(currentSession, out bytesForHexView, true);
                                }
                            }
                        }

                        currentSession = MapiInspector.MAPIParser.AllSessions[Convert.ToInt32(currentSession["Number"]) + 1];
                        if (MapiInspector.MAPIParser.IsFromFiddlerCore(currentSession))
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

                    obj = MapiInspector.MAPIParser.ParseRequestMessage(thisSession, out bytesForHexView, true);
                    DecodingContext.PartialInformationReady = new Dictionary<int, bool>();
                    bytes = bytesForHexView;
                }
            }

            return obj;
        }

        /// <summary>
        /// Clean partial fast transfer stream related dictionaries
        /// </summary>
        public static void ResetPartialContextInformation()
        {
            HandleWithSessionGetContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();
            HandleWithSessionPutContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();
            HandleWithSessionPutExtendContextInformation = new Dictionary<uint, SortedDictionary<int, PartialContextInformation>>();
        }

        /// <summary>
        /// Empty the partial related parameters information
        /// </summary>
        public static void ResetPartialParameters()
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

        public static Block CreatePartialComment()
        {
            var comment = Block.Create("Partial Details");
            if (IsGet) comment.AddHeader("IsGet");
            if (PartialGetType != PropertyDataType.PtypUnspecified) comment.AddHeader($"PartialGetType:{PartialGetType}");
            if (PartialGetId != 0) comment.AddHeader($"PartialGetId:{PartialGetId}");
            if (PartialGetRemainSize != -1) comment.AddHeader($"PartialGetRemainSize:{PartialGetRemainSize:X}");
            if (PartialGetSubRemainSize != -1) comment.AddHeader($"PartialGetSubRemainSize:{PartialGetSubRemainSize:X}");

            if (IsPut) comment.AddHeader("IsPut");
            if (PartialPutExtendType != PropertyDataType.PtypUnspecified) comment.AddHeader($"PartialPutExtendType:{PartialPutExtendType}");
            if (PartialGetId != 0) comment.AddHeader($"PartialPutExtendId:{PartialPutExtendId}");
            if (PartialPutExtendRemainSize != -1) comment.AddHeader($"PartialPutExtendRemainSize:{PartialPutExtendRemainSize:X}");
            if (PartialPutExtendRemainSize != -1) comment.AddHeader($"PartialPutExtendRemainSize:{PartialPutExtendSubRemainSize:X}");

            if (IsPutExtend) comment.AddHeader("IsPutExtend");
            if (PartialPutType != PropertyDataType.PtypUnspecified) comment.AddHeader($"PartialPutType:{PartialPutType}");
            if (PartialPutId != 0) comment.AddHeader($"PartialPutId:{PartialPutId}");
            if (PartialPutRemainSize != -1) comment.AddHeader($"PartialPutRemainSize:{PartialPutRemainSize}");
            if (PartialPutSubRemainSize != -1) comment.AddHeader($"PartialPutSubRemainSize:{PartialPutSubRemainSize:X}");

            if (IsGet) comment.AddHeader($"IsOneMoreByteToRead:{IsOneMoreByteToRead}");
            return comment;
        }
    }
}
