namespace MAPIInspector.Parsers
{
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
        /// Too many columns requested in RopSetColumns ([MS-OXCROPS] section 2.2.5.1).
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
        /// During RopAbortSubmit ([MS-OXCROPS] section 2.2.7.2), a message was not saved.
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
        /// The collapse state given to RopSetCollapseState ([MS-OXCROPS] section 2.2.5.19) is invalid.
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
        /// The Store object has been locked by the ISINTEG (Information store integrity) utility.
        /// </summary>
        IsintegMDB = 0x0000048C,

        /// <summary>
        /// A recovery storage group operation was attempted on a non-RSG (recovery storage group) Store object, or vice versa.
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
        /// A sequential index counter has reached the maximum value. An offline defragmentation has to be performed to reclaim Free or unused SequentialIndex values.
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
        /// The database engine attempted to replay a CreateBlock Database operation from the transaction log but failed due to an incompatible version of that operation.
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
}
