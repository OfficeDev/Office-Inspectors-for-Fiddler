using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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

            this.RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();

            if (this.RopSize > 2)
            {
                do
                {
                    int CurrentByte = s.ReadByte();
                    s.Position -= 1;
                    switch ((RopIdType)CurrentByte)
                    {
                        // MS-OXCSTOR Rops
                        case RopIdType.RopLogon:
                            RopLogonRequest RopLogonRequest = new RopLogonRequest();
                            RopLogonRequest.Parse(s);
                            ropsList.Add(RopLogonRequest);
                            DecodingContext.SessionLogonFlag = new Dictionary<int, LogonFlags>() { { MapiInspector.MAPIInspector.currentSessionID, RopLogonRequest.LogonFlags } };
                            break;

                        // MS-OXCROPS Rops
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
                            break;
                        // MSOXCTABL Rop
                        case RopIdType.RopSetColumns:
                            RopSetColumnsRequest RopSetColumnsRequest = new RopSetColumnsRequest();
                            RopSetColumnsRequest.Parse(s);
                            ropsList.Add(RopSetColumnsRequest);
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

                        // MSOXORULE Rop
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
							
                        //MS-OXCFXICS
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
                            if (DecodingContext.SessionFastTransferStreamType != null && DecodingContext.SessionFastTransferStreamType.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                DecodingContext.StreamType_Getbuffer = DecodingContext.SessionFastTransferStreamType[MapiInspector.MAPIInspector.currentSessionID];
                            }
                            else
                            {
                                throw new MissingInformationException("Missing TransferStream type information for RopFastTransferDestinationPutBufferRequest", (ushort)CurrentByte, null);
                            }
                            RopFastTransferDestinationPutBufferRequest RopFastTransferDestinationPutBufferRequest = new RopFastTransferDestinationPutBufferRequest();
                            RopFastTransferDestinationPutBufferRequest.Parse(s);
                            ropsList.Add(RopFastTransferDestinationPutBufferRequest);
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
                        default:
                            object RopsBytes = ReadBytes(this.RopSize - 2);
                            ropsList.Add(RopsBytes);
                            break;
                    }

                } while (s.Position < this.RopSize);
               
            }
            else
            {
                this.RopsList = null;
            }

            this.RopsList = ropsList.ToArray();
            while (s.Position < s.Length)
            {
                uint ServerObjectHandle = ReadUint();
                serverObjectHandleTable.Add(ServerObjectHandle);
            }
            this.ServerObjectHandleTable = serverObjectHandleTable.ToArray();

            if (this.RopsList.Length > 0)
            {
                foreach (var ropRequest in this.RopsList)
                {
                    // This is used to store InputServerObject in RopFastTransferSourceGetBufferRequest.
                    if (ropRequest is RopFastTransferSourceGetBufferRequest)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropRequest as RopFastTransferSourceGetBufferRequest).InputHandleIndex];

                        if (DecodingContext.GetBuffer_InPutHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.GetBuffer_InPutHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.GetBuffer_InPutHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }

                    // This is used to set FastTransfer stream root type according to InputServerObject of RopFastTransferSourceCopyTo and RopFastTransferSourceCopyProperties rops,  which are used in MS-OXCFXICS
                    else if (ropRequest is RopFastTransferSourceCopyToRequest || ropRequest is RopFastTransferSourceCopyPropertiesRequest)
                    {
                        uint objectHandleKey;
                        if(ropRequest is RopFastTransferSourceCopyToRequest)
                        { 
                            objectHandleKey = this.ServerObjectHandleTable[(ropRequest as RopFastTransferSourceCopyToRequest).InputHandleIndex];
                            if (DecodingContext.CopyTo_InputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                DecodingContext.CopyTo_InputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                            }
                            DecodingContext.CopyTo_InputHandles.Add(MapiInspector.MAPIInspector.currentSessionID,(int)objectHandleKey);
                        }
                        else
                        {
                            objectHandleKey = this.ServerObjectHandleTable[(ropRequest as RopFastTransferSourceCopyPropertiesRequest).InputHandleIndex];
                            if (DecodingContext.CopyProperties_InputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                DecodingContext.CopyProperties_InputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                            }
                            DecodingContext.CopyProperties_InputHandles.Add(MapiInspector.MAPIInspector.currentSessionID,(int)objectHandleKey);
                        }

                        if (!DecodingContext.ObjectHandles.ContainsKey((int)objectHandleKey))
                        {
                            if (ropRequest is RopFastTransferSourceCopyToRequest)
                            {
                                throw new MissingInformationException("Need more information about foler or message or attachment object handle", (ushort)(ropRequest as RopFastTransferSourceCopyToRequest).RopId, objectHandleKey);
                            }
                            else
                            {
                                throw new MissingInformationException("Need more information about foler or message or attachment object handle", (ushort)(ropRequest as RopFastTransferSourceCopyPropertiesRequest).RopId, objectHandleKey);
                            } 
                        }
                    }
                    // This is used to set FastTransfer stream root type according to InputServerObject of RopFastTransferDestinationConfigureRequest rop,  which are used in MS-OXCFXICS
                    else if (ropRequest is RopFastTransferDestinationConfigureRequest)
                    {
                        RopFastTransferDestinationConfigureRequest request = ropRequest as RopFastTransferDestinationConfigureRequest;
                       
                        if (request.SourceOperation == SourceOperation.CopyTo || request.SourceOperation == SourceOperation.CopyProperties)
                        {
                            uint objectHandleKeyII = this.ServerObjectHandleTable[request.InputHandleIndex];

                            if (DecodingContext.DestinationConfigure_InputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                DecodingContext.DestinationConfigure_InputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                            }
                            DecodingContext.DestinationConfigure_InputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKeyII);

                            if (!DecodingContext.ObjectHandles.ContainsKey((int)objectHandleKeyII))
                            {
                                throw new MissingInformationException("Need more information about foler or message or attachment object handle", (ushort)(ropRequest as RopFastTransferDestinationConfigureRequest).RopId, objectHandleKeyII); 
                            }
                        }
                    }
                }   
            }
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

            this.RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();

            if (this.RopSize > 2)
            {
                do
                {
                    int CurrentByte = s.ReadByte();
                    s.Position -= 1;
                    switch ((RopIdType)CurrentByte)
                    {
                        // MS-OXCSTOR Rops
                        case RopIdType.RopLogon:
                            if (DecodingContext.SessionLogonFlag != null && DecodingContext.SessionLogonFlag.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                DecodingContext.LogonFlags = DecodingContext.SessionLogonFlag[MapiInspector.MAPIInspector.currentSessionID];
                            }
                            else
                            {
                                throw new MissingInformationException("Missing LogonFlags information for RopLogon", (ushort)CurrentByte, null);
                            }
                            if (((byte)DecodingContext.LogonFlags & 0x01) == (byte)LogonFlags.Private)
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

                        // MS-OXCROPS Rops
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
                            RopBufferTooSmallResponse RopBufferTooSmallResponse = new RopBufferTooSmallResponse();
                            RopBufferTooSmallResponse.Parse(s);
                            ropsList.Add(RopBufferTooSmallResponse);
                            break;
                        // MSOXCTABL Rop
                        case RopIdType.RopSetColumns:
                            RopSetColumnsResponse RopSetColumnsResponse = new RopSetColumnsResponse();
                            RopSetColumnsResponse.Parse(s);
                            ropsList.Add(RopSetColumnsResponse);
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
                            object RopsBytes1 = ReadBytes(this.RopSize - 2);
                            ropsList.Add(RopsBytes1);
                            /* TODO
                            RopQueryRowsResponse RopQueryRowsResponse = new RopQueryRowsResponse();
                            RopQueryRowsResponse.Parse(s);
                            ropsList.Add(RopQueryRowsResponse);
                             */
                            break;

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
                            object RopsBytes2 = ReadBytes(this.RopSize - 2);
                            ropsList.Add(RopsBytes2);
                            /* TODO
                            RopFindRowResponse RopFindRowResponse = new RopFindRowResponse();
                            RopFindRowResponse.Parse(s);
                            ropsList.Add(RopFindRowResponse);
                              */
                            break;

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
                            object RopsBytes3 = ReadBytes(this.RopSize - 2);
                            ropsList.Add(RopsBytes3);
                            /* TODO
                            RopExpandRowResponse RopExpandRowResponse = new RopExpandRowResponse();
                            RopExpandRowResponse.Parse(s);
                            ropsList.Add(RopExpandRowResponse);
                               */
                            break;

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

                        // MSOXORULE Rop
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
								
                        //MS-OXCFXICS Rops
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
                            if (DecodingContext.SessionFastTransferStreamType != null && DecodingContext.SessionFastTransferStreamType.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                DecodingContext.StreamType_Getbuffer = DecodingContext.SessionFastTransferStreamType[MapiInspector.MAPIInspector.currentSessionID];
                            }
                            else
                            {
                                throw new MissingInformationException("Missing TransferStream type information for RopFastTransferSourceGetBufferResponse", (ushort)CurrentByte, null);
                            }
                            RopFastTransferSourceGetBufferResponse RopFastTransferSourceGetBufferResponse = new RopFastTransferSourceGetBufferResponse();
                            RopFastTransferSourceGetBufferResponse.Parse(s);
                            ropsList.Add(RopFastTransferSourceGetBufferResponse);
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

                        // TODO This is just used for FasttransferStream now, after these rops has added in MS-OXCFOLD and MS-OXCMSG, will delete these rop here
                        case RopIdType.RopCreateFolder:
                            RopCreateFolderResponse RopCreateFolderResponse = new RopCreateFolderResponse();
                            RopCreateFolderResponse.Parse(s);
                            ropsList.Add(RopCreateFolderResponse);
                            break;
                        case RopIdType.RopCreateMessage:
                            RopCreateMessageResponse RopCreateMessageResponse = new RopCreateMessageResponse();
                            RopCreateMessageResponse.Parse(s);
                            ropsList.Add(RopCreateMessageResponse);
                            break;
                        case RopIdType.RopCreateAttachment:
                            RopCreateAttachmentResponse RopCreateAttachmentResponse = new RopCreateAttachmentResponse();
                            RopCreateAttachmentResponse.Parse(s);
                            ropsList.Add(RopCreateAttachmentResponse);
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

            this.RopsList = ropsList.ToArray();
            while (s.Position < s.Length)
            {
                uint ServerObjectHandle = ReadUint();
                serverObjectHandleTable.Add(ServerObjectHandle);
            }
            this.ServerObjectHandleTable = serverObjectHandleTable.ToArray();

            if (this.RopsList.Length > 0)
            {
                foreach (var ropResponse in this.RopsList)
                {

                    if(ropResponse is RopCreateFolderResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopCreateFolderResponse).OutputHandleIndex];
                        if (DecodingContext.SessionObjectHandles != null && DecodingContext.SessionObjectHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.SessionObjectHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        Dictionary<int, ObjectHandlesType> tmpObjectHandle = new Dictionary<int, ObjectHandlesType>() { { (int)objectHandleKey, ObjectHandlesType.FolderHandles } };
                        DecodingContext.SessionObjectHandles.Add(MapiInspector.MAPIInspector.currentSessionID, tmpObjectHandle);
                        if((DecodingContext.CopyTo_InputHandles != null && DecodingContext.CopyTo_InputHandles.ContainsValue((int)objectHandleKey))
                            || DecodingContext.CopyProperties_InputHandles != null && DecodingContext.CopyProperties_InputHandles.ContainsValue((int)objectHandleKey)
                            || DecodingContext.DestinationConfigure_InputHandles != null && DecodingContext.DestinationConfigure_InputHandles.ContainsValue((int)objectHandleKey))
                        {
                            if (DecodingContext.ObjectHandles != null && DecodingContext.ObjectHandles.ContainsKey((int)objectHandleKey))
                            {
                                DecodingContext.ObjectHandles.Remove((int)objectHandleKey);
                            }
                            DecodingContext.ObjectHandles.Add((int)objectHandleKey, ObjectHandlesType.FolderHandles);
                        }   
                    }
                   else if (ropResponse is RopCreateMessageResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopCreateMessageResponse).OutputHandleIndex];
                        if (DecodingContext.SessionObjectHandles != null && DecodingContext.SessionObjectHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.SessionObjectHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        Dictionary<int, ObjectHandlesType> tmpObjectHandle = new Dictionary<int, ObjectHandlesType>() { { (int)objectHandleKey, ObjectHandlesType.MessageHandles } };
                        DecodingContext.SessionObjectHandles.Add(MapiInspector.MAPIInspector.currentSessionID, tmpObjectHandle);
                        if ((DecodingContext.CopyTo_InputHandles != null && DecodingContext.CopyTo_InputHandles.ContainsValue((int)objectHandleKey))
                            || DecodingContext.CopyProperties_InputHandles != null && DecodingContext.CopyProperties_InputHandles.ContainsValue((int)objectHandleKey)
                            || DecodingContext.DestinationConfigure_InputHandles != null && DecodingContext.DestinationConfigure_InputHandles.ContainsValue((int)objectHandleKey))
                        {
                            if (DecodingContext.ObjectHandles != null && DecodingContext.ObjectHandles.ContainsKey((int)objectHandleKey))
                            {
                                DecodingContext.ObjectHandles.Remove((int)objectHandleKey);
                            }
                            DecodingContext.ObjectHandles.Add((int)objectHandleKey, ObjectHandlesType.MessageHandles);
                        }   
                   }
                   else if (ropResponse is RopCreateAttachmentResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopCreateAttachmentResponse).OutputHandleIndex];
                        if (DecodingContext.SessionObjectHandles != null && DecodingContext.SessionObjectHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.SessionObjectHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        Dictionary<int, ObjectHandlesType> tmpObjectHandle = new Dictionary<int, ObjectHandlesType>() { { (int)objectHandleKey, ObjectHandlesType.AttachmentHandles } };
                        DecodingContext.SessionObjectHandles.Add(MapiInspector.MAPIInspector.currentSessionID, tmpObjectHandle);

                        if ((DecodingContext.CopyTo_InputHandles != null && DecodingContext.CopyTo_InputHandles.ContainsValue((int)objectHandleKey))
                            || DecodingContext.CopyProperties_InputHandles != null && DecodingContext.CopyProperties_InputHandles.ContainsValue((int)objectHandleKey)
                            || DecodingContext.DestinationConfigure_InputHandles != null && DecodingContext.DestinationConfigure_InputHandles.ContainsValue((int)objectHandleKey))
                        {
                            if (DecodingContext.ObjectHandles != null && DecodingContext.ObjectHandles.ContainsKey((int)objectHandleKey))
                            {
                                DecodingContext.ObjectHandles.Remove((int)objectHandleKey);
                            }
                            DecodingContext.ObjectHandles.Add((int)objectHandleKey, ObjectHandlesType.AttachmentHandles);
                        }   
                   }
                   else if (ropResponse is RopFastTransferSourceCopyToResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopFastTransferSourceCopyToResponse).OutputHandleIndex];
                        if (DecodingContext.CopyTo_OutputHandles != null && DecodingContext.CopyTo_OutputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.CopyTo_OutputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.CopyTo_OutputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                   else if (ropResponse is RopFastTransferSourceCopyPropertiesResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopFastTransferSourceCopyPropertiesResponse).OutputHandleIndex];
                        if (DecodingContext.CopyProperties_OutputHandles != null && DecodingContext.CopyProperties_OutputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.CopyProperties_OutputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.CopyProperties_OutputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                   else if (ropResponse is RopFastTransferSourceCopyMessagesResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopFastTransferSourceCopyMessagesResponse).OutputHandleIndex];
                        if (DecodingContext.CopyMessage_OutputHandles != null && DecodingContext.CopyMessage_OutputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.CopyMessage_OutputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.CopyMessage_OutputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                   else if (ropResponse is RopFastTransferSourceCopyFolderResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopFastTransferSourceCopyFolderResponse).OutputHandleIndex];
                        if (DecodingContext.CopyFolder_OutputHandles != null && DecodingContext.CopyFolder_OutputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.CopyFolder_OutputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.CopyFolder_OutputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                   else if (ropResponse is RopSynchronizationConfigureResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopSynchronizationConfigureResponse).OutputHandleIndex];
                        if (DecodingContext.SyncConfigure_OutputHandles != null && DecodingContext.SyncConfigure_OutputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.SyncConfigure_OutputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.SyncConfigure_OutputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                   else if (ropResponse is RopSynchronizationGetTransferStateResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopSynchronizationGetTransferStateResponse).OutputHandleIndex];
                        if (DecodingContext.SyncGetTransferState_OutputHandles != null && DecodingContext.SyncGetTransferState_OutputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.SyncGetTransferState_OutputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.SyncGetTransferState_OutputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                    else if (ropResponse is RopFastTransferDestinationConfigureResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopFastTransferDestinationConfigureResponse).OutputHandleIndex];
                        if (DecodingContext.DestinationConfigure_OutputHandles != null && DecodingContext.DestinationConfigure_OutputHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.DestinationConfigure_OutputHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.DestinationConfigure_OutputHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                   else if (ropResponse is RopFastTransferDestinationPutBufferResponse)
                    {
                        uint objectHandleKey = this.ServerObjectHandleTable[(ropResponse as RopFastTransferDestinationPutBufferResponse).InputHandleIndex];

                        if (DecodingContext.PutBuffer_InPutHandles.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                        {
                            DecodingContext.PutBuffer_InPutHandles.Remove(MapiInspector.MAPIInspector.currentSessionID);
                        }
                        DecodingContext.PutBuffer_InPutHandles.Add(MapiInspector.MAPIInspector.currentSessionID, (int)objectHandleKey);
                    }
                    else
                    {
                        continue;
                    }
                }   
            }
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
        /// <param name="s">An stream containing RopSubmitMessageRequest structure.</param>
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
        /// <param name="s">An stream containing RopSubmitMessageResponse structure.</param>
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
        /// <param name="s">An stream containing RopAbortSubmitRequest structure.</param>
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
        /// <param name="s">An stream containing RopAbortSubmitResponse structure.</param>
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
        /// <param name="s">An stream containing RopGetAddressTypesRequest structure.</param>
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
        [HelpAttribute(StringEncoding.ASCII, false, 1)]
        public string[] AddressTypes;

        /// <summary>
        /// Parse the RopGetAddressTypesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetAddressTypesResponse structure.</param>
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
                this.AddressTypes = new string[(int)this.AddressTypeCount];

                for (int i = 0; i < this.AddressTypeCount; i++)
                {
                    string AddressType = ReadString();
                    this.AddressTypes[i] = AddressType;
                }

                ModifyIsExistAttribute(this, "AddressTypes");
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
        /// <param name="s">An stream containing RopSetSpoolerRequest structure.</param>
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
        /// <param name="s">An stream containing RopSetSpoolerResponse structure.</param>
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
        /// <param name="s">An stream containing RopSpoolerLockMessageRequest structure.</param>
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
        /// <param name="s">An stream containing RopSpoolerLockMessageResponse structure.</param>
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
        /// <param name="s">An stream containing RopTransportSendRequest structure.</param>
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
        /// <param name="s">An stream containing RopTransportSendResponse structure.</param>
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
        [HelpAttribute(StringEncoding.ASCII, true, 1)]
        public string MessageClass;

        // A flags structure that contains the message flags of the new message object.
        public MessageFlags MessageFlags;

        /// <summary>
        /// Parse the RopTransportNewMailRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopTransportNewMailRequest structure.</param>
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
            this.MessageClass = ReadString();
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
        /// <param name="s">An stream containing RopTransportNewMailResponse structure.</param>
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
        /// <param name="s">An stream containing RopGetTransportFolderRequest structure.</param>
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
        /// <param name="s">An stream containing RopGetTransportFolderResponse structure.</param>
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
        [HelpAttribute(StringEncoding.ASCII, true, 1)]
        public string AddressType;

        // A boolean that specifies whether the help file data is to be returned in a format that is suited for 32-bit machines.
        public byte WantWin32;

        /// <summary>
        /// Parse the RopOptionsDataRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopOptionsDataRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.AddressType = ReadString();
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
        [HelpAttribute(StringEncoding.ASCII, false, 2)]
        public string HelpFileName;

        /// <summary>
        /// Parse the RopOptionsDataResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopOptionsDataResponse structure.</param>
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
                    this.HelpFileName = ReadString();
                    ModifyIsExistAttribute(this, "HelpFileName");
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

        /// <summary>
        /// Parse the RopBufferTooSmallResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopBufferTooSmallResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SizeNeeded = ReadUshort();
            // TODO 
            this.RequestBuffers = ReadBytes(SizeNeeded);
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
        /// <param name="s">An stream containing RopBackoffResponse structure.</param>
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
        /// <param name="s">An stream containing BackoffRop structure.</param>
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
        /// <param name="s">An stream containing RopReleaseResquest structure.</param>
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
        // Flags that control the behavior of the logon.
        private static LogonFlags logOnFlags;

        // Indicate the type of FastTransferStream for RopFastTransferSourceGetBufferResponse.
        private static FastTransferStreamType streamType_Getbuffer;

        // Indicate the type of FastTransferStream for RopFastTransferDestinationPutBufferRequest.
        private static FastTransferStreamType streamType_Putbuffer;
        
        // Record FastTransferStream type because this session.
        private static Dictionary<int, FastTransferStreamType> sessionFastTransferStreamType;

        // Record current session logon flags.
        private static Dictionary<int, LogonFlags> sessionLogonFlag;

        // Record the LogonId and logon flags.
        private static Dictionary<byte, LogonFlags> logonFlagMapLogId;

        // Record current session(RopFastTransferSourceGetBuffer) InputObjectHandle.
        private static Dictionary<int, int> getBuffer_InPutHandles;

        //Record current session(RopFastTransferDestinationPutBuffer) InputObjectHandle.
        private static Dictionary<int, int> putBuffer_InPutHandles;

        // Record current session(RopSynchronizationConfigure) OutputObjectHandle.
        private static Dictionary<int, int> syncConfigure_OutputHandles;

        // Record current session(RopFastTransferSourceCopyTo) InputObjectHandle.
        private static Dictionary<int, int> copyTo_InputHandles;

        // Record current session(RopFastTransferSourceCopyProperties) InputObjectHandle.
        private static Dictionary<int, int> copyProperties_InputHandles;

        // Record current session(RopFastTransferSourceCopyTo) OutputObjectHandle.
        private static Dictionary<int, int> copyTo_OutputHandles;

        // Record current session(RopFastTransferSourceCopyProperties) OutputObjectHandle.
        private static Dictionary<int, int> copyProperties_OutputHandles;

        // Record current session(RopFastTransferSourceCopyMessages) OutputObjectHandle.
        private static Dictionary<int, int> copyMessage_OutputHandles;

        // Record current session(RopFastTransferSourceCopyFolder) OutputObjectHandle.
        private static Dictionary<int, int> copyFolder_OutputHandles;

        // Record current session(RopSynchronizationGetTransferState) OutputObjectHandle.
        private static Dictionary<int, int> syncGetTransferState_OutputHandles;

        // Record current session(RopFastTransferDestinationConfigure) InputObjectHandle.
        private static Dictionary<int, int> destinationConfigure_InputHandles;

        // Record current session(RopFastTransferDestinationConfigure) OutputObjectHandle.
        private static Dictionary<int, int> destinationConfigure_OutputHandles;

        // Record the map in session information and object handles value and type.
        private static Dictionary<int, Dictionary<int, ObjectHandlesType>> sessionObjectHandles;

        // Record object handles value and type, contains FolderHandles, MessageHandles and AttachmentHandles.
        private static Dictionary<int, ObjectHandlesType> objectHandles;

        public DecodingContext()
        {
            objectHandles = new Dictionary<int, ObjectHandlesType>();
            sessionFastTransferStreamType = new Dictionary<int, FastTransferStreamType>();
            streamType_Getbuffer = new FastTransferStreamType();
            getBuffer_InPutHandles = new Dictionary<int, int>();
            putBuffer_InPutHandles = new Dictionary<int,int>();
            copyTo_InputHandles = new Dictionary<int, int>();
            copyProperties_InputHandles = new Dictionary<int, int>();
            copyTo_OutputHandles = new Dictionary<int, int>();
            copyProperties_OutputHandles = new Dictionary<int, int>();
            copyMessage_OutputHandles = new Dictionary<int, int>();
            copyFolder_OutputHandles = new Dictionary<int, int>();
            syncGetTransferState_OutputHandles = new Dictionary<int, int>();
            syncConfigure_OutputHandles = new Dictionary<int, int>();
            destinationConfigure_OutputHandles = new Dictionary<int,int>();
            destinationConfigure_InputHandles = new Dictionary<int, int>();
            sessionObjectHandles = new Dictionary<int, Dictionary<int, ObjectHandlesType>>();
        }

        // Gets or sets the logOnFlags.
        public static LogonFlags LogonFlags
        {
            get
            {
                return logOnFlags;
            }
            set
            {
                logOnFlags = value;
            }
        }

        // Gets or sets the session logon flags
        public static Dictionary<int, LogonFlags> SessionLogonFlag
        {
            get
            {
                return sessionLogonFlag;
            }
            set
            {
                sessionLogonFlag = value;
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

        // Gets or sets the getBuffer_InPutHandles
        public static Dictionary<int, int> GetBuffer_InPutHandles
        {
            get
            {
                return getBuffer_InPutHandles;
            }
            set
            {
                getBuffer_InPutHandles = value;
            }
        }

        // Gets or sets the putBuffer_InPutHandles
        public static Dictionary<int, int> PutBuffer_InPutHandles
        {
            get
            {
                return putBuffer_InPutHandles;
            }
            set
            {
                putBuffer_InPutHandles = value;
            }
        }

        // Gets or sets the syncConfigure_OutputHandles
        public static Dictionary<int, int> SyncConfigure_OutputHandles
        {
            get
            {
                return syncConfigure_OutputHandles;
            }
            set
            {
                syncConfigure_OutputHandles = value;
            }
        }

        // Gets or sets the CopyTo_InputHandles
        public static Dictionary<int, int> CopyTo_InputHandles
        {
            get
            {
                return copyTo_InputHandles;
            }
            set
            {
                copyTo_InputHandles = value;
            }
        }

        // Gets or sets the copyProperties_InputHandles 
        public static Dictionary<int, int> CopyProperties_InputHandles 
        {
            get
            {
                return copyProperties_InputHandles ;
            }
            set
            {
                copyProperties_InputHandles  = value;
            }
        }

        // Gets or sets the copyTo_OutputHandles
        public static Dictionary<int, int> CopyTo_OutputHandles
        {
            get
            {
                return copyTo_OutputHandles;
            }
            set
            {
                copyTo_OutputHandles = value;
            }
        }

        // Gets or sets the copyProperties_OutputHandles
        public static Dictionary<int, int> CopyProperties_OutputHandles
        {
            get
            {
                return copyProperties_OutputHandles;
            }
            set
            {
                copyProperties_OutputHandles = value;
            }
        }

        // Gets or sets the copyMessage_OutputHandles
        public static Dictionary<int, int> CopyMessage_OutputHandles
        {
            get
            {
                return copyMessage_OutputHandles;
            }
            set
            {
                copyMessage_OutputHandles = value;
            }
        }
        // Gets or sets the copyFolder_OutputHandles
        public static Dictionary<int, int> CopyFolder_OutputHandles
        {
            get
            {
                return copyFolder_OutputHandles;
            }
            set
            {
                copyFolder_OutputHandles = value;
            }
        }

        // Gets or sets the syncGetTransferState_OutputHandles
        public static Dictionary<int, int> SyncGetTransferState_OutputHandles
        {
            get
            {
                return syncGetTransferState_OutputHandles;
            }
            set
            {
                syncGetTransferState_OutputHandles = value;
            }
        }

        // Gets or sets the destinationConfigure_InputHandles
        public static Dictionary<int, int> DestinationConfigure_InputHandles
        {
            get
            {
                return destinationConfigure_InputHandles;
            }
            set
            {
                destinationConfigure_InputHandles = value;
            }
        }

        // Gets or sets the destinationConfigure_OutputHandles
        public static Dictionary<int, int> DestinationConfigure_OutputHandles
        {
            get
            {
                return destinationConfigure_OutputHandles;
            }
            set
            {
                destinationConfigure_OutputHandles = value;
            }
        }

        // Gets or sets the objectHandles
        public static Dictionary<int, ObjectHandlesType> ObjectHandles
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

        // Gets or sets the sessionObjectHandles;
        public static Dictionary<int, Dictionary<int, ObjectHandlesType>> SessionObjectHandles
        {
            get
            {
                return sessionObjectHandles;
            }
            set
            {
                sessionObjectHandles = value;
            }
        }

        // Gets or sets the sessionFastTransferStreamType.
        public static Dictionary<int, FastTransferStreamType> SessionFastTransferStreamType
        {
            get
            {
                return sessionFastTransferStreamType;
            }
            set
            {
                sessionFastTransferStreamType = value;
            }
        }

        // Gets or sets the streamType_Getbuffer.
        public static FastTransferStreamType StreamType_Getbuffer
        {
            get
            {
                return streamType_Getbuffer;
            }
            set
            {
                streamType_Getbuffer = value;
            }
        }

        // Gets or sets the streamType_Putbuffer.
        public static FastTransferStreamType StreamType_Putbuffer
        {
            get
            {
                return streamType_Putbuffer;
            }
            set
            {
                streamType_Putbuffer = value;
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
        public object Parameters;

        public MissingInformationException(string message, ushort ropID, object parameter)
        {
            this.ErrorMessage = message;
            this.RopID = ropID;
            this.Parameters = parameter;
        }
    }
}
