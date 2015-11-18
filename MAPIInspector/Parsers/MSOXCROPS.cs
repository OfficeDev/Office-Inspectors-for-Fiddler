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
                            // Record the property tags.
                            DecodingContext.SetColumnsPropertyTags = new Dictionary<int, PropertyTag[]>() { { MapiInspector.MAPIInspector.currentSessionID, RopSetColumnsRequest.PropertyTags } };
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

                        // MSOXCFOLD Rop
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
                            // If this session alreadby is successfully parsed, get it from the dictionary.
                            if (DecodingContext.ColumnsRelatedRops != null && DecodingContext.ColumnsRelatedRops.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {

                                RopQueryRowsResponse RopQueryRowsResponse = (RopQueryRowsResponse)DecodingContext.ColumnsRelatedRops[MapiInspector.MAPIInspector.currentSessionID];
                                ropsList.Add(RopQueryRowsResponse);
                                s.Position += RopSize;
                            }
                            // If the related property tags is alreadby in dictionary and this session is not parsed
                            else if (DecodingContext.SetColumnsPropertyTags != null && DecodingContext.SetColumnsPropertyTags.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                RopQueryRowsResponse RopQueryRowsResponse = new RopQueryRowsResponse(DecodingContext.SetColumnsPropertyTags[MapiInspector.MAPIInspector.currentSessionID]);
                                RopQueryRowsResponse.Parse(s);
                                ropsList.Add(RopQueryRowsResponse);
                                DecodingContext.ColumnsRelatedRops = new Dictionary<int, object> { { MapiInspector.MAPIInspector.currentSessionID, RopQueryRowsResponse } };
                            }
                            // If the related property tags is not exist.
                            else
                            {
                                throw new MissingInformationException("Missing LogonFlags information for RopLogon", (ushort)CurrentByte, null);
                            }
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
                            // If this session alreadby is successfully parsed, get it from the dictionary.
                            if (DecodingContext.ColumnsRelatedRops != null && DecodingContext.ColumnsRelatedRops.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {

                                RopFindRowResponse RopFindRowResponse = (RopFindRowResponse)DecodingContext.ColumnsRelatedRops[MapiInspector.MAPIInspector.currentSessionID];
                                ropsList.Add(RopFindRowResponse);
                                s.Position += RopSize;
                            }
                            // If the related property tags is alreadby in dictionary and this session is not parsed
                            else if (DecodingContext.SetColumnsPropertyTags != null && DecodingContext.SetColumnsPropertyTags.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                RopFindRowResponse RopFindRowResponse = new RopFindRowResponse(DecodingContext.SetColumnsPropertyTags[MapiInspector.MAPIInspector.currentSessionID]);
                                RopFindRowResponse.Parse(s);
                                ropsList.Add(RopFindRowResponse);
                                DecodingContext.ColumnsRelatedRops = new Dictionary<int, object> { { MapiInspector.MAPIInspector.currentSessionID, RopFindRowResponse } };
                            }
                            // If the related property tags is not exist.
                            else
                            {
                                throw new MissingInformationException("Missing LogonFlags information for RopLogon", (ushort)CurrentByte, null);
                            }
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
                            // If this session alreadby is successfully parsed, get it from the dictionary.
                            if (DecodingContext.ColumnsRelatedRops != null && DecodingContext.ColumnsRelatedRops.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {

                                RopExpandRowResponse RopExpandRowResponse = (RopExpandRowResponse)DecodingContext.ColumnsRelatedRops[MapiInspector.MAPIInspector.currentSessionID];
                                ropsList.Add(RopExpandRowResponse);
                                s.Position += RopSize;
                            }
                            // If the related property tags is alreadby in dictionary and this session is not parsed
                            else if (DecodingContext.SetColumnsPropertyTags != null && DecodingContext.SetColumnsPropertyTags.ContainsKey(MapiInspector.MAPIInspector.currentSessionID))
                            {
                                RopExpandRowResponse RopExpandRowResponse = new RopExpandRowResponse(DecodingContext.SetColumnsPropertyTags[MapiInspector.MAPIInspector.currentSessionID]);
                                RopExpandRowResponse.Parse(s);
                                ropsList.Add(RopExpandRowResponse);
                                DecodingContext.ColumnsRelatedRops = new Dictionary<int, object> { { MapiInspector.MAPIInspector.currentSessionID, RopExpandRowResponse } };
                            }
                            // If the related property tags is not exist.
                            else
                            {
                                throw new MissingInformationException("Missing SetColumns PropertyTags information for RopLogon", (ushort)CurrentByte, null);
                            }
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

                        // MSOXCFOLD Rop
                        case RopIdType.RopOpenFolder:
                            RopOpenFolderResponse RopOpenFolderResponse = new RopOpenFolderResponse();
                            RopOpenFolderResponse.Parse(s);
                            ropsList.Add(RopOpenFolderResponse);
                            break;

                        case RopIdType.RopCreateFolder:
                            RopCreateFolderResponse RopCreateFolderResponse = new RopCreateFolderResponse();
                            RopCreateFolderResponse.Parse(s);
                            ropsList.Add(RopCreateFolderResponse);
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

        // TODO: An identifier that identifies the folder in which the submitted message is located.
        public ulong FolderId;

        // TODO: An identifier that specifies the submitted message.
        public ulong MessageId;

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
            this.FolderId = ReadUlong();
            this.MessageId = ReadUlong();
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
        public uint ReturnValue;

        /// <summary>
        /// Parse the RopAbortSubmitResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopAbortSubmitResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();
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
        public uint ReturnValue;

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
            this.ReturnValue = ReadUint();

            if (ReturnValue == 0)
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
        public uint ReturnValue;

        /// <summary>
        /// Parse the RopSetSpoolerResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetSpoolerResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();
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

        // TODO: An identifier that specifies the message for which the status will be changed.
        public ulong MessageId;

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
            this.MessageId = ReadUlong();
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
        public uint ReturnValue;

        /// <summary>
        /// Parse the RopSpoolerLockMessageResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSpoolerLockMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();
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
        public uint ReturnValue;

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
            this.ReturnValue = ReadUint();

            if (this.ReturnValue == 0)
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

        // TODO: An identifier that specifies the new message object.
        public ulong MessageId;

        // TODO: An identifier that identifies the folder of the new message object.
        public ulong FolderId;

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
            this.MessageId = ReadUlong();
            this.FolderId = ReadUlong();
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
        public uint ReturnValue;

        /// <summary>
        /// Parse the RopTransportNewMailResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopTransportNewMailResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();
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
        public uint ReturnValue;

        // An identifier that specifies the transport folder.
        public ulong? FolderId;

        /// <summary>
        /// Parse the RopGetTransportFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetTransportFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();

            if (this.ReturnValue == 0)
            {
                this.FolderId = ReadUlong();
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
        public uint ReturnValue;

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
            this.ReturnValue = ReadUint();

            if (this.ReturnValue == 0)
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

        // Record current session logon flags.
        private static Dictionary<int, LogonFlags> sessionLogonFlag;

        // Record the LogonId and logon flags.
        private static Dictionary<byte, LogonFlags> logonFlagMapLogId;

        // Record the SetColumns's property tags.
        private static Dictionary<int, PropertyTag[]> setColumnsPropertyTags;

        // Record the roplist related to SetColumns's property tags.
        private static Dictionary<int, object> columnsRelatedRops;

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

        // Get or set setColumnsPropertyTags
        public static Dictionary<int, PropertyTag[]> SetColumnsPropertyTags
        {
            get
            {
                return setColumnsPropertyTags;
            }
            set
            {
                setColumnsPropertyTags = value;
            }
        }

        // Get or set columnsRelatedRops
        public static Dictionary<int, object> ColumnsRelatedRops
        {
            get
            {
                return columnsRelatedRops;
            }
            set
            {
                columnsRelatedRops = value;
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
