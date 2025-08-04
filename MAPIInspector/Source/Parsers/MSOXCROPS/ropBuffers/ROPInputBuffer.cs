using BlockParser;
using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.1 ROP Input and Output Buffers
    /// A class indicates the ROP input buffer, which is sent by the client, includes an array of ROP request buffers to be processed by the server.
    /// </summary>
    public class ROPInputBuffer : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public BlockT<ushort> RopSize;

        /// <summary>
        /// An array of ROP request buffers.
        /// The size of this field is 2 bytes less than the value specified in the RopSize field.
        /// </summary>
        public Block[] RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public BlockT<uint>[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPInputBuffer structure.
        /// </summary>
        protected override void Parse()
        {
            bool parseToCROPSRequestLayer = false;
            RopSize = ParseT<ushort>();
            var ropsList = new List<Block>();
            var serverObjectHandleTable = new List<BlockT<uint>>();
            var ropRemainSize = new List<uint>();
            var tempServerObjectHandleTable = new List<uint>();
            int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
            }
            int currentPosition = parser.Offset;
            parser.Advance(RopSize - sizeof(ushort));

            while (parser.RemainingBytes >= sizeof(uint))
            {
                uint serverObjectTable = ParseT<uint>();

                if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                {
                    MapiInspector.MAPIParser.IsLooperCall = true;
                    var item = MapiInspector.MAPIParser.TargetHandle.Peek();

                    if (item.First().Value.ContainsValue(serverObjectTable))
                    {
                        parseToCROPSRequestLayer = true;
                    }
                }
                else
                {
                    MapiInspector.MAPIParser.IsLooperCall = false;
                }

                tempServerObjectHandleTable.Add(serverObjectTable);
            }

            parser.Offset = currentPosition;

            if (!MapiInspector.MAPIParser.IsLooperCall || parseToCROPSRequestLayer || MapiInspector.MAPIParser.NeedToParseCROPSLayer)
            {
                var proDics = new Queue<PropertyTag[]>();
                var propertyTagsForGetPropertiesSpec = new Dictionary<uint, Queue<PropertyTag[]>>();
                var logonFlagsInLogonRop = new Dictionary<uint, LogonFlags>();

                // RopSize includes ropSize itself, so we need to subtract sizeof(ushort) to get the actual size of RopsList
                // We need this to be at least sizeof(RopIdType) for the ropID
                if (RopSize - sizeof(ushort) > sizeof(RopIdType))
                {
                    ropRemainSize.Add(RopSize - (uint)sizeof(ushort));

                    parser.PushCap(RopSize - sizeof(ushort));
                    do
                    {
                        var currentRop = TestParse<RopIdType>();

                        switch (currentRop.Data)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                var ropLogonRequest = Parse<RopLogonRequest>();
                                ropsList.Add(ropLogonRequest);

                                // update variables used for parsing RopLogon response
                                if (logonFlagsInLogonRop.Count > 0 &&
                                    logonFlagsInLogonRop.ContainsKey(ropLogonRequest.OutputHandleIndex))
                                {
                                    logonFlagsInLogonRop[ropLogonRequest.OutputHandleIndex] = ropLogonRequest.LogonFlags;
                                }
                                else
                                {
                                    logonFlagsInLogonRop.Add(ropLogonRequest.OutputHandleIndex, ropLogonRequest.LogonFlags);
                                }

                                if (logonFlagsInLogonRop.Count > 0)
                                {
                                    if (DecodingContext.SessionLogonFlagsInLogonRop.ContainsKey(parsingSessionID))
                                    {
                                        DecodingContext.SessionLogonFlagsInLogonRop.Remove(parsingSessionID);
                                    }

                                    DecodingContext.SessionLogonFlagsInLogonRop.Add(parsingSessionID, logonFlagsInLogonRop);
                                }

                                var processNameMap = new Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>();
                                var clientInfoMap = new Dictionary<string, Dictionary<byte, LogonFlags>>();
                                var logIdAndFlags = new Dictionary<byte, LogonFlags>();

                                // update variables used for parsing messages in other ROPs which need logonFlags
                                if (DecodingContext.LogonFlagMapLogId.Count > 0 &&
                                    DecodingContext.LogonFlagMapLogId.ContainsKey(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath))
                                {
                                    processNameMap = DecodingContext.LogonFlagMapLogId[MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath];
                                    DecodingContext.LogonFlagMapLogId.Remove(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath);

                                    if (processNameMap.ContainsKey(MapiInspector.MAPIParser.ParsingSession.LocalProcess))
                                    {
                                        clientInfoMap = processNameMap[MapiInspector.MAPIParser.ParsingSession.LocalProcess];
                                        processNameMap.Remove(MapiInspector.MAPIParser.ParsingSession.LocalProcess);
                                    }

                                    if (clientInfoMap.ContainsKey(MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"]))
                                    {
                                        logIdAndFlags = clientInfoMap[MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"]];
                                        clientInfoMap.Remove(MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"]);
                                    }

                                    if (logIdAndFlags.ContainsKey(ropLogonRequest.LogonId))
                                    {
                                        logIdAndFlags.Remove(ropLogonRequest.LogonId);
                                    }
                                }

                                logIdAndFlags.Add(ropLogonRequest.LogonId, ropLogonRequest.LogonFlags);
                                clientInfoMap.Add(MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"], logIdAndFlags);
                                processNameMap.Add(MapiInspector.MAPIParser.ParsingSession.LocalProcess, clientInfoMap);
                                DecodingContext.LogonFlagMapLogId.Add(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath, processNameMap);
                                break;
                            case RopIdType.RopGetReceiveFolder:
                                ropsList.Add(Parse<RopGetReceiveFolderRequest>());
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                ropsList.Add(Parse<RopSetReceiveFolderRequest>());
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                ropsList.Add(Parse<RopGetReceiveFolderTableRequest>());
                                break;
                            case RopIdType.RopGetStoreState:
                                ropsList.Add(Parse<RopGetStoreStateRequest>());
                                break;
                            case RopIdType.RopGetOwningServers:
                                ropsList.Add(Parse<RopGetOwningServersRequest>());
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                ropsList.Add(Parse<RopPublicFolderIsGhostedRequest>());
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                ropsList.Add(Parse<RopLongTermIdFromIdRequest>());
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                ropsList.Add(Parse<RopIdFromLongTermIdRequest>());
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                ropsList.Add(Parse<RopGetPerUserLongTermIdsRequest>());
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                ropsList.Add(Parse<RopGetPerUserGuidRequest>());
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                ropsList.Add(Parse<RopReadPerUserInformationRequest>());
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                var ropWritePerUserInformationPosition = parser.Offset;
                                var ropId = ParseT<RopIdType>();
                                var logonId = ParseT<byte>();
                                parser.Offset = ropWritePerUserInformationPosition;

                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 &&
                                    DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID) &&
                                    DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logonId)))
                                {
                                    ropsList.Add(MissingInformationException.MaybeThrow(
                                        "Missing LogonFlags information for RopWritePerUserInformation",
                                        currentRop,
                                        new uint[] {
                                            logonId
                                        }));
                                    ropsList.Add(ParseJunk("Remaining Data"));
                                }
                                else
                                {
                                    ropsList.Add(Parse<RopWritePerUserInformationRequest>());
                                }

                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                ropsList.Add(Parse<RopSubmitMessageRequest>());
                                break;
                            case RopIdType.RopAbortSubmit:
                                ropsList.Add(Parse<RopAbortSubmitRequest>());
                                break;
                            case RopIdType.RopGetAddressTypes:
                                ropsList.Add(Parse<RopGetAddressTypesRequest>());
                                break;
                            case RopIdType.RopSetSpooler:
                                ropsList.Add(Parse<RopSetSpoolerRequest>());
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                ropsList.Add(Parse<RopSpoolerLockMessageRequest>());
                                break;
                            case RopIdType.RopTransportSend:
                                ropsList.Add(Parse<RopTransportSendRequest>());
                                break;
                            case RopIdType.RopTransportNewMail:
                                ropsList.Add(Parse<RopTransportNewMailRequest>());
                                break;
                            case RopIdType.RopGetTransportFolder:
                                ropsList.Add(Parse<RopGetTransportFolderRequest>());
                                break;
                            case RopIdType.RopOptionsData:
                                ropsList.Add(Parse<RopOptionsDataRequest>());
                                break;
                            case RopIdType.RopRelease:
                                var ropReleaseRequest = Parse<RopReleaseRequest>();
                                ropsList.Add(ropReleaseRequest);
                                uint handle_Release = tempServerObjectHandleTable[ropReleaseRequest.InputHandleIndex];
                                string serverRequestPath = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;

                                if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(handle_Release))
                                {
                                    var sessions = new List<int>();

                                    foreach (var ele in DecodingContext.RowRops_handlePropertyTags[handle_Release])
                                    {
                                        if (ele.Value.Item1 == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                                            ele.Value.Item2 == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                                            ele.Value.Item3 == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                                        {
                                            sessions.Add(ele.Key);
                                        }
                                    }

                                    if (sessions.Count > 0)
                                    {
                                        Dictionary<int, Tuple<string, string, string, PropertyTag[]>> temp = DecodingContext.RowRops_handlePropertyTags[handle_Release];
                                        DecodingContext.RowRops_handlePropertyTags.Remove(handle_Release);

                                        foreach (int var in sessions)
                                        {
                                            temp.Remove(var);
                                        }

                                        if (temp.Count != 0)
                                        {
                                            DecodingContext.RowRops_handlePropertyTags.Add(handle_Release, temp);
                                        }
                                    }
                                }

                                break;

                            // MSOXCTABL ROPs
                            case RopIdType.RopSetColumns:
                                var ropSetColumnsRequest = Parse<RopSetColumnsRequest>();
                                ropsList.Add(ropSetColumnsRequest);
                                uint handle_SetColumns = tempServerObjectHandleTable[ropSetColumnsRequest.InputHandleIndex];
                                string serverUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;

                                if (handle_SetColumns != 0xFFFFFFFF)
                                {
                                    if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                                    {
                                        var target = MapiInspector.MAPIParser.TargetHandle.Peek();

                                        if (target.First().Key == RopIdType.RopQueryRows ||
                                            target.First().Key == RopIdType.RopFindRow ||
                                            target.First().Key == RopIdType.RopExpandRow)
                                        {
                                            // When the object handle is not equal to 0xFFFFFFFF, add objectHandle and Property Tags to the dictionary.
                                            var sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[]>>();
                                            Tuple<string, string, string, PropertyTag[]> tuples;

                                            if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(handle_SetColumns))
                                            {
                                                sessionTuples = DecodingContext.RowRops_handlePropertyTags[handle_SetColumns];
                                                DecodingContext.RowRops_handlePropertyTags.Remove(handle_SetColumns);

                                                if (sessionTuples.ContainsKey(parsingSessionID))
                                                {
                                                    sessionTuples.Remove(parsingSessionID);
                                                }
                                            }

                                            tuples = new Tuple<string, string, string, PropertyTag[]>(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath, MapiInspector.MAPIParser.ParsingSession.LocalProcess, MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"], ropSetColumnsRequest.PropertyTags);
                                            sessionTuples.Add(parsingSessionID, tuples);
                                            DecodingContext.RowRops_handlePropertyTags.Add(handle_SetColumns, sessionTuples);
                                        }

                                        if (target.First().Key == RopIdType.RopNotify)
                                        {
                                            var sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>();
                                            Tuple<string, string, string, PropertyTag[], string> tuples;

                                            if (DecodingContext.Notify_handlePropertyTags.ContainsKey(handle_SetColumns))
                                            {
                                                sessionTuples = DecodingContext.Notify_handlePropertyTags[handle_SetColumns];
                                                DecodingContext.Notify_handlePropertyTags.Remove(handle_SetColumns);

                                                if (sessionTuples.ContainsKey(parsingSessionID))
                                                {
                                                    sessionTuples.Remove(parsingSessionID);
                                                }
                                            }

                                            tuples = new Tuple<string, string, string, PropertyTag[], string>(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath, MapiInspector.MAPIParser.ParsingSession.LocalProcess, MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"], ropSetColumnsRequest.PropertyTags, string.Empty);
                                            sessionTuples.Add(parsingSessionID, tuples);
                                            DecodingContext.Notify_handlePropertyTags.Add(handle_SetColumns, sessionTuples);
                                        }
                                    }
                                }
                                else if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
                                {
                                    if (MapiInspector.MAPIParser.ParsingSession["X-ResponseCode"] == "0")
                                    {
                                        uint outputHandle;

                                        try
                                        {
                                            MapiInspector.MAPIParser.IsOnlyGetServerHandle = true;
                                            outputHandle = MapiInspector.MAPIParser.ParseResponseMessageSimply(MapiInspector.MAPIParser.ParsingSession, ropSetColumnsRequest.InputHandleIndex);
                                        }
                                        finally
                                        {
                                            MapiInspector.MAPIParser.IsOnlyGetServerHandle = false;
                                        }

                                        if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                                        {
                                            var target = MapiInspector.MAPIParser.TargetHandle.Peek();

                                            if (target.First().Key == RopIdType.RopQueryRows ||
                                                target.First().Key == RopIdType.RopFindRow ||
                                                target.First().Key == RopIdType.RopExpandRow)
                                            {
                                                // This is for Row related rops
                                                var sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[]>>();
                                                Tuple<string, string, string, PropertyTag[]> tuples;

                                                if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(outputHandle))
                                                {
                                                    sessionTuples = DecodingContext.RowRops_handlePropertyTags[outputHandle];
                                                    DecodingContext.RowRops_handlePropertyTags.Remove(outputHandle);

                                                    if (sessionTuples.ContainsKey(parsingSessionID))
                                                    {
                                                        sessionTuples.Remove(parsingSessionID);
                                                    }
                                                }

                                                tuples = new Tuple<string, string, string, PropertyTag[]>(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath, MapiInspector.MAPIParser.ParsingSession.LocalProcess, MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"], ropSetColumnsRequest.PropertyTags);
                                                sessionTuples.Add(parsingSessionID, tuples);
                                                DecodingContext.RowRops_handlePropertyTags.Add(outputHandle, sessionTuples);
                                            }

                                            if (target.First().Key == RopIdType.RopNotify)
                                            {
                                                // This is for ROPNotify
                                                var sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>();
                                                Tuple<string, string, string, PropertyTag[], string> tuples;

                                                if (DecodingContext.Notify_handlePropertyTags.ContainsKey(outputHandle))
                                                {
                                                    sessionTuples = DecodingContext.Notify_handlePropertyTags[outputHandle];
                                                    DecodingContext.Notify_handlePropertyTags.Remove(outputHandle);

                                                    if (sessionTuples.ContainsKey(parsingSessionID))
                                                    {
                                                        sessionTuples.Remove(parsingSessionID);
                                                    }
                                                }

                                                tuples = new Tuple<string, string, string, PropertyTag[], string>(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath, MapiInspector.MAPIParser.ParsingSession.LocalProcess, MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"], ropSetColumnsRequest.PropertyTags, string.Empty);
                                                sessionTuples.Add(parsingSessionID, tuples);
                                                DecodingContext.Notify_handlePropertyTags.Add(outputHandle, sessionTuples);
                                            }
                                        }
                                    }
                                }
                                else if (MapiInspector.MAPIParser.ParsingSession.ResponseHeaders["X-ResponseCode"] == "0")
                                {
                                    uint outputHandle;

                                    try
                                    {
                                        MapiInspector.MAPIParser.IsOnlyGetServerHandle = true;
                                        outputHandle = MapiInspector.MAPIParser.ParseResponseMessageSimply(MapiInspector.MAPIParser.ParsingSession, ropSetColumnsRequest.InputHandleIndex);
                                    }
                                    finally
                                    {
                                        MapiInspector.MAPIParser.IsOnlyGetServerHandle = false;
                                    }

                                    if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                                    {
                                        var target = MapiInspector.MAPIParser.TargetHandle.Peek();

                                        if (target.First().Key == RopIdType.RopQueryRows ||
                                            target.First().Key == RopIdType.RopFindRow ||
                                            target.First().Key == RopIdType.RopExpandRow)
                                        {
                                            // This is for Row related rops
                                            var sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[]>>();
                                            Tuple<string, string, string, PropertyTag[]> tuples;

                                            if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(outputHandle))
                                            {
                                                sessionTuples = DecodingContext.RowRops_handlePropertyTags[outputHandle];
                                                DecodingContext.RowRops_handlePropertyTags.Remove(outputHandle);

                                                if (sessionTuples.ContainsKey(parsingSessionID))
                                                {
                                                    sessionTuples.Remove(parsingSessionID);
                                                }
                                            }

                                            tuples = new Tuple<string, string, string, PropertyTag[]>(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath, MapiInspector.MAPIParser.ParsingSession.LocalProcess, MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"], ropSetColumnsRequest.PropertyTags);
                                            sessionTuples.Add(parsingSessionID, tuples);
                                            DecodingContext.RowRops_handlePropertyTags.Add(outputHandle, sessionTuples);
                                        }

                                        if (target.First().Key == RopIdType.RopNotify)
                                        {
                                            // This is for ROPNotify
                                            var sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>();
                                            Tuple<string, string, string, PropertyTag[], string> tuples;

                                            if (DecodingContext.Notify_handlePropertyTags.ContainsKey(outputHandle))
                                            {
                                                sessionTuples = DecodingContext.Notify_handlePropertyTags[outputHandle];
                                                DecodingContext.Notify_handlePropertyTags.Remove(outputHandle);

                                                if (sessionTuples.ContainsKey(parsingSessionID))
                                                {
                                                    sessionTuples.Remove(parsingSessionID);
                                                }
                                            }

                                            tuples = new Tuple<string, string, string, PropertyTag[], string>(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath, MapiInspector.MAPIParser.ParsingSession.LocalProcess, MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"], ropSetColumnsRequest.PropertyTags, string.Empty);
                                            sessionTuples.Add(parsingSessionID, tuples);
                                            DecodingContext.Notify_handlePropertyTags.Add(outputHandle, sessionTuples);
                                        }
                                    }
                                }

                                break;

                            case RopIdType.RopSortTable:
                                ropsList.Add(Parse<RopSortTableRequest>());
                                break;

                            case RopIdType.RopRestrict:
                                ropsList.Add(Parse<RopRestrictRequest>());
                                break;

                            case RopIdType.RopQueryRows:
                                ropsList.Add(Parse<RopQueryRowsRequest>());
                                break;

                            case RopIdType.RopAbort:
                                ropsList.Add(Parse<RopAbortRequest>());
                                break;

                            case RopIdType.RopGetStatus:
                                ropsList.Add(Parse<RopGetStatusRequest>());
                                break;

                            case RopIdType.RopQueryPosition:
                                ropsList.Add(Parse<RopQueryPositionRequest>());
                                break;

                            case RopIdType.RopSeekRow:
                                ropsList.Add(Parse<RopSeekRowRequest>());
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                ropsList.Add(Parse<RopSeekRowBookmarkRequest>());
                                break;

                            case RopIdType.RopSeekRowFractional:
                                ropsList.Add(Parse<RopSeekRowFractionalRequest>());
                                break;

                            case RopIdType.RopCreateBookmark:
                                ropsList.Add(Parse<RopCreateBookmarkRequest>());
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                ropsList.Add(Parse<RopQueryColumnsAllRequest>());
                                break;

                            case RopIdType.RopFindRow:
                                ropsList.Add(Parse<RopFindRowRequest>());
                                break;

                            case RopIdType.RopFreeBookmark:
                                ropsList.Add(Parse<RopFreeBookmarkRequest>());
                                break;

                            case RopIdType.RopResetTable:
                                ropsList.Add(Parse<RopResetTableRequest>());
                                break;

                            case RopIdType.RopExpandRow:
                                ropsList.Add(Parse<RopExpandRowRequest>());
                                break;

                            case RopIdType.RopCollapseRow:
                                ropsList.Add(Parse<RopCollapseRowRequest>());
                                break;

                            case RopIdType.RopGetCollapseState:
                                ropsList.Add(Parse<RopGetCollapseStateRequest>());
                                break;

                            case RopIdType.RopSetCollapseState:
                                ropsList.Add(Parse<RopSetCollapseStateRequest>());
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                ropsList.Add(Parse<RopModifyRulesRequest>());
                                break;

                            case RopIdType.RopGetRulesTable:
                                ropsList.Add(Parse<RopGetRulesTableRequest>());
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                ropsList.Add(Parse<RopUpdateDeferredActionMessagesRequest>());
                                break;

                            // MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                ropsList.Add(Parse<RopFastTransferSourceCopyPropertiesRequest>());
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                ropsList.Add(Parse<RopFastTransferSourceCopyToRequest>());
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                ropsList.Add(Parse<RopFastTransferSourceCopyMessagesRequest>());
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                ropsList.Add(Parse<RopFastTransferSourceCopyFolderRequest>());
                                break;
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                ropsList.Add(Parse<RopFastTransferSourceGetBufferRequest>());
                                break;
                            case RopIdType.RopTellVersion:
                                ropsList.Add(Parse<RopTellVersionRequest>());
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                ropsList.Add(Parse<RopFastTransferDestinationConfigureRequest>());
                                break;
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                                var currentPos_putBuffer = parser.Offset;
                                parser.Advance(sizeof(RopIdType) + sizeof(byte));
                                var tempInputHandleIndex_putBuffer = ParseT<byte>();
                                parser.Offset = currentPos_putBuffer;
                                uint ropPutbufferHandle = tempServerObjectHandleTable[tempInputHandleIndex_putBuffer];
                                Session destinationParsingSession = MapiInspector.MAPIParser.ParsingSession;
                                int destinationParsingSessionID = parsingSessionID;

                                if (tempServerObjectHandleTable[tempInputHandleIndex_putBuffer] != 0xffffffff)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey(destinationParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException(currentRop, ropPutbufferHandle);
                                    }
                                }
                                else
                                {
                                    Partial.PartialPutType = 0;
                                    Partial.PartialPutRemainSize = -1;
                                    Partial.PartialPutSubRemainSize = -1;
                                }

                                var ropFastTransferDestinationPutBufferRequest = new RopFastTransferDestinationPutBufferRequest();
                                Partial.IsPut = true;
                                ropFastTransferDestinationPutBufferRequest.Parse(parser);
                                ropsList.Add(ropFastTransferDestinationPutBufferRequest);

                                var putBufferPartialInformaiton = new PartialContextInformation(
                                    Partial.PartialPutType,
                                    Partial.PartialPutId,
                                    Partial.PartialPutRemainSize,
                                    Partial.PartialPutSubRemainSize,
                                    false,
                                    destinationParsingSession,
                                    MapiInspector.MAPIParser.InputPayLoadCompressedXOR);
                                var sessionputContextInfor = new SortedDictionary<int, PartialContextInformation>();

                                if (Partial.HandleWithSessionPutContextInformation.ContainsKey(ropPutbufferHandle))
                                {
                                    sessionputContextInfor = Partial.HandleWithSessionPutContextInformation[ropPutbufferHandle];
                                    Partial.HandleWithSessionPutContextInformation.Remove(ropPutbufferHandle);
                                }

                                if (sessionputContextInfor.ContainsKey(destinationParsingSessionID))
                                {
                                    sessionputContextInfor[destinationParsingSessionID] = putBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionputContextInfor.Add(destinationParsingSessionID, putBufferPartialInformaiton);
                                }

                                Partial.HandleWithSessionPutContextInformation.Add(ropPutbufferHandle, sessionputContextInfor);
                                Partial.IsPut = false;
                                break;

                            case RopIdType.RopFastTransferDestinationPutBufferExtended:
                                var currentPos_putBufferExtended = parser.Offset;
                                parser.Advance(sizeof(RopIdType) + sizeof(byte));
                                var tempInputHandleIndex_putBufferExtended = ParseT<byte>();
                                parser.Offset = currentPos_putBufferExtended;
                                uint ropPutExtendbufferHandle = tempServerObjectHandleTable[tempInputHandleIndex_putBufferExtended];
                                int aimsParsingSessionID = parsingSessionID;
                                Session aimsParsingSession = MapiInspector.MAPIParser.ParsingSession;

                                if (tempServerObjectHandleTable[tempInputHandleIndex_putBufferExtended] != 0xffffffff)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey(aimsParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException(currentRop, ropPutExtendbufferHandle);
                                    }
                                }
                                else
                                {
                                    Partial.PartialPutExtendType = 0;
                                    Partial.PartialPutExtendRemainSize = -1;
                                    Partial.PartialPutExtendSubRemainSize = -1;
                                }

                                var ropFastTransferDestinationPutBufferExtendedRequest = new RopFastTransferDestinationPutBufferExtendedRequest();
                                Partial.IsPutExtend = true;
                                ropFastTransferDestinationPutBufferExtendedRequest.Parse(parser);
                                ropsList.Add(ropFastTransferDestinationPutBufferExtendedRequest);

                                var putExtendBufferPartialInformaiton = new PartialContextInformation(
                                    Partial.PartialPutType,
                                    Partial.PartialPutId,
                                    Partial.PartialPutRemainSize,
                                    Partial.PartialPutSubRemainSize,
                                    false, aimsParsingSession,
                                    MapiInspector.MAPIParser.InputPayLoadCompressedXOR);
                                var sessionputExtendContextInfor = new SortedDictionary<int, PartialContextInformation>();

                                if (Partial.HandleWithSessionPutExtendContextInformation.ContainsKey(ropPutExtendbufferHandle))
                                {
                                    sessionputExtendContextInfor = Partial.HandleWithSessionPutExtendContextInformation[ropPutExtendbufferHandle];
                                    Partial.HandleWithSessionPutExtendContextInformation.Remove(ropPutExtendbufferHandle);
                                }

                                if (sessionputExtendContextInfor.ContainsKey(aimsParsingSessionID))
                                {
                                    sessionputExtendContextInfor[aimsParsingSessionID] = putExtendBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionputExtendContextInfor.Add(aimsParsingSessionID, putExtendBufferPartialInformaiton);
                                }

                                Partial.HandleWithSessionPutExtendContextInformation.Add(ropPutExtendbufferHandle, sessionputExtendContextInfor);
                                Partial.IsPutExtend = false;
                                break;

                            case RopIdType.RopSynchronizationConfigure:
                                ropsList.Add(Parse<RopSynchronizationConfigureRequest>());
                                break;

                            case RopIdType.RopSynchronizationGetTransferState:
                                ropsList.Add(Parse<RopSynchronizationGetTransferStateRequest>());
                                break;

                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                ropsList.Add(Parse<RopSynchronizationUploadStateStreamBeginRequest>());
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                ropsList.Add(Parse<RopSynchronizationUploadStateStreamContinueRequest>());
                                break;

                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                ropsList.Add(Parse<RopSynchronizationUploadStateStreamEndRequest>());
                                break;

                            case RopIdType.RopSynchronizationOpenCollector:
                                ropsList.Add(Parse<RopSynchronizationOpenCollectorRequest>());
                                break;

                            case RopIdType.RopSynchronizationImportMessageChange:
                                ropsList.Add(Parse<RopSynchronizationImportMessageChangeRequest>());
                                break;

                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                ropsList.Add(Parse<RopSynchronizationImportHierarchyChangeRequest>());
                                break;

                            case RopIdType.RopSynchronizationImportMessageMove:
                                ropsList.Add(Parse<RopSynchronizationImportMessageMoveRequest>());
                                break;

                            case RopIdType.RopSynchronizationImportDeletes:
                                ropsList.Add(Parse<RopSynchronizationImportDeletesRequest>());
                                break;

                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                ropsList.Add(Parse<RopSynchronizationImportReadStateChangesRequest>());
                                break;

                            case RopIdType.RopGetLocalReplicaIds:
                                ropsList.Add(Parse<RopGetLocalReplicaIdsRequest>());
                                break;

                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                ropsList.Add(Parse<RopSetLocalReplicaMidsetDeletedRequest>());
                                break;

                            // MS-OXCPRPT ROPs
                            case RopIdType.RopGetPropertiesSpecific:
                                var ropGetPropertiesSpecificRequest = Parse<RopGetPropertiesSpecificRequest>();
                                ropsList.Add(ropGetPropertiesSpecificRequest);

                                if (propertyTagsForGetPropertiesSpec.ContainsKey(ropGetPropertiesSpecificRequest.InputHandleIndex))
                                {
                                    if (propertyTagsForGetPropertiesSpec[ropGetPropertiesSpecificRequest.InputHandleIndex].Count == 1)
                                    {
                                        proDics.Enqueue(propertyTagsForGetPropertiesSpec[ropGetPropertiesSpecificRequest.InputHandleIndex].Dequeue());
                                    }

                                    proDics.Enqueue(ropGetPropertiesSpecificRequest.PropertyTags);
                                    propertyTagsForGetPropertiesSpec[ropGetPropertiesSpecificRequest.InputHandleIndex] = proDics;
                                }
                                else
                                {
                                    var proDic0 = new Queue<PropertyTag[]>();
                                    proDic0.Enqueue(ropGetPropertiesSpecificRequest.PropertyTags);
                                    propertyTagsForGetPropertiesSpec.Add(ropGetPropertiesSpecificRequest.InputHandleIndex, proDic0);
                                }

                                if (propertyTagsForGetPropertiesSpec.Count > 0)
                                {
                                    if (DecodingContext.GetPropertiesSpec_propertyTags.ContainsKey(parsingSessionID))
                                    {
                                        DecodingContext.GetPropertiesSpec_propertyTags.Remove(parsingSessionID);
                                    }

                                    DecodingContext.GetPropertiesSpec_propertyTags.Add(parsingSessionID, propertyTagsForGetPropertiesSpec);
                                }

                                break;

                            case RopIdType.RopGetPropertiesAll:
                                ropsList.Add(Parse<RopGetPropertiesAllRequest>());
                                break;

                            case RopIdType.RopGetPropertiesList:
                                ropsList.Add(Parse<RopGetPropertiesListRequest>());
                                break;

                            case RopIdType.RopSetProperties:
                                ropsList.Add(Parse<RopSetPropertiesRequest>());
                                break;

                            case RopIdType.RopSetPropertiesNoReplicate:
                                ropsList.Add(Parse<RopSetPropertiesNoReplicateRequest>());
                                break;

                            case RopIdType.RopDeleteProperties:
                                ropsList.Add(Parse<RopDeletePropertiesRequest>());
                                break;

                            case RopIdType.RopDeletePropertiesNoReplicate:
                                ropsList.Add(Parse<RopDeletePropertiesNoReplicateRequest>());
                                break;

                            case RopIdType.RopQueryNamedProperties:
                                ropsList.Add(Parse<RopQueryNamedPropertiesRequest>());
                                break;
                            case RopIdType.RopCopyProperties:
                                ropsList.Add(Parse<RopCopyPropertiesRequest>());
                                break;

                            case RopIdType.RopCopyTo:
                                ropsList.Add(Parse<RopCopyToRequest>());
                                break;

                            case RopIdType.RopGetPropertyIdsFromNames:
                                ropsList.Add(Parse<RopGetPropertyIdsFromNamesRequest>());
                                break;

                            case RopIdType.RopGetNamesFromPropertyIds:
                                ropsList.Add(Parse<RopGetNamesFromPropertyIdsRequest>());
                                break;

                            case RopIdType.RopOpenStream:
                                ropsList.Add(Parse<RopOpenStreamRequest>());
                                break;

                            case RopIdType.RopReadStream:
                                ropsList.Add(Parse<RopReadStreamRequest>());
                                break;

                            case RopIdType.RopWriteStream:
                                ropsList.Add(Parse<RopWriteStreamRequest>());
                                break;

                            case RopIdType.RopWriteStreamExtended:
                                ropsList.Add(Parse<RopWriteStreamExtendedRequest>());
                                break;

                            case RopIdType.RopCommitStream:
                                ropsList.Add(Parse<RopCommitStreamRequest>());
                                break;

                            case RopIdType.RopGetStreamSize:
                                ropsList.Add(Parse<RopGetStreamSizeRequest>());
                                break;

                            case RopIdType.RopSetStreamSize:
                                ropsList.Add(Parse<RopSetStreamSizeRequest>());
                                break;

                            case RopIdType.RopSeekStream:
                                ropsList.Add(Parse<RopSeekStreamRequest>());
                                break;
                            case RopIdType.RopCopyToStream:
                                ropsList.Add(Parse<RopCopyToStreamRequest>());
                                break;

                            case RopIdType.RopProgress:
                                ropsList.Add(Parse<RopProgressRequest>());
                                break;

                            case RopIdType.RopLockRegionStream:
                                ropsList.Add(Parse<RopLockRegionStreamRequest>());
                                break;

                            case RopIdType.RopUnlockRegionStream:
                                ropsList.Add(Parse<RopUnlockRegionStreamRequest>());
                                break;

                            case RopIdType.RopWriteAndCommitStream:
                                ropsList.Add(Parse<RopWriteAndCommitStreamRequest>());
                                break;

                            case RopIdType.RopCloneStream:
                                ropsList.Add(Parse<RopCloneStreamRequest>());
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                ropsList.Add(Parse<RopOpenFolderRequest>());
                                break;

                            case RopIdType.RopCreateFolder:
                                ropsList.Add(Parse<RopCreateFolderRequest>());
                                break;

                            case RopIdType.RopDeleteFolder:
                                ropsList.Add(Parse<RopDeleteFolderRequest>());
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                ropsList.Add(Parse<RopSetSearchCriteriaRequest>());
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                ropsList.Add(Parse<RopGetSearchCriteriaRequest>());
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                ropsList.Add(Parse<RopMoveCopyMessagesRequest>());
                                break;

                            case RopIdType.RopMoveFolder:
                                ropsList.Add(Parse<RopMoveFolderRequest>());
                                break;

                            case RopIdType.RopCopyFolder:
                                ropsList.Add(Parse<RopCopyFolderRequest>());
                                break;

                            case RopIdType.RopEmptyFolder:
                                ropsList.Add(Parse<RopEmptyFolderRequest>());
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                ropsList.Add(Parse<RopHardDeleteMessagesAndSubfoldersRequest>());
                                break;

                            case RopIdType.RopDeleteMessages:
                                ropsList.Add(Parse<RopDeleteMessagesRequest>());
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                ropsList.Add(Parse<RopHardDeleteMessagesRequest>());
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                ropsList.Add(Parse<RopGetHierarchyTableRequest>());
                                break;

                            case RopIdType.RopGetContentsTable:
                                ropsList.Add(Parse<RopGetContentsTableRequest>());
                                break;

                            // MS-OXCMSG ROPs
                            case RopIdType.RopOpenMessage:
                                ropsList.Add(Parse<RopOpenMessageRequest>());
                                break;

                            case RopIdType.RopCreateMessage:
                                ropsList.Add(Parse<RopCreateMessageRequest>());
                                break;

                            case RopIdType.RopSaveChangesMessage:
                                ropsList.Add(Parse<RopSaveChangesMessageRequest>());
                                break;

                            case RopIdType.RopRemoveAllRecipients:
                                ropsList.Add(Parse<RopRemoveAllRecipientsRequest>());
                                break;

                            case RopIdType.RopModifyRecipients:
                                ropsList.Add(Parse<RopModifyRecipientsRequest>());
                                break;

                            case RopIdType.RopReadRecipients:
                                ropsList.Add(Parse<RopReadRecipientsRequest>());
                                break;

                            case RopIdType.RopReloadCachedInformation:
                                ropsList.Add(Parse<RopReloadCachedInformationRequest>());
                                break;

                            case RopIdType.RopSetMessageStatus:
                                ropsList.Add(Parse<RopSetMessageStatusRequest>());
                                break;

                            case RopIdType.RopGetMessageStatus:
                                ropsList.Add(Parse<RopGetMessageStatusRequest>());
                                break;

                            case RopIdType.RopSetReadFlags:
                                ropsList.Add(Parse<RopSetReadFlagsRequest>());
                                break;

                            case RopIdType.RopSetMessageReadFlag:
                                var ropSetMessageReadFlagPosition = parser.Offset;
                                var ropId_setReadFlag = ParseT<RopIdType>();
                                var logId = ParseT<byte>();
                                parser.Offset = ropSetMessageReadFlagPosition;
                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 &&
                                    DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID) &&
                                    DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logId)))
                                {
                                    ropsList.Add(MissingInformationException.MaybeThrow(
                                        "Missing LogonFlags information for RopSetMessageReadFlag",
                                        currentRop,
                                        new uint[] {
                                            logId }
                                        ));
                                    ropsList.Add(ParseJunk("Remaining Data"));
                                }
                                else
                                {
                                    ropsList.Add(Parse<RopSetMessageReadFlagRequest>());
                                }

                                break;

                            case RopIdType.RopOpenAttachment:
                                ropsList.Add(Parse<RopOpenAttachmentRequest>());
                                break;

                            case RopIdType.RopCreateAttachment:
                                ropsList.Add(Parse<RopCreateAttachmentRequest>());
                                break;

                            case RopIdType.RopDeleteAttachment:
                                ropsList.Add(Parse<RopDeleteAttachmentRequest>());
                                break;

                            case RopIdType.RopSaveChangesAttachment:
                                ropsList.Add(Parse<RopSaveChangesAttachmentRequest>());
                                break;

                            case RopIdType.RopOpenEmbeddedMessage:
                                ropsList.Add(Parse<RopOpenEmbeddedMessageRequest>());
                                break;

                            case RopIdType.RopGetAttachmentTable:
                                ropsList.Add(Parse<RopGetAttachmentTableRequest>());
                                break;

                            case RopIdType.RopGetValidAttachments:
                                ropsList.Add(Parse<RopGetValidAttachmentsRequest>());
                                break;

                            // MSOXCNOTIF ROPs
                            case RopIdType.RopRegisterNotification:
                                ropsList.Add(Parse<RopRegisterNotificationRequest>());
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                ropsList.Add(Parse<RopGetPermissionsTableRequest>());
                                break;

                            case RopIdType.RopModifyPermissions:
                                ropsList.Add(Parse<RopModifyPermissionsRequest>());
                                break;

                            default:
                                ropsList.Add(ParseJunk("Remaining Data"));
                                break;
                        }

                        if (currentRop == RopIdType.RopRelease)
                        {
                            ropRemainSize.RemoveAt(ropRemainSize.Count - 1);
                        }

                        ropRemainSize.Add(RopSize - (uint)parser.Offset);
                    }
                    while (parser.RemainingBytes > 0);

                    parser.PopCap();
                }
                else
                {
                    RopsList = null;
                }

                if (DecodingContext.SessionRequestRemainSize.ContainsKey(parsingSessionID))
                {
                    DecodingContext.SessionRequestRemainSize.Remove(parsingSessionID);
                }

                DecodingContext.SessionRequestRemainSize.Add(parsingSessionID, ropRemainSize);
                RopsList = ropsList.ToArray();
            }
            else
            {
                ropsList.Add(ParseBytes(RopSize - sizeof(RopIdType)));
            }

            RopsList = ropsList.ToArray();

            if (RopsList.Length != 0)
            {
                foreach (var obj in ropsList)
                {
                    if (MapiInspector.MAPIParser.AllRopsList.Count <= 0 ||
                        !MapiInspector.MAPIParser.AllRopsList.Contains(obj.GetType().Name))
                    {
                        MapiInspector.MAPIParser.AllRopsList.Add(obj.GetType().Name);
                    }
                }
            }

            while (parser.RemainingBytes >= sizeof(uint))
            {
                serverObjectHandleTable.Add(ParseT<uint>());
            }

            ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "ROPInputBuffer";
            AddChildBlockT(RopSize, "RopSize");
            AddLabeledChildren(RopsList, "RopsList");
            AddLabeledChildren(ServerObjectHandleTable, "ServerObjectHandleTable");
        }
    }
}
