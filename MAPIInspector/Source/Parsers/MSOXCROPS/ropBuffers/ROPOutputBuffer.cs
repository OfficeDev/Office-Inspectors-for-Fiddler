using BlockParser;
using Fiddler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ROP output buffer, which is sent by the server, includes an array of ROP response buffers. 
    /// </summary>
    public class ROPOutputBuffer : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public ushort RopSize;

        /// <summary>
        /// An array of ROP response buffers.
        /// </summary>
        public object[] RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPOutputBuffer structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPOutputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            bool parseToCROPSResponseLayer = false;
            RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            List<uint> tempServerObjectHandleTable = new List<uint>();
            long currentPosition = s.Position;
            s.Position += RopSize - 2;
            int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
            }
            while (s.Position < s.Length)
            {
                uint serverObjectTable = ReadUint();

                if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                {
                    MapiInspector.MAPIParser.IsLooperCall = true;
                    Dictionary<ushort, Dictionary<int, uint>> item = new Dictionary<ushort, Dictionary<int, uint>>();
                    item = MapiInspector.MAPIParser.TargetHandle.Peek();

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

            s.Position = currentPosition;

            if (!MapiInspector.MAPIParser.IsLooperCall || parseToCROPSResponseLayer || MapiInspector.MAPIParser.NeedToParseCROPSLayer)
            {
                // empty intermediate variables for ROPs need context information 
                DecodingContext.SetColumn_InputHandles_InResponse = new List<uint>();

                if (RopSize > 2)
                {
                    do
                    {
                        int currentByte = s.ReadByte();
                        s.Position -= 1;

                        switch ((RopIdType)currentByte)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                long currentPos_logon = s.Position;
                                s.Position += 1;
                                int tempOutputHandleIndex_logon = s.ReadByte();
                                s.Position = currentPos_logon;
                                if (!(DecodingContext.SessionLogonFlagsInLogonRop.Count > 0 && DecodingContext.SessionLogonFlagsInLogonRop.ContainsKey(parsingSessionID) && DecodingContext.SessionLogonFlagsInLogonRop[parsingSessionID].ContainsKey((uint)tempOutputHandleIndex_logon)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopLogon", (ushort)currentByte);
                                }
                                else
                                {
                                    if (((byte)DecodingContext.SessionLogonFlagsInLogonRop[parsingSessionID][(uint)tempOutputHandleIndex_logon] & 0x01) == (byte)LogonFlags.Private)
                                    {
                                        RopLogonResponse_PrivateMailboxes ropLogonResponse_PrivateMailboxes = new RopLogonResponse_PrivateMailboxes();
                                        ropLogonResponse_PrivateMailboxes.Parse(s);
                                        ropsList.Add(ropLogonResponse_PrivateMailboxes);
                                        break;
                                    }
                                    else
                                    {
                                        RopLogonResponse_PublicFolders ropLogonResponse_PublicFolders = new RopLogonResponse_PublicFolders();
                                        ropLogonResponse_PublicFolders.Parse(s);
                                        ropsList.Add(ropLogonResponse_PublicFolders);
                                        break;
                                    }
                                }

                            case RopIdType.RopGetReceiveFolder:
                                RopGetReceiveFolderResponse ropGetReceiveFolderResponse = new RopGetReceiveFolderResponse();
                                ropGetReceiveFolderResponse.Parse(s);
                                ropsList.Add(ropGetReceiveFolderResponse);
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                RopSetReceiveFolderResponse ropSetReceiveFolderResponse = new RopSetReceiveFolderResponse();
                                ropSetReceiveFolderResponse.Parse(s);
                                ropsList.Add(ropSetReceiveFolderResponse);
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                RopGetReceiveFolderTableResponse ropGetReceiveFolderTableResponse = new RopGetReceiveFolderTableResponse();
                                ropGetReceiveFolderTableResponse.Parse(s);
                                ropsList.Add(ropGetReceiveFolderTableResponse);
                                break;
                            case RopIdType.RopGetStoreState:
                                RopGetStoreStateResponse ropGetStoreStateResponse = new RopGetStoreStateResponse();
                                ropGetStoreStateResponse.Parse(s);
                                ropsList.Add(ropGetStoreStateResponse);
                                break;
                            case RopIdType.RopGetOwningServers:
                                RopGetOwningServersResponse ropGetOwningServersResponse = new RopGetOwningServersResponse();
                                ropGetOwningServersResponse.Parse(s);
                                ropsList.Add(ropGetOwningServersResponse);
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                RopPublicFolderIsGhostedResponse ropPublicFolderIsGhostedResponse = new RopPublicFolderIsGhostedResponse();
                                ropPublicFolderIsGhostedResponse.Parse(s);
                                ropsList.Add(ropPublicFolderIsGhostedResponse);
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                RopLongTermIdFromIdResponse ropLongTermIdFromIdResponse = new RopLongTermIdFromIdResponse();
                                ropLongTermIdFromIdResponse.Parse(s);
                                ropsList.Add(ropLongTermIdFromIdResponse);
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                RopIdFromLongTermIdResponse ropIdFromLongTermIdResponse = new RopIdFromLongTermIdResponse();
                                ropIdFromLongTermIdResponse.Parse(s);
                                ropsList.Add(ropIdFromLongTermIdResponse);
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                RopGetPerUserLongTermIdsResponse ropGetPerUserLongTermIdsResponse = new RopGetPerUserLongTermIdsResponse();
                                ropGetPerUserLongTermIdsResponse.Parse(s);
                                ropsList.Add(ropGetPerUserLongTermIdsResponse);
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                RopGetPerUserGuidResponse ropGetPerUserGuidResponse = new RopGetPerUserGuidResponse();
                                ropGetPerUserGuidResponse.Parse(s);
                                ropsList.Add(ropGetPerUserGuidResponse);
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                RopReadPerUserInformationResponse ropReadPerUserInformationResponse = new RopReadPerUserInformationResponse();
                                ropReadPerUserInformationResponse.Parse(s);
                                ropsList.Add(ropReadPerUserInformationResponse);
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                RopWritePerUserInformationResponse ropWritePerUserInformationResponse = new RopWritePerUserInformationResponse();
                                ropWritePerUserInformationResponse.Parse(s);
                                ropsList.Add(ropWritePerUserInformationResponse);
                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                RopSubmitMessageResponse ropSubmitMessageResponse = new RopSubmitMessageResponse();
                                ropSubmitMessageResponse.Parse(s);
                                ropsList.Add(ropSubmitMessageResponse);
                                break;
                            case RopIdType.RopAbortSubmit:
                                RopAbortSubmitResponse ropAbortSubmitResponse = new RopAbortSubmitResponse();
                                ropAbortSubmitResponse.Parse(s);
                                ropsList.Add(ropAbortSubmitResponse);
                                break;
                            case RopIdType.RopGetAddressTypes:
                                RopGetAddressTypesResponse ropGetAddressTypesResponse = new RopGetAddressTypesResponse();
                                ropGetAddressTypesResponse.Parse(s);
                                ropsList.Add(ropGetAddressTypesResponse);
                                break;
                            case RopIdType.RopSetSpooler:
                                RopSetSpoolerResponse ropSetSpoolerResponse = new RopSetSpoolerResponse();
                                ropSetSpoolerResponse.Parse(s);
                                ropsList.Add(ropSetSpoolerResponse);
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                RopSpoolerLockMessageResponse ropSpoolerLockMessageResponse = new RopSpoolerLockMessageResponse();
                                ropSpoolerLockMessageResponse.Parse(s);
                                ropsList.Add(ropSpoolerLockMessageResponse);
                                break;
                            case RopIdType.RopTransportSend:
                                RopTransportSendResponse ropTransportSendResponse = new RopTransportSendResponse();
                                ropTransportSendResponse.Parse(s);
                                ropsList.Add(ropTransportSendResponse);
                                break;
                            case RopIdType.RopTransportNewMail:
                                RopTransportNewMailResponse ropTransportNewMailResponse = new RopTransportNewMailResponse();
                                ropTransportNewMailResponse.Parse(s);
                                ropsList.Add(ropTransportNewMailResponse);
                                break;
                            case RopIdType.RopGetTransportFolder:
                                RopGetTransportFolderResponse ropGetTransportFolderResponse = new RopGetTransportFolderResponse();
                                ropGetTransportFolderResponse.Parse(s);
                                ropsList.Add(ropGetTransportFolderResponse);
                                break;
                            case RopIdType.RopOptionsData:
                                RopOptionsDataResponse ropOptionsDataResponse = new RopOptionsDataResponse();
                                ropOptionsDataResponse.Parse(s);
                                ropsList.Add(ropOptionsDataResponse);
                                break;
                            case RopIdType.RopBackoff:
                                RopBackoffResponse ropBackoffResponse = new RopBackoffResponse();
                                ropBackoffResponse.Parse(s);
                                ropsList.Add(ropBackoffResponse);
                                break;
                            case RopIdType.RopBufferTooSmall:
                                if (DecodingContext.SessionRequestRemainSize.Count > 0 && DecodingContext.SessionRequestRemainSize.ContainsKey(parsingSessionID))
                                {
                                    uint requestBuffersSize = 0;
                                    int ropCountInResponse = ropsList.Count;
                                    if (DecodingContext.SessionRequestRemainSize[parsingSessionID].Count > ropCountInResponse)
                                    {
                                        requestBuffersSize = DecodingContext.SessionRequestRemainSize[parsingSessionID][ropCountInResponse];
                                    }

                                    RopBufferTooSmallResponse ropBufferTooSmallResponse = new RopBufferTooSmallResponse(requestBuffersSize);
                                    ropBufferTooSmallResponse.Parse(s);
                                    ropsList.Add(ropBufferTooSmallResponse);
                                    break;
                                }
                                else
                                {
                                    throw new MissingInformationException("Missing RequestBuffersSize information for RopBufferTooSmall", (ushort)currentByte);
                                }

                            // MSOXCTABL ROPs
                            case RopIdType.RopSetColumns:
                                RopSetColumnsResponse ropSetColumnsResponse = Block.Parse<RopSetColumnsResponse>(s);
                                ropsList.Add(ropSetColumnsResponse);

                                if (!(DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 && DecodingContext.SetColumn_InputHandles_InResponse.Contains(tempServerObjectHandleTable[ropSetColumnsResponse.InputHandleIndex])))
                                {
                                    DecodingContext.SetColumn_InputHandles_InResponse.Add(tempServerObjectHandleTable[ropSetColumnsResponse.InputHandleIndex]);
                                }

                                break;

                            case RopIdType.RopSortTable:
                                RopSortTableResponse ropSortTableResponse = new RopSortTableResponse();
                                ropSortTableResponse.Parse(s);
                                ropsList.Add(ropSortTableResponse);
                                break;

                            case RopIdType.RopRestrict:
                                RopRestrictResponse ropRestrictResponse = new RopRestrictResponse();
                                ropRestrictResponse.Parse(s);
                                ropsList.Add(ropRestrictResponse);
                                break;

                            case RopIdType.RopQueryRows:
                                long currentPos = s.Position;
                                s.Position += 1;
                                int tempInputHandleIndex_QueryRow = s.ReadByte();
                                uint returnValue_queryRow = ReadUint();
                                s.Position = currentPos;
                                string serverPath_QueryRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                                string processName_QueryROw = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                                string clientInfo_QueryROw = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                                uint objHandle_QueryROw = tempServerObjectHandleTable[tempInputHandleIndex_QueryRow];
                                if (returnValue_queryRow == 0)
                                {
                                    if (!(DecodingContext.RowRops_handlePropertyTags.ContainsKey(objHandle_QueryROw) && DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw].ContainsKey(parsingSessionID) && DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item1 == serverPath_QueryRow
                                     && DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item2 == processName_QueryROw && DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item3 == clientInfo_QueryROw))
                                    {
                                        throw new MissingInformationException("Missing PropertyTags information for RopQueryRowsResponse", (ushort)RopIdType.RopQueryRows, new uint[] { (uint)tempInputHandleIndex_QueryRow, tempServerObjectHandleTable[tempInputHandleIndex_QueryRow] });
                                    }

                                    RopQueryRowsResponse ropQueryRowsResponse = new RopQueryRowsResponse(DecodingContext.RowRops_handlePropertyTags[objHandle_QueryROw][parsingSessionID].Item4);
                                    ropQueryRowsResponse.Parse(s);
                                    ropsList.Add(ropQueryRowsResponse);
                                    break;
                                }
                                else
                                {
                                    RopQueryRowsResponse ropQueryRowsResponse = new RopQueryRowsResponse(null);
                                    ropQueryRowsResponse.Parse(s);
                                    ropsList.Add(ropQueryRowsResponse);
                                    break;
                                }

                            case RopIdType.RopAbort:
                                RopAbortResponse ropAbortResponse = new RopAbortResponse();
                                ropAbortResponse.Parse(s);
                                ropsList.Add(ropAbortResponse);
                                break;

                            case RopIdType.RopGetStatus:
                                RopGetStatusResponse ropGetStatusResponse = new RopGetStatusResponse();
                                ropGetStatusResponse.Parse(s);
                                ropsList.Add(ropGetStatusResponse);
                                break;

                            case RopIdType.RopQueryPosition:
                                RopQueryPositionResponse ropQueryPositionResponse = new RopQueryPositionResponse();
                                ropQueryPositionResponse.Parse(s);
                                ropsList.Add(ropQueryPositionResponse);
                                break;

                            case RopIdType.RopSeekRow:
                                ropsList.Add(Block.Parse<RopSeekRowResponse>(s));
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                RopSeekRowBookmarkResponse ropSeekRowBookmarkResponse = new RopSeekRowBookmarkResponse();
                                ropSeekRowBookmarkResponse.Parse(s);
                                ropsList.Add(ropSeekRowBookmarkResponse);
                                break;

                            case RopIdType.RopSeekRowFractional:
                                RopSeekRowFractionalResponse ropSeekRowFractionalResponse = new RopSeekRowFractionalResponse();
                                ropSeekRowFractionalResponse.Parse(s);
                                ropsList.Add(ropSeekRowFractionalResponse);
                                break;

                            case RopIdType.RopCreateBookmark:
                                RopCreateBookmarkResponse ropCreateBookmarkResponse = new RopCreateBookmarkResponse();
                                ropCreateBookmarkResponse.Parse(s);
                                ropsList.Add(ropCreateBookmarkResponse);
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                RopQueryColumnsAllResponse ropQueryColumnsAllResponse = new RopQueryColumnsAllResponse();
                                ropQueryColumnsAllResponse.Parse(s);
                                ropsList.Add(ropQueryColumnsAllResponse);
                                break;

                            case RopIdType.RopFindRow:
                                long currentPos_findRow = s.Position;
                                s.Position += 1;
                                int tempInputHandleIndex_findRow = s.ReadByte();
                                uint returnValue_findRow = ReadUint();
                                s.Position = currentPos_findRow;
                                uint objHandle_FindRow = tempServerObjectHandleTable[tempInputHandleIndex_findRow];
                                string serverPath_FindRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                                string processName_FindRow = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                                string clientInfo_FindRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                                if (returnValue_findRow == 0)
                                {
                                    if (!(DecodingContext.RowRops_handlePropertyTags.ContainsKey(objHandle_FindRow) && DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow].ContainsKey(parsingSessionID) && DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item1 == serverPath_FindRow
                                     && DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item2 == processName_FindRow && DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item3 == clientInfo_FindRow))
                                    {
                                        throw new MissingInformationException("Missing PropertyTags information for RopFindRowsResponse", (ushort)RopIdType.RopFindRow, new uint[] { (uint)tempInputHandleIndex_findRow, objHandle_FindRow });
                                    }

                                    RopFindRowResponse ropFindRowResponse = new RopFindRowResponse(DecodingContext.RowRops_handlePropertyTags[objHandle_FindRow][parsingSessionID].Item4);
                                    ropFindRowResponse.Parse(s);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }
                                else
                                {
                                    RopFindRowResponse ropFindRowResponse = new RopFindRowResponse(null);
                                    ropFindRowResponse.Parse(s);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }

                            case RopIdType.RopFreeBookmark:
                                RopFreeBookmarkResponse ropFreeBookmarkResponse = new RopFreeBookmarkResponse();
                                ropFreeBookmarkResponse.Parse(s);
                                ropsList.Add(ropFreeBookmarkResponse);
                                break;

                            case RopIdType.RopResetTable:
                                RopResetTableResponse ropResetTableResponse = new RopResetTableResponse();
                                ropResetTableResponse.Parse(s);
                                ropsList.Add(ropResetTableResponse);
                                break;

                            case RopIdType.RopExpandRow:
                                long currentPos_expandRow = s.Position;
                                s.Position += 1;
                                int tempInputHandleIndex_expandRow = s.ReadByte();
                                uint returnValue_expandRow = ReadUint();
                                s.Position = currentPos_expandRow;
                                uint objHandle_ExpandRow = tempServerObjectHandleTable[tempInputHandleIndex_expandRow];
                                string serverPath_ExpandRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                                string processName_ExpandRow = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                                string clientInfo_ExpandRow = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                                if (returnValue_expandRow == 0)
                                {
                                    if (!(DecodingContext.RowRops_handlePropertyTags.ContainsKey(objHandle_ExpandRow) && DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow].ContainsKey(parsingSessionID) && DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item1 == serverPath_ExpandRow
                                    && DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item2 == processName_ExpandRow && DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item3 == clientInfo_ExpandRow))
                                    {
                                        throw new MissingInformationException("Missing PropertyTags information for RopExpandRowsResponse", (ushort)RopIdType.RopExpandRow, new uint[] { (uint)tempInputHandleIndex_expandRow, objHandle_ExpandRow });
                                    }

                                    RopExpandRowResponse ropFindRowResponse = new RopExpandRowResponse(DecodingContext.RowRops_handlePropertyTags[objHandle_ExpandRow][parsingSessionID].Item4);
                                    ropFindRowResponse.Parse(s);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }
                                else
                                {
                                    RopExpandRowResponse ropFindRowResponse = new RopExpandRowResponse(null);
                                    ropFindRowResponse.Parse(s);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }

                            case RopIdType.RopCollapseRow:
                                RopCollapseRowResponse ropCollapseRowResponse = new RopCollapseRowResponse();
                                ropCollapseRowResponse.Parse(s);
                                ropsList.Add(ropCollapseRowResponse);
                                break;

                            case RopIdType.RopGetCollapseState:
                                RopGetCollapseStateResponse ropGetCollapseStateResponse = new RopGetCollapseStateResponse();
                                ropGetCollapseStateResponse.Parse(s);
                                ropsList.Add(ropGetCollapseStateResponse);
                                break;

                            case RopIdType.RopSetCollapseState:
                                RopSetCollapseStateResponse ropSetCollapseStateResponse = new RopSetCollapseStateResponse();
                                ropSetCollapseStateResponse.Parse(s);
                                ropsList.Add(ropSetCollapseStateResponse);
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                RopModifyRulesResponse ropModifyRulesResponse = new RopModifyRulesResponse();
                                ropModifyRulesResponse.Parse(s);
                                ropsList.Add(ropModifyRulesResponse);
                                break;

                            case RopIdType.RopGetRulesTable:
                                RopGetRulesTableResponse ropGetRulesTableResponse = new RopGetRulesTableResponse();
                                ropGetRulesTableResponse.Parse(s);
                                ropsList.Add(ropGetRulesTableResponse);
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                RopUpdateDeferredActionMessagesResponse ropUpdateDeferredActionMessagesResponse = new RopUpdateDeferredActionMessagesResponse();
                                ropUpdateDeferredActionMessagesResponse.Parse(s);
                                ropsList.Add(ropUpdateDeferredActionMessagesResponse);
                                break;

                            // MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                RopFastTransferSourceCopyPropertiesResponse ropFastTransferSourceCopyPropertiesResponse = new RopFastTransferSourceCopyPropertiesResponse();
                                ropFastTransferSourceCopyPropertiesResponse.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyPropertiesResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                RopFastTransferSourceCopyToResponse ropFastTransferSourceCopyToResponse = new RopFastTransferSourceCopyToResponse();
                                ropFastTransferSourceCopyToResponse.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyToResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                RopFastTransferSourceCopyMessagesResponse ropFastTransferSourceCopyMessagesResponse = new RopFastTransferSourceCopyMessagesResponse();
                                ropFastTransferSourceCopyMessagesResponse.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyMessagesResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                RopFastTransferSourceCopyFolderResponse ropFastTransferSourceCopyFolderResponse = new RopFastTransferSourceCopyFolderResponse();
                                ropFastTransferSourceCopyFolderResponse.Parse(s);
                                ropsList.Add(ropFastTransferSourceCopyFolderResponse);
                                break;
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                long currentPos_getBuffer = s.Position;
                                s.Position += 1;
                                int tempInputHandleIndex_getBuffer = s.ReadByte();
                                uint returnValue = ReadUint();
                                ushort status = ReadUshort();
                                s.Position = currentPos_getBuffer;
                                int getParsingSessionID = parsingSessionID;
                                Session getParsingSession = MapiInspector.MAPIParser.ParsingSession;
                                uint ropGetbufferHandle = tempServerObjectHandleTable[tempInputHandleIndex_getBuffer];
                                var partialBeforeAndAfterInformation = new PartialContextInformation[2];
                                if (returnValue == 0)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey((int)getParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException((RopIdType)currentByte, ropGetbufferHandle);
                                    }
                                }

                                RopFastTransferSourceGetBufferResponse ropFastTransferSourceGetBufferResponse = new RopFastTransferSourceGetBufferResponse();
                                Partial.IsGet = true;
                                ropFastTransferSourceGetBufferResponse.Parse(s);
                                ropsList.Add(ropFastTransferSourceGetBufferResponse);
                                var getBufferPartialInformaiton = new PartialContextInformation(
                                    Partial.PartialGetType,
                                    Partial.PartialGetId,
                                    Partial.PartialGetRemainSize,
                                    Partial.PartialGetSubRemainSize,
                                    true,
                                    getParsingSession,
                                    MapiInspector.MAPIParser.OutputPayLoadCompressedXOR);
                                SortedDictionary<int, PartialContextInformation> sessionGetContextInfor = new SortedDictionary<int, PartialContextInformation>();

                                if (Partial.HandleWithSessionGetContextInformation.ContainsKey(ropGetbufferHandle))
                                {
                                    sessionGetContextInfor = Partial.HandleWithSessionGetContextInformation[ropGetbufferHandle];
                                    Partial.HandleWithSessionGetContextInformation.Remove(ropGetbufferHandle);
                                }

                                if (sessionGetContextInfor.ContainsKey(getParsingSessionID))
                                {
                                    sessionGetContextInfor[getParsingSessionID] = getBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionGetContextInfor.Add(getParsingSessionID, getBufferPartialInformaiton);
                                }

                                Partial.HandleWithSessionGetContextInformation.Add(ropGetbufferHandle, sessionGetContextInfor);
                                Partial.IsGet = false;
                                break;

                            case RopIdType.RopTellVersion:
                                RopTellVersionResponse ropTellVersionResponse = new RopTellVersionResponse();
                                ropTellVersionResponse.Parse(s);
                                ropsList.Add(ropTellVersionResponse);
                                break;
                            case RopIdType.RopSynchronizationGetTransferState:
                                RopSynchronizationGetTransferStateResponse ropSynchronizationGetTransferStateResponse = new RopSynchronizationGetTransferStateResponse();
                                ropSynchronizationGetTransferStateResponse.Parse(s);
                                ropsList.Add(ropSynchronizationGetTransferStateResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                RopFastTransferDestinationConfigureResponse ropFastTransferDestinationConfigureResponse = new RopFastTransferDestinationConfigureResponse();
                                ropFastTransferDestinationConfigureResponse.Parse(s);
                                ropsList.Add(ropFastTransferDestinationConfigureResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                                RopFastTransferDestinationPutBufferResponse ropFastTransferDestinationPutBufferResponse = new RopFastTransferDestinationPutBufferResponse();
                                ropFastTransferDestinationPutBufferResponse.Parse(s);
                                ropsList.Add(ropFastTransferDestinationPutBufferResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBufferExtended:
                                RopFastTransferDestinationPutBufferExtendedResponse ropFastTransferDestinationPutBufferExtendedResponse = new RopFastTransferDestinationPutBufferExtendedResponse();
                                ropFastTransferDestinationPutBufferExtendedResponse.Parse(s);
                                ropsList.Add(ropFastTransferDestinationPutBufferExtendedResponse);
                                break;
                            case RopIdType.RopSynchronizationConfigure:
                                RopSynchronizationConfigureResponse ropSynchronizationConfigureResponse = new RopSynchronizationConfigureResponse();
                                ropSynchronizationConfigureResponse.Parse(s);
                                ropsList.Add(ropSynchronizationConfigureResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                RopSynchronizationUploadStateStreamBeginResponse ropSynchronizationUploadStateStreamBeginResponse = new RopSynchronizationUploadStateStreamBeginResponse();
                                ropSynchronizationUploadStateStreamBeginResponse.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamBeginResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                RopSynchronizationUploadStateStreamContinueResponse ropSynchronizationUploadStateStreamContinueResponse = new RopSynchronizationUploadStateStreamContinueResponse();
                                ropSynchronizationUploadStateStreamContinueResponse.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamContinueResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                RopSynchronizationUploadStateStreamEndResponse ropSynchronizationUploadStateStreamEndResponse = new RopSynchronizationUploadStateStreamEndResponse();
                                ropSynchronizationUploadStateStreamEndResponse.Parse(s);
                                ropsList.Add(ropSynchronizationUploadStateStreamEndResponse);
                                break;
                            case RopIdType.RopSynchronizationOpenCollector:
                                RopSynchronizationOpenCollectorResponse ropSynchronizationOpenCollectorResponse = new RopSynchronizationOpenCollectorResponse();
                                ropSynchronizationOpenCollectorResponse.Parse(s);
                                ropsList.Add(ropSynchronizationOpenCollectorResponse);
                                break;
                            case RopIdType.RopSynchronizationImportMessageChange:
                                RopSynchronizationImportMessageChangeResponse ropSynchronizationImportMessageChangeResponse = new RopSynchronizationImportMessageChangeResponse();
                                ropSynchronizationImportMessageChangeResponse.Parse(s);
                                ropsList.Add(ropSynchronizationImportMessageChangeResponse);
                                break;
                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                RopSynchronizationImportHierarchyChangeResponse ropSynchronizationImportHierarchyChangeResponse = new RopSynchronizationImportHierarchyChangeResponse();
                                ropSynchronizationImportHierarchyChangeResponse.Parse(s);
                                ropsList.Add(ropSynchronizationImportHierarchyChangeResponse);
                                break;
                            case RopIdType.RopSynchronizationImportMessageMove:
                                RopSynchronizationImportMessageMoveResponse ropSynchronizationImportMessageMoveResponse = new RopSynchronizationImportMessageMoveResponse();
                                ropSynchronizationImportMessageMoveResponse.Parse(s);
                                ropsList.Add(ropSynchronizationImportMessageMoveResponse);
                                break;
                            case RopIdType.RopSynchronizationImportDeletes:
                                RopSynchronizationImportDeletesResponse ropSynchronizationImportDeletesResponse = new RopSynchronizationImportDeletesResponse();
                                ropSynchronizationImportDeletesResponse.Parse(s);
                                ropsList.Add(ropSynchronizationImportDeletesResponse);
                                break;
                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                RopSynchronizationImportReadStateChangesResponse ropSynchronizationImportReadStateChangesResponse = new RopSynchronizationImportReadStateChangesResponse();
                                ropSynchronizationImportReadStateChangesResponse.Parse(s);
                                ropsList.Add(ropSynchronizationImportReadStateChangesResponse);
                                break;
                            case RopIdType.RopGetLocalReplicaIds:
                                RopGetLocalReplicaIdsResponse ropGetLocalReplicaIdsResponse = new RopGetLocalReplicaIdsResponse();
                                ropGetLocalReplicaIdsResponse.Parse(s);
                                ropsList.Add(ropGetLocalReplicaIdsResponse);
                                break;
                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                RopSetLocalReplicaMidsetDeletedResponse ropSetLocalReplicaMidsetDeletedResponse = new RopSetLocalReplicaMidsetDeletedResponse();
                                ropSetLocalReplicaMidsetDeletedResponse.Parse(s);
                                ropsList.Add(ropSetLocalReplicaMidsetDeletedResponse);
                                break;

                            // MS-OXCPRPT ROPs
                            case RopIdType.RopGetPropertiesSpecific:
                                long currentPos_getPropertiesSpec = s.Position;
                                s.Position += 1;
                                int tempInputHandleIndex_getPropertiesSpec = s.ReadByte();
                                s.Position = currentPos_getPropertiesSpec;
                                if (!(DecodingContext.GetPropertiesSpec_propertyTags.Count > 0 && DecodingContext.GetPropertiesSpec_propertyTags.ContainsKey(parsingSessionID) && DecodingContext.GetPropertiesSpec_propertyTags[parsingSessionID].ContainsKey((uint)tempInputHandleIndex_getPropertiesSpec)
                                    && DecodingContext.GetPropertiesSpec_propertyTags[parsingSessionID][(uint)tempInputHandleIndex_getPropertiesSpec].Count != 0))
                                {
                                    throw new MissingInformationException("Missing PropertyTags information for RopGetPropertiesSpecific", (ushort)currentByte);
                                }

                                RopGetPropertiesSpecificResponse ropGetPropertiesSpecificResponse = new RopGetPropertiesSpecificResponse();
                                ropGetPropertiesSpecificResponse.Parse(s);
                                ropsList.Add(ropGetPropertiesSpecificResponse);
                                break;

                            case RopIdType.RopGetPropertiesAll:
                                RopGetPropertiesAllResponse ropGetPropertiesAllResponse = new RopGetPropertiesAllResponse();
                                ropGetPropertiesAllResponse.Parse(s);
                                ropsList.Add(ropGetPropertiesAllResponse);
                                break;
                            case RopIdType.RopGetPropertiesList:
                                RopGetPropertiesListResponse ropGetPropertiesListResponse = new RopGetPropertiesListResponse();
                                ropGetPropertiesListResponse.Parse(s);
                                ropsList.Add(ropGetPropertiesListResponse);
                                break;
                            case RopIdType.RopSetProperties:
                                RopSetPropertiesResponse ropSetPropertiesResponse = new RopSetPropertiesResponse();
                                ropSetPropertiesResponse.Parse(s);
                                ropsList.Add(ropSetPropertiesResponse);
                                break;
                            case RopIdType.RopSetPropertiesNoReplicate:
                                RopSetPropertiesNoReplicateResponse ropSetPropertiesNoReplicateResponse = new RopSetPropertiesNoReplicateResponse();
                                ropSetPropertiesNoReplicateResponse.Parse(s);
                                ropsList.Add(ropSetPropertiesNoReplicateResponse);
                                break;
                            case RopIdType.RopDeleteProperties:
                                RopDeletePropertiesResponse ropDeletePropertiesResponse = new RopDeletePropertiesResponse();
                                ropDeletePropertiesResponse.Parse(s);
                                ropsList.Add(ropDeletePropertiesResponse);
                                break;
                            case RopIdType.RopDeletePropertiesNoReplicate:
                                RopDeletePropertiesNoReplicateResponse ropDeletePropertiesNoReplicateResponse = new RopDeletePropertiesNoReplicateResponse();
                                ropDeletePropertiesNoReplicateResponse.Parse(s);
                                ropsList.Add(ropDeletePropertiesNoReplicateResponse);
                                break;
                            case RopIdType.RopQueryNamedProperties:
                                RopQueryNamedPropertiesResponse ropQueryNamedPropertiesResponse = new RopQueryNamedPropertiesResponse();
                                ropQueryNamedPropertiesResponse.Parse(s);
                                ropsList.Add(ropQueryNamedPropertiesResponse);
                                break;
                            case RopIdType.RopCopyProperties:
                                RopCopyPropertiesResponse ropCopyPropertiesResponse = new RopCopyPropertiesResponse();
                                ropCopyPropertiesResponse.Parse(s);
                                ropsList.Add(ropCopyPropertiesResponse);
                                break;
                            case RopIdType.RopCopyTo:
                                RopCopyToResponse ropCopyToResponse = new RopCopyToResponse();
                                ropCopyToResponse.Parse(s);
                                ropsList.Add(ropCopyToResponse);
                                break;
                            case RopIdType.RopGetPropertyIdsFromNames:
                                RopGetPropertyIdsFromNamesResponse ropGetPropertyIdsFromNamesResponse = new RopGetPropertyIdsFromNamesResponse();
                                ropGetPropertyIdsFromNamesResponse.Parse(s);
                                ropsList.Add(ropGetPropertyIdsFromNamesResponse);
                                break;
                            case RopIdType.RopGetNamesFromPropertyIds:
                                RopGetNamesFromPropertyIdsResponse ropGetNamesFromPropertyIdsResponse = new RopGetNamesFromPropertyIdsResponse();
                                ropGetNamesFromPropertyIdsResponse.Parse(s);
                                ropsList.Add(ropGetNamesFromPropertyIdsResponse);
                                break;
                            case RopIdType.RopOpenStream:
                                RopOpenStreamResponse ropOpenStreamResponse = new RopOpenStreamResponse();
                                ropOpenStreamResponse.Parse(s);
                                ropsList.Add(ropOpenStreamResponse);
                                break;
                            case RopIdType.RopReadStream:
                                RopReadStreamResponse ropReadStreamResponse = new RopReadStreamResponse();
                                ropReadStreamResponse.Parse(s);
                                ropsList.Add(ropReadStreamResponse);
                                break;
                            case RopIdType.RopWriteStream:
                                RopWriteStreamResponse ropWriteStreamResponse = new RopWriteStreamResponse();
                                ropWriteStreamResponse.Parse(s);
                                ropsList.Add(ropWriteStreamResponse);
                                break;
                            case RopIdType.RopWriteStreamExtended:
                                RopWriteStreamExtendedResponse ropWriteStreamExtendedResponse = new RopWriteStreamExtendedResponse();
                                ropWriteStreamExtendedResponse.Parse(s);
                                ropsList.Add(ropWriteStreamExtendedResponse);
                                break;
                            case RopIdType.RopCommitStream:
                                RopCommitStreamResponse ropCommitStreamResponse = new RopCommitStreamResponse();
                                ropCommitStreamResponse.Parse(s);
                                ropsList.Add(ropCommitStreamResponse);
                                break;
                            case RopIdType.RopGetStreamSize:
                                RopGetStreamSizeResponse ropGetStreamSizeResponse = new RopGetStreamSizeResponse();
                                ropGetStreamSizeResponse.Parse(s);
                                ropsList.Add(ropGetStreamSizeResponse);
                                break;
                            case RopIdType.RopSetStreamSize:
                                RopSetStreamSizeResponse ropSetStreamSizeResponse = new RopSetStreamSizeResponse();
                                ropSetStreamSizeResponse.Parse(s);
                                ropsList.Add(ropSetStreamSizeResponse);
                                break;
                            case RopIdType.RopSeekStream:
                                RopSeekStreamResponse ropSeekStreamResponse = new RopSeekStreamResponse();
                                ropSeekStreamResponse.Parse(s);
                                ropsList.Add(ropSeekStreamResponse);
                                break;
                            case RopIdType.RopCopyToStream:
                                RopCopyToStreamResponse ropCopyToStreamResponse = new RopCopyToStreamResponse();
                                ropCopyToStreamResponse.Parse(s);
                                ropsList.Add(ropCopyToStreamResponse);
                                break;
                            case RopIdType.RopProgress:
                                RopProgressResponse ropProgressResponse = new RopProgressResponse();
                                ropProgressResponse.Parse(s);
                                ropsList.Add(ropProgressResponse);
                                break;
                            case RopIdType.RopLockRegionStream:
                                RopLockRegionStreamResponse ropLockRegionStreamResponse = new RopLockRegionStreamResponse();
                                ropLockRegionStreamResponse.Parse(s);
                                ropsList.Add(ropLockRegionStreamResponse);
                                break;
                            case RopIdType.RopUnlockRegionStream:
                                RopUnlockRegionStreamResponse ropUnlockRegionStreamResponse = new RopUnlockRegionStreamResponse();
                                ropUnlockRegionStreamResponse.Parse(s);
                                ropsList.Add(ropUnlockRegionStreamResponse);
                                break;
                            case RopIdType.RopWriteAndCommitStream:
                                RopWriteAndCommitStreamResponse ropWriteAndCommitStreamResponse = new RopWriteAndCommitStreamResponse();
                                ropWriteAndCommitStreamResponse.Parse(s);
                                ropsList.Add(ropWriteAndCommitStreamResponse);
                                break;
                            case RopIdType.RopCloneStream:
                                RopCloneStreamResponse ropCloneStreamResponse = new RopCloneStreamResponse();
                                ropCloneStreamResponse.Parse(s);
                                ropsList.Add(ropCloneStreamResponse);
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                RopOpenFolderResponse ropOpenFolderResponse = new RopOpenFolderResponse();
                                ropOpenFolderResponse.Parse(s);
                                ropsList.Add(ropOpenFolderResponse);
                                break;

                            case RopIdType.RopCreateFolder:
                                RopCreateFolderResponse ropCreateFolderResponse = new RopCreateFolderResponse();
                                ropCreateFolderResponse.Parse(s);
                                ropsList.Add(ropCreateFolderResponse);
                                break;

                            case RopIdType.RopDeleteFolder:
                                RopDeleteFolderResponse ropDeleteFolderResponse = new RopDeleteFolderResponse();
                                ropDeleteFolderResponse.Parse(s);
                                ropsList.Add(ropDeleteFolderResponse);
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                RopSetSearchCriteriaResponse ropSetSearchCriteriaResponse = new RopSetSearchCriteriaResponse();
                                ropSetSearchCriteriaResponse.Parse(s);
                                ropsList.Add(ropSetSearchCriteriaResponse);
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                RopGetSearchCriteriaResponse ropGetSearchCriteriaResponse = new RopGetSearchCriteriaResponse();
                                ropGetSearchCriteriaResponse.Parse(s);
                                ropsList.Add(ropGetSearchCriteriaResponse);
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                RopMoveCopyMessagesResponse ropMoveCopyMessagesResponse = new RopMoveCopyMessagesResponse();
                                ropMoveCopyMessagesResponse.Parse(s);
                                ropsList.Add(ropMoveCopyMessagesResponse);
                                break;

                            case RopIdType.RopMoveFolder:
                                RopMoveFolderResponse ropMoveFolderResponse = new RopMoveFolderResponse();
                                ropMoveFolderResponse.Parse(s);
                                ropsList.Add(ropMoveFolderResponse);
                                break;

                            case RopIdType.RopCopyFolder:
                                RopCopyFolderResponse ropCopyFolderResponse = new RopCopyFolderResponse();
                                ropCopyFolderResponse.Parse(s);
                                ropsList.Add(ropCopyFolderResponse);
                                break;

                            case RopIdType.RopEmptyFolder:
                                RopEmptyFolderResponse ropEmptyFolderResponse = new RopEmptyFolderResponse();
                                ropEmptyFolderResponse.Parse(s);
                                ropsList.Add(ropEmptyFolderResponse);
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                RopHardDeleteMessagesAndSubfoldersResponse ropHardDeleteMessagesAndSubfoldersResponse = new RopHardDeleteMessagesAndSubfoldersResponse();
                                ropHardDeleteMessagesAndSubfoldersResponse.Parse(s);
                                ropsList.Add(ropHardDeleteMessagesAndSubfoldersResponse);
                                break;

                            case RopIdType.RopDeleteMessages:
                                RopDeleteMessagesResponse ropDeleteMessagesResponse = new RopDeleteMessagesResponse();
                                ropDeleteMessagesResponse.Parse(s);
                                ropsList.Add(ropDeleteMessagesResponse);
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                RopHardDeleteMessagesResponse ropHardDeleteMessagesResponse = new RopHardDeleteMessagesResponse();
                                ropHardDeleteMessagesResponse.Parse(s);
                                ropsList.Add(ropHardDeleteMessagesResponse);
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                RopGetHierarchyTableResponse ropGetHierarchyTableResponse = new RopGetHierarchyTableResponse();
                                ropGetHierarchyTableResponse.Parse(s);
                                ropsList.Add(ropGetHierarchyTableResponse);
                                break;

                            case RopIdType.RopGetContentsTable:
                                RopGetContentsTableResponse ropGetContentsTableResponse = new RopGetContentsTableResponse();
                                ropGetContentsTableResponse.Parse(s);
                                ropsList.Add(ropGetContentsTableResponse);
                                break;

                            // MS-OXCMSG ROPs
                            case RopIdType.RopOpenMessage:
                                RopOpenMessageResponse ropOpenMessageResponse = new RopOpenMessageResponse();
                                ropOpenMessageResponse.Parse(s);
                                ropsList.Add(ropOpenMessageResponse);
                                break;

                            case RopIdType.RopCreateMessage:
                                RopCreateMessageResponse ropCreateMessageResponse = new RopCreateMessageResponse();
                                ropCreateMessageResponse.Parse(s);
                                ropsList.Add(ropCreateMessageResponse);
                                break;

                            case RopIdType.RopSaveChangesMessage:
                                RopSaveChangesMessageResponse ropSaveChangesMessageResponse = new RopSaveChangesMessageResponse();
                                ropSaveChangesMessageResponse.Parse(s);
                                ropsList.Add(ropSaveChangesMessageResponse);
                                break;

                            case RopIdType.RopRemoveAllRecipients:
                                RopRemoveAllRecipientsResponse ropRemoveAllRecipientsResponse = new RopRemoveAllRecipientsResponse();
                                ropRemoveAllRecipientsResponse.Parse(s);
                                ropsList.Add(ropRemoveAllRecipientsResponse);
                                break;

                            case RopIdType.RopModifyRecipients:
                                RopModifyRecipientsResponse ropModifyRecipientsResponse = new RopModifyRecipientsResponse();
                                ropModifyRecipientsResponse.Parse(s);
                                ropsList.Add(ropModifyRecipientsResponse);
                                break;

                            case RopIdType.RopReadRecipients:
                                RopReadRecipientsResponse ropReadRecipientsResponse = new RopReadRecipientsResponse();
                                ropReadRecipientsResponse.Parse(s);
                                ropsList.Add(ropReadRecipientsResponse);
                                break;

                            case RopIdType.RopReloadCachedInformation:
                                RopReloadCachedInformationResponse ropReloadCachedInformationResponse = new RopReloadCachedInformationResponse();
                                ropReloadCachedInformationResponse.Parse(s);
                                ropsList.Add(ropReloadCachedInformationResponse);
                                break;
                            case RopIdType.RopSetMessageStatus:
                                RopSetMessageStatusResponse ropSetMessageStatusResponse = new RopSetMessageStatusResponse();
                                ropSetMessageStatusResponse.Parse(s);
                                ropsList.Add(ropSetMessageStatusResponse);
                                break;

                            case RopIdType.RopGetMessageStatus:
                                RopGetMessageStatusResponse ropGetMessageStatusResponse = new RopGetMessageStatusResponse();
                                ropGetMessageStatusResponse.Parse(s);
                                ropsList.Add(ropGetMessageStatusResponse);
                                break;

                            case RopIdType.RopSetReadFlags:
                                RopSetReadFlagsResponse ropSetReadFlagsResponse = new RopSetReadFlagsResponse();
                                ropSetReadFlagsResponse.Parse(s);
                                ropsList.Add(ropSetReadFlagsResponse);
                                break;
                            case RopIdType.RopSetMessageReadFlag:
                                RopSetMessageReadFlagResponse ropSetMessageReadFlagResponse = new RopSetMessageReadFlagResponse();
                                ropSetMessageReadFlagResponse.Parse(s);
                                ropsList.Add(ropSetMessageReadFlagResponse);
                                break;

                            case RopIdType.RopOpenAttachment:
                                RopOpenAttachmentResponse ropOpenAttachmentResponse = new RopOpenAttachmentResponse();
                                ropOpenAttachmentResponse.Parse(s);
                                ropsList.Add(ropOpenAttachmentResponse);
                                break;

                            case RopIdType.RopCreateAttachment:
                                RopCreateAttachmentResponse ropCreateAttachmentResponse = new RopCreateAttachmentResponse();
                                ropCreateAttachmentResponse.Parse(s);
                                ropsList.Add(ropCreateAttachmentResponse);
                                break;

                            case RopIdType.RopDeleteAttachment:
                                RopDeleteAttachmentResponse ropDeleteAttachmentResponse = new RopDeleteAttachmentResponse();
                                ropDeleteAttachmentResponse.Parse(s);
                                ropsList.Add(ropDeleteAttachmentResponse);
                                break;

                            case RopIdType.RopSaveChangesAttachment:
                                RopSaveChangesAttachmentResponse ropSaveChangesAttachmentResponse = new RopSaveChangesAttachmentResponse();
                                ropSaveChangesAttachmentResponse.Parse(s);
                                ropsList.Add(ropSaveChangesAttachmentResponse);
                                break;

                            case RopIdType.RopOpenEmbeddedMessage:
                                RopOpenEmbeddedMessageResponse ropOpenEmbeddedMessageResponse = new RopOpenEmbeddedMessageResponse();
                                ropOpenEmbeddedMessageResponse.Parse(s);
                                ropsList.Add(ropOpenEmbeddedMessageResponse);
                                break;

                            case RopIdType.RopGetAttachmentTable:
                                RopGetAttachmentTableResponse ropGetAttachmentTableResponse = new RopGetAttachmentTableResponse();
                                ropGetAttachmentTableResponse.Parse(s);
                                ropsList.Add(ropGetAttachmentTableResponse);
                                break;

                            case RopIdType.RopGetValidAttachments:
                                RopGetValidAttachmentsResponse ropGetValidAttachmentsResponse = new RopGetValidAttachmentsResponse();
                                ropGetValidAttachmentsResponse.Parse(s);
                                ropsList.Add(ropGetValidAttachmentsResponse);
                                break;

                            // MSOXCNOTIF ROPs
                            case RopIdType.RopRegisterNotification:
                                RopRegisterNotificationResponse ropRegisterNotificationResponse = new RopRegisterNotificationResponse();
                                ropRegisterNotificationResponse.Parse(s);
                                ropsList.Add(ropRegisterNotificationResponse);
                                break;

                            case RopIdType.RopPending:
                                RopPendingResponse ropPendingResponse = new RopPendingResponse();
                                ropPendingResponse.Parse(s);
                                ropsList.Add(ropPendingResponse);
                                break;

                            case RopIdType.RopNotify:
                                RopNotifyResponse ropNotifyResponse = new RopNotifyResponse();
                                ropNotifyResponse.Parse(s);
                                ropsList.Add(ropNotifyResponse);
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                RopGetPermissionsTableResponse ropGetPermissionsTableResponse = new RopGetPermissionsTableResponse();
                                ropGetPermissionsTableResponse.Parse(s);
                                ropsList.Add(ropGetPermissionsTableResponse);
                                break;

                            case RopIdType.RopModifyPermissions:
                                RopModifyPermissionsResponse ropModifyPermissionsResponse = new RopModifyPermissionsResponse();
                                ropModifyPermissionsResponse.Parse(s);
                                ropsList.Add(ropModifyPermissionsResponse);
                                break;

                            default:
                                object ropsBytes = ReadBytes(RopSize - (int)s.Position);
                                ropsList.Add(ropsBytes);
                                break;
                        }
                    }
                    while (s.Position < RopSize);
                }
                else
                {
                    RopsList = null;
                }
            }
            else
            {
                byte[] ropListBytes = ReadBytes(RopSize - 2);
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

            while (s.Position + sizeof(uint) <= s.Length)
            {
                uint serverObjectHandle = ReadUint();
                serverObjectHandleTable.Add(serverObjectHandle);
            }

            ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
}
