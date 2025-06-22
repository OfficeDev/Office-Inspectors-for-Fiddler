namespace MAPIInspector.Parsers
{
    using BlockParser;
    using Fiddler;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    #region Enums defined in MS-OXOMSG

    /// <summary>
    /// The enum type for flags indicates the status of a message object.
    /// </summary>
    [Flags]
    public enum MessageFlags : uint
    {
        /// <summary>
        /// mfRead flag
        /// </summary>
        mfRead = 0x00000001,

        /// <summary>
        /// mfUnsent flag
        /// </summary>
        mfUnsent = 0x00000008,

        /// <summary>
        /// mfResend flag
        /// </summary>
        mfResend = 0x00000080
    }

    /// <summary>
    /// The enum type for flags indicates how the message is to be delivered.
    /// </summary>
    public enum SubmitFlags : byte
    {
        /// <summary>
        /// No special behavior is specified
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The message needs to be preprocessed by the server.
        /// </summary>
        PreProcess = 0x01,

        /// <summary>
        /// The message is to be processed by a client spooler.
        /// </summary>
        NeedsSpooler = 0x02
    }

    /// <summary>
    /// The enum type for flags specifies a status to set on a message.
    /// </summary>
    public enum LockState : byte
    {
        /// <summary>
        /// Mark the message as locked.
        /// </summary>
        IstLock = 0x00,

        /// <summary>
        /// Mark the message as unlocked.
        /// </summary>
        IstUnlock = 0x01,

        /// <summary>
        /// Mark the message as ready for processing by the server. 
        /// </summary>
        IstFininshed = 0x02
    }

    #endregion

    #region 2.2.2 RopIds
    /// <summary>
    /// The enum type for RopIds.
    /// </summary>
    public enum RopIdType : byte
    {
        /// <summary>
        /// RopRelease ROP
        /// </summary>
        RopRelease = 0x01,

        /// <summary>
        /// RopOpenFolder ROP 
        /// </summary>
        RopOpenFolder = 0x02,

        /// <summary>
        /// RopOpenMessage ROP
        /// </summary>
        RopOpenMessage = 0x03,

        /// <summary>
        /// RopGetHierarchyTable ROP
        /// </summary>
        RopGetHierarchyTable = 0x04,

        /// <summary>
        /// RopGetContentsTable ROP
        /// </summary>
        RopGetContentsTable = 0x05,

        /// <summary>
        /// RopCreateMessage ROP
        /// </summary>
        RopCreateMessage = 0x06,

        /// <summary>
        /// RopGetPropertiesSpecific ROP
        /// </summary>
        RopGetPropertiesSpecific = 0x07,

        /// <summary>
        /// RopGetPropertiesAll ROP
        /// </summary>
        RopGetPropertiesAll = 0x08,

        /// <summary>
        /// RopGetPropertiesList ROP
        /// </summary>
        RopGetPropertiesList = 0x09,

        /// <summary>
        /// RopSetProperties ROP
        /// </summary>
        RopSetProperties = 0x0A,

        /// <summary>
        /// RopDeleteProperties ROP
        /// </summary>
        RopDeleteProperties = 0x0B,

        /// <summary>
        /// RopSaveChangesMessage ROP
        /// </summary>
        RopSaveChangesMessage = 0x0C,

        /// <summary>
        /// RopRemoveAllRecipients ROP
        /// </summary>
        RopRemoveAllRecipients = 0x0D,

        /// <summary>
        /// RopModifyRecipients ROP
        /// </summary>
        RopModifyRecipients = 0x0E,

        /// <summary>
        /// RopReadRecipients ROP
        /// </summary>
        RopReadRecipients = 0x0F,

        /// <summary>
        /// RopReloadCachedInformation ROP
        /// </summary>
        RopReloadCachedInformation = 0x10,

        /// <summary>
        /// RopSetMessageReadFlag ROP
        /// </summary>
        RopSetMessageReadFlag = 0x11,

        /// <summary>
        /// RopSetColumns ROP
        /// </summary>
        RopSetColumns = 0x12,

        /// <summary>
        /// RopSortTable ROP
        /// </summary>
        RopSortTable = 0x13,

        /// <summary>
        /// RopRestrict ROP
        /// </summary>
        RopRestrict = 0x14,

        /// <summary>
        /// RopQueryRows ROP
        /// </summary>
        RopQueryRows = 0x15,

        /// <summary>
        /// RopGetStatus ROP
        /// </summary>
        RopGetStatus = 0x16,

        /// <summary>
        /// RopQueryPosition ROP
        /// </summary>
        RopQueryPosition = 0x17,

        /// <summary>
        /// RopSeekRow ROP
        /// </summary>
        RopSeekRow = 0x18,

        /// <summary>
        /// RopSeekRowBookmark ROP
        /// </summary>
        RopSeekRowBookmark = 0x19,

        /// <summary>
        /// RopSeekRowFractional ROP
        /// </summary>
        RopSeekRowFractional = 0x1A,

        /// <summary>
        /// RopCreateBookmark ROP
        /// </summary>
        RopCreateBookmark = 0x1B,

        /// <summary>
        /// RopCreateFolder ROP
        /// </summary>
        RopCreateFolder = 0x1C,

        /// <summary>
        /// RopDeleteFolder ROP
        /// </summary>
        RopDeleteFolder = 0x1D,

        /// <summary>
        /// RopDeleteMessages ROP
        /// </summary>
        RopDeleteMessages = 0x1E,

        /// <summary>
        /// RopGetMessageStatus ROP
        /// </summary>
        RopGetMessageStatus = 0x1F,

        /// <summary>
        /// RopSetMessageStatus ROP
        /// </summary>
        RopSetMessageStatus = 0x20,

        /// <summary>
        /// RopGetAttachmentTable ROP
        /// </summary>
        RopGetAttachmentTable = 0x21,

        /// <summary>
        /// RopOpenAttachment ROP
        /// </summary>
        RopOpenAttachment = 0x22,

        /// <summary>
        /// RopCreateAttachment ROP
        /// </summary>
        RopCreateAttachment = 0x23,

        /// <summary>
        /// RopDeleteAttachment ROP
        /// </summary>
        RopDeleteAttachment = 0x24,

        /// <summary>
        /// RopSaveChangesAttachment ROP
        /// </summary>
        RopSaveChangesAttachment = 0x25,

        /// <summary>
        /// RopSetReceiveFolder ROP
        /// </summary>
        RopSetReceiveFolder = 0x26,

        /// <summary>
        /// RopGetReceiveFolder ROP
        /// </summary>
        RopGetReceiveFolder = 0x27,

        /// <summary>
        /// RopRegisterNotification ROP
        /// </summary>
        RopRegisterNotification = 0x29,

        /// <summary>
        /// RopNotify ROP
        /// </summary>
        RopNotify = 0x2A,

        /// <summary>
        /// RopOpenStream ROP
        /// </summary>
        RopOpenStream = 0x2B,

        /// <summary>
        /// RopReadStream ROP
        /// </summary>
        RopReadStream = 0x2C,

        /// <summary>
        /// RopWriteStream ROP
        /// </summary>
        RopWriteStream = 0x2D,

        /// <summary>
        /// RopSeekStream ROP
        /// </summary>
        RopSeekStream = 0x2E,

        /// <summary>
        /// RopSetStreamSize ROP
        /// </summary>
        RopSetStreamSize = 0x2F,

        /// <summary>
        /// RopSetSearchCriteria ROP
        /// </summary>
        RopSetSearchCriteria = 0x30,

        /// <summary>
        /// RopGetSearchCriteria ROP
        /// </summary>
        RopGetSearchCriteria = 0x31,

        /// <summary>
        /// RopSubmitMessage ROP
        /// </summary>
        RopSubmitMessage = 0x32,

        /// <summary>
        /// RopMoveCopyMessages ROP
        /// </summary>
        RopMoveCopyMessages = 0x33,

        /// <summary>
        /// RopAbortSubmit ROP
        /// </summary>
        RopAbortSubmit = 0x34,

        /// <summary>
        /// RopMoveFolder ROP
        /// </summary>
        RopMoveFolder = 0x35,

        /// <summary>
        /// RopCopyFolder ROP
        /// </summary>
        RopCopyFolder = 0x36,

        /// <summary>
        /// RopQueryColumnsAll ROP
        /// </summary>
        RopQueryColumnsAll = 0x37,

        /// <summary>
        /// RopAbort ROP
        /// </summary>
        RopAbort = 0x38,

        /// <summary>
        /// RopCopyTo ROP
        /// </summary>
        RopCopyTo = 0x39,

        /// <summary>
        /// RopCopyToStream ROP
        /// </summary>
        RopCopyToStream = 0x3A,

        /// <summary>
        /// RopCloneStream ROP
        /// </summary>
        RopCloneStream = 0x3B,

        /// <summary>
        /// RopGetPermissionsTable ROP
        /// </summary>
        RopGetPermissionsTable = 0x3E,

        /// <summary>
        /// RopGetRulesTable ROP
        /// </summary>
        RopGetRulesTable = 0x3F,

        /// <summary>
        /// RopModifyPermissions ROP
        /// </summary>
        RopModifyPermissions = 0x40,

        /// <summary>
        /// RopModifyRules ROP
        /// </summary>
        RopModifyRules = 0x41,

        /// <summary>
        /// RopGetOwningServers ROP
        /// </summary>
        RopGetOwningServers = 0x42,

        /// <summary>
        /// RopLongTermIdFromId ROP
        /// </summary>
        RopLongTermIdFromId = 0x43,

        /// <summary>
        /// RopIdFromLongTermId ROP
        /// </summary>
        RopIdFromLongTermId = 0x44,

        /// <summary>
        /// RopPublicFolderIsGhosted ROP
        /// </summary>
        RopPublicFolderIsGhosted = 0x45,

        /// <summary>
        /// RopOpenEmbeddedMessage ROP
        /// </summary>
        RopOpenEmbeddedMessage = 0x46,

        /// <summary>
        /// RopSetSpooler ROP
        /// </summary>
        RopSetSpooler = 0x47,

        /// <summary>
        /// RopSpoolerLockMessage ROP
        /// </summary>
        RopSpoolerLockMessage = 0x48,

        /// <summary>
        /// RopGetAddressTypes ROP
        /// </summary>
        RopGetAddressTypes = 0x49,

        /// <summary>
        /// RopTransportSend ROP
        /// </summary>
        RopTransportSend = 0x4A,

        /// <summary>
        /// RopFastTransferSourceCopyMessages ROP
        /// </summary>
        RopFastTransferSourceCopyMessages = 0x4B,

        /// <summary>
        /// RopFastTransferSourceCopyFolder ROP
        /// </summary>
        RopFastTransferSourceCopyFolder = 0x4C,

        /// <summary>
        /// RopFastTransferSourceCopyTo ROP
        /// </summary>
        RopFastTransferSourceCopyTo = 0x4D,

        /// <summary>
        /// RopFastTransferSourceGetBuffer ROP
        /// </summary>
        RopFastTransferSourceGetBuffer = 0x4E,

        /// <summary>
        /// RopFindRow ROP
        /// </summary>
        RopFindRow = 0x4F,

        /// <summary>
        /// RopProgress ROP
        /// </summary>
        RopProgress = 0x50,

        /// <summary>
        /// RopTransportNewMail ROP
        /// </summary>
        RopTransportNewMail = 0x51,

        /// <summary>
        /// RopGetValidAttachments ROP
        /// </summary>
        RopGetValidAttachments = 0x52,

        /// <summary>
        /// RopFastTransferDestinationConfigure ROP
        /// </summary>
        RopFastTransferDestinationConfigure = 0x53,

        /// <summary>
        /// RopFastTransferDestinationPutBuffer ROP
        /// </summary>
        RopFastTransferDestinationPutBuffer = 0x54,

        /// <summary>
        /// RopGetNamesFromPropertyIds ROP
        /// </summary>
        RopGetNamesFromPropertyIds = 0x55,

        /// <summary>
        /// RopGetPropertyIdsFromNames ROP
        /// </summary>
        RopGetPropertyIdsFromNames = 0x56,

        /// <summary>
        /// RopUpdateDeferredActionMessages ROP
        /// </summary>
        RopUpdateDeferredActionMessages = 0x57,

        /// <summary>
        /// RopEmptyFolder ROP
        /// </summary>
        RopEmptyFolder = 0x58,

        /// <summary>
        /// RopExpandRow ROP
        /// </summary>
        RopExpandRow = 0x59,

        /// <summary>
        /// RopCollapseRow ROP
        /// </summary>
        RopCollapseRow = 0x5A,

        /// <summary>
        /// RopLockRegionStream ROP
        /// </summary>
        RopLockRegionStream = 0x5B,

        /// <summary>
        /// RopUnlockRegionStream ROP
        /// </summary>
        RopUnlockRegionStream = 0x5C,

        /// <summary>
        /// RopCommitStream ROP
        /// </summary>
        RopCommitStream = 0x5D,

        /// <summary>
        /// RopGetStreamSize ROP
        /// </summary>
        RopGetStreamSize = 0x5E,

        /// <summary>
        /// RopQueryNamedProperties ROP
        /// </summary>
        RopQueryNamedProperties = 0x5F,

        /// <summary>
        /// RopGetPerUserLongTermIds ROP
        /// </summary>
        RopGetPerUserLongTermIds = 0x60,

        /// <summary>
        /// RopGetPerUserGuid ROP
        /// </summary>
        RopGetPerUserGuid = 0x61,

        /// <summary>
        /// RopReadPerUserInformation ROP
        /// </summary>
        RopReadPerUserInformation = 0x63,

        /// <summary>
        /// RopWritePerUserInformation ROP
        /// </summary>
        RopWritePerUserInformation = 0x64,

        /// <summary>
        /// RopSetReadFlags ROP
        /// </summary>
        RopSetReadFlags = 0x66,

        /// <summary>
        /// RopCopyProperties ROP
        /// </summary>
        RopCopyProperties = 0x67,

        /// <summary>
        /// RopGetReceiveFolderTable ROP
        /// </summary>
        RopGetReceiveFolderTable = 0x68,

        /// <summary>
        /// RopFastTransferSourceCopyProperties ROP
        /// </summary>
        RopFastTransferSourceCopyProperties = 0x69,

        /// <summary>
        /// RopGetCollapseState ROP
        /// </summary>
        RopGetCollapseState = 0x6B,

        /// <summary>
        /// RopSetCollapseState ROP
        /// </summary>
        RopSetCollapseState = 0x6C,

        /// <summary>
        /// RopGetTransportFolder ROP
        /// </summary>
        RopGetTransportFolder = 0x6D,

        /// <summary>
        /// RopPending ROP
        /// </summary>
        RopPending = 0x6E,

        /// <summary>
        /// RopOptionsData ROP
        /// </summary>
        RopOptionsData = 0x6F,

        /// <summary>
        /// RopSynchronizationConfigure ROP
        /// </summary>
        RopSynchronizationConfigure = 0x70,

        /// <summary>
        /// RopSynchronizationImportMessageChange ROP
        /// </summary>
        RopSynchronizationImportMessageChange = 0x72,

        /// <summary>
        /// RopSynchronizationImportHierarchyChange ROP
        /// </summary>
        RopSynchronizationImportHierarchyChange = 0x73,

        /// <summary>
        /// RopSynchronizationImportDeletes ROP
        /// </summary>
        RopSynchronizationImportDeletes = 0x74,

        /// <summary>
        /// RopSynchronizationUploadStateStreamBegin ROP
        /// </summary>
        RopSynchronizationUploadStateStreamBegin = 0x75,

        /// <summary>
        /// RopSynchronizationUploadStateStreamContinue ROP 
        /// </summary>
        RopSynchronizationUploadStateStreamContinue = 0x76,

        /// <summary>
        /// RopSynchronizationUploadStateStreamEnd ROP
        /// </summary>
        RopSynchronizationUploadStateStreamEnd = 0x77,

        /// <summary>
        /// RopSynchronizationImportMessageMove ROP
        /// </summary>
        RopSynchronizationImportMessageMove = 0x78,

        /// <summary>
        /// RopSetPropertiesNoReplicate ROP
        /// </summary>
        RopSetPropertiesNoReplicate = 0x79,

        /// <summary>
        /// RopDeletePropertiesNoReplicate ROP
        /// </summary>
        RopDeletePropertiesNoReplicate = 0x7A,

        /// <summary>
        /// RopGetStoreState ROP
        /// </summary>
        RopGetStoreState = 0x7B,

        /// <summary>
        /// RopSynchronizationOpenCollector ROP
        /// </summary>
        RopSynchronizationOpenCollector = 0x7E,

        /// <summary>
        /// RopGetLocalReplicaIds ROP
        /// </summary>
        RopGetLocalReplicaIds = 0x7F,

        /// <summary>
        /// RopSynchronizationImportReadStateChanges ROP
        /// </summary>
        RopSynchronizationImportReadStateChanges = 0x80,

        /// <summary>
        /// RopResetTable ROP
        /// </summary>
        RopResetTable = 0x81,

        /// <summary>
        /// RopSynchronizationGetTransferState ROP
        /// </summary>
        RopSynchronizationGetTransferState = 0x82,

        /// <summary>
        /// RopTellVersion ROP
        /// </summary>
        RopTellVersion = 0x86,

        /// <summary>
        /// RopFreeBookmark ROP
        /// </summary>
        RopFreeBookmark = 0x89,

        /// <summary>
        /// RopWriteAndCommitStream ROP
        /// </summary>
        RopWriteAndCommitStream = 0x90,

        /// <summary>
        /// RopHardDeleteMessages ROP
        /// </summary>
        RopHardDeleteMessages = 0x91,

        /// <summary>
        /// RopHardDeleteMessagesAndSubfolders ROP
        /// </summary>
        RopHardDeleteMessagesAndSubfolders = 0x92,

        /// <summary>
        /// RopSetLocalReplicaMidsetDeleted ROP
        /// </summary>
        RopSetLocalReplicaMidsetDeleted = 0x93,

        /// <summary>
        /// RopFastTransferDestinationPutBufferExtended ROP
        /// </summary>
        RopFastTransferDestinationPutBufferExtended = 0x9D,

        /// <summary>
        /// RopWriteStreamExtended ROP
        /// </summary>
        RopWriteStreamExtended = 0xA3,

        /// <summary>
        /// RopBackoff ROP
        /// </summary>
        RopBackoff = 0xF9,

        /// <summary>
        /// RopLogon ROP
        /// </summary>
        RopLogon = 0xFE,

        /// <summary>
        /// RopBufferTooSmall ROP
        /// </summary>
        RopBufferTooSmall = 0xFF
    }

    /// <summary>
    /// The enum value of ROP response status.
    /// </summary>
    public enum RopResponseStatus : uint
    {
        /// <summary>
        /// Success response
        /// </summary>
        Success = 0x00000000,

        /// <summary>
        /// Log on redirect response
        /// </summary>
        LogonRedirect = 0x00000478,

        /// <summary>
        /// Null destination object
        /// </summary>
        NullDestinationObject = 0x00000503
    }
    #endregion

    #region ROP Input Buffer
    /// <summary>
    ///  A class indicates the ROP input buffer, which is sent by the client, includes an array of ROP request buffers to be processed by the server.
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
            this.RopSize = this.ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            List<uint> ropRemainSize = new List<uint>();
            List<uint> tempServerObjectHandleTable = new List<uint>();
            int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
            }
            long currentPosition = s.Position;
            s.Position += this.RopSize - 2;

            while (s.Position < s.Length)
            {
                uint serverObjectTable = this.ReadUint();

                if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                {
                    MapiInspector.MAPIParser.IsLooperCall = true;
                    Dictionary<ushort, Dictionary<int, uint>> item = new Dictionary<ushort, Dictionary<int, uint>>();
                    item = MapiInspector.MAPIParser.TargetHandle.Peek();

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

                if (this.RopSize > 2)
                {
                    ropRemainSize.Add(this.RopSize - (uint)2);

                    do
                    {
                        int currentByte = s.ReadByte();
                        s.Position -= 1;

                        switch ((RopIdType)currentByte)
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
                                byte ropId = this.ReadByte();
                                byte logonId = this.ReadByte();
                                s.Position -= 2;

                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID)
                                      && DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logonId)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopWritePerUserInformation", (ushort)currentByte, new uint[] { logonId });
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
                                uint handle_Release = tempServerObjectHandleTable[ropReleaseRequest.InputHandleIndex.Data];
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
                                uint handle_SetColumns = tempServerObjectHandleTable[ropSetColumnsRequest.InputHandleIndex.Data];
                                string serverUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;

                                if (handle_SetColumns != 0xFFFFFFFF)
                                {
                                    if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                                    {
                                        Dictionary<ushort, Dictionary<int, uint>> target = MapiInspector.MAPIParser.TargetHandle.Peek();

                                        if ((RopIdType)target.First().Key == RopIdType.RopQueryRows || (RopIdType)target.First().Key == RopIdType.RopFindRow || (RopIdType)target.First().Key == RopIdType.RopExpandRow)
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

                                        if ((RopIdType)target.First().Key == RopIdType.RopNotify)
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
                                            outputHandle = MapiInspector.MAPIParser.ParseResponseMessageSimplely(MapiInspector.MAPIParser.ParsingSession, ropSetColumnsRequest.InputHandleIndex.Data);
                                        }
                                        finally
                                        {
                                            MapiInspector.MAPIParser.IsOnlyGetServerHandle = false;
                                        }

                                        if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                                        {
                                            Dictionary<ushort, Dictionary<int, uint>> target = MapiInspector.MAPIParser.TargetHandle.Peek();

                                            if ((RopIdType)target.First().Key == RopIdType.RopQueryRows || (RopIdType)target.First().Key == RopIdType.RopFindRow || (RopIdType)target.First().Key == RopIdType.RopExpandRow)
                                            {
                                                // This is for Row related rops 
                                                Dictionary<int, Tuple<string, string, string, PropertyTag[]>> sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[]>>();
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
                                        outputHandle = MapiInspector.MAPIParser.ParseResponseMessageSimplely(MapiInspector.MAPIParser.ParsingSession, ropSetColumnsRequest.InputHandleIndex.Data);
                                    }
                                    finally
                                    {
                                        MapiInspector.MAPIParser.IsOnlyGetServerHandle = false;
                                    }

                                    if (MapiInspector.MAPIParser.TargetHandle.Count > 0)
                                    {
                                        Dictionary<ushort, Dictionary<int, uint>> target = MapiInspector.MAPIParser.TargetHandle.Peek();

                                        if ((RopIdType)target.First().Key == RopIdType.RopQueryRows || (RopIdType)target.First().Key == RopIdType.RopFindRow || (RopIdType)target.First().Key == RopIdType.RopExpandRow)
                                        {
                                            // This is for Row related rops 
                                            Dictionary<int, Tuple<string, string, string, PropertyTag[]>> sessionTuples = new Dictionary<int, Tuple<string, string, string, PropertyTag[]>>();
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
                                    if (!DecodingContext.PartialInformationReady.ContainsKey((int)destinationParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException((RopIdType)currentByte, ropPutbufferHandle);
                                    }
                                }
                                else
                                {
                                    MapiInspector.MAPIParser.PartialPutType = 0;
                                    MapiInspector.MAPIParser.PartialPutRemainSize = -1;
                                    MapiInspector.MAPIParser.PartialPutSubRemainSize = -1;
                                }

                                RopFastTransferDestinationPutBufferRequest ropFastTransferDestinationPutBufferRequest = new RopFastTransferDestinationPutBufferRequest();
                                MapiInspector.MAPIParser.IsPut = true;
                                ropFastTransferDestinationPutBufferRequest.Parse(s);
                                ropsList.Add(ropFastTransferDestinationPutBufferRequest);

                                PartialContextInformation putBufferPartialInformaiton = new PartialContextInformation(MapiInspector.MAPIParser.PartialPutType, MapiInspector.MAPIParser.PartialPutId, MapiInspector.MAPIParser.PartialPutRemainSize, MapiInspector.MAPIParser.PartialPutSubRemainSize, false, destinationParsingSession, MapiInspector.MAPIParser.InputPayLoadCompressedXOR);
                                SortedDictionary<int, PartialContextInformation> sessionputContextInfor = new SortedDictionary<int, PartialContextInformation>();

                                if (MapiInspector.MAPIParser.HandleWithSessionPutContextInformation.ContainsKey(ropPutbufferHandle))
                                {
                                    sessionputContextInfor = MapiInspector.MAPIParser.HandleWithSessionPutContextInformation[ropPutbufferHandle];
                                    MapiInspector.MAPIParser.HandleWithSessionPutContextInformation.Remove(ropPutbufferHandle);
                                }

                                if (sessionputContextInfor.ContainsKey(destinationParsingSessionID))
                                {
                                    sessionputContextInfor[destinationParsingSessionID] = putBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionputContextInfor.Add(destinationParsingSessionID, putBufferPartialInformaiton);
                                }

                                MapiInspector.MAPIParser.HandleWithSessionPutContextInformation.Add(ropPutbufferHandle, sessionputContextInfor);
                                MapiInspector.MAPIParser.IsPut = false;
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
                                    if (!DecodingContext.PartialInformationReady.ContainsKey((int)aimsParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException((RopIdType)currentByte, ropPutExtendbufferHandle);
                                    }
                                }
                                else
                                {
                                    MapiInspector.MAPIParser.PartialPutExtendType = 0;
                                    MapiInspector.MAPIParser.PartialPutExtendRemainSize = -1;
                                    MapiInspector.MAPIParser.PartialPutExtendSubRemainSize = -1;
                                }

                                RopFastTransferDestinationPutBufferExtendedRequest ropFastTransferDestinationPutBufferExtendedRequest = new RopFastTransferDestinationPutBufferExtendedRequest();
                                MapiInspector.MAPIParser.IsPutExtend = true;
                                ropFastTransferDestinationPutBufferExtendedRequest.Parse(s);
                                ropsList.Add(ropFastTransferDestinationPutBufferExtendedRequest);

                                PartialContextInformation putExtendBufferPartialInformaiton = new PartialContextInformation(MapiInspector.MAPIParser.PartialPutType, MapiInspector.MAPIParser.PartialPutId, MapiInspector.MAPIParser.PartialPutRemainSize, MapiInspector.MAPIParser.PartialPutSubRemainSize, false, aimsParsingSession, MapiInspector.MAPIParser.InputPayLoadCompressedXOR);
                                SortedDictionary<int, PartialContextInformation> sessionputExtendContextInfor = new SortedDictionary<int, PartialContextInformation>();

                                if (MapiInspector.MAPIParser.HandleWithSessionPutExtendContextInformation.ContainsKey(ropPutExtendbufferHandle))
                                {
                                    sessionputExtendContextInfor = MapiInspector.MAPIParser.HandleWithSessionPutExtendContextInformation[ropPutExtendbufferHandle];
                                    MapiInspector.MAPIParser.HandleWithSessionPutExtendContextInformation.Remove(ropPutExtendbufferHandle);
                                }

                                if (sessionputExtendContextInfor.ContainsKey(aimsParsingSessionID))
                                {
                                    sessionputExtendContextInfor[aimsParsingSessionID] = putExtendBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionputExtendContextInfor.Add(aimsParsingSessionID, putExtendBufferPartialInformaiton);
                                }

                                MapiInspector.MAPIParser.HandleWithSessionPutExtendContextInformation.Add(ropPutExtendbufferHandle, sessionputExtendContextInfor);
                                MapiInspector.MAPIParser.IsPutExtend = false;
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
                                byte ropId_setReadFlag = this.ReadByte();
                                byte logId = this.ReadByte();
                                s.Position -= 2;
                                if (!(DecodingContext.SessionLogonFlagMapLogId.Count > 0 && DecodingContext.SessionLogonFlagMapLogId.ContainsKey(parsingSessionID)
                                    && DecodingContext.SessionLogonFlagMapLogId[parsingSessionID].ContainsKey(logId)))
                                {
                                    throw new MissingInformationException("Missing LogonFlags information for RopSetMessageReadFlag", (ushort)currentByte, new uint[] { logId });
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
                                object ropsBytes = this.ReadBytes(this.RopSize - (ushort)s.Position);
                                ropsList.Add(ropsBytes);
                                break;
                        }

                        if ((RopIdType)currentByte != RopIdType.RopRelease)
                        {
                            ropRemainSize.Add(this.RopSize - (uint)s.Position);
                        }
                        else
                        {
                            ropRemainSize.RemoveAt(ropRemainSize.Count - 1);
                            ropRemainSize.Add(this.RopSize - (uint)s.Position);
                        }
                    }
                    while (s.Position < this.RopSize);
                }
                else
                {
                    this.RopsList = null;
                }

                if (DecodingContext.SessionRequestRemainSize.ContainsKey(parsingSessionID))
                {
                    DecodingContext.SessionRequestRemainSize.Remove(parsingSessionID);
                }

                DecodingContext.SessionRequestRemainSize.Add(parsingSessionID, ropRemainSize);
                this.RopsList = ropsList.ToArray();
            }
            else
            {
                byte[] ropListBytes = this.ReadBytes(this.RopSize - 2);
                ropsList.AddRange(ropListBytes.Cast<object>().ToArray());
            }

            this.RopsList = ropsList.ToArray();

            if (this.RopsList.Length != 0)
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
                uint serverObjectHandle = this.ReadUint();
                serverObjectHandleTable.Add(serverObjectHandle);
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
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public ushort RopSize;

        /// <summary>
        /// An array of ROP request buffers.
        /// </summary>
        public byte[] RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPInputBuffer_WithoutCROPS structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPInputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopSize = this.ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            byte[] ropListBytes = this.ReadBytes(this.RopSize - 2);
            this.RopsList = ropListBytes;

            while (s.Position < s.Length)
            {
                uint serverObjectHandle = this.ReadUint();
                serverObjectHandleTable.Add(serverObjectHandle);
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
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public ushort RopSize;

        /// <summary>
        /// An array of ROP request buffers.
        /// </summary>
        public byte[] RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPOutputBuffer_WithoutCROPS structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPOutputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopSize = this.ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            byte[] ropListBytes = this.ReadBytes(this.RopSize - 2);
            this.RopsList = ropListBytes;

            while (s.Position < s.Length)
            {
                uint serverObjectHandle = this.ReadUint();
                serverObjectHandleTable.Add(serverObjectHandle);
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
            this.RopSize = this.ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            List<uint> tempServerObjectHandleTable = new List<uint>();
            long currentPosition = s.Position;
            s.Position += this.RopSize - 2;
            int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
            if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
            }
            while (s.Position < s.Length)
            {
                uint serverObjectTable = this.ReadUint();

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

                if (this.RopSize > 2)
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

                                if (!(DecodingContext.SetColumn_InputHandles_InResponse.Count > 0 && DecodingContext.SetColumn_InputHandles_InResponse.Contains(tempServerObjectHandleTable[ropSetColumnsResponse.InputHandleIndex.Data])))
                                {
                                    DecodingContext.SetColumn_InputHandles_InResponse.Add(tempServerObjectHandleTable[ropSetColumnsResponse.InputHandleIndex.Data]);
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
                                uint returnValue_queryRow = this.ReadUint();
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
                                uint returnValue_findRow = this.ReadUint();
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
                                uint returnValue_expandRow = this.ReadUint();
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
                                uint returnValue = this.ReadUint();
                                ushort status = this.ReadUshort();
                                s.Position = currentPos_getBuffer;
                                int getParsingSessionID = parsingSessionID;
                                Session getParsingSession = MapiInspector.MAPIParser.ParsingSession;
                                uint ropGetbufferHandle = tempServerObjectHandleTable[tempInputHandleIndex_getBuffer];
                                PartialContextInformation[] partialBeforeAndAfterInformation = new PartialContextInformation[2];
                                if (returnValue == 0)
                                {
                                    if (!DecodingContext.PartialInformationReady.ContainsKey((int)getParsingSessionID))
                                    {
                                        throw new MissingPartialInformationException((RopIdType)currentByte, ropGetbufferHandle);
                                    }
                                }

                                RopFastTransferSourceGetBufferResponse ropFastTransferSourceGetBufferResponse = new RopFastTransferSourceGetBufferResponse();
                                MapiInspector.MAPIParser.IsGet = true;
                                ropFastTransferSourceGetBufferResponse.Parse(s);
                                ropsList.Add(ropFastTransferSourceGetBufferResponse);
                                PartialContextInformation getBufferPartialInformaiton = new PartialContextInformation(MapiInspector.MAPIParser.PartialGetType, MapiInspector.MAPIParser.PartialGetId, MapiInspector.MAPIParser.PartialGetRemainSize, MapiInspector.MAPIParser.PartialGetSubRemainSize, true, getParsingSession, MapiInspector.MAPIParser.OutputPayLoadCompressedXOR);
                                SortedDictionary<int, PartialContextInformation> sessionGetContextInfor = new SortedDictionary<int, PartialContextInformation>();

                                if (MapiInspector.MAPIParser.HandleWithSessionGetContextInformation.ContainsKey(ropGetbufferHandle))
                                {
                                    sessionGetContextInfor = MapiInspector.MAPIParser.HandleWithSessionGetContextInformation[ropGetbufferHandle];
                                    MapiInspector.MAPIParser.HandleWithSessionGetContextInformation.Remove(ropGetbufferHandle);
                                }

                                if (sessionGetContextInfor.ContainsKey(getParsingSessionID))
                                {
                                    sessionGetContextInfor[getParsingSessionID] = getBufferPartialInformaiton;
                                }
                                else
                                {
                                    sessionGetContextInfor.Add(getParsingSessionID, getBufferPartialInformaiton);
                                }

                                MapiInspector.MAPIParser.HandleWithSessionGetContextInformation.Add(ropGetbufferHandle, sessionGetContextInfor);
                                MapiInspector.MAPIParser.IsGet = false;
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
                                object ropsBytes = this.ReadBytes(this.RopSize - (int)s.Position);
                                ropsList.Add(ropsBytes);
                                break;
                        }
                    }
                    while (s.Position < this.RopSize);
                }
                else
                {
                    this.RopsList = null;
                }
            }
            else
            {
                byte[] ropListBytes = this.ReadBytes(this.RopSize - 2);
            }

            this.RopsList = ropsList.ToArray();

            if (this.RopsList.Length != 0)
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
                uint serverObjectHandle = this.ReadUint();
                serverObjectHandleTable.Add(serverObjectHandle);
            }

            this.ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
    #endregion

    #region 2.2.7.1 RopSubmitMessage
    /// <summary>
    ///  A class indicates the RopSubmitMessage ROP Request Buffer.
    /// </summary>
    public class RopSubmitMessageRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that specify special behavior for submitting the message.
        /// </summary>
        public SubmitFlags SubmitFlags;

        /// <summary>
        /// Parse the RopSubmitMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSubmitMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.SubmitFlags = (SubmitFlags)this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopSubmitMessage ROP Response Buffer.
    /// </summary>
    public class RopSubmitMessageResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public uint ReturnValue;

        /// <summary>
        /// Parse the RopSubmitMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSubmitMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = this.ReadUint();
        }
    }

    #endregion

    #region 2.2.7.2 RopAbortSubmit
    /// <summary>
    ///  A class indicates the RopAbortSubmit ROP Request Buffer.
    /// </summary>
    public class RopAbortSubmitRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that identifies the folder in which the submitted message is located.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// An identifier that specifies the submitted message.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopAbortSubmitRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortSubmitRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopAbortSubmitResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortSubmitResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }

    #endregion

    #region 2.2.7.3 RopGetAddressTypes
    /// <summary>
    ///  A class indicates the RopGetAddressTypes ROP Request Buffer.
    /// </summary>
    public class RopGetAddressTypesRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetAddressTypesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAddressTypesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetAddressTypes ROP Response Buffer.
    /// </summary>
    public class RopGetAddressTypesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the AddressTypes field.
        /// </summary>
        public ushort? AddressTypeCount;

        /// <summary>
        /// An unsigned integer that specifies the length of the AddressTypes field.
        /// </summary>
        public ushort? AddressTypeSize;

        /// <summary>
        /// A list of null-terminated ASCII strings.
        /// </summary>
        public MAPIString[] AddressTypes;

        /// <summary>
        /// Parse the RopGetAddressTypesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAddressTypesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.AddressTypeCount = this.ReadUshort();
                this.AddressTypeSize = this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopSetSpoolerRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetSpoolerRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopSetSpooler ROP Response Buffer.
    /// </summary>
    public class RopSetSpoolerResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetSpoolerResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetSpoolerResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }

    #endregion

    #region 2.2.7.5 RopSpoolerLockMessage
    /// <summary>
    ///  A class indicates the RopSpoolerLockMessage ROP Request Buffer.
    /// </summary>
    public class RopSpoolerLockMessageRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the message for which the status will be changed.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// An integer flag specifies a status to set on the message.
        /// </summary>
        public LockState LockState;

        /// <summary>
        /// Parse the RopSpoolerLockMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSpoolerLockMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
            this.LockState = (LockState)this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopSpoolerLockMessage ROP Response Buffer.
    /// </summary>
    public class RopSpoolerLockMessageResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSpoolerLockMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSpoolerLockMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }

    #endregion

    #region 2.2.7.6 RopTransportSend
    /// <summary>
    ///  A class indicates the RopTransportSend ROP Request Buffer.
    /// </summary>
    public class RopTransportSendRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopTransportSendRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportSendRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopTransportSend ROP Response Buffer.
    /// </summary>
    public class RopTransportSendResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A boolean that specifies whether property values are returned.
        /// </summary>
        public byte? NoPropertiesReturned;

        /// <summary>
        /// An unsigned integer that specifies the number of structures returned in the PropertyValues field.
        /// </summary>
        public ushort? PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specifies the properties to copy.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopTransportSendResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportSendResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.NoPropertiesReturned = this.ReadByte();
                this.PropertyValueCount = this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the new message object.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// An identifier that identifies the folder of the new message object.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies the message class of the new message object;
        /// </summary>
        public MAPIString MessageClass;

        /// <summary>
        /// A flags structure that contains the message flags of the new message object.
        /// </summary>
        public MessageFlags MessageFlags;

        /// <summary>
        /// Parse the RopTransportNewMailRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportNewMailRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.MessageClass = new MAPIString(Encoding.ASCII);
            this.MessageClass.Parse(s);
            this.MessageFlags = (MessageFlags)this.ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopTransportNewMail ROP Response Buffer.
    /// </summary>
    public class RopTransportNewMailResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopTransportNewMailResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportNewMailResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }

    #endregion

    #region 2.2.7.8 RopGetTransportFolder
    /// <summary>
    ///  A class indicates the RopGetTransportFolder ROP Request Buffer.
    /// </summary>
    public class RopGetTransportFolderRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetTransportFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetTransportFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetTransportFolder ROP Response Buffer.
    /// </summary>
    public class RopGetTransportFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An identifier that specifies the transport folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopGetTransportFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetTransportFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A null-terminated ASCII string that specifies the address type that options are to be returned for.
        /// </summary>
        public MAPIString AddressType;

        /// <summary>
        /// A boolean that specifies whether the help file data is to be returned in a format that is suited for 32-bit machines.
        /// </summary>
        public byte WantWin32;

        /// <summary>
        /// Parse the RopOptionsDataRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOptionsDataRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.AddressType = new MAPIString(Encoding.ASCII);
            this.AddressType.Parse(s);
            this.WantWin32 = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopOptionsData ROP Response Buffer.
    /// </summary>
    public class RopOptionsDataResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Reserved byte
        /// </summary>
        public byte? Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the OptionsInfo field.
        /// </summary>
        public ushort? OptionalInfoSize;

        /// <summary>
        /// An array of bytes that contains opaque data from the server.
        /// </summary>
        public byte?[] OptionalInfo;

        /// <summary>
        /// An unsigned integer that specifies the size of the HelpFile field.
        /// </summary>
        public ushort? HelpFileSize;

        /// <summary>
        /// An array of bytes that contains the help file associated with the specified address type.
        /// </summary>
        public byte?[] HelpFile;

        /// <summary>
        /// A null-terminated multibyte string that specifies the name of the help file that is associated with the specified address type.
        /// </summary>
        public MAPIString HelpFileName;

        /// <summary>
        /// Parse the RopOptionsDataResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOptionsDataResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.Reserved = this.ReadByte();
                this.OptionalInfoSize = this.ReadUshort();
                this.OptionalInfo = this.ConvertArray(this.ReadBytes((int)this.OptionalInfoSize));
                this.HelpFileSize = this.ReadUshort();

                if (this.HelpFileSize != 0)
                {
                    this.HelpFile = this.ConvertArray(this.ReadBytes((int)this.HelpFileSize));
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the size required for the ROP output buffer.
        /// </summary>
        public ushort SizeNeeded;

        /// <summary>
        /// An array of bytes that contains the section of the ROP input buffer that was not executed because of the insufficient size of the ROP output buffer.
        /// </summary>
        public byte[] RequestBuffers;

        /// <summary>
        /// An unsigned integer that specifies the size of RequestBuffers.
        /// </summary>
        private uint requestBuffersSize;

        /// <summary>
        /// Initializes a new instance of the RopBufferTooSmallResponse class.
        /// </summary>
        /// <param name="requestBuffersSize"> The size of RequestBuffers.</param>
        public RopBufferTooSmallResponse(uint requestBuffersSize)
        {
            this.requestBuffersSize = requestBuffersSize;
        }

        /// <summary>
        /// Parse the RopBufferTooSmallResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopBufferTooSmallResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.SizeNeeded = this.ReadUshort();
            this.RequestBuffers = this.ReadBytes((int)this.requestBuffersSize);
        }
    }

    #endregion

    #region 2.2.15.2 RopBackoff

    /// <summary>
    /// A class indicates the RopBackoff ROP Response Buffer.
    /// </summary>
    public class RopBackoffResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x01.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds to apply a ROP BackOff.
        /// </summary>
        public uint Duration;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the BackoffRopData field.
        /// </summary>
        public byte BackoffRopCount;

        /// <summary>
        /// An array of BackoffRop structures. 
        /// </summary>
        public BackoffRop[] BackoffRopData;

        /// <summary>
        /// An unsigned integer that specifies the size of the AdditionalData field.
        /// </summary>
        public ushort AdditionalDataSize;

        /// <summary>
        /// An array of bytes that specifies additional information about the ROP BackOff response. 
        /// </summary>
        public byte[] AdditionalData;

        /// <summary>
        /// Parse the RopBackoffResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopBackoffResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.Duration = this.ReadUint();
            this.BackoffRopCount = this.ReadByte();
            List<BackoffRop> backoffRopDataList = new List<BackoffRop>();

            for (int i = 0; i < this.BackoffRopCount; i++)
            {
                BackoffRop subBackoffRop = new BackoffRop();
                subBackoffRop.Parse(s);
                backoffRopDataList.Add(subBackoffRop);
            }

            this.BackoffRopData = backoffRopDataList.ToArray();
            this.AdditionalDataSize = this.ReadUshort();
            this.AdditionalData = this.ReadBytes(this.AdditionalDataSize);
        }
    }

    /// <summary>
    ///  A class indicates the BackoffRop structure which is defined in section 2.2.15.2.1.1.
    /// </summary>
    public class BackoffRop : BaseStructure
    {
        /// <summary>
        /// An unsigned integer index that identifies the ROP to apply the ROP BackOff to
        /// </summary>
        public byte RopIdBackoff;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds to apply a ROP BackOff.
        /// </summary>
        public uint Duration;

        /// <summary>
        /// Parse the BackoffRop structure.
        /// </summary>
        /// <param name="s">A stream containing BackoffRop structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopIdBackoff = this.ReadByte();
            this.Duration = this.ReadUint();
        }
    }
    #endregion

    #region 2.2.15.3 RopRelease

    /// <summary>
    ///  A class indicates the RopRelease ROP Request Buffer.
    /// </summary>
    public class RopReleaseRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x01.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// Parse the RopReleaseRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopReleaseRequest");
            AddChildBlockT(RopId, "RopId");
            if (LogonId != null) AddChild(LogonId, "LogonId:0x{0:X2}", LogonId.Data);
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
        }
    }

    #endregion

    #region Helper method for Decoding
    /// <summary>
    ///  The DecodingContext is shared between some ROP request and response.
    /// </summary>
    public class DecodingContext
    {
        /// <summary>
        /// Record the LogonId and RopLogon flags.
        /// </summary>
        private static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>> logonFlagMapLogId;

        /// <summary>
        /// Record the map in session information,LogonId and RopLogon flags.
        /// </summary>
        private static Dictionary<int, Dictionary<byte, LogonFlags>> sessionLogonFlagMapLogId;

        /// <summary>
        /// Record the map in session information, handle index and logonFlags in RopLogon ROP.
        /// </summary>
        private static Dictionary<int, Dictionary<uint, LogonFlags>> sessionLogonFlagsInLogonRop;

        /// <summary>
        /// Record the map in session information, handle index, and PropertyTags for getPropertiesSpecific ROP.
        /// </summary>
        private static Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>> getPropertiesSpecPropertyTags;

        /// <summary>
        /// Record the map in session id and the remain seize in ROP list parsing.
        /// </summary>
        private static Dictionary<int, List<uint>> sessionRequestRemainSize;

        /// <summary>
        /// Record RopSetColumn InputObjectHandle in setColumn Response.
        /// </summary>
        private static List<uint> setColumnInputHandlesInResponse;

        /// <summary>
        /// Record the map of SetColumns's output handle, session id and tuple for row ROPs.
        /// </summary>
        private static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>> rowRopsHandlePropertyTags;

        /// <summary>
        /// Record the map in session id, handle index and PropertyTags for row ROPs.
        /// </summary>
        private static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> rowRopsSessionPropertyTags;

        /// <summary>
        /// Record the map of SetColumns's output handle, session id and tuple for RopNotify.
        /// </summary>
        private static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>> notifyHandlePropertyTags;

        /// <summary>
        /// Record the map of serverUrl, session id, object handle and PropertyTags for RopNotify.
        /// </summary>
        private static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> notifySessionPropertyTags;

        /// <summary>
        /// Record the map in session id and partial information is ready.
        /// </summary>
        private static Dictionary<int, bool> partialInformationReady;

        /// <summary>
        /// Initializes a new instance of the DecodingContext class
        /// </summary>
        public DecodingContext()
        {
            logonFlagMapLogId = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>>();
            sessionLogonFlagMapLogId = new Dictionary<int, Dictionary<byte, LogonFlags>>();
            sessionLogonFlagsInLogonRop = new Dictionary<int, Dictionary<uint, LogonFlags>>();
            getPropertiesSpecPropertyTags = new Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>>();
            sessionRequestRemainSize = new Dictionary<int, List<uint>>();
            rowRopsHandlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>>();
            notifyHandlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>>();
            notifySessionPropertyTags = new Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>>();
            rowRopsSessionPropertyTags = new Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>>();
            setColumnInputHandlesInResponse = new List<uint>();
            partialInformationReady = new Dictionary<int, bool>();
        }

        /// <summary>
        /// Gets or sets the LogonId and RopLogon flags
        /// </summary>
        public static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>> LogonFlagMapLogId
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

        /// <summary>
        /// Gets or sets the sessionLogonFlagMapLogId
        /// </summary>
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

        /// <summary>
        /// Gets or sets the sessionLogonFlagsInLogonRop
        /// </summary>
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

        /// <summary>
        /// Gets or sets the getPropertiesSpec_propertyTags
        /// </summary>
        public static Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>> GetPropertiesSpec_propertyTags
        {
            get
            {
                return getPropertiesSpecPropertyTags;
            }

            set
            {
                getPropertiesSpecPropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the sessionRequestRemainSize
        /// </summary>
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

        /// <summary>
        /// Gets or sets the rowRops_handlePropertyTags
        /// </summary>
        public static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>> RowRops_handlePropertyTags
        {
            get
            {
                return rowRopsHandlePropertyTags;
            }

            set
            {
                rowRopsHandlePropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the rowRops_sessionpropertyTags
        /// </summary>
        public static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> RowRops_sessionPropertyTags
        {
            get
            {
                return rowRopsSessionPropertyTags;
            }

            set
            {
                rowRopsSessionPropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the notify_handlePropertyTags
        /// </summary>
        public static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>> Notify_handlePropertyTags
        {
            get
            {
                return notifyHandlePropertyTags;
            }

            set
            {
                notifyHandlePropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the notify_sessionPropertyTags
        /// </summary>
        public static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> Notify_sessionPropertyTags
        {
            get
            {
                return notifySessionPropertyTags;
            }

            set
            {
                notifySessionPropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the setColumn_InputHandles
        /// </summary>
        public static List<uint> SetColumn_InputHandles_InResponse
        {
            get
            {
                return setColumnInputHandlesInResponse;
            }

            set
            {
                setColumnInputHandlesInResponse = value;
            }
        }

        /// <summary>
        /// Gets or sets the partialInformationReady
        /// </summary>
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

    /// <summary>
    /// The MissingInformationException is used to define the exception, which are caused by missing context information.
    /// </summary>
    public class MissingInformationException : Exception
    {
        /// <summary>
        /// The exception message thrown
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// The ROP ID needs context information
        /// </summary>
        public ushort RopID;

        /// <summary>
        /// The source ROP parameters to pass
        /// </summary>
        public uint[] Parameters;

        /// <summary>
        /// Initializes a new instance of the MissingInformationException class
        /// </summary>
        /// <param name="message">Exception error messge</param>
        /// <param name="ropID">ROP id</param>
        /// <param name="parameter">parameters for this missing information exception</param>
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
        /// <summary>
        /// Gets or sets RopId indicates the target ROP searched
        /// </summary>
        public RopIdType RopID { get; set; }

        /// <summary>
        /// Gets or sets handle indicates the target handle searched
        /// </summary>
        public uint Handle { get; set; }

        /// <summary>
        /// Gets or sets result searched for the target context information
        /// </summary>
        public object RelatedInformation { get; set; }
    }

    /// <summary>
    /// The MissingPartialInformationException is used to define the exception, which are caused by missing context information for partial.
    /// </summary>
    public class MissingPartialInformationException : Exception
    {
        /// <summary>
        /// The ROP ID needs context information
        /// </summary>
        public RopIdType RopID;

        /// <summary>
        /// The source ROP parameters to pass
        /// </summary>
        public uint Parameter;

        /// <summary>
        /// Initializes a new instance of the MissingPartialInformationException class
        /// </summary>
        /// <param name="ropID">ROP id</param>
        /// <param name="parameter">parameters for this missing partial information exception</param>
        public MissingPartialInformationException(RopIdType ropID, uint parameter)
        {
            this.RopID = ropID;
            this.Parameter = parameter;
        }
    }

    /// <summary>
    /// Information for FastertransferStream Partial
    /// </summary>
    public class PartialContextInformation
    {
        /// <summary>
        /// Initializes a new instance of the PartialContextInformation class
        /// </summary>
        /// <param name="type">The property type</param>
        /// <param name="id">The property id</param>
        /// <param name="remainSize">The property value remain size</param>
        /// <param name="subRemainSize">The property value sub remain size for multiple type data</param>
        /// <param name="isGet">Boolean value indicates if this is about RopGetBuffer ROP</param>
        /// <param name="session">The session that contains this</param>
        /// <param name="payLoadCompresssedXOR">The payload value about this</param>
        public PartialContextInformation(PropertyDataType type = 0, PidTagPropertyEnum id = 0, int remainSize = -1, int subRemainSize = -1, bool isGet = true, Session session = null, List<byte[]> payLoadCompresssedXOR = null)
        {
            this.Type = type;
            this.ID = id;
            this.RemainSize = remainSize;
            this.SubRemainSize = subRemainSize;
            this.IsGet = isGet;
            this.PayLoadCompresssedXOR = payLoadCompresssedXOR;
            this.Session = session;
        }

        /// <summary>
        /// Gets or sets the property type
        /// </summary>
        public PropertyDataType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the property ID
        /// </summary>
        public PidTagPropertyEnum ID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the property value remain size
        /// </summary>
        public int RemainSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the property value sub remain size for multiple type data
        /// </summary>
        public int SubRemainSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is about RopGetBuffer ROP
        /// </summary>
        public bool IsGet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the payload value about this
        /// </summary>
        public List<byte[]> PayLoadCompresssedXOR
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the session that contains this
        /// </summary>
        public Session Session
        {
            get;
            set;
        }
    }
    #endregion
}
