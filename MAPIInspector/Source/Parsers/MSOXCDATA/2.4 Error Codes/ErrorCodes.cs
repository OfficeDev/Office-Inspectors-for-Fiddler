namespace MAPIInspector.Parsers
{

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
}
