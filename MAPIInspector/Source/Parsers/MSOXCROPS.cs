using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MAPIInspector.Parsers
{
    #region ROP Input Buffer
    /// <summary>
    ///  A class indicates the ROP input buffer, which is sent by the client, includes an array of ROP request buffers to be processed by the server.
    /// </summary>
    public class ROPInputBuffer : BaseStructure
    {
        // An unsigned integer that specifies the size of both this field and the RopsList field.
        public ushort RopSize;

        // An array of ROP request buffers.
        public object[] RopsList;

        // An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPInputBuffer structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPInputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            bool ParseToCROPSRequestLayer = false;
            this.RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            List<uint> RopRemainSize = new List<uint>();
            List<uint> tempServerObjectHandleTable = new List<uint>();

            long currentPosition = s.Position;
            s.Position += (this.RopSize - 2);
            while (s.Position < s.Length)
            {
                uint serverObjectTable = ReadUint();
                if (MapiInspector.MAPIInspector.targetHandle.Count > 0)
                {
                    MapiInspector.MAPIInspector.isLooperCall = true;
                    Dictionary<int, uint> item = new Dictionary<int, uint>();
                    item = MapiInspector.MAPIInspector.targetHandle.Peek();
                    if (item.First().Value == serverObjectTable)
                    {
                        ParseToCROPSRequestLayer = true;
                    }
                }
                else
                {
                    MapiInspector.MAPIInspector.isLooperCall = false;
                }

                tempServerObjectHandleTable.Add(serverObjectTable);
            }
            s.Position = currentPosition;

            if (!MapiInspector.MAPIInspector.isLooperCall || ParseToCROPSRequestLayer || MapiInspector.MAPIInspector.needToParseCROPSLayer)
            {
                Queue<PropertyTag[]> ProDics = new Queue<PropertyTag[]>();
                Dictionary<uint, Queue<PropertyTag[]>> PropertyTagsForGetPropertiesSpec = new Dictionary<uint, Queue<PropertyTag[]>>();
                Dictionary<uint, LogonFlags> LogonFlagsInLogonRop = new Dictionary<uint, LogonFlags>();
                int setColumnCountInThisList = 0;
                if (this.RopSize > 2)
                {
                    RopRemainSize.Add(this.RopSize - (uint)2);
                    do
                    {
                        int CurrentByte = s.ReadByte();
                        s.Position -= 1;
                        switch ((RopIdType)CurrentByte)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                RopLogonRequest RopLogonRequest = new RopLogonRequest();
                                RopLogonRequest.Parse(s);
                                ropsList.Add(RopLogonRequest);

                                // update variables used for parsing logon response
                                if (LogonFlagsInLogonRop.Count > 0 && LogonFlagsInLogonRop.ContainsKey(RopLogonRequest.OutputHandleIndex))
                                {
                                    LogonFlagsInLogonRop[RopLogonRequest.OutputHandleIndex] = RopLogonRequest.LogonFlags;
                                }
                                else
                                {
                                    LogonFlagsInLogonRop.Add(RopLogonRequest.OutputHandleIndex, RopLogonRequest.LogonFlags);
                                }
                                if (LogonFlagsInLogonRop.Count > 0)
                                {
                                    if (DecodingContext.SessionLogonFlagsInLogonRop.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id))
                                    {
                                        DecodingContext.SessionLogonFlagsInLogonRop.Remove(MapiInspector.MAPIInspector.parsingSession.id);
                                    }
                                    DecodingContext.SessionLogonFlagsInLogonRop.Add(MapiInspector.MAPIInspector.parsingSession.id, LogonFlagsInLogonRop);
                                }

                                // update variables used for parsing messages in other rops which need logonFlags
                                if (DecodingContext.LogonFlagMapLogId.Count > 0 && DecodingContext.LogonFlagMapLogId.ContainsKey(RopLogonRequest.LogonId))
                                {
                                    DecodingContext.LogonFlagMapLogId.Remove(RopLogonRequest.LogonId);
                                }
                                DecodingContext.LogonFlagMapLogId.Add(RopLogonRequest.LogonId, RopLogonRequest.LogonFlags);
                                break;
                            case RopIdType.RopGetReceiveFolder:
                                RopGetReceiveFolderRequest RopGetReceiveFolderRequest = new RopGetReceiveFolderRequest();
                                RopGetReceiveFolderRequest.Parse(s);
                                ropsList.Add(RopGetReceiveFolderRequest);
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                RopSetReceiveFolderRequest RopSetReceiveFolderRequest = new RopSetReceiveFolderRequest();
                                RopSetReceiveFolderRequest.Parse(s);
                                ropsList.Add(RopSetReceiveFolderRequest);
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                RopGetReceiveFolderTableRequest RopGetReceiveFolderTableRequest = new RopGetReceiveFolderTableRequest();
                                RopGetReceiveFolderTableRequest.Parse(s);
                                ropsList.Add(RopGetReceiveFolderTableRequest);
                                break;
                            case RopIdType.RopGetStoreState:
                                RopGetStoreStateRequest RopGetStoreStateRequest = new RopGetStoreStateRequest();
                                RopGetStoreStateRequest.Parse(s);
                                ropsList.Add(RopGetStoreStateRequest);
                                break;
                            case RopIdType.RopGetOwningServers:
                                RopGetOwningServersRequest RopGetOwningServersRequest = new RopGetOwningServersRequest();
                                RopGetOwningServersRequest.Parse(s);
                                ropsList.Add(RopGetOwningServersRequest);
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                RopPublicFolderIsGhostedRequest RopPublicFolderIsGhostedRequest = new RopPublicFolderIsGhostedRequest();
                                RopPublicFolderIsGhostedRequest.Parse(s);
                                ropsList.Add(RopPublicFolderIsGhostedRequest);
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                RopLongTermIdFromIdRequest RopLongTermIdFromIdRequest = new RopLongTermIdFromIdRequest();
                                RopLongTermIdFromIdRequest.Parse(s);
                                ropsList.Add(RopLongTermIdFromIdRequest);
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                RopIdFromLongTermIdRequest RopIdFromLongTermIdRequest = new RopIdFromLongTermIdRequest();
                                RopIdFromLongTermIdRequest.Parse(s);
                                ropsList.Add(RopIdFromLongTermIdRequest);
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                RopGetPerUserLongTermIdsRequest RopGetPerUserLongTermIdsRequest = new RopGetPerUserLongTermIdsRequest();
                                RopGetPerUserLongTermIdsRequest.Parse(s);
                                ropsList.Add(RopGetPerUserLongTermIdsRequest);
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                RopGetPerUserGuidRequest RopGetPerUserGuidRequest = new RopGetPerUserGuidRequest();
                                RopGetPerUserGuidRequest.Parse(s);
                                ropsList.Add(RopGetPerUserGuidRequest);
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                RopReadPerUserInformationRequest RopReadPerUserInformationRequest = new RopReadPerUserInformationRequest();
                                RopReadPerUserInformationRequest.Parse(s);
                                ropsList.Add(RopReadPerUserInformationRequest);
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                byte RopId = ReadByte();
                                byte logonId = ReadByte();
                                s.Position -= 2;
                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id)
                                      && DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIInspector.parsingSession.id].ContainsKey(logonId)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopWritePerUserInformation", (ushort)CurrentByte, new uint[] { logonId });
                                }
                                RopWritePerUserInformationRequest RopWritePerUserInformationRequest = new RopWritePerUserInformationRequest();
                                RopWritePerUserInformationRequest.Parse(s);
                                ropsList.Add(RopWritePerUserInformationRequest);
                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                RopSubmitMessageRequest RopSubmitMessageRequest = new RopSubmitMessageRequest();
                                RopSubmitMessageRequest.Parse(s);
                                ropsList.Add(RopSubmitMessageRequest);
                                break;
                            case RopIdType.RopAbortSubmit:
                                RopAbortSubmitRequest RopAbortSubmitRequest = new RopAbortSubmitRequest();
                                RopAbortSubmitRequest.Parse(s);
                                ropsList.Add(RopAbortSubmitRequest);
                                break;
                            case RopIdType.RopGetAddressTypes:
                                RopGetAddressTypesRequest RopGetAddressTypesRequest = new RopGetAddressTypesRequest();
                                RopGetAddressTypesRequest.Parse(s);
                                ropsList.Add(RopGetAddressTypesRequest);
                                break;
                            case RopIdType.RopSetSpooler:
                                RopSetSpoolerRequest RopSetSpoolerRequest = new RopSetSpoolerRequest();
                                RopSetSpoolerRequest.Parse(s);
                                ropsList.Add(RopSetSpoolerRequest);
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                RopSpoolerLockMessageRequest RopSpoolerLockMessageRequest = new RopSpoolerLockMessageRequest();
                                RopSpoolerLockMessageRequest.Parse(s);
                                ropsList.Add(RopSpoolerLockMessageRequest);
                                break;
                            case RopIdType.RopTransportSend:
                                RopTransportSendRequest RopTransportSendRequest = new RopTransportSendRequest();
                                RopTransportSendRequest.Parse(s);
                                ropsList.Add(RopTransportSendRequest);
                                break;
                            case RopIdType.RopTransportNewMail:
                                RopTransportNewMailRequest RopTransportNewMailRequest = new RopTransportNewMailRequest();
                                RopTransportNewMailRequest.Parse(s);
                                ropsList.Add(RopTransportNewMailRequest);
                                break;
                            case RopIdType.RopGetTransportFolder:
                                RopGetTransportFolderRequest RopGetTransportFolderRequest = new RopGetTransportFolderRequest();
                                RopGetTransportFolderRequest.Parse(s);
                                ropsList.Add(RopGetTransportFolderRequest);
                                break;
                            case RopIdType.RopOptionsData:
                                RopOptionsDataRequest RopOptionsDataRequest = new RopOptionsDataRequest();
                                RopOptionsDataRequest.Parse(s);
                                ropsList.Add(RopOptionsDataRequest);
                                break;
                            case RopIdType.RopRelease:
                                RopReleaseRequest RopReleaseRequest = new RopReleaseRequest();
                                RopReleaseRequest.Parse(s);
                                ropsList.Add(RopReleaseRequest);
                                uint handle_Release = tempServerObjectHandleTable[RopReleaseRequest.InputHandleIndex];
                                if (DecodingContext.SetColumnProTagMap_Handle.ContainsKey(handle_Release))
                                {
                                    DecodingContext.SetColumnProTagMap_Handle.Remove(handle_Release);
                                }
                                break;

                            // MSOXCTABL ROPs
                            case RopIdType.RopSetColumns:
                                RopSetColumnsRequest RopSetColumnsRequest = new RopSetColumnsRequest();
                                RopSetColumnsRequest.Parse(s);
                                ropsList.Add(RopSetColumnsRequest);
                                uint handle_SetColumns = tempServerObjectHandleTable[RopSetColumnsRequest.InputHandleIndex];
                                if (handle_SetColumns != 0xFFFFFFFF)
                                {
                                    // When the object handle is not equal to 0xFFFFFFFF, add objectHandle and Property Tags to the dictionary.                                    
                                    Dictionary<uint, PropertyTag[]> propTags = new Dictionary<uint, PropertyTag[]>();

                                    if (DecodingContext.SetColumnProTagMap_Handle.ContainsKey(handle_SetColumns))
                                    {
                                        if (DecodingContext.SetColumnProTagMap_Handle[handle_SetColumns].First().Key <= MapiInspector.MAPIInspector.parsingSession.id)
                                        {
                                            DecodingContext.SetColumnProTagMap_Handle.Remove(handle_SetColumns);
                                            propTags.Add((uint)MapiInspector.MAPIInspector.parsingSession.id, RopSetColumnsRequest.PropertyTags);
                                            DecodingContext.SetColumnProTagMap_Handle.Add(handle_SetColumns, propTags);
                                        }
                                    }
                                    else
                                    {
                                        propTags.Add((uint)MapiInspector.MAPIInspector.parsingSession.id, RopSetColumnsRequest.PropertyTags);
                                        DecodingContext.SetColumnProTagMap_Handle.Add(handle_SetColumns, propTags);
                                    }
                                    // setColumnCountInThisList recored the index of the setColumns in same session
                                    setColumnCountInThisList += 1;
                                    if (!DecodingContext.PropertyTagsForNotify.ContainsKey(handle_SetColumns))
                                    {
                                        DecodingContext.PropertyTagsForNotify.Add(handle_SetColumns, RopSetColumnsRequest.PropertyTags);
                                    }
                                    if ((setColumnCountInThisList > 1) && DecodingContext.PropertyTagsForNotify.ContainsKey(handle_SetColumns))
                                    {
                                        DecodingContext.PropertyTagsForNotify[handle_SetColumns] = RopSetColumnsRequest.PropertyTags;
                                    }
                                }
                                else
                                {
                                    uint outputHandle;
                                    try
                                    {
										MapiInspector.MAPIInspector.isOnlyGetServerHandle = true;
                                        outputHandle = MapiInspector.MAPIInspector.ParseResponseMessageSimplely(MapiInspector.MAPIInspector.parsingSession, RopSetColumnsRequest.InputHandleIndex);
                                    }
                                    finally
                                    {
                                        MapiInspector.MAPIInspector.isOnlyGetServerHandle = false;
                                    }

                                    Dictionary<uint, PropertyTag[]> propTags = new Dictionary<uint, PropertyTag[]>();
                                    if (DecodingContext.SetColumnProTagMap_Handle.ContainsKey(outputHandle))
                                    {
                                        if (DecodingContext.SetColumnProTagMap_Handle[outputHandle].First().Key <= MapiInspector.MAPIInspector.parsingSession.id)
                                        {
                                            DecodingContext.SetColumnProTagMap_Handle.Remove(outputHandle);
                                            propTags.Add((uint)MapiInspector.MAPIInspector.parsingSession.id, RopSetColumnsRequest.PropertyTags);
                                            DecodingContext.SetColumnProTagMap_Handle.Add(outputHandle, propTags);
                                        }
                                    }
                                    else
                                    {
                                        propTags.Add((uint)MapiInspector.MAPIInspector.parsingSession.id, RopSetColumnsRequest.PropertyTags);
                                        DecodingContext.SetColumnProTagMap_Handle.Add(outputHandle, propTags);
                                    }

                                    // if the setColumnCountInThisList count is not zero, it means there is more than one RopSetcolumns in current session. If the handles are equal, the last one PropertyTags should be used.
                                    if (setColumnCountInThisList != 0)
                                    {
                                        if (DecodingContext.SetColumn_InputHandles_InRequest.ContainsKey(outputHandle))
                                        {
                                            DecodingContext.SetColumn_InputHandles_InRequest[outputHandle] = RopSetColumnsRequest.PropertyTags;
                                        }
                                        else
                                        {
                                            DecodingContext.SetColumn_InputHandles_InRequest.Add(outputHandle, RopSetColumnsRequest.PropertyTags);
                                        }
                                    }
                                    else
                                    {
                                        // if the setColumnCountInThisList count is zero, it means there is only one RopSetcolumns in current session. The PropertyTags will not be covered.                                    
                                        if (!DecodingContext.SetColumn_InputHandles_InRequest.ContainsKey(outputHandle))
                                        {
                                            DecodingContext.SetColumn_InputHandles_InRequest.Add(outputHandle, RopSetColumnsRequest.PropertyTags);
                                        }
                                    }
                                    setColumnCountInThisList += 1;

                                    if (DecodingContext.SetColumn_InputHandles_InRequest.Count > 0)
                                    {
                                        if (!DecodingContext.PropertyTagsForNotify.ContainsKey(outputHandle))
                                        {
                                            DecodingContext.PropertyTagsForNotify.Add(outputHandle, DecodingContext.SetColumn_InputHandles_InRequest[outputHandle]);
                                        }

                                        if ((setColumnCountInThisList > 1) && DecodingContext.PropertyTagsForNotify.ContainsKey(outputHandle))
                                        {
                                            DecodingContext.PropertyTagsForNotify[outputHandle] = DecodingContext.SetColumn_InputHandles_InRequest[outputHandle];
                                        }
                                    }

                                }
                                break;

                            case RopIdType.RopSortTable:
                                RopSortTableRequest RopSortTableRequest = new RopSortTableRequest();
                                RopSortTableRequest.Parse(s);
                                ropsList.Add(RopSortTableRequest);
                                break;

                            case RopIdType.RopRestrict:
                                RopRestrictRequest RopRestrictRequest = new RopRestrictRequest();
                                RopRestrictRequest.Parse(s);
                                ropsList.Add(RopRestrictRequest);
                                break;

                            case RopIdType.RopQueryRows:
                                RopQueryRowsRequest RopQueryRowsRequest = new RopQueryRowsRequest();
                                RopQueryRowsRequest.Parse(s);
                                ropsList.Add(RopQueryRowsRequest);
                                break;

                            case RopIdType.RopAbort:
                                RopAbortRequest RopAbortRequest = new RopAbortRequest();
                                RopAbortRequest.Parse(s);
                                ropsList.Add(RopAbortRequest);
                                break;

                            case RopIdType.RopGetStatus:
                                RopGetStatusRequest RopGetStatusRequest = new RopGetStatusRequest();
                                RopGetStatusRequest.Parse(s);
                                ropsList.Add(RopGetStatusRequest);
                                break;

                            case RopIdType.RopQueryPosition:
                                RopQueryPositionRequest RopQueryPositionRequest = new RopQueryPositionRequest();
                                RopQueryPositionRequest.Parse(s);
                                ropsList.Add(RopQueryPositionRequest);
                                break;

                            case RopIdType.RopSeekRow:
                                RopSeekRowRequest RopSeekRowRequest = new RopSeekRowRequest();
                                RopSeekRowRequest.Parse(s);
                                ropsList.Add(RopSeekRowRequest);
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                RopSeekRowBookmarkRequest RopSeekRowBookmarkRequest = new RopSeekRowBookmarkRequest();
                                RopSeekRowBookmarkRequest.Parse(s);
                                ropsList.Add(RopSeekRowBookmarkRequest);
                                break;

                            case RopIdType.RopSeekRowFractional:
                                RopSeekRowFractionalRequest RopSeekRowFractionalRequest = new RopSeekRowFractionalRequest();
                                RopSeekRowFractionalRequest.Parse(s);
                                ropsList.Add(RopSeekRowFractionalRequest);
                                break;

                            case RopIdType.RopCreateBookmark:
                                RopCreateBookmarkRequest RopCreateBookmarkRequest = new RopCreateBookmarkRequest();
                                RopCreateBookmarkRequest.Parse(s);
                                ropsList.Add(RopCreateBookmarkRequest);
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                RopQueryColumnsAllRequest RopQueryColumnsAllRequest = new RopQueryColumnsAllRequest();
                                RopQueryColumnsAllRequest.Parse(s);
                                ropsList.Add(RopQueryColumnsAllRequest);
                                break;

                            case RopIdType.RopFindRow:
                                RopFindRowRequest RopFindRowRequest = new RopFindRowRequest();
                                RopFindRowRequest.Parse(s);
                                ropsList.Add(RopFindRowRequest);
                                break;

                            case RopIdType.RopFreeBookmark:
                                RopFreeBookmarkRequest RopFreeBookmarkRequest = new RopFreeBookmarkRequest();
                                RopFreeBookmarkRequest.Parse(s);
                                ropsList.Add(RopFreeBookmarkRequest);
                                break;

                            case RopIdType.RopResetTable:
                                RopResetTableRequest RopResetTableRequest = new RopResetTableRequest();
                                RopResetTableRequest.Parse(s);
                                ropsList.Add(RopResetTableRequest);
                                break;

                            case RopIdType.RopExpandRow:
                                RopExpandRowRequest RopExpandRowRequest = new RopExpandRowRequest();
                                RopExpandRowRequest.Parse(s);
                                ropsList.Add(RopExpandRowRequest);
                                break;

                            case RopIdType.RopCollapseRow:
                                RopCollapseRowRequest RopCollapseRowRequest = new RopCollapseRowRequest();
                                RopCollapseRowRequest.Parse(s);
                                ropsList.Add(RopCollapseRowRequest);
                                break;

                            case RopIdType.RopGetCollapseState:
                                RopGetCollapseStateRequest RopGetCollapseStateRequest = new RopGetCollapseStateRequest();
                                RopGetCollapseStateRequest.Parse(s);
                                ropsList.Add(RopGetCollapseStateRequest);
                                break;

                            case RopIdType.RopSetCollapseState:
                                RopSetCollapseStateRequest RopSetCollapseStateRequest = new RopSetCollapseStateRequest();
                                RopSetCollapseStateRequest.Parse(s);
                                ropsList.Add(RopSetCollapseStateRequest);
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                RopModifyRulesRequest RopModifyRulesRequest = new RopModifyRulesRequest();
                                RopModifyRulesRequest.Parse(s);
                                ropsList.Add(RopModifyRulesRequest);
                                break;

                            case RopIdType.RopGetRulesTable:
                                RopGetRulesTableRequest RopGetRulesTableRequest = new RopGetRulesTableRequest();
                                RopGetRulesTableRequest.Parse(s);
                                ropsList.Add(RopGetRulesTableRequest);
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                RopUpdateDeferredActionMessagesRequest RopUpdateDeferredActionMessagesRequest = new RopUpdateDeferredActionMessagesRequest();
                                RopUpdateDeferredActionMessagesRequest.Parse(s);
                                ropsList.Add(RopUpdateDeferredActionMessagesRequest);
                                break;

                            //MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                RopFastTransferSourceCopyPropertiesRequest RopFastTransferSourceCopyPropertiesRequest = new RopFastTransferSourceCopyPropertiesRequest();
                                RopFastTransferSourceCopyPropertiesRequest.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyPropertiesRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                RopFastTransferSourceCopyToRequest RopFastTransferSourceCopyToRequest = new RopFastTransferSourceCopyToRequest();
                                RopFastTransferSourceCopyToRequest.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyToRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                RopFastTransferSourceCopyMessagesRequest RopFastTransferSourceCopyMessagesRequest = new RopFastTransferSourceCopyMessagesRequest();
                                RopFastTransferSourceCopyMessagesRequest.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyMessagesRequest);
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                RopFastTransferSourceCopyFolderRequest RopFastTransferSourceCopyFolderRequest = new RopFastTransferSourceCopyFolderRequest();
                                RopFastTransferSourceCopyFolderRequest.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyFolderRequest);
                                break;
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                RopFastTransferSourceGetBufferRequest RopFastTransferSourceGetBufferRequest = new RopFastTransferSourceGetBufferRequest();
                                RopFastTransferSourceGetBufferRequest.Parse(s);
                                ropsList.Add(RopFastTransferSourceGetBufferRequest);
                                break;
                            case RopIdType.RopTellVersion:
                                RopTellVersionRequest RopTellVersionRequest = new RopTellVersionRequest();
                                RopTellVersionRequest.Parse(s);
                                ropsList.Add(RopTellVersionRequest);
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                RopFastTransferDestinationConfigureRequest RopFastTransferDestinationConfigureRequest = new RopFastTransferDestinationConfigureRequest();
                                RopFastTransferDestinationConfigureRequest.Parse(s);
                                ropsList.Add(RopFastTransferDestinationConfigureRequest);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                                long currentPos_putBuffer = s.Position;
                                s.Position += 2;
                                int TempInputHandleIndex_putBuffer = s.ReadByte();
                                s.Position = currentPos_putBuffer;
                                uint ropPutbufferHandle = tempServerObjectHandleTable[TempInputHandleIndex_putBuffer];
                                uint uParsingSessionID = (uint)MapiInspector.MAPIInspector.parsingSession.id;
                                if (tempServerObjectHandleTable[TempInputHandleIndex_putBuffer] != 0xffffffff)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey((int)uParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException((ushort)CurrentByte, ropPutbufferHandle);
                                    }
                                }
                                else
                                {
                                    MapiInspector.MAPIInspector.pPutType = 0;
                                    MapiInspector.MAPIInspector.pPutRemainSize = -1;
                                    MapiInspector.MAPIInspector.pPutSubRemainSize = -1;
                                }
                                RopFastTransferDestinationPutBufferRequest RopFastTransferDestinationPutBufferRequest = new RopFastTransferDestinationPutBufferRequest();
                                MapiInspector.MAPIInspector.isPut = true;
                                RopFastTransferDestinationPutBufferRequest.Parse(s);
                                ropsList.Add(RopFastTransferDestinationPutBufferRequest);

                                MapiInspector.MAPIInspector.PartialContextInformation putBufferPartialInformaiton = new MapiInspector.MAPIInspector.PartialContextInformation(MapiInspector.MAPIInspector.pPutType, MapiInspector.MAPIInspector.pPutId, MapiInspector.MAPIInspector.pPutRemainSize, MapiInspector.MAPIInspector.pPutSubRemainSize, false, MapiInspector.MAPIInspector.inputPayLoadCompresssedXOR);
                                SortedDictionary<uint, MapiInspector.MAPIInspector.PartialContextInformation> sessionputContextInfor = new SortedDictionary<uint, MapiInspector.MAPIInspector.PartialContextInformation>();
                                if (MapiInspector.MAPIInspector.handleWithSessionPutContextInformation.ContainsKey(ropPutbufferHandle))
                                {
                                    sessionputContextInfor = MapiInspector.MAPIInspector.handleWithSessionPutContextInformation[ropPutbufferHandle];
                                    MapiInspector.MAPIInspector.handleWithSessionPutContextInformation.Remove(ropPutbufferHandle);
                                }
                                if (sessionputContextInfor.ContainsKey(uParsingSessionID))
                                {
                                    sessionputContextInfor[uParsingSessionID] = putBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionputContextInfor.Add(uParsingSessionID, putBufferPartialInformaiton);
                                }
                                MapiInspector.MAPIInspector.handleWithSessionPutContextInformation.Add(ropPutbufferHandle, sessionputContextInfor);
                                MapiInspector.MAPIInspector.isPut = false;
                                break;

                            case RopIdType.RopFastTransferDestinationPutBufferExtended:
                                long currentPos_putBufferExtended = s.Position;
                                s.Position += 2;
                                int TempInputHandleIndex_putBufferExtended = s.ReadByte();
                                s.Position = currentPos_putBufferExtended;
                                uint ropPutExtendbufferHandle = tempServerObjectHandleTable[TempInputHandleIndex_putBufferExtended];
                                uint eParsingSessionID = (uint)MapiInspector.MAPIInspector.parsingSession.id;
                                if (tempServerObjectHandleTable[TempInputHandleIndex_putBufferExtended] != 0xffffffff)
                                {                                
                                    if (!DecodingContext.PartialInformationReady.ContainsKey((int)eParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException((ushort)CurrentByte, ropPutExtendbufferHandle);
                                    }
                                }
                                else
                                {
                                    MapiInspector.MAPIInspector.pPutExtendType = 0;
                                    MapiInspector.MAPIInspector.pPutExtendRemainSize = -1;
                                    MapiInspector.MAPIInspector.pPutExtendSubRemainSize = -1;
                                }
                                RopFastTransferDestinationPutBufferExtendedRequest RopFastTransferDestinationPutBufferExtendedRequest = new RopFastTransferDestinationPutBufferExtendedRequest();
                                MapiInspector.MAPIInspector.isPutExtend = true;
                                RopFastTransferDestinationPutBufferExtendedRequest.Parse(s);
                                ropsList.Add(RopFastTransferDestinationPutBufferExtendedRequest);

                                MapiInspector.MAPIInspector.PartialContextInformation putExtendBufferPartialInformaiton = new MapiInspector.MAPIInspector.PartialContextInformation(MapiInspector.MAPIInspector.pPutType, MapiInspector.MAPIInspector.pPutId, MapiInspector.MAPIInspector.pPutRemainSize, MapiInspector.MAPIInspector.pPutSubRemainSize, false, MapiInspector.MAPIInspector.inputPayLoadCompresssedXOR);
                                SortedDictionary<uint, MapiInspector.MAPIInspector.PartialContextInformation> sessionputExtendContextInfor = new SortedDictionary<uint, MapiInspector.MAPIInspector.PartialContextInformation>();
                                if (MapiInspector.MAPIInspector.handleWithSessionPutExtendContextInformation.ContainsKey(ropPutExtendbufferHandle))
                                {
                                    sessionputExtendContextInfor = MapiInspector.MAPIInspector.handleWithSessionPutExtendContextInformation[ropPutExtendbufferHandle];
                                    MapiInspector.MAPIInspector.handleWithSessionPutExtendContextInformation.Remove(ropPutExtendbufferHandle);
                                }
                                if (sessionputExtendContextInfor.ContainsKey(eParsingSessionID))
                                {
                                    sessionputExtendContextInfor[eParsingSessionID] = putExtendBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionputExtendContextInfor.Add(eParsingSessionID, putExtendBufferPartialInformaiton);
                                }
                                MapiInspector.MAPIInspector.handleWithSessionPutExtendContextInformation.Add(ropPutExtendbufferHandle, sessionputExtendContextInfor);
                                MapiInspector.MAPIInspector.isPutExtend = false;

                                break;
                            case RopIdType.RopSynchronizationConfigure:
                                RopSynchronizationConfigureRequest RopSynchronizationConfigureRequest = new RopSynchronizationConfigureRequest();
                                RopSynchronizationConfigureRequest.Parse(s);
                                ropsList.Add(RopSynchronizationConfigureRequest);
                                break;
                            case RopIdType.RopSynchronizationGetTransferState:
                                RopSynchronizationGetTransferStateRequest RopSynchronizationGetTransferStateRequest = new RopSynchronizationGetTransferStateRequest();
                                RopSynchronizationGetTransferStateRequest.Parse(s);
                                ropsList.Add(RopSynchronizationGetTransferStateRequest);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                RopSynchronizationUploadStateStreamBeginRequest RopSynchronizationUploadStateStreamBeginRequest = new RopSynchronizationUploadStateStreamBeginRequest();
                                RopSynchronizationUploadStateStreamBeginRequest.Parse(s);
                                ropsList.Add(RopSynchronizationUploadStateStreamBeginRequest);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                RopSynchronizationUploadStateStreamContinueRequest RopSynchronizationUploadStateStreamContinueRequest = new RopSynchronizationUploadStateStreamContinueRequest();
                                RopSynchronizationUploadStateStreamContinueRequest.Parse(s);
                                ropsList.Add(RopSynchronizationUploadStateStreamContinueRequest);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                RopSynchronizationUploadStateStreamEndRequest RopSynchronizationUploadStateStreamEndRequest = new RopSynchronizationUploadStateStreamEndRequest();
                                RopSynchronizationUploadStateStreamEndRequest.Parse(s);
                                ropsList.Add(RopSynchronizationUploadStateStreamEndRequest);
                                break;
                            case RopIdType.RopSynchronizationOpenCollector:
                                RopSynchronizationOpenCollectorRequest RopSynchronizationOpenCollectorRequest = new RopSynchronizationOpenCollectorRequest();
                                RopSynchronizationOpenCollectorRequest.Parse(s);
                                ropsList.Add(RopSynchronizationOpenCollectorRequest);
                                break;
                            case RopIdType.RopSynchronizationImportMessageChange:
                                RopSynchronizationImportMessageChangeRequest RopSynchronizationImportMessageChangeRequest = new RopSynchronizationImportMessageChangeRequest();
                                RopSynchronizationImportMessageChangeRequest.Parse(s);
                                ropsList.Add(RopSynchronizationImportMessageChangeRequest);
                                break;
                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                RopSynchronizationImportHierarchyChangeRequest RopSynchronizationImportHierarchyChangeRequest = new RopSynchronizationImportHierarchyChangeRequest();
                                RopSynchronizationImportHierarchyChangeRequest.Parse(s);
                                ropsList.Add(RopSynchronizationImportHierarchyChangeRequest);
                                break;
                            case RopIdType.RopSynchronizationImportMessageMove:
                                RopSynchronizationImportMessageMoveRequest RopSynchronizationImportMessageMoveRequest = new RopSynchronizationImportMessageMoveRequest();
                                RopSynchronizationImportMessageMoveRequest.Parse(s);
                                ropsList.Add(RopSynchronizationImportMessageMoveRequest);
                                break;
                            case RopIdType.RopSynchronizationImportDeletes:
                                RopSynchronizationImportDeletesRequest RopSynchronizationImportDeletesRequest = new RopSynchronizationImportDeletesRequest();
                                RopSynchronizationImportDeletesRequest.Parse(s);
                                ropsList.Add(RopSynchronizationImportDeletesRequest);
                                break;
                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                RopSynchronizationImportReadStateChangesRequest RopSynchronizationImportReadStateChangesRequest = new RopSynchronizationImportReadStateChangesRequest();
                                RopSynchronizationImportReadStateChangesRequest.Parse(s);
                                ropsList.Add(RopSynchronizationImportReadStateChangesRequest);
                                break;
                            case RopIdType.RopGetLocalReplicaIds:
                                RopGetLocalReplicaIdsRequest RopGetLocalReplicaIdsRequest = new RopGetLocalReplicaIdsRequest();
                                RopGetLocalReplicaIdsRequest.Parse(s);
                                ropsList.Add(RopGetLocalReplicaIdsRequest);
                                break;
                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                RopSetLocalReplicaMidsetDeletedRequest RopSetLocalReplicaMidsetDeletedRequest = new RopSetLocalReplicaMidsetDeletedRequest();
                                RopSetLocalReplicaMidsetDeletedRequest.Parse(s);
                                ropsList.Add(RopSetLocalReplicaMidsetDeletedRequest);
                                break;

                            // MS-OXCPRPT ROPs
                            case RopIdType.RopGetPropertiesSpecific:
                                RopGetPropertiesSpecificRequest RopGetPropertiesSpecificRequest = new RopGetPropertiesSpecificRequest();
                                RopGetPropertiesSpecificRequest.Parse(s);
                                ropsList.Add(RopGetPropertiesSpecificRequest);

                                if (PropertyTagsForGetPropertiesSpec.ContainsKey(RopGetPropertiesSpecificRequest.InputHandleIndex))
                                {
                                    if (PropertyTagsForGetPropertiesSpec[RopGetPropertiesSpecificRequest.InputHandleIndex].Count == 1)
                                    {
                                        ProDics.Enqueue(PropertyTagsForGetPropertiesSpec[RopGetPropertiesSpecificRequest.InputHandleIndex].Dequeue());
                                    }
                                    ProDics.Enqueue(RopGetPropertiesSpecificRequest.PropertyTags);
                                    PropertyTagsForGetPropertiesSpec[RopGetPropertiesSpecificRequest.InputHandleIndex] = ProDics;
                                }
                                else
                                {
                                    Queue<PropertyTag[]> ProDic0 = new Queue<PropertyTag[]>();
                                    ProDic0.Enqueue(RopGetPropertiesSpecificRequest.PropertyTags);
                                    PropertyTagsForGetPropertiesSpec.Add(RopGetPropertiesSpecificRequest.InputHandleIndex, ProDic0);
                                }
                                if (PropertyTagsForGetPropertiesSpec.Count > 0)
                                {
                                    if (DecodingContext.GetPropertiesSpec_propertyTags.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id))
                                    {
                                        DecodingContext.GetPropertiesSpec_propertyTags.Remove(MapiInspector.MAPIInspector.parsingSession.id);
                                    }
                                    DecodingContext.GetPropertiesSpec_propertyTags.Add(MapiInspector.MAPIInspector.parsingSession.id, PropertyTagsForGetPropertiesSpec);
                                }
                                break;
                            case RopIdType.RopGetPropertiesAll:
                                RopGetPropertiesAllRequest RopGetPropertiesAllRequest = new RopGetPropertiesAllRequest();
                                RopGetPropertiesAllRequest.Parse(s);
                                ropsList.Add(RopGetPropertiesAllRequest);
                                break;
                            case RopIdType.RopGetPropertiesList:
                                RopGetPropertiesListRequest RopGetPropertiesListRequest = new RopGetPropertiesListRequest();
                                RopGetPropertiesListRequest.Parse(s);
                                ropsList.Add(RopGetPropertiesListRequest);
                                break;
                            case RopIdType.RopSetProperties:
                                RopSetPropertiesRequest RopSetPropertiesRequest = new RopSetPropertiesRequest();
                                RopSetPropertiesRequest.Parse(s);
                                ropsList.Add(RopSetPropertiesRequest);
                                break;
                            case RopIdType.RopSetPropertiesNoReplicate:
                                RopSetPropertiesNoReplicateRequest RopSetPropertiesNoReplicateRequest = new RopSetPropertiesNoReplicateRequest();
                                RopSetPropertiesNoReplicateRequest.Parse(s);
                                ropsList.Add(RopSetPropertiesNoReplicateRequest);
                                break;
                            case RopIdType.RopDeleteProperties:
                                RopDeletePropertiesRequest RopDeletePropertiesRequest = new RopDeletePropertiesRequest();
                                RopDeletePropertiesRequest.Parse(s);
                                ropsList.Add(RopDeletePropertiesRequest);
                                break;
                            case RopIdType.RopDeletePropertiesNoReplicate:
                                RopDeletePropertiesNoReplicateRequest RopDeletePropertiesNoReplicateRequest = new RopDeletePropertiesNoReplicateRequest();
                                RopDeletePropertiesNoReplicateRequest.Parse(s);
                                ropsList.Add(RopDeletePropertiesNoReplicateRequest);
                                break;
                            case RopIdType.RopQueryNamedProperties:
                                RopQueryNamedPropertiesRequest RopQueryNamedPropertiesRequest = new RopQueryNamedPropertiesRequest();
                                RopQueryNamedPropertiesRequest.Parse(s);
                                ropsList.Add(RopQueryNamedPropertiesRequest);
                                break;
                            case RopIdType.RopCopyProperties:
                                RopCopyPropertiesRequest RopCopyPropertiesRequest = new RopCopyPropertiesRequest();
                                RopCopyPropertiesRequest.Parse(s);
                                ropsList.Add(RopCopyPropertiesRequest);
                                break;
                            case RopIdType.RopCopyTo:
                                RopCopyToRequest RopCopyToRequest = new RopCopyToRequest();
                                RopCopyToRequest.Parse(s);
                                ropsList.Add(RopCopyToRequest);
                                break;
                            case RopIdType.RopGetPropertyIdsFromNames:
                                RopGetPropertyIdsFromNamesRequest RopGetPropertyIdsFromNamesRequest = new RopGetPropertyIdsFromNamesRequest();
                                RopGetPropertyIdsFromNamesRequest.Parse(s);
                                ropsList.Add(RopGetPropertyIdsFromNamesRequest);
                                break;
                            case RopIdType.RopGetNamesFromPropertyIds:
                                RopGetNamesFromPropertyIdsRequest RopGetNamesFromPropertyIdsRequest = new RopGetNamesFromPropertyIdsRequest();
                                RopGetNamesFromPropertyIdsRequest.Parse(s);
                                ropsList.Add(RopGetNamesFromPropertyIdsRequest);
                                break;
                            case RopIdType.RopOpenStream:
                                RopOpenStreamRequest RopOpenStreamRequest = new RopOpenStreamRequest();
                                RopOpenStreamRequest.Parse(s);
                                ropsList.Add(RopOpenStreamRequest);
                                break;
                            case RopIdType.RopReadStream:
                                RopReadStreamRequest RopReadStreamRequest = new RopReadStreamRequest();
                                RopReadStreamRequest.Parse(s);
                                ropsList.Add(RopReadStreamRequest);
                                break;
                            case RopIdType.RopWriteStream:
                                RopWriteStreamRequest RopWriteStreamRequest = new RopWriteStreamRequest();
                                RopWriteStreamRequest.Parse(s);
                                ropsList.Add(RopWriteStreamRequest);
                                break;
                            case RopIdType.RopWriteStreamExtended:
                                RopWriteStreamExtendedRequest RopWriteStreamExtendedRequest = new RopWriteStreamExtendedRequest();
                                RopWriteStreamExtendedRequest.Parse(s);
                                ropsList.Add(RopWriteStreamExtendedRequest);
                                break;
                            case RopIdType.RopCommitStream:
                                RopCommitStreamRequest RopCommitStreamRequest = new RopCommitStreamRequest();
                                RopCommitStreamRequest.Parse(s);
                                ropsList.Add(RopCommitStreamRequest);
                                break;
                            case RopIdType.RopGetStreamSize:
                                RopGetStreamSizeRequest RopGetStreamSizeRequest = new RopGetStreamSizeRequest();
                                RopGetStreamSizeRequest.Parse(s);
                                ropsList.Add(RopGetStreamSizeRequest);
                                break;
                            case RopIdType.RopSetStreamSize:
                                RopSetStreamSizeRequest RopSetStreamSizeRequest = new RopSetStreamSizeRequest();
                                RopSetStreamSizeRequest.Parse(s);
                                ropsList.Add(RopSetStreamSizeRequest);
                                break;
                            case RopIdType.RopSeekStream:
                                RopSeekStreamRequest RopSeekStreamRequest = new RopSeekStreamRequest();
                                RopSeekStreamRequest.Parse(s);
                                ropsList.Add(RopSeekStreamRequest);
                                break;
                            case RopIdType.RopCopyToStream:
                                RopCopyToStreamRequest RopCopyToStreamRequest = new RopCopyToStreamRequest();
                                RopCopyToStreamRequest.Parse(s);
                                ropsList.Add(RopCopyToStreamRequest);
                                break;
                            case RopIdType.RopProgress:
                                RopProgressRequest RopProgressRequest = new RopProgressRequest();
                                RopProgressRequest.Parse(s);
                                ropsList.Add(RopProgressRequest);
                                break;
                            case RopIdType.RopLockRegionStream:
                                RopLockRegionStreamRequest RopLockRegionStreamRequest = new RopLockRegionStreamRequest();
                                RopLockRegionStreamRequest.Parse(s);
                                ropsList.Add(RopLockRegionStreamRequest);
                                break;
                            case RopIdType.RopUnlockRegionStream:
                                RopUnlockRegionStreamRequest RopUnlockRegionStreamRequest = new RopUnlockRegionStreamRequest();
                                RopUnlockRegionStreamRequest.Parse(s);
                                ropsList.Add(RopUnlockRegionStreamRequest);
                                break;
                            case RopIdType.RopWriteAndCommitStream:
                                RopWriteAndCommitStreamRequest RopWriteAndCommitStreamRequest = new RopWriteAndCommitStreamRequest();
                                RopWriteAndCommitStreamRequest.Parse(s);
                                ropsList.Add(RopWriteAndCommitStreamRequest);
                                break;
                            case RopIdType.RopCloneStream:
                                RopCloneStreamRequest RopCloneStreamRequest = new RopCloneStreamRequest();
                                RopCloneStreamRequest.Parse(s);
                                ropsList.Add(RopCloneStreamRequest);
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                RopOpenFolderRequest RopOpenFolderRequest = new RopOpenFolderRequest();
                                RopOpenFolderRequest.Parse(s);
                                ropsList.Add(RopOpenFolderRequest);
                                break;

                            case RopIdType.RopCreateFolder:
                                RopCreateFolderRequest RopCreateFolderRequest = new RopCreateFolderRequest();
                                RopCreateFolderRequest.Parse(s);
                                ropsList.Add(RopCreateFolderRequest);
                                break;

                            case RopIdType.RopDeleteFolder:
                                RopDeleteFolderRequest RopDeleteFolderRequest = new RopDeleteFolderRequest();
                                RopDeleteFolderRequest.Parse(s);
                                ropsList.Add(RopDeleteFolderRequest);
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                RopSetSearchCriteriaRequest RopSetSearchCriteriaRequest = new RopSetSearchCriteriaRequest();
                                RopSetSearchCriteriaRequest.Parse(s);
                                ropsList.Add(RopSetSearchCriteriaRequest);
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                RopGetSearchCriteriaRequest RopGetSearchCriteriaRequest = new RopGetSearchCriteriaRequest();
                                RopGetSearchCriteriaRequest.Parse(s);
                                ropsList.Add(RopGetSearchCriteriaRequest);
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                RopMoveCopyMessagesRequest RopMoveCopyMessagesRequest = new RopMoveCopyMessagesRequest();
                                RopMoveCopyMessagesRequest.Parse(s);
                                ropsList.Add(RopMoveCopyMessagesRequest);
                                break;

                            case RopIdType.RopMoveFolder:
                                RopMoveFolderRequest RopMoveFolderRequest = new RopMoveFolderRequest();
                                RopMoveFolderRequest.Parse(s);
                                ropsList.Add(RopMoveFolderRequest);
                                break;

                            case RopIdType.RopCopyFolder:
                                RopCopyFolderRequest RopCopyFolderRequest = new RopCopyFolderRequest();
                                RopCopyFolderRequest.Parse(s);
                                ropsList.Add(RopCopyFolderRequest);
                                break;

                            case RopIdType.RopEmptyFolder:
                                RopEmptyFolderRequest RopEmptyFolderRequest = new RopEmptyFolderRequest();
                                RopEmptyFolderRequest.Parse(s);
                                ropsList.Add(RopEmptyFolderRequest);
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                RopHardDeleteMessagesAndSubfoldersRequest RopHardDeleteMessagesAndSubfoldersRequest = new RopHardDeleteMessagesAndSubfoldersRequest();
                                RopHardDeleteMessagesAndSubfoldersRequest.Parse(s);
                                ropsList.Add(RopHardDeleteMessagesAndSubfoldersRequest);
                                break;

                            case RopIdType.RopDeleteMessages:
                                RopDeleteMessagesRequest RopDeleteMessagesRequest = new RopDeleteMessagesRequest();
                                RopDeleteMessagesRequest.Parse(s);
                                ropsList.Add(RopDeleteMessagesRequest);
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                RopHardDeleteMessagesRequest RopHardDeleteMessagesRequest = new RopHardDeleteMessagesRequest();
                                RopHardDeleteMessagesRequest.Parse(s);
                                ropsList.Add(RopHardDeleteMessagesRequest);
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                RopGetHierarchyTableRequest RopGetHierarchyTableRequest = new RopGetHierarchyTableRequest();
                                RopGetHierarchyTableRequest.Parse(s);
                                ropsList.Add(RopGetHierarchyTableRequest);
                                break;

                            case RopIdType.RopGetContentsTable:
                                RopGetContentsTableRequest RopGetContentsTableRequest = new RopGetContentsTableRequest();
                                RopGetContentsTableRequest.Parse(s);
                                ropsList.Add(RopGetContentsTableRequest);
                                break;

                            //MS-OXCMSG ROPs
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
                                byte ropId = ReadByte();
                                byte logId = ReadByte();
                                s.Position -= 2;
                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id)
                                    && DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIInspector.parsingSession.id].ContainsKey(logId)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopSetMessageReadFlag", (ushort)CurrentByte, new uint[] { logId });
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
                                RopRegisterNotificationRequest RopRegisterNotificationRequest = new RopRegisterNotificationRequest();
                                RopRegisterNotificationRequest.Parse(s);
                                ropsList.Add(RopRegisterNotificationRequest);
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                RopGetPermissionsTableRequest RopGetPermissionsTableRequest = new RopGetPermissionsTableRequest();
                                RopGetPermissionsTableRequest.Parse(s);
                                ropsList.Add(RopGetPermissionsTableRequest);
                                break;

                            case RopIdType.RopModifyPermissions:
                                RopModifyPermissionsRequest RopModifyPermissionsRequest = new RopModifyPermissionsRequest();
                                RopModifyPermissionsRequest.Parse(s);
                                ropsList.Add(RopModifyPermissionsRequest);
                                break;

                            default:
                                object RopsBytes = ReadBytes(this.RopSize - (ushort)s.Position);
                                ropsList.Add(RopsBytes);
                                break;
                        }
                        if ((RopIdType)CurrentByte != RopIdType.RopRelease)
                        {
                            RopRemainSize.Add(this.RopSize - (uint)s.Position);
                        }

                    } while (s.Position < this.RopSize);
                }
                else
                {
                    this.RopsList = null;
                }

                if (DecodingContext.SessionRequestRemainSize.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id))
                {
                    DecodingContext.SessionRequestRemainSize.Remove(MapiInspector.MAPIInspector.parsingSession.id);
                }
                DecodingContext.SessionRequestRemainSize.Add(MapiInspector.MAPIInspector.parsingSession.id, RopRemainSize);

                this.RopsList = ropsList.ToArray();
            }
            else
            {
                byte[] ropListBytes = ReadBytes(this.RopSize - 2);
                ropsList.AddRange(ropListBytes.Cast<object>().ToArray());
            }
            this.RopsList = ropsList.ToArray();
            while (s.Position < s.Length)
            {
                uint ServerObjectHandle = ReadUint();
                serverObjectHandleTable.Add(ServerObjectHandle);
            }
            this.ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
    #endregion

    #region ROP Input Buffer
    /// <summary>
    ///  A class indicates the ROP output buffer, which is sent by the server, includes an array of ROP response buffers. 
    /// </summary>
    public class ROPInputBuffer_WithoutCROPS : BaseStructure
    {
        // An unsigned integer that specifies the size of both this field and the RopsList field.
        public ushort RopSize;

        // An array of ROP request buffers.
        public byte[] RopsList;

        // An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPInputBuffer_WithoutCROPS structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPInputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            byte[] ropListBytes = ReadBytes(this.RopSize - 2);
            this.RopsList = ropListBytes;
            while (s.Position < s.Length)
            {
                uint ServerObjectHandle = ReadUint();
                serverObjectHandleTable.Add(ServerObjectHandle);
            }
            this.ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
    #endregion

    #region ROP Output Buffer
    /// <summary>
    ///  A class indicates the ROP output buffer, which is sent by the server, includes an array of ROP response buffers. 
    /// </summary>
    public class ROPOutputBuffer_WithoutCROPS : BaseStructure
    {
        // An unsigned integer that specifies the size of both this field and the RopsList field.
        public ushort RopSize;

        // An array of ROP request buffers.
        public byte[] RopsList;

        // An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPOutputBuffer_WithoutCROPS structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPOutputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            byte[] ropListBytes = ReadBytes(this.RopSize - 2);
            //ropsList.AddRange(ropListBytes.Cast<object>().ToArray());
            this.RopsList = ropListBytes;
            while (s.Position < s.Length)
            {
                uint ServerObjectHandle = ReadUint();
                serverObjectHandleTable.Add(ServerObjectHandle);
            }
            this.ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
    #endregion

    #region ROP Output Buffer
    /// <summary>
    ///  A class indicates the ROP output buffer, which is sent by the server, includes an array of ROP response buffers. 
    /// </summary>
    public class ROPOutputBuffer : BaseStructure
    {
        // An unsigned integer that specifies the size of both this field and the RopsList field.
        public ushort RopSize;

        // An array of ROP response buffers.
        public object[] RopsList;

        // An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPOutputBuffer structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPOutputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            bool ParseToCROPSResponseLayer = false;
            this.RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            List<uint> tempServerObjectHandleTable = new List<uint>();
            long currentPosition = s.Position;
            s.Position += (this.RopSize - 2);
            while (s.Position < s.Length)
            {
                uint serverObjectTable = ReadUint();

                if (MapiInspector.MAPIInspector.targetHandle.Count > 0)
                {
                    MapiInspector.MAPIInspector.isLooperCall = true;
                    Dictionary<int, uint> item = new Dictionary<int, uint>();
                    item = MapiInspector.MAPIInspector.targetHandle.Peek();
                    if (item.First().Value == serverObjectTable)
                    {
                        ParseToCROPSResponseLayer = true;
                    }
                }
                else
                {
                    MapiInspector.MAPIInspector.isLooperCall = false;
                }
                tempServerObjectHandleTable.Add(serverObjectTable);
            }
            s.Position = currentPosition;

            if (!MapiInspector.MAPIInspector.isLooperCall || ParseToCROPSResponseLayer || MapiInspector.MAPIInspector.needToParseCROPSLayer)
            {

                // empty intermediate variables for rops need context information 
                DecodingContext.SetColumn_InputHandles_InResponse = new List<uint>();
                if (this.RopSize > 2)
                {
                    do
                    {
                        int CurrentByte = s.ReadByte();
                        s.Position -= 1;
                        switch ((RopIdType)CurrentByte)
                        {
                            // MS-OXCSTOR ROPs
                            case RopIdType.RopLogon:
                                long currentPos_logon = s.Position;
                                s.Position += 1;
                                int TempOutputHandleIndex_logon = s.ReadByte();
                                s.Position = currentPos_logon;
                                if (!(DecodingContext.SessionLogonFlagsInLogonRop.Count > 0 && DecodingContext.SessionLogonFlagsInLogonRop.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id) && DecodingContext.SessionLogonFlagsInLogonRop[MapiInspector.MAPIInspector.parsingSession.id].ContainsKey((uint)TempOutputHandleIndex_logon)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopLogon", (ushort)CurrentByte);
                                }
                                else
                                {
                                    if (((byte)DecodingContext.SessionLogonFlagsInLogonRop[MapiInspector.MAPIInspector.parsingSession.id][(uint)TempOutputHandleIndex_logon] & 0x01) == (byte)LogonFlags.Private)
                                    {
                                        RopLogonResponse_PrivateMailboxes RopLogonResponse_PrivateMailboxes = new RopLogonResponse_PrivateMailboxes();
                                        RopLogonResponse_PrivateMailboxes.Parse(s);
                                        ropsList.Add(RopLogonResponse_PrivateMailboxes);
                                        break;
                                    }
                                    else
                                    {
                                        RopLogonResponse_PublicFolders RopLogonResponse_PublicFolders = new RopLogonResponse_PublicFolders();
                                        RopLogonResponse_PublicFolders.Parse(s);
                                        ropsList.Add(RopLogonResponse_PublicFolders);
                                        break;
                                    }
                                }
                            case RopIdType.RopGetReceiveFolder:
                                RopGetReceiveFolderResponse RopGetReceiveFolderResponse = new RopGetReceiveFolderResponse();
                                RopGetReceiveFolderResponse.Parse(s);
                                ropsList.Add(RopGetReceiveFolderResponse);
                                break;
                            case RopIdType.RopSetReceiveFolder:
                                RopSetReceiveFolderResponse RopSetReceiveFolderResponse = new RopSetReceiveFolderResponse();
                                RopSetReceiveFolderResponse.Parse(s);
                                ropsList.Add(RopSetReceiveFolderResponse);
                                break;
                            case RopIdType.RopGetReceiveFolderTable:
                                RopGetReceiveFolderTableResponse RopGetReceiveFolderTableResponse = new RopGetReceiveFolderTableResponse();
                                RopGetReceiveFolderTableResponse.Parse(s);
                                ropsList.Add(RopGetReceiveFolderTableResponse);
                                break;
                            case RopIdType.RopGetStoreState:
                                RopGetStoreStateResponse RopGetStoreStateResponse = new RopGetStoreStateResponse();
                                RopGetStoreStateResponse.Parse(s);
                                ropsList.Add(RopGetStoreStateResponse);
                                break;
                            case RopIdType.RopGetOwningServers:
                                RopGetOwningServersResponse RopGetOwningServersResponse = new RopGetOwningServersResponse();
                                RopGetOwningServersResponse.Parse(s);
                                ropsList.Add(RopGetOwningServersResponse);
                                break;
                            case RopIdType.RopPublicFolderIsGhosted:
                                RopPublicFolderIsGhostedResponse RopPublicFolderIsGhostedResponse = new RopPublicFolderIsGhostedResponse();
                                RopPublicFolderIsGhostedResponse.Parse(s);
                                ropsList.Add(RopPublicFolderIsGhostedResponse);
                                break;
                            case RopIdType.RopLongTermIdFromId:
                                RopLongTermIdFromIdResponse RopLongTermIdFromIdResponse = new RopLongTermIdFromIdResponse();
                                RopLongTermIdFromIdResponse.Parse(s);
                                ropsList.Add(RopLongTermIdFromIdResponse);
                                break;
                            case RopIdType.RopIdFromLongTermId:
                                RopIdFromLongTermIdResponse RopIdFromLongTermIdResponse = new RopIdFromLongTermIdResponse();
                                RopIdFromLongTermIdResponse.Parse(s);
                                ropsList.Add(RopIdFromLongTermIdResponse);
                                break;
                            case RopIdType.RopGetPerUserLongTermIds:
                                RopGetPerUserLongTermIdsResponse RopGetPerUserLongTermIdsResponse = new RopGetPerUserLongTermIdsResponse();
                                RopGetPerUserLongTermIdsResponse.Parse(s);
                                ropsList.Add(RopGetPerUserLongTermIdsResponse);
                                break;
                            case RopIdType.RopGetPerUserGuid:
                                RopGetPerUserGuidResponse RopGetPerUserGuidResponse = new RopGetPerUserGuidResponse();
                                RopGetPerUserGuidResponse.Parse(s);
                                ropsList.Add(RopGetPerUserGuidResponse);
                                break;
                            case RopIdType.RopReadPerUserInformation:
                                RopReadPerUserInformationResponse RopReadPerUserInformationResponse = new RopReadPerUserInformationResponse();
                                RopReadPerUserInformationResponse.Parse(s);
                                ropsList.Add(RopReadPerUserInformationResponse);
                                break;
                            case RopIdType.RopWritePerUserInformation:
                                RopWritePerUserInformationResponse RopWritePerUserInformationResponse = new RopWritePerUserInformationResponse();
                                RopWritePerUserInformationResponse.Parse(s);
                                ropsList.Add(RopWritePerUserInformationResponse);
                                break;

                            // MS-OXCROPS ROPs
                            case RopIdType.RopSubmitMessage:
                                RopSubmitMessageResponse RopSubmitMessageResponse = new RopSubmitMessageResponse();
                                RopSubmitMessageResponse.Parse(s);
                                ropsList.Add(RopSubmitMessageResponse);
                                break;
                            case RopIdType.RopAbortSubmit:
                                RopAbortSubmitResponse RopAbortSubmitResponse = new RopAbortSubmitResponse();
                                RopAbortSubmitResponse.Parse(s);
                                ropsList.Add(RopAbortSubmitResponse);
                                break;
                            case RopIdType.RopGetAddressTypes:
                                RopGetAddressTypesResponse RopGetAddressTypesResponse = new RopGetAddressTypesResponse();
                                RopGetAddressTypesResponse.Parse(s);
                                ropsList.Add(RopGetAddressTypesResponse);
                                break;
                            case RopIdType.RopSetSpooler:
                                RopSetSpoolerResponse RopSetSpoolerResponse = new RopSetSpoolerResponse();
                                RopSetSpoolerResponse.Parse(s);
                                ropsList.Add(RopSetSpoolerResponse);
                                break;
                            case RopIdType.RopSpoolerLockMessage:
                                RopSpoolerLockMessageResponse RopSpoolerLockMessageResponse = new RopSpoolerLockMessageResponse();
                                RopSpoolerLockMessageResponse.Parse(s);
                                ropsList.Add(RopSpoolerLockMessageResponse);
                                break;
                            case RopIdType.RopTransportSend:
                                RopTransportSendResponse RopTransportSendResponse = new RopTransportSendResponse();
                                RopTransportSendResponse.Parse(s);
                                ropsList.Add(RopTransportSendResponse);
                                break;
                            case RopIdType.RopTransportNewMail:
                                RopTransportNewMailResponse RopTransportNewMailResponse = new RopTransportNewMailResponse();
                                RopTransportNewMailResponse.Parse(s);
                                ropsList.Add(RopTransportNewMailResponse);
                                break;
                            case RopIdType.RopGetTransportFolder:
                                RopGetTransportFolderResponse RopGetTransportFolderResponse = new RopGetTransportFolderResponse();
                                RopGetTransportFolderResponse.Parse(s);
                                ropsList.Add(RopGetTransportFolderResponse);
                                break;
                            case RopIdType.RopOptionsData:
                                RopOptionsDataResponse RopOptionsDataResponse = new RopOptionsDataResponse();
                                RopOptionsDataResponse.Parse(s);
                                ropsList.Add(RopOptionsDataResponse);
                                break;
                            case RopIdType.RopBackoff:
                                RopBackoffResponse RopBackoffResponse = new RopBackoffResponse();
                                RopBackoffResponse.Parse(s);
                                ropsList.Add(RopBackoffResponse);
                                break;
                            case RopIdType.RopBufferTooSmall:
                                if (DecodingContext.SessionRequestRemainSize.Count > 0 && DecodingContext.SessionRequestRemainSize.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id))
                                {
                                    uint RequestBuffersSize = 0;
                                    int RopCountInResponse = ropsList.Count;
                                    if (DecodingContext.SessionRequestRemainSize[MapiInspector.MAPIInspector.parsingSession.id].Count > RopCountInResponse)
                                    {
                                        RequestBuffersSize = DecodingContext.SessionRequestRemainSize[MapiInspector.MAPIInspector.parsingSession.id][RopCountInResponse];
                                    }
                                    RopBufferTooSmallResponse RopBufferTooSmallResponse = new RopBufferTooSmallResponse(RequestBuffersSize);
                                    RopBufferTooSmallResponse.Parse(s);
                                    ropsList.Add(RopBufferTooSmallResponse);
                                    break;
                                }
                                else
                                {
                                    throw new MissingInformationException("Missing RequestBuffersSize information for RopBufferTooSmall", (ushort)CurrentByte);
                                }
                            // MSOXCTABL ROPs
                            case RopIdType.RopSetColumns:
                                RopSetColumnsResponse RopSetColumnsResponse = new RopSetColumnsResponse();
                                RopSetColumnsResponse.Parse(s);
                                ropsList.Add(RopSetColumnsResponse);
                                if (!(DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 && DecodingContext.SetColumn_InputHandles_InResponse.Contains(tempServerObjectHandleTable[RopSetColumnsResponse.InputHandleIndex])))
                                {
                                    DecodingContext.SetColumn_InputHandles_InResponse.Add(tempServerObjectHandleTable[RopSetColumnsResponse.InputHandleIndex]);
                                }
                                break;

                            case RopIdType.RopSortTable:
                                RopSortTableResponse RopSortTableResponse = new RopSortTableResponse();
                                RopSortTableResponse.Parse(s);
                                ropsList.Add(RopSortTableResponse);
                                break;

                            case RopIdType.RopRestrict:
                                RopRestrictResponse RopRestrictResponse = new RopRestrictResponse();
                                RopRestrictResponse.Parse(s);
                                ropsList.Add(RopRestrictResponse);
                                break;

                            case RopIdType.RopQueryRows:
                                long currentPos = s.Position;
                                s.Position += 1;
                                int TempInputHandleIndex_QueryRow = s.ReadByte();
                                UInt32 returnValue_queryRow = ReadUint();
                                s.Position = currentPos;
                                if (returnValue_queryRow == 0)
                                {
                                    if (!(DecodingContext.RowRops_propertyTags.Count > 0 && DecodingContext.RowRops_propertyTags.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id) && DecodingContext.RowRops_propertyTags[MapiInspector.MAPIInspector.parsingSession.id].ContainsKey(tempServerObjectHandleTable[TempInputHandleIndex_QueryRow])))
                                    {
                                        throw new MissingInformationException("Missing PropertyTags information for RopQueryRowsResponse", (ushort)RopIdType.RopQueryRows, new uint[] { (uint)TempInputHandleIndex_QueryRow, tempServerObjectHandleTable[TempInputHandleIndex_QueryRow] });
                                    }
                                    RopQueryRowsResponse RopQueryRowsResponse = new RopQueryRowsResponse(DecodingContext.RowRops_propertyTags[MapiInspector.MAPIInspector.parsingSession.id][tempServerObjectHandleTable[TempInputHandleIndex_QueryRow]]);
                                    RopQueryRowsResponse.Parse(s);
                                    ropsList.Add(RopQueryRowsResponse);
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
                                RopAbortResponse RopAbortResponse = new RopAbortResponse();
                                RopAbortResponse.Parse(s);
                                ropsList.Add(RopAbortResponse);
                                break;

                            case RopIdType.RopGetStatus:
                                RopGetStatusResponse RopGetStatusResponse = new RopGetStatusResponse();
                                RopGetStatusResponse.Parse(s);
                                ropsList.Add(RopGetStatusResponse);
                                break;

                            case RopIdType.RopQueryPosition:
                                RopQueryPositionResponse RopQueryPositionResponse = new RopQueryPositionResponse();
                                RopQueryPositionResponse.Parse(s);
                                ropsList.Add(RopQueryPositionResponse);
                                break;

                            case RopIdType.RopSeekRow:
                                RopSeekRowResponse RopSeekRowResponse = new RopSeekRowResponse();
                                RopSeekRowResponse.Parse(s);
                                ropsList.Add(RopSeekRowResponse);
                                break;

                            case RopIdType.RopSeekRowBookmark:
                                RopSeekRowBookmarkResponse RopSeekRowBookmarkResponse = new RopSeekRowBookmarkResponse();
                                RopSeekRowBookmarkResponse.Parse(s);
                                ropsList.Add(RopSeekRowBookmarkResponse);
                                break;

                            case RopIdType.RopSeekRowFractional:
                                RopSeekRowFractionalResponse RopSeekRowFractionalResponse = new RopSeekRowFractionalResponse();
                                RopSeekRowFractionalResponse.Parse(s);
                                ropsList.Add(RopSeekRowFractionalResponse);
                                break;

                            case RopIdType.RopCreateBookmark:
                                RopCreateBookmarkResponse RopCreateBookmarkResponse = new RopCreateBookmarkResponse();
                                RopCreateBookmarkResponse.Parse(s);
                                ropsList.Add(RopCreateBookmarkResponse);
                                break;

                            case RopIdType.RopQueryColumnsAll:
                                RopQueryColumnsAllResponse RopQueryColumnsAllResponse = new RopQueryColumnsAllResponse();
                                RopQueryColumnsAllResponse.Parse(s);
                                ropsList.Add(RopQueryColumnsAllResponse);
                                break;

                            case RopIdType.RopFindRow:
                                long currentPos_findRow = s.Position;
                                s.Position += 1;
                                int TempInputHandleIndex_findRow = s.ReadByte();
                                UInt32 returnValue_findRow = ReadUint();
                                s.Position = currentPos_findRow;
                                if (returnValue_findRow == 0)
                                {
                                    if (!(DecodingContext.RowRops_propertyTags.Count > 0 && DecodingContext.RowRops_propertyTags.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id) && DecodingContext.RowRops_propertyTags[MapiInspector.MAPIInspector.parsingSession.id].ContainsKey(tempServerObjectHandleTable[TempInputHandleIndex_findRow])))
                                    {
                                        throw new MissingInformationException("Missing PropertyTags information for RopFindRowsResponse", (ushort)RopIdType.RopFindRow, new uint[] { (uint)TempInputHandleIndex_findRow, tempServerObjectHandleTable[TempInputHandleIndex_findRow] });
                                    }
                                    RopFindRowResponse ropFindRowResponse = new RopFindRowResponse(DecodingContext.RowRops_propertyTags[MapiInspector.MAPIInspector.parsingSession.id][tempServerObjectHandleTable[TempInputHandleIndex_findRow]]);
                                    ropFindRowResponse.Parse(s);
                                    ropsList.Add(ropFindRowResponse);
                                    break;

                                }
                                else
                                {
                                    RopFindRowResponse RopFindRowResponse = new RopFindRowResponse(null);
                                    RopFindRowResponse.Parse(s);
                                    ropsList.Add(RopFindRowResponse);
                                    break;
                                }

                            case RopIdType.RopFreeBookmark:
                                RopFreeBookmarkResponse RopFreeBookmarkResponse = new RopFreeBookmarkResponse();
                                RopFreeBookmarkResponse.Parse(s);
                                ropsList.Add(RopFreeBookmarkResponse);
                                break;

                            case RopIdType.RopResetTable:
                                RopResetTableResponse RopResetTableResponse = new RopResetTableResponse();
                                RopResetTableResponse.Parse(s);
                                ropsList.Add(RopResetTableResponse);
                                break;

                            case RopIdType.RopExpandRow:
                                long currentPos_expandRow = s.Position;
                                s.Position += 1;
                                int TempInputHandleIndex_expandRow = s.ReadByte();
                                UInt32 returnValue_expandRow = ReadUint();
                                s.Position = currentPos_expandRow;
                                if (returnValue_expandRow == 0)
                                {
                                    if (!(DecodingContext.RowRops_propertyTags.Count > 0 && DecodingContext.RowRops_propertyTags.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id) && DecodingContext.RowRops_propertyTags[MapiInspector.MAPIInspector.parsingSession.id].ContainsKey(tempServerObjectHandleTable[TempInputHandleIndex_expandRow])))
                                    {
                                        throw new MissingInformationException("Missing PropertyTags information for RopExpandRowsResponse", (ushort)RopIdType.RopExpandRow, new uint[] { (uint)TempInputHandleIndex_expandRow, tempServerObjectHandleTable[TempInputHandleIndex_expandRow] });
                                    }
                                    RopExpandRowResponse ropFindRowResponse = new RopExpandRowResponse(DecodingContext.RowRops_propertyTags[MapiInspector.MAPIInspector.parsingSession.id][tempServerObjectHandleTable[TempInputHandleIndex_expandRow]]);
                                    ropFindRowResponse.Parse(s);
                                    ropsList.Add(ropFindRowResponse);
                                    break;
                                }
                                else
                                {
                                    RopExpandRowResponse RopFindRowResponse = new RopExpandRowResponse(null);
                                    RopFindRowResponse.Parse(s);
                                    ropsList.Add(RopFindRowResponse);
                                    break;
                                }

                            case RopIdType.RopCollapseRow:
                                RopCollapseRowResponse RopCollapseRowResponse = new RopCollapseRowResponse();
                                RopCollapseRowResponse.Parse(s);
                                ropsList.Add(RopCollapseRowResponse);
                                break;

                            case RopIdType.RopGetCollapseState:
                                RopGetCollapseStateResponse RopGetCollapseStateResponse = new RopGetCollapseStateResponse();
                                RopGetCollapseStateResponse.Parse(s);
                                ropsList.Add(RopGetCollapseStateResponse);
                                break;

                            case RopIdType.RopSetCollapseState:
                                RopSetCollapseStateResponse RopSetCollapseStateResponse = new RopSetCollapseStateResponse();
                                RopSetCollapseStateResponse.Parse(s);
                                ropsList.Add(RopSetCollapseStateResponse);
                                break;

                            // MSOXORULE ROPs
                            case RopIdType.RopModifyRules:
                                RopModifyRulesResponse RopModifyRulesResponse = new RopModifyRulesResponse();
                                RopModifyRulesResponse.Parse(s);
                                ropsList.Add(RopModifyRulesResponse);
                                break;

                            case RopIdType.RopGetRulesTable:
                                RopGetRulesTableResponse RopGetRulesTableResponse = new RopGetRulesTableResponse();
                                RopGetRulesTableResponse.Parse(s);
                                ropsList.Add(RopGetRulesTableResponse);
                                break;

                            case RopIdType.RopUpdateDeferredActionMessages:
                                RopUpdateDeferredActionMessagesResponse RopUpdateDeferredActionMessagesResponse = new RopUpdateDeferredActionMessagesResponse();
                                RopUpdateDeferredActionMessagesResponse.Parse(s);
                                ropsList.Add(RopUpdateDeferredActionMessagesResponse);
                                break;

                            //MS-OXCFXICS ROPs
                            case RopIdType.RopFastTransferSourceCopyProperties:
                                RopFastTransferSourceCopyPropertiesResponse RopFastTransferSourceCopyPropertiesResponse = new RopFastTransferSourceCopyPropertiesResponse();
                                RopFastTransferSourceCopyPropertiesResponse.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyPropertiesResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyTo:
                                RopFastTransferSourceCopyToResponse RopFastTransferSourceCopyToResponse = new RopFastTransferSourceCopyToResponse();
                                RopFastTransferSourceCopyToResponse.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyToResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyMessages:
                                RopFastTransferSourceCopyMessagesResponse RopFastTransferSourceCopyMessagesResponse = new RopFastTransferSourceCopyMessagesResponse();
                                RopFastTransferSourceCopyMessagesResponse.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyMessagesResponse);
                                break;
                            case RopIdType.RopFastTransferSourceCopyFolder:
                                RopFastTransferSourceCopyFolderResponse RopFastTransferSourceCopyFolderResponse = new RopFastTransferSourceCopyFolderResponse();
                                RopFastTransferSourceCopyFolderResponse.Parse(s);
                                ropsList.Add(RopFastTransferSourceCopyFolderResponse);
                                break;
                            case RopIdType.RopFastTransferSourceGetBuffer:
                                long currentPos_getBuffer = s.Position;
                                s.Position += 1;
                                int TempInputHandleIndex_getBuffer = s.ReadByte();
                                uint ReturnValue = ReadUint();
                                ushort status = ReadUshort();
                                s.Position = currentPos_getBuffer;
                                uint parsingSessionID = (uint)MapiInspector.MAPIInspector.parsingSession.id;
                                uint ropGetbufferHandle = tempServerObjectHandleTable[TempInputHandleIndex_getBuffer];
                                MapiInspector.MAPIInspector.PartialContextInformation[] PartialBeforeAndAfterInformation = new MapiInspector.MAPIInspector.PartialContextInformation[2];
                                if (ReturnValue == 0)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey((int)parsingSessionID))
                                    {
                                        throw new MissingPartialInformationException((ushort)CurrentByte, ropGetbufferHandle);
                                    }
                                }

                                RopFastTransferSourceGetBufferResponse RopFastTransferSourceGetBufferResponse = new RopFastTransferSourceGetBufferResponse();
                                MapiInspector.MAPIInspector.isGet = true;
                                RopFastTransferSourceGetBufferResponse.Parse(s);
                                ropsList.Add(RopFastTransferSourceGetBufferResponse);
                                MapiInspector.MAPIInspector.PartialContextInformation getBufferPartialInformaiton = new MapiInspector.MAPIInspector.PartialContextInformation(MapiInspector.MAPIInspector.pGetType, MapiInspector.MAPIInspector.pGetId, MapiInspector.MAPIInspector.pGetRemainSize, MapiInspector.MAPIInspector.pGetSubRemainSize, true, MapiInspector.MAPIInspector.outputPayLoadCompresssedXOR);
                                SortedDictionary<uint, MapiInspector.MAPIInspector.PartialContextInformation> sessionGetContextInfor = new SortedDictionary<uint, MapiInspector.MAPIInspector.PartialContextInformation>();

                                if (MapiInspector.MAPIInspector.handleWithSessionGetContextInformation.ContainsKey(ropGetbufferHandle))
                                {
                                    sessionGetContextInfor = MapiInspector.MAPIInspector.handleWithSessionGetContextInformation[ropGetbufferHandle];
                                    MapiInspector.MAPIInspector.handleWithSessionGetContextInformation.Remove(ropGetbufferHandle);
                                }
                                if (sessionGetContextInfor.ContainsKey(parsingSessionID))
                                {
                                    sessionGetContextInfor[parsingSessionID] = getBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionGetContextInfor.Add(parsingSessionID, getBufferPartialInformaiton);
                                }
                                MapiInspector.MAPIInspector.handleWithSessionGetContextInformation.Add(ropGetbufferHandle, sessionGetContextInfor);
                                MapiInspector.MAPIInspector.isGet = false;
                                break;
                            case RopIdType.RopTellVersion:
                                RopTellVersionResponse RopTellVersionResponse = new RopTellVersionResponse();
                                RopTellVersionResponse.Parse(s);
                                ropsList.Add(RopTellVersionResponse);
                                break;
                            case RopIdType.RopSynchronizationGetTransferState:
                                RopSynchronizationGetTransferStateResponse RopSynchronizationGetTransferStateResponse = new RopSynchronizationGetTransferStateResponse();
                                RopSynchronizationGetTransferStateResponse.Parse(s);
                                ropsList.Add(RopSynchronizationGetTransferStateResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationConfigure:
                                RopFastTransferDestinationConfigureResponse RopFastTransferDestinationConfigureResponse = new RopFastTransferDestinationConfigureResponse();
                                RopFastTransferDestinationConfigureResponse.Parse(s);
                                ropsList.Add(RopFastTransferDestinationConfigureResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBuffer:
                                RopFastTransferDestinationPutBufferResponse RopFastTransferDestinationPutBufferResponse = new RopFastTransferDestinationPutBufferResponse();
                                RopFastTransferDestinationPutBufferResponse.Parse(s);
                                ropsList.Add(RopFastTransferDestinationPutBufferResponse);
                                break;
                            case RopIdType.RopFastTransferDestinationPutBufferExtended:
                                RopFastTransferDestinationPutBufferExtendedResponse RopFastTransferDestinationPutBufferExtendedResponse = new RopFastTransferDestinationPutBufferExtendedResponse();
                                RopFastTransferDestinationPutBufferExtendedResponse.Parse(s);
                                ropsList.Add(RopFastTransferDestinationPutBufferExtendedResponse);
                                break;
                            case RopIdType.RopSynchronizationConfigure:
                                RopSynchronizationConfigureResponse RopSynchronizationConfigureResponse = new RopSynchronizationConfigureResponse();
                                RopSynchronizationConfigureResponse.Parse(s);
                                ropsList.Add(RopSynchronizationConfigureResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamBegin:
                                RopSynchronizationUploadStateStreamBeginResponse RopSynchronizationUploadStateStreamBeginResponse = new RopSynchronizationUploadStateStreamBeginResponse();
                                RopSynchronizationUploadStateStreamBeginResponse.Parse(s);
                                ropsList.Add(RopSynchronizationUploadStateStreamBeginResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamContinue:
                                RopSynchronizationUploadStateStreamContinueResponse RopSynchronizationUploadStateStreamContinueResponse = new RopSynchronizationUploadStateStreamContinueResponse();
                                RopSynchronizationUploadStateStreamContinueResponse.Parse(s);
                                ropsList.Add(RopSynchronizationUploadStateStreamContinueResponse);
                                break;
                            case RopIdType.RopSynchronizationUploadStateStreamEnd:
                                RopSynchronizationUploadStateStreamEndResponse RopSynchronizationUploadStateStreamEndResponse = new RopSynchronizationUploadStateStreamEndResponse();
                                RopSynchronizationUploadStateStreamEndResponse.Parse(s);
                                ropsList.Add(RopSynchronizationUploadStateStreamEndResponse);
                                break;
                            case RopIdType.RopSynchronizationOpenCollector:
                                RopSynchronizationOpenCollectorResponse RopSynchronizationOpenCollectorResponse = new RopSynchronizationOpenCollectorResponse();
                                RopSynchronizationOpenCollectorResponse.Parse(s);
                                ropsList.Add(RopSynchronizationOpenCollectorResponse);
                                break;
                            case RopIdType.RopSynchronizationImportMessageChange:
                                RopSynchronizationImportMessageChangeResponse RopSynchronizationImportMessageChangeResponse = new RopSynchronizationImportMessageChangeResponse();
                                RopSynchronizationImportMessageChangeResponse.Parse(s);
                                ropsList.Add(RopSynchronizationImportMessageChangeResponse);
                                break;
                            case RopIdType.RopSynchronizationImportHierarchyChange:
                                RopSynchronizationImportHierarchyChangeResponse RopSynchronizationImportHierarchyChangeResponse = new RopSynchronizationImportHierarchyChangeResponse();
                                RopSynchronizationImportHierarchyChangeResponse.Parse(s);
                                ropsList.Add(RopSynchronizationImportHierarchyChangeResponse);
                                break;
                            case RopIdType.RopSynchronizationImportMessageMove:
                                RopSynchronizationImportMessageMoveResponse RopSynchronizationImportMessageMoveResponse = new RopSynchronizationImportMessageMoveResponse();
                                RopSynchronizationImportMessageMoveResponse.Parse(s);
                                ropsList.Add(RopSynchronizationImportMessageMoveResponse);
                                break;
                            case RopIdType.RopSynchronizationImportDeletes:
                                RopSynchronizationImportDeletesResponse RopSynchronizationImportDeletesResponse = new RopSynchronizationImportDeletesResponse();
                                RopSynchronizationImportDeletesResponse.Parse(s);
                                ropsList.Add(RopSynchronizationImportDeletesResponse);
                                break;
                            case RopIdType.RopSynchronizationImportReadStateChanges:
                                RopSynchronizationImportReadStateChangesResponse RopSynchronizationImportReadStateChangesResponse = new RopSynchronizationImportReadStateChangesResponse();
                                RopSynchronizationImportReadStateChangesResponse.Parse(s);
                                ropsList.Add(RopSynchronizationImportReadStateChangesResponse);
                                break;
                            case RopIdType.RopGetLocalReplicaIds:
                                RopGetLocalReplicaIdsResponse RopGetLocalReplicaIdsResponse = new RopGetLocalReplicaIdsResponse();
                                RopGetLocalReplicaIdsResponse.Parse(s);
                                ropsList.Add(RopGetLocalReplicaIdsResponse);
                                break;
                            case RopIdType.RopSetLocalReplicaMidsetDeleted:
                                RopSetLocalReplicaMidsetDeletedResponse RopSetLocalReplicaMidsetDeletedResponse = new RopSetLocalReplicaMidsetDeletedResponse();
                                RopSetLocalReplicaMidsetDeletedResponse.Parse(s);
                                ropsList.Add(RopSetLocalReplicaMidsetDeletedResponse);
                                break;

                            // MS-OXCPRPT ROPs
                            case RopIdType.RopGetPropertiesSpecific:
                                long currentPos_getPropertiesSpec = s.Position;
                                s.Position += 1;
                                int TempInputHandleIndex_getPropertiesSpec = s.ReadByte();
                                s.Position = currentPos_getPropertiesSpec;
                                if (!(DecodingContext.GetPropertiesSpec_propertyTags.Count > 0 && DecodingContext.GetPropertiesSpec_propertyTags.ContainsKey(MapiInspector.MAPIInspector.parsingSession.id) && DecodingContext.GetPropertiesSpec_propertyTags[MapiInspector.MAPIInspector.parsingSession.id].ContainsKey((uint)TempInputHandleIndex_getPropertiesSpec)
                                    && DecodingContext.GetPropertiesSpec_propertyTags[MapiInspector.MAPIInspector.parsingSession.id][(uint)TempInputHandleIndex_getPropertiesSpec].Count != 0))
                                {
                                    throw new MissingInformationException("Missing PropertyTags information for RopGetPropertiesSpecific", (ushort)CurrentByte);
                                }
                                RopGetPropertiesSpecificResponse RopGetPropertiesSpecificResponse = new RopGetPropertiesSpecificResponse();
                                RopGetPropertiesSpecificResponse.Parse(s);
                                ropsList.Add(RopGetPropertiesSpecificResponse);
                                break;
                            case RopIdType.RopGetPropertiesAll:
                                RopGetPropertiesAllResponse RopGetPropertiesAllResponse = new RopGetPropertiesAllResponse();
                                RopGetPropertiesAllResponse.Parse(s);
                                ropsList.Add(RopGetPropertiesAllResponse);
                                break;
                            case RopIdType.RopGetPropertiesList:
                                RopGetPropertiesListResponse RopGetPropertiesListResponse = new RopGetPropertiesListResponse();
                                RopGetPropertiesListResponse.Parse(s);
                                ropsList.Add(RopGetPropertiesListResponse);
                                break;
                            case RopIdType.RopSetProperties:
                                RopSetPropertiesResponse RopSetPropertiesResponse = new RopSetPropertiesResponse();
                                RopSetPropertiesResponse.Parse(s);
                                ropsList.Add(RopSetPropertiesResponse);
                                break;
                            case RopIdType.RopSetPropertiesNoReplicate:
                                RopSetPropertiesNoReplicateResponse RopSetPropertiesNoReplicateResponse = new RopSetPropertiesNoReplicateResponse();
                                RopSetPropertiesNoReplicateResponse.Parse(s);
                                ropsList.Add(RopSetPropertiesNoReplicateResponse);
                                break;
                            case RopIdType.RopDeleteProperties:
                                RopDeletePropertiesResponse RopDeletePropertiesResponse = new RopDeletePropertiesResponse();
                                RopDeletePropertiesResponse.Parse(s);
                                ropsList.Add(RopDeletePropertiesResponse);
                                break;
                            case RopIdType.RopDeletePropertiesNoReplicate:
                                RopDeletePropertiesNoReplicateResponse RopDeletePropertiesNoReplicateResponse = new RopDeletePropertiesNoReplicateResponse();
                                RopDeletePropertiesNoReplicateResponse.Parse(s);
                                ropsList.Add(RopDeletePropertiesNoReplicateResponse);
                                break;
                            case RopIdType.RopQueryNamedProperties:
                                RopQueryNamedPropertiesResponse RopQueryNamedPropertiesResponse = new RopQueryNamedPropertiesResponse();
                                RopQueryNamedPropertiesResponse.Parse(s);
                                ropsList.Add(RopQueryNamedPropertiesResponse);
                                break;
                            case RopIdType.RopCopyProperties:
                                RopCopyPropertiesResponse RopCopyPropertiesResponse = new RopCopyPropertiesResponse();
                                RopCopyPropertiesResponse.Parse(s);
                                ropsList.Add(RopCopyPropertiesResponse);
                                break;
                            case RopIdType.RopCopyTo:
                                RopCopyToResponse RopCopyToResponse = new RopCopyToResponse();
                                RopCopyToResponse.Parse(s);
                                ropsList.Add(RopCopyToResponse);
                                break;
                            case RopIdType.RopGetPropertyIdsFromNames:
                                RopGetPropertyIdsFromNamesResponse RopGetPropertyIdsFromNamesResponse = new RopGetPropertyIdsFromNamesResponse();
                                RopGetPropertyIdsFromNamesResponse.Parse(s);
                                ropsList.Add(RopGetPropertyIdsFromNamesResponse);
                                break;
                            case RopIdType.RopGetNamesFromPropertyIds:
                                RopGetNamesFromPropertyIdsResponse RopGetNamesFromPropertyIdsResponse = new RopGetNamesFromPropertyIdsResponse();
                                RopGetNamesFromPropertyIdsResponse.Parse(s);
                                ropsList.Add(RopGetNamesFromPropertyIdsResponse);
                                break;
                            case RopIdType.RopOpenStream:
                                RopOpenStreamResponse RopOpenStreamResponse = new RopOpenStreamResponse();
                                RopOpenStreamResponse.Parse(s);
                                ropsList.Add(RopOpenStreamResponse);
                                break;
                            case RopIdType.RopReadStream:
                                RopReadStreamResponse RopReadStreamResponse = new RopReadStreamResponse();
                                RopReadStreamResponse.Parse(s);
                                ropsList.Add(RopReadStreamResponse);
                                break;
                            case RopIdType.RopWriteStream:
                                RopWriteStreamResponse RopWriteStreamResponse = new RopWriteStreamResponse();
                                RopWriteStreamResponse.Parse(s);
                                ropsList.Add(RopWriteStreamResponse);
                                break;
                            case RopIdType.RopWriteStreamExtended:
                                RopWriteStreamExtendedResponse RopWriteStreamExtendedResponse = new RopWriteStreamExtendedResponse();
                                RopWriteStreamExtendedResponse.Parse(s);
                                ropsList.Add(RopWriteStreamExtendedResponse);
                                break;
                            case RopIdType.RopCommitStream:
                                RopCommitStreamResponse RopCommitStreamResponse = new RopCommitStreamResponse();
                                RopCommitStreamResponse.Parse(s);
                                ropsList.Add(RopCommitStreamResponse);
                                break;
                            case RopIdType.RopGetStreamSize:
                                RopGetStreamSizeResponse RopGetStreamSizeResponse = new RopGetStreamSizeResponse();
                                RopGetStreamSizeResponse.Parse(s);
                                ropsList.Add(RopGetStreamSizeResponse);
                                break;
                            case RopIdType.RopSetStreamSize:
                                RopSetStreamSizeResponse RopSetStreamSizeResponse = new RopSetStreamSizeResponse();
                                RopSetStreamSizeResponse.Parse(s);
                                ropsList.Add(RopSetStreamSizeResponse);
                                break;
                            case RopIdType.RopSeekStream:
                                RopSeekStreamResponse RopSeekStreamResponse = new RopSeekStreamResponse();
                                RopSeekStreamResponse.Parse(s);
                                ropsList.Add(RopSeekStreamResponse);
                                break;
                            case RopIdType.RopCopyToStream:
                                RopCopyToStreamResponse RopCopyToStreamResponse = new RopCopyToStreamResponse();
                                RopCopyToStreamResponse.Parse(s);
                                ropsList.Add(RopCopyToStreamResponse);
                                break;
                            case RopIdType.RopProgress:
                                RopProgressResponse RopProgressResponse = new RopProgressResponse();
                                RopProgressResponse.Parse(s);
                                ropsList.Add(RopProgressResponse);
                                break;
                            case RopIdType.RopLockRegionStream:
                                RopLockRegionStreamResponse RopLockRegionStreamResponse = new RopLockRegionStreamResponse();
                                RopLockRegionStreamResponse.Parse(s);
                                ropsList.Add(RopLockRegionStreamResponse);
                                break;
                            case RopIdType.RopUnlockRegionStream:
                                RopUnlockRegionStreamResponse RopUnlockRegionStreamResponse = new RopUnlockRegionStreamResponse();
                                RopUnlockRegionStreamResponse.Parse(s);
                                ropsList.Add(RopUnlockRegionStreamResponse);
                                break;
                            case RopIdType.RopWriteAndCommitStream:
                                RopWriteAndCommitStreamResponse RopWriteAndCommitStreamResponse = new RopWriteAndCommitStreamResponse();
                                RopWriteAndCommitStreamResponse.Parse(s);
                                ropsList.Add(RopWriteAndCommitStreamResponse);
                                break;
                            case RopIdType.RopCloneStream:
                                RopCloneStreamResponse RopCloneStreamResponse = new RopCloneStreamResponse();
                                RopCloneStreamResponse.Parse(s);
                                ropsList.Add(RopCloneStreamResponse);
                                break;

                            // MSOXCFOLD ROPs
                            case RopIdType.RopOpenFolder:
                                RopOpenFolderResponse RopOpenFolderResponse = new RopOpenFolderResponse();
                                RopOpenFolderResponse.Parse(s);
                                ropsList.Add(RopOpenFolderResponse);
                                if ((ErrorCodes)RopOpenFolderResponse.ReturnValue == ErrorCodes.Success)
                                {
                                    uint handle_OpenFolder = tempServerObjectHandleTable[RopOpenFolderResponse.OutputHandleIndex];
                                    if (DecodingContext.ObjectHandles.Count > 0 && DecodingContext.ObjectHandles.ContainsKey(handle_OpenFolder))
                                    {
                                        DecodingContext.ObjectHandles.Remove(handle_OpenFolder);
                                    }
                                    DecodingContext.ObjectHandles.Add(handle_OpenFolder, ObjectHandlesType.FolderHandles);
                                }
                                break;

                            case RopIdType.RopCreateFolder:
                                RopCreateFolderResponse RopCreateFolderResponse = new RopCreateFolderResponse();
                                RopCreateFolderResponse.Parse(s);
                                ropsList.Add(RopCreateFolderResponse);
                                if ((ErrorCodes)RopCreateFolderResponse.ReturnValue == ErrorCodes.Success)
                                {
                                    uint handle_CreateFolder = tempServerObjectHandleTable[RopCreateFolderResponse.OutputHandleIndex];
                                    if (DecodingContext.ObjectHandles.Count > 0 && DecodingContext.ObjectHandles.ContainsKey(handle_CreateFolder))
                                    {
                                        DecodingContext.ObjectHandles.Remove(handle_CreateFolder);
                                    }
                                    DecodingContext.ObjectHandles.Add(handle_CreateFolder, ObjectHandlesType.FolderHandles);
                                }
                                break;

                            case RopIdType.RopDeleteFolder:
                                RopDeleteFolderResponse RopDeleteFolderResponse = new RopDeleteFolderResponse();
                                RopDeleteFolderResponse.Parse(s);
                                ropsList.Add(RopDeleteFolderResponse);
                                break;

                            case RopIdType.RopSetSearchCriteria:
                                RopSetSearchCriteriaResponse RopSetSearchCriteriaResponse = new RopSetSearchCriteriaResponse();
                                RopSetSearchCriteriaResponse.Parse(s);
                                ropsList.Add(RopSetSearchCriteriaResponse);
                                break;

                            case RopIdType.RopGetSearchCriteria:
                                RopGetSearchCriteriaResponse RopGetSearchCriteriaResponse = new RopGetSearchCriteriaResponse();
                                RopGetSearchCriteriaResponse.Parse(s);
                                ropsList.Add(RopGetSearchCriteriaResponse);
                                break;

                            case RopIdType.RopMoveCopyMessages:
                                RopMoveCopyMessagesResponse RopMoveCopyMessagesResponse = new RopMoveCopyMessagesResponse();
                                RopMoveCopyMessagesResponse.Parse(s);
                                ropsList.Add(RopMoveCopyMessagesResponse);
                                break;

                            case RopIdType.RopMoveFolder:
                                RopMoveFolderResponse RopMoveFolderResponse = new RopMoveFolderResponse();
                                RopMoveFolderResponse.Parse(s);
                                ropsList.Add(RopMoveFolderResponse);
                                break;

                            case RopIdType.RopCopyFolder:
                                RopCopyFolderResponse RopCopyFolderResponse = new RopCopyFolderResponse();
                                RopCopyFolderResponse.Parse(s);
                                ropsList.Add(RopCopyFolderResponse);
                                break;

                            case RopIdType.RopEmptyFolder:
                                RopEmptyFolderResponse RopEmptyFolderResponse = new RopEmptyFolderResponse();
                                RopEmptyFolderResponse.Parse(s);
                                ropsList.Add(RopEmptyFolderResponse);
                                break;

                            case RopIdType.RopHardDeleteMessagesAndSubfolders:
                                RopHardDeleteMessagesAndSubfoldersResponse RopHardDeleteMessagesAndSubfoldersResponse = new RopHardDeleteMessagesAndSubfoldersResponse();
                                RopHardDeleteMessagesAndSubfoldersResponse.Parse(s);
                                ropsList.Add(RopHardDeleteMessagesAndSubfoldersResponse);
                                break;

                            case RopIdType.RopDeleteMessages:
                                RopDeleteMessagesResponse RopDeleteMessagesResponse = new RopDeleteMessagesResponse();
                                RopDeleteMessagesResponse.Parse(s);
                                ropsList.Add(RopDeleteMessagesResponse);
                                break;

                            case RopIdType.RopHardDeleteMessages:
                                RopHardDeleteMessagesResponse RopHardDeleteMessagesResponse = new RopHardDeleteMessagesResponse();
                                RopHardDeleteMessagesResponse.Parse(s);
                                ropsList.Add(RopHardDeleteMessagesResponse);
                                break;

                            case RopIdType.RopGetHierarchyTable:
                                RopGetHierarchyTableResponse RopGetHierarchyTableResponse = new RopGetHierarchyTableResponse();
                                RopGetHierarchyTableResponse.Parse(s);
                                ropsList.Add(RopGetHierarchyTableResponse);
                                break;

                            case RopIdType.RopGetContentsTable:
                                RopGetContentsTableResponse RopGetContentsTableResponse = new RopGetContentsTableResponse();
                                RopGetContentsTableResponse.Parse(s);
                                ropsList.Add(RopGetContentsTableResponse);
                                break;

                            //MS-OXCMSG ROPs
                            case RopIdType.RopOpenMessage:
                                RopOpenMessageResponse ropOpenMessageResponse = new RopOpenMessageResponse();
                                ropOpenMessageResponse.Parse(s);
                                ropsList.Add(ropOpenMessageResponse);
                                if ((ErrorCodes)ropOpenMessageResponse.ReturnValue == ErrorCodes.Success)
                                {
                                    uint handle_OpenMessage = tempServerObjectHandleTable[ropOpenMessageResponse.OutputHandleIndex];
                                    if (DecodingContext.ObjectHandles.Count > 0 && DecodingContext.ObjectHandles.ContainsKey(handle_OpenMessage))
                                    {
                                        DecodingContext.ObjectHandles.Remove(handle_OpenMessage);
                                    }
                                    DecodingContext.ObjectHandles.Add(handle_OpenMessage, ObjectHandlesType.MessageHandles);
                                }
                                break;
                            case RopIdType.RopCreateMessage:
                                RopCreateMessageResponse ropCreateMessageResponse = new RopCreateMessageResponse();
                                ropCreateMessageResponse.Parse(s);
                                ropsList.Add(ropCreateMessageResponse);
                                if ((ErrorCodes)ropCreateMessageResponse.ReturnValue == ErrorCodes.Success)
                                {
                                    uint handle_CreateMessage = tempServerObjectHandleTable[ropCreateMessageResponse.OutputHandleIndex];
                                    if (DecodingContext.ObjectHandles.Count > 0 && DecodingContext.ObjectHandles.ContainsKey(handle_CreateMessage))
                                    {
                                        DecodingContext.ObjectHandles.Remove(handle_CreateMessage);
                                    }
                                    DecodingContext.ObjectHandles.Add(handle_CreateMessage, ObjectHandlesType.MessageHandles);
                                }
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
                                if ((ErrorCodes)ropCreateAttachmentResponse.ReturnValue == ErrorCodes.Success)
                                {
                                    uint handle_CreateAttach = tempServerObjectHandleTable[ropCreateAttachmentResponse.OutputHandleIndex];
                                    if (DecodingContext.ObjectHandles.Count > 0 && DecodingContext.ObjectHandles.ContainsKey(handle_CreateAttach))
                                    {
                                        DecodingContext.ObjectHandles.Remove(handle_CreateAttach);
                                    }
                                    DecodingContext.ObjectHandles.Add(handle_CreateAttach, ObjectHandlesType.AttachmentHandles);
                                }
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
                                RopRegisterNotificationResponse RopRegisterNotificationResponse = new RopRegisterNotificationResponse();
                                RopRegisterNotificationResponse.Parse(s);
                                ropsList.Add(RopRegisterNotificationResponse);
                                break;

                            case RopIdType.RopPending:
                                RopPendingResponse RopPendingResponse = new RopPendingResponse();
                                RopPendingResponse.Parse(s);
                                ropsList.Add(RopPendingResponse);
                                break;

                            case RopIdType.RopNotify:
                                RopNotifyResponse ropNotifyResponse = new RopNotifyResponse();
                                ropNotifyResponse.Parse(s);
                                ropsList.Add(ropNotifyResponse);
                                break;

                            // MS-OXCPERM ROPs
                            case RopIdType.RopGetPermissionsTable:
                                RopGetPermissionsTableResponse RopGetPermissionsTableResponse = new RopGetPermissionsTableResponse();
                                RopGetPermissionsTableResponse.Parse(s);
                                ropsList.Add(RopGetPermissionsTableResponse);
                                break;

                            case RopIdType.RopModifyPermissions:
                                RopModifyPermissionsResponse RopModifyPermissionsResponse = new RopModifyPermissionsResponse();
                                RopModifyPermissionsResponse.Parse(s);
                                ropsList.Add(RopModifyPermissionsResponse);
                                break;

                            default:
                                object RopsBytes = ReadBytes(this.RopSize - (int)s.Position);
                                ropsList.Add(RopsBytes);
                                break;
                        }

                    } while (s.Position < this.RopSize);

                }
                else
                {
                    this.RopsList = null;
                }
            }
            else
            {
                byte[] ropListBytes = ReadBytes(this.RopSize - 2);
            }

            this.RopsList = ropsList.ToArray();
            while (s.Position < s.Length)
            {
                uint ServerObjectHandle = ReadUint();
                serverObjectHandleTable.Add(ServerObjectHandle);
            }
            this.ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
    #endregion

    #region 2.2.2 RopIds
    /// <summary>
    /// The enum type for RopIds.
    /// </summary>
    public enum RopIdType : byte
    {
        RopRelease = 0x01,
        RopOpenFolder = 0x02,
        RopOpenMessage = 0x03,
        RopGetHierarchyTable = 0x04,
        RopGetContentsTable = 0x05,
        RopCreateMessage = 0x06,
        RopGetPropertiesSpecific = 0x07,
        RopGetPropertiesAll = 0x08,
        RopGetPropertiesList = 0x09,
        RopSetProperties = 0x0A,
        RopDeleteProperties = 0x0B,
        RopSaveChangesMessage = 0x0C,
        RopRemoveAllRecipients = 0x0D,
        RopModifyRecipients = 0x0E,
        RopReadRecipients = 0x0F,
        RopReloadCachedInformation = 0x10,
        RopSetMessageReadFlag = 0x11,
        RopSetColumns = 0x12,
        RopSortTable = 0x13,
        RopRestrict = 0x14,
        RopQueryRows = 0x15,
        RopGetStatus = 0x16,
        RopQueryPosition = 0x17,
        RopSeekRow = 0x18,
        RopSeekRowBookmark = 0x19,
        RopSeekRowFractional = 0x1A,
        RopCreateBookmark = 0x1B,
        RopCreateFolder = 0x1C,
        RopDeleteFolder = 0x1D,
        RopDeleteMessages = 0x1E,
        RopGetMessageStatus = 0x1F,
        RopSetMessageStatus = 0x20,
        RopGetAttachmentTable = 0x21,
        RopOpenAttachment = 0x22,
        RopCreateAttachment = 0x23,
        RopDeleteAttachment = 0x24,
        RopSaveChangesAttachment = 0x25,
        RopSetReceiveFolder = 0x26,
        RopGetReceiveFolder = 0x27,
        RopRegisterNotification = 0x29,
        RopNotify = 0x2A,
        RopOpenStream = 0x2B,
        RopReadStream = 0x2C,
        RopWriteStream = 0x2D,
        RopSeekStream = 0x2E,
        RopSetStreamSize = 0x2F,
        RopSetSearchCriteria = 0x30,
        RopGetSearchCriteria = 0x31,
        RopSubmitMessage = 0x32,
        RopMoveCopyMessages = 0x33,
        RopAbortSubmit = 0x34,
        RopMoveFolder = 0x35,
        RopCopyFolder = 0x36,
        RopQueryColumnsAll = 0x37,
        RopAbort = 0x38,
        RopCopyTo = 0x39,
        RopCopyToStream = 0x3A,
        RopCloneStream = 0x3B,
        RopGetPermissionsTable = 0x3E,
        RopGetRulesTable = 0x3F,
        RopModifyPermissions = 0x40,
        RopModifyRules = 0x41,
        RopGetOwningServers = 0x42,
        RopLongTermIdFromId = 0x43,
        RopIdFromLongTermId = 0x44,
        RopPublicFolderIsGhosted = 0x45,
        RopOpenEmbeddedMessage = 0x46,
        RopSetSpooler = 0x47,
        RopSpoolerLockMessage = 0x48,
        RopGetAddressTypes = 0x49,
        RopTransportSend = 0x4A,
        RopFastTransferSourceCopyMessages = 0x4B,
        RopFastTransferSourceCopyFolder = 0x4C,
        RopFastTransferSourceCopyTo = 0x4D,
        RopFastTransferSourceGetBuffer = 0x4E,
        RopFindRow = 0x4F,
        RopProgress = 0x50,
        RopTransportNewMail = 0x51,
        RopGetValidAttachments = 0x52,
        RopFastTransferDestinationConfigure = 0x53,
        RopFastTransferDestinationPutBuffer = 0x54,
        RopGetNamesFromPropertyIds = 0x55,
        RopGetPropertyIdsFromNames = 0x56,
        RopUpdateDeferredActionMessages = 0x57,
        RopEmptyFolder = 0x58,
        RopExpandRow = 0x59,
        RopCollapseRow = 0x5A,
        RopLockRegionStream = 0x5B,
        RopUnlockRegionStream = 0x5C,
        RopCommitStream = 0x5D,
        RopGetStreamSize = 0x5E,
        RopQueryNamedProperties = 0x5F,
        RopGetPerUserLongTermIds = 0x60,
        RopGetPerUserGuid = 0x61,
        RopReadPerUserInformation = 0x63,
        RopWritePerUserInformation = 0x64,
        RopSetReadFlags = 0x66,
        RopCopyProperties = 0x67,
        RopGetReceiveFolderTable = 0x68,
        RopFastTransferSourceCopyProperties = 0x69,
        RopGetCollapseState = 0x6B,
        RopSetCollapseState = 0x6C,
        RopGetTransportFolder = 0x6D,
        RopPending = 0x6E,
        RopOptionsData = 0x6F,
        RopSynchronizationConfigure = 0x70,
        RopSynchronizationImportMessageChange = 0x72,
        RopSynchronizationImportHierarchyChange = 0x73,
        RopSynchronizationImportDeletes = 0x74,
        RopSynchronizationUploadStateStreamBegin = 0x75,
        RopSynchronizationUploadStateStreamContinue = 0x76,
        RopSynchronizationUploadStateStreamEnd = 0x77,
        RopSynchronizationImportMessageMove = 0x78,
        RopSetPropertiesNoReplicate = 0x79,
        RopDeletePropertiesNoReplicate = 0x7A,
        RopGetStoreState = 0x7B,
        RopSynchronizationOpenCollector = 0x7E,
        RopGetLocalReplicaIds = 0x7F,
        RopSynchronizationImportReadStateChanges = 0x80,
        RopResetTable = 0x81,
        RopSynchronizationGetTransferState = 0x82,
        RopTellVersion = 0x86,
        RopFreeBookmark = 0x89,
        RopWriteAndCommitStream = 0x90,
        RopHardDeleteMessages = 0x91,
        RopHardDeleteMessagesAndSubfolders = 0x92,
        RopSetLocalReplicaMidsetDeleted = 0x93,
        RopFastTransferDestinationPutBufferExtended = 0x9D,
        RopWriteStreamExtended = 0xA3,
        RopBackoff = 0xF9,
        RopLogon = 0xFE,
        RopBufferTooSmall = 0xFF
    }

    /// <summary>
    /// The enum value of rop response status.
    /// </summary>
    public enum RopResponseStatus : uint
    {
        Success = 0x00000000,
        LogonRedirect = 0x00000478,
        NullDestinationObject = 0x00000503
    }

    #endregion

    #region 2.2.7.1 RopSubmitMessage
    /// <summary>
    ///  A class indicates the RopSubmitMessage ROP Request Buffer.
    /// </summary>
    public class RopSubmitMessageRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A flags structure that contains flags that specify special behavior for submitting the message.
        public SubmitFlags SubmitFlags;

        /// <summary>
        /// Parse the RopSubmitMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSubmitMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.SubmitFlags = (SubmitFlags)ReadByte();
        }

    }

    /// <summary>
    ///  A class indicates the RopSubmitMessage ROP Response Buffer.
    /// </summary>
    public class RopSubmitMessageResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public uint ReturnValue;

        /// <summary>
        /// Parse the RopSubmitMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSubmitMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();
        }
    }

    #endregion

    #region 2.2.7.2 RopAbortSubmit
    /// <summary>
    ///  A class indicates the RopAbortSubmit ROP Request Buffer.
    /// </summary>
    public class RopAbortSubmitRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An identifier that identifies the folder in which the submitted message is located.
        public FolderID FolderId;

        // An identifier that specifies the submitted message.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopAbortSubmitRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortSubmitRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
        }

    }

    /// <summary>
    ///  A class indicates the RopSubmitMessage ROP Response Buffer.
    /// </summary>
    public class RopAbortSubmitResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopAbortSubmitResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortSubmitResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }

    #endregion

    #region 2.2.7.3 RopGetAddressTypes
    /// <summary>
    ///  A class indicates the RopGetAddressTypes ROP Request Buffer.
    /// </summary>
    public class RopGetAddressTypesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetAddressTypesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAddressTypesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }

    }

    /// <summary>
    ///  A class indicates the RopGetAddressTypes ROP Response Buffer.
    /// </summary>
    public class RopGetAddressTypesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of strings in the AddressTypes field.
        public ushort? AddressTypeCount;

        // An unsigned integer that specifies the length of the AddressTypes field.
        public ushort? AddressTypeSize;

        // A list of null-terminated ASCII strings.
        public MAPIString[] AddressTypes;

        /// <summary>
        /// Parse the RopGetAddressTypesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAddressTypesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.AddressTypeCount = ReadUshort();
                this.AddressTypeSize = ReadUshort();
                List<MAPIString> listAddressTypes = new List<MAPIString>();

                for (int i = 0; i < this.AddressTypeCount; i++)
                {
                    MAPIString tempAddressTypes = new MAPIString(Encoding.ASCII);
                    tempAddressTypes.Parse(s);
                    listAddressTypes.Add(tempAddressTypes);
                }
                this.AddressTypes = listAddressTypes.ToArray();

            }
        }
    }

    #endregion

    #region 2.2.7.4 RopSetSpooler
    /// <summary>
    ///  A class indicates the RopSetSpooler ROP Request Buffer.
    /// </summary>
    public class RopSetSpoolerRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopSetSpoolerRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetSpoolerRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }

    }

    /// <summary>
    ///  A class indicates the RopSetSpooler ROP Response Buffer.
    /// </summary>
    public class RopSetSpoolerResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetSpoolerResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetSpoolerResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }

    #endregion

    #region 2.2.7.5 RopSpoolerLockMessage
    /// <summary>
    ///  A class indicates the RopSpoolerLockMessage ROP Request Buffer.
    /// </summary>
    public class RopSpoolerLockMessageRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An identifier that specifies the message for which the status will be changed.
        public MessageID MessageId;

        // An integer flag specifies a status to set on the message.
        public LockState LockState;

        /// <summary>
        /// Parse the RopSpoolerLockMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSpoolerLockMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
            this.LockState = (LockState)ReadByte();
        }

    }

    /// <summary>
    ///  A class indicates the RopSpoolerLockMessage ROP Response Buffer.
    /// </summary>
    public class RopSpoolerLockMessageResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSpoolerLockMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSpoolerLockMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }

    #endregion

    #region 2.2.7.6 RopTransportSend
    /// <summary>
    ///  A class indicates the RopTransportSend ROP Request Buffer.
    /// </summary>
    public class RopTransportSendRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopTransportSendRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportSendRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }

    }

    /// <summary>
    ///  A class indicates the RopTransportSend ROP Response Buffer.
    /// </summary>
    public class RopTransportSendResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A boolean that specifies whether property values are teturned.
        public byte? NoPropertiesReturned;

        // An unsigned integer that specifies the number of structures returned in the PropertyValues field.
        public ushort? PropertyValueCount;

        // An array of TaggedPropertyValue structures that specifies the properties to copy.
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopTransportSendResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportSendResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.NoPropertiesReturned = ReadByte();
                this.PropertyValueCount = ReadUshort();
                List<TaggedPropertyValue> tempPropertyValues = new List<TaggedPropertyValue>();
                for (int i = 0; i < this.PropertyValueCount; i++)
                {
                    TaggedPropertyValue temptaggedPropertyValue = new TaggedPropertyValue(CountWideEnum.twoBytes);
                    temptaggedPropertyValue.Parse(s);
                    tempPropertyValues.Add(temptaggedPropertyValue);
                }
                this.PropertyValues = tempPropertyValues.ToArray();
            }
        }
    }

    #endregion

    #region 2.2.7.7 RopTransportNewMail
    /// <summary>
    ///  A class indicates the RopTransportNewMail ROP Request Buffer.
    /// </summary>
    public class RopTransportNewMailRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An identifier that specifies the new message object.
        public MessageID MessageId;

        // An identifier that identifies the folder of the new message object.
        public FolderID FolderId;

        // A null-terminated ASCII string that specifies the message class of the new message object;

        public MAPIString MessageClass;

        // A flags structure that contains the message flags of the new message object.
        public MessageFlags MessageFlags;

        /// <summary>
        /// Parse the RopTransportNewMailRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportNewMailRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.MessageClass = new MAPIString(Encoding.ASCII);
            this.MessageClass.Parse(s);
            this.MessageFlags = (MessageFlags)ReadUint();
        }

    }

    /// <summary>
    ///  A class indicates the RopTransportNewMail ROP Response Buffer.
    /// </summary>
    public class RopTransportNewMailResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopTransportNewMailResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportNewMailResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

        }
    }

    #endregion

    #region 2.2.7.8 RopGetTransportFolder
    /// <summary>
    ///  A class indicates the RopGetTransportFolder ROP Request Buffer.
    /// </summary>
    public class RopGetTransportFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetTransportFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetTransportFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }

    }

    /// <summary>
    ///  A class indicates the RopGetTransportFolder ROP Response Buffer.
    /// </summary>
    public class RopGetTransportFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An identifier that specifies the transport folder.
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopGetTransportFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetTransportFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
            }
        }
    }

    #endregion

    #region 2.2.7.9 RopOptionsData
    /// <summary>
    ///  A class indicates the RopOptionsData ROP Request Buffer.
    /// </summary>
    public class RopOptionsDataRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A null-terminated ASCII string that specifies the address type that options are to be returned for.

        public MAPIString AddressType;

        // A boolean that specifies whether the help file data is to be returned in a format that is suited for 32-bit machines.
        public byte WantWin32;

        /// <summary>
        /// Parse the RopOptionsDataRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOptionsDataRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.AddressType = new MAPIString(Encoding.ASCII);
            this.AddressType.Parse(s);
            this.WantWin32 = ReadByte();
        }

    }

    /// <summary>
    ///  A class indicates the RopOptionsData ROP Response Buffer.
    /// </summary>
    public class RopOptionsDataResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // Reserved.
        public byte? Reserved;

        // An unsigned integer that specifies the size of the OptionsInfo field.
        public ushort? OptionalInfoSize;

        // An array of bytes that contains opaque data from the server.
        public byte?[] OptionalInfo;

        // An unsigned integer that specifies the size of the HelpFile field.
        public ushort? HelpFileSize;

        // An array of bytes that contains the help file associated with the specified address type.
        public byte?[] HelpFile;

        // A null-terminated multibyte string that specifies the name of the help file that is associated with the specified address type.
        public MAPIString HelpFileName;

        /// <summary>
        /// Parse the RopOptionsDataResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOptionsDataResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.Reserved = ReadByte();
                this.OptionalInfoSize = ReadUshort();
                this.OptionalInfo = ConvertArray(ReadBytes((int)this.OptionalInfoSize));
                this.HelpFileSize = ReadUshort();
                if (this.HelpFileSize != 0)
                {
                    this.HelpFile = ConvertArray(ReadBytes((int)this.HelpFileSize));
                    this.HelpFileName = new MAPIString(Encoding.ASCII);
                    this.HelpFileName.Parse(s);
                }
            }
        }
    }

    #endregion

    #region 2.2.15.1 RopBufferTooSmall

    /// <summary>
    ///  A class indicates the RopBufferTooSmall ROP Response Buffer.
    /// </summary>
    public class RopBufferTooSmallResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the size required for the ROP output buffer.
        public ushort SizeNeeded;

        // An array of bytes that contains the section of the ROP input buffer that was not executed because of the insufficient size of the ROP output buffer.
        public byte[] RequestBuffers;

        // An unsigned integer that specifies the size of RequestBuffers.
        private uint RequestBuffersSize;

        /// <summary>
        /// The Constructor of RopBufferTooSmallResponse.
        /// </summary>
        /// <param name="RequestBuffersSize"> The size of RequestBuffers.</param>
        public RopBufferTooSmallResponse(uint RequestBuffersSize)
        {
            this.RequestBuffersSize = RequestBuffersSize;
        }

        /// <summary>
        /// Parse the RopBufferTooSmallResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopBufferTooSmallResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SizeNeeded = ReadUshort();
            this.RequestBuffers = ReadBytes((int)this.RequestBuffersSize);
        }
    }

    #endregion

    #region 2.2.15.2 RopBackoff

    /// <summary>
    /// A class indicates the RopBackoff ROP Response Buffer.
    /// </summary>
    public class RopBackoffResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x01.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer that specifies the number of milliseconds to apply a ROP backoff.
        public uint Duration;

        // An unsigned integer that specifies the number of structures in the BackoffRopData field.
        public byte BackoffRopCount;

        // An array of BackoffRop structures. 
        public BackoffRop[] BackoffRopData;

        // An unsigned integer that specifies the size of the AdditionalData field.
        public ushort AdditionalDataSize;

        // An array of bytes that specifies additional information about the backoff response. 
        public byte[] AdditionalData;

        /// <summary>
        /// Parse the RopBackoffResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopBackoffResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.Duration = ReadUint();
            this.BackoffRopCount = ReadByte();
            List<BackoffRop> BackoffRopDataList = new List<BackoffRop>();
            for (int i = 0; i < this.BackoffRopCount; i++)
            {
                BackoffRop SubBackoffRop = new BackoffRop();
                SubBackoffRop.Parse(s);
                BackoffRopDataList.Add(SubBackoffRop);
            }

            this.BackoffRopData = BackoffRopDataList.ToArray();
            this.AdditionalDataSize = ReadUshort();
            this.AdditionalData = ReadBytes(this.AdditionalDataSize);
        }
    }

    /// <summary>
    ///  A class indicates the BackoffRop structure which is defined in section 2.2.15.2.1.1.
    /// </summary>
    public class BackoffRop : BaseStructure
    {
        // An unsigned integer index that identifies the ROP to apply the ROP backoff to
        public byte RopIdBackoff;

        // An unsigned integer that specifies the number of milliseconds to apply a ROP backoff.
        public uint Duration;

        /// <summary>
        /// Parse the BackoffRop structure.
        /// </summary>
        /// <param name="s">A stream containing BackoffRop structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopIdBackoff = ReadByte();
            this.Duration = ReadUint();
        }
    }
    #endregion

    #region 2.2.15.3 RopRelease

    /// <summary>
    ///  A class indicates the RopRelease ROP Request Buffer.
    /// </summary>
    public class RopReleaseRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x01.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopReleaseResquest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReleaseResquest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    #endregion

    #region Enums defined in MS-OXOMSG
    /// <summary>
    /// The enum type for flags indicates the status of a message object.
    /// </summary>
    [Flags]
    public enum MessageFlags : uint
    {
        mfRead = 0x00000001,
        mfUnsent = 0x00000008,
        mfResend = 0x00000080
    }

    /// <summary>
    /// The enum type for flags indicates how the message is to be delivered.
    /// </summary>
    public enum SubmitFlags : byte
    {
        None = 0x00,
        PreProcess = 0x01,
        NeedsSpooler = 0x02
    }

    /// <summary>
    /// The enum type for flags specifies a status to set on a message.
    /// </summary>
    public enum LockState : byte
    {
        IstLock = 0x00,
        IstUnlock = 0x01,
        IstFininshed = 0x02
    }

    #endregion

    #region Helper method for Decoding
    /// <summary>
    ///  The DecodingContext is shared between some ROP request and response.
    /// </summary>
    public class DecodingContext
    {
        // Record the LogonId and logon flags.
        private static Dictionary<byte, LogonFlags> logonFlagMapLogId;

        // Record the map in session information,LogonId and logon flags.
        private static Dictionary<int, Dictionary<byte, LogonFlags>> sessionLogonFlagMapLogId;

        // Record the map in session information, handle index and logonFlags in logon rop.
        private static Dictionary<int, Dictionary<uint, LogonFlags>> sessionLogonFlagsInLogonRop;

        // Record the map in session information, handle index, and PropertyTags for getPropertiesSpecific rop.
        private static Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>> getPropertiesSpec_propertyTags;

        // Record object handles value and type, contains FolderHandles, MessageHandles and AttachmentHandles.
        private static Dictionary<uint, ObjectHandlesType> objectHandles;

        // Record the map in session id and the remain seize in roplist parsing.
        private static Dictionary<int, List<uint>> sessionRequestRemainSize;

        // Record the map of SetColumns's output handle and property tags.
        private static Dictionary<uint, Dictionary<uint, PropertyTag[]>> setColumnProTagMap_Handle;

        // Record RopSetColumn InputObjectHandle in setColumn Response.
        private static List<uint> setColumn_InputHandles_InResponse;

        // Record RopSetColumn InputObjectHandle in setColumn request for notify.
        private static Dictionary<uint, PropertyTag[]> setColumn_InputHandles_InRequest;

        // Record the map of session id and PropertyTags for RopNotify.
        private static Dictionary<uint, PropertyTag[]> propertyTagsForNotify;

        // Record the map in session id, handle index and PropertyTags for row rops.
        private static Dictionary<int, Dictionary<uint, PropertyTag[]>> rowRops_propertyTags;

        // Record the map in session id and partial information is ready.
        private static Dictionary<int, bool> partialInformationReady;

        public DecodingContext()
        {
            objectHandles = new Dictionary<uint, ObjectHandlesType>();
            logonFlagMapLogId = new Dictionary<byte, LogonFlags>();
            sessionLogonFlagMapLogId = new Dictionary<int, Dictionary<byte, LogonFlags>>();
            sessionLogonFlagsInLogonRop = new Dictionary<int, Dictionary<uint, LogonFlags>>();
            getPropertiesSpec_propertyTags = new Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>>();
            sessionRequestRemainSize = new Dictionary<int, List<uint>>();
            setColumnProTagMap_Handle = new Dictionary<uint, Dictionary<uint, PropertyTag[]>>();
            propertyTagsForNotify = new Dictionary<uint, PropertyTag[]>();
            rowRops_propertyTags = new Dictionary<int, Dictionary<uint, PropertyTag[]>>();
            setColumn_InputHandles_InResponse = new List<uint>();
            setColumn_InputHandles_InRequest = new Dictionary<uint, PropertyTag[]>();
            partialInformationReady = new Dictionary<int, bool>();
        }

        // Gets or sets the objectHandles
        public static Dictionary<uint, ObjectHandlesType> ObjectHandles
        {
            get
            {
                return objectHandles;
            }
            set
            {
                objectHandles = value;
            }
        }

        // Gets or sets the LogonId and logon flags
        public static Dictionary<byte, LogonFlags> LogonFlagMapLogId
        {
            get
            {
                return logonFlagMapLogId;
            }
            set
            {
                logonFlagMapLogId = value;
            }
        }

        // Gets or sets the sessionLogonFlagMapLogId
        public static Dictionary<int, Dictionary<byte, LogonFlags>> SessionLogonFlagMapLogId
        {
            get
            {
                return sessionLogonFlagMapLogId;
            }
            set
            {
                sessionLogonFlagMapLogId = value;
            }
        }

        // Gets or sets the sessionLogonFlagsInLogonRop
        public static Dictionary<int, Dictionary<uint, LogonFlags>> SessionLogonFlagsInLogonRop
        {
            get
            {
                return sessionLogonFlagsInLogonRop;
            }
            set
            {
                sessionLogonFlagsInLogonRop = value;
            }
        }

        // Gets or sets the getPropertiesSpec_propertyTags
        public static Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>> GetPropertiesSpec_propertyTags
        {
            get
            {
                return getPropertiesSpec_propertyTags;
            }
            set
            {
                getPropertiesSpec_propertyTags = value;
            }
        }

        // Gets or sets the sessionRequestRemainSize
        public static Dictionary<int, List<uint>> SessionRequestRemainSize
        {
            get
            {
                return sessionRequestRemainSize;
            }
            set
            {
                sessionRequestRemainSize = value;
            }
        }

        // Gets or sets the SetColumnProTagMap_Handle
        public static Dictionary<uint, Dictionary<uint, PropertyTag[]>> SetColumnProTagMap_Handle
        {
            get
            {
                return setColumnProTagMap_Handle;
            }
            set
            {
                setColumnProTagMap_Handle = value;
            }
        }

        // Gets or sets the propertyTagsForNotify
        public static Dictionary<uint, PropertyTag[]> PropertyTagsForNotify
        {
            get
            {
                return propertyTagsForNotify;
            }
            set
            {
                propertyTagsForNotify = value;
            }
        }

        // Gets or sets the rowRops_propertyTags
        public static Dictionary<int, Dictionary<uint, PropertyTag[]>> RowRops_propertyTags
        {
            get
            {
                return rowRops_propertyTags;
            }
            set
            {
                rowRops_propertyTags = value;
            }
        }

        // Gets or sets the setColumn_InputHandles
        public static List<uint> SetColumn_InputHandles_InResponse
        {
            get
            {
                return setColumn_InputHandles_InResponse;
            }
            set
            {
                setColumn_InputHandles_InResponse = value;
            }
        }

        // Gets or sets the setColumn_InputHandles
        public static Dictionary<uint, PropertyTag[]> SetColumn_InputHandles_InRequest
        {
            get
            {
                return setColumn_InputHandles_InRequest;
            }
            set
            {
                setColumn_InputHandles_InRequest = value;
            }
        }

        // Gets or sets the partialInformationReady
        public static Dictionary<int, bool> PartialInformationReady
        {
            get
            {
                return partialInformationReady;
            }
            set
            {
                partialInformationReady = value;
            }
        }
    }

    #endregion

    /// <summary>
    /// The MissingInformationException is used to define the exception, which are caused by missing context information.
    /// </summary>
    public class MissingInformationException : Exception
    {
        // The exception message thrown
        public string ErrorMessage;

        // The ROP ID needs context information
        public ushort RopID;

        // The source ROP parameters to pass
        public uint[] Parameters;

        public MissingInformationException(string message, ushort ropID, uint[] parameter = null)
        {
            this.ErrorMessage = message;
            this.RopID = ropID;
            this.Parameters = parameter;
        }
    }

    /// <summary>
    /// The ContextInformation is used to save the related parameters during parsing.  
    /// </summary>
    public class ContextInformation
    {
        // The RopId indicates the target ROP searched
        public RopIdType RopID { get; set; }
        // The handle indicates the target handle searched
        public uint Handle { get; set; }
        // The result searched for the target context information
        public object RelatedInformation { get; set; }
    }

    public class MissingPartialInformationException : Exception
    {
        // The ROP ID needs context information
        public ushort RopID;

        // The source ROP parameters to pass
        public uint Parameter;

        public MissingPartialInformationException(ushort ropID, uint parameter)
        {
            this.RopID = ropID;
            this.Parameter = parameter;
        }
    }
}
