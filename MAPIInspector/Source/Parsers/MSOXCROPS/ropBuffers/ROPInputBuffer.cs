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

            if (!MapiInspector.MAPIParser.IsLooperCall || parseToCROPSRequestLayer || MapiInspector.MAPIParser.NeedToParseCROPSLayer)
            {
                Queue<PropertyTag[]> proDics = new Queue<PropertyTag[]>();
                Dictionary<uint, Queue<PropertyTag[]>> propertyTagsForGetPropertiesSpec = new Dictionary<uint, Queue<PropertyTag[]>>();
                Dictionary<uint, LogonFlags> logonFlagsInLogonRop = new Dictionary<uint, LogonFlags>();

                if (RopSize > 2)
                {
                    ropRemainSize.Add(RopSize - (uint)2);

                    do
                    {
                        RopIdType currentByte = (RopIdType)s.ReadByte();
                        s.Position -= 1;

                        switch (currentByte)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                RopLogonRequest ropLogonRequest = new RopLogonRequest();
                                ropLogonRequest.Parse(s);
                                ropsList.Add(ropLogonRequest);

                                // update variables used for parsing RopLogon response
                                if (logonFlagsInLogonRop.Count > 0 && logonFlagsInLogonRop.ContainsKey(ropLogonRequest.OutputHandleIndex))
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

                                Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>> processNameMap = new Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>();
                                Dictionary<string, Dictionary<byte, LogonFlags>> clientInfoMap = new Dictionary<string, Dictionary<byte, LogonFlags>>();
                                Dictionary<byte, LogonFlags> logIdAndFlags = new Dictionary<byte, LogonFlags>();

                                // update variables used for parsing messages in other ROPs which need logonFlags
                                if (DecodingContext.LogonFlagMapLogId.Count > 0 && DecodingContext.LogonFlagMapLogId.ContainsKey(MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath))
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
                                RopGetReceiveFolderRequest ropGetReceiveFolderRequest = new RopGetReceiveFolderRequest();
                                ropGetReceiveFolderRequest.Parse(s);
                                ropsList.Add(ropGetReceiveFolderRequest);
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                RopSetReceiveFolderRequest ropSetReceiveFolderRequest = new RopSetReceiveFolderRequest();
                                ropSetReceiveFolderRequest.Parse(s);
                                ropsList.Add(ropSetReceiveFolderRequest);
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                RopGetReceiveFolderTableRequest ropGetReceiveFolderTableRequest = new RopGetReceiveFolderTableRequest();
                                ropGetReceiveFolderTableRequest.Parse(s);
                                ropsList.Add(ropGetReceiveFolderTableRequest);
                                break;
                            case RopIdType.RopGetStoreState:
                                RopGetStoreStateRequest ropGetStoreStateRequest = new RopGetStoreStateRequest();
                                ropGetStoreStateRequest.Parse(s);
                                ropsList.Add(ropGetStoreStateRequest);
                                break;
                            case RopIdType.RopGetOwningServers:
                                RopGetOwningServersRequest ropGetOwningServersRequest = new RopGetOwningServersRequest();
                                ropGetOwningServersRequest.Parse(s);
                                ropsList.Add(ropGetOwningServersRequest);
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                RopPublicFolderIsGhostedRequest ropPublicFolderIsGhostedRequest = new RopPublicFolderIsGhostedRequest();
                                ropPublicFolderIsGhostedRequest.Parse(s);
                                ropsList.Add(ropPublicFolderIsGhostedRequest);
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                RopLongTermIdFromIdRequest ropLongTermIdFromIdRequest = new RopLongTermIdFromIdRequest();
                                ropLongTermIdFromIdRequest.Parse(s);
                                ropsList.Add(ropLongTermIdFromIdRequest);
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                RopIdFromLongTermIdRequest ropIdFromLongTermIdRequest = new RopIdFromLongTermIdRequest();
                                ropIdFromLongTermIdRequest.Parse(s);
                                ropsList.Add(ropIdFromLongTermIdRequest);
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                RopGetPerUserLongTermIdsRequest ropGetPerUserLongTermIdsRequest = new RopGetPerUserLongTermIdsRequest();
                                ropGetPerUserLongTermIdsRequest.Parse(s);
                                ropsList.Add(ropGetPerUserLongTermIdsRequest);
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                RopGetPerUserGuidRequest ropGetPerUserGuidRequest = new RopGetPerUserGuidRequest();
                                ropGetPerUserGuidRequest.Parse(s);
                                ropsList.Add(ropGetPerUserGuidRequest);
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                RopReadPerUserInformationRequest ropReadPerUserInformationRequest = new RopReadPerUserInformationRequest();
                                ropReadPerUserInformationRequest.Parse(s);
                                ropsList.Add(ropReadPerUserInformationRequest);
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                byte ropId = ReadByte();
                                byte logonId = ReadByte();
                                s.Position -= 2;

                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID)
                                      && DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logonId)))
                                {
                                    throw new MissingInformationException(
                                        "Missing LogonFlags information for RopWritePerUserInformation",
                                        currentByte,
                                        new uint[] {
                                            logonId
                                        });
                                }

                                RopWritePerUserInformationRequest ropWritePerUserInformationRequest = new RopWritePerUserInformationRequest();
                                ropWritePerUserInformationRequest.Parse(s);
                                ropsList.Add(ropWritePerUserInformationRequest);
                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                RopSubmitMessageRequest ropSubmitMessageRequest = new RopSubmitMessageRequest();
                                ropSubmitMessageRequest.Parse(s);
                                ropsList.Add(ropSubmitMessageRequest);
                                break;
                            case RopIdType.RopAbortSubmit:
                                RopAbortSubmitRequest ropAbortSubmitRequest = new RopAbortSubmitRequest();
                                ropAbortSubmitRequest.Parse(s);
                                ropsList.Add(ropAbortSubmitRequest);
                                break;
                            case RopIdType.RopGetAddressTypes:
                                RopGetAddressTypesRequest ropGetAddressTypesRequest = new RopGetAddressTypesRequest();
                                ropGetAddressTypesRequest.Parse(s);
                                ropsList.Add(ropGetAddressTypesRequest);
                                break;
                            case RopIdType.RopSetSpooler:
                                RopSetSpoolerRequest ropSetSpoolerRequest = new RopSetSpoolerRequest();
                                ropSetSpoolerRequest.Parse(s);
                                ropsList.Add(ropSetSpoolerRequest);
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                RopSpoolerLockMessageRequest ropSpoolerLockMessageRequest = new RopSpoolerLockMessageRequest();
                                ropSpoolerLockMessageRequest.Parse(s);
                                ropsList.Add(ropSpoolerLockMessageRequest);
                                break;
                            case RopIdType.RopTransportSend:
                                RopTransportSendRequest ropTransportSendRequest = new RopTransportSendRequest();
                                ropTransportSendRequest.Parse(s);
                                ropsList.Add(ropTransportSendRequest);
                                break;
                            case RopIdType.RopTransportNewMail:
                                RopTransportNewMailRequest ropTransportNewMailRequest = new RopTransportNewMailRequest();
                                ropTransportNewMailRequest.Parse(s);
                                ropsList.Add(ropTransportNewMailRequest);
                                break;
                            case RopIdType.RopGetTransportFolder:
                                RopGetTransportFolderRequest ropGetTransportFolderRequest = new RopGetTransportFolderRequest();
                                ropGetTransportFolderRequest.Parse(s);
                                ropsList.Add(ropGetTransportFolderRequest);
                                break;
                            case RopIdType.RopOptionsData:
                                RopOptionsDataRequest ropOptionsDataRequest = new RopOptionsDataRequest();
                                ropOptionsDataRequest.Parse(s);
                                ropsList.Add(ropOptionsDataRequest);
                                break;
                            case RopIdType.RopRelease:
                                RopReleaseRequest ropReleaseRequest = Block.Parse<RopReleaseRequest>(s);
                                ropsList.Add(ropReleaseRequest);
                                uint handle_Release = tempServerObjectHandleTable[ropReleaseRequest.InputHandleIndex];
                                string serverRequestPath = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;

                                if (DecodingContext.RowRops_handlePropertyTags.ContainsKey(handle_Release))
                                {
                                    List<int> sessions = new List<int>();

                                    foreach (var ele in DecodingContext.RowRops_handlePropertyTags[handle_Release])
                                    {
                                        if (ele.Value.Item1 == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && ele.Value.Item2 == MapiInspector.MAPIParser.ParsingSession.LocalProcess && ele.Value.Item3 == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
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
                                RopSetColumnsRequest ropSetColumnsRequest = Block.Parse<RopSetColumnsRequest>(s);
                                ropsList.Add(ropSetColumnsRequest);
                                uint handle_SetColumns = tempServerObjectHandleTable[ropSetColumnsRequest.InputHandleIndex];
                                string serverUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;

                                if (handle_SetColumns != 0xFFFFFFFF)
                                {
                                    if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                                    {
                                        var target = MapiInspector.MAPIParser.TargetHandle.Peek();

                                        if (target.First().Key == RopIdType.RopQueryRows || target.First().Key == RopIdType.RopFindRow || target.First().Key == RopIdType.RopExpandRow)
                                        {
                                            // When the object handle is not equal to 0xFFFFFFFF, add objectHandle and Property Tags to the dictionary.
                                            Dictionary<int, Tuple<string, string, string, PropertyTag[]>> sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[]>>();
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
                                            Dictionary<int, Tuple<string, string, string, PropertyTag[], string>> sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>();
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

                                            if (target.First().Key == RopIdType.RopQueryRows || target.First().Key == RopIdType.RopFindRow || target.First().Key == RopIdType.RopExpandRow)
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

                                            if ((RopIdType)target.First().Key == RopIdType.RopNotify)
                                            {
                                                // This is for ROPNotify
                                                Dictionary<int, Tuple<string, string, string, PropertyTag[], string>> sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>();
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

                                        if (target.First().Key == RopIdType.RopQueryRows || target.First().Key == RopIdType.RopFindRow || target.First().Key == RopIdType.RopExpandRow)
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

                                        if ((RopIdType)target.First().Key == RopIdType.RopNotify)
                                        {
                                            // This is for ROPNotify
                                            Dictionary<int, Tuple<string, string, string, PropertyTag[], string>> sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>();
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
                                RopSortTableRequest ropSortTableRequest = new RopSortTableRequest();
                                ropSortTableRequest.Parse(s);
                                ropsList.Add(ropSortTableRequest);
                                break;

                            case RopIdType.RopRestrict:
                                RopRestrictRequest ropRestrictRequest = new RopRestrictRequest();
                                ropRestrictRequest.Parse(s);
                                ropsList.Add(ropRestrictRequest);
                                break;

                            case RopIdType.RopQueryRows:
                                ropsList.Add(Block.Parse<RopQueryRowsRequest>(s));
                                break;

                            case RopIdType.RopAbort:
                                RopAbortRequest ropAbortRequest = new RopAbortRequest();
                                ropAbortRequest.Parse(s);
                                ropsList.Add(ropAbortRequest);
                                break;

                            case RopIdType.RopGetStatus:
                                RopGetStatusRequest ropGetStatusRequest = new RopGetStatusRequest();
                                ropGetStatusRequest.Parse(s);
                                ropsList.Add(ropGetStatusRequest);
                                break;

                            case RopIdType.RopQueryPosition:
                                RopQueryPositionRequest ropQueryPositionRequest = new RopQueryPositionRequest();
                                ropQueryPositionRequest.Parse(s);
                                ropsList.Add(ropQueryPositionRequest);
                                break;

                            case RopIdType.RopSeekRow:
                                ropsList.Add(Block.Parse<RopSeekRowRequest>(s));
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                RopSeekRowBookmarkRequest ropSeekRowBookmarkRequest = new RopSeekRowBookmarkRequest();
                                ropSeekRowBookmarkRequest.Parse(s);
                                ropsList.Add(ropSeekRowBookmarkRequest);
                                break;

                            case RopIdType.RopSeekRowFractional:
                                RopSeekRowFractionalRequest ropSeekRowFractionalRequest = new RopSeekRowFractionalRequest();
                                ropSeekRowFractionalRequest.Parse(s);
                                ropsList.Add(ropSeekRowFractionalRequest);
                                break;

                            case RopIdType.RopCreateBookmark:
                                RopCreateBookmarkRequest ropCreateBookmarkRequest = new RopCreateBookmarkRequest();
                                ropCreateBookmarkRequest.Parse(s);
                                ropsList.Add(ropCreateBookmarkRequest);
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                RopQueryColumnsAllRequest ropQueryColumnsAllRequest = new RopQueryColumnsAllRequest();
                                ropQueryColumnsAllRequest.Parse(s);
                                ropsList.Add(ropQueryColumnsAllRequest);
                                break;

                            case RopIdType.RopFindRow:
                                RopFindRowRequest ropFindRowRequest = new RopFindRowRequest();
                                ropFindRowRequest.Parse(s);
                                ropsList.Add(ropFindRowRequest);
                                break;

                            case RopIdType.RopFreeBookmark:
                                RopFreeBookmarkRequest ropFreeBookmarkRequest = new RopFreeBookmarkRequest();
                                ropFreeBookmarkRequest.Parse(s);
                                ropsList.Add(ropFreeBookmarkRequest);
                                break;

                            case RopIdType.RopResetTable:
                                RopResetTableRequest ropResetTableRequest = new RopResetTableRequest();
                                ropResetTableRequest.Parse(s);
                                ropsList.Add(ropResetTableRequest);
                                break;

                            case RopIdType.RopExpandRow:
                                RopExpandRowRequest ropExpandRowRequest = new RopExpandRowRequest();
                                ropExpandRowRequest.Parse(s);
                                ropsList.Add(ropExpandRowRequest);
                                break;

                            case RopIdType.RopCollapseRow:
                                RopCollapseRowRequest ropCollapseRowRequest = new RopCollapseRowRequest();
                                ropCollapseRowRequest.Parse(s);
                                ropsList.Add(ropCollapseRowRequest);
                                break;

                            case RopIdType.RopGetCollapseState:
                                RopGetCollapseStateRequest ropGetCollapseStateRequest = new RopGetCollapseStateRequest();
                                ropGetCollapseStateRequest.Parse(s);
                                ropsList.Add(ropGetCollapseStateRequest);
                                break;

                            case RopIdType.RopSetCollapseState:
                                RopSetCollapseStateRequest ropSetCollapseStateRequest = new RopSetCollapseStateRequest();
                                ropSetCollapseStateRequest.Parse(s);
                                ropsList.Add(ropSetCollapseStateRequest);
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                RopModifyRulesRequest ropModifyRulesRequest = new RopModifyRulesRequest();
                                ropModifyRulesRequest.Parse(s);
                                ropsList.Add(ropModifyRulesRequest);
                                break;

                            case RopIdType.RopGetRulesTable:
                                RopGetRulesTableRequest ropGetRulesTableRequest = new RopGetRulesTableRequest();
                                ropGetRulesTableRequest.Parse(s);
                                ropsList.Add(ropGetRulesTableRequest);
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                RopUpdateDeferredActionMessagesRequest ropUpdateDeferredActionMessagesRequest = new RopUpdateDeferredActionMessagesRequest();
                                ropUpdateDeferredActionMessagesRequest.Parse(s);
                                ropsList.Add(ropUpdateDeferredActionMessagesRequest);
                                break;

                            // MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                RopFastTransferSourceCopyPropertiesRequest ropFastTransferSourceCopyPropertiesRequest = new RopFastTransferSourceCopyPropertiesRequest();
                                ropFastTransferSourceCopyPropertiesRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyPropertiesRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                RopFastTransferSourceCopyToRequest ropFastTransferSourceCopyToRequest = new RopFastTransferSourceCopyToRequest();
                                ropFastTransferSourceCopyToRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyToRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                RopFastTransferSourceCopyMessagesRequest ropFastTransferSourceCopyMessagesRequest = new RopFastTransferSourceCopyMessagesRequest();
                                ropFastTransferSourceCopyMessagesRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyMessagesRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                RopFastTransferSourceCopyFolderRequest ropFastTransferSourceCopyFolderRequest = new RopFastTransferSourceCopyFolderRequest();
                                ropFastTransferSourceCopyFolderRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyFolderRequest);
                                break;
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                RopFastTransferSourceGetBufferRequest ropFastTransferSourceGetBufferRequest = new RopFastTransferSourceGetBufferRequest();
                                ropFastTransferSourceGetBufferRequest.Parse(s);
                                ropsList.Add(ropFastTransferSourceGetBufferRequest);
                                break;
                            case RopIdType.RopTellVersion:
                                RopTellVersionRequest ropTellVersionRequest = new RopTellVersionRequest();
                                ropTellVersionRequest.Parse(s);
                                ropsList.Add(ropTellVersionRequest);
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                RopFastTransferDestinationConfigureRequest ropFastTransferDestinationConfigureRequest = new RopFastTransferDestinationConfigureRequest();
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

                                RopFastTransferDestinationPutBufferRequest ropFastTransferDestinationPutBufferRequest = new RopFastTransferDestinationPutBufferRequest();
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
                                SortedDictionary<int, PartialContextInformation> sessionputContextInfor = new SortedDictionary<int, PartialContextInformation>();

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

                                RopFastTransferDestinationPutBufferExtendedRequest ropFastTransferDestinationPutBufferExtendedRequest = new RopFastTransferDestinationPutBufferExtendedRequest();
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
                                SortedDictionary<int, PartialContextInformation> sessionputExtendContextInfor = new SortedDictionary<int, PartialContextInformation>();

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
                                RopSynchronizationConfigureRequest ropSynchronizationConfigureRequest = new RopSynchronizationConfigureRequest();
                                ropSynchronizationConfigureRequest.Parse(s);
                                ropsList.Add(ropSynchronizationConfigureRequest);
                                break;

                            case RopIdType.RopSynchronizationGetTransferState:
                                RopSynchronizationGetTransferStateRequest ropSynchronizationGetTransferStateRequest = new RopSynchronizationGetTransferStateRequest();
                                ropSynchronizationGetTransferStateRequest.Parse(s);
                                ropsList.Add(ropSynchronizationGetTransferStateRequest);
                                break;

                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                RopSynchronizationUploadStateStreamBeginRequest ropSynchronizationUploadStateStreamBeginRequest = new RopSynchronizationUploadStateStreamBeginRequest();
                                ropSynchronizationUploadStateStreamBeginRequest.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamBeginRequest);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                RopSynchronizationUploadStateStreamContinueRequest ropSynchronizationUploadStateStreamContinueRequest = new RopSynchronizationUploadStateStreamContinueRequest();
                                ropSynchronizationUploadStateStreamContinueRequest.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamContinueRequest);
                                break;

                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                RopSynchronizationUploadStateStreamEndRequest ropSynchronizationUploadStateStreamEndRequest = new RopSynchronizationUploadStateStreamEndRequest();
                                ropSynchronizationUploadStateStreamEndRequest.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamEndRequest);
                                break;

                            case RopIdType.RopSynchronizationOpenCollector:
                                RopSynchronizationOpenCollectorRequest ropSynchronizationOpenCollectorRequest = new RopSynchronizationOpenCollectorRequest();
                                ropSynchronizationOpenCollectorRequest.Parse(s);
                                ropsList.Add(ropSynchronizationOpenCollectorRequest);
                                break;

                            case RopIdType.RopSynchronizationImportMessageChange:
                                RopSynchronizationImportMessageChangeRequest ropSynchronizationImportMessageChangeRequest = new RopSynchronizationImportMessageChangeRequest();
                                ropSynchronizationImportMessageChangeRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportMessageChangeRequest);
                                break;

                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                RopSynchronizationImportHierarchyChangeRequest ropSynchronizationImportHierarchyChangeRequest = new RopSynchronizationImportHierarchyChangeRequest();
                                ropSynchronizationImportHierarchyChangeRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportHierarchyChangeRequest);
                                break;

                            case RopIdType.RopSynchronizationImportMessageMove:
                                RopSynchronizationImportMessageMoveRequest ropSynchronizationImportMessageMoveRequest = new RopSynchronizationImportMessageMoveRequest();
                                ropSynchronizationImportMessageMoveRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportMessageMoveRequest);
                                break;

                            case RopIdType.RopSynchronizationImportDeletes:
                                RopSynchronizationImportDeletesRequest ropSynchronizationImportDeletesRequest = new RopSynchronizationImportDeletesRequest();
                                ropSynchronizationImportDeletesRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportDeletesRequest);
                                break;

                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                RopSynchronizationImportReadStateChangesRequest ropSynchronizationImportReadStateChangesRequest = new RopSynchronizationImportReadStateChangesRequest();
                                ropSynchronizationImportReadStateChangesRequest.Parse(s);
                                ropsList.Add(ropSynchronizationImportReadStateChangesRequest);
                                break;

                            case RopIdType.RopGetLocalReplicaIds:
                                RopGetLocalReplicaIdsRequest ropGetLocalReplicaIdsRequest = new RopGetLocalReplicaIdsRequest();
                                ropGetLocalReplicaIdsRequest.Parse(s);
                                ropsList.Add(ropGetLocalReplicaIdsRequest);
                                break;

                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                RopSetLocalReplicaMidsetDeletedRequest ropSetLocalReplicaMidsetDeletedRequest = new RopSetLocalReplicaMidsetDeletedRequest();
                                ropSetLocalReplicaMidsetDeletedRequest.Parse(s);
                                ropsList.Add(ropSetLocalReplicaMidsetDeletedRequest);
                                break;

                            // MS-OXCPRPT ROPs
                            case RopIdType.RopGetPropertiesSpecific:
                                RopGetPropertiesSpecificRequest ropGetPropertiesSpecificRequest = new RopGetPropertiesSpecificRequest();
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
                                    Queue<PropertyTag[]> proDic0 = new Queue<PropertyTag[]>();
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
                                RopGetPropertiesAllRequest ropGetPropertiesAllRequest = new RopGetPropertiesAllRequest();
                                ropGetPropertiesAllRequest.Parse(s);
                                ropsList.Add(ropGetPropertiesAllRequest);
                                break;

                            case RopIdType.RopGetPropertiesList:
                                RopGetPropertiesListRequest ropGetPropertiesListRequest = new RopGetPropertiesListRequest();
                                ropGetPropertiesListRequest.Parse(s);
                                ropsList.Add(ropGetPropertiesListRequest);
                                break;

                            case RopIdType.RopSetProperties:
                                RopSetPropertiesRequest ropSetPropertiesRequest = new RopSetPropertiesRequest();
                                ropSetPropertiesRequest.Parse(s);
                                ropsList.Add(ropSetPropertiesRequest);
                                break;

                            case RopIdType.RopSetPropertiesNoReplicate:
                                RopSetPropertiesNoReplicateRequest ropSetPropertiesNoReplicateRequest = new RopSetPropertiesNoReplicateRequest();
                                ropSetPropertiesNoReplicateRequest.Parse(s);
                                ropsList.Add(ropSetPropertiesNoReplicateRequest);
                                break;

                            case RopIdType.RopDeleteProperties:
                                RopDeletePropertiesRequest ropDeletePropertiesRequest = new RopDeletePropertiesRequest();
                                ropDeletePropertiesRequest.Parse(s);
                                ropsList.Add(ropDeletePropertiesRequest);
                                break;

                            case RopIdType.RopDeletePropertiesNoReplicate:
                                RopDeletePropertiesNoReplicateRequest ropDeletePropertiesNoReplicateRequest = new RopDeletePropertiesNoReplicateRequest();
                                ropDeletePropertiesNoReplicateRequest.Parse(s);
                                ropsList.Add(ropDeletePropertiesNoReplicateRequest);
                                break;

                            case RopIdType.RopQueryNamedProperties:
                                RopQueryNamedPropertiesRequest ropQueryNamedPropertiesRequest = new RopQueryNamedPropertiesRequest();
                                ropQueryNamedPropertiesRequest.Parse(s);
                                ropsList.Add(ropQueryNamedPropertiesRequest);
                                break;
                            case RopIdType.RopCopyProperties:
                                RopCopyPropertiesRequest ropCopyPropertiesRequest = new RopCopyPropertiesRequest();
                                ropCopyPropertiesRequest.Parse(s);
                                ropsList.Add(ropCopyPropertiesRequest);
                                break;

                            case RopIdType.RopCopyTo:
                                RopCopyToRequest ropCopyToRequest = new RopCopyToRequest();
                                ropCopyToRequest.Parse(s);
                                ropsList.Add(ropCopyToRequest);
                                break;

                            case RopIdType.RopGetPropertyIdsFromNames:
                                RopGetPropertyIdsFromNamesRequest ropGetPropertyIdsFromNamesRequest = new RopGetPropertyIdsFromNamesRequest();
                                ropGetPropertyIdsFromNamesRequest.Parse(s);
                                ropsList.Add(ropGetPropertyIdsFromNamesRequest);
                                break;

                            case RopIdType.RopGetNamesFromPropertyIds:
                                RopGetNamesFromPropertyIdsRequest ropGetNamesFromPropertyIdsRequest = new RopGetNamesFromPropertyIdsRequest();
                                ropGetNamesFromPropertyIdsRequest.Parse(s);
                                ropsList.Add(ropGetNamesFromPropertyIdsRequest);
                                break;

                            case RopIdType.RopOpenStream:
                                RopOpenStreamRequest ropOpenStreamRequest = new RopOpenStreamRequest();
                                ropOpenStreamRequest.Parse(s);
                                ropsList.Add(ropOpenStreamRequest);
                                break;

                            case RopIdType.RopReadStream:
                                RopReadStreamRequest ropReadStreamRequest = new RopReadStreamRequest();
                                ropReadStreamRequest.Parse(s);
                                ropsList.Add(ropReadStreamRequest);
                                break;

                            case RopIdType.RopWriteStream:
                                RopWriteStreamRequest ropWriteStreamRequest = new RopWriteStreamRequest();
                                ropWriteStreamRequest.Parse(s);
                                ropsList.Add(ropWriteStreamRequest);
                                break;

                            case RopIdType.RopWriteStreamExtended:
                                RopWriteStreamExtendedRequest ropWriteStreamExtendedRequest = new RopWriteStreamExtendedRequest();
                                ropWriteStreamExtendedRequest.Parse(s);
                                ropsList.Add(ropWriteStreamExtendedRequest);
                                break;

                            case RopIdType.RopCommitStream:
                                RopCommitStreamRequest ropCommitStreamRequest = new RopCommitStreamRequest();
                                ropCommitStreamRequest.Parse(s);
                                ropsList.Add(ropCommitStreamRequest);
                                break;

                            case RopIdType.RopGetStreamSize:
                                RopGetStreamSizeRequest ropGetStreamSizeRequest = new RopGetStreamSizeRequest();
                                ropGetStreamSizeRequest.Parse(s);
                                ropsList.Add(ropGetStreamSizeRequest);
                                break;

                            case RopIdType.RopSetStreamSize:
                                RopSetStreamSizeRequest ropSetStreamSizeRequest = new RopSetStreamSizeRequest();
                                ropSetStreamSizeRequest.Parse(s);
                                ropsList.Add(ropSetStreamSizeRequest);
                                break;

                            case RopIdType.RopSeekStream:
                                RopSeekStreamRequest ropSeekStreamRequest = new RopSeekStreamRequest();
                                ropSeekStreamRequest.Parse(s);
                                ropsList.Add(ropSeekStreamRequest);
                                break;
                            case RopIdType.RopCopyToStream:
                                RopCopyToStreamRequest ropCopyToStreamRequest = new RopCopyToStreamRequest();
                                ropCopyToStreamRequest.Parse(s);
                                ropsList.Add(ropCopyToStreamRequest);
                                break;

                            case RopIdType.RopProgress:
                                RopProgressRequest ropProgressRequest = new RopProgressRequest();
                                ropProgressRequest.Parse(s);
                                ropsList.Add(ropProgressRequest);
                                break;

                            case RopIdType.RopLockRegionStream:
                                RopLockRegionStreamRequest ropLockRegionStreamRequest = new RopLockRegionStreamRequest();
                                ropLockRegionStreamRequest.Parse(s);
                                ropsList.Add(ropLockRegionStreamRequest);
                                break;

                            case RopIdType.RopUnlockRegionStream:
                                RopUnlockRegionStreamRequest ropUnlockRegionStreamRequest = new RopUnlockRegionStreamRequest();
                                ropUnlockRegionStreamRequest.Parse(s);
                                ropsList.Add(ropUnlockRegionStreamRequest);
                                break;

                            case RopIdType.RopWriteAndCommitStream:
                                RopWriteAndCommitStreamRequest ropWriteAndCommitStreamRequest = new RopWriteAndCommitStreamRequest();
                                ropWriteAndCommitStreamRequest.Parse(s);
                                ropsList.Add(ropWriteAndCommitStreamRequest);
                                break;

                            case RopIdType.RopCloneStream:
                                RopCloneStreamRequest ropCloneStreamRequest = new RopCloneStreamRequest();
                                ropCloneStreamRequest.Parse(s);
                                ropsList.Add(ropCloneStreamRequest);
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                RopOpenFolderRequest ropOpenFolderRequest = new RopOpenFolderRequest();
                                ropOpenFolderRequest.Parse(s);
                                ropsList.Add(ropOpenFolderRequest);
                                break;

                            case RopIdType.RopCreateFolder:
                                RopCreateFolderRequest ropCreateFolderRequest = new RopCreateFolderRequest();
                                ropCreateFolderRequest.Parse(s);
                                ropsList.Add(ropCreateFolderRequest);
                                break;

                            case RopIdType.RopDeleteFolder:
                                RopDeleteFolderRequest ropDeleteFolderRequest = new RopDeleteFolderRequest();
                                ropDeleteFolderRequest.Parse(s);
                                ropsList.Add(ropDeleteFolderRequest);
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                RopSetSearchCriteriaRequest ropSetSearchCriteriaRequest = new RopSetSearchCriteriaRequest();
                                ropSetSearchCriteriaRequest.Parse(s);
                                ropsList.Add(ropSetSearchCriteriaRequest);
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                RopGetSearchCriteriaRequest ropGetSearchCriteriaRequest = new RopGetSearchCriteriaRequest();
                                ropGetSearchCriteriaRequest.Parse(s);
                                ropsList.Add(ropGetSearchCriteriaRequest);
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                RopMoveCopyMessagesRequest ropMoveCopyMessagesRequest = new RopMoveCopyMessagesRequest();
                                ropMoveCopyMessagesRequest.Parse(s);
                                ropsList.Add(ropMoveCopyMessagesRequest);
                                break;

                            case RopIdType.RopMoveFolder:
                                RopMoveFolderRequest ropMoveFolderRequest = new RopMoveFolderRequest();
                                ropMoveFolderRequest.Parse(s);
                                ropsList.Add(ropMoveFolderRequest);
                                break;

                            case RopIdType.RopCopyFolder:
                                RopCopyFolderRequest ropCopyFolderRequest = new RopCopyFolderRequest();
                                ropCopyFolderRequest.Parse(s);
                                ropsList.Add(ropCopyFolderRequest);
                                break;

                            case RopIdType.RopEmptyFolder:
                                RopEmptyFolderRequest ropEmptyFolderRequest = new RopEmptyFolderRequest();
                                ropEmptyFolderRequest.Parse(s);
                                ropsList.Add(ropEmptyFolderRequest);
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                RopHardDeleteMessagesAndSubfoldersRequest ropHardDeleteMessagesAndSubfoldersRequest = new RopHardDeleteMessagesAndSubfoldersRequest();
                                ropHardDeleteMessagesAndSubfoldersRequest.Parse(s);
                                ropsList.Add(ropHardDeleteMessagesAndSubfoldersRequest);
                                break;

                            case RopIdType.RopDeleteMessages:
                                RopDeleteMessagesRequest ropDeleteMessagesRequest = new RopDeleteMessagesRequest();
                                ropDeleteMessagesRequest.Parse(s);
                                ropsList.Add(ropDeleteMessagesRequest);
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                RopHardDeleteMessagesRequest ropHardDeleteMessagesRequest = new RopHardDeleteMessagesRequest();
                                ropHardDeleteMessagesRequest.Parse(s);
                                ropsList.Add(ropHardDeleteMessagesRequest);
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                RopGetHierarchyTableRequest ropGetHierarchyTableRequest = new RopGetHierarchyTableRequest();
                                ropGetHierarchyTableRequest.Parse(s);
                                ropsList.Add(ropGetHierarchyTableRequest);
                                break;

                            case RopIdType.RopGetContentsTable:
                                RopGetContentsTableRequest ropGetContentsTableRequest = new RopGetContentsTableRequest();
                                ropGetContentsTableRequest.Parse(s);
                                ropsList.Add(ropGetContentsTableRequest);
                                break;

                            // MS-OXCMSG ROPs
                            case RopIdType.RopOpenMessage:
                                RopOpenMessageRequest ropOpenMessageRequest = new RopOpenMessageRequest();
                                ropOpenMessageRequest.Parse(s);
                                ropsList.Add(ropOpenMessageRequest);
                                break;

                            case RopIdType.RopCreateMessage:
                                RopCreateMessageRequest ropCreateMessageRequest = new RopCreateMessageRequest();
                                ropCreateMessageRequest.Parse(s);
                                ropsList.Add(ropCreateMessageRequest);
                                break;

                            case RopIdType.RopSaveChangesMessage:
                                RopSaveChangesMessageRequest ropSaveChangesMessageRequest = new RopSaveChangesMessageRequest();
                                ropSaveChangesMessageRequest.Parse(s);
                                ropsList.Add(ropSaveChangesMessageRequest);
                                break;

                            case RopIdType.RopRemoveAllRecipients:
                                RopRemoveAllRecipientsRequest ropRemoveAllRecipientsRequest = new RopRemoveAllRecipientsRequest();
                                ropRemoveAllRecipientsRequest.Parse(s);
                                ropsList.Add(ropRemoveAllRecipientsRequest);
                                break;

                            case RopIdType.RopModifyRecipients:
                                RopModifyRecipientsRequest ropModifyRecipientsRequest = new RopModifyRecipientsRequest();
                                ropModifyRecipientsRequest.Parse(s);
                                ropsList.Add(ropModifyRecipientsRequest);
                                break;

                            case RopIdType.RopReadRecipients:
                                RopReadRecipientsRequest ropReadRecipientsRequest = new RopReadRecipientsRequest();
                                ropReadRecipientsRequest.Parse(s);
                                ropsList.Add(ropReadRecipientsRequest);
                                break;

                            case RopIdType.RopReloadCachedInformation:
                                RopReloadCachedInformationRequest ropReloadCachedInformationRequest = new RopReloadCachedInformationRequest();
                                ropReloadCachedInformationRequest.Parse(s);
                                ropsList.Add(ropReloadCachedInformationRequest);
                                break;

                            case RopIdType.RopSetMessageStatus:
                                RopSetMessageStatusRequest ropSetMessageStatusRequest = new RopSetMessageStatusRequest();
                                ropSetMessageStatusRequest.Parse(s);
                                ropsList.Add(ropSetMessageStatusRequest);
                                break;

                            case RopIdType.RopGetMessageStatus:
                                RopGetMessageStatusRequest ropGetMessageStatusRequest = new RopGetMessageStatusRequest();
                                ropGetMessageStatusRequest.Parse(s);
                                ropsList.Add(ropGetMessageStatusRequest);
                                break;

                            case RopIdType.RopSetReadFlags:
                                RopSetReadFlagsRequest ropSetReadFlagsRequest = new RopSetReadFlagsRequest();
                                ropSetReadFlagsRequest.Parse(s);
                                ropsList.Add(ropSetReadFlagsRequest);
                                break;

                            case RopIdType.RopSetMessageReadFlag:
                                byte ropId_setReadFlag = ReadByte();
                                byte logId = ReadByte();
                                s.Position -= 2;
                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID)
                                    && DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logId)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopSetMessageReadFlag",
                                        currentByte,
                                        new uint[] {
                                            logId }
                                        );
                                }

                                RopSetMessageReadFlagRequest ropSetMessageReadFlagRequest = new RopSetMessageReadFlagRequest();
                                ropSetMessageReadFlagRequest.Parse(s);
                                ropsList.Add(ropSetMessageReadFlagRequest);
                                break;

                            case RopIdType.RopOpenAttachment:
                                RopOpenAttachmentRequest ropOpenAttachmentRequest = new RopOpenAttachmentRequest();
                                ropOpenAttachmentRequest.Parse(s);
                                ropsList.Add(ropOpenAttachmentRequest);
                                break;

                            case RopIdType.RopCreateAttachment:
                                RopCreateAttachmentRequest ropCreateAttachmentRequest = new RopCreateAttachmentRequest();
                                ropCreateAttachmentRequest.Parse(s);
                                ropsList.Add(ropCreateAttachmentRequest);
                                break;

                            case RopIdType.RopDeleteAttachment:
                                RopDeleteAttachmentRequest ropDeleteAttachmentRequest = new RopDeleteAttachmentRequest();
                                ropDeleteAttachmentRequest.Parse(s);
                                ropsList.Add(ropDeleteAttachmentRequest);
                                break;

                            case RopIdType.RopSaveChangesAttachment:
                                RopSaveChangesAttachmentRequest ropSaveChangesAttachmentRequest = new RopSaveChangesAttachmentRequest();
                                ropSaveChangesAttachmentRequest.Parse(s);
                                ropsList.Add(ropSaveChangesAttachmentRequest);
                                break;

                            case RopIdType.RopOpenEmbeddedMessage:
                                RopOpenEmbeddedMessageRequest ropOpenEmbeddedMessageRequest = new RopOpenEmbeddedMessageRequest();
                                ropOpenEmbeddedMessageRequest.Parse(s);
                                ropsList.Add(ropOpenEmbeddedMessageRequest);
                                break;

                            case RopIdType.RopGetAttachmentTable:
                                RopGetAttachmentTableRequest ropGetAttachmentTableRequest = new RopGetAttachmentTableRequest();
                                ropGetAttachmentTableRequest.Parse(s);
                                ropsList.Add(ropGetAttachmentTableRequest);
                                break;

                            case RopIdType.RopGetValidAttachments:
                                RopGetValidAttachmentsRequest ropGetValidAttachmentsRequest = new RopGetValidAttachmentsRequest();
                                ropGetValidAttachmentsRequest.Parse(s);
                                ropsList.Add(ropGetValidAttachmentsRequest);
                                break;

                            // MSOXCNOTIF ROPs
                            case RopIdType.RopRegisterNotification:
                                RopRegisterNotificationRequest ropRegisterNotificationRequest = new RopRegisterNotificationRequest();
                                ropRegisterNotificationRequest.Parse(s);
                                ropsList.Add(ropRegisterNotificationRequest);
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                RopGetPermissionsTableRequest ropGetPermissionsTableRequest = new RopGetPermissionsTableRequest();
                                ropGetPermissionsTableRequest.Parse(s);
                                ropsList.Add(ropGetPermissionsTableRequest);
                                break;

                            case RopIdType.RopModifyPermissions:
                                RopModifyPermissionsRequest ropModifyPermissionsRequest = new RopModifyPermissionsRequest();
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
                byte[] ropListBytes = ReadBytes(RopSize - 2);
                ropsList.AddRange(ropListBytes.Cast<object>().ToArray());
            }

            RopsList = ropsList.ToArray();

            if (RopsList.Length != 0)
            {
                object[] roplist = RopsList;
                foreach (object obj in roplist)
                {
                    if (MapiInspector.MAPIParser.AllRopsList.Count <= 0 || !MapiInspector.MAPIParser.AllRopsList.Contains(obj.GetType().Name))
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
