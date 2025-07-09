using BlockParser;
using Fiddler;
using System.Collections.Generic;
using System.Linq;

namespace MAPIInspector.Parsers
{
    /// <summary>
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
            parser.Advance(RopSize - sizeof(RopIdType));
            int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
            }
            while (parser.RemainingBytes > 0)
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

                if (RopSize > sizeof(RopIdType))
                {
                    do
                    {
                        BlockT<RopIdType> currentRop = TestParse<RopIdType>(parser);

                        switch (currentRop.Data)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                int currentPos_logon = parser.Offset;
                                parser.Advance(sizeof(RopIdType));
                                BlockT<uint> tempOutputHandleIndex_logon = ParseT<uint>();
                                parser.Offset = currentPos_logon;
                                if (!(DecodingContext.SessionLogonFlagsInLogonRop.Count > 0 &&
                                    DecodingContext.SessionLogonFlagsInLogonRop.ContainsKey(parsingSessionID) &&
                                    DecodingContext.SessionLogonFlagsInLogonRop[parsingSessionID].ContainsKey(tempOutputHandleIndex_logon)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopLogon", currentRop);
                                }
                                else
                                {
                                    if (((byte)DecodingContext.SessionLogonFlagsInLogonRop[parsingSessionID][tempOutputHandleIndex_logon] & 0x01) == (byte)LogonFlags.Private)
                                    {
                                        var ropLogonResponse_PrivateMailboxes = new RopLogonResponse_PrivateMailboxes();
                                        ropLogonResponse_PrivateMailboxes.Parse(parser);
                                        ropsList.Add(ropLogonResponse_PrivateMailboxes);
                                        break;
                                    }
                                    else
                                    {
                                        var ropLogonResponse_PublicFolders = new RopLogonResponse_PublicFolders();
                                        ropLogonResponse_PublicFolders.Parse(parser);
                                        ropsList.Add(ropLogonResponse_PublicFolders);
                                        break;
                                    }
                                }

                            case RopIdType.RopGetReceiveFolder:
                                var ropGetReceiveFolderResponse = new RopGetReceiveFolderResponse();
                                ropGetReceiveFolderResponse.Parse(parser);
                                ropsList.Add(ropGetReceiveFolderResponse);
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                var ropSetReceiveFolderResponse = new RopSetReceiveFolderResponse();
                                ropSetReceiveFolderResponse.Parse(parser);
                                ropsList.Add(ropSetReceiveFolderResponse);
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                var ropGetReceiveFolderTableResponse = new RopGetReceiveFolderTableResponse();
                                ropGetReceiveFolderTableResponse.Parse(parser);
                                ropsList.Add(ropGetReceiveFolderTableResponse);
                                break;
                            case RopIdType.RopGetStoreState:
                                var ropGetStoreStateResponse = new RopGetStoreStateResponse();
                                ropGetStoreStateResponse.Parse(parser);
                                ropsList.Add(ropGetStoreStateResponse);
                                break;
                            case RopIdType.RopGetOwningServers:
                                var ropGetOwningServersResponse = new RopGetOwningServersResponse();
                                ropGetOwningServersResponse.Parse(parser);
                                ropsList.Add(ropGetOwningServersResponse);
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                var ropPublicFolderIsGhostedResponse = new RopPublicFolderIsGhostedResponse();
                                ropPublicFolderIsGhostedResponse.Parse(parser);
                                ropsList.Add(ropPublicFolderIsGhostedResponse);
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                var ropLongTermIdFromIdResponse = new RopLongTermIdFromIdResponse();
                                ropLongTermIdFromIdResponse.Parse(parser);
                                ropsList.Add(ropLongTermIdFromIdResponse);
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                var ropIdFromLongTermIdResponse = new RopIdFromLongTermIdResponse();
                                ropIdFromLongTermIdResponse.Parse(parser);
                                ropsList.Add(ropIdFromLongTermIdResponse);
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                var ropGetPerUserLongTermIdsResponse = new RopGetPerUserLongTermIdsResponse();
                                ropGetPerUserLongTermIdsResponse.Parse(parser);
                                ropsList.Add(ropGetPerUserLongTermIdsResponse);
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                var ropGetPerUserGuidResponse = new RopGetPerUserGuidResponse();
                                ropGetPerUserGuidResponse.Parse(parser);
                                ropsList.Add(ropGetPerUserGuidResponse);
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                var ropReadPerUserInformationResponse = new RopReadPerUserInformationResponse();
                                ropReadPerUserInformationResponse.Parse(parser);
                                ropsList.Add(ropReadPerUserInformationResponse);
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                var ropWritePerUserInformationResponse = new RopWritePerUserInformationResponse();
                                ropWritePerUserInformationResponse.Parse(parser);
                                ropsList.Add(ropWritePerUserInformationResponse);
                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                var ropSubmitMessageResponse = new RopSubmitMessageResponse();
                                ropSubmitMessageResponse.Parse(parser);
                                ropsList.Add(ropSubmitMessageResponse);
                                break;
                            case RopIdType.RopAbortSubmit:
                                var ropAbortSubmitResponse = new RopAbortSubmitResponse();
                                ropAbortSubmitResponse.Parse(parser);
                                ropsList.Add(ropAbortSubmitResponse);
                                break;
                            case RopIdType.RopGetAddressTypes:
                                var ropGetAddressTypesResponse = new RopGetAddressTypesResponse();
                                ropGetAddressTypesResponse.Parse(parser);
                                ropsList.Add(ropGetAddressTypesResponse);
                                break;
                            case RopIdType.RopSetSpooler:
                                var ropSetSpoolerResponse = new RopSetSpoolerResponse();
                                ropSetSpoolerResponse.Parse(parser);
                                ropsList.Add(ropSetSpoolerResponse);
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                var ropSpoolerLockMessageResponse = new RopSpoolerLockMessageResponse();
                                ropSpoolerLockMessageResponse.Parse(parser);
                                ropsList.Add(ropSpoolerLockMessageResponse);
                                break;
                            case RopIdType.RopTransportSend:
                                var ropTransportSendResponse = new RopTransportSendResponse();
                                ropTransportSendResponse.Parse(parser);
                                ropsList.Add(ropTransportSendResponse);
                                break;
                            case RopIdType.RopTransportNewMail:
                                var ropTransportNewMailResponse = new RopTransportNewMailResponse();
                                ropTransportNewMailResponse.Parse(parser);
                                ropsList.Add(ropTransportNewMailResponse);
                                break;
                            case RopIdType.RopGetTransportFolder:
                                var ropGetTransportFolderResponse = new RopGetTransportFolderResponse();
                                ropGetTransportFolderResponse.Parse(parser);
                                ropsList.Add(ropGetTransportFolderResponse);
                                break;
                            case RopIdType.RopOptionsData:
                                var ropOptionsDataResponse = new RopOptionsDataResponse();
                                ropOptionsDataResponse.Parse(parser);
                                ropsList.Add(ropOptionsDataResponse);
                                break;
                            case RopIdType.RopBackoff:
                                var ropBackoffResponse = new RopBackoffResponse();
                                ropBackoffResponse.Parse(parser);
                                ropsList.Add(ropBackoffResponse);
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
                                    break;
                                }
                                else
                                {
                                    throw new MissingInformationException("Missing RequestBuffersSize information for RopBufferTooSmall", currentRop);
                                }

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
                                var ropSortTableResponse = new RopSortTableResponse();
                                ropSortTableResponse.Parse(parser);
                                ropsList.Add(ropSortTableResponse);
                                break;

                            case RopIdType.RopRestrict:
                                var ropRestrictResponse = new RopRestrictResponse();
                                ropRestrictResponse.Parse(parser);
                                ropsList.Add(ropRestrictResponse);
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
                                        throw new MissingInformationException(
                                            "Missing PropertyTags information for RopQueryRowsResponse",
                                            RopIdType.RopQueryRows,
                                            new uint[] {
                                                (uint)tempInputHandleIndex_QueryRow,
                                                tempServerObjectHandleTable[tempInputHandleIndex_QueryRow]
                                            });
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
                                var ropAbortResponse = new RopAbortResponse();
                                ropAbortResponse.Parse(parser);
                                ropsList.Add(ropAbortResponse);
                                break;

                            case RopIdType.RopGetStatus:
                                var ropGetStatusResponse = new RopGetStatusResponse();
                                ropGetStatusResponse.Parse(parser);
                                ropsList.Add(ropGetStatusResponse);
                                break;

                            case RopIdType.RopQueryPosition:
                                var ropQueryPositionResponse = new RopQueryPositionResponse();
                                ropQueryPositionResponse.Parse(parser);
                                ropsList.Add(ropQueryPositionResponse);
                                break;

                            case RopIdType.RopSeekRow:
                                ropsList.Add(Parse<RopSeekRowResponse>());
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                var ropSeekRowBookmarkResponse = new RopSeekRowBookmarkResponse();
                                ropSeekRowBookmarkResponse.Parse(parser);
                                ropsList.Add(ropSeekRowBookmarkResponse);
                                break;

                            case RopIdType.RopSeekRowFractional:
                                var ropSeekRowFractionalResponse = new RopSeekRowFractionalResponse();
                                ropSeekRowFractionalResponse.Parse(parser);
                                ropsList.Add(ropSeekRowFractionalResponse);
                                break;

                            case RopIdType.RopCreateBookmark:
                                var ropCreateBookmarkResponse = new RopCreateBookmarkResponse();
                                ropCreateBookmarkResponse.Parse(parser);
                                ropsList.Add(ropCreateBookmarkResponse);
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                var ropQueryColumnsAllResponse = new RopQueryColumnsAllResponse();
                                ropQueryColumnsAllResponse.Parse(parser);
                                ropsList.Add(ropQueryColumnsAllResponse);
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
                                        throw new MissingInformationException(
                                            "Missing PropertyTags information for RopFindRowsResponse",
                                            RopIdType.RopFindRow,
                                            new uint[] {
                                                (uint)tempInputHandleIndex_findRow,
                                                objHandle_FindRow
                                            });
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
                                var ropFreeBookmarkResponse = new RopFreeBookmarkResponse();
                                ropFreeBookmarkResponse.Parse(parser);
                                ropsList.Add(ropFreeBookmarkResponse);
                                break;

                            case RopIdType.RopResetTable:
                                var ropResetTableResponse = new RopResetTableResponse();
                                ropResetTableResponse.Parse(parser);
                                ropsList.Add(ropResetTableResponse);
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
                                        throw new MissingInformationException(
                                            "Missing PropertyTags information for RopExpandRowsResponse",
                                            RopIdType.RopExpandRow,
                                            new uint[] {
                                                (uint)tempInputHandleIndex_expandRow,
                                                objHandle_ExpandRow
                                            });
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
                                var ropCollapseRowResponse = new RopCollapseRowResponse();
                                ropCollapseRowResponse.Parse(parser);
                                ropsList.Add(ropCollapseRowResponse);
                                break;

                            case RopIdType.RopGetCollapseState:
                                var ropGetCollapseStateResponse = new RopGetCollapseStateResponse();
                                ropGetCollapseStateResponse.Parse(parser);
                                ropsList.Add(ropGetCollapseStateResponse);
                                break;

                            case RopIdType.RopSetCollapseState:
                                var ropSetCollapseStateResponse = new RopSetCollapseStateResponse();
                                ropSetCollapseStateResponse.Parse(parser);
                                ropsList.Add(ropSetCollapseStateResponse);
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                var ropModifyRulesResponse = new RopModifyRulesResponse();
                                ropModifyRulesResponse.Parse(parser);
                                ropsList.Add(ropModifyRulesResponse);
                                break;

                            case RopIdType.RopGetRulesTable:
                                var ropGetRulesTableResponse = new RopGetRulesTableResponse();
                                ropGetRulesTableResponse.Parse(parser);
                                ropsList.Add(ropGetRulesTableResponse);
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                var ropUpdateDeferredActionMessagesResponse = new RopUpdateDeferredActionMessagesResponse();
                                ropUpdateDeferredActionMessagesResponse.Parse(parser);
                                ropsList.Add(ropUpdateDeferredActionMessagesResponse);
                                break;

                            // MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                var ropFastTransferSourceCopyPropertiesResponse = new RopFastTransferSourceCopyPropertiesResponse();
                                ropFastTransferSourceCopyPropertiesResponse.Parse(parser);
                                ropsList.Add(ropFastTransferSourceCopyPropertiesResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                var ropFastTransferSourceCopyToResponse = new RopFastTransferSourceCopyToResponse();
                                ropFastTransferSourceCopyToResponse.Parse(parser);
                                ropsList.Add(ropFastTransferSourceCopyToResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                var ropFastTransferSourceCopyMessagesResponse = new RopFastTransferSourceCopyMessagesResponse();
                                ropFastTransferSourceCopyMessagesResponse.Parse(parser);
                                ropsList.Add(ropFastTransferSourceCopyMessagesResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                var ropFastTransferSourceCopyFolderResponse = new RopFastTransferSourceCopyFolderResponse();
                                ropFastTransferSourceCopyFolderResponse.Parse(parser);
                                ropsList.Add(ropFastTransferSourceCopyFolderResponse);
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
                                var ropTellVersionResponse = new RopTellVersionResponse();
                                ropTellVersionResponse.Parse(parser);
                                ropsList.Add(ropTellVersionResponse);
                                break;
                            case RopIdType.RopSynchronizationGetTransferState:
                                var ropSynchronizationGetTransferStateResponse = new RopSynchronizationGetTransferStateResponse();
                                ropSynchronizationGetTransferStateResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationGetTransferStateResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                var ropFastTransferDestinationConfigureResponse = new RopFastTransferDestinationConfigureResponse();
                                ropFastTransferDestinationConfigureResponse.Parse(parser);
                                ropsList.Add(ropFastTransferDestinationConfigureResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                                var ropFastTransferDestinationPutBufferResponse = new RopFastTransferDestinationPutBufferResponse();
                                ropFastTransferDestinationPutBufferResponse.Parse(parser);
                                ropsList.Add(ropFastTransferDestinationPutBufferResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBufferExtended:
                                var ropFastTransferDestinationPutBufferExtendedResponse = new RopFastTransferDestinationPutBufferExtendedResponse();
                                ropFastTransferDestinationPutBufferExtendedResponse.Parse(parser);
                                ropsList.Add(ropFastTransferDestinationPutBufferExtendedResponse);
                                break;
                            case RopIdType.RopSynchronizationConfigure:
                                var ropSynchronizationConfigureResponse = new RopSynchronizationConfigureResponse();
                                ropSynchronizationConfigureResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationConfigureResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                var ropSynchronizationUploadStateStreamBeginResponse = new RopSynchronizationUploadStateStreamBeginResponse();
                                ropSynchronizationUploadStateStreamBeginResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationUploadStateStreamBeginResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                var ropSynchronizationUploadStateStreamContinueResponse = new RopSynchronizationUploadStateStreamContinueResponse();
                                ropSynchronizationUploadStateStreamContinueResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationUploadStateStreamContinueResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                var ropSynchronizationUploadStateStreamEndResponse = new RopSynchronizationUploadStateStreamEndResponse();
                                ropSynchronizationUploadStateStreamEndResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationUploadStateStreamEndResponse);
                                break;
                            case RopIdType.RopSynchronizationOpenCollector:
                                var ropSynchronizationOpenCollectorResponse = new RopSynchronizationOpenCollectorResponse();
                                ropSynchronizationOpenCollectorResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationOpenCollectorResponse);
                                break;
                            case RopIdType.RopSynchronizationImportMessageChange:
                                var ropSynchronizationImportMessageChangeResponse = new RopSynchronizationImportMessageChangeResponse();
                                ropSynchronizationImportMessageChangeResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationImportMessageChangeResponse);
                                break;
                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                var ropSynchronizationImportHierarchyChangeResponse = new RopSynchronizationImportHierarchyChangeResponse();
                                ropSynchronizationImportHierarchyChangeResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationImportHierarchyChangeResponse);
                                break;
                            case RopIdType.RopSynchronizationImportMessageMove:
                                var ropSynchronizationImportMessageMoveResponse = new RopSynchronizationImportMessageMoveResponse();
                                ropSynchronizationImportMessageMoveResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationImportMessageMoveResponse);
                                break;
                            case RopIdType.RopSynchronizationImportDeletes:
                                var ropSynchronizationImportDeletesResponse = new RopSynchronizationImportDeletesResponse();
                                ropSynchronizationImportDeletesResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationImportDeletesResponse);
                                break;
                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                var ropSynchronizationImportReadStateChangesResponse = new RopSynchronizationImportReadStateChangesResponse();
                                ropSynchronizationImportReadStateChangesResponse.Parse(parser);
                                ropsList.Add(ropSynchronizationImportReadStateChangesResponse);
                                break;
                            case RopIdType.RopGetLocalReplicaIds:
                                var ropGetLocalReplicaIdsResponse = new RopGetLocalReplicaIdsResponse();
                                ropGetLocalReplicaIdsResponse.Parse(parser);
                                ropsList.Add(ropGetLocalReplicaIdsResponse);
                                break;
                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                var ropSetLocalReplicaMidsetDeletedResponse = new RopSetLocalReplicaMidsetDeletedResponse();
                                ropSetLocalReplicaMidsetDeletedResponse.Parse(parser);
                                ropsList.Add(ropSetLocalReplicaMidsetDeletedResponse);
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
                                    throw new MissingInformationException("Missing PropertyTags information for RopGetPropertiesSpecific", currentRop);
                                }

                                var ropGetPropertiesSpecificResponse = new RopGetPropertiesSpecificResponse();
                                ropGetPropertiesSpecificResponse.Parse(parser);
                                ropsList.Add(ropGetPropertiesSpecificResponse);
                                break;

                            case RopIdType.RopGetPropertiesAll:
                                var ropGetPropertiesAllResponse = new RopGetPropertiesAllResponse();
                                ropGetPropertiesAllResponse.Parse(parser);
                                ropsList.Add(ropGetPropertiesAllResponse);
                                break;
                            case RopIdType.RopGetPropertiesList:
                                var ropGetPropertiesListResponse = new RopGetPropertiesListResponse();
                                ropGetPropertiesListResponse.Parse(parser);
                                ropsList.Add(ropGetPropertiesListResponse);
                                break;
                            case RopIdType.RopSetProperties:
                                var ropSetPropertiesResponse = new RopSetPropertiesResponse();
                                ropSetPropertiesResponse.Parse(parser);
                                ropsList.Add(ropSetPropertiesResponse);
                                break;
                            case RopIdType.RopSetPropertiesNoReplicate:
                                var ropSetPropertiesNoReplicateResponse = new RopSetPropertiesNoReplicateResponse();
                                ropSetPropertiesNoReplicateResponse.Parse(parser);
                                ropsList.Add(ropSetPropertiesNoReplicateResponse);
                                break;
                            case RopIdType.RopDeleteProperties:
                                var ropDeletePropertiesResponse = new RopDeletePropertiesResponse();
                                ropDeletePropertiesResponse.Parse(parser);
                                ropsList.Add(ropDeletePropertiesResponse);
                                break;
                            case RopIdType.RopDeletePropertiesNoReplicate:
                                var ropDeletePropertiesNoReplicateResponse = new RopDeletePropertiesNoReplicateResponse();
                                ropDeletePropertiesNoReplicateResponse.Parse(parser);
                                ropsList.Add(ropDeletePropertiesNoReplicateResponse);
                                break;
                            case RopIdType.RopQueryNamedProperties:
                                var ropQueryNamedPropertiesResponse = new RopQueryNamedPropertiesResponse();
                                ropQueryNamedPropertiesResponse.Parse(parser);
                                ropsList.Add(ropQueryNamedPropertiesResponse);
                                break;
                            case RopIdType.RopCopyProperties:
                                var ropCopyPropertiesResponse = new RopCopyPropertiesResponse();
                                ropCopyPropertiesResponse.Parse(parser);
                                ropsList.Add(ropCopyPropertiesResponse);
                                break;
                            case RopIdType.RopCopyTo:
                                var ropCopyToResponse = new RopCopyToResponse();
                                ropCopyToResponse.Parse(parser);
                                ropsList.Add(ropCopyToResponse);
                                break;
                            case RopIdType.RopGetPropertyIdsFromNames:
                                var ropGetPropertyIdsFromNamesResponse = new RopGetPropertyIdsFromNamesResponse();
                                ropGetPropertyIdsFromNamesResponse.Parse(parser);
                                ropsList.Add(ropGetPropertyIdsFromNamesResponse);
                                break;
                            case RopIdType.RopGetNamesFromPropertyIds:
                                var ropGetNamesFromPropertyIdsResponse = new RopGetNamesFromPropertyIdsResponse();
                                ropGetNamesFromPropertyIdsResponse.Parse(parser);
                                ropsList.Add(ropGetNamesFromPropertyIdsResponse);
                                break;
                            case RopIdType.RopOpenStream:
                                var ropOpenStreamResponse = new RopOpenStreamResponse();
                                ropOpenStreamResponse.Parse(parser);
                                ropsList.Add(ropOpenStreamResponse);
                                break;
                            case RopIdType.RopReadStream:
                                var ropReadStreamResponse = new RopReadStreamResponse();
                                ropReadStreamResponse.Parse(parser);
                                ropsList.Add(ropReadStreamResponse);
                                break;
                            case RopIdType.RopWriteStream:
                                var ropWriteStreamResponse = new RopWriteStreamResponse();
                                ropWriteStreamResponse.Parse(parser);
                                ropsList.Add(ropWriteStreamResponse);
                                break;
                            case RopIdType.RopWriteStreamExtended:
                                var ropWriteStreamExtendedResponse = new RopWriteStreamExtendedResponse();
                                ropWriteStreamExtendedResponse.Parse(parser);
                                ropsList.Add(ropWriteStreamExtendedResponse);
                                break;
                            case RopIdType.RopCommitStream:
                                var ropCommitStreamResponse = new RopCommitStreamResponse();
                                ropCommitStreamResponse.Parse(parser);
                                ropsList.Add(ropCommitStreamResponse);
                                break;
                            case RopIdType.RopGetStreamSize:
                                var ropGetStreamSizeResponse = new RopGetStreamSizeResponse();
                                ropGetStreamSizeResponse.Parse(parser);
                                ropsList.Add(ropGetStreamSizeResponse);
                                break;
                            case RopIdType.RopSetStreamSize:
                                var ropSetStreamSizeResponse = new RopSetStreamSizeResponse();
                                ropSetStreamSizeResponse.Parse(parser);
                                ropsList.Add(ropSetStreamSizeResponse);
                                break;
                            case RopIdType.RopSeekStream:
                                var ropSeekStreamResponse = new RopSeekStreamResponse();
                                ropSeekStreamResponse.Parse(parser);
                                ropsList.Add(ropSeekStreamResponse);
                                break;
                            case RopIdType.RopCopyToStream:
                                var ropCopyToStreamResponse = new RopCopyToStreamResponse();
                                ropCopyToStreamResponse.Parse(parser);
                                ropsList.Add(ropCopyToStreamResponse);
                                break;
                            case RopIdType.RopProgress:
                                var ropProgressResponse = new RopProgressResponse();
                                ropProgressResponse.Parse(parser);
                                ropsList.Add(ropProgressResponse);
                                break;
                            case RopIdType.RopLockRegionStream:
                                var ropLockRegionStreamResponse = new RopLockRegionStreamResponse();
                                ropLockRegionStreamResponse.Parse(parser);
                                ropsList.Add(ropLockRegionStreamResponse);
                                break;
                            case RopIdType.RopUnlockRegionStream:
                                var ropUnlockRegionStreamResponse = new RopUnlockRegionStreamResponse();
                                ropUnlockRegionStreamResponse.Parse(parser);
                                ropsList.Add(ropUnlockRegionStreamResponse);
                                break;
                            case RopIdType.RopWriteAndCommitStream:
                                var ropWriteAndCommitStreamResponse = new RopWriteAndCommitStreamResponse();
                                ropWriteAndCommitStreamResponse.Parse(parser);
                                ropsList.Add(ropWriteAndCommitStreamResponse);
                                break;
                            case RopIdType.RopCloneStream:
                                var ropCloneStreamResponse = new RopCloneStreamResponse();
                                ropCloneStreamResponse.Parse(parser);
                                ropsList.Add(ropCloneStreamResponse);
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                var ropOpenFolderResponse = new RopOpenFolderResponse();
                                ropOpenFolderResponse.Parse(parser);
                                ropsList.Add(ropOpenFolderResponse);
                                break;

                            case RopIdType.RopCreateFolder:
                                var ropCreateFolderResponse = new RopCreateFolderResponse();
                                ropCreateFolderResponse.Parse(parser);
                                ropsList.Add(ropCreateFolderResponse);
                                break;

                            case RopIdType.RopDeleteFolder:
                                var ropDeleteFolderResponse = new RopDeleteFolderResponse();
                                ropDeleteFolderResponse.Parse(parser);
                                ropsList.Add(ropDeleteFolderResponse);
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                var ropSetSearchCriteriaResponse = new RopSetSearchCriteriaResponse();
                                ropSetSearchCriteriaResponse.Parse(parser);
                                ropsList.Add(ropSetSearchCriteriaResponse);
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                var ropGetSearchCriteriaResponse = new RopGetSearchCriteriaResponse();
                                ropGetSearchCriteriaResponse.Parse(parser);
                                ropsList.Add(ropGetSearchCriteriaResponse);
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                var ropMoveCopyMessagesResponse = new RopMoveCopyMessagesResponse();
                                ropMoveCopyMessagesResponse.Parse(parser);
                                ropsList.Add(ropMoveCopyMessagesResponse);
                                break;

                            case RopIdType.RopMoveFolder:
                                var ropMoveFolderResponse = new RopMoveFolderResponse();
                                ropMoveFolderResponse.Parse(parser);
                                ropsList.Add(ropMoveFolderResponse);
                                break;

                            case RopIdType.RopCopyFolder:
                                var ropCopyFolderResponse = new RopCopyFolderResponse();
                                ropCopyFolderResponse.Parse(parser);
                                ropsList.Add(ropCopyFolderResponse);
                                break;

                            case RopIdType.RopEmptyFolder:
                                var ropEmptyFolderResponse = new RopEmptyFolderResponse();
                                ropEmptyFolderResponse.Parse(parser);
                                ropsList.Add(ropEmptyFolderResponse);
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                var ropHardDeleteMessagesAndSubfoldersResponse = new RopHardDeleteMessagesAndSubfoldersResponse();
                                ropHardDeleteMessagesAndSubfoldersResponse.Parse(parser);
                                ropsList.Add(ropHardDeleteMessagesAndSubfoldersResponse);
                                break;

                            case RopIdType.RopDeleteMessages:
                                var ropDeleteMessagesResponse = new RopDeleteMessagesResponse();
                                ropDeleteMessagesResponse.Parse(parser);
                                ropsList.Add(ropDeleteMessagesResponse);
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                var ropHardDeleteMessagesResponse = new RopHardDeleteMessagesResponse();
                                ropHardDeleteMessagesResponse.Parse(parser);
                                ropsList.Add(ropHardDeleteMessagesResponse);
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                var ropGetHierarchyTableResponse = new RopGetHierarchyTableResponse();
                                ropGetHierarchyTableResponse.Parse(parser);
                                ropsList.Add(ropGetHierarchyTableResponse);
                                break;

                            case RopIdType.RopGetContentsTable:
                                var ropGetContentsTableResponse = new RopGetContentsTableResponse();
                                ropGetContentsTableResponse.Parse(parser);
                                ropsList.Add(ropGetContentsTableResponse);
                                break;

                            // MS-OXCMSG ROPs
                            case RopIdType.RopOpenMessage:
                                var ropOpenMessageResponse = new RopOpenMessageResponse();
                                ropOpenMessageResponse.Parse(parser);
                                ropsList.Add(ropOpenMessageResponse);
                                break;

                            case RopIdType.RopCreateMessage:
                                var ropCreateMessageResponse = new RopCreateMessageResponse();
                                ropCreateMessageResponse.Parse(parser);
                                ropsList.Add(ropCreateMessageResponse);
                                break;

                            case RopIdType.RopSaveChangesMessage:
                                var ropSaveChangesMessageResponse = new RopSaveChangesMessageResponse();
                                ropSaveChangesMessageResponse.Parse(parser);
                                ropsList.Add(ropSaveChangesMessageResponse);
                                break;

                            case RopIdType.RopRemoveAllRecipients:
                                var ropRemoveAllRecipientsResponse = new RopRemoveAllRecipientsResponse();
                                ropRemoveAllRecipientsResponse.Parse(parser);
                                ropsList.Add(ropRemoveAllRecipientsResponse);
                                break;

                            case RopIdType.RopModifyRecipients:
                                var ropModifyRecipientsResponse = new RopModifyRecipientsResponse();
                                ropModifyRecipientsResponse.Parse(parser);
                                ropsList.Add(ropModifyRecipientsResponse);
                                break;

                            case RopIdType.RopReadRecipients:
                                var ropReadRecipientsResponse = new RopReadRecipientsResponse();
                                ropReadRecipientsResponse.Parse(parser);
                                ropsList.Add(ropReadRecipientsResponse);
                                break;

                            case RopIdType.RopReloadCachedInformation:
                                var ropReloadCachedInformationResponse = new RopReloadCachedInformationResponse();
                                ropReloadCachedInformationResponse.Parse(parser);
                                ropsList.Add(ropReloadCachedInformationResponse);
                                break;
                            case RopIdType.RopSetMessageStatus:
                                var ropSetMessageStatusResponse = new RopSetMessageStatusResponse();
                                ropSetMessageStatusResponse.Parse(parser);
                                ropsList.Add(ropSetMessageStatusResponse);
                                break;

                            case RopIdType.RopGetMessageStatus:
                                var ropGetMessageStatusResponse = new RopGetMessageStatusResponse();
                                ropGetMessageStatusResponse.Parse(parser);
                                ropsList.Add(ropGetMessageStatusResponse);
                                break;

                            case RopIdType.RopSetReadFlags:
                                var ropSetReadFlagsResponse = new RopSetReadFlagsResponse();
                                ropSetReadFlagsResponse.Parse(parser);
                                ropsList.Add(ropSetReadFlagsResponse);
                                break;
                            case RopIdType.RopSetMessageReadFlag:
                                var ropSetMessageReadFlagResponse = new RopSetMessageReadFlagResponse();
                                ropSetMessageReadFlagResponse.Parse(parser);
                                ropsList.Add(ropSetMessageReadFlagResponse);
                                break;

                            case RopIdType.RopOpenAttachment:
                                var ropOpenAttachmentResponse = new RopOpenAttachmentResponse();
                                ropOpenAttachmentResponse.Parse(parser);
                                ropsList.Add(ropOpenAttachmentResponse);
                                break;

                            case RopIdType.RopCreateAttachment:
                                var ropCreateAttachmentResponse = new RopCreateAttachmentResponse();
                                ropCreateAttachmentResponse.Parse(parser);
                                ropsList.Add(ropCreateAttachmentResponse);
                                break;

                            case RopIdType.RopDeleteAttachment:
                                var ropDeleteAttachmentResponse = new RopDeleteAttachmentResponse();
                                ropDeleteAttachmentResponse.Parse(parser);
                                ropsList.Add(ropDeleteAttachmentResponse);
                                break;

                            case RopIdType.RopSaveChangesAttachment:
                                var ropSaveChangesAttachmentResponse = new RopSaveChangesAttachmentResponse();
                                ropSaveChangesAttachmentResponse.Parse(parser);
                                ropsList.Add(ropSaveChangesAttachmentResponse);
                                break;

                            case RopIdType.RopOpenEmbeddedMessage:
                                var ropOpenEmbeddedMessageResponse = new RopOpenEmbeddedMessageResponse();
                                ropOpenEmbeddedMessageResponse.Parse(parser);
                                ropsList.Add(ropOpenEmbeddedMessageResponse);
                                break;

                            case RopIdType.RopGetAttachmentTable:
                                var ropGetAttachmentTableResponse = new RopGetAttachmentTableResponse();
                                ropGetAttachmentTableResponse.Parse(parser);
                                ropsList.Add(ropGetAttachmentTableResponse);
                                break;

                            case RopIdType.RopGetValidAttachments:
                                var ropGetValidAttachmentsResponse = new RopGetValidAttachmentsResponse();
                                ropGetValidAttachmentsResponse.Parse(parser);
                                ropsList.Add(ropGetValidAttachmentsResponse);
                                break;

                            // MSOXCNOTIF ROPs
                            case RopIdType.RopRegisterNotification:
                                var ropRegisterNotificationResponse = new RopRegisterNotificationResponse();
                                ropRegisterNotificationResponse.Parse(parser);
                                ropsList.Add(ropRegisterNotificationResponse);
                                break;

                            case RopIdType.RopPending:
                                var ropPendingResponse = new RopPendingResponse();
                                ropPendingResponse.Parse(parser);
                                ropsList.Add(ropPendingResponse);
                                break;

                            case RopIdType.RopNotify:
                                var ropNotifyResponse = new RopNotifyResponse();
                                ropNotifyResponse.Parse(parser);
                                ropsList.Add(ropNotifyResponse);
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                var ropGetPermissionsTableResponse = new RopGetPermissionsTableResponse();
                                ropGetPermissionsTableResponse.Parse(parser);
                                ropsList.Add(ropGetPermissionsTableResponse);
                                break;

                            case RopIdType.RopModifyPermissions:
                                var ropModifyPermissionsResponse = new RopModifyPermissionsResponse();
                                ropModifyPermissionsResponse.Parse(parser);
                                ropsList.Add(ropModifyPermissionsResponse);
                                break;

                            default:
                                BlockBytes ropsBytes = ParseBytes(RopSize - parser.Offset);
                                ropsList.Add(ropsBytes);
                                break;
                        }
                    }
                    while (parser.Offset < RopSize);
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
                object[] roplist = RopsList;
                foreach (object obj in roplist)
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
            SetText("ROPOutputBuffer");
            AddChildBlockT(RopSize, "RopSize");
            AddLabeledChildren(RopsList, "RopsList");
            AddLabeledChildren(ServerObjectHandleTable, "ServerObjectHandleTable");
        }
    }
}