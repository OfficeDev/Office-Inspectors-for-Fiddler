namespace MAPIInspector.Parsers
{
    using MapiInspector;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The enum of StoreObject type.
    /// </summary>
    public enum StoreObjectType : ushort
    {
        /// <summary>
        /// Private Folder (eitLTPrivateFolder) type
        /// </summary>
        PrivateFolder = 0x0001,

        /// <summary>
        /// PublicFolder (eitLTPublicFolder) type
        /// </summary>
        PublicFolder = 0x0003,

        /// <summary>
        /// MappedPublicFolder (eitLTWackyFolder) type
        /// </summary>
        MappedPublicFolder = 0x0005,

        /// <summary>
        /// PrivateMessage (eitLTPrivateMessage) type
        /// </summary>
        PrivateMessage = 0x0007,

        /// <summary>
        /// PublicMessage (eitLTPublicMessage) type
        /// </summary>
        PublicMessage = 0x0009,

        /// <summary>
        /// MappedPublicMessage (eitLTWackyMessage) type
        /// </summary>
        MappedPublicMessage = 0x000B,

        /// <summary>
        /// PublicNewsgroupFolder (eitLTPublicFolderByName) type
        /// </summary>
        PublicNewsgroupFolder = 0x000C
    }

    /// <summary>
    /// The enum of AddressbookEntryID type.
    /// </summary>
    public enum AddressbookEntryIDtype : uint
    {
        /// <summary>
        /// Local mail user
        /// </summary>
        Localmailuser = 0x00000000,

        /// <summary>
        /// Distribution list
        /// </summary>
        Distributionlist = 0x00000001,

        /// <summary>
        /// Bulletin board or public folder
        /// </summary>
        Bulletinboardorpublicfolder = 0x00000002,

        /// <summary>
        /// Automated mailbox
        /// </summary>
        Automatedmailbox = 0x00000003,

        /// <summary>
        /// Organizational mailbox
        /// </summary>
        Organizationalmailbox = 0x00000004,

        /// <summary>
        /// Private distribution list
        /// </summary>
        Privatedistributionlist = 0x00000005,

        /// <summary>
        /// Remote mail user 
        /// </summary>
        Remotemailuser = 0x00000006,

        /// <summary>
        /// A Container
        /// </summary>
        Container = 0x00000100,

        /// <summary>
        /// A Template 
        /// </summary>
        Template = 0x00000101,

        /// <summary>
        /// One-off user
        /// </summary>
        Oneoffuser = 0x00000102,

        /// <summary>
        /// A Search
        /// </summary>
        Search = 0x00000200
    }

    #region 2.4 Error Codes

    /// <summary>
    /// 2.4 Error Codes
    /// </summary>
    public enum ErrorCodes : uint
    {
        /// <summary>
        /// The operation succeeded.
        /// </summary>
        Success = 0x00000000,

        /// <summary>
        /// The operation failed for an unspecified reason.
        /// </summary>
        GeneralFailure = 0x80004005,

        /// <summary>
        /// Not enough memory was available to complete the operation.
        /// </summary>
        OutOfMemory = 0x8007000E,

        /// <summary>
        /// An invalid parameter was passed to a remote procedure call (RPC).
        /// </summary>
        InvalidParameter = 0x80070057,

        /// <summary>
        /// The requested interface is not supported.
        /// </summary>
        NoInterface = 0x80004002,

        /// <summary>
        /// The caller does not have sufficient access rights to perform the operation.
        /// </summary>
        AccessDenied = 0x80070005,

        /// <summary>
        /// The server was unable to perform the requested operation.
        /// </summary>
        StorageInvalidFunction = 0x80030001,

        /// <summary>
        /// The caller does not have sufficient access rights to perform the operation.
        /// </summary>
        StorageAccessDenied = 0x80030005,

        /// <summary>
        /// There is insufficient memory available to complete the operation.
        /// </summary>
        StorageInsufficientMemory = 0x80030008,

        /// <summary>
        /// An invalid pointer was passed to the remote procedure call.
        /// </summary>
        StorageInvalidPointer = 0x80030009,

        /// <summary>
        /// A disk error occurred during a read operation.
        /// </summary>
        StorageReadFault = 0x8003001E,

        /// <summary>
        /// A lock violation has occurred.
        /// </summary>
        StorageLockViolation = 0x80030021,

        /// <summary>
        /// An invalid parameter was passed to the remote procedure call.
        /// </summary>
        StorageInvalidParameter = 0x80030057,

        /// <summary>
        /// There is insufficient disk space to complete the operation. 
        /// </summary>
        StreamSizeError = 0x80030070,

        /// <summary>
        /// An invalid flag was passed to a remote procedure call.
        /// </summary>
        StorageInvalidFlag = 0x800300FF,

        /// <summary>
        /// A stream could not be saved.
        /// </summary>
        StorageCannotSave = 0x80030103,

        /// <summary>
        /// The server does not support this method call.
        /// </summary>
        NotSupported = 0x80040102,

        /// <summary>
        /// Unicode characters were requested when only 8-bit characters are supported, or vice versa.
        /// </summary>
        InvalidCharacterWidth = 0x80040103,

        /// <summary>
        /// In the context of this method call, a string exceeds the maximum permitted length.
        /// </summary>
        StringTooLong = 0x80040105,

        /// <summary>
        /// An unrecognized flag bit was passed to a method call.
        /// </summary>
        InvalidFlag = 0x80040106,

        /// <summary>
        /// An incorrectly formatted EntryID was passed to a method call.
        /// </summary>
        InvalidEntryID = 0x80040107,

        /// <summary>
        /// A method call was made using a reference to an object that has been destroyed or is not in a viable state.
        /// </summary>
        InvalidObject = 0x80040108,

        /// <summary>
        /// An attempt to commit changes failed because the object was changed separately.
        /// </summary>
        ObjectChanged = 0x80040109,

        /// <summary>
        /// An operation failed because the object was deleted separately.
        /// </summary>
        ObjectDeleted = 0x8004010A,

        /// <summary>
        /// A table operation failed because a separate operation was in progress at the same time.
        /// </summary>
        ServerBusy = 0x8004010B,

        /// <summary>
        /// Not enough disk space was available to complete the operation.
        /// </summary>
        OutOfDisk = 0x8004010D,

        /// <summary>
        /// Not enough of an unspecified resource was available to complete the operation.
        /// </summary>
        OutOfResources = 0x8004010E,

        /// <summary>
        /// The requested object could not be found at the server.
        /// </summary>
        NotFound = 0x8004010F,

        /// <summary>
        /// Client and server versions are not compatible.
        /// </summary>
        VersionMismatch = 0x80040110,

        /// <summary>
        /// A client was unable to log on to the server.
        /// </summary>
        LogonFailed = 0x80040111,

        /// <summary>
        /// A server or service is unable to create any more sessions.
        /// </summary>
        TooManySessions = 0x80040112,

        /// <summary>
        /// An operation failed because a user cancelled it.
        /// </summary>
        UserCanceled = 0x80040113,

        /// <summary>
        /// A RopAbort ([MS-OXCROPS] section 2.2.5.5) or RopAbortSubmit ([MS-OXCROPS] section 2.2.7.2) ROP request was unsuccessful.
        /// </summary>
        AbortFailed = 0x80040114,

        /// <summary>
        /// An operation was unsuccessful because of a problem with network operations or services.
        /// </summary>
        NetworkError = 0x80040115,

        /// <summary>
        /// There was a problem writing to or reading from disk.
        /// </summary>
        DiskError = 0x80040116,

        /// <summary>
        /// The operation requested is too complex for the server to handle; often applied to restrictions.
        /// </summary>
        TooComplex = 0x80040117,

        /// <summary>
        /// The column requested is not allowed in this type of table.
        /// </summary>
        InvalidColumn = 0x80040118,

        /// <summary>
        /// A property cannot be updated because it is read-only, computed by the server.
        /// </summary>
        ComputedValue = 0x8004011A,

        /// <summary>
        /// There is an internal inconsistency in a database, or in a complex property value.
        /// </summary>
        CorruptData = 0x8004011B,

        /// <summary>
        /// The server is not configured to support the code page requested by the client.
        /// </summary>
        InvalidCodepage = 0x8004011E,

        /// <summary>
        /// The server is not configured to support the locale requested by the client.
        /// </summary>
        InvalidLocale = 0x8004011F,

        /// <summary>
        /// The operation failed due to clock skew between servers.
        /// </summary>
        TimeSkew = 0x80040123,

        /// <summary>
        /// Indicates that the server session has been destroyed, possibly by a server restart.
        /// </summary>
        EndOfSession = 0x80040200,

        /// <summary>
        /// Indicates that the EntryID passed to OpenEntry was created by a different MAPI provider.
        /// </summary>
        UnknownEntryId = 0x80040201,

        /// <summary>
        /// A complex operation such as building a table row set could not be completed.
        /// </summary>
        NotCompleted = 0x80040400,

        /// <summary>
        /// An asynchronous operation did not succeed within the specified time-out.
        /// </summary>
        Timeout = 0x80040401,

        /// <summary>
        /// A table essential to the operation is empty.
        /// </summary>
        EmptyTable = 0x80040402,

        /// <summary>
        /// The table is too big for the requested operation to complete.
        /// </summary>
        TableTooBig = 0x80040403,

        /// <summary>
        /// The bookmark passed to a table operation was not created on the same table.
        /// </summary>
        InvalidBookmark = 0x80040405,

        /// <summary>
        /// A wait time-out has expired.
        /// </summary>
        ErrorWait = 0x80040500,

        /// <summary>
        /// The operation had to be canceled.
        /// </summary>
        ErrorCancel = 0x80040501,

        /// <summary>
        /// The server does not support the suppression of read receipts.
        /// </summary>
        NoSuppress = 0x80040602,

        /// <summary>
        /// A folder or item cannot be created because one with the same name or other criteria already exists.
        /// </summary>
        CollidingNames = 0x80040604,

        /// <summary>
        /// The subsystem is not ready.
        /// </summary>
        NotInitialized = 0x80040605,

        /// <summary>
        /// A message cannot be sent because it has no recipients (1).
        /// </summary>
        NoRecipients = 0x80040607,

        /// <summary>
        /// A message cannot be opened for modification because it has already been sent.
        /// </summary>
        AlreadySent = 0x80040608,

        /// <summary>
        /// A folder cannot be deleted because it still contains subfolders.
        /// </summary>
        HasFolders = 0x80040609,

        /// <summary>
        /// A folder cannot be deleted because it still contains messages.
        /// </summary>
        HasMessages = 0x8004060A,

        /// <summary>
        /// A folder move or copy operation would create a cycle (typically when the request is to copy a parent folder to one of its subfolders).
        /// </summary>
        FolderCycle = 0x8004060B,

        /// <summary>
        /// Too many locks have been requested.
        /// </summary>
        TooManyLocks = 0x8004060D,

        /// <summary>
        /// An unresolved recipient (2) matches more than one entry in the directory.
        /// </summary>
        AmbiguousRecipient = 0x80040700,

        /// <summary>
        /// The requested object was previously deleted.
        /// </summary>
        SyncObjectDeleted = 0x80040800,

        /// <summary>
        /// An error occurred, but it's safe to ignore the error, perhaps because the change in question has been superseded.
        /// </summary>
        IgnoreFailure = 0x80040801,

        /// <summary>
        /// Conflicting changes to an object have been detected.
        /// </summary>
        SyncConflict = 0x80040802,

        /// <summary>
        /// The parent folder could not be found.
        /// </summary>
        NoParentFolder = 0x80040803,

        /// <summary>
        /// An operation would create a cycle (for instance, by copying a parent folder to one of its subfolders). 
        /// </summary>
        CycleDetected = 0x80040804,

        /// <summary>
        /// A sync operation did not take place, possibly due to a conflicting change.
        /// </summary>
        NotSynchronized = 0x80040805,

        /// <summary>
        /// The Store object cannot store any more named property mappings.
        /// </summary>
        NamedPropertyQuota = 0x80040900,

        /// <summary>
        /// The server does not implement this method call.
        /// </summary>
        NotImplemented = 0x80040FFF
    }

    /// <summary>
    /// 2.4.1 Additional Error Codes
    /// </summary>
    public enum AdditionalErrorCodes : uint
    {
        /// <summary>
        /// Unspecified database failure.
        /// </summary>
        IsamError = 0x000003EA,

        /// <summary>
        /// Unable to identify a home Store object for this user.
        /// </summary>
        UnknownUser = 0x000003EB,

        /// <summary>
        /// The server is in the process of stopping.
        /// </summary>
        Exiting = 0x000003ED,

        /// <summary>
        /// Protocol settings for this user are incorrect.
        /// </summary>
        BadConfiguration = 0x000003EE,

        /// <summary>
        /// The specified code page is not installed on the server.
        /// </summary>
        UnknownCodePage = 0x000003EF,

        /// <summary>
        /// The server is out of memory.
        /// </summary>
        ServerMemory = 0x000003F0,

        /// <summary>
        /// This user does not have access rights to the mailbox.
        /// </summary>
        LoginPermission = 0x000003F2,

        /// <summary>
        /// The database has been restored and needs fix-up but cannot be fixed up.
        /// </summary>
        DatabaseRolledBack = 0x000003F3,

        /// <summary>
        /// The database file has been copied from another server.
        /// </summary>
        DatabaseCopiedError = 0x000003F4,

        /// <summary>
        /// Auditing of security operations is not permitted.
        /// </summary>
        AuditNotAllowed = 0x000003F5,

        /// <summary>
        /// User has no security identifier.
        /// </summary>
        ZombieUser = 0x000003F6,

        /// <summary>
        /// An access control list (ACL) cannot be converted to NTFS format.
        /// </summary>
        UnconvertableACL = 0x000003F7,

        /// <summary>
        /// No Jet session is available.
        /// </summary>
        NoFreeJetSessions = 0x0000044C,

        /// <summary>
        /// Warning, a Jet session other than the one requested was returned.
        /// </summary>
        DifferentJetSession = 0x0000044D,

        /// <summary>
        /// An error occurred when attempting to remove a database file.
        /// </summary>
        FileRemove = 0x0000044F,

        /// <summary>
        /// Parameter value overflow.
        /// </summary>
        ParameterOverflow = 0x00000450,

        /// <summary>
        /// Bad message store database version number
        /// </summary>
        BadVersion = 0x00000451,

        /// <summary>
        /// Too many columns requested in SetColumns.
        /// </summary>
        TooManyColumns = 0x00000452,

        /// <summary>
        /// A ROP has more data to return.
        /// </summary>
        HaveMore = 0x00000453,

        /// <summary>
        /// General database problem.
        /// </summary>
        DatabaseError = 0x00000454,

        /// <summary>
        /// An index name is larger than what Jet allows.
        /// </summary>
        IndexNameTooBig = 0x00000455,

        /// <summary>
        /// The property data type is not supported.
        /// </summary>
        UnsupportedProperty = 0x00000456,

        /// <summary>
        /// During AbortSubmit, a message was not saved.
        /// </summary>
        MessageNotSaved = 0x00000457,

        /// <summary>
        /// A notification could not be published at this time.
        /// </summary>
        UnpublishedNotification = 0x00000459,

        /// <summary>
        /// Moving or copying folders to a different top-level hierarchy is not supported.
        /// </summary>
        DifferentRoot = 0x0000045B,

        /// <summary>
        /// Invalid folder name.
        /// </summary>
        BadFolderName = 0x0000045C,

        /// <summary>
        /// The attachment is open.
        /// </summary>
        AttachmentOpen = 0x0000045D,

        /// <summary>
        /// The collapse state given to SetCollapseState is invalid.
        /// </summary>
        InvalidCollapseState = 0x0000045E,

        /// <summary>
        /// While walking a folder tree, do not consider children of this folder.
        /// </summary>
        SkipMyChildren = 0x0000045F,

        /// <summary>
        /// The operation is not supported on a search folder.
        /// </summary>
        SearchFolder = 0x00000460,

        /// <summary>
        /// The operation is valid only on a search folder.
        /// </summary>
        NotSearchFolder = 0x00000461,

        /// <summary>
        /// This is a Receive folder and cannot be deleted.
        /// </summary>
        FolderSetReceive = 0x00000462,

        /// <summary>
        /// No Receive folder is available (even no default).
        /// </summary>
        NoReceiveFolder = 0x00000463,

        /// <summary>
        /// Deleting a message that has been submitted for sending is not permitted.
        /// </summary>
        DeleteSubmittedMessage = 0x00000465,

        /// <summary>
        /// It was impossible to deliver to this recipient (1).
        /// </summary>
        InvalidRecipients = 0x00000467,

        /// <summary>
        /// No replica of the public folder in this Store object.
        /// </summary>
        NoReplicaHere = 0x00000468,

        /// <summary>
        /// No available Store object has a replica of this public folder.
        /// </summary>
        NoReplicaAvailable = 0x00000469,

        /// <summary>
        /// The operation is invalid on a public Store object.
        /// </summary>
        PublicDatabase = 0x0000046A,

        /// <summary>
        /// The operation is valid only on a public Store object.
        /// </summary>
        NotPublicDatabase = 0x0000046B,

        /// <summary>
        /// The record was not found.
        /// </summary>
        RecordNotFound = 0x0000046C,

        /// <summary>
        /// A replication conflict was detected.
        /// </summary>
        ReplicationConflict = 0x0000046D,

        /// <summary>
        /// Prevented an overrun while reading a fast transfer buffer.
        /// </summary>
        FXBufferOverrun = 0x00000470,

        /// <summary>
        /// No more in a fast transfer buffer.
        /// </summary>
        FXBufferEmpty = 0x00000471,

        /// <summary>
        /// Partial long value in a fast transfer buffer.
        /// </summary>
        FXPartialValue = 0x00000472,

        /// <summary>
        /// No room for an atomic value in a fast transfer buffer.
        /// </summary>
        FxNoRoom = 0x00000473,

        /// <summary>
        /// Housekeeping functions have exceeded their time window.
        /// </summary>
        TimeExpired = 0x00000474,

        /// <summary>
        /// An error occurred on the destination folder during a copy operation.
        /// </summary>
        DestinationError = 0x00000475,

        /// <summary>
        /// The Store object was not properly initialized.
        /// </summary>
        DatabaseNotInitialized = 0x00000476,

        /// <summary>
        /// This server does not host the user's mailbox database.
        /// </summary>
        WrongServer = 0x00000478,

        /// <summary>
        /// A buffer passed to this function is not big enough.
        /// </summary>
        BufferTooSmall = 0x0000047D,

        /// <summary>
        /// Linked attachments could not be resolved to actual files.
        /// </summary>
        AttachmentResolutionRequired = 0x0000047E,

        /// <summary>
        /// The service is in a paused state.
        /// </summary>
        ServerPaused = 0x0000047F,

        /// <summary>
        /// The server is too busy to complete an operation.
        /// </summary>
        ServerBusy = 0x00000480,

        /// <summary>
        /// No such logon exists in the Store object's Logon list.
        /// </summary>
        NoSuchLogon = 0x00000481,

        /// <summary>
        /// Internal error: the service cannot load a required DLL.
        /// </summary>
        LoadLibraryFailed = 0x00000482,

        /// <summary>
        /// A synchronization object has already been configured.
        /// </summary>
        AlreadyConfigured = 0x00000483,

        /// <summary>
        /// A synchronization object has not yet been configured.
        /// </summary>
        NotConfigured = 0x00000484,

        /// <summary>
        /// A code page conversion incurred data loss.
        /// </summary>
        DataLoss = 0x00000485,

        /// <summary>
        /// The maximum number of send threads has been exceeded.
        /// </summary>
        MaximumSendThreadExceeded = 0x00000488,

        /// <summary>
        /// A fast transfer error marker was found, and recovery is necessary.
        /// </summary>
        FxErrorMarker = 0x00000489,

        /// <summary>
        /// There are no more free Jet tables.
        /// </summary>
        NoFreeJtabs = 0x0000048A,

        /// <summary>
        /// The operation is valid only on a private mailbox database.
        /// </summary>
        NotPrivateDatabase = 0x0000048B,

        /// <summary>
        /// The Store object has been locked by the ISINTEG utility.
        /// </summary>
        IsintegMDB = 0x0000048C,

        /// <summary>
        /// A recovery storage group operation was attempted on a non-RSG Store object, or vice versa.
        /// </summary>
        RecoveryMismatch = 0x0000048D,

        /// <summary>
        /// Attempt to delete a critical table, such as the messages or attachments table.
        /// </summary>
        TableMayNotBeDeleted = 0x0000048E,

        /// <summary>
        /// Attempt to perform a recursive search on a search folder.
        /// </summary>
        SearchFolderScopeViolation = 0x00000490,

        /// <summary>
        /// Error in registering RPC interfaces.
        /// </summary>
        RpcRegisterIf = 0x000004B1,

        /// <summary>
        /// Error in starting the RPC listener.
        /// </summary>
        RpcListen = 0x000004B2,

        /// <summary>
        /// A badly formatted RPC buffer was detected. (ecRpcFormat)
        /// </summary>
        RpcFormat = 0x000004B6,

        /// <summary>
        /// Single instance storage cannot be used in this case.
        /// </summary>
        NoCopyTo = 0x000004B7,

        /// <summary>
        /// An object handle reference in the RPC buffer could not be resolved.
        /// </summary>
        NullObject = 0x000004B9,

        /// <summary>
        /// Server requests client to use authentication.
        /// </summary>
        RpcAuthentication = 0x000004BC,

        /// <summary>
        /// The server doesn't recognize a client's authentication level.
        /// </summary>
        RpcBadAuthenticationLevel = 0x000004BD,

        /// <summary>
        /// The subrestriction of a comment restriction is empty.
        /// </summary>
        NullCommentRestriction = 0x000004BE,

        /// <summary>
        /// Rule data was unavailable for this folder.
        /// </summary>
        RulesLoadError = 0x000004CC,

        /// <summary>
        /// Delivery-time failure in rule execution.
        /// </summary>
        RulesDeliverErr = 0x000004CD,

        /// <summary>
        /// Invalid syntax in a stored rule condition or action.
        /// </summary>
        RulesParsingErr = 0x000004CE,

        /// <summary>
        /// Failure creating a deferred rule action error message.
        /// </summary>
        RulesCreateDAE = 0x000004CF,

        /// <summary>
        /// Failure creating a deferred rule action message.
        /// </summary>
        RulesCreateDAM = 0x000004D0,

        /// <summary>
        /// A move or copy rule action could not be performed due to a problem with the target folder.
        /// </summary>
        RulesNoMoveCopyFolder = 0x000004D1,

        /// <summary>
        /// A move or copy rule action could not be performed due to a permissions problem with the target folder.
        /// </summary>
        RulesNoFolderRights = 0x000004D2,

        /// <summary>
        /// A message could not be delivered because it exceeds a size limit.
        /// </summary>
        MessageTooBig = 0x000004D4,

        /// <summary>
        /// There is a problem with the form mapped to the message's message class.
        /// </summary>
        FormNotValid = 0x000004D5,

        /// <summary>
        /// Delivery to the desired folder was not authorized.
        /// </summary>
        NotAuthorized = 0x000004D6,

        /// <summary>
        /// The message was deleted by a rule action.
        /// </summary>
        DeleteMessage = 0x000004D7,

        /// <summary>
        /// Delivery of the message was denied by a rule action.
        /// </summary>
        BounceMessage = 0x000004D8,

        /// <summary>
        /// The operation failed because it would have exceeded a resource quota.
        /// </summary>
        QuotaExceeded = 0x000004D9,

        /// <summary>
        /// A message could not be submitted because its size exceeds the defined maximum.
        /// </summary>
        MaxSubmissionExceeded = 0x000004DA,

        /// <summary>
        /// The maximum number of message attachments has been exceeded.
        /// </summary>
        MaxAttachmentExceeded = 0x000004DB,

        /// <summary>
        /// The user account does not have permission to send mail as the owner of this mailbox.
        /// </summary>
        SendAsDenied = 0x000004DC,

        /// <summary>
        /// The operation failed because it would have exceeded the mailbox's shutoff quota.
        /// </summary>
        ShutoffQuotaExceeded = 0x000004DD,

        /// <summary>
        /// A client has opened too many objects of a specific type.
        /// </summary>
        TooManyOpenObjects = 0x000004DE,

        /// <summary>
        /// The server is configured to block clients of this version.
        /// </summary>
        ClientVersionBlocked = 0x000004DF,

        /// <summary>
        /// The server is configured to block RPC connections via HTTP.
        /// </summary>
        RpcHttpDisallowed = 0x000004E0,

        /// <summary>
        /// The server is configured to block online mode connections; only cached mode connections are allowed.
        /// </summary>
        CachedModeRequired = 0x000004E1,

        /// <summary>
        /// The folder has been deleted but not yet cleaned up.
        /// </summary>
        FolderNotCleanedUp = 0x000004E3,

        /// <summary>
        /// Part of a ROP buffer was incorrectly formatted.
        /// </summary>
        FormatError = 0x000004ED,

        /// <summary>
        /// Error in expanding or collapsing rows in a categorized view.
        /// </summary>
        NotExpanded = 0x000004F7,

        /// <summary>
        /// Error in expanding or collapsing rows in a categorized view.
        /// </summary>
        NotCollapsed = 0x000004F8,

        /// <summary>
        /// Leaf rows cannot be expanded; only category header rows can be expanded.
        /// </summary>
        NoExpandLeafRow = 0x000004F9,

        /// <summary>
        /// An operation was attempted on a named property ID for which no name has been registered.
        /// </summary>
        UnregisteredNameProp = 0x000004FA,

        /// <summary>
        /// Access to the folder is disabled, perhaps because form design is in progress.
        /// </summary>
        FolderDisabled = 0x000004FB,

        /// <summary>
        /// There is an inconsistency in the Store object's association with its server.
        /// </summary>
        DomainError = 0x000004FC,

        /// <summary>
        /// The operation requires create access rights that the user does not have.
        /// </summary>
        NoCreateRight = 0x000004FF,

        /// <summary>
        /// The operation requires create access rights at a public folder root.
        /// </summary>
        PublicRoot = 0x00000500,

        /// <summary>
        /// The operation requires read access rights that the user does not have.
        /// </summary>
        NoReadRight = 0x00000501,

        /// <summary>
        /// The operation requires create subfolder access rights that the user does not have.
        /// </summary>
        NoCreateSubfolderRight = 0x00000502,

        /// <summary>
        /// The source message contains the destination message and cannot be attached to it.
        /// </summary>
        MessageCycle = 0x00000504,

        /// <summary>
        /// The RPC buffer contains a destination object handle that could not be resolved to a Server object.
        /// </summary>
        NullDestinationObject = 0x00000503,

        /// <summary>
        /// A hard limit on the number of recipients (1) per message was exceeded.
        /// </summary>
        TooManyRecips = 0x00000505,

        /// <summary>
        /// The operation failed because the target message is being scanned for viruses.
        /// </summary>
        VirusScanInProgress = 0x0000050A,

        /// <summary>
        /// The operation failed because the target message is infected with a virus.
        /// </summary>
        VirusDetected = 0x0000050B,

        /// <summary>
        /// The mailbox is in transit and is not accepting mail.
        /// </summary>
        MailboxInTransit = 0x0000050C,

        /// <summary>
        /// The operation failed because the Store object is being backed up.
        /// </summary>
        BackupInProgress = 0x0000050D,

        /// <summary>
        /// The operation failed because the target message was infected with a virus and has been deleted.
        /// </summary>
        VirusMessageDeleted = 0x0000050E,

        /// <summary>
        /// Backup steps were performed out of sequence.
        /// </summary>
        InvalidBackupSequence = 0x0000050F,

        /// <summary>
        /// The requested backup type was not recognized.
        /// </summary>
        InvalidBackupType = 0x00000510,

        /// <summary>
        /// Too many backups are already in progress.
        /// </summary>
        TooManyBackups = 0x00000511,

        /// <summary>
        /// A restore is already in progress.
        /// </summary>
        RestoreInProgress = 0x00000512,

        /// <summary>
        /// The object already exists.
        /// </summary>
        DuplicateObject = 0x00000579,

        /// <summary>
        /// An internal database object could not be found.
        /// </summary>
        ObjectNotFound = 0x0000057A,

        /// <summary>
        /// The template Message ID in a reply rule object is missing or incorrect.
        /// </summary>
        FixupReplyRule = 0x0000057B,

        /// <summary>
        /// The reply template could not be found for a message that triggered an auto-reply rule.
        /// </summary>
        TemplateNotFound = 0x0000057C,

        /// <summary>
        /// An error occurred while executing a rule action.
        /// </summary>
        RuleExecution = 0x0000057D,

        /// <summary>
        /// A Server object could not be found in the directory.
        /// </summary>
        DSNoSuchObject = 0x0000057E,

        /// <summary>
        /// An attempt to tombstone a message already in the message tombstone list failed.
        /// </summary>
        AlreadyTombstoned = 0x0000057F,

        /// <summary>
        /// A write operation was attempted in a read-only transaction.
        /// </summary>
        ReadOnlyTransaction = 0x00000596,

        /// <summary>
        /// Attempt to pause a server that is already paused.
        /// </summary>
        Paused = 0x0000060E,

        /// <summary>
        /// Attempt to unpause a server that is not paused. 
        /// </summary>
        NotPaused = 0x0000060F,

        /// <summary>
        /// The operation was attempted on the wrong mailbox.
        /// </summary>
        WrongMailbox = 0x00000648,

        /// <summary>
        /// The account password needs to be changed.
        /// </summary>
        ChangePassword = 0x0000064C,

        /// <summary>
        /// The account password has expired.
        /// </summary>
        PasswordExpired = 0x0000064D,

        /// <summary>
        /// The account has logged on from the wrong workstation.
        /// </summary>
        InvalidWorkstation = 0x0000064E,

        /// <summary>
        /// The account has logged on at the wrong time of day.
        /// </summary>
        InvalidLogonHours = 0x0000064F,

        /// <summary>
        /// The account is disabled.
        /// </summary>
        AccountDisabled = 0x00000650,

        /// <summary>
        /// The rule data contains an invalid rule version.
        /// </summary>
        RuleVersion = 0x000006A4,

        /// <summary>
        /// The rule condition or action was incorrectly formatted.
        /// </summary>
        RuleFormat = 0x000006A5,

        /// <summary>
        /// The rule is not authorized to send from this mailbox.
        /// </summary>
        RuleSendAsDenied = 0x000006A6,

        /// <summary>
        /// A newer client requires functionality that an older server does not support.
        /// </summary>
        NoServerSupport = 0x000006B9,

        /// <summary>
        /// An attempt to unlock a message failed because the lock had already timed out.
        /// </summary>
        LockTimedOut = 0x000006BA,

        /// <summary>
        /// The operation failed because the target object is locked.
        /// </summary>
        ObjectLocked = 0x000006BB,

        /// <summary>
        /// Attempt to lock a nonexistent object.
        /// </summary>
        InvalidLockNamespace = 0x000006BD,

        /// <summary>
        /// Operation failed because the message has been deleted.
        /// </summary>
        MessageDeleted = 0x000007D6,

        /// <summary>
        /// The requested protocol is disabled in the server configuration.
        /// </summary>
        ProtocolDisabled = 0x000007D8,

        /// <summary>
        /// Clear text logons were disabled.
        /// </summary>
        CleartextLogonDisabled = 0x000007D9,

        /// <summary>
        /// The operation was rejected, perhaps because it is not supported.
        /// </summary>
        Rejected = 0x000007EE,

        /// <summary>
        /// User account information did not uniquely identify a user.
        /// </summary>
        AmbiguousAlias = 0x0000089A,

        /// <summary>
        /// No mailbox object for this logon exists in the address book.
        /// </summary>
        UnknownMailbox = 0x0000089B,

        /// <summary>
        /// Internal error in evaluating an expression.
        /// </summary>
        ExpressionReserved = 0x000008FC,

        /// <summary>
        /// The expression tree exceeds a defined depth limit.
        /// </summary>
        ExpressionParseDepth = 0x000008FD,

        /// <summary>
        /// An argument to a function has the wrong type.
        /// </summary>
        ExpressionArgumentType = 0x000008FE,

        /// <summary>
        /// Syntax error in expression.
        /// </summary>
        ExpressionSyntax = 0x000008FF,

        /// <summary>
        /// Invalid string token in expression.
        /// </summary>
        ExpressionBadStringToken = 0x00000900,

        /// <summary>
        /// Invalid column name in expression.
        /// </summary>
        ExpressionBadColToken = 0x00000901,

        /// <summary>
        /// Property types, for example, in a comparison expression, are incompatible.
        /// </summary>
        ExpressionTypeMismatch = 0x00000902,

        /// <summary>
        /// The requested operator is not supported.
        /// </summary>
        ExpressionOperatorNotSupported = 0x00000903,

        /// <summary>
        /// Divide by zero doesn't work.
        /// </summary>
        ExpressionDivideByZero = 0x00000904,

        /// <summary>
        /// The argument to a unary expression is of incorrect type.
        /// </summary>
        ExpressionUnaryArgument = 0x00000905,

        /// <summary>
        /// An attempt to lock a resource failed.
        /// </summary>
        NotLocked = 0x00000960,

        /// <summary>
        /// A client-supplied event has fired.
        /// </summary>
        ClientEvent = 0x00000961,

        /// <summary>
        /// Data in the event table is bad.
        /// </summary>
        CorruptEvent = 0x00000965,

        /// <summary>
        /// A watermark in the event table is bad.
        /// </summary>
        CorruptWatermark = 0x00000966,

        /// <summary>
        /// General event processing error.
        /// </summary>
        EventError = 0x00000967,

        /// <summary>
        /// An event watermark is out of range or otherwise invalid.
        /// </summary>
        WatermarkError = 0x00000968,

        /// <summary>
        /// A modification to an ACL failed because the existing ACL is not in canonical format.
        /// </summary>
        NonCanonicalACL = 0x00000969,

        /// <summary>
        /// Logon was unsuccessful because the mailbox is disabled.
        /// </summary>
        MailboxDisabled = 0x0000096C,

        /// <summary>
        /// A move or copy rule action failed because the destination folder is over quota.
        /// </summary>
        RulesFolderOverQuota = 0x0000096D,

        /// <summary>
        /// The address book server could not be reached.
        /// </summary>
        AddressBookUnavailable = 0x0000096E,

        /// <summary>
        /// Unspecified error from the address book server.
        /// </summary>
        AddressBookError = 0x0000096F,

        /// <summary>
        /// An object was not found in the address book.
        /// </summary>
        AddressBookObjectNotFound = 0x00000971,

        /// <summary>
        /// A property was not found in the address book.
        /// </summary>
        AddressBookPropertyError = 0x00000972,

        /// <summary>
        /// The server is configured to force encrypted connections, but the client requested an unencrypted connection.
        /// </summary>
        NotEncrypted = 0x00000970,

        /// <summary>
        /// An external RPC failed because the server was too busy.
        /// </summary>
        RpcServerTooBusy = 0x00000973,

        /// <summary>
        /// An external RPC failed because the local server was out of memory. 
        /// </summary>
        RpcOutOfMemory = 0x00000974,

        /// <summary>
        /// An external RPC failed because the remote server was out of memory. 
        /// </summary>
        RpcServerOutOfMemory = 0x00000975,

        /// <summary>
        /// An external RPC failed because the remote server was out of an unspecified resource.
        /// </summary>
        RpcOutOfResources = 0x00000976,

        /// <summary>
        /// An external RPC failed because the remote server was unavailable
        /// </summary>
        RpcServerUnavailable = 0x00000977,

        /// <summary>
        /// A failure occurred while setting the secure submission state of a message.
        /// </summary>
        SecureSubmitError = 0x0000097A,

        /// <summary>
        /// Requested events were already deleted from the queue.
        /// </summary>
        EventsDeleted = 0x0000097C,

        /// <summary>
        /// A component service is in the process of shutting down.
        /// </summary>
        SubsystemStopping = 0x0000097D,

        /// <summary>
        /// The system attendant service is unavailable.
        /// </summary>
        AttendantUnavailable = 0x0000097E,

        /// <summary>
        /// The content indexer service is stopping.
        /// </summary>
        CIStopping = 0x00000A28,

        /// <summary>
        /// An internal fast transfer object has invalid state.
        /// </summary>
        FxInvalidState = 0x00000A29,

        /// <summary>
        /// Fast transfer parsing has hit an invalid marker.
        /// </summary>
        FxUnexpectedMarker = 0x00000A2A,

        /// <summary>
        /// A copy of this message has already been delivered.
        /// </summary>
        DuplicateDelivery = 0x00000A2B,

        /// <summary>
        /// The condition was not met for a conditional operation.
        /// </summary>
        ConditionViolation = 0x00000A2C,

        /// <summary>
        /// An RPC client has exceeded the defined limit of RPC connection pools.
        /// </summary>
        MaximumConnectionPoolsExceeded = 0x00000A2D,

        /// <summary>
        /// The RPC connection is no longer valid.
        /// </summary>
        InvalidRpcHandle = 0x00000A2E,

        /// <summary>
        /// There are no events in the event table, or the requested event was not found.
        /// </summary>
        EventNotFound = 0x00000A2F,

        /// <summary>
        /// A property was not copied from the message table to the message header table.
        /// </summary>
        PropertyNotPromoted = 0x00000A30,

        /// <summary>
        /// The drive hosting database files has little or no free space.
        /// </summary>
        LowFreeSpaceForDatabase = 0x00000A31,

        /// <summary>
        /// The drive hosting log files for the database has little or no free space.
        /// </summary>
        LowFreeSpaceForLogs = 0x00000A32,

        /// <summary>
        /// The mailbox has been placed under quarantine by an administrator.
        /// </summary>
        MailboxIsQuarantined = 0x00000A33,

        /// <summary>
        /// The mailbox database is being mounted.
        /// </summary>
        DatabaseMountInProgress = 0x00000A34,

        /// <summary>
        /// The mailbox database is being dismounted.
        /// </summary>
        DatabaseDismountInProgress = 0x00000A35,

        /// <summary>
        /// The number of RPC connections in use exceeds the amount budgeted for this client.
        /// </summary>
        ConnectionsOverBudget = 0x00000A36,

        /// <summary>
        /// The mailbox was not found in the mailbox metadata cache.
        /// </summary>
        NotFoundInContainer = 0x00000A37,

        /// <summary>
        /// An item cannot be removed from an internal list.
        /// </summary>
        CannotRemove = 0x00000A38,

        /// <summary>
        /// An RPC client has attempted connection using a connection pool unknown to the server.
        /// </summary>
        InvalidConnectionPool = 0x00000A39,

        /// <summary>
        /// A nonspecified failure occurred while scanning an item.
        /// </summary>
        VirusScanGeneralFailure = 0x00000A3A,

        /// <summary>
        /// The Resource Failure Simulator failed.
        /// </summary>
        IsamErrorRfsFailure = 0xFFFFFF9C,

        /// <summary>
        /// The Resource Failure Simulator has not been initialized.
        /// </summary>
        IsamErrorRfsNotArmed = 0xFFFFFF9B,

        /// <summary>
        /// The file could not be closed.
        /// </summary>
        IsamErrorFileClose = 0xFFFFFF9A,

        /// <summary>
        /// The thread could not be started.
        /// </summary>
        IsamErrorOutOfThreads = 0xFFFFFF99,

        /// <summary>
        /// The system is busy due to too many I/Os.
        /// </summary>
        IsamErrorTooManyIO = 0xFFFFFF97,

        /// <summary>
        /// The requested asynchronous task could not be executed.
        /// </summary>
        IsamErrorTaskDropped = 0xFFFFFF96,

        /// <summary>
        /// There was a fatal internal error.
        /// </summary>
        IsamErrorInternalError = 0xFFFFFF95,

        /// <summary>
        /// The buffer dependencies were set improperly and there was a recovery failure.
        /// </summary>
        IsamErrorDatabaseBufferDependenciesCorrupted = 0xFFFFFF01,

        /// <summary>
        /// The version already existed and there was a recovery failure. 
        /// </summary>
        IsamErrorPreviousVersion = 0xFFFFFEBE,

        /// <summary>
        /// The page boundary has been reached. 
        /// </summary>
        IsamErrorPageBoundary = 0xFFFFFEBD,

        /// <summary>
        /// The key boundary has been reached. 
        /// </summary>
        IsamErrorKeyBoundary = 0xFFFFFEBC,

        /// <summary>
        /// The database is corrupt. 
        /// </summary>
        IsamErrorBadPageLink = 0xFFFFFEB9,

        /// <summary>
        /// The bookmark has no corresponding address in the database. 
        /// </summary>
        IsamErrorBadBookmark = 0xFFFFFEB8,

        /// <summary>
        /// The call to the operating system failed. 
        /// </summary>
        IsamErrorNTSystemCallFailed = 0xFFFFFEB2,

        /// <summary>
        /// A parent database is corrupt. 
        /// </summary>
        IsamErrorBadParentPageLink = 0xFFFFFEAE,

        /// <summary>
        /// The AvailExt cache does not match the B+ tree. 
        /// </summary>
        IsamErrorSPAvailExtCacheOutOfSync = 0xFFFFFEAC,

        /// <summary>
        /// The AllAvailExt space tree is corrupt. 
        /// </summary>
        IsamErrorSPAvailExtCorrupted = 0xFFFFFEAB,

        /// <summary>
        /// An out of memory error occurred while allocating an AvailExt cache node. 
        /// </summary>
        IsamErrorSPAvailExtCacheOutOfMemory = 0xFFFFFEAA,

        /// <summary>
        /// The OwnExt space tree is corrupt. 
        /// </summary>
        IsamErrorSPOwnExtCorrupted = 0xFFFFFEA9,

        /// <summary>
        /// The Dbtime on the current page is greater than the global database dbtime. 
        /// </summary>
        IsamErrorDbTimeCorrupted = 0xFFFFFEA8,

        /// <summary>
        /// An attempt to create a key for an index entry failed because the key would have been truncated and the index definition disallows key truncation. 
        /// </summary>
        IsamErrorKeyTruncated = 0xFFFFFEA6,

        /// <summary>
        /// The key is too large. 
        /// </summary>
        IsamErrorKeyTooBig = 0xFFFFFE68,

        /// <summary>
        /// The logged operation cannot be redone. 
        /// </summary>
        IsamErrorInvalidLoggedOperation = 0xFFFFFE0C,

        /// <summary>
        /// The log file is corrupt. 
        /// </summary>
        IsamErrorLogFileCorrupt = 0xFFFFFE0B,

        /// <summary>
        /// A backup directory was not given. 
        /// </summary>
        IsamErrorNoBackupDirectory = 0xFFFFFE09,

        /// <summary>
        /// The backup directory is not empty. 
        /// </summary>
        IsamErrorBackupDirectoryNotEmpty = 0xFFFFFE08,

        /// <summary>
        /// The backup is already active. (JET_errBackupInProgress)
        /// </summary>
        IsamErrorBackupInProgress = 0xFFFFFE07,

        /// <summary>
        /// A restore is in progress. (JET_errRestoreInProgress)
        /// </summary>
        IsamErrorRestoreInProgress = 0xFFFFFE06,

        /// <summary>
        /// The log file is missing for the checkpoint. (JET_errMissingPreviousLogFile)
        /// </summary>
        IsamErrorMissingPreviousLogFile = 0xFFFFFE03,

        /// <summary>
        /// There was a failure writing to the log file. (JET_errLogWriteFail)
        /// </summary>
        IsamErrorLogWriteFail = 0xFFFFFE02,

        /// <summary>
        /// The attempt to write to the log after recovery failed. (JET_errLogDisabledDueToRecoveryFailure)
        /// </summary>
        IsamErrorLogDisabledDueToRecoveryFailure = 0xFFFFFE01,

        /// <summary>
        /// The attempt to write to the log during the recovery redo failed. (JET_errCannotLogDuringRecoveryRedo)
        /// </summary>
        IsamErrorCannotLogDuringRecoveryRedo = 0xFFFFFE00,

        /// <summary>
        /// The name of the log file does not match the internal generation number. (JET_errLogGenerationMismatch)
        /// </summary>
        IsamErrorLogGenerationMismatch = 0xFFFFFDFF,

        /// <summary>
        /// The version of the log file is not compatible with the ESE version. (JET_errBadLogVersion)
        /// </summary>
        IsamErrorBadLogVersion = 0xFFFFFDFE,

        /// <summary>
        /// The time stamp in the next log does not match the expected time stamp. (JET_errInvalidLogSequence)
        /// </summary>
        IsamErrorInvalidLogSequence = 0xFFFFFDFD,

        /// <summary>
        /// The log is not active. (JET_errLoggingDisabled)
        /// </summary>
        IsamErrorLoggingDisabled = 0xFFFFFDFC,

        /// <summary>
        /// The log buffer is too small for recovery. (JET_errLogBufferTooSmall)
        /// </summary>
        IsamErrorLogBufferTooSmall = 0xFFFFFDFB,

        /// <summary>
        /// The maximum log file number has been exceeded. (JET_errLogSequenceEnd)
        /// </summary>
        IsamErrorLogSequenceEnd = 0xFFFFFDF9,

        /// <summary>
        /// There is no backup in progress. (JET_errNoBackup)
        /// </summary>
        IsamErrorNoBackup = 0xFFFFFDF8,

        /// <summary>
        /// The backup call is out of sequence. (JET_errInvalidBackupSequence)
        /// </summary>
        IsamErrorInvalidBackupSequence = 0xFFFFFDF7,

        /// <summary>
        /// A backup cannot be done at this time. (JET_errBackupNotAllowedYet)
        /// </summary>
        IsamErrorBackupNotAllowedYet = 0xFFFFFDF5,

        /// <summary>
        /// A backup file could not be deleted. (JET_errDeleteBackupFileFail)
        /// </summary>
        IsamErrorDeleteBackupFileFail = 0xFFFFFDF4,

        /// <summary>
        /// The backup temporary directory could not be created. (JET_errMakeBackupDirectoryFail)
        /// </summary>
        IsamErrorMakeBackupDirectoryFail = 0xFFFFFDF3,

        /// <summary>
        /// Circular logging is enabled; an incremental backup cannot be performed. (JET_errInvalidBackup)
        /// </summary>
        IsamErrorInvalidBackup = 0xFFFFFDF2,

        /// <summary>
        /// The data was restored with errors. (JET_errRecoveredWithErrors)
        /// </summary>
        IsamErrorRecoveredWithErrors = 0xFFFFFDF1,

        /// <summary>
        /// The current log file is missing. (JET_errMissingLogFile)
        /// </summary>
        IsamErrorMissingLogFile = 0xFFFFFDF0,

        /// <summary>
        /// The log disk is full. (JET_errLogDiskFull)
        /// </summary>
        IsamErrorLogDiskFull = 0xFFFFFDEF,

        /// <summary>
        /// There is a bad signature for a log file. (JET_errBadLogSignature)
        /// </summary>
        IsamErrorBadLogSignature = 0xFFFFFDEE,

        /// <summary>
        /// There is a bad signature for a database file. (JET_errBadDbSignature)
        /// </summary>
        IsamErrorBadDbSignature = 0xFFFFFDED,

        /// <summary>
        /// There is a bad signature for a checkpoint file. (JET_errBadCheckpointSignature)
        /// </summary>
        IsamErrorBadCheckpointSignature = 0xFFFFFDEC,

        /// <summary>
        /// The checkpoint file was not found or was corrupt. (JET_errCheckpointCorrupt)
        /// </summary>
        IsamErrorCheckpointCorrupt = 0xFFFFFDEB,

        /// <summary>
        /// The database patch file page was not found during recovery. 
        /// </summary>
        IsamErrorMissingPatchPage = 0xFFFFFDEA,

        /// <summary>
        /// The database patch file page is not valid. 
        /// </summary>
        IsamErrorBadPatchPage = 0xFFFFFDE9,

        /// <summary>
        /// The redo abruptly ended due to a sudden failure while reading logs from the log file. (JET_errRedoAbruptEnded)
        /// </summary>
        IsamErrorRedoAbruptEnded = 0xFFFFFDE8,

        /// <summary>
        /// The signature in the SLV file does not agree with the database. (JET_errBadSLVSignature)
        /// </summary>
        IsamErrorBadSLVSignature = 0xFFFFFDE7,

        /// <summary>
        /// The hard restore detected that a database patch file is missing from the backup set. (JET_errPatchFileMissing)
        /// </summary>
        IsamErrorPatchFileMissing = 0xFFFFFDE6,

        /// <summary>
        /// The database does not belong with the current set of log files. (JET_errDatabaseLogSetMismatch)
        /// </summary>
        IsamErrorDatabaseLogSetMismatch = 0xFFFFFDE5,

        /// <summary>
        /// This flag is reserved. (JET_errDatabaseStreamingFileMismatch)
        /// </summary>
        IsamErrorDatabaseStreamingFileMismatch = 0xFFFFFDE4,

        /// <summary>
        /// The actual log file size does not match the configured size. (JET_errLogFileSizeMismatch)
        /// </summary>
        IsamErrorLogFileSizeMismatch = 0xFFFFFDE3,

        /// <summary>
        /// The checkpoint file could not be located. (JET_errCheckpointFileNotFound)
        /// </summary>
        IsamErrorCheckpointFileNotFound = 0xFFFFFDE2,

        /// <summary>
        /// The required log files for recovery are missing. (JET_errRequiredLogFilesMissing) 
        /// </summary>
        IsamErrorRequiredLogFilesMissing = 0xFFFFFDE1,

        /// <summary>
        /// A soft recovery is about to be used on a backup database when a restore is supposed to be used instead. 
        /// </summary>
        IsamErrorSoftRecoveryOnBackupDatabase = 0xFFFFFDE0,

        /// <summary>
        /// The databases have been recovered, but the log file size used during recovery does not match JET_paramLogFileSize. 
        /// </summary>
        IsamErrorLogFileSizeMismatchDatabasesConsistent = 0xFFFFFDDF,

        /// <summary>
        /// The log file sector size does not match the sector size of the current volume. 
        /// </summary>
        IsamErrorLogSectorSizeMismatch = 0xFFFFFDDE,

        /// <summary>
        /// The databases have been recovered, but the log file sector size (used during recovery) does not match the sector size of the current volume. 
        /// </summary>
        IsamErrorLogSectorSizeMismatchDatabasesConsistent = 0xFFFFFDDD,

        /// <summary>
        /// The databases have been recovered, but all possible log generations in the current sequence have been used. All log files and the checkpoint file is required to be deleted and databases are required to be backed up before continuing. 
        /// </summary>
        IsamErrorLogSequenceEndDatabasesConsistent = 0xFFFFFDDC,

        /// <summary>
        /// There was an illegal attempt to replay a streaming file operation where the data was not logged. This is probably caused by an attempt to roll forward with circular logging enabled. 
        /// </summary>
        IsamErrorStreamingDataNotLogged = 0xFFFFFDDB,

        /// <summary>
        /// The database was not shut down cleanly. A recovery is required first be run to properly complete database operations for the previous shutdown. 
        /// </summary>
        IsamErrorDatabaseDirtyShutdown = 0xFFFFFDDA,

        /// <summary>
        /// The last consistent time for the database has not been matched. 
        /// </summary>
        IsamErrorConsistentTimeMismatch = 0xFFFFFDD9,

        /// <summary>
        /// The database patch file is not generated from this backup. 
        /// </summary>
        IsamErrorDatabasePatchFileMismatch = 0xFFFFFDD8,

        /// <summary>
        /// The starting log number is too low for the restore. 
        /// </summary>
        IsamErrorEndingRestoreLogTooLow = 0xFFFFFDD7,

        /// <summary>
        /// The starting log number is too high for the restore. 
        /// </summary>
        IsamErrorStartingRestoreLogTooHigh = 0xFFFFFDD6,

        /// <summary>
        /// The restore log file has a bad signature. 
        /// </summary>
        IsamErrorGivenLogFileHasBadSignature = 0xFFFFFDD5,

        /// <summary>
        /// The restore log file is not contiguous. 
        /// </summary>
        IsamErrorGivenLogFileIsNotContiguous = 0xFFFFFDD4,

        /// <summary>
        /// Some restore log files are missing. 
        /// </summary>
        IsamErrorMissingRestoreLogFiles = 0xFFFFFDD3,

        /// <summary>
        /// The database missed a previous full backup before attempting to perform an incremental backup. 
        /// </summary>
        IsamErrorMissingFullBackup = 0xFFFFFDD0,

        /// <summary>
        /// The backup database size is not a multiple of the database page size. 
        /// </summary>
        IsamErrorBadBackupDatabaseSize = 0xFFFFFDCF,

        /// <summary>
        /// The current attempt to upgrade a database has been stopped because the database is already current. 
        /// </summary>
        IsamErrorDatabaseAlreadyUpgraded = 0xFFFFFDCE,

        /// <summary>
        /// The database was only partially converted to the current format. The database is required to be restored from backup. 
        /// </summary>
        IsamErrorDatabaseIncompleteUpgrade = 0xFFFFFDCD,

        /// <summary>
        /// Some current log files are missing for continuous restore. 
        /// </summary>
        IsamErrorMissingCurrentLogFiles = 0xFFFFFDCB,

        /// <summary>
        /// The dbtime on a page is smaller than the dbtimeBefore that is in the record. 
        /// </summary>
        IsamErrorDbTimeTooOld = 0xFFFFFDCA,

        /// <summary>
        /// The dbtime on a page is in advance of the dbtimeBefore that is in the record. 
        /// </summary>
        IsamErrorDbTimeTooNew = 0xFFFFFDC9,

        /// <summary>
        /// Some log or database patch files were missing during the backup. 
        /// </summary>
        IsamErrorMissingFileToBackup = 0xFFFFFDC7,

        /// <summary>
        /// A torn write was detected in a backup that was set during a hard restore. 
        /// </summary>
        IsamErrorLogTornWriteDuringHardRestore = 0xFFFFFDC6,

        /// <summary>
        /// A torn write was detected during a hard recovery (the log was not part of a backup set). 
        /// </summary>
        IsamErrorLogTornWriteDuringHardRecovery = 0xFFFFFDC5,

        /// <summary>
        /// Corruption was detected in a backup set during a hard restore. 
        /// </summary>
        IsamErrorLogCorruptDuringHardRestore = 0xFFFFFDC3,

        /// <summary>
        /// Corruption was detected during hard recovery (the log was not part of a backup set). 
        /// </summary>
        IsamErrorLogCorruptDuringHardRecovery = 0xFFFFFDC2,

        /// <summary>
        /// Logging cannot be enabled while attempting to upgrade a database. 
        /// </summary>
        IsamErrorMustDisableLoggingForDbUpgrade = 0xFFFFFDC1,

        /// <summary>
        /// Either the TargetInstance that was specified for restore has not been found or the log files do not match. 
        /// </summary>
        IsamErrorBadRestoreTargetInstance = 0xFFFFFDBF,

        /// <summary>
        /// The database engine successfully replayed all operations in the transaction log to perform a crash recovery but the caller elected to stop recovery without rolling back uncommitted updates. 
        /// </summary>
        IsamErrorRecoveredWithoutUndo = 0xFFFFFDBD,

        /// <summary>
        /// The databases to be restored are not from the same shadow copy backup. 
        /// </summary>
        IsamErrorDatabasesNotFromSameSnapshot = 0xFFFFFDBC,

        /// <summary>
        /// There is a soft recovery on a database from a shadow copy backup set. 
        /// </summary>
        IsamErrorSoftRecoveryOnSnapshot = 0xFFFFFDBB,

        /// <summary>
        /// One or more logs that were committed to this database are missing. 
        /// </summary>
        IsamErrorCommittedLogFilesMissing = 0xFFFFFDBA,

        /// <summary>
        /// One or more logs were found to be corrupt during recovery. 
        /// </summary>
        IsamErrorCommittedLogFilesCorrupt = 0xFFFFFDB6,

        /// <summary>
        /// The Unicode translation buffer is too small. (JET_errUnicodeTranslationBufferTooSmall)
        /// </summary>
        IsamErrorUnicodeTranslationBufferTooSmall = 0xFFFFFDA7,

        /// <summary>
        /// The Unicode normalization failed. 
        /// </summary>
        IsamErrorUnicodeTranslationFail = 0xFFFFFDA6,

        /// <summary>
        /// The operating system does not provide support for Unicode normalization and a normalization callback was not specified. 
        /// </summary>
        IsamErrorUnicodeNormalizationNotSupported = 0xFFFFFDA5,

        /// <summary>
        /// The existing log file has a bad signature. 
        /// </summary>
        IsamErrorExistingLogFileHasBadSignature = 0xFFFFFD9E,

        /// <summary>
        /// An existing log file is not contiguous. 
        /// </summary>
        IsamErrorExistingLogFileIsNotContiguous = 0xFFFFFD9D,

        /// <summary>
        /// A checksum error was found in the log file during backup. 
        /// </summary>
        IsamErrorLogReadVerifyFailure = 0xFFFFFD9C,

        /// <summary>
        /// A checksum error was found in the SLV file during backup. 
        /// </summary>
        IsamErrorSLVReadVerifyFailure = 0xFFFFFD9B,

        /// <summary>
        /// There are too many outstanding generations between the checkpoint and the current generation. 
        /// </summary>
        IsamErrorCheckpointDepthTooDeep = 0xFFFFFD9A,

        /// <summary>
        /// A hard recovery was attempted on a database that was not a backup database. 
        /// </summary>
        IsamErrorRestoreOfNonBackupDatabase = 0xFFFFFD99,

        /// <summary>
        /// There is an invalid grbit parameter. 
        /// </summary>
        IsamErrorInvalidGrbit = 0xFFFFFC7C,

        /// <summary>
        /// Termination is in progress. 
        /// </summary>
        IsamErrorTermInProgress = 0xFFFFFC18,

        /// <summary>
        /// This API element is not supported. 
        /// </summary>
        IsamErrorFeatureNotAvailable = 0xFFFFFC17,

        /// <summary>
        /// An invalid name is being used. 
        /// </summary>
        IsamErrorInvalidName = 0xFFFFFC16,

        /// <summary>
        /// An invalid API parameter is being used. 
        /// </summary>
        IsamErrorInvalidParameter = 0xFFFFFC15,

        /// <summary>
        /// There was an attempt to attach to a read-only database file for read/write operations. 
        /// </summary>
        IsamErrorDatabaseFileReadOnly = 0xFFFFFC10,

        /// <summary>
        /// There is an invalid database ID. 
        /// </summary>
        IsamErrorInvalidDatabaseId = 0xFFFFFC0E,

        /// <summary>
        /// The system is out of memory. 
        /// </summary>
        IsamErrorOutOfMemory = 0xFFFFFC0D,

        /// <summary>
        /// The maximum database size has been reached. 
        /// </summary>
        IsamErrorOutOfDatabaseSpace = 0xFFFFFC0C,

        /// <summary>
        /// The table is out of cursors. 
        /// </summary>
        IsamErrorOutOfCursors = 0xFFFFFC0B,

        /// <summary>
        /// The database is out of page buffers. 
        /// </summary>
        IsamErrorOutOfBuffers = 0xFFFFFC0A,

        /// <summary>
        /// There are too many indexes. 
        /// </summary>
        IsamErrorTooManyIndexes = 0xFFFFFC09,

        /// <summary>
        /// There are too many columns in an index. 
        /// </summary>
        IsamErrorTooManyKeys = 0xFFFFFC08,

        /// <summary>
        /// The record has been deleted. 
        /// </summary>
        IsamErrorRecordDeleted = 0xFFFFFC07,

        /// <summary>
        /// There is a checksum error on a database page. 
        /// </summary>
        IsamErrorReadVerifyFailure = 0xFFFFFC06,

        /// <summary>
        /// There is a blank database page. 
        /// </summary>
        IsamErrorPageNotInitialized = 0xFFFFFC05,

        /// <summary>
        /// There are no file handles. 
        /// </summary>
        IsamErrorOutOfFileHandles = 0xFFFFFC04,

        /// <summary>
        /// There is a disk I/O error. 
        /// </summary>
        IsamErrorDiskIO = 0xFFFFFC02,

        /// <summary>
        /// A file path is invalid. 
        /// </summary>
        IsamErrorInvalidPath = 0xFFFFFC01,

        /// <summary>
        /// A system path is invalid. 
        /// </summary>
        IsamErrorInvalidSystemPath = 0xFFFFFC00,

        /// <summary>
        /// A log directory is invalid. 
        /// </summary>
        IsamErrorInvalidLogDirectory = 0xFFFFFBFF,

        /// <summary>
        /// The record is larger than maximum size. 
        /// </summary>
        IsamErrorRecordTooBig = 0xFFFFFBFE,

        /// <summary>
        /// Too many databases are open. 
        /// </summary>
        IsamErrorTooManyOpenDatabases = 0xFFFFFBFD,

        /// <summary>
        /// This is not a database file. 
        /// </summary>
        IsamErrorInvalidDatabase = 0xFFFFFBFC,

        /// <summary>
        /// The database engine has not been initialized. 
        /// </summary>
        IsamErrorNotInitialized = 0xFFFFFBFB,

        /// <summary>
        /// The database engine is already initialized. 
        /// </summary>
        IsamErrorAlreadyInitialized = 0xFFFFFBFA,

        /// <summary>
        /// The database engine is being initialized. 
        /// </summary>
        IsamErrorInitInProgress = 0xFFFFFBF9,

        /// <summary>
        /// The file cannot be accessed because the file is locked or in use. 
        /// </summary>
        IsamErrorFileAccessDenied = 0xFFFFFBF8,

        /// <summary>
        /// The buffer is too small. 
        /// </summary>
        IsamErrorBufferTooSmall = 0xFFFFFBF2,

        /// <summary>
        /// Too many columns are defined. 
        /// </summary>
        IsamErrorTooManyColumns = 0xFFFFFBF0,

        /// <summary>
        /// The container is not empty. 
        /// </summary>
        IsamErrorContainerNotEmpty = 0xFFFFFBED,

        /// <summary>
        /// The file name is invalid. 
        /// </summary>
        IsamErrorInvalidFilename = 0xFFFFFBEC,

        /// <summary>
        /// A bookmark is invalid. 
        /// </summary>
        IsamErrorInvalidBookmark = 0xFFFFFBEB,

        /// <summary>
        /// The column used is in an index. 
        /// </summary>
        IsamErrorColumnInUse = 0xFFFFFBEA,

        /// <summary>
        /// The data buffer does not match the column size. 
        /// </summary>
        IsamErrorInvalidBufferSize = 0xFFFFFBE9,

        /// <summary>
        /// The column value cannot be set. 
        /// </summary>
        IsamErrorColumnNotUpdatable = 0xFFFFFBE8,

        /// <summary>
        /// The index is in use. 
        /// </summary>
        IsamErrorIndexInUse = 0xFFFFFBE5,

        /// <summary>
        /// The link support is unavailable. 
        /// </summary>
        IsamErrorLinkNotSupported = 0xFFFFFBE4,

        /// <summary>
        /// Null keys are not allowed on an index. 
        /// </summary>
        IsamErrorNullKeyDisallowed = 0xFFFFFBE3,

        /// <summary>
        /// The operation has to occur within a transaction. 
        /// </summary>
        IsamErrorNotInTransaction = 0xFFFFFBE2,

        /// <summary>
        /// There are too many active database users. 
        /// </summary>
        IsamErrorTooManyActiveUsers = 0xFFFFFBDD,

        /// <summary>
        /// A country/region code is invalid or unknown. 
        /// </summary>
        IsamErrorInvalidCountry = 0xFFFFFBDB,

        /// <summary>
        /// A language ID is invalid or unknown. 
        /// </summary>
        IsamErrorInvalidLanguageId = 0xFFFFFBDA,

        /// <summary>
        /// A code page is invalid or unknown. 
        /// </summary>
        IsamErrorInvalidCodePage = 0xFFFFFBD9,

        /// <summary>
        /// Invalid flags are being used for LCMapString. 
        /// </summary>
        IsamErrorInvalidLCMapStringFlags = 0xFFFFFBD8,

        /// <summary>
        /// There was an attempt to create a version store entry (RCE) that was larger than a version bucket. (JET_errVersionStoreEntryTooBig)
        /// </summary>
        IsamErrorVersionStoreEntryTooBig = 0xFFFFFBD7,

        /// <summary>
        /// The version store is out of memory and the cleanup attempt failed to complete. (JET_errVersionStoreOutOfMemoryAndCleanupTimedOut)
        /// </summary>
        IsamErrorVersionStoreOutOfMemoryAndCleanupTimedOut = 0xFFFFFBD6,

        /// <summary>
        /// The version store is out of memory and a cleanup was already attempted. (JET_errVersionStoreOutOfMemory)
        /// </summary>
        IsamErrorVersionStoreOutOfMemory = 0xFFFFFBD3,

        /// <summary>
        /// The escrow and SLV columns cannot be indexed. (JET_errCannotIndex)
        /// </summary>
        IsamErrorCannotIndex = 0xFFFFFBD1,

        /// <summary>
        /// The record has not been deleted. 
        /// </summary>
        IsamErrorRecordNotDeleted = 0xFFFFFBD0,

        /// <summary>
        /// Too many mempool entries have been requested. 
        /// </summary>
        IsamErrorTooManyMempoolEntries = 0xFFFFFBCF,

        /// <summary>
        /// The database is out of B+ tree ObjectIDs so an offline defragmentation has to be performed to reclaim freed or unused ObjectIDs. 
        /// </summary>
        IsamErrorOutOfObjectIDs = 0xFFFFFBCE,

        /// <summary>
        /// The Long-value ID counter has reached the maximum value. An offline defragmentation has to be performed to reclaim free or unused LongValueIDs. 
        /// </summary>
        IsamErrorOutOfLongValueIDs = 0xFFFFFBCD,

        /// <summary>
        /// The automatic increment counter has reached the maximum value. An offline defragmentation will not be able to reclaim free or unused automatically increment values. 
        /// </summary>
        IsamErrorOutOfAutoincrementValues = 0xFFFFFBCC,

        /// <summary>
        /// The Dbtime counter has reached the maximum value. An offline defragmentation is required to be performed to reclaim free or unused Dbtime values. 
        /// </summary>
        IsamErrorOutOfDbtimeValues = 0xFFFFFBCB,

        /// <summary>
        /// A sequential index counter has reached the maximum value. An offline defragmentation has to  be performed to reclaim Free or unused SequentialIndex values. 
        /// </summary>
        IsamErrorOutOfSequentialIndexValues = 0xFFFFFBCA,

        /// <summary>
        /// This multi-instance call has the single-instance mode enabled. 
        /// </summary>
        IsamErrorRunningInOneInstanceMode = 0xFFFFFBC8,

        /// <summary>
        /// This single-instance call has the multi-instance mode enabled. 
        /// </summary>
        IsamErrorRunningInMultiInstanceMode = 0xFFFFFBC7,

        /// <summary>
        /// The global system parameters have already been set. 
        /// </summary>
        IsamErrorSystemParamsAlreadySet = 0xFFFFFBC6,

        /// <summary>
        /// The system path is already being used by another database instance. 
        /// </summary>
        IsamErrorSystemPathInUse = 0xFFFFFBC5,

        /// <summary>
        /// The log file path is already being used by another database instance. 
        /// </summary>
        IsamErrorLogFilePathInUse = 0xFFFFFBC4,

        /// <summary>
        /// The path to the temporary database is already being used by another database instance. 
        /// </summary>
        IsamErrorTempPathInUse = 0xFFFFFBC3,

        /// <summary>
        /// The instance name is already in use. 
        /// </summary>
        IsamErrorInstanceNameInUse = 0xFFFFFBC2,

        /// <summary>
        /// This instance cannot be used because it encountered a fatal error. 
        /// </summary>
        IsamErrorInstanceUnavailable = 0xFFFFFBBE,

        /// <summary>
        /// This database cannot be used because it encountered a fatal error. 
        /// </summary>
        IsamErrorDatabaseUnavailable = 0xFFFFFBBD,

        /// <summary>
        /// This instance cannot be used because it encountered a log-disk-full error while performing an operation (such as a transaction rollback) that could not tolerate failure. 
        /// </summary>
        IsamErrorInstanceUnavailableDueToFatalLogDiskFull = 0xFFFFFBBC,

        /// <summary>
        /// The database is out of sessions. (JET_errOutOfSessions)
        /// </summary>
        IsamErrorOutOfSessions = 0xFFFFFBB3,

        /// <summary>
        /// The write lock failed due to the existence of an outstanding write lock. 
        /// </summary>
        IsamErrorWriteConflict = 0xFFFFFBB2,

        /// <summary>
        /// The transactions are nested too deeply. 
        /// </summary>
        IsamErrorTransTooDeep = 0xFFFFFBB1,

        /// <summary>
        /// A session handle is invalid. 
        /// </summary>
        IsamErrorInvalidSesid = 0xFFFFFBB0,

        /// <summary>
        /// An update was attempted on an uncommitted primary index. 
        /// </summary>
        IsamErrorWriteConflictPrimaryIndex = 0xFFFFFBAF,

        /// <summary>
        /// The operation is not allowed within a transaction. 
        /// </summary>
        IsamErrorInTransaction = 0xFFFFFBAC,

        /// <summary>
        /// The current transaction is required to be rolled back. It cannot be committed and a new one cannot be started. 
        /// </summary>
        IsamErrorRollbackRequired = 0xFFFFFBAB,

        /// <summary>
        /// A read-only transaction tried to modify the database. 
        /// </summary>
        IsamErrorTransReadOnly = 0xFFFFFBAA,

        /// <summary>
        /// Two different cursors attempted to replace the same record in the same session. 
        /// </summary>
        IsamErrorSessionWriteConflict = 0xFFFFFBA9,

        /// <summary>
        /// The record would be too big if represented in a database format from a previous version of Jet. 
        /// </summary>
        IsamErrorRecordTooBigForBackwardCompatibility = 0xFFFFFBA8,

        /// <summary>
        /// The temporary table could not be created due to parameters that conflict with JET_bitTTForwardOnly. 
        /// </summary>
        IsamErrorCannotMaterializeForwardOnlySort = 0xFFFFFBA7,

        /// <summary>
        /// The session handle cannot be used with the table id because it was not used to create it. 
        /// </summary>
        IsamErrorSesidTableIdMismatch = 0xFFFFFBA6,

        /// <summary>
        /// The instance handle is invalid or refers to an instance that has been shut down. 
        /// </summary>
        IsamErrorInvalidInstance = 0xFFFFFBA5,

        /// <summary>
        /// The database already exists. 
        /// </summary>
        IsamErrorDatabaseDuplicate = 0xFFFFFB4F,

        /// <summary>
        /// The database in use. 
        /// </summary>
        IsamErrorDatabaseInUse = 0xFFFFFB4E,

        /// <summary>
        /// No such database exists. 
        /// </summary>
        IsamErrorDatabaseNotFound = 0xFFFFFB4D,

        /// <summary>
        /// The database name is invalid. 
        /// </summary>
        IsamErrorDatabaseInvalidName = 0xFFFFFB4C,

        /// <summary>
        /// The number of pages is invalid. 
        /// </summary>
        IsamErrorDatabaseInvalidPages = 0xFFFFFB4B,

        /// <summary>
        /// There is a nondatabase file or corrupt database. 
        /// </summary>
        IsamErrorDatabaseCorrupted = 0xFFFFFB4A,

        /// <summary>
        /// The database is exclusively locked. 
        /// </summary>
        IsamErrorDatabaseLocked = 0xFFFFFB49,

        /// <summary>
        /// The versioning for this database cannot be disabled. 
        /// </summary>
        IsamErrorCannotDisableVersioning = 0xFFFFFB48,

        /// <summary>
        /// The database engine is incompatible with the database. 
        /// </summary>
        IsamErrorInvalidDatabaseVersion = 0xFFFFFB47,

        /// <summary>
        /// The database is in an older (200) format.  
        /// </summary>
        IsamErrorDatabase200Format = 0xFFFFFB46,

        /// <summary>
        /// The database is in an older (400) format. 
        /// </summary>
        IsamErrorDatabase400Format = 0xFFFFFB45,

        /// <summary>
        /// The database is in an older (500) format. 
        /// </summary>
        IsamErrorDatabase500Format = 0xFFFFFB44,

        /// <summary>
        /// The database page size does not match the engine. 
        /// </summary>
        IsamErrorPageSizeMismatch = 0xFFFFFB43,

        /// <summary>
        /// No more database instances can be started. 
        /// </summary>
        IsamErrorTooManyInstances = 0xFFFFFB42,

        /// <summary>
        /// A different database instance is using this database. 
        /// </summary>
        IsamErrorDatabaseSharingViolation = 0xFFFFFB41,

        /// <summary>
        /// An outstanding database attachment has been detected at the start or end of the recovery, but the database is missing or does not match attachment info. 
        /// </summary>
        IsamErrorAttachedDatabaseMismatch = 0xFFFFFB40,

        /// <summary>
        /// The specified path to the database file is illegal. 
        /// </summary>
        IsamErrorDatabaseInvalidPath = 0xFFFFFB3F,

        /// <summary>
        /// A database is being assigned an ID that is already in use. 
        /// </summary>
        IsamErrorDatabaseIdInUse = 0xFFFFFB3E,

        /// <summary>
        /// The forced detach is allowed only after the normal detach was stopped due to an error. 
        /// </summary>
        IsamErrorForceDetachNotAllowed = 0xFFFFFB3D,

        /// <summary>
        /// Corruption was detected in the catalog. 
        /// </summary>
        IsamErrorCatalogCorrupted = 0xFFFFFB3C,

        /// <summary>
        /// The database is only partially attached and the attach operation cannot be completed. 
        /// </summary>
        IsamErrorPartiallyAttachedDB = 0xFFFFFB3B,

        /// <summary>
        /// The database with the same signature is already in use. 
        /// </summary>
        IsamErrorDatabaseSignInUse = 0xFFFFFB3A,

        /// <summary>
        /// The database is corrupted but a repair is not allowed. 
        /// </summary>
        IsamErrorDatabaseCorruptedNoRepair = 0xFFFFFB38,

        /// <summary>
        /// The database engine attempted to replay a Create Database operation from the transaction log but failed due to an incompatible version of that operation. 
        /// </summary>
        IsamErrorInvalidCreateDbVersion = 0xFFFFFB37,

        /// <summary>
        /// The table is exclusively locked. 
        /// </summary>
        IsamErrorTableLocked = 0xFFFFFAEA,

        /// <summary>
        /// The table already exists. 
        /// </summary>
        IsamErrorTableDuplicate = 0xFFFFFAE9,

        /// <summary>
        /// The table is in use and cannot be locked. 
        /// </summary>
        IsamErrorTableInUse = 0xFFFFFAE8,

        /// <summary>
        /// There is no such table or object. 
        /// </summary>
        IsamErrorObjectNotFound = 0xFFFFFAE7,

        /// <summary>
        /// There is a bad file or index density. 
        /// </summary>
        IsamErrorDensityInvalid = 0xFFFFFAE5,

        /// <summary>
        /// The table is not empty. 
        /// </summary>
        IsamErrorTableNotEmpty = 0xFFFFFAE4,

        /// <summary>
        /// The table ID is invalid. 
        /// </summary>
        IsamErrorInvalidTableId = 0xFFFFFAE2,

        /// <summary>
        /// No more tables can be opened, even after the internal cleanup task has run. 
        /// </summary>
        IsamErrorTooManyOpenTables = 0xFFFFFAE1,

        /// <summary>
        /// The operation is not supported on the table. 
        /// </summary>
        IsamErrorIllegalOperation = 0xFFFFFAE0,

        /// <summary>
        /// No more tables can be opened because the cleanup attempt failed to complete. 
        /// </summary>
        IsamErrorTooManyOpenTablesAndCleanupTimedOut = 0xFFFFFADF,

        /// <summary>
        /// The table or object name is in use. 
        /// </summary>
        IsamErrorObjectDuplicate = 0xFFFFFADE,

        /// <summary>
        /// The object is invalid for operation. 
        /// </summary>
        IsamErrorInvalidObject = 0xFFFFFADC,

        /// <summary>
        /// JetCloseTable is required to be used instead of JetDeleteTable to delete a temporary table. 
        /// </summary>
        IsamErrorCannotDeleteTempTable = 0xFFFFFADB,

        /// <summary>
        /// There was an illegal attempt to delete a system table. 
        /// </summary>
        IsamErrorCannotDeleteSystemTable = 0xFFFFFADA,

        /// <summary>
        /// There was an illegal attempt to delete a template table. 
        /// </summary>
        IsamErrorCannotDeleteTemplateTable = 0xFFFFFAD9,

        /// <summary>
        /// There has to be an exclusive lock on the table. 
        /// </summary>
        IsamErrorExclusiveTableLockRequired = 0xFFFFFAD6,

        /// <summary>
        /// DDL operations are prohibited on this table. 
        /// </summary>
        IsamErrorFixedDDL = 0xFFFFFAD5,

        /// <summary>
        /// On a derived table, DDL operations are prohibited on the inherited portion of the DDL. 
        /// </summary>
        IsamErrorFixedInheritedDDL = 0xFFFFFAD4,

        /// <summary>
        /// Nesting the hierarchical DDL is not currently supported.
        /// </summary>
        IsamErrorCannotNestDDL = 0xFFFFFAD3,

        /// <summary>
        /// There was an attempt to inherit a DDL from a table that is not marked as a template table. 
        /// </summary>
        IsamErrorDDLNotInheritable = 0xFFFFFAD2,

        /// <summary>
        /// The system parameters were set improperly. 
        /// </summary>
        IsamErrorInvalidSettings = 0xFFFFFAD0,

        /// <summary>
        /// The client has requested that the service be stopped. 
        /// </summary>
        IsamErrorClientRequestToStopJetService = 0xFFFFFACF,

        /// <summary>
        /// The template table was created with the NoFixedVarColumnsInDerivedTables flag set. 
        /// </summary>
        IsamErrorCannotAddFixedVarColumnToDerivedTable = 0xFFFFFACE,

        /// <summary>
        /// The index build failed. 
        /// </summary>
        IsamErrorIndexCantBuild = 0xFFFFFA87,

        /// <summary>
        /// The primary index is already defined. 
        /// </summary>
        IsamErrorIndexHasPrimary = 0xFFFFFA86,

        /// <summary>
        /// The index is already defined. 
        /// </summary>
        IsamErrorIndexDuplicate = 0xFFFFFA85,

        /// <summary>
        /// There is no such index. 
        /// </summary>
        IsamErrorIndexNotFound = 0xFFFFFA84,

        /// <summary>
        /// The clustered index cannot be deleted. 
        /// </summary>
        IsamErrorIndexMustStay = 0xFFFFFA83,

        /// <summary>
        /// The index definition is invalid. 
        /// </summary>
        IsamErrorIndexInvalidDef = 0xFFFFFA82,

        /// <summary>
        /// The creation of the index description was invalid. 
        /// </summary>
        IsamErrorInvalidCreateIndex = 0xFFFFFA7F,

        /// <summary>
        /// The database is out of index description blocks. 
        /// </summary>
        IsamErrorTooManyOpenIndexes = 0xFFFFFA7E,

        /// <summary>
        /// Non-unique inter-record index keys have been generated for a multivalued index. 
        /// </summary>
        IsamErrorMultiValuedIndexViolation = 0xFFFFFA7D,

        /// <summary>
        /// A secondary index that properly reflects the primary index failed to build. 
        /// </summary>
        IsamErrorIndexBuildCorrupted = 0xFFFFFA7C,

        /// <summary>
        /// The primary index is corrupt and the database is required be defragmented. 
        /// </summary>
        IsamErrorPrimaryIndexCorrupted = 0xFFFFFA7B,

        /// <summary>
        /// The secondary index is corrupt and the database is required to be defragmented. 
        /// </summary>
        IsamErrorSecondaryIndexCorrupted = 0xFFFFFA7A,

        /// <summary>
        /// The index ID is invalid. 
        /// </summary>
        IsamErrorInvalidIndexId = 0xFFFFFA78,

        /// <summary>
        /// The tuple index can only be set on a secondary index. 
        /// </summary>
        IsamErrorIndexTuplesSecondaryIndexOnly = 0xFFFFFA6A,

        /// <summary>
        /// The index definition for the tuple index contains more key columns that the database engine can support. 
        /// </summary>
        IsamErrorIndexTuplesTooManyColumns = 0xFFFFFA69,

        /// <summary>
        /// The tuple index cannot be a unique index. 
        /// </summary>
        IsamErrorIndexTuplesNonUniqueOnly = 0xFFFFFA68,

        /// <summary>
        /// A tuple index definition can only contain key columns that have text or binary column types. 
        /// </summary>
        IsamErrorIndexTuplesTextBinaryColumnsOnly = 0xFFFFFA67,

        /// <summary>
        /// The tuple index does not allow setting cbVarSegMac. 
        /// </summary>
        IsamErrorIndexTuplesVarSegMacNotAllowed = 0xFFFFFA66,

        /// <summary>
        /// The minimum/maximum tuple length or the maximum number of characters that are specified for an index is invalid. 
        /// </summary>
        IsamErrorIndexTuplesInvalidLimits = 0xFFFFFA65,

        /// <summary>
        /// JetRetrieveColumn cannot be called with the JET_bitRetrieveFromIndex flag set while retrieving a column on a tuple index. 
        /// </summary>
        IsamErrorIndexTuplesCannotRetrieveFromIndex = 0xFFFFFA64,

        /// <summary>
        /// The specified key does not meet the minimum tuple length. 
        /// </summary>
        IsamErrorIndexTuplesKeyTooSmall = 0xFFFFFA63,

        /// <summary>
        /// The column value is long. 
        /// </summary>
        IsamErrorColumnLong = 0xFFFFFA23,

        /// <summary>
        /// There is no such chunk in a long value. 
        /// </summary>
        IsamErrorColumnNoChunk = 0xFFFFFA22,

        /// <summary>
        /// The field will not fit in the record. 
        /// </summary>
        IsamErrorColumnDoesNotFit = 0xFFFFFA21,

        /// <summary>
        /// Null is not valid. 
        /// </summary>
        IsamErrorNullInvalid = 0xFFFFFA20,

        /// <summary>
        /// The column is indexed and cannot be deleted. 
        /// </summary>
        IsamErrorColumnIndexed = 0xFFFFFA1F,

        /// <summary>
        /// The field length is greater than the maximum allowed length. 
        /// </summary>
        IsamErrorColumnTooBig = 0xFFFFFA1E,

        /// <summary>
        /// No such column exists. 
        /// </summary>
        IsamErrorColumnNotFound = 0xFFFFFA1D,

        /// <summary>
        /// This field is already defined. 
        /// </summary>
        IsamErrorColumnDuplicate = 0xFFFFFA1C,

        /// <summary>
        /// An attempt was made to create a multivalued column, but the column was not tagged. 
        /// </summary>
        IsamErrorMultiValuedColumnMustBeTagged = 0xFFFFFA1B,

        /// <summary>
        /// There is a second automatic increment or version column. 
        /// </summary>
        IsamErrorColumnRedundant = 0xFFFFFA1A,

        /// <summary>
        /// The column data type is invalid. 
        /// </summary>
        IsamErrorInvalidColumnType = 0xFFFFFA19,

        /// <summary>
        /// There are no non-NULL tagged columns. 
        /// </summary>
        IsamErrorTaggedNotNULL = 0xFFFFFA16,

        /// <summary>
        /// The database is invalid because it does not contain a current index. 
        /// </summary>
        IsamErrorNoCurrentIndex = 0xFFFFFA15,

        /// <summary>
        /// The key is completely made. 
        /// </summary>
        IsamErrorKeyIsMade = 0xFFFFFA14,

        /// <summary>
        /// The column ID is incorrect. 
        /// </summary>
        IsamErrorBadColumnId = 0xFFFFFA13,

        /// <summary>
        /// There is a bad itagSequence for the tagged column. 
        /// </summary>
        IsamErrorBadItagSequence = 0xFFFFFA12,

        /// <summary>
        /// A column cannot be deleted because it is part of a relationship. 
        /// </summary>
        IsamErrorColumnInRelationship = 0xFFFFFA11,

        /// <summary>
        /// The automatic increment and version cannot be tagged. 
        /// </summary>
        IsamErrorCannotBeTagged = 0xFFFFFA0F,

        /// <summary>
        /// The default value exceeds the maximum size. 
        /// </summary>
        IsamErrorDefaultValueTooBig = 0xFFFFFA0C,

        /// <summary>
        /// A duplicate value was detected on a unique multivalued column. 
        /// </summary>
        IsamErrorMultiValuedDuplicate = 0xFFFFFA0B,

        /// <summary>
        /// Corruption was encountered in a long-value tree. 
        /// </summary>
        IsamErrorLVCorrupted = 0xFFFFFA0A,

        /// <summary>
        /// A duplicate value was detected on a unique multivalued column after the data was normalized, and it is normalizing truncated data before comparison. 
        /// </summary>
        IsamErrorMultiValuedDuplicateAfterTruncation = 0xFFFFFA08,

        /// <summary>
        /// There is an invalid column in a derived table. 
        /// </summary>
        IsamErrorDerivedColumnCorruption = 0xFFFFFA07,

        /// <summary>
        /// An attempt was made to convert a column to a primary index placeholder, but the column does not meet the necessary criteria. 
        /// </summary>
        IsamErrorInvalidPlaceholderColumn = 0xFFFFFA06,

        /// <summary>
        /// The key was not found. 
        /// </summary>
        IsamErrorRecordNotFound = 0xFFFFF9BF,

        /// <summary>
        /// There is no working buffer. 
        /// </summary>
        IsamErrorRecordNoCopy = 0xFFFFF9BE,

        /// <summary>
        /// There is no current record. 
        /// </summary>
        IsamErrorNoCurrentRecord = 0xFFFFF9BD,

        /// <summary>
        /// The primary key might not change. 
        /// </summary>
        IsamErrorRecordPrimaryChanged = 0xFFFFF9BC,

        /// <summary>
        /// There is an illegal duplicate key. 
        /// </summary>
        IsamErrorKeyDuplicate = 0xFFFFF9BB,

        /// <summary>
        /// An attempt was made to update a record while a record update was already in progress. 
        /// </summary>
        IsamErrorAlreadyPrepared = 0xFFFFF9B9,

        /// <summary>
        /// A call was not made to JetMakeKey. 
        /// </summary>
        IsamErrorKeyNotMade = 0xFFFFF9B8,

        /// <summary>
        /// A call was not made to JetPrepareUpdate. 
        /// </summary>
        IsamErrorUpdateNotPrepared = 0xFFFFF9B7,

        /// <summary>
        /// The data has changed and the operation was aborted. 
        /// </summary>
        IsamErrorDataHasChanged = 0xFFFFF9B5,

        /// <summary>
        /// The operating system does not support the selected language. 
        /// </summary>
        IsamErrorLanguageNotSupported = 0xFFFFF9AD,

        /// <summary>
        /// There are too many sort processes. 
        /// </summary>
        IsamErrorTooManySorts = 0xFFFFF95B,

        /// <summary>
        /// An invalid operation occurred during a sort. 
        /// </summary>
        IsamErrorInvalidOnSort = 0xFFFFF95A,

        /// <summary>
        /// The temporary file could not be opened. 
        /// </summary>
        IsamErrorTempFileOpenError = 0xFFFFF8F5,

        /// <summary>
        /// Too many databases are open. 
        /// </summary>
        IsamErrorTooManyAttachedDatabases = 0xFFFFF8F3,

        /// <summary>
        /// There is no space left on disk. 
        /// </summary>
        IsamErrorDiskFull = 0xFFFFF8F0,

        /// <summary>
        /// Permission is denied. 
        /// </summary>
        IsamErrorPermissionDenied = 0xFFFFF8EF,

        /// <summary>
        /// The file was not found. 
        /// </summary>
        IsamErrorFileNotFound = 0xFFFFF8ED,

        /// <summary>
        /// The file type is invalid. 
        /// </summary>
        IsamErrorFileInvalidType = 0xFFFFF8EC,

        /// <summary>
        /// A restore cannot be started after initialization. 
        /// </summary>
        IsamErrorAfterInitialization = 0xFFFFF8C6,

        /// <summary>
        /// The logs could not be interpreted. 
        /// </summary>
        IsamErrorLogCorrupted = 0xFFFFF8C4,

        /// <summary>
        /// The operation is invalid. 
        /// </summary>
        IsamErrorInvalidOperation = 0xFFFFF88E,

        /// <summary>
        /// Access is denied. 
        /// </summary>
        IsamErrorAccessDenied = 0xFFFFF88D,

        /// <summary>
        /// An infinite split. 
        /// </summary>
        IsamErrorTooManySplits = 0xFFFFF88B,

        /// <summary>
        /// Multiple threads are using the same session. 
        /// </summary>
        IsamErrorSessionSharingViolation = 0xFFFFF88A,

        /// <summary>
        /// An entry point in a required DLL could not be found. 
        /// </summary>
        IsamErrorEntryPointNotFound = 0xFFFFF889,

        /// <summary>
        /// The specified session already has a session context set. 
        /// </summary>
        IsamErrorSessionContextAlreadySet = 0xFFFFF888,

        /// <summary>
        /// An attempt was made to reset the session context, but the current thread was not the original one that set the session context. 
        /// </summary>
        IsamErrorSessionContextNotSetByThisThread = 0xFFFFF887,

        /// <summary>
        /// An attempt was made to terminate the session currently in use. 
        /// </summary>
        IsamErrorSessionInUse = 0xFFFFF886,

        /// <summary>
        /// An internal error occurred during a dynamic record format conversion. 
        /// </summary>
        IsamErrorRecordFormatConversionFailed = 0xFFFFF885,

        /// <summary>
        /// Only one open user database per session is allowed. 
        /// </summary>
        IsamErrorOneDatabasePerSession = 0xFFFFF884,

        /// <summary>
        /// There was an error during rollback. 
        /// </summary>
        IsamErrorRollbackError = 0xFFFFF883,

        /// <summary>
        /// A callback function call failed. 
        /// </summary>
        IsamErrorCallbackFailed = 0xFFFFF7CB,

        /// <summary>
        /// A callback function could not be found. 
        /// </summary>
        IsamErrorCallbackNotResolved = 0xFFFFF7CA,

        /// <summary>
        /// The operating system shadow copy API was used in an invalid sequence. 
        /// </summary>
        IsamErrorOSSnapshotInvalidSequence = 0xFFFFF69F,

        /// <summary>
        /// The operating system shadow copy ended with a time-out. 
        /// </summary>
        IsamErrorOSSnapshotTimeOut = 0xFFFFF69E,

        /// <summary>
        /// The operating system shadow copy is not allowed because a backup or recovery in is progress. 
        /// </summary>
        IsamErrorOSSnapshotNotAllowed = 0xFFFFF69D,

        /// <summary>
        /// The operation failed because the specified operating system shadow copy handle was invalid. 
        /// </summary>
        IsamErrorOSSnapshotInvalidSnapId = 0xFFFFF69C,

        /// <summary>
        /// An attempt was made to use local storage without a callback function being specified. 
        /// </summary>
        IsamErrorLSCallbackNotSpecified = 0xFFFFF448,

        /// <summary>
        /// An attempt was made to set the local storage for an object that already had it set. 
        /// </summary>
        IsamErrorLSAlreadySet = 0xFFFFF447,

        /// <summary>
        /// An attempt was made to retrieve local storage from an object that did not have it set. 
        /// </summary>
        IsamErrorLSNotSet = 0xFFFFF446,

        /// <summary>
        /// An I/O operation failed because it was attempted against an unallocated region of a file. 
        /// </summary>
        IsamErrorFileIOSparse = 0xFFFFF060,

        /// <summary>
        /// A read was issued to a location beyond the EOF (writes will expand the file). 
        /// </summary>
        IsamErrorFileIOBeyondEOF = 0xFFFFF05F,

        /// <summary>
        /// Read/write access is not supported on compressed files. 
        /// </summary>
        IsamErrorFileCompressed = 0xFFFFF05B
    }

    /// <summary>
    /// 2.4.2 Property Error Codes
    /// </summary>
    public enum PropertyErrorCodes : uint
    {
        /// <summary>
        /// On get, indicates that the property or column value is too large to be retrieved by the request, and the property value needs to be accessed with the RopOpenStream ROP ([MS-OXCROPS] section 2.2.9.1).
        /// </summary>
        NotEnoughMemory = 0x8007000E,

        /// <summary>
        /// On get, indicates that the property or column has no value for this object.
        /// </summary>
        NotFound = 0x8004010F,

        /// <summary>
        /// On set, indicates that the property value is not acceptable to the server.
        /// </summary>
        BadValue = 0x80040301,

        /// <summary>
        /// On get or set, indicates that the data type passed with the property or column is undefined.
        /// </summary>
        InvalidType = 0x80040302,

        /// <summary>
        /// On get or set, indicates that the data type passed with the property or column is not acceptable to the server.
        /// </summary>
        UnsupportedType = 0x80040303,

        /// <summary>
        /// On get or set, indicates that the data type passed with the property or column is not the type expected by the server.
        /// </summary>
        UnexpectedType = 0x80040304,

        /// <summary>
        /// Indicates that the result set of the operation is too big for the server to return.
        /// </summary>
        TooBig = 0x80040305,

        /// <summary>
        /// On a copy operation, indicates that the server cannot copy the object, possibly because the source and destination are on different types of servers, and the server will delegate the copying to client code.
        /// </summary>
        DeclineCopy = 0x80040306,

        /// <summary>
        /// On get or set, indicates that the server does not support property IDs in this range, usually the named property ID range (from 0x8000 through 0xFFFF).
        /// </summary>
        UnexpectedId = 0x80040307
    }

    /// <summary>
    /// 2.4.3 Warning Codes
    /// </summary>
    public enum WarningCodes : uint
    {
        /// <summary>
        /// A request involving multiple properties failed for one or more individual properties, while succeeding overall.
        /// </summary>
        ErrorsReturned = 0x00040380,

        /// <summary>
        /// A table operation succeeded, but the bookmark specified is no longer set at the same row as when it was last used.
        /// </summary>
        PositionChanged = 0x00040481,

        /// <summary>
        /// The row count returned by a table operation is approximate, not exact.
        /// </summary>
        ApproximateCount = 0x00040482,

        /// <summary>
        /// A move, copy, or delete operation succeeded for some messages but not for others.
        /// </summary>
        PartiallyComplete = 0x00040680,

        /// <summary>
        /// The operation succeeded, but there is more to do.
        /// </summary>
        SyncProgress = 0x00040820,

        /// <summary>
        /// In a change conflict, the client has the more recent change.
        /// </summary>
        NewerClientChange = 0x00040821,

        /// <summary>
        /// The version store is still active. 
        /// </summary>
        IsamWarningRemainingVersions = 0x00000141,

        /// <summary>
        /// A seek on an index that is not unique yielded a unique key. 
        /// </summary>
        IsamWarningUniqueKey = 0x00000159,

        /// <summary>
        /// A database column is a separated long value. 
        /// </summary>
        IsamWarningSeparateLongValue = 0x00000196,

        /// <summary>
        /// The existing log file has a bad signature. 
        /// </summary>
        IsamWarningExistingLogFileHasBadSignature = 0x0000022E,

        /// <summary>
        /// The existing log file is not contiguous. 
        /// </summary>
        IsamWarningExistingLogFileIsNotContiguous = 0x0000022F,

        /// <summary>
        /// This error is for internal use only. 
        /// </summary>
        IsamWarningSkipThisRecord = 0x00000234,

        /// <summary>
        /// The TargetInstance specified for the restore is running. 
        /// </summary>
        IsamWarningTargetInstanceRunning = 0x00000242,

        /// <summary>
        /// The database corruption has been repaired. 
        /// </summary>
        IsamWarningDatabaseRepaired = 0x00000253,

        /// <summary>
        /// The column has a null value. 
        /// </summary>
        IsamWarningColumnNull = 0x000003EC,

        /// <summary>
        /// The buffer is too small for the data. 
        /// </summary>
        IsamWarningBufferTruncated = 0x000003EE,

        /// <summary>
        /// The database is already attached. 
        /// </summary>
        IsamWarningDatabaseAttached = 0x000003EF,

        /// <summary>
        /// The sort that is being attempted does not have enough memory to complete. 
        /// </summary>
        IsamWarningSortOverflow = 0x000003F1,

        /// <summary>
        /// An exact match was not found during a seek. 
        /// </summary>
        IsamWarningSeekNotEqual = 0x0000040F,

        /// <summary>
        /// There is no extended error information. 
        /// </summary>
        IsamWarningNoErrorInfo = 0x0000041F,

        /// <summary>
        /// No idle activity occurred. 
        /// </summary>
        IsamWarningNoIdleActivity = 0x00000422,

        /// <summary>
        /// There is a no write lock at transaction level 0. 
        /// </summary>
        IsamWarningNoWriteLock = 0x0000042B,

        /// <summary>
        /// The column is set to a null value. 
        /// </summary>
        IsamWarningColumnSetNull = 0x0000042C,

        /// <summary>
        /// An empty table was opened. 
        /// </summary>
        IsamWarningTableEmpty = 0x00000515,

        /// <summary>
        /// The system cleanup has a cursor open on the table. 
        /// </summary>
        IsamWarningTableInUseBySystem = 0x0000052F,

        /// <summary>
        /// The out-of-date index is required to be removed. 
        /// </summary>
        IsamWarningCorruptIndexDeleted = 0x00000587,

        /// <summary>
        /// The maximum length is too large and has been truncated. 
        /// </summary>
        IsamWarningColumnMaxTruncated = 0x000005E8,

        /// <summary>
        /// A binary large object (BLOB) value has been moved from the record into a separate storage of BLOBs. 
        /// </summary>
        IsamWarningCopyLongValue = 0x000005F0,

        /// <summary>
        /// The column values were not returned because the corresponding column ID or itagSequence member from the JET_ENUMCOLUMNVALUE structure that was requested for enumeration was null. 
        /// </summary>
        IsamWarningColumnSkipped = 0x000005FB,

        /// <summary>
        /// The column values were not returned because they could not be reconstructed from the existing data. 
        /// </summary>
        IsamWarningColumnNotLocal = 0x000005FC,

        /// <summary>
        /// The existing column values were not requested for enumeration. 
        /// </summary>
        IsamWarningColumnMoreTags = 0x000005FD,

        /// <summary>
        /// The column value was truncated at the requested size limit during enumeration. 
        /// </summary>
        IsamWarningColumnTruncated = 0x000005FE,

        /// <summary>
        /// The column values exist but were not returned by the request. 
        /// </summary>
        IsamWarningColumnPresent = 0x000005FF,

        /// <summary>
        /// The column value was returned in JET_COLUMNENUM as a result of the JET_bitEnumerateCompressOutput being set. 
        /// </summary>
        IsamWarningColumnSingleValue = 0x00000600,

        /// <summary>
        /// The column value is set to the default value of the column. 
        /// </summary>
        IsamWarningColumnDefault = 0x00000601,

        /// <summary>
        /// The data has changed. 
        /// </summary>
        IsamWarningDataHasChanged = 0x0000064A,

        /// <summary>
        /// A new key is being used. 
        /// </summary>
        IsamWarningKeyChanged = 0x00000652,

        /// <summary>
        /// The database file is read-only. 
        /// </summary>
        IsamWarningFileOpenReadOnly = 0x00000715,

        /// <summary>
        /// The idle registry is full. 
        /// </summary>
        IsamWarningIdleFull = 0x00000774,

        /// <summary>
        /// An online defragmentation was already running on the specified database. 
        /// </summary>
        IsamWarningDefragAlreadyRunning = 0x000007D0,

        /// <summary>
        /// An online defragmentation is not running on the specified database. 
        /// </summary>
        IsamWarningDefragNotRunning = 0x000007D1,

        /// <summary>
        /// A nonexistent callback function was unregistered. 
        /// </summary>
        IsamWarningCallbackNotRegistered = 0x00000834,

        /// <summary>
        /// The function is not yet implemented. 
        /// </summary>
        IsamWarningNotYetImplemented = 0xFFFFFFFF,

        /// <summary>
        /// Warning code returned by the NSPI server to indicate that the unbind call was successful.
        /// </summary>
        UnbindSuccess = 0x000000001,

        /// <summary>
        /// Warning code returned by the NSPI server to indicate that the NSPI bind call failed.
        /// </summary>
        UnbindFailure = 0x00000002,
    }
    #endregion

    /// <summary>
    /// The enum of Kind.
    /// </summary>
    public enum KindEnum : byte
    {
        /// <summary>
        /// The property is identified by the LID field. 
        /// </summary>
        LID = 0x00,

        /// <summary>
        /// The property is identified by the Name field.
        /// </summary>
        Name = 0x01,

        /// <summary>
        /// The property does not have an associated PropertyName field.
        /// </summary>
        NoPropertyName = 0xFF
    }

    /// <summary>
    /// The enumeration specifies the type of address. 
    /// </summary>
    public enum AddressTypeEnum : int
    {
        /// <summary>
        /// There is no type 
        /// </summary>
        NoType = 0x0,

        /// <summary>
        /// X500DN type
        /// </summary>
        X500DN = 0x1,

        /// <summary>
        /// MsMail type
        /// </summary>
        MsMail = 0x2,

        /// <summary>
        /// SMTP type
        /// </summary>
        SMTP = 0x3,

        /// <summary>
        /// Fax type
        /// </summary>
        Fax = 0x4,

        /// <summary>
        /// ProfessionalOfficeSystem type
        /// </summary>
        ProfessionalOfficeSystem = 0x5,

        /// <summary>
        /// PersonalDistributionList1 type
        /// </summary>
        PersonalDistributionList1 = 0x6,

        /// <summary>
        /// PersonalDistributionList2 type
        /// </summary>
        PersonalDistributionList2 = 0x7
    }

    /// <summary>
    /// The enum value of DisplayType.
    /// </summary>
    public enum DisplayType : byte
    {
        /// <summary>
        /// A messaging user
        /// </summary>
        MessagingUser = 0x00,

        /// <summary>
        /// A distribution list
        /// </summary>
        DistributionList = 0x01,

        /// <summary>
        /// A forum, such as a bulletin board service or a public or shared folder
        /// </summary>
        Forum = 0x02,

        /// <summary>
        /// An automated agent
        /// </summary>
        AutomatedAgent = 0x03,

        /// <summary>
        /// An Address Book object defined for a large group, such as helpdesk, accounting, coordinator, or department
        /// </summary>
        AddressBookforLargeGroup = 0x04,

        /// <summary>
        /// A private, personally administered distribution list
        /// </summary>
        Private = 0x05,

        /// <summary>
        /// An Address Book object known to be from a foreign or remote messaging system
        /// </summary>
        AddressBookfromMessagingSystem = 0x06
    }

    /// <summary>
    /// Section 2.11.1   Property Data Types
    /// </summary>
    public enum PropertyDataType : ushort
    {
        /// <summary>
        /// PtypInteger16 type
        /// </summary>
        PtypInteger16 = 0x0002,

        /// <summary>
        /// PtypInteger32 type
        /// </summary>
        PtypInteger32 = 0x0003,

        /// <summary>
        /// PtypFloating32 type
        /// </summary>
        PtypFloating32 = 0x0004,

        /// <summary>
        /// PtypFloating64 type
        /// </summary>
        PtypFloating64 = 0x0005,

        /// <summary>
        /// PtypCurrency type
        /// </summary>
        PtypCurrency = 0x0006,

        /// <summary>
        /// PtypFloatingTime type
        /// </summary>
        PtypFloatingTime = 0x0007,

        /// <summary>
        /// PtypErrorCode type
        /// </summary>
        PtypErrorCode = 0x000A,

        /// <summary>
        /// PtypBoolean type
        /// </summary>
        PtypBoolean = 0x000B,

        /// <summary>
        /// PtypInteger64 type
        /// </summary>
        PtypInteger64 = 0x0014,

        /// <summary>
        /// PtypString type
        /// </summary>
        PtypString = 0x001F,

        /// <summary>
        /// PtypString8 type
        /// </summary>
        PtypString8 = 0x001E,

        /// <summary>
        /// PtypTime type
        /// </summary>
        PtypTime = 0x0040,

        /// <summary>
        /// PtypGuid type
        /// </summary>
        PtypGuid = 0x0048,

        /// <summary>
        /// PtypServerId type
        /// </summary>
        PtypServerId = 0x00FB,

        /// <summary>
        /// PtypRestriction type
        /// </summary>
        PtypRestriction = 0x00FD,

        /// <summary>
        /// PtypRuleAction type
        /// </summary>
        PtypRuleAction = 0x00FE,

        /// <summary>
        /// PtypBinary type
        /// </summary>
        PtypBinary = 0x0102,

        /// <summary>
        /// PtypMultipleInteger16 type
        /// </summary>
        PtypMultipleInteger16 = 0x1002,

        /// <summary>
        /// PtypMultipleInteger32 type
        /// </summary>
        PtypMultipleInteger32 = 0x1003,

        /// <summary>
        /// PtypMultipleFloating32 type
        /// </summary>
        PtypMultipleFloating32 = 0x1004,

        /// <summary>
        /// PtypMultipleFloating64 type
        /// </summary>
        PtypMultipleFloating64 = 0x1005,

        /// <summary>
        /// PtypMultipleCurrency type
        /// </summary>
        PtypMultipleCurrency = 0x1006,

        /// <summary>
        /// PtypMultipleFloatingTime type
        /// </summary>
        PtypMultipleFloatingTime = 0x1007,

        /// <summary>
        /// PtypMultipleInteger64 type
        /// </summary>
        PtypMultipleInteger64 = 0x1014,

        /// <summary>
        /// PtypMultipleString type
        /// </summary>
        PtypMultipleString = 0x101F,

        /// <summary>
        /// PtypMultipleString8 type
        /// </summary>
        PtypMultipleString8 = 0x101E,

        /// <summary>
        /// PtypMultipleTime type
        /// </summary>
        PtypMultipleTime = 0x1040,

        /// <summary>
        /// PtypMultipleGuid type
        /// </summary>
        PtypMultipleGuid = 0x1048,

        /// <summary>
        /// PtypMultipleBinary type
        /// </summary>
        PtypMultipleBinary = 0x1102,

        /// <summary>
        /// PtypUnspecified type
        /// </summary>
        PtypUnspecified = 0x0000,

        /// <summary>
        /// PtypNull type
        /// </summary>
        PtypNull = 0x0001,

        /// <summary>
        /// IN FUTURE: How to distinguish PtypObject from PtypEmbeddedTable since they share the same value
        /// </summary>
        PtypObject_Or_PtypEmbeddedTable = 0x000D,
    }

    /// <summary>
    /// Section 2.11.1.3   Multi-value Property Value Instances
    /// </summary>
    public enum PropertyDataTypeFlag : ushort
    {
        /// <summary>
        /// MutltiValue flag
        /// </summary>
        MutltiValue = 0x1000,

        /// <summary>
        /// MultivalueInstance flag
        /// </summary>
        MultivalueInstance = 0x2000,
    }

    /// <summary>
    /// The enum of the Ptyp data type Count wide : 16 bits wide or 32 bits wide.
    /// </summary>
    public enum CountWideEnum : uint
    {
        /// <summary>
        /// The count width is two bytes
        /// </summary>
        twoBytes = 2,

        /// <summary>
        /// The count width is four bytes
        /// </summary>
        fourBytes = 4
    }

    /// <summary>
    /// The enum value of StringType
    /// </summary>
    public enum StringTypeEnum : byte
    {
        /// <summary>
        /// No string is present.
        /// </summary>
        NoPresent = 0x00,

        /// <summary>
        /// The string is empty.
        /// </summary>
        Empty = 0x01,

        /// <summary>
        /// Null-terminated 8-bit character string. 
        /// </summary>
        CharacterString = 0x02,

        /// <summary>
        /// Null-terminated reduced Unicode character string. 
        /// </summary>
        ReducedUnicodeCharacterString = 0x03,

        /// <summary>
        /// Null-terminated Unicode character string. 
        /// </summary>
        UnicodeCharacterString = 0x04
    }

    /// <summary>
    /// The enum value of Order type.
    /// </summary>
    public enum OrderType : byte
    {
        /// <summary>
        /// Sort by this column in ascending order.
        /// </summary>
        Ascending = 0x00,

        /// <summary>
        /// Sort by this column in descending order.
        /// </summary>
        Descending = 0x01,

        /// <summary>
        /// This is an aggregated column in a categorized sort, whose maximum value (within the group of items with the same value as that of the previous category) is to be used as the sort key for the entire group.
        /// </summary>
        MaximumCategory = 0x04
    }

    /// <summary>
    /// The enum value of restriction value.
    /// </summary>
    public enum RestrictTypeEnum : byte
    {
        /// <summary>
        /// Logical AND operation applied to a list of subrestrictions.
        /// </summary>
        AndRestriction = 0x00,

        /// <summary>
        /// Logical OR operation applied to a list of subrestrictions.
        /// </summary>
        OrRestriction = 0x01,

        /// <summary>
        /// Logical NOT operation applied to a subrestriction.
        /// </summary>
        NotRestriction = 0x02,

        /// <summary>
        /// Search a property value for specific content.
        /// </summary>
        ContentRestriction = 0x03,

        /// <summary>
        /// Compare a property value with a particular value.
        /// </summary>
        PropertyRestriction = 0x04,

        /// <summary>
        /// Compare the values of two properties.
        /// </summary>
        ComparePropertiesRestriction = 0x05,

        /// <summary>
        /// Perform a bitwise AND operation on a property value with a mask and compare that with 0 (zero).
        /// </summary>
        BitMaskRestriction = 0x06,

        /// <summary>
        /// Compare the size of a property value to a particular figure.
        /// </summary>
        SizeRestriction = 0x07,

        /// <summary>
        /// Test whether a property has a value.
        /// </summary>
        ExistRestriction = 0x08,

        /// <summary>
        /// Test whether any row of a message's attachment or recipient table satisfies a subrestriction.
        /// </summary>
        SubObjectRestriction = 0x09,

        /// <summary>
        /// Associates a comment with a subrestriction.
        /// </summary>
        CommentRestriction = 0x0A,

        /// <summary>
        /// Limits the number of matches returned from a subrestriction.
        /// </summary>
        CountRestriction = 0x0B
    }

    /// <summary>
    /// The enum of FuzzyLevelLow.
    /// </summary>
    public enum FuzzyLevelLowEnum : ushort
    {
        /// <summary>
        /// The value stored in the TaggedValue field and the value of the column property tag match one another in their entirety.
        /// </summary>
        FL_FULLSTRING = 0x0000,

        /// <summary>
        /// The value stored in the TaggedValue field matches some portion of the value of the column property tag.
        /// </summary>
        FL_SUBSTRING = 0x0001,

        /// <summary>
        /// The value stored in the TaggedValue field matches a starting portion of the value of the column property tag.
        /// </summary>
        FL_PREFIX = 0x0002
    }

    /// <summary>
    /// The enum of FuzzyLevelHighEnum.
    /// </summary>
    public enum FuzzyLevelHighEnum : ushort
    {
        /// <summary>
        /// The comparison does not consider case.
        /// </summary>
        FL_IGNORECASE = 0x00001,

        /// <summary>
        /// The comparison ignores Unicode-defined nonspacing characters such as diacritical marks.
        /// </summary>
        FL_IGNORENONSPACE = 0x0002,

        /// <summary>
        /// The comparison results in a match whenever possible, ignoring case and nonspacing characters.
        /// </summary>
        FL_LOOSE = 0x0004
    }

    /// <summary>
    /// The enum type of BitmapRelOp.
    /// </summary>
    public enum BitmapRelOpType : byte
    {
        /// <summary>
        /// Perform a bitwise AND operation on the value of the Mask field with the value of the property PropTag field, and test for being equal to 0 (zero).
        /// </summary>
        BMR_EQZ = 0x00,

        /// <summary>
        /// Perform a bitwise AND operation on the value of the Mask field with the value of the property PropTag field, and test for not being equal to 0 (zero).
        /// </summary>
        BMR_NEZ = 0x01
    }

    /// <summary>
    /// The enum type of RelOp.
    /// </summary>
    public enum RelOpType : byte
    {
        /// <summary>
        /// TRUE if the value of the object's property is less than the specified value.
        /// </summary>
        RelationalOperatorLessThan = 0x00,

        /// <summary>
        /// TRUE if the value of the object's property is less than or equal to the specified value.
        /// </summary>
        RelationalOperatorLessThanOrEqual = 0x01,

        /// <summary>
        /// TRUE if the value of the object's property value is greater than the specified value.
        /// </summary>
        RelationalOperatorGreaterThan = 0x02,

        /// <summary>
        /// TRUE if the value of the object's property value is greater than or equal to the specified value.
        /// </summary>
        RelationalOperatorGreaterThanOrEqual = 0x03,

        /// <summary>
        /// TRUE if the object's property value equals the specified value.
        /// </summary>
        RelationalOperatorEqual = 0x04,

        /// <summary>
        /// TRUE if the object's property value does not equal the specified value.
        /// </summary>
        RelationalOperatorNotEqual = 0x5,

        /// <summary>
        /// TRUE if the value of the object's property is in the DL membership of the specified property value. 
        /// </summary>
        RelationalOperatorMemberOfDL = 0x64
    }

    /// <summary>
    /// The AnnotatedBytes class to a byte stream with an alternate version of it (typically ConvertByteArrayToString)
    /// </summary>
    public class AnnotatedBytes : BaseStructure
    {
        /// <summary>
        /// Bytes as byte array.
        /// </summary>
        public byte[] bytes;

        /// <summary>
        /// The annotated value
        /// </summary>
        public string Annotation;

        //public int StartIndex;

        private int Size;

        /// <summary>
        /// Initializes a new instance of the MAPIString class without parameters.
        /// </summary>
        public AnnotatedBytes()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Information class with parameters.
        /// </summary>
        /// <param name="size">Size of the byte array</param>
        public AnnotatedBytes(int size)
        {
            this.Size = size;
        }

        /// <summary>
        /// Parse method
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            var offset = (int)s.Position;
            this.bytes = this.ReadBytes(this.Size);
            this.Annotation = Utilities.ConvertByteArrayToString(this.bytes);
        }

        public void SetAnnotation(string annotation) { this.Annotation = annotation; }
    }

    /// <summary>
    /// The MAPIString class to record the related attributes of string.
    /// </summary>
    public class MAPIString : BaseStructure
    {
        /// <summary>
        /// The string value
        /// </summary>
        public string Value;

        /// <summary>
        /// The string Encoding : ASCII or Unicode
        /// </summary>
        public Encoding Encode;

        /// <summary>
        /// The string Terminator. Default is "\0"
        /// </summary>
        public string Terminator;

        /// <summary>
        /// If the StringLength is not 0, The StringLength will be as the string length
        /// </summary>
        public int StringLength;

        /// <summary>
        /// If the Encoding is Unicode, and it is reduced Unicode, it is true
        /// </summary>
        public bool ReducedUnicode;

        /// <summary>
        /// Initializes a new instance of the MAPIString class without parameters.
        /// </summary>
        public MAPIString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MAPIString class with parameters.
        /// </summary>
        /// <param name="encode">The encode type</param>
        /// <param name="terminator">Specify the terminator of the string</param>
        /// <param name="stringLength">Length of the string</param>
        /// <param name="reducedUnicode">Indicate Whether the terminator is reduced</param>
        public MAPIString(Encoding encode, string terminator = "\0", int stringLength = 0, bool reducedUnicode = false)
        {
            this.Encode = encode;
            this.Terminator = terminator;
            this.StringLength = stringLength;
            this.ReducedUnicode = reducedUnicode;
        }

        /// <summary>
        /// Parse method
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadString(this.Encode, this.Terminator, this.StringLength, this.ReducedUnicode);
        }
    }

    /// <summary>
    /// The MAPIString class to record the related attributes of string.
    /// </summary>
    public class MAPIStringAddressBook : BaseStructure
    {
        /// <summary>
        /// The string value
        /// </summary>
        public string Value;

        /// <summary>
        /// TDI#76879 tell us the real MapiHttp traffic will add the magic byte 'FF' for the string or binary based property value.
        /// </summary>
        public byte? MagicNumber;

        /// <summary>
        /// The string Encoding : ASCII or Unicode
        /// </summary>
        public Encoding Encode;

        /// <summary>
        /// The string Terminator. Default is "\0".
        /// </summary>
        public string Terminator;

        /// <summary>
        /// If the StringLength is not 0, The StringLength will be as the string length.
        /// </summary>
        public int StringLength;

        /// <summary>
        /// If the Encoding is Unicode, and it is reduced Unicode, it is true.
        /// </summary>
        public bool ReducedUnicode;

        /// <summary>
        /// Initializes a new instance of the MAPIStringAddressBook class without parameters.
        /// </summary>
        public MAPIStringAddressBook()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MAPIStringAddressBook class with parameters.
        /// </summary>
        /// <param name="encode">The encoding type</param>
        /// <param name="terminator">The string terminator</param>
        /// <param name="stringLength">The string length</param>
        /// <param name="reducedUnicode">INdicate whether the terminator is reduced</param>
        public MAPIStringAddressBook(Encoding encode, string terminator = "\0", int stringLength = 0, bool reducedUnicode = false)
        {
            this.Encode = encode;
            this.Terminator = terminator;
            this.StringLength = stringLength;
            this.ReducedUnicode = reducedUnicode;
        }

        /// <summary>
        /// The Parse method
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            if (this.ReadByte() == 0xff)
            {
                this.MagicNumber = 0xff;
            }
            else
            {
                s.Position -= 1;
            }

            this.Value = this.ReadString(this.Encode, this.Terminator, this.StringLength, this.ReducedUnicode);
        }
    }

    #region 2.1	AddressList Structures

    /// <summary>
    /// 2.1.1 AddressEntry Structure
    /// </summary>
    public class AddressEntry : BaseStructure
    {
        /// <summary>
        /// An unsigned integer whose value is equal to the number of associated TaggedPropertyValue structures, as specified in section 2.11.4. 
        /// </summary>
        public uint PropertyCount;

        /// <summary>
        /// A set of TaggedPropertyValue structures representing one addressee.
        /// </summary>
        public TaggedPropertyValue[] Values;

        /// <summary>
        /// Parse the AddressEntry structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressEntry structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyCount = this.ReadUint();
            List<TaggedPropertyValue> tempArray = new List<TaggedPropertyValue>();
            for (int i = 0; i < this.PropertyCount; i++)
            {
                TaggedPropertyValue tempproperty = new TaggedPropertyValue();
                tempproperty.Parse(s);
                tempArray.Add(tempproperty);
            }

            this.Values = tempArray.ToArray();
        }
    }

    /// <summary>
    ///  2.1.2 AddressList Structure
    /// </summary>
    public class AddressList : BaseStructure
    {
        /// <summary>
        /// An unsigned integer whose value is equal to the number of associated addressees.
        /// </summary>
        public uint AddressCount;

        /// <summary>
        /// An array of AddressEntry structures. The number of structures is indicated by the AddressCount field.
        /// </summary>
        public AddressEntry[] Addresses;

        /// <summary>
        /// Parse the AddressList structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressList structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.AddressCount = this.ReadUint();
            List<AddressEntry> tempArray = new List<AddressEntry>();
            for (int i = 0; i < this.AddressCount; i++)
            {
                AddressEntry tempAddress = new AddressEntry();
                tempAddress.Parse(s);
                tempArray.Add(tempAddress);
            }

            this.Addresses = tempArray.ToArray();
        }
    }
    #endregion

    #region 2.2	EntryID and Related Types

    #region 2.2.1	Folder ID, Message ID, and Global Identifier Structures

    /// <summary>
    /// 2.2.1.1 Folder ID Structure
    /// </summary>
    public class FolderID : BaseStructure
    {
        /// <summary>
        /// An unsigned integer identifying a Store object.
        /// </summary>
        public ushort ReplicaId;

        /// <summary>
        /// An unsigned integer identifying the folder within its Store object. 6 bytes
        /// </summary>
        public byte[] GlobalCounter;

        /// <summary>
        /// Parse the FolderID structure.
        /// </summary>
        /// <param name="s">A stream containing the FolderID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ReplicaId = this.ReadUshort();
            this.GlobalCounter = this.ReadBytes(6);
        }
    }

    /// <summary>
    /// 2.2.1.2 Message ID Structure
    /// </summary>
    public class MessageID : BaseStructure
    {
        /// <summary>
        /// An unsigned integer identifying a Store object.
        /// </summary>
        public ushort ReplicaId;

        /// <summary>
        /// An unsigned integer identifying the message within its Store object. 6 bytes
        /// </summary>
        public byte[] GlobalCounter;

        /// <summary>
        /// Parse the MessageID structure.
        /// </summary>
        /// <param name="s">A stream containing the MessageID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ReplicaId = this.ReadUshort();
            this.GlobalCounter = this.ReadBytes(6);
        }
    }

    /// <summary>
    /// 2.2.1.3.1 LongTermID Structure
    /// </summary>
    public class LongTermID : BaseStructure
    {
        /// <summary>
        /// An unsigned integer identifying a Store object.
        /// </summary>
        public Guid DatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the folder or message within its Store object. 6 bytes
        /// </summary>
        public byte[] GlobalCounter;

        /// <summary>
        /// A 2-byte Pad field. 
        /// </summary>
        public ushort Pad;

        /// <summary>
        /// Parse the LongTermID structure.
        /// </summary>
        /// <param name="s">A stream containing the LongTermID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.DatabaseGuid = this.ReadGuid();
            this.GlobalCounter = this.ReadBytes(6);
            this.Pad = this.ReadUshort();
        }
    }
    #endregion

    #region 2.2.4	Messaging Object EntryIDs Structures

    /// <summary>
    /// 2.2.4.1 Folder EntryID Structure
    /// </summary>
    public class FolderEntryID : BaseStructure
    {
        /// <summary>
        /// This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// The value of this field is determined by where the folder is located. 
        /// </summary>
        public object ProviderUID;

        /// <summary>
        /// One of several Store object types specified in the table in section 2.2.4.
        /// </summary>
        public StoreObjectType FolderType;

        /// <summary>
        /// A GUID associated with the Store object and corresponding to the ReplicaId field of the FID structure.
        /// </summary>
        public Guid DatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the folder. 6 bytes
        /// </summary>
        public byte[] GlobalCounter;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public ushort Pad;

        /// <summary>
        /// Parse the FolderEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the FolderEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            byte[] tempProviderUID = this.ReadBytes(16);
            byte[] verifyProviderUID = { 0x1A, 0x44, 0x73, 0x90, 0xAA, 0x66, 0x11, 0xCD, 0x9B, 0xC8, 0x00, 0xAA, 0x00, 0x2F, 0xC4, 0x5A };
            if (tempProviderUID == verifyProviderUID)
            {
                this.ProviderUID = tempProviderUID;
            }
            else
            {
                this.ProviderUID = new Guid(tempProviderUID);
            }

            this.FolderType = (StoreObjectType)this.ReadUshort();
            this.DatabaseGuid = this.ReadGuid();
            this.GlobalCounter = this.ReadBytes(6);
            this.Pad = this.ReadUshort();
        }
    }

    /// <summary>
    /// 2.2.4.2 Message EntryID Structure
    /// </summary>
    public class MessageEntryID : BaseStructure
    {
        /// <summary>
        /// This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// The value of this field is determined by where the folder is located.
        /// </summary>
        public object ProviderUID;

        /// <summary>
        /// One of several Store object types specified in the table in section 2.2.4.
        /// </summary>
        public StoreObjectType MessageType;

        /// <summary>
        /// A GUID associated with the Store object of the folder in which the message resides and corresponding to the ReplicaId field in the folder ID structure, as specified in section 2.2.1.1.
        /// </summary>
        public Guid FolderDatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the folder in which the message resides. 6 bytes
        /// </summary>
        public byte[] FolderGlobalCounter;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public ushort Pad1;

        /// <summary>
        /// A GUID associated with the Store object of the message and corresponding to the ReplicaId field of the Message ID structure, as specified in section 2.2.1.2.
        /// </summary>
        public Guid MessageDatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the message. 6 bytes
        /// </summary>
        public byte[] MessageGlobalCounter;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public ushort Pad2;

        /// <summary>
        /// Parse the MessageEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the MessageEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            byte[] tempProviderUID = this.ReadBytes(16);
            if (tempProviderUID.ToString() == "%x1A.44.73.90.AA.66.11.CD.9B.C8.00.AA.00.2F.C4.5A")
            {
                this.ProviderUID = tempProviderUID;
            }
            else
            {
                this.ProviderUID = new Guid(tempProviderUID);
            }

            this.MessageType = (StoreObjectType)this.ReadUshort();
            this.FolderDatabaseGuid = this.ReadGuid();
            this.FolderGlobalCounter = this.ReadBytes(6);
            this.Pad1 = this.ReadUshort();
            this.MessageDatabaseGuid = this.ReadGuid();
            this.MessageGlobalCounter = this.ReadBytes(6);
            this.Pad2 = this.ReadUshort();
        }
    }

    /// <summary>
    /// 2.2.4.3 Store Object EntryID Structure
    /// </summary>
    public class StoreObjectEntryID : BaseStructure
    {
        /// <summary>
        /// This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// The identifier for the provider that created the EntryID. 
        /// </summary>
        public byte[] ProviderUID;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public byte Version;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// This field MUST be set to the following value, which represents "emsmdb.dll": %x45.4D.53.4D.44.42.2E.44.4C.4C.00.00.00.00.
        /// </summary>
        public byte[] DLLFileName;

        /// <summary>
        /// This value MUST be set to 0x00000000
        /// </summary>
        public uint WrappedFlags;

        /// <summary>
        /// This Wrapped Provider UID.
        /// </summary>
        public byte[] WrappedProviderUID;

        /// <summary>
        /// The value of this field is determined by where the folder is located. 
        /// </summary>
        public uint WrappedType;

        /// <summary>
        /// A string of single-byte characters terminated by a single zero byte, indicating the short name or NetBIOS name of the server.
        /// </summary>
        public MAPIString ServerShortname;

        /// <summary>
        /// A string of single-byte characters terminated by a single zero byte and representing the X500 DN of the mailbox, as specified in [MS-OXOAB]. 
        /// </summary>
        public MAPIString MailboxDN;

        /// <summary>
        /// Parse the StoreObjectEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the StoreObjectEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.ProviderUID = this.ReadBytes(16);
            this.Version = this.ReadByte();
            this.Flag = this.ReadByte();
            this.DLLFileName = this.ReadBytes(14);
            this.WrappedFlags = this.ReadUint();
            this.WrappedProviderUID = this.ReadBytes(16);
            this.WrappedType = this.ReadUint();
            this.ServerShortname = new MAPIString(Encoding.ASCII);
            this.ServerShortname.Parse(s);
            this.MailboxDN = new MAPIString(Encoding.ASCII);
            this.MailboxDN.Parse(s);
        }
    }
    #endregion

    #region 2.2.5.2  Address Book EntryID Structure
    /// <summary>
    /// 2.2.5.2  Address Book EntryID Structure
    /// </summary>
    public class AddressBookEntryID : BaseStructure
    {
        /// <summary>
        /// This value MUST be set to 0x00000000, indicating a long-term EntryID.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// The identifier for the provider that created the EntryID. 
        /// </summary>
        public byte[] ProviderUID;

        /// <summary>
        /// This value MUST be set to %x01.00.00.00.
        /// </summary>
        public uint Version;

        /// <summary>
        /// An integer representing the type of the object. 
        /// </summary>
        public AddressbookEntryIDtype Type;

        /// <summary>
        /// The X500 DN of the Address Book object. 
        /// </summary>
        public MAPIString X500DN;

        /// <summary>
        /// Parse the AddressBookEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressBookEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.ProviderUID = this.ReadBytes(16);
            this.Version = this.ReadUint();
            this.Type = (AddressbookEntryIDtype)this.ReadUint();
            this.X500DN = new MAPIString(Encoding.ASCII);
            this.X500DN.Parse(s);
        }
    }

    #endregion
    #endregion

    #region 2.6	Property Name Structures

    /// <summary>
    /// 2.6.1 PropertyName Structure
    /// </summary>
    public class PropertyName : BaseStructure
    {
        /// <summary>
        /// The Kind field. 
        /// </summary>
        public KindEnum Kind;

        /// <summary>
        /// The GUID that identifies the property set for the named property.
        /// </summary>
        public Guid GUID;

        /// <summary>
        /// This field is present only if the value of the Kind field is equal to 0x00.
        /// </summary>
        public uint? LID;

        /// <summary>
        /// The value of this field is equal to the number of bytes in the Name string that follows it. 
        /// </summary>
        public byte? NameSize;

        /// <summary>
        /// This field is present only if Kind is equal to 0x01.
        /// </summary>
        public MAPIString Name;

        /// <summary>
        /// Parse the PropertyName structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyName structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Kind = (KindEnum)ReadByte();
            this.GUID = this.ReadGuid();
            switch (this.Kind)
            {
                case KindEnum.LID:
                    {
                        this.LID = this.ReadUint();
                        break;
                    }

                case KindEnum.Name:
                    {
                        this.NameSize = this.ReadByte();
                        this.Name = new MAPIString(Encoding.Unicode, string.Empty, (int)this.NameSize / 2);
                        this.Name.Parse(s);

                        break;
                    }

                case KindEnum.NoPropertyName:
                default:
                    {
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// 2.6.2 PropertyName_r Structure
    /// </summary>
    public class PropertyName_r : BaseStructure
    {
        /// <summary>
        /// Encodes the GUID field of the PropertyName structure, as specified in section 2.6.1.
        /// </summary>
        public Guid GUID;

        /// <summary>
        /// All clients and servers MUST set this value to 0x00000000.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// This value encodes the LID field in the PropertyName structure, as specified in section 2.6.1.
        /// </summary>
        public uint LID;

        /// <summary>
        /// Parse the PropertyName_r structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyName_r structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.GUID = this.ReadGuid();
            this.Reserved = this.ReadUint();
            this.LID = this.ReadUint();
        }
    }
    #endregion

    #region 2.7	PropertyProblem Structure
    /// <summary>
    /// 2.7 PropertyProblem Structure
    /// </summary>
    public class PropertyProblem : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value specifies an index into an array of property tags.
        /// </summary>
        public ushort Index;

        /// <summary>
        /// A PropertyTag structure, as specified in section 2.9. 
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// An unsigned integer. This value specifies the error that occurred when processing this property.
        /// </summary>
        public PropertyErrorCodes ErrorCode;

        /// <summary>
        /// Parse the PropertyProblem structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyProblem structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Index = this.ReadUshort();
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            this.ErrorCode = (PropertyErrorCodes)this.ReadUint();
        }
    }
    #endregion

    #region 2.8	Property Row Structures
    /// <summary>
    /// 2.8.1 PropertyRow Structures
    /// </summary>
    public class PropertyRow : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicate if all property values are present and without error.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// An array of variable-sized structures.
        /// </summary>
        public object[] ValueArray;

        /// <summary>
        /// The array of property tag.
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Initializes a new instance of the PropertyRow class
        /// </summary>
        /// <param name="propTags">The array of property tag.</param>
        public PropertyRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the PropertyRow structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flag = this.ReadByte();
            List<object> tempPropArray = new List<object>();
            if (this.propTags != null)
            {
                foreach (PropertyTag tempPropTag in this.propTags)
                {
                    object rowPropValue = null;
                    tempPropTag.PropertyType = this.ConvertToPropType((ushort)tempPropTag.PropertyType);

                    if (this.Flag == 0x00)
                    {
                        if (tempPropTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            PropertyValue propValue = new PropertyValue(tempPropTag.PropertyType);
                            propValue.Parse(s);
                            rowPropValue = propValue;
                        }
                        else
                        {
                            TypedPropertyValue typePropValue = new TypedPropertyValue();
                            typePropValue.Parse(s);
                            rowPropValue = typePropValue;
                        }
                    }
                    else if (this.Flag == 0x01)
                    {
                        if (tempPropTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            FlaggedPropertyValue flagPropValue = new FlaggedPropertyValue(tempPropTag.PropertyType);
                            flagPropValue.Parse(s);
                            rowPropValue = flagPropValue;
                        }
                        else
                        {
                            FlaggedPropertyValueWithType flagPropValue = new FlaggedPropertyValueWithType();
                            flagPropValue.Parse(s);
                            rowPropValue = flagPropValue;
                        }
                    }

                    tempPropArray.Add(rowPropValue);
                }
            }

            this.ValueArray = tempPropArray.ToArray();
        }
    }

    /// <summary>
    /// 2.8.3.1 RecipientFlags Field
    /// </summary>
    public class RecipientFlags : BaseStructure
    {
        /// <summary>
        /// If this flag is b'1', a different transport is responsible for delivery to this recipient (1).
        /// </summary>
        [BitAttribute(1)]
        public byte R;

        /// <summary>
        /// If this flag is b'1', the value of the TransmittableDisplayName field is the same as the value of the DisplayName field.
        /// </summary>
        [BitAttribute(1)]
        public byte S;

        /// <summary>
        /// If this flag is b'1', the TransmittableDisplayName (section 2.8.3.2) field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte T;

        /// <summary>
        /// If this flag is b'1', the DisplayName (section 2.8.3.2) field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte D;

        /// <summary>
        /// If this flag is b'1', the EmailAddress (section 2.8.3.2) field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte E;

        /// <summary>
        /// This enumeration specifies the type of address. 
        /// </summary>
        [BitAttribute(3)]
        public AddressTypeEnum Type;

        /// <summary>
        /// If this flag is b'1', this recipient (1) has a non-standard address type and the AddressType field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte O;

        /// <summary>
        /// The server MUST set this to b'0000'.
        /// </summary>
        [BitAttribute(4)]
        public byte Reserved;

        /// <summary>
        /// If this flag is b'1', the SimpleDisplayName field is included.
        /// </summary>
        [BitAttribute(1)]
        public byte I;

        /// <summary>
        /// If this flag is b'1', the associated string properties are in Unicode with a 2-byte terminating null character; if this flag is b'0', string properties are MBCS with a single terminating null character.
        /// </summary>
        [BitAttribute(1)]
        public byte U;

        /// <summary>
        /// If b'1', this flag specifies that the recipient (1) does not support receiving rich text messages.
        /// </summary>
        [BitAttribute(1)]
        public byte N;

        /// <summary>
        /// Parse the RecipientFlags structure.
        /// </summary>
        /// <param name="s">A stream containing the RecipientFlags structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte tempByte = ReadByte();
            int index = 0;
            this.R = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.S = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.T = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.D = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.E = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.Type = (AddressTypeEnum)GetBits(tempByte, index, 3);

            tempByte = this.ReadByte();
            index = 0;
            this.O = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.Reserved = this.GetBits(tempByte, index, 4);
            index = index + 4;
            this.I = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.U = this.GetBits(tempByte, index, 1);
            index = index + 1;
            this.N = this.GetBits(tempByte, index, 1);
        }
    }

    /// <summary>
    /// 2.8.3.2 RecipientRow Structure
    /// </summary>
    public class RecipientRow : BaseStructure
    {
        /// <summary>
        /// A RecipientFlags structure, as specified in section 2.8.3.1. 
        /// </summary>
        public RecipientFlags RecipientFlags;

        /// <summary>
        /// Unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise. 
        /// </summary>
        public byte? AddressPrefixUsed;

        /// <summary>
        /// An enumeration. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise. 
        /// </summary>
        public DisplayType? DisplayType;

        /// <summary>
        /// A null-terminated ASCII string. 
        /// </summary>
        public MAPIString X500DN;

        /// <summary>
        /// An unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7). 
        /// </summary>
        public ushort? EntryIdSize;

        /// <summary>
        /// An array of bytes. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7). 
        /// </summary>
        public AddressBookEntryID EntryID;

        /// <summary>
        /// This value specifies the size of the SearchKey field.
        /// </summary>
        public ushort? SearchKeySize;

        /// <summary>
        /// This array specifies the search key of the distribution list.
        /// </summary>
        public byte?[] SearchKey;

        /// <summary>
        /// This string specifies the address type of the recipient (1).
        /// </summary>
        public MAPIString AddressType;

        /// <summary>
        /// This string specifies the email address of the recipient (1).
        /// </summary>
        public MAPIString EmailAddress;

        /// <summary>
        /// This string specifies the display name of the recipient (1).
        /// </summary>
        public MAPIString DisplayName;

        /// <summary>
        /// This string specifies the simple display name of the recipient (1).
        /// </summary>
        public MAPIString SimpleDisplayName;

        /// <summary>
        /// This string specifies the transmittable display name of the recipient (1).
        /// </summary>
        public MAPIString TransmittableDisplayName;

        /// <summary>
        /// This value specifies the number of columns from the RecipientColumns field that are included in the RecipientProperties field. 
        /// </summary>
        public ushort? RecipientColumnCount;

        /// <summary>
        /// The columns used for this row are those specified in RecipientProperties.
        /// </summary>
        public PropertyRow RecipientProperties;

        /// <summary>
        /// The array of property tag.
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Initializes a new instance of the RecipientRow class
        /// </summary>
        /// <param name="propTags">The property Tags</param>
        public RecipientRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the RecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing the RecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RecipientFlags = new RecipientFlags();
            this.RecipientFlags.Parse(s);
            if (this.RecipientFlags.Type == AddressTypeEnum.X500DN)
            {
                this.AddressPrefixUsed = this.ReadByte();
                this.DisplayType = (DisplayType)ReadByte();
                this.X500DN = new MAPIString(Encoding.ASCII);
                this.X500DN.Parse(s);
            }
            else if (this.RecipientFlags.Type == AddressTypeEnum.PersonalDistributionList1 || this.RecipientFlags.Type == AddressTypeEnum.PersonalDistributionList2)
            {
                this.EntryIdSize = this.ReadUshort();
                this.EntryID = new AddressBookEntryID();
                this.EntryID.Parse(s);
                this.SearchKeySize = this.ReadUshort();
                this.SearchKey = this.ConvertArray(this.ReadBytes((int)this.SearchKeySize));
            }
            else if (this.RecipientFlags.Type == AddressTypeEnum.NoType && this.RecipientFlags.O == 0x1)
            {
                this.AddressType = new MAPIString(Encoding.ASCII);
                this.AddressType.Parse(s);
            }

            if (this.RecipientFlags.E == 0x1)
            {
                this.EmailAddress = new MAPIString((this.RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.EmailAddress.Parse(s);
            }

            if (this.RecipientFlags.D == 0x1)
            {
                this.DisplayName = new MAPIString((this.RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.DisplayName.Parse(s);
            }

            if (this.RecipientFlags.I == 0x1)
            {
                this.SimpleDisplayName = new MAPIString((this.RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.SimpleDisplayName.Parse(s);
            }

            if (this.RecipientFlags.T == 0x1)
            {
                this.TransmittableDisplayName = new MAPIString((this.RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.TransmittableDisplayName.Parse(s);
            }

            this.RecipientColumnCount = this.ReadUshort();
            List<PropertyTag> propTagsActually = new List<PropertyTag>();
            if (this.propTags.Length >= this.RecipientColumnCount)
            {
                for (int i = 0; i < this.RecipientColumnCount; i++)
                {
                    propTagsActually.Add(this.propTags[i]);
                }
            }
            else
            {
                throw new Exception(string.Format("Request format error: the RecipientColumnCount {0} should be less than RecipientColumns count {1}", this.RecipientColumnCount, this.propTags.Length));
            }

            PropertyRow tempPropertyRow = new PropertyRow(propTagsActually.ToArray());
            this.RecipientProperties = tempPropertyRow;
            this.RecipientProperties.Parse(s);
        }
    }
    #endregion

    #region 2.9	PropertyTag Structure

    /// <summary>
    /// 2.9 PropertyTag Structure
    /// </summary>
    public class PropertyTag : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value, as specified by the table in section 2.11.1.
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public PidTagPropertyEnum PropertyId;

        /// <summary>
        /// Initializes a new instance of the PropertyTag class with parameters.
        /// </summary>
        /// <param name="ptype">The Type of the PropertyTag.</param>
        /// <param name="pId">The Id of the PropertyTag.</param>
        public PropertyTag(PropertyDataType ptype, PidTagPropertyEnum pId)
        {
            this.PropertyType = ptype;
            this.PropertyId = pId;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyTag class without parameters.
        /// </summary>
        public PropertyTag()
        {
        }

        /// <summary>
        /// Parse the PropertyTag structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyTag structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)ReadUshort();
            this.PropertyId = (PidTagPropertyEnum)ReadUshort();
        }
    }
    #endregion

    #region 2.11	Property Values
    #region 2.11.1   Property Data Types

    /// <summary>
    /// 2 bytes; a 16-bit integer. [MS-DTYP]: INT16
    /// </summary>
    public class PtypInteger16 : BaseStructure
    {
        /// <summary>
        /// 16-bit integer. 
        /// </summary>
        public short Value;

        /// <summary>
        /// Parse the PtypInteger16 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypInteger16 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadINT16();
        }
    }

    /// <summary>
    /// 4 bytes; a 32-bit integer. [MS-DTYP]: INT32
    /// </summary>
    public class PtypInteger32 : BaseStructure
    {
        /// <summary>
        /// 32-bit integer. 
        /// </summary>
        public int Value;

        /// <summary>
        /// Parse the PtypInteger32 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypInteger32 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadINT32();
        }
    }

    /// <summary>
    /// 4 bytes; a 32-bit floating point number. [MS-DTYP]: FLOAT
    /// </summary>
    public class PtypFloating32 : BaseStructure
    {
        /// <summary>
        /// 32-bit floating point number.
        /// </summary>
        public float Value;

        /// <summary>
        /// Parse the PtypFloating32 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloating32 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (float)this.ReadINT32();
        }
    }

    /// <summary>
    /// 8 bytes; a 64-bit floating point number. [MS-DTYP]: DOUBLE
    /// </summary>
    public class PtypFloating64 : BaseStructure
    {
        /// <summary>
        /// 64-bit floating point number. 
        /// </summary>
        public double Value;

        /// <summary>
        /// Parse the PtypFloating64 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloating64 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (double)this.ReadINT64();
        }
    }

    /// <summary>
    /// 8 bytes; a 64-bit signed, scaled integer representation of a decimal currency value, with four places to the right of the decimal point. [MS-DTYP]: LONGLONG, [MS-OAUT]: CURRENCY
    /// </summary>
    public class PtypCurrency : BaseStructure
    {
        /// <summary>
        /// 64-bit signed, scaled integer representation of a decimal currency value
        /// </summary>
        public long Value;

        /// <summary>
        /// Parse the PtypCurrency structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypCurrency structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadINT64();
        }
    }

    /// <summary>
    /// 8 bytes; a 64-bit floating point number. 
    /// </summary>
    public class PtypFloatingTime : BaseStructure
    {
        /// <summary>
        /// 64-bit floating point number. 
        /// </summary>
        public double Value;

        /// <summary>
        /// Parse the PtypFloatingTime structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloatingTime structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (double)this.ReadINT64();
        }
    }

    /// <summary>
    /// 4 bytes; a 32-bit integer encoding error information as specified in section 2.4.1.
    /// </summary>
    public class PtypErrorCode : BaseStructure
    {
        /// <summary>
        /// 32-bit integer encoding error information.
        /// </summary>
        public AdditionalErrorCodes Value;

        /// <summary>
        /// Parse the PtypErrorCode structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypErrorCode structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (AdditionalErrorCodes)this.ReadUint();
        }
    }

    /// <summary>
    /// 1 byte; restricted to 1 or 0.
    /// </summary>
    public class PtypBoolean : BaseStructure
    {
        /// <summary>
        /// 1 byte; restricted to 1 or 0.
        /// </summary>
        public bool Value;

        /// <summary>
        /// Parse the PtypBoolean structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypBoolean structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadBoolean();
        }
    }

    /// <summary>
    /// 8 bytes; a 64-bit integer.[MS-DTYP]: LONGLONG.
    /// </summary>
    public class PtypInteger64 : BaseStructure
    {
        /// <summary>
        /// 64-bit integer.
        /// </summary>
        public long Value;

        /// <summary>
        /// Parse the PtypInteger64 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypInteger64 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadINT64();
        }
    }

    /// <summary>
    /// Variable size; a string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
    /// </summary>
    public class PtypString : BaseStructure
    {
        /// <summary>
        /// A string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
        /// </summary>
        public MAPIString Value;

        /// <summary>
        /// The length value
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the PtypString class
        /// </summary>
        /// <param name="len">The length parameter</param>
        public PtypString(int len = 0)
        {
            this.length = len;
        }

        /// <summary>
        /// Parse the PtypString structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypString structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = new MAPIString(Encoding.Unicode, "\0", this.length);
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// Variable size; a string of multibyte characters in externally specified encoding with terminating null character (single 0 byte).
    /// </summary>
    public class PtypString8 : BaseStructure
    {
        /// <summary>
        /// A string of multibyte characters in externally specified encoding with terminating null character (single 0 byte).
        /// </summary>
        public MAPIString Value;

        /// <summary>
        /// The length value
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        /// <param name="len">The length parameter</param>
        public PtypString8(int len = 0)
        {
            this.length = len;
        }

        /// <summary>
        /// Parse the PtypString8 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypString8 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = new MAPIString(Encoding.ASCII, "\0", this.length);
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// 8 bytes; a 64-bit integer representing the number of 100-nanosecond intervals since January 1, 1601.[MS-DTYP]: FILETIME.
    /// </summary>
    public class PtypTime : BaseStructure
    {
        /// <summary>
        /// 64-bit integer representing the number of 100-nanosecond intervals since January 1, 1601.[MS-DTYP]: FILETIME.
        /// </summary>
        public DateTime Value;

        /// <summary>
        /// Parse the PtypTime structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypTime structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            try
            {
                ulong temp = this.ReadUlong();
                DateTime startdate = new DateTime(1601, 1, 1).AddMilliseconds(temp / 10000);
                this.Value = startdate.ToLocalTime();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Used to deal special date of PidTagMessageDeliveryTime property
                this.Value = new DateTime();
            }
        }
    }

    /// <summary>
    /// 16 bytes; a GUID with Data1, Data2, and Data3 fields in little-endian format.[MS-DTYP]: GUID.
    /// </summary>
    public class PtypGuid : BaseStructure
    {
        /// <summary>
        /// A GUID value.
        /// </summary>
        public Guid Value;

        /// <summary>
        /// Parse the PtypGuid structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypGuid structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadGuid();
        }
    }

    /// <summary>
    /// Variable size; a 16-bit COUNT field followed by a structure as specified in section 2.11.1.4.
    /// </summary>
    public class PtypServerId : BaseStructure
    {
        /// <summary>
        /// The COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The value 0x01 indicates the remaining bytes conform to this structure; 
        /// </summary>
        public byte Ours;

        /// <summary>
        /// A Folder ID structure, as specified in section 2.2.1.1.
        /// </summary>
        public FolderID FolderID;

        /// <summary>
        /// A Message ID structure, as specified in section 2.2.1.2, identifying a message in a folder identified by an associated folder ID. 
        /// </summary>
        public MessageID MessageID;

        /// <summary>
        /// An unsigned instance number within an array of ServerIds to compare against. 
        /// </summary>
        public uint? Instance;

        /// <summary>
        /// The Ours value 0x00 indicates this is a client-defined value and has whatever size and structure the client has defined.
        /// </summary>
        public byte?[] ClientData;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypServerId class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypBinary type.</param>
        public PtypServerId(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypServerId structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypServerId structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            this.Ours = this.ReadByte();
            if (this.Ours == 0x01)
            {
                this.FolderID = new FolderID();
                this.FolderID.Parse(s);
                this.MessageID = new MessageID();
                this.MessageID.Parse(s);
                this.Instance = this.ReadUint();
            }
            else
            {
                this.ClientData = this.ConvertArray(this.ReadBytes(this.Count.GetHashCode() - 1));
            }
        }
    }

    /// <summary>
    /// Variable size; a byte array representing one or more Restriction structures as specified in section 2.12.
    /// </summary>
    public class PtypRestriction : RestrictionType
    {
        // None, class PtypRestriction is same as RestrictionType.
    }

    /// <summary>
    /// Variable size; a 16-bit COUNT field followed by that many rule action structures, as specified in [MS-OXORULE] section 2.2.5.
    /// </summary>
    public class PtypRuleAction : RuleAction
    {
        // None, class PtypRuleAction is same as RuleAction.
    }

    /// <summary>
    /// The help method to read the Count of Ptyp data type
    /// </summary>
    public class HelpMethod : BaseStructure
    {
        /// <summary>
        /// The method to read the Count of Ptyp type.
        /// </summary>
        /// <param name="countWide">The count wide.</param>
        /// <param name="s">The stream contain the COUNT</param>
        /// <returns>The COUNT value.</returns>
        public object ReadCount(CountWideEnum countWide, Stream s)
        {
            this.Parse(s);

            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    {
                        return this.ReadUshort();
                    }

                case CountWideEnum.fourBytes:
                    {
                        return this.ReadUint();
                    }

                default:
                    return this.ReadUshort();
            }
        }

        /// <summary>
        /// Format the error codes.
        /// </summary>
        /// <param name="errorCodeUint">The UInt error code</param>
        /// <returns>The enum error code name.</returns>
        public object FormatErrorCode(uint errorCodeUint)
        {
            object errorCode = null;
            if (Enum.IsDefined(typeof(ErrorCodes), errorCodeUint))
            {
                errorCode = (ErrorCodes)errorCodeUint;
            }
            else if (Enum.IsDefined(typeof(AdditionalErrorCodes), errorCodeUint))
            {
                errorCode = (AdditionalErrorCodes)errorCodeUint;
            }
            else if (Enum.IsDefined(typeof(WarningCodes), errorCodeUint))
            {
                errorCode = (WarningCodes)errorCodeUint;
            }
            else
            {
                errorCode = errorCodeUint;
            }

            return errorCode;
        }

        /// <summary>
        /// Override parse method.
        /// </summary>
        /// <param name="s">Stream used to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many bytes.
    /// </summary>
    public class PtypBinary : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The binary value.
        /// </summary>
        public byte[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypBinary class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypBinary type.</param>
        public PtypBinary(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypBinary structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypBinary structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            this.Value = this.ReadBytes(this.Count.GetHashCode());
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypInteger16 values.
    /// </summary>
    public class PtypMultipleInteger16 : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// Workaround, need to update once the COUNT wide of PtypMultipleBinary is confirmed.
        /// </summary>
        public ushort UndefinedCount;

        /// <summary>
        /// The Int16 value.
        /// </summary>
        public short[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleInteger16 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleInteger16 type.</param>
        public PtypMultipleInteger16(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleInteger16 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleInteger16 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            this.UndefinedCount = this.ReadUshort();
            List<short> tempvalue = new List<short>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadINT16());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypInteger32 values.
    /// </summary>
    public class PtypMultipleInteger32 : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// Workaround, need to update once the COUNT wide of PtypMultipleBinary is confirmed.
        /// </summary>
        public ushort UndefinedCount;

        /// <summary>
        ///  The Int32 value.
        /// </summary>
        public int[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleInteger32 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleInteger32 type.</param>
        public PtypMultipleInteger32(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleInteger32 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleInteger32 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            this.UndefinedCount = this.ReadUshort();
            List<int> tempvalue = new List<int>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadINT32());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypFloating32 values.
    /// </summary>
    public class PtypMultipleFloating32 : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The float value.
        /// </summary>
        public float[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleFloating32 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleFloating32 type.</param>
        public PtypMultipleFloating32(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleFloating32 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleFloating32 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<float> tempvalue = new List<float>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadINT32());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypFloating64 values.
    /// </summary>
    public class PtypMultipleFloating64 : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of double value.
        /// </summary>
        public double[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleFloating64 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleFloating64 type.</param>
        public PtypMultipleFloating64(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleFloating64 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleFloating64 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<double> tempvalue = new List<double>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadINT64());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypCurrency values.
    /// </summary>
    public class PtypMultipleCurrency : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of Int64 value.
        /// </summary>
        public long[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        ///  Initializes a new instance of the PtypMultipleCurrency class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleCurrency type.</param>
        public PtypMultipleCurrency(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleCurrency structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleCurrency structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<long> tempvalue = new List<long>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadINT64());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypFloatingTime values.
    /// </summary>
    public class PtypMultipleFloatingTime : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of double value.
        /// </summary>
        public double[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleFloatingTime class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleFloatingTime type.</param>
        public PtypMultipleFloatingTime(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleFloatingTime structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleFloatingTime structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<double> tempvalue = new List<double>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadINT64());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypInteger64 values.
    /// </summary>
    public class PtypMultipleInteger64 : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of Int64 value.
        /// </summary>
        public long[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleInteger64 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleInteger64 type.</param>
        public PtypMultipleInteger64(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleInteger64 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleInteger64 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<long> tempvalue = new List<long>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadINT64());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypString values.
    /// </summary>
    public class PtypMultipleString : BaseStructure
    {
        /// <summary>
        /// Count values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The undefined count field
        /// </summary>
        public object UndefinedCount;

        /// <summary>
        /// The array of string value.
        /// </summary>
        public MAPIString[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleString class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleString type.</param>
        public PtypMultipleString(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleString structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleString structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            byte temp = this.ReadByte();
            if (temp == 0xff)
            {
                this.UndefinedCount = 0xff;
            }
            else
            {
                s.Position -= 1;
                this.UndefinedCount = this.ReadUshort();
            }

            List<MAPIString> tempvalue = new List<MAPIString>();
            MAPIString str;
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                str = new MAPIString(Encoding.Unicode);
                str.Parse(s);
                tempvalue.Add(str);
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypString values.
    /// </summary>
    public class PtypMultipleString_AddressBook : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of string value.
        /// </summary>
        public MAPIStringAddressBook[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleString_AddressBook class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleString type.</param>
        public PtypMultipleString_AddressBook(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleString_AddressBook structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleString_AddressBook structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<MAPIStringAddressBook> tempvalue = new List<MAPIStringAddressBook>();
            MAPIStringAddressBook str;
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                str = new MAPIStringAddressBook(Encoding.Unicode);
                str.Parse(s);
                tempvalue.Add(str);
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypString8 values.
    /// </summary>
    public class PtypMultipleString8 : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of string value.
        /// </summary>
        public MAPIString[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleString8 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleString8 type.</param>
        public PtypMultipleString8(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleString8 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleString8 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<MAPIString> tempvalue = new List<MAPIString>();
            MAPIString str;
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                str = new MAPIString(Encoding.ASCII);
                str.Parse(s);
                tempvalue.Add(str);
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypTime values.
    /// </summary>
    public class PtypMultipleTime : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of time value.
        /// </summary>
        public PtypTime[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleTime class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleTime type.</param>
        public PtypMultipleTime(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleTime structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleTime structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<PtypTime> tempvalue = new List<PtypTime>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                PtypTime time = new PtypTime();
                time.Parse(s);
                tempvalue.Add(time);
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypGuid values.
    /// </summary>
    public class PtypMultipleGuid : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The array of GUID value.
        /// </summary>
        public Guid[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleGuid class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleGuid type.</param>
        public PtypMultipleGuid(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleGuid structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleGuid structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            List<Guid> tempvalue = new List<Guid>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(this.ReadGuid());
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypBinary values.
    /// </summary>
    public class PtypMultipleBinary : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// Workaround, need to update once the COUNT wide of PtypMultipleBinary is confirmed.
        /// </summary>
        public object UndefinedCount;

        /// <summary>
        /// The array of binary value.
        /// </summary>
        public PtypBinary[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleBinary class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleBinary type.</param>
        public PtypMultipleBinary(CountWideEnum wide)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleBinary structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypMultipleBinary structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.Count = help.ReadCount(this.countWide, s);
            byte nextbyte = ReadByte();
            s.Position -= 1;
            if (nextbyte == 0xff)
            {
                this.UndefinedCount = this.ReadByte();
            }
            else
            {
                this.UndefinedCount = this.ReadUshort();
            }

            List<PtypBinary> tempvalue = new List<PtypBinary>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                PtypBinary binary = new PtypBinary(this.countWide);
                binary.Parse(s);
                tempvalue.Add(binary);
            }

            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Any: this property type value matches any type; 
    /// </summary>
    public class PtypUnspecified : BaseStructure
    {
        /// <summary>
        /// Initializes a new instance of the PtypUnspecified class
        /// </summary>
        public PtypUnspecified()
        {
            throw new Exception("MSOXCDATA: Not implemented type definition - PtypUnspecified");
        }
    }

    /// <summary>
    /// None: This property is a placeholder.
    /// </summary>
    public class PtypNull : BaseStructure
    {
        /// <summary>
        /// The null value.
        /// </summary>
        public MAPIString Value;

        /// <summary>
        /// Parse the PtypNull structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypNull structure</param>
        public override void Parse(Stream s)
        {
            this.Value = null;
        }
    }

    /// <summary>
    /// IN FUTURE: How to distinguish PtypObject from PtypEmbeddedTable since they share the same value
    /// </summary>
    public class PtypObject_Or_PtypEmbeddedTable : BaseStructure
    {
        /// <summary>
        /// Initializes a new instance of the PtypObject_Or_PtypEmbeddedTable class
        /// </summary>
        public PtypObject_Or_PtypEmbeddedTable()
        {
            throw new Exception("MSOXCDATA: Not implemented type definition - PtypObject_Or_PtypEmbeddedTable");
        }
    }

    #endregion

    #region 2.11.2	Property Value Structures

    /// <summary>
    /// 2.11.2 Property Value Structures
    /// </summary>
    public class PropertyValue : BaseStructure
    {
        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2. The value MUST be compatible with the value of the propertyType field.
        /// </summary>
        public object Value;

        /// <summary>
        /// The Count wide size of ptypMutiple type.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        /// </summary>
        private PropertyDataType propertyType;

        /// <summary>
        /// Boole value indicates if this property value is for address book.
        /// </summary>
        private bool isAddressBook;

        /// <summary>
        /// Initializes a new instance of the PropertyValue class
        /// </summary>
        /// <param name="proType">The property type</param>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type</param>
        /// <param name="addressBook">Whether it is AddressBook related property</param>
        public PropertyValue(PropertyDataType proType, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes, bool addressBook = false)
        {
            this.countWide = ptypMultiCountSize;
            this.propertyType = proType;
            this.isAddressBook = addressBook;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyValue class.
        /// </summary>
        /// <param name="addressBook">The AddressBook</param>
        public PropertyValue(bool addressBook = false)
        {
            this.isAddressBook = addressBook;
        }

        /// <summary>
        /// Parse the PropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyValue structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadPropertyValue(this.propertyType, s, this.countWide);
        }

        /// <summary>
        /// The method to return the object of PropertyValue.
        /// </summary>
        /// <param name="dataType">The Property data type.</param>
        /// <param name="s">A stream containing the PropertyValue structure</param>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        /// <returns>The object of PropertyValue.</returns>
        public object ReadPropertyValue(PropertyDataType dataType, Stream s, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            base.Parse(s);
            object propertyValue;
            switch (dataType)
            {
                case PropertyDataType.PtypInteger16:
                    {
                        PtypInteger16 tempPropertyValue = new PtypInteger16();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypInteger32:
                    {
                        PtypInteger32 tempPropertyValue = new PtypInteger32();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypFloating32:
                    {
                        PtypFloating32 tempPropertyValue = new PtypFloating32();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypFloating64:
                    {
                        PtypFloating64 tempPropertyValue = new PtypFloating64();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypCurrency:
                    {
                        PtypCurrency tempPropertyValue = new PtypCurrency();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypFloatingTime:
                    {
                        PtypFloatingTime tempPropertyValue = new PtypFloatingTime();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypErrorCode:
                    {
                        PtypErrorCode tempPropertyValue = new PtypErrorCode();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypBoolean:
                    {
                        PtypBoolean tempPropertyValue = new PtypBoolean();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypInteger64:
                    {
                        PtypInteger64 tempPropertyValue = new PtypInteger64();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypString:
                    {
                        PtypString tempPropertyValue = new PtypString();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypString8:
                    {
                        PtypString8 tempPropertyValue = new PtypString8();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypTime:
                    {
                        PtypTime tempPropertyValue = new PtypTime();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypGuid:
                    {
                        PtypGuid tempPropertyValue = new PtypGuid();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypServerId:
                    {
                        PtypServerId tempPropertyValue = new PtypServerId(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypRestriction:
                    {
                        PtypRestriction tempPropertyValue = new PtypRestriction();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypRuleAction:
                    {
                        PtypRuleAction tempPropertyValue = new PtypRuleAction();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypUnspecified:
                    {
                        PtypUnspecified tempPropertyValue = new PtypUnspecified();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypNull:
                    {
                        PtypNull tempPropertyValue = new PtypNull();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypBinary:
                    {
                        PtypBinary tempPropertyValue = new PtypBinary(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleInteger16:
                    {
                        PtypMultipleInteger16 tempPropertyValue = new PtypMultipleInteger16(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleInteger32:
                    {
                        PtypMultipleInteger32 tempPropertyValue = new PtypMultipleInteger32(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleFloating32:
                    {
                        PtypMultipleFloating32 tempPropertyValue = new PtypMultipleFloating32(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleFloating64:
                    {
                        PtypMultipleFloating64 tempPropertyValue = new PtypMultipleFloating64(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleCurrency:
                    {
                        PtypMultipleCurrency tempPropertyValue = new PtypMultipleCurrency(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleFloatingTime:
                    {
                        PtypMultipleFloatingTime tempPropertyValue = new PtypMultipleFloatingTime(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleInteger64:
                    {
                        PtypMultipleInteger64 tempPropertyValue = new PtypMultipleInteger64(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleString:
                    {
                        if (this.isAddressBook)
                        {
                            PtypMultipleString_AddressBook tempPropertyValue = new PtypMultipleString_AddressBook(ptypMultiCountSize);
                            tempPropertyValue.Parse(s);
                            propertyValue = tempPropertyValue;
                        }
                        else
                        {
                            PtypMultipleString tempPropertyValue = new PtypMultipleString(ptypMultiCountSize);
                            tempPropertyValue.Parse(s);
                            propertyValue = tempPropertyValue;
                        }

                        break;
                    }

                case PropertyDataType.PtypMultipleString8:
                    {
                        PtypMultipleString8 tempPropertyValue = new PtypMultipleString8(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleTime:
                    {
                        PtypMultipleTime tempPropertyValue = new PtypMultipleTime(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleGuid:
                    {
                        PtypMultipleGuid tempPropertyValue = new PtypMultipleGuid(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypMultipleBinary:
                    {
                        PtypMultipleBinary tempPropertyValue = new PtypMultipleBinary(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                    {
                        PtypObject_Or_PtypEmbeddedTable tempPropertyValue = new PtypObject_Or_PtypEmbeddedTable();
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
                        break;
                    }

                default:
                    propertyValue = null;
                    break;
            }

            return propertyValue;
        }
    }

    #endregion

    #region 2.11.3	TypedPropertyValue Structure
    /// <summary>
    /// 2.11.3 TypedPropertyValue Structure 
    /// </summary>
    public class TypedPropertyValue : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2. The value MUST be compatible with the value of the propertyType field.
        /// </summary>
        public object PropertyValue;

        /// <summary>
        /// The Count wide size of ptypMutiple type.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the TypedPropertyValue class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type</param>
        public TypedPropertyValue(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the TypedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing the TypedPropertyValue structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = this.ConvertToPropType(this.ReadUshort());
            PropertyValue propertyValue = new PropertyValue();
            this.PropertyValue = propertyValue.ReadPropertyValue(this.PropertyType, s, this.countWide);
        }
    }
    #endregion

    #region 2.11.4	TaggedPropertyValue Structure
    /// <summary>
    /// 2.11.4 TaggedPropertyValue Structure
    /// </summary>
    public class TaggedPropertyValue : BaseStructure
    {
        /// <summary>
        /// A PropertyTag structure, as specified in section 2.9, giving the values of the PropertyId and propertyType fields for the property.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2.1. specifying the value of the property. 
        /// </summary>
        public object PropertyValue;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// A propertyTag structure, used for PropertyRestriction
        /// </summary>
        private PropertyTag tagInRestriction;

        /// <summary>
        /// Initializes a new instance of the TaggedPropertyValue class
        /// </summary>
        /// <param name="ptypMultiCountSize">The count size of multiple property</param>
        /// <param name="propertyTag">The PropertyTag structure</param>
        public TaggedPropertyValue(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes, PropertyTag propertyTag = null)
        {
            this.countWide = ptypMultiCountSize;
            this.tagInRestriction = propertyTag;
        }

        /// <summary>
        /// Parse the TaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing the TaggedPropertyValue structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            PropertyValue propertyValue = new PropertyValue();
            if (this.tagInRestriction != null)
            {
                if (((ushort)this.tagInRestriction.PropertyType & 0x1000) == 0x1000)
                {
                    this.tagInRestriction.PropertyType = (PropertyDataType)((ushort)this.tagInRestriction.PropertyType & 0xfff);
                }

                this.PropertyValue = propertyValue.ReadPropertyValue(this.tagInRestriction.PropertyType, s, this.countWide);
            }
            else
            {
                this.PropertyValue = propertyValue.ReadPropertyValue(this.PropertyTag.PropertyType, s, this.countWide);
            }
        }
    }
    #endregion

    #region 2.11.5	FlaggedPropertyValue Structure
    /// <summary>
    /// 2.11.5 FlaggedPropertyValue Structure
    /// </summary>
    public class FlaggedPropertyValue : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value of this flag determines what is conveyed in the PropertyValue field. 
        /// </summary>
        public byte Flag;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2.1, unless the Flag field is set to 0x1.
        /// </summary>
        public object PropertyValue;

        /// <summary>
        /// The Property data type.
        /// </summary>
        private PropertyDataType propertyType;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the FlaggedPropertyValue class
        /// </summary>
        /// <param name="propertyType">The Property data type.</param>
        /// <param name="ptypMultiCountSize">The Count wide size.</param>
        public FlaggedPropertyValue(PropertyDataType propertyType, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            this.propertyType = propertyType;
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the FlaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing the FlaggedPropertyValue structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flag = this.ReadByte();
            if (this.Flag == 0x00)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(this.propertyType, s, this.countWide);
            }
            else if (this.Flag == 0x0A)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(PropertyDataType.PtypErrorCode, s, this.countWide);
            }
            else
            {
                this.PropertyValue = null;
            }
        }
    }
    #endregion

    #region 2.11.6	FlaggedPropertyValueWithType Structure
    /// <summary>
    /// 2.11.6 FlaggedPropertyValueWithType Structure
    /// </summary>
    public class FlaggedPropertyValueWithType : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer. This flag MUST be set one of three possible values: 0x0, 0x1, or 0xA, which determines what is conveyed in the PropertyValue field. 
        /// </summary>
        public byte Flag;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2.1, unless the Flag field is set to 0x1. 
        /// </summary>
        public object PropertyValue;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        ///  Initializes a new instance of the FlaggedPropertyValueWithType class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size.</param>
        public FlaggedPropertyValueWithType(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the FlaggedPropertyValueWithType structure.
        /// </summary>
        /// <param name="s">A stream containing the FlaggedPropertyValueWithType structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = this.ConvertToPropType(this.ReadUshort());
            this.Flag = this.ReadByte();
            if (this.Flag == 0x00)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(this.PropertyType, s, this.countWide);
            }
            else if (this.Flag == 0x0A)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(PropertyDataType.PtypErrorCode, s, this.countWide);
            }
            else
            {
                this.PropertyValue = null;
            }
        }
    }
    #endregion

    #region 2.11.7	TypedString Structure

    /// <summary>
    /// 2.11.7 TypedString Structure
    /// </summary>
    public class TypedString : BaseStructure
    {
        /// <summary>
        /// An enum value of StringType
        /// </summary>
        public StringTypeEnum StringType;

        /// <summary>
        /// If the StringType field is set to 0x02, 0x03, or 0x04, then this field MUST be present and in the format specified by the Type field. Otherwise, this field MUST NOT be present.
        /// </summary>
        public MAPIString String;

        /// <summary>
        /// Parse the TypedString structure.
        /// </summary>
        /// <param name="s">A stream containing the TypedString structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.StringType = (StringTypeEnum)ReadByte();
            switch (this.StringType)
            {
                case StringTypeEnum.NoPresent:
                case StringTypeEnum.Empty:
                    {
                        this.String = null;
                        break;
                    }

                case StringTypeEnum.CharacterString:
                    {
                        this.String = new MAPIString(Encoding.ASCII);
                        this.String.Parse(s);
                        break;
                    }

                case StringTypeEnum.ReducedUnicodeCharacterString:
                    {
                        this.String = new MAPIString(Encoding.ASCII);
                        this.String.Parse(s);
                        break;
                    }

                case StringTypeEnum.UnicodeCharacterString:
                    {
                        this.String = new MAPIString(Encoding.Unicode);
                        this.String.Parse(s);
                        break;
                    }

                default:
                    break;
            }
        }
    }
    #endregion
    #endregion

    #region 2.12   Restrictions

    /// <summary>
    ///  2.12   Restrictions
    /// </summary>
    public class RestrictionType : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction.
        /// </summary>
        public object Restriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the RestrictionType class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public RestrictionType(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the RestrictionType structure.
        /// </summary>
        /// <param name="s">A stream containing the RestrictionType structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RestrictTypeEnum tempRestrictType = (RestrictTypeEnum)ReadByte();
            s.Position -= 1;
            switch (tempRestrictType)
            {
                case RestrictTypeEnum.AndRestriction:
                    {
                        AndRestriction restriction = new AndRestriction(this.countWide);
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.OrRestriction:
                    {
                        OrRestriction restriction = new OrRestriction(this.countWide);
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.NotRestriction:
                    {
                        NotRestriction restriction = new NotRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.ContentRestriction:
                    {
                        ContentRestriction restriction = new ContentRestriction(this.countWide);
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.PropertyRestriction:
                    {
                        PropertyRestriction restriction = new PropertyRestriction(this.countWide);
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.ComparePropertiesRestriction:
                    {
                        ComparePropertiesRestriction restriction = new ComparePropertiesRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.BitMaskRestriction:
                    {
                        BitMaskRestriction restriction = new BitMaskRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.SizeRestriction:
                    {
                        SizeRestriction restriction = new SizeRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.ExistRestriction:
                    {
                        ExistRestriction restriction = new ExistRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.SubObjectRestriction:
                    {
                        SubObjectRestriction restriction = new SubObjectRestriction(this.countWide);
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.CommentRestriction:
                    {
                        CommentRestriction restriction = new CommentRestriction(this.countWide);
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.CountRestriction:
                    {
                        CountRestriction restriction = new CountRestriction(this.countWide);
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 2.12.1 And Restriction Structures
    /// </summary>
    public class AndRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x00.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// This value specifies how many restriction structures are present in the Restricts field. The width of this field is 16 bits in the context of ROPs and 32 bits in the context of extended rules.
        /// </summary>
        public object RestrictCount;

        /// <summary>
        /// An array of restriction structures. 
        /// </summary>
        public RestrictionType[] Restricts;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the AndRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public AndRestriction(CountWideEnum ptypMultiCountSize)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the AndRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the AndRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            if (this.countWide == CountWideEnum.twoBytes)
            {
                this.RestrictCount = this.ReadUshort();
            }
            else
            {
                this.RestrictCount = this.ReadUint();
            }

            List<RestrictionType> tempRestricts = new List<RestrictionType>();
            for (int length = 0; length < this.RestrictCount.GetHashCode(); length++)
            {
                RestrictionType tempRestriction = new RestrictionType();
                tempRestriction.Parse(s);
                tempRestricts.Add(tempRestriction);
            }

            this.Restricts = tempRestricts.ToArray();
        }
    }

    /// <summary>
    /// 2.12.2.1 OrRestriction Structure
    /// </summary>
    public class OrRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x01.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// This value specifies how many restriction structures are present in the Restricts field. The width of this field is 16 bits in the context of ROPs and 32 bits in the context of extended rules.
        /// </summary>
        public object RestrictCount;

        /// <summary>
        /// An array of restriction structures. This field MUST contain the number of structures indicated by the RestrictCount field.
        /// </summary>
        public RestrictionType[] Restricts;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the OrRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public OrRestriction(CountWideEnum ptypMultiCountSize)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the OrRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the OrRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)this.ReadByte();
            if (this.countWide == CountWideEnum.twoBytes)
            {
                this.RestrictCount = this.ReadUshort();
            }
            else
            {
                this.RestrictCount = this.ReadUint();
            }

            List<RestrictionType> tempRestricts = new List<RestrictionType>();
            for (int length = 0; length < this.RestrictCount.GetHashCode(); length++)
            {
                RestrictionType tempRestriction = new RestrictionType();
                tempRestriction.Parse(s);
                tempRestricts.Add(tempRestriction);
            }

            this.Restricts = tempRestricts.ToArray();
        }
    }

    /// <summary>
    /// 2.12.3 Not Restriction Structures
    /// </summary>
    public class NotRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x02.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// A restriction structure. This value specifies the restriction (2) that the logical NOT operation applies to.
        /// </summary>
        public RestrictionType Restriction;

        /// <summary>
        /// Parse the NotRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the NotRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.Restriction = new RestrictionType();
            this.Restriction.Parse(s);
        }
    }

    /// <summary>
    /// 2.12.4 Content Restriction Structures
    /// </summary>
    public class ContentRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x03.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// An unsigned integer. This field specifies the level of precision that the server enforces when checking for a match against a ContentRestriction structure.
        /// </summary>
        public FuzzyLevelLowEnum FuzzyLevelLow;

        /// <summary>
        /// This field applies only to string-value properties. 
        /// </summary>
        public FuzzyLevelHighEnum FuzzyLevelHigh;

        /// <summary>
        /// This value indicates the property tag of the column whose value MUST be matched against the value specified in the TaggedValue field.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A TaggedPropertyValue structure, as specified in section 2.11.4. 
        /// </summary>
        public TaggedPropertyValue TaggedValue;

        /// <summary>
        ///  The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the ContentRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public ContentRestriction(CountWideEnum ptypMultiCountSize)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the ContentRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the ContentRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.FuzzyLevelLow = (FuzzyLevelLowEnum)ReadUshort();
            this.FuzzyLevelHigh = (FuzzyLevelHighEnum)ReadUshort();
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            this.TaggedValue = new TaggedPropertyValue(this.countWide, this.PropertyTag);
            this.TaggedValue.Parse(s);
        }
    }

    /// <summary>
    /// 2.12.5 Property Restriction Structures
    /// </summary>
    public class PropertyRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x4.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// An unsigned integer. This value indicates the relational operator that is used to compare the property on the object with the value of the TaggedValue field. 
        /// </summary>
        public RelOpType RelOp;

        /// <summary>
        /// An unsigned integer. This value indicates the property tag of the property that MUST be compared.
        /// </summary>
        public uint PropTag;

        /// <summary>
        ///  A TaggedValue structure, as specified in section 2.11.4. 
        /// </summary>
        public TaggedPropertyValue TaggedValue;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PropertyRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public PropertyRestriction(CountWideEnum ptypMultiCountSize)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the PropertyRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)this.ReadByte();
            this.RelOp = (RelOpType)this.ReadByte();
            this.PropTag = this.ReadUint();
            s.Position -= 4;
            PropertyTag propertyTag = new PropertyTag();
            propertyTag.Parse(s);
            this.TaggedValue = new TaggedPropertyValue(this.countWide, propertyTag);
            this.TaggedValue.Parse(s);
        }
    }

    /// <summary>
    /// 2.12.6 Compare Properties Restriction Structures
    /// </summary>
    public class ComparePropertiesRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x05.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// An unsigned integer. This value indicates the relational operator used to compare the two properties. 
        /// </summary>
        public RelOpType RelOp;

        /// <summary>
        /// An unsigned integer. This value is the property tag of the first property that MUST be compared.
        /// </summary>
        public uint PropTag1;

        /// <summary>
        /// An unsigned integer. This value is the property tag of the second property that MUST be compared.
        /// </summary>
        public uint PropTag2;

        /// <summary>
        /// Parse the ComparePropertiesRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the ComparePropertiesRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.RelOp = (RelOpType)ReadByte();
            this.PropTag1 = this.ReadUint();
            this.PropTag2 = this.ReadUint();
        }
    }

    /// <summary>
    /// 2.12.7 Bitmask Restriction Structures
    /// </summary>
    public class BitMaskRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x06.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// An unsigned integer. This value specifies how the server MUST perform the masking operation. 
        /// </summary>
        public BitmapRelOpType BitmapRelOp;

        /// <summary>
        /// An unsigned integer. This value is the property tag of the property to be tested. 
        /// </summary>
        public PtypInteger32 PropTag;

        /// <summary>
        /// An unsigned integer. The bitmask to be used for the AND operation.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// Parse the BitMaskRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the BitMaskRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.BitmapRelOp = (BitmapRelOpType)ReadByte();
            this.PropTag = new PtypInteger32();
            this.PropTag.Parse(s);
            this.Mask = this.ReadUint();
        }
    }

    /// <summary>
    /// 2.12.8 Size Restriction Structures
    /// </summary>
    public class SizeRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x07.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        ///  An unsigned integer. This value indicates the relational operator used in the size comparison.
        /// </summary>
        public RelOpType RelOp;

        /// <summary>
        /// An unsigned integer. This value indicates the property tag of the property whose value size is being tested.
        /// </summary>
        public uint PropTag;

        /// <summary>
        /// An unsigned integer. This value indicates the size, in bytes, that is to be used in the comparison.
        /// </summary>
        public uint Size;

        /// <summary>
        /// Parse the SizeRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the SizeRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)this.ReadByte();
            this.RelOp = (RelOpType)this.ReadByte();
            this.PropTag = this.ReadUint();
            this.Size = this.ReadUint();
        }
    }

    /// <summary>
    /// 2.12.9 Exist Restriction Structures
    /// </summary>
    public class ExistRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x08.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// This value encodes the PropTag field of the SizeRestriction structure. 
        /// </summary>
        public uint PropTag;

        /// <summary>
        /// Parse the ExistRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the ExistRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.PropTag = this.ReadUint();
        }
    }

    /// <summary>
    /// 2.12.10 subObject Restriction Structures
    /// </summary>
    public class SubObjectRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x09.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// An unsigned integer. This value is a property tag that designates the target of the subrestriction. 
        /// </summary>
        public uint Subobject;

        /// <summary>
        /// A Restriction structure. 
        /// </summary>
        public RestrictionType Restriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        ///  Initializes a new instance of the SubObjectRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public SubObjectRestriction(CountWideEnum ptypMultiCountSize)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the SubObjectRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the SubObjectRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)this.ReadByte();
            this.Subobject = this.ReadUint();
            this.Restriction = new RestrictionType(this.countWide);
            this.Restriction.Parse(s);
        }
    }

    /// <summary>
    /// 2.12.11 CommentRestriction Structure
    /// </summary>
    public class CommentRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x0A.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// An unsigned integer. This value specifies how many TaggedValue structures are present in the TaggedValues field.
        /// </summary>
        public byte TaggedValuesCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures, as specified in section 2.11.4. 
        /// </summary>
        public TaggedPropertyValue[] TaggedValues;

        /// <summary>
        /// An unsigned integer. This field MUST contain either TRUE (0x01) or FALSE (0x00). 
        /// </summary>
        public bool RestrictionPresent;

        /// <summary>
        /// A Restriction structure. This field is present only if RestrictionPresent is TRUE.
        /// </summary>
        public RestrictionType Restriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the CommentRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public CommentRestriction(CountWideEnum ptypMultiCountSize)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the CommentRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the CommentRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.TaggedValuesCount = this.ReadByte();
            List<TaggedPropertyValue> tempTaggedValue = new List<TaggedPropertyValue>();
            for (int i = 0; i < this.TaggedValuesCount; i++)
            {
                TaggedPropertyValue tempproperty = new TaggedPropertyValue(this.countWide);
                tempproperty.Parse(s);
                tempTaggedValue.Add(tempproperty);
            }

            this.TaggedValues = tempTaggedValue.ToArray();
            this.RestrictionPresent = this.ReadBoolean();
            if (this.RestrictionPresent == true)
            {
                this.Restriction = new RestrictionType(this.countWide);
                this.Restriction.Parse(s);
            }
        }
    }

    /// <summary>
    /// 2.12.12 CountRestriction Structure
    /// </summary>
    public class CountRestriction : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x0B.
        /// </summary>
        public RestrictTypeEnum RestrictType;

        /// <summary>
        /// An unsigned integer. This value specifies the limit on the number of matches to be returned when the value of the SubRestriction field is evaluated.
        /// </summary>
        public uint Count;

        /// <summary>
        /// A restriction structure. This field specifies the restriction (2) to be limited.
        /// </summary>
        public RestrictionType SubRestriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the CountRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public CountRestriction(CountWideEnum ptypMultiCountSize)
        {
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the CountRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the CountRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)this.ReadByte();
            this.Count = this.ReadUint();
            this.SubRestriction = new RestrictionType(this.countWide);
            this.SubRestriction.Parse(s);
        }
    }
    #endregion

    #region 2.13	Table Sorting Structures

    /// <summary>
    /// 2.13.1 sortOrder Structure
    /// </summary>
    public class SortOrder : BaseStructure
    {
        /// <summary>
        /// This value identifies the data type of the column to be used for sorting.
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// This value identifies the column to be used for sorting.
        /// </summary>
        public PidTagPropertyEnum PropertyId;

        /// <summary>
        /// The order type.
        /// </summary>
        public OrderType Order;

        /// <summary>
        /// Parse the sortOrder structure.
        /// </summary>
        /// <param name="s">A stream containing the sortOrder structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)ReadUshort();
            this.PropertyId = (PidTagPropertyEnum)ReadUshort();
            this.Order = (OrderType)ReadByte();
        }
    }

    /// <summary>
    /// 2.13.2 SortOrderSet Structure
    /// </summary>
    public class SortOrderSet : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value specifies how many sortOrder structures are present in the SortOrders field.
        /// </summary>
        public ushort SortOrderCount;

        /// <summary>
        /// An unsigned integer. This value specifies that the first CategorizedCount columns are categorized. 
        /// </summary>
        public ushort CategorizedCount;

        /// <summary>
        /// An unsigned integer. This value specifies that the first ExpandedCount field in the categorized columns starts in an expanded state in which all of the rows that apply to the category are visible in the table view. 
        /// </summary>
        public ushort ExpandedCount;

        /// <summary>
        /// An array of sortOrder structures. This field MUST contain the number of structures indicated by the value of the SortOrderCount field. 
        /// </summary>
        public SortOrder[] SortOrders;

        /// <summary>
        /// Parse the SortOrderSet structure.
        /// </summary>
        /// <param name="s">A stream containing the SortOrderSet structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SortOrderCount = this.ReadUshort();
            this.CategorizedCount = this.ReadUshort();
            this.ExpandedCount = this.ReadUshort();
            List<SortOrder> tempSortOrders = new List<SortOrder>();
            for (int i = 0; i < this.SortOrderCount; i++)
            {
                SortOrder sortOrder = new SortOrder();
                sortOrder.Parse(s);
                tempSortOrders.Add(sortOrder);
            }

            this.SortOrders = tempSortOrders.ToArray();
        }
    }
    #endregion
}
