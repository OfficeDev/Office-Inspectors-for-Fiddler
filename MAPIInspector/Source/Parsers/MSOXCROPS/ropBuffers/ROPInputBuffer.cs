using BlockParser;
using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ROP input buffer, which is sent by the client, includes an array of ROP request buffers to be processed by the server.
    /// </summary>
    public class ROPInputBuffer : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public ushort RopSize;

        /// <summary>
        /// An array of ROP request buffers.
        /// </summary>
        /// </summary
        public object[] RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPInputBuffer structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPInputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            bool parseToCROPSRequestLayer = false;
            RopSize = ReadUshort();
            var ropsList = new List<object>();
            var serverObjectHandleTable = new List<uint>();
            var ropRemainSize = new List<uint>();
            var tempServerObjectHandleTable = new List<uint>();
            int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
            }
            long currentPosition = s.Position;
            s.Position += RopSize - 2;

            while (s.Position < s.Length)
            {
                uint serverObjectTable = ReadUint();

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

            s.Position = currentPosition;

            if (!MapiInspector.MAPIParser.IsLooperCall ||
                parseToCROPSRequestLayer ||
                MapiInspector.MAPIParser.NeedToParseCROPSLayer)
            {
                var proDics = new Queue<PropertyTag[]>();
                var propertyTagsForGetPropertiesSpec = new Dictionary<uint, Queue<PropertyTag[]>>();
                var logonFlagsInLogonRop = new Dictionary<uint, LogonFlags>();

                if (RopSize > 2)
                {
                    ropRemainSize.Add(RopSize - (uint)2);

                    do
                    {
                        var currentByte = (RopIdType)s.ReadByte();
                        s.Position -= 1;

                        switch (currentByte)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                var ropLogonRequest = new RopLogonRequest();
                                ropLogonRequest.Parse(s);
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
                                var ropGetReceiveFolderRequest = new RopGetReceiveFolderRequest();
                                ropGetReceiveFolderRequest.Parse(s);
                                ropsList.Add(ropGetReceiveFolderRequest);
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                var ropSetReceiveFolderRequest = new RopSetReceiveFolderRequest();
                                ropSetReceiveFolderRequest.Parse(s);
                                ropsList.Add(ropSetReceiveFolderRequest);
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                var ropGetReceiveFolderTableRequest = new RopGetReceiveFolderTableRequest();
                                ropGetReceiveFolderTableRequest.Parse(s);
                                ropsList.Add(ropGetReceiveFolderTableRequest);
                                break;
                            case RopIdType.RopGetStoreState:
                                var ropGetStoreStateRequest = new RopGetStoreStateRequest();
                                ropGetStoreStateRequest.Parse(s);
                                ropsList.Add(ropGetStoreStateRequest);
                                break;
                            case RopIdType.RopGetOwningServers:
                                var ropGetOwningServersRequest = new RopGetOwningServersRequest();
                                ropGetOwningServersRequest.Parse(s);
                                ropsList.Add(ropGetOwningServersRequest);
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                var ropPublicFolderIsGhostedRequest = new RopPublicFolderIsGhostedRequest();
                                ropPublicFolderIsGhostedRequest.Parse(s);
                                ropsList.Add(ropPublicFolderIsGhostedRequest);
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                var ropLongTermIdFromIdRequest = new RopLongTermIdFromIdRequest();
                                ropLongTermIdFromIdRequest.Parse(s);
                                ropsList.Add(ropLongTermIdFromIdRequest);
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                var ropIdFromLongTermIdRequest = new RopIdFromLongTermIdRequest();
                                ropIdFromLongTermIdRequest.Parse(s);
                                ropsList.Add(ropIdFromLongTermIdRequest);
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                var ropGetPerUserLongTermIdsRequest = new RopGetPerUserLongTermIdsRequest();
                                ropGetPerUserLongTermIdsRequest.Parse(s);
                                ropsList.Add(ropGetPerUserLongTermIdsRequest);
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                var ropGetPerUserGuidRequest = new RopGetPerUserGuidRequest();
                                ropGetPerUserGuidRequest.Parse(s);
                                ropsList.Add(ropGetPerUserGuidRequest);
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                var ropReadPerUserInformationRequest = new RopReadPerUserInformationRequest();
                                ropReadPerUserInformationRequest.Parse(s);
                                ropsList.Add(ropReadPerUserInformationRequest);
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                byte ropId = ReadByte();
                                byte logonId = ReadByte();
                                s.Position -= 2;

                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 &&
                                    DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID) &&
                                    DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logonId)))
                                {
                                    throw new MissingInformationException(
                                        "Missing LogonFlags information for RopWritePerUserInformation",
                                        currentByte,
                                        new uint[] {
                                            logonId
                                        });
                                }

                                var ropWritePerUserInformationRequest = new RopWritePerUserInformationRequest();
                                ropWritePerUserInformationRequest.Parse(s);
                                ropsList.Add(ropWritePerUserInformationRequest);
                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                var ropSubmitMessageRequest = new RopSubmitMessageRequest();
                                ropSubmitMessageRequest.Parse(s);
                                ropsList.Add(ropSubmitMessageRequest);
                                break;
                            case RopIdType.RopAbortSubmit:
                                var ropAbortSubmitRequest = new RopAbortSubmitRequest();
                                ropAbortSubmitRequest.Parse(s);
                                ropsList.Add(ropAbortSubmitRequest);
                                break;
                            case RopIdType.RopGetAddressTypes:
                                var ropGetAddressTypesRequest = new RopGetAddressTypesRequest();
                                ropGetAddressTypesRequest.Parse(s);
                                ropsList.Add(ropGetAddressTypesRequest);
                                break;
                            case RopIdType.RopSetSpooler:
                                var ropSetSpoolerRequest = new RopSetSpoolerRequest();
                                ropSetSpoolerRequest.Parse(s);
                                ropsList.Add(ropSetSpoolerRequest);
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                var ropSpoolerLockMessageRequest = new RopSpoolerLockMessageRequest();
                                ropSpoolerLockMessageRequest.Parse(s);
                                ropsList.Add(ropSpoolerLockMessageRequest);
                                break;
                            case RopIdType.RopTransportSend:
                                var ropTransportSendRequest = new RopTransportSendRequest();
                                ropTransportSendRequest.Parse(s);
                                ropsList.Add(ropTransportSendRequest);
                                break;
                            case RopIdType.RopTransportNewMail:
                                var ropTransportNewMailRequest = new RopTransportNewMailRequest();
                                ropTransportNewMailRequest.Parse(s);
                                ropsList.Add(ropTransportNewMailRequest);
                                break;
                            case RopIdType.RopGetTransportFolder:
                                var ropGetTransportFolderRequest = new RopGetTransportFolderRequest();
                                ropGetTransportFolderRequest.Parse(s);
                                ropsList.Add(ropGetTransportFolderRequest);
                                break;
                            case RopIdType.RopOptionsData:
                                var ropOptionsDataRequest = new RopOptionsDataRequest();
                                ropOptionsDataRequest.Parse(s);
                                ropsList.Add(ropOptionsDataRequest);
                                break;
                            case RopIdType.RopRelease:
                                var ropReleaseRequest = Block.Parse<RopReleaseRequest>(s);
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
                                var ropSetColumnsRequest = Block.Parse<RopSetColumnsRequest>(s);
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
                                            outputHandle = MapiInspector.MAPIParser.ParseResponseMessageSimplely(MapiInspector.MAPIParser.ParsingSession, ropSetColumnsRequest.InputHandleIndex);
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
                                        outputHandle = MapiInspector.MAPIParser.ParseResponseMessageSimplely(MapiInspector.MAPIParser.ParsingSession, ropSetColumnsRequest.InputHandleIndex);
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
                                var ropSortTableRequest = new RopSortTableRequest();
                                ropSortTableRequest.Parse(s);
                                ropsList.Add(ropSortTableRequest);
                                break;

                            case RopIdType.RopRestrict:
                                var ropRestrictRequest = new RopRestrictRequest();
                                ropRestrictRequest.Parse(s);
                                ropsList.Add(ropRestrictRequest);
                                break;

                            case RopIdType.RopQueryRows:
                                ropsList.Add(Block.Parse<RopQueryRowsRequest>(s));
                                break;

                            case RopIdType.RopAbort:
                                var ropAbortRequest = new RopAbortRequest();
                                ropAbortRequest.Parse(s);
                                ropsList.Add(ropAbortRequest);
                                break;

                            case RopIdType.RopGetStatus:
                                var ropGetStatusRequest = new RopGetStatusRequest();
                                ropGetStatusRequest.Parse(s);
                                ropsList.Add(ropGetStatusRequest);
                                break;

                            case RopIdType.RopQueryPosition:
                                var ropQueryPositionRequest = new RopQueryPositionRequest();
                                ropQueryPositionRequest.Parse(s);
                                ropsList.Add(ropQueryPositionRequest);
                                break;

                            case RopIdType.RopSeekRow:
                                ropsList.Add(Block.Parse<RopSeekRowRequest>(s));
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                var ropSeekRowBookmarkRequest = new RopSeekRowBookmarkRequest();
                                ropSeekRowBookmarkRequest.Parse(s);
                                ropsList.Add(ropSeekRowBookmarkRequest);
                                break;

                            case RopIdType.RopSeekRowFractional:
                                var ropSeekRowFractionalRequest = new RopSeekRowFractionalRequest();
                                ropSeekRowFractionalRequest.Parse(s);
                                ropsList.Add(ropSeekRowFractionalRequest);
                                break;

                            case RopIdType.RopCreateBookmark:
                                var ropCreateBookmarkRequest = new RopCreateBookmarkRequest();
                                ropCreateBookmarkRequest.Parse(s);
                                ropsList.Add(ropCreateBookmarkRequest);
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                var ropQueryColumnsAllRequest = new RopQueryColumnsAllRequest();
                                ropQueryColumnsAllRequest.Parse(s);
                                ropsList.Add(ropQueryColumnsAllRequest);
                                break;

                            case RopIdType.RopFindRow:
                                var ropFindRowRequest = new RopFindRowRequest();
                                ropFindRowRequest.Parse(s);
                                ropsList.Add(ropFindRowRequest);
                                break;

                            case RopIdType.RopFreeBookmark:
                                var ropFreeBookmarkRequest = new RopFreeBookmarkRequest();
                                ropFreeBookmarkRequest.Parse(s);
                                ropsList.Add(ropFreeBookmarkRequest);
                                break;

                            case RopIdType.RopResetTable:
                                var ropResetTableRequest = new RopResetTableRequest();
                                ropResetTableRequest.Parse(s);
                                ropsList.Add(ropResetTableRequest);
                                break;

                            case RopIdType.RopExpandRow:
                                var ropExpandRowRequest = new RopExpandRowRequest();
                                ropExpandRowRequest.Parse(s);
                                ropsList.Add(ropExpandRowRequest);
                                break;

                            case RopIdType.RopCollapseRow:
                                var ropCollapseRowRequest = new RopCollapseRowRequest();
                                ropCollapseRowRequest.Parse(s);
                                ropsList.Add(ropCollapseRowRequest);
                                break;

                            case RopIdType.RopGetCollapseState:
                                var ropGetCollapseStateRequest = new RopGetCollapseStateRequest();
                                ropGetCollapseStateRequest.Parse(s);
                                ropsList.Add(ropGetCollapseStateRequest);
                                break;

                            case RopIdType.RopSetCollapseState:
                                var ropSetCollapseStateRequest = new RopSetCollapseStateRequest();
                                ropSetCollapseStateRequest.Parse(s);
                                ropsList.Add(ropSetCollapseStateRequest);
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                var ropModifyRulesRequest = new RopModifyRulesRequest();
                                ropModifyRulesRequest.Parse(s);
                                ropsList.Add(ropModifyRulesRequest);
                                break;

                            case RopIdType.RopGetRulesTable:
                                var ropGetRulesTableRequest = new RopGetRulesTableRequest();
                                ropGetRulesTableRequest.Parse(s);
                                ropsList.Add(ropGetRulesTableRequest);
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                var ropUpdateDeferredActionMessagesRequest = new RopUpdateDeferredActionMessagesRequest();
                                ropUpdateDeferredActionMessagesRequest.Parse(s);
                                ropsList.Add(ropUpdateDeferredActionMessagesRequest);
                                break;

                            // MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                var ropFastTransferSourceCopyPropertiesRequest = new RopFastTransferSourceCopyPropertiesRequest();
                                ropFastTransferSourceCopyPropertiesRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyPropertiesRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                var ropFastTransferSourceCopyToRequest = new RopFastTransferSourceCopyToRequest();
                                ropFastTransferSourceCopyToRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyToRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                var ropFastTransferSourceCopyMessagesRequest = new RopFastTransferSourceCopyMessagesRequest();
                                ropFastTransferSourceCopyMessagesRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyMessagesRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                var ropFastTransferSourceCopyFolderRequest = new RopFastTransferSourceCopyFolderRequest();
                                ropFastTransferSourceCopyFolderRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyFolderRequest);
                                break;
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                var ropFastTransferSourceGetBufferRequest = new RopFastTransferSourceGetBufferRequest();
                                ropFastTransferSourceGetBufferRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceGetBufferRequest);
                                break;
                            case RopIdType.RopTellVersion:
                                var ropTellVersionRequest = new RopTellVersionRequest();
                                ropTellVersionRequest.Parse(s);
                                ropsList.Add(ropTellVersionRequest);
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                var ropFastTransferDestinationConfigureRequest = new RopFastTransferDestinationConfigureRequest();
                                ropFastTransferDestinationConfigureRequest.Parse(s);
                                ropsList.Add(ropFastTransferDestinationConfigureRequest);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                                long currentPos_putBuffer = s.Position;
                                s.Position += 2;
                                int tempInputHandleIndex_putBuffer = s.ReadByte();
                                s.Position = currentPos_putBuffer;
                                uint ropPutbufferHandle = tempServerObjectHandleTable[tempInputHandleIndex_putBuffer];
                                Session destinationParsingSession = MapiInspector.MAPIParser.ParsingSession;
                                int destinationParsingSessionID = parsingSessionID;

                                if (tempServerObjectHandleTable[tempInputHandleIndex_putBuffer] != 0xffffffff)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey(destinationParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException(currentByte, ropPutbufferHandle);
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
                                ropFastTransferDestinationPutBufferRequest.Parse(s);
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
                                long currentPos_putBufferExtended = s.Position;
                                s.Position += 2;
                                int tempInputHandleIndex_putBufferExtended = s.ReadByte();
                                s.Position = currentPos_putBufferExtended;
                                uint ropPutExtendbufferHandle = tempServerObjectHandleTable[tempInputHandleIndex_putBufferExtended];
                                int aimsParsingSessionID = parsingSessionID;
                                Session aimsParsingSession = MapiInspector.MAPIParser.ParsingSession;

                                if (tempServerObjectHandleTable[tempInputHandleIndex_putBufferExtended] != 0xffffffff)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey(aimsParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException(currentByte, ropPutExtendbufferHandle);
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
                                ropFastTransferDestinationPutBufferExtendedRequest.Parse(s);
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
                                var ropSynchronizationConfigureRequest = new RopSynchronizationConfigureRequest();
                                ropSynchronizationConfigureRequest.Parse(s);
                                ropsList.Add(ropSynchronizationConfigureRequest);
                                break;

                            case RopIdType.RopSynchronizationGetTransferState:
                                var ropSynchronizationGetTransferStateRequest = new RopSynchronizationGetTransferStateRequest();
                                ropSynchronizationGetTransferStateRequest.Parse(s);
                                ropsList.Add(ropSynchronizationGetTransferStateRequest);
                                break;

                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                var ropSynchronizationUploadStateStreamBeginRequest = new RopSynchronizationUploadStateStreamBeginRequest();
                                ropSynchronizationUploadStateStreamBeginRequest.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamBeginRequest);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                var ropSynchronizationUploadStateStreamContinueRequest = new RopSynchronizationUploadStateStreamContinueRequest();
                                ropSynchronizationUploadStateStreamContinueRequest.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamContinueRequest);
                                break;

                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                var ropSynchronizationUploadStateStreamEndRequest = new RopSynchronizationUploadStateStreamEndRequest();
                                ropSynchronizationUploadStateStreamEndRequest.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamEndRequest);
                                break;

                            case RopIdType.RopSynchronizationOpenCollector:
                                var ropSynchronizationOpenCollectorRequest = new RopSynchronizationOpenCollectorRequest();
                                ropSynchronizationOpenCollectorRequest.Parse(s);
                                ropsList.Add(ropSynchronizationOpenCollectorRequest);
                                break;

                            case RopIdType.RopSynchronizationImportMessageChange:
                                var ropSynchronizationImportMessageChangeRequest = new RopSynchronizationImportMessageChangeRequest();
                                ropSynchronizationImportMessageChangeRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportMessageChangeRequest);
                                break;

                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                var ropSynchronizationImportHierarchyChangeRequest = new RopSynchronizationImportHierarchyChangeRequest();
                                ropSynchronizationImportHierarchyChangeRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportHierarchyChangeRequest);
                                break;

                            case RopIdType.RopSynchronizationImportMessageMove:
                                var ropSynchronizationImportMessageMoveRequest = new RopSynchronizationImportMessageMoveRequest();
                                ropSynchronizationImportMessageMoveRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportMessageMoveRequest);
                                break;

                            case RopIdType.RopSynchronizationImportDeletes:
                                var ropSynchronizationImportDeletesRequest = new RopSynchronizationImportDeletesRequest();
                                ropSynchronizationImportDeletesRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportDeletesRequest);
                                break;

                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                var ropSynchronizationImportReadStateChangesRequest = new RopSynchronizationImportReadStateChangesRequest();
                                ropSynchronizationImportReadStateChangesRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportReadStateChangesRequest);
                                break;

                            case RopIdType.RopGetLocalReplicaIds:
                                var ropGetLocalReplicaIdsRequest = new RopGetLocalReplicaIdsRequest();
                                ropGetLocalReplicaIdsRequest.Parse(s);
                                ropsList.Add(ropGetLocalReplicaIdsRequest);
                                break;

                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                var ropSetLocalReplicaMidsetDeletedRequest = new RopSetLocalReplicaMidsetDeletedRequest();
                                ropSetLocalReplicaMidsetDeletedRequest.Parse(s);
                                ropsList.Add(ropSetLocalReplicaMidsetDeletedRequest);
                                break;

                            // MS-OXCPRPT ROPs
                            case RopIdType.RopGetPropertiesSpecific:
                                var ropGetPropertiesSpecificRequest = new RopGetPropertiesSpecificRequest();
                                ropGetPropertiesSpecificRequest.Parse(s);
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
                                var ropGetPropertiesAllRequest = new RopGetPropertiesAllRequest();
                                ropGetPropertiesAllRequest.Parse(s);
                                ropsList.Add(ropGetPropertiesAllRequest);
                                break;

                            case RopIdType.RopGetPropertiesList:
                                var ropGetPropertiesListRequest = new RopGetPropertiesListRequest();
                                ropGetPropertiesListRequest.Parse(s);
                                ropsList.Add(ropGetPropertiesListRequest);
                                break;

                            case RopIdType.RopSetProperties:
                                var ropSetPropertiesRequest = new RopSetPropertiesRequest();
                                ropSetPropertiesRequest.Parse(s);
                                ropsList.Add(ropSetPropertiesRequest);
                                break;

                            case RopIdType.RopSetPropertiesNoReplicate:
                                var ropSetPropertiesNoReplicateRequest = new RopSetPropertiesNoReplicateRequest();
                                ropSetPropertiesNoReplicateRequest.Parse(s);
                                ropsList.Add(ropSetPropertiesNoReplicateRequest);
                                break;

                            case RopIdType.RopDeleteProperties:
                                var ropDeletePropertiesRequest = new RopDeletePropertiesRequest();
                                ropDeletePropertiesRequest.Parse(s);
                                ropsList.Add(ropDeletePropertiesRequest);
                                break;

                            case RopIdType.RopDeletePropertiesNoReplicate:
                                var ropDeletePropertiesNoReplicateRequest = new RopDeletePropertiesNoReplicateRequest();
                                ropDeletePropertiesNoReplicateRequest.Parse(s);
                                ropsList.Add(ropDeletePropertiesNoReplicateRequest);
                                break;

                            case RopIdType.RopQueryNamedProperties:
                                var ropQueryNamedPropertiesRequest = new RopQueryNamedPropertiesRequest();
                                ropQueryNamedPropertiesRequest.Parse(s);
                                ropsList.Add(ropQueryNamedPropertiesRequest);
                                break;
                            case RopIdType.RopCopyProperties:
                                var ropCopyPropertiesRequest = new RopCopyPropertiesRequest();
                                ropCopyPropertiesRequest.Parse(s);
                                ropsList.Add(ropCopyPropertiesRequest);
                                break;

                            case RopIdType.RopCopyTo:
                                var ropCopyToRequest = new RopCopyToRequest();
                                ropCopyToRequest.Parse(s);
                                ropsList.Add(ropCopyToRequest);
                                break;

                            case RopIdType.RopGetPropertyIdsFromNames:
                                var ropGetPropertyIdsFromNamesRequest = new RopGetPropertyIdsFromNamesRequest();
                                ropGetPropertyIdsFromNamesRequest.Parse(s);
                                ropsList.Add(ropGetPropertyIdsFromNamesRequest);
                                break;

                            case RopIdType.RopGetNamesFromPropertyIds:
                                var ropGetNamesFromPropertyIdsRequest = new RopGetNamesFromPropertyIdsRequest();
                                ropGetNamesFromPropertyIdsRequest.Parse(s);
                                ropsList.Add(ropGetNamesFromPropertyIdsRequest);
                                break;

                            case RopIdType.RopOpenStream:
                                var ropOpenStreamRequest = new RopOpenStreamRequest();
                                ropOpenStreamRequest.Parse(s);
                                ropsList.Add(ropOpenStreamRequest);
                                break;

                            case RopIdType.RopReadStream:
                                var ropReadStreamRequest = new RopReadStreamRequest();
                                ropReadStreamRequest.Parse(s);
                                ropsList.Add(ropReadStreamRequest);
                                break;

                            case RopIdType.RopWriteStream:
                                var ropWriteStreamRequest = new RopWriteStreamRequest();
                                ropWriteStreamRequest.Parse(s);
                                ropsList.Add(ropWriteStreamRequest);
                                break;

                            case RopIdType.RopWriteStreamExtended:
                                var ropWriteStreamExtendedRequest = new RopWriteStreamExtendedRequest();
                                ropWriteStreamExtendedRequest.Parse(s);
                                ropsList.Add(ropWriteStreamExtendedRequest);
                                break;

                            case RopIdType.RopCommitStream:
                                var ropCommitStreamRequest = new RopCommitStreamRequest();
                                ropCommitStreamRequest.Parse(s);
                                ropsList.Add(ropCommitStreamRequest);
                                break;

                            case RopIdType.RopGetStreamSize:
                                var ropGetStreamSizeRequest = new RopGetStreamSizeRequest();
                                ropGetStreamSizeRequest.Parse(s);
                                ropsList.Add(ropGetStreamSizeRequest);
                                break;

                            case RopIdType.RopSetStreamSize:
                                var ropSetStreamSizeRequest = new RopSetStreamSizeRequest();
                                ropSetStreamSizeRequest.Parse(s);
                                ropsList.Add(ropSetStreamSizeRequest);
                                break;

                            case RopIdType.RopSeekStream:
                                var ropSeekStreamRequest = new RopSeekStreamRequest();
                                ropSeekStreamRequest.Parse(s);
                                ropsList.Add(ropSeekStreamRequest);
                                break;
                            case RopIdType.RopCopyToStream:
                                var ropCopyToStreamRequest = new RopCopyToStreamRequest();
                                ropCopyToStreamRequest.Parse(s);
                                ropsList.Add(ropCopyToStreamRequest);
                                break;

                            case RopIdType.RopProgress:
                                var ropProgressRequest = new RopProgressRequest();
                                ropProgressRequest.Parse(s);
                                ropsList.Add(ropProgressRequest);
                                break;

                            case RopIdType.RopLockRegionStream:
                                var ropLockRegionStreamRequest = new RopLockRegionStreamRequest();
                                ropLockRegionStreamRequest.Parse(s);
                                ropsList.Add(ropLockRegionStreamRequest);
                                break;

                            case RopIdType.RopUnlockRegionStream:
                                var ropUnlockRegionStreamRequest = new RopUnlockRegionStreamRequest();
                                ropUnlockRegionStreamRequest.Parse(s);
                                ropsList.Add(ropUnlockRegionStreamRequest);
                                break;

                            case RopIdType.RopWriteAndCommitStream:
                                var ropWriteAndCommitStreamRequest = new RopWriteAndCommitStreamRequest();
                                ropWriteAndCommitStreamRequest.Parse(s);
                                ropsList.Add(ropWriteAndCommitStreamRequest);
                                break;

                            case RopIdType.RopCloneStream:
                                var ropCloneStreamRequest = new RopCloneStreamRequest();
                                ropCloneStreamRequest.Parse(s);
                                ropsList.Add(ropCloneStreamRequest);
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                var ropOpenFolderRequest = new RopOpenFolderRequest();
                                ropOpenFolderRequest.Parse(s);
                                ropsList.Add(ropOpenFolderRequest);
                                break;

                            case RopIdType.RopCreateFolder:
                                var ropCreateFolderRequest = new RopCreateFolderRequest();
                                ropCreateFolderRequest.Parse(s);
                                ropsList.Add(ropCreateFolderRequest);
                                break;

                            case RopIdType.RopDeleteFolder:
                                var ropDeleteFolderRequest = new RopDeleteFolderRequest();
                                ropDeleteFolderRequest.Parse(s);
                                ropsList.Add(ropDeleteFolderRequest);
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                var ropSetSearchCriteriaRequest = new RopSetSearchCriteriaRequest();
                                ropSetSearchCriteriaRequest.Parse(s);
                                ropsList.Add(ropSetSearchCriteriaRequest);
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                var ropGetSearchCriteriaRequest = new RopGetSearchCriteriaRequest();
                                ropGetSearchCriteriaRequest.Parse(s);
                                ropsList.Add(ropGetSearchCriteriaRequest);
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                var ropMoveCopyMessagesRequest = new RopMoveCopyMessagesRequest();
                                ropMoveCopyMessagesRequest.Parse(s);
                                ropsList.Add(ropMoveCopyMessagesRequest);
                                break;

                            case RopIdType.RopMoveFolder:
                                var ropMoveFolderRequest = new RopMoveFolderRequest();
                                ropMoveFolderRequest.Parse(s);
                                ropsList.Add(ropMoveFolderRequest);
                                break;

                            case RopIdType.RopCopyFolder:
                                var ropCopyFolderRequest = new RopCopyFolderRequest();
                                ropCopyFolderRequest.Parse(s);
                                ropsList.Add(ropCopyFolderRequest);
                                break;

                            case RopIdType.RopEmptyFolder:
                                var ropEmptyFolderRequest = new RopEmptyFolderRequest();
                                ropEmptyFolderRequest.Parse(s);
                                ropsList.Add(ropEmptyFolderRequest);
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                var ropHardDeleteMessagesAndSubfoldersRequest = new RopHardDeleteMessagesAndSubfoldersRequest();
                                ropHardDeleteMessagesAndSubfoldersRequest.Parse(s);
                                ropsList.Add(ropHardDeleteMessagesAndSubfoldersRequest);
                                break;

                            case RopIdType.RopDeleteMessages:
                                var ropDeleteMessagesRequest = new RopDeleteMessagesRequest();
                                ropDeleteMessagesRequest.Parse(s);
                                ropsList.Add(ropDeleteMessagesRequest);
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                var ropHardDeleteMessagesRequest = new RopHardDeleteMessagesRequest();
                                ropHardDeleteMessagesRequest.Parse(s);
                                ropsList.Add(ropHardDeleteMessagesRequest);
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                var ropGetHierarchyTableRequest = new RopGetHierarchyTableRequest();
                                ropGetHierarchyTableRequest.Parse(s);
                                ropsList.Add(ropGetHierarchyTableRequest);
                                break;

                            case RopIdType.RopGetContentsTable:
                                var ropGetContentsTableRequest = new RopGetContentsTableRequest();
                                ropGetContentsTableRequest.Parse(s);
                                ropsList.Add(ropGetContentsTableRequest);
                                break;

                            // MS-OXCMSG ROPs
                            case RopIdType.RopOpenMessage:
                                var ropOpenMessageRequest = new RopOpenMessageRequest();
                                ropOpenMessageRequest.Parse(s);
                                ropsList.Add(ropOpenMessageRequest);
                                break;

                            case RopIdType.RopCreateMessage:
                                var ropCreateMessageRequest = new RopCreateMessageRequest();
                                ropCreateMessageRequest.Parse(s);
                                ropsList.Add(ropCreateMessageRequest);
                                break;

                            case RopIdType.RopSaveChangesMessage:
                                var ropSaveChangesMessageRequest = new RopSaveChangesMessageRequest();
                                ropSaveChangesMessageRequest.Parse(s);
                                ropsList.Add(ropSaveChangesMessageRequest);
                                break;

                            case RopIdType.RopRemoveAllRecipients:
                                var ropRemoveAllRecipientsRequest = new RopRemoveAllRecipientsRequest();
                                ropRemoveAllRecipientsRequest.Parse(s);
                                ropsList.Add(ropRemoveAllRecipientsRequest);
                                break;

                            case RopIdType.RopModifyRecipients:
                                var ropModifyRecipientsRequest = new RopModifyRecipientsRequest();
                                ropModifyRecipientsRequest.Parse(s);
                                ropsList.Add(ropModifyRecipientsRequest);
                                break;

                            case RopIdType.RopReadRecipients:
                                var ropReadRecipientsRequest = new RopReadRecipientsRequest();
                                ropReadRecipientsRequest.Parse(s);
                                ropsList.Add(ropReadRecipientsRequest);
                                break;

                            case RopIdType.RopReloadCachedInformation:
                                var ropReloadCachedInformationRequest = new RopReloadCachedInformationRequest();
                                ropReloadCachedInformationRequest.Parse(s);
                                ropsList.Add(ropReloadCachedInformationRequest);
                                break;

                            case RopIdType.RopSetMessageStatus:
                                var ropSetMessageStatusRequest = new RopSetMessageStatusRequest();
                                ropSetMessageStatusRequest.Parse(s);
                                ropsList.Add(ropSetMessageStatusRequest);
                                break;

                            case RopIdType.RopGetMessageStatus:
                                var ropGetMessageStatusRequest = new RopGetMessageStatusRequest();
                                ropGetMessageStatusRequest.Parse(s);
                                ropsList.Add(ropGetMessageStatusRequest);
                                break;

                            case RopIdType.RopSetReadFlags:
                                var ropSetReadFlagsRequest = new RopSetReadFlagsRequest();
                                ropSetReadFlagsRequest.Parse(s);
                                ropsList.Add(ropSetReadFlagsRequest);
                                break;

                            case RopIdType.RopSetMessageReadFlag:
                                byte ropId_setReadFlag = ReadByte();
                                byte logId = ReadByte();
                                s.Position -= 2;
                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 &&
                                    DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID) &&
                                    DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logId)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopSetMessageReadFlag",
                                        currentByte,
                                        new uint[] {
                                            logId }
                                        );
                                }

                                var ropSetMessageReadFlagRequest = new RopSetMessageReadFlagRequest();
                                ropSetMessageReadFlagRequest.Parse(s);
                                ropsList.Add(ropSetMessageReadFlagRequest);
                                break;

                            case RopIdType.RopOpenAttachment:
                                var ropOpenAttachmentRequest = new RopOpenAttachmentRequest();
                                ropOpenAttachmentRequest.Parse(s);
                                ropsList.Add(ropOpenAttachmentRequest);
                                break;

                            case RopIdType.RopCreateAttachment:
                                var ropCreateAttachmentRequest = new RopCreateAttachmentRequest();
                                ropCreateAttachmentRequest.Parse(s);
                                ropsList.Add(ropCreateAttachmentRequest);
                                break;

                            case RopIdType.RopDeleteAttachment:
                                var ropDeleteAttachmentRequest = new RopDeleteAttachmentRequest();
                                ropDeleteAttachmentRequest.Parse(s);
                                ropsList.Add(ropDeleteAttachmentRequest);
                                break;

                            case RopIdType.RopSaveChangesAttachment:
                                var ropSaveChangesAttachmentRequest = new RopSaveChangesAttachmentRequest();
                                ropSaveChangesAttachmentRequest.Parse(s);
                                ropsList.Add(ropSaveChangesAttachmentRequest);
                                break;

                            case RopIdType.RopOpenEmbeddedMessage:
                                var ropOpenEmbeddedMessageRequest = new RopOpenEmbeddedMessageRequest();
                                ropOpenEmbeddedMessageRequest.Parse(s);
                                ropsList.Add(ropOpenEmbeddedMessageRequest);
                                break;

                            case RopIdType.RopGetAttachmentTable:
                                var ropGetAttachmentTableRequest = new RopGetAttachmentTableRequest();
                                ropGetAttachmentTableRequest.Parse(s);
                                ropsList.Add(ropGetAttachmentTableRequest);
                                break;

                            case RopIdType.RopGetValidAttachments:
                                var ropGetValidAttachmentsRequest = new RopGetValidAttachmentsRequest();
                                ropGetValidAttachmentsRequest.Parse(s);
                                ropsList.Add(ropGetValidAttachmentsRequest);
                                break;

                            // MSOXCNOTIF ROPs
                            case RopIdType.RopRegisterNotification:
                                var ropRegisterNotificationRequest = new RopRegisterNotificationRequest();
                                ropRegisterNotificationRequest.Parse(s);
                                ropsList.Add(ropRegisterNotificationRequest);
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                var ropGetPermissionsTableRequest = new RopGetPermissionsTableRequest();
                                ropGetPermissionsTableRequest.Parse(s);
                                ropsList.Add(ropGetPermissionsTableRequest);
                                break;

                            case RopIdType.RopModifyPermissions:
                                var ropModifyPermissionsRequest = new RopModifyPermissionsRequest();
                                ropModifyPermissionsRequest.Parse(s);
                                ropsList.Add(ropModifyPermissionsRequest);
                                break;

                            default:
                                object ropsBytes = ReadBytes(RopSize - (ushort)s.Position);
                                ropsList.Add(ropsBytes);
                                break;
                        }

                        if (currentByte != RopIdType.RopRelease)
                        {
                            ropRemainSize.Add(RopSize - (uint)s.Position);
                        }
                        else
                        {
                            ropRemainSize.RemoveAt(ropRemainSize.Count - 1);
                            ropRemainSize.Add(RopSize - (uint)s.Position);
                        }
                    }
                    while (s.Position < RopSize);
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
                var ropListBytes = ReadBytes(RopSize - 2);
                ropsList.AddRange(ropListBytes.Cast<object>().ToArray());
            }

            RopsList = ropsList.ToArray();

            if (RopsList.Length != 0)
            {
                object[] roplist = RopsList;
                foreach (object obj in roplist)
                {
                    if (MapiInspector.MAPIParser.AllRopsList.Count <= 0 ||
                        !MapiInspector.MAPIParser.AllRopsList.Contains(obj.GetType().Name))
                    {
                        MapiInspector.MAPIParser.AllRopsList.Add(obj.GetType().Name);
                    }
                }
            }

            while (s.Position < s.Length)
            {
                uint serverObjectHandle = ReadUint();
                serverObjectHandleTable.Add(serverObjectHandle);
            }

            ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
}
