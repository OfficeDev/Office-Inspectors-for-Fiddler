using BlockParser;
using Fiddler;
using System.Collections.Generic;
using System.Linq;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.1 ROP Input and Output Buffers
    /// A class indicates the ROP output buffer, which is sent by the server, includes an array of ROP response buffers.
    /// </summary>
    public class ROPOutputBuffer : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public BlockT<ushort> RopSize;

        /// <summary>
        /// An array of ROP response buffers.
        /// The size of this field is 2 bytes less than the value specified in the RopSize field.
        /// </summary>
        public Block[] RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public BlockT<uint>[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPOutputBuffer structure.
        /// </summary>
        protected override void Parse()
        {
            // Build tempServerObjectHandleTable first
            bool parseToCROPSResponseLayer = false;
            RopSize = ParseT<ushort>();
            var ropsList = new List<Block>();
            var serverObjectHandleTable = new List<BlockT<uint>>();
            var tempServerObjectHandleTable = new List<BlockT<uint>>();
            int currentPosition = parser.Offset;
            parser.Advance(RopSize - sizeof(ushort));
            int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
            }

            while (parser.RemainingBytes >= sizeof(uint))
            {
                var serverObjectTable = ParseT<uint>();

                if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                {
                    MapiInspector.MAPIParser.IsLooperCall = true;
                    var item = MapiInspector.MAPIParser.TargetHandle.Peek();

                    if (item.First().Value.ContainsValue(serverObjectTable))
                    {
                        parseToCROPSResponseLayer = true;
                    }
                }
                else
                {
                    MapiInspector.MAPIParser.IsLooperCall = false;
                }

                tempServerObjectHandleTable.Add(serverObjectTable);
            }

            parser.Offset = currentPosition;

            // Now we do regular parsing
            if (!MapiInspector.MAPIParser.IsLooperCall || parseToCROPSResponseLayer || MapiInspector.MAPIParser.NeedToParseCROPSLayer)
            {
                // empty intermediate variables for ROPs need context information
                DecodingContext.SetColumn_InputHandles_InResponse = new List<uint>();

                // RopSize includes ropSize itself, so we need to subtract sizeof(ushort) to get the actual size of RopsList
                // We need this to be at least sizeof(RopIdType) for the ropID
                if (RopSize - sizeof(ushort) > sizeof(RopIdType))
                {
                    parser.PushCap(RopSize - sizeof(ushort));
                    do
                    {
                        BlockT<RopIdType> currentRop = TestParse<RopIdType>(parser);

                        switch (currentRop.Data)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                int currentPos_logon = parser.Offset;
                                parser.Advance(sizeof(RopIdType));
                                var tempOutputHandleIndex_logon = ParseT<byte>();
                                parser.Offset = currentPos_logon;
                                if (!(DecodingContext.SessionLogonFlagsInLogonRop.Count > 0 &&
                                    DecodingContext.SessionLogonFlagsInLogonRop.ContainsKey(parsingSessionID) &&
                                    DecodingContext.SessionLogonFlagsInLogonRop[parsingSessionID].ContainsKey(tempOutputHandleIndex_logon)))
                                {
                                    ropsList.Add(MissingInformationException.MaybeThrow("Missing LogonFlags information for RopLogon", currentRop));
                                    ropsList.Add(ParseJunk("Remaining Data"));
                                }
                                else
                                {
                                    if (((byte)DecodingContext.SessionLogonFlagsInLogonRop[parsingSessionID][tempOutputHandleIndex_logon] & 0x01) == (byte)LogonFlags.Private)
                                    {
                                        ropsList.Add(Parse<RopLogonResponse_PrivateMailboxes>());
                                    }
                                    else
                                    {
                                        ropsList.Add(Parse<RopLogonResponse_PublicFolders>());
                                    }
                                }

                                break;

                            case RopIdType.RopGetReceiveFolder:
                                ropsList.Add(Parse<RopGetReceiveFolderResponse>());
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                ropsList.Add(Parse<RopSetReceiveFolderResponse>());
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                ropsList.Add(Parse<RopGetReceiveFolderTableResponse>());
                                break;
                            case RopIdType.RopGetStoreState:
                                ropsList.Add(Parse<RopGetStoreStateResponse>());
                                break;
                            case RopIdType.RopGetOwningServers:
                                ropsList.Add(Parse<RopGetOwningServersResponse>());
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                ropsList.Add(Parse<RopPublicFolderIsGhostedResponse>());
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                ropsList.Add(Parse<RopLongTermIdFromIdResponse>());
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                ropsList.Add(Parse<RopIdFromLongTermIdResponse>());
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                ropsList.Add(Parse<RopGetPerUserLongTermIdsResponse>());
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                ropsList.Add(Parse<RopGetPerUserGuidResponse>());
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                ropsList.Add(Parse<RopReadPerUserInformationResponse>());
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                ropsList.Add(Parse<RopWritePerUserInformationResponse>());
                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                ropsList.Add(Parse<RopSubmitMessageResponse>());
                                break;
                            case RopIdType.RopAbortSubmit:
                                ropsList.Add(Parse<RopAbortSubmitResponse>());
                                break;
                            case RopIdType.RopGetAddressTypes:
                                ropsList.Add(Parse<RopGetAddressTypesResponse>());
                                break;
                            case RopIdType.RopSetSpooler:
                                ropsList.Add(Parse<RopSetSpoolerResponse>());
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                ropsList.Add(Parse<RopSpoolerLockMessageResponse>());
                                break;
                            case RopIdType.RopTransportSend:
                                ropsList.Add(Parse<RopTransportSendResponse>());
                                break;
                            case RopIdType.RopTransportNewMail:
                                ropsList.Add(Parse<RopTransportNewMailResponse>());
                                break;
                            case RopIdType.RopGetTransportFolder:
                                ropsList.Add(Parse<RopGetTransportFolderResponse>());
                                break;
                            case RopIdType.RopOptionsData:
                                ropsList.Add(Parse<RopOptionsDataResponse>());
                                break;
                            case RopIdType.RopBackoff:
                                ropsList.Add(Parse<RopBackoffResponse>());
                                break;
                            case RopIdType.RopBufferTooSmall:
                                if (DecodingContext.SessionRequestRemainSize.Count > 0 &&
                                    DecodingContext.SessionRequestRemainSize.ContainsKey(parsingSessionID))
                                {
                                    uint requestBuffersSize = 0;
                                    int ropCountInResponse = ropsList.Count;
                                    if (DecodingContext.SessionRequestRemainSize[parsingSessionID].Count > ropCountInResponse)
                                    {
                                        requestBuffersSize = DecodingContext.SessionRequestRemainSize[parsingSessionID][ropCountInResponse];
                                    }

                                    var ropBufferTooSmallResponse = new RopBufferTooSmallResponse(requestBuffersSize);
                                    ropBufferTooSmallResponse.Parse(parser);
                                    ropsList.Add(ropBufferTooSmallResponse);
                                }
                                else
                                {
                                    ropsList.Add(MissingInformationException.MaybeThrow(
                                        "Missing RequestBuffersSize information for RopBufferTooSmall",
                                        currentRop));
                                    ropsList.Add(ParseJunk("Remaining Data"));
                                }
                                break;

                            // MSOXCTABL ROPs
                            case RopIdType.RopSetColumns:
                                RopSetColumnsResponse ropSetColumnsResponse = Parse<RopSetColumnsResponse>();
                                ropsList.Add(ropSetColumnsResponse);

                                if (!(DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 &&
                                    DecodingContext.SetColumn_InputHandles_InResponse.Contains(tempServerObjectHandleTable[ropSetColumnsResponse.InputHandleIndex])))
                                {
                                    DecodingContext.SetColumn_InputHandles_InResponse.Add(tempServerObjectHandleTable[ropSetColumnsResponse.InputHandleIndex]);
                                }

                                break;

                            case RopIdType.RopSortTable:
                                ropsList.Add(Parse<RopSortTableResponse>());
                                break;

                            case RopIdType.RopRestrict:
                                ropsList.Add(Parse<RopRestrictResponse>());
                                break;

                            case RopIdType.RopQueryRows:
                                int currentPos = parser.Offset;
                                parser.Advance(sizeof(RopIdType));
                                BlockT<byte> tempInputHandleIndex_QueryRow = ParseT<byte>();
                                BlockT<ErrorCodes> returnValue_queryRow = ParseT<ErrorCodes>();
                                parser.Offset = currentPos;
                                string serverPath_QueryRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                                string processName_QueryROw = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                                string clientInfo_QueryROw = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                                uint objHandle_QueryROw = tempServerObjectHandleTable[tempInputHandleIndex_QueryRow];
                                if (returnValue_queryRow == ErrorCodes.Success)
                                {
                                    if (!(DecodingContext.RowRops_handlePropertyTags.ContainsKey(objHandle_QueryROw) &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw].ContainsKey(parsingSessionID) &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item1 == serverPath_QueryRow &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item2 == processName_QueryROw &&
                                     DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item3 == clientInfo_QueryROw))
                                    {
                                        ropsList.Add(MissingInformationException.MaybeThrow(
                                            "Missing PropertyTags information for RopQueryRowsResponse",
                                            RopIdType.RopQueryRows,
                                            new uint[] {
                                                (uint)tempInputHandleIndex_QueryRow,
                                                tempServerObjectHandleTable[tempInputHandleIndex_QueryRow]
                                            }));
                                        ropsList.Add(ParseJunk("Remaining Data"));
                                    }

                                    var ropQueryRowsResponse = new RopQueryRowsResponse(DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item4);
                                    ropQueryRowsResponse.Parse(parser);
                                    ropsList.Add(ropQueryRowsResponse);
                                    break;
                                }
                                else
                                {
                                    var ropQueryRowsResponse = new RopQueryRowsResponse(null);
                                    ropQueryRowsResponse.Parse(parser);
                                    ropsList.Add(ropQueryRowsResponse);
                                    break;
                                }

                            case RopIdType.RopAbort:
                                ropsList.Add(Parse<RopAbortResponse>());
                                break;

                            case RopIdType.RopGetStatus:
                                ropsList.Add(Parse<RopGetStatusResponse>());
                                break;

                            case RopIdType.RopQueryPosition:
                                ropsList.Add(Parse<RopQueryPositionResponse>());
                                break;

                            case RopIdType.RopSeekRow:
                                ropsList.Add(Parse<RopSeekRowResponse>());
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                ropsList.Add(Parse<RopSeekRowBookmarkResponse>());
                                break;

                            case RopIdType.RopSeekRowFractional:
                                ropsList.Add(Parse<RopSeekRowFractionalResponse>());
                                break;

                            case RopIdType.RopCreateBookmark:
                                ropsList.Add(Parse<RopCreateBookmarkResponse>());
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                ropsList.Add(Parse<RopQueryColumnsAllResponse>());
                                break;

                            case RopIdType.RopFindRow:
                                int currentPos_findRow = parser.Offset;
                                parser.Advance(sizeof(RopIdType));
                                BlockT<byte> tempInputHandleIndex_findRow = ParseT<byte>();
                                BlockT<ErrorCodes> returnValue_findRow = ParseT<ErrorCodes>();
                                parser.Offset = currentPos_findRow;
                                uint objHandle_FindRow = tempServerObjectHandleTable[tempInputHandleIndex_findRow];
                                string serverPath_FindRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                                string processName_FindRow = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                                string clientInfo_FindRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                                if (returnValue_findRow == ErrorCodes.Success)
                                {
                                    if (!(DecodingContext.RowRops_handlePropertyTags.ContainsKey(objHandle_FindRow) &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow].ContainsKey(parsingSessionID) &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item1 == serverPath_FindRow &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item2 == processName_FindRow &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item3 == clientInfo_FindRow))
                                    {
                                        ropsList.Add(MissingInformationException.MaybeThrow(
                                            "Missing PropertyTags information for RopFindRowsResponse",
                                            RopIdType.RopFindRow,
                                            new uint[] {
                                                (uint)tempInputHandleIndex_findRow,
                                                objHandle_FindRow
                                            }));
                                        ropsList.Add(ParseJunk("Remaining Data"));
                                    }

                                    var ropFindRowResponse = new RopFindRowResponse(DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item4);
                                    ropFindRowResponse.Parse(parser);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }
                                else
                                {
                                    var ropFindRowResponse = new RopFindRowResponse(null);
                                    ropFindRowResponse.Parse(parser);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }

                            case RopIdType.RopFreeBookmark:
                                ropsList.Add(Parse<RopFreeBookmarkResponse>());
                                break;

                            case RopIdType.RopResetTable:
                                ropsList.Add(Parse<RopResetTableResponse>());
                                break;

                            case RopIdType.RopExpandRow:
                                int currentPos_expandRow = parser.Offset;
                                parser.Advance(sizeof(RopIdType));
                                BlockT<byte> tempInputHandleIndex_expandRow = ParseT<byte>();
                                BlockT<ErrorCodes> returnValue_expandRow = ParseT<ErrorCodes>();
                                parser.Offset = currentPos_expandRow;
                                uint objHandle_ExpandRow = tempServerObjectHandleTable[tempInputHandleIndex_expandRow];
                                string serverPath_ExpandRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                                string processName_ExpandRow = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                                string clientInfo_ExpandRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                                if (returnValue_expandRow == ErrorCodes.Success)
                                {
                                    if (!(DecodingContext.RowRops_handlePropertyTags.ContainsKey(objHandle_ExpandRow) &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow].ContainsKey(parsingSessionID) &&
                                        DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item1 == serverPath_ExpandRow &&
                                    DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item2 == processName_ExpandRow &&
                                    DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item3 == clientInfo_ExpandRow))
                                    {
                                        ropsList.Add(MissingInformationException.MaybeThrow(
                                            "Missing PropertyTags information for RopExpandRowsResponse",
                                            RopIdType.RopExpandRow,
                                            new uint[] {
                                                (uint)tempInputHandleIndex_expandRow,
                                                objHandle_ExpandRow
                                            }));
                                        ropsList.Add(ParseJunk("Remaining Data"));
                                    }

                                    var ropFindRowResponse = new RopExpandRowResponse(DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item4);
                                    ropFindRowResponse.Parse(parser);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }
                                else
                                {
                                    var ropFindRowResponse = new RopExpandRowResponse(null);
                                    ropFindRowResponse.Parse(parser);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }

                            case RopIdType.RopCollapseRow:
                                ropsList.Add(Parse<RopCollapseRowResponse>());
                                break;

                            case RopIdType.RopGetCollapseState:
                                ropsList.Add(Parse<RopGetCollapseStateResponse>());
                                break;

                            case RopIdType.RopSetCollapseState:
                                ropsList.Add(Parse<RopSetCollapseStateResponse>());
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                ropsList.Add(Parse<RopModifyRulesResponse>());
                                break;

                            case RopIdType.RopGetRulesTable:
                                ropsList.Add(Parse<RopGetRulesTableResponse>());
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                ropsList.Add(Parse<RopUpdateDeferredActionMessagesResponse>());
                                break;

                            // MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                ropsList.Add(Parse<RopFastTransferSourceCopyPropertiesResponse>());
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                ropsList.Add(Parse<RopFastTransferSourceCopyToResponse>());
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                ropsList.Add(Parse<RopFastTransferSourceCopyMessagesResponse>());
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                ropsList.Add(Parse<RopFastTransferSourceCopyFolderResponse>());
                                break;
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                int currentPos_getBuffer = parser.Offset;
                                parser.Advance(sizeof(RopIdType));
                                BlockT<byte> tempInputHandleIndex_getBuffer = ParseT<byte>();
                                BlockT<ErrorCodes> returnValue = ParseT<ErrorCodes>();
                                parser.Offset = currentPos_getBuffer;
                                int getParsingSessionID = parsingSessionID;
                                Session getParsingSession = MapiInspector.MAPIParser.ParsingSession;
                                uint ropGetbufferHandle = tempServerObjectHandleTable[tempInputHandleIndex_getBuffer];
                                var partialBeforeAndAfterInformation = new PartialContextInformation[2];
                                if (returnValue == ErrorCodes.Success)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey(getParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException(currentRop, ropGetbufferHandle);
                                    }
                                }

                                var ropFastTransferSourceGetBufferResponse = new RopFastTransferSourceGetBufferResponse();
                                Partial.IsGet = true;
                                ropFastTransferSourceGetBufferResponse.Parse(parser);
                                ropsList.Add(ropFastTransferSourceGetBufferResponse);
                                var getBufferPartialInformation = new PartialContextInformation(
                                    Partial.PartialGetType,
                                    Partial.PartialGetId,
                                    Partial.PartialGetRemainSize,
                                    Partial.PartialGetSubRemainSize,
                                    true,
                                    getParsingSession,
                                    MapiInspector.MAPIParser.OutputPayLoadCompressedXOR);
                                var sessionGetContextInfor = new SortedDictionary<int, PartialContextInformation>();

                                if (Partial.HandleWithSessionGetContextInformation.ContainsKey(ropGetbufferHandle))
                                {
                                    sessionGetContextInfor = Partial.HandleWithSessionGetContextInformation[ropGetbufferHandle];
                                    Partial.HandleWithSessionGetContextInformation.Remove(ropGetbufferHandle);
                                }

                                if (sessionGetContextInfor.ContainsKey(getParsingSessionID))
                                {
                                    sessionGetContextInfor[getParsingSessionID] = getBufferPartialInformation;
                                }
                                else
                                {
                                    sessionGetContextInfor.Add(getParsingSessionID, getBufferPartialInformation);
                                }

                                Partial.HandleWithSessionGetContextInformation.Add(ropGetbufferHandle, sessionGetContextInfor);
                                Partial.IsGet = false;
                                break;

                            case RopIdType.RopTellVersion:
                                ropsList.Add(Parse<RopTellVersionResponse>());
                                break;
                            case RopIdType.RopSynchronizationGetTransferState:
                                ropsList.Add(Parse<RopSynchronizationGetTransferStateResponse>());
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                ropsList.Add(Parse<RopFastTransferDestinationConfigureResponse>());
                                break;
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                                ropsList.Add(Parse<RopFastTransferDestinationPutBufferResponse>());
                                break;
                            case RopIdType.RopFastTransferDestinationPutBufferExtended:
                                ropsList.Add(Parse<RopFastTransferDestinationPutBufferExtendedResponse>());
                                break;
                            case RopIdType.RopSynchronizationConfigure:
                                ropsList.Add(Parse<RopSynchronizationConfigureResponse>());
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                ropsList.Add(Parse<RopSynchronizationUploadStateStreamBeginResponse>());
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                ropsList.Add(Parse<RopSynchronizationUploadStateStreamContinueResponse>());
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                ropsList.Add(Parse<RopSynchronizationUploadStateStreamEndResponse>());
                                break;
                            case RopIdType.RopSynchronizationOpenCollector:
                                ropsList.Add(Parse<RopSynchronizationOpenCollectorResponse>());
                                break;
                            case RopIdType.RopSynchronizationImportMessageChange:
                                ropsList.Add(Parse<RopSynchronizationImportMessageChangeResponse>());
                                break;
                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                ropsList.Add(Parse<RopSynchronizationImportHierarchyChangeResponse>());
                                break;
                            case RopIdType.RopSynchronizationImportMessageMove:
                                ropsList.Add(Parse<RopSynchronizationImportMessageMoveResponse>());
                                break;
                            case RopIdType.RopSynchronizationImportDeletes:
                                ropsList.Add(Parse<RopSynchronizationImportDeletesResponse>());
                                break;
                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                ropsList.Add(Parse<RopSynchronizationImportReadStateChangesResponse>());
                                break;
                            case RopIdType.RopGetLocalReplicaIds:
                                ropsList.Add(Parse<RopGetLocalReplicaIdsResponse>());
                                break;
                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                ropsList.Add(Parse<RopSetLocalReplicaMidsetDeletedResponse>());
                                break;

                            // MS-OXCPRPT ROPs
                            case RopIdType.RopGetPropertiesSpecific:
                                int currentPos_getPropertiesSpec = parser.Offset;
                                parser.Advance(sizeof(RopIdType));
                                BlockT<byte> tempInputHandleIndex_getPropertiesSpec = ParseT<byte>();
                                parser.Offset = currentPos_getPropertiesSpec;
                                if (!(DecodingContext.GetPropertiesSpec_propertyTags.Count > 0 &&
                                    DecodingContext.GetPropertiesSpec_propertyTags.ContainsKey(parsingSessionID) &&
                                    DecodingContext.GetPropertiesSpec_propertyTags[parsingSessionID].ContainsKey((uint)tempInputHandleIndex_getPropertiesSpec) &&
                                    DecodingContext.GetPropertiesSpec_propertyTags[parsingSessionID][(uint)tempInputHandleIndex_getPropertiesSpec].Count != 0))
                                {
                                    ropsList.Add(MissingInformationException.MaybeThrow(
                                        "Missing PropertyTags information for RopGetPropertiesSpecific",
                                        currentRop));
                                    ropsList.Add(ParseJunk("Remaining Data"));
                                }

                                ropsList.Add(Parse<RopGetPropertiesSpecificResponse>());
                                break;

                            case RopIdType.RopGetPropertiesAll:
                                ropsList.Add(Parse<RopGetPropertiesAllResponse>());
                                break;
                            case RopIdType.RopGetPropertiesList:
                                ropsList.Add(Parse<RopGetPropertiesListResponse>());
                                break;
                            case RopIdType.RopSetProperties:
                                ropsList.Add(Parse<RopSetPropertiesResponse>());
                                break;
                            case RopIdType.RopSetPropertiesNoReplicate:
                                ropsList.Add(Parse<RopSetPropertiesNoReplicateResponse>());
                                break;
                            case RopIdType.RopDeleteProperties:
                                ropsList.Add(Parse<RopDeletePropertiesResponse>());
                                break;
                            case RopIdType.RopDeletePropertiesNoReplicate:
                                ropsList.Add(Parse<RopDeletePropertiesNoReplicateResponse>());
                                break;
                            case RopIdType.RopQueryNamedProperties:
                                ropsList.Add(Parse<RopQueryNamedPropertiesResponse>());
                                break;
                            case RopIdType.RopCopyProperties:
                                ropsList.Add(Parse<RopCopyPropertiesResponse>());
                                break;
                            case RopIdType.RopCopyTo:
                                ropsList.Add(Parse<RopCopyToResponse>());
                                break;
                            case RopIdType.RopGetPropertyIdsFromNames:
                                ropsList.Add(Parse<RopGetPropertyIdsFromNamesResponse>());
                                break;
                            case RopIdType.RopGetNamesFromPropertyIds:
                                ropsList.Add(Parse<RopGetNamesFromPropertyIdsResponse>());
                                break;
                            case RopIdType.RopOpenStream:
                                ropsList.Add(Parse<RopOpenStreamResponse>());
                                break;
                            case RopIdType.RopReadStream:
                                ropsList.Add(Parse<RopReadStreamResponse>());
                                break;
                            case RopIdType.RopWriteStream:
                                ropsList.Add(Parse<RopWriteStreamResponse>());
                                break;
                            case RopIdType.RopWriteStreamExtended:
                                ropsList.Add(Parse<RopWriteStreamExtendedResponse>());
                                break;
                            case RopIdType.RopCommitStream:
                                ropsList.Add(Parse<RopCommitStreamResponse>());
                                break;
                            case RopIdType.RopGetStreamSize:
                                ropsList.Add(Parse<RopGetStreamSizeResponse>());
                                break;
                            case RopIdType.RopSetStreamSize:
                                ropsList.Add(Parse<RopSetStreamSizeResponse>());
                                break;
                            case RopIdType.RopSeekStream:
                                ropsList.Add(Parse<RopSeekStreamResponse>());
                                break;
                            case RopIdType.RopCopyToStream:
                                ropsList.Add(Parse<RopCopyToStreamResponse>());
                                break;
                            case RopIdType.RopProgress:
                                ropsList.Add(Parse<RopProgressResponse>());
                                break;
                            case RopIdType.RopLockRegionStream:
                                ropsList.Add(Parse<RopLockRegionStreamResponse>());
                                break;
                            case RopIdType.RopUnlockRegionStream:
                                ropsList.Add(Parse<RopUnlockRegionStreamResponse>());
                                break;
                            case RopIdType.RopWriteAndCommitStream:
                                ropsList.Add(Parse<RopWriteAndCommitStreamResponse>());
                                break;
                            case RopIdType.RopCloneStream:
                                ropsList.Add(Parse<RopCloneStreamResponse>());
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                ropsList.Add(Parse<RopOpenFolderResponse>());
                                break;

                            case RopIdType.RopCreateFolder:
                                ropsList.Add(Parse<RopCreateFolderResponse>());
                                break;

                            case RopIdType.RopDeleteFolder:
                                ropsList.Add(Parse<RopDeleteFolderResponse>());
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                ropsList.Add(Parse<RopSetSearchCriteriaResponse>());
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                ropsList.Add(Parse<RopGetSearchCriteriaResponse>());
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                ropsList.Add(Parse<RopMoveCopyMessagesResponse>());
                                break;

                            case RopIdType.RopMoveFolder:
                                ropsList.Add(Parse<RopMoveFolderResponse>());
                                break;

                            case RopIdType.RopCopyFolder:
                                ropsList.Add(Parse<RopCopyFolderResponse>());
                                break;

                            case RopIdType.RopEmptyFolder:
                                ropsList.Add(Parse<RopEmptyFolderResponse>());
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                ropsList.Add(Parse<RopHardDeleteMessagesAndSubfoldersResponse>());
                                break;

                            case RopIdType.RopDeleteMessages:
                                ropsList.Add(Parse<RopDeleteMessagesResponse>());
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                ropsList.Add(Parse<RopHardDeleteMessagesResponse>());
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                ropsList.Add(Parse<RopGetHierarchyTableResponse>());
                                break;

                            case RopIdType.RopGetContentsTable:
                                ropsList.Add(Parse<RopGetContentsTableResponse>());
                                break;

                            // MS-OXCMSG ROPs
                            case RopIdType.RopOpenMessage:
                                ropsList.Add(Parse<RopOpenMessageResponse>());
                                break;

                            case RopIdType.RopCreateMessage:
                                ropsList.Add(Parse<RopCreateMessageResponse>());
                                break;

                            case RopIdType.RopSaveChangesMessage:
                                ropsList.Add(Parse<RopSaveChangesMessageResponse>());
                                break;

                            case RopIdType.RopRemoveAllRecipients:
                                ropsList.Add(Parse<RopRemoveAllRecipientsResponse>());
                                break;

                            case RopIdType.RopModifyRecipients:
                                ropsList.Add(Parse<RopModifyRecipientsResponse>());
                                break;

                            case RopIdType.RopReadRecipients:
                                ropsList.Add(Parse<RopReadRecipientsResponse>());
                                break;

                            case RopIdType.RopReloadCachedInformation:
                                ropsList.Add(Parse<RopReloadCachedInformationResponse>());
                                break;
                            case RopIdType.RopSetMessageStatus:
                                ropsList.Add(Parse<RopSetMessageStatusResponse>());
                                break;

                            case RopIdType.RopGetMessageStatus:
                                ropsList.Add(Parse<RopGetMessageStatusResponse>());
                                break;

                            case RopIdType.RopSetReadFlags:
                                ropsList.Add(Parse<RopSetReadFlagsResponse>());
                                break;
                            case RopIdType.RopSetMessageReadFlag:
                                ropsList.Add(Parse<RopSetMessageReadFlagResponse>());
                                break;

                            case RopIdType.RopOpenAttachment:
                                ropsList.Add(Parse<RopOpenAttachmentResponse>());
                                break;

                            case RopIdType.RopCreateAttachment:
                                ropsList.Add(Parse<RopCreateAttachmentResponse>());
                                break;

                            case RopIdType.RopDeleteAttachment:
                                ropsList.Add(Parse<RopDeleteAttachmentResponse>());
                                break;

                            case RopIdType.RopSaveChangesAttachment:
                                ropsList.Add(Parse<RopSaveChangesAttachmentResponse>());
                                break;

                            case RopIdType.RopOpenEmbeddedMessage:
                                ropsList.Add(Parse<RopOpenEmbeddedMessageResponse>());
                                break;

                            case RopIdType.RopGetAttachmentTable:
                                ropsList.Add(Parse<RopGetAttachmentTableResponse>());
                                break;

                            case RopIdType.RopGetValidAttachments:
                                ropsList.Add(Parse<RopGetValidAttachmentsResponse>());
                                break;

                            // MSOXCNOTIF ROPs
                            case RopIdType.RopRegisterNotification:
                                ropsList.Add(Parse<RopRegisterNotificationResponse>());
                                break;

                            case RopIdType.RopPending:
                                ropsList.Add(Parse<RopPendingResponse>());
                                break;

                            case RopIdType.RopNotify:
                                ropsList.Add(Parse<RopNotifyResponse>());
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                ropsList.Add(Parse<RopGetPermissionsTableResponse>());
                                break;

                            case RopIdType.RopModifyPermissions:
                                ropsList.Add(Parse<RopModifyPermissionsResponse>());
                                break;

                            default:
                                ropsList.Add(ParseJunk("Remaining Data"));
                                break;
                        }
                    }
                    while (parser.RemainingBytes > 0);

                    parser.PopCap();
                }
                else
                {
                    RopsList = null;
                }
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
                    if (MapiInspector.MAPIParser.AllRopsList.Count <= 0 || !MapiInspector.MAPIParser.AllRopsList.Contains(obj.GetType().Name))
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
            Text = "ROPOutputBuffer";
            AddChildBlockT(RopSize, "RopSize");
            AddLabeledChildren(RopsList, "RopsList");
            AddLabeledChildren(ServerObjectHandleTable, "ServerObjectHandleTable");
        }
    }
}
