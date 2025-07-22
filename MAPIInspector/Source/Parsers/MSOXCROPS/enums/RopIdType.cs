namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2 RopIds
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
}
