using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MapiInspector;
using System.Reflection;
using Fiddler;
namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MAPIString class to record the related attributes of string.
    /// </summary>
    public class MAPIString : BaseStructure
    {
        // The string value
        public string Value;

        // TDI#76879 tell us the real MapiHttp traffic will add the magic byte 'FF' for the string or binary based property value.
        public byte? MagicNumber;

        // The string Encoding : ASCII or Unicode
        public Encoding Encode;

        // The string Terminator. Default is "\0".
        public string Terminator;

        // If the StringLength is not 0, The StringLength will be as the string length.
        public int StringLength;

        // If the Encoding is Unicode, and it is reduced unicode, it is true.
        public bool ReducedUnicode;

        /// <summary>
        /// A The Constructor of MAPIString without parameters.
        /// </summary>
        public MAPIString()
        { }

        /// <summary>
        /// A The Constructor of MAPIString with parameters.
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="terminator"></param>
        /// <param name="stringLength"></param>
        /// <param name="reducedUnicode"></param>
        public MAPIString(Encoding encode, string terminator = "\0", int stringLength = 0, bool reducedUnicode = false)
        {
            this.Encode = encode;
            this.Terminator = terminator;
            this.StringLength = stringLength;
            this.ReducedUnicode = reducedUnicode;
        }

        public override void Parse(Stream s)
        {
            base.Parse(s);
            if(ReadByte() == 0xff)
            {
                this.MagicNumber = 0xff;
            }
            else
            {
                s.Position -= 1;
            }
            
            this.Value = ReadString(Encode, Terminator, StringLength, ReducedUnicode);
        }
    }

    #region 2.1	AddressList Structures

    /// <summary>
    /// 2.1.1	AddressEntry Structure
    /// </summary>
    public class AddressEntry : BaseStructure
    {
        // An unsigned integer whose value is equal to the number of associated TaggedPropertyValue structures, as specified in section 2.11.4. 
        public UInt32 PropertyCount;

        // A set of TaggedPropertyValue structures representing one addressee.
        public TaggedPropertyValue[] Values;

        /// <summary>
        /// Parse the AddressEntry structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressEntry structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyCount = ReadUint();
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

    ///  <summary>
    ///  2.1.2	AddressList Structure
    /// </summary>
    public class AddressList : BaseStructure
    {
        // An unsigned integer whose value is equal to the number of associated addressees.
        public UInt32 AddressCount;

        // An array of AddressEntry structures. The number of structures is indicated by the AddressCount field.
        public AddressEntry[] Addresses;

        /// <summary>
        /// Parse the AddressList structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressList structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.AddressCount = ReadUint();
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
    /// 2.2.1.1	Folder ID Structure
    /// </summary>
    public class FolderID : BaseStructure
    {
        // An unsigned integer identifying a Store object.
        public ushort ReplicaId;

        // An unsigned integer identifying the folder within its Store object. 6 bytes
        public byte[] GlobalCounter;

        /// <summary>
        /// Parse the FolderID structure.
        /// </summary>
        /// <param name="s">A stream containing the FolderID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ReplicaId = ReadUshort();
            this.GlobalCounter = ReadBytes(6);
        }
    }

    ///  <summary>
    /// 2.2.1.2	Message ID Structure
    /// </summary>
    public class MessageID : BaseStructure
    {
        // An unsigned integer identifying a Store object.
        public ushort ReplicaId;

        // An unsigned integer identifying the message within its Store object. 6 bytes
        public byte[] GlobalCounter;

        /// <summary>
        /// Parse the MessageID structure.
        /// </summary>
        /// <param name="s">A stream containing the MessageID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ReplicaId = ReadUshort();
            this.GlobalCounter = ReadBytes(6);
        }

    }

    /// <summary>
    /// 2.2.1.3.1	LongTermID Structure
    /// </summary>
    public class LongTermID : BaseStructure
    {
        // An unsigned integer identifying a Store object.
        public Guid DatabaseGuid;

        // An unsigned integer identifying the folder or message within its Store object. 6 bytes
        public byte[] GlobalCounter;

        // A 2-byte Pad field. 
        public ushort Pad;

        /// <summary>
        /// Parse the LongTermID structure.
        /// </summary>
        /// <param name="s">A stream containing the LongTermID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.DatabaseGuid = ReadGuid();
            this.GlobalCounter = ReadBytes(6);
            this.Pad = ReadUshort();
        }
    }
    #endregion

    #region 2.2.4	Messaging Object EntryIDs Structures

    /// <summary>
    /// 2.2.4.1	Folder EntryID Structure
    /// </summary>
    public class FolderEntryID : BaseStructure
    {
        // This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid. 
        public uint Flags;

        // The value of this field is determined by where the folder is located. 
        public object ProviderUID;

        // One of several Store object types specified in the table in section 2.2.4.
        public StoreObjectType FolderType;

        // A GUID associated with the Store object and corresponding to the ReplicaId field of the FID structure.
        public Guid DatabaseGuid;

        // An unsigned integer identifying the folder. 6 bytes
        public byte[] GlobalCounter;

        // This value MUST be set to zero.
        public ushort Pad;

        /// <summary>
        /// Parse the FolderEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the FolderEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            byte[] tempProviderUID = ReadBytes(16);
            byte[] VerifyProviderUID = { 0x1A, 0x44, 0x73, 0x90, 0xAA, 0x66, 0x11, 0xCD, 0x9B, 0xC8, 0x00, 0xAA, 0x00, 0x2F, 0xC4, 0x5A };
            if (tempProviderUID == VerifyProviderUID)
            {
                this.ProviderUID = tempProviderUID;
            }
            else
            {
                this.ProviderUID = new Guid(tempProviderUID);
            }
            this.FolderType = (StoreObjectType)ReadUshort();
            this.DatabaseGuid = ReadGuid();
            this.GlobalCounter = ReadBytes(6);
            this.Pad = ReadUshort();
        }
    }

    /// <summary>
    /// The enum of StoreObject type.
    /// </summary>
    public enum StoreObjectType : ushort
    {
        PrivateFolder = 0x0001,
        PublicFolder = 0x0003,
        MappedPublicFolder = 0x0005,
        PrivateMessage = 0x0007,
        PublicMessage = 0x0009,
        MappedPublicMessage = 0x000B,
        PublicNewsgroupFolder = 0x000C
    }

    /// <summary>
    /// 2.2.4.2	Message EntryID Structure
    /// </summary>
    public class MessageEntryID : BaseStructure
    {
        // This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid. 
        public uint Flags;

        // The value of this field is determined by where the folder is located.
        public object ProviderUID;

        // One of several Store object types specified in the table in section 2.2.4.
        public StoreObjectType MessageType;

        // A GUID associated with the Store object of the folder in which the message resides and corresponding to the ReplicaId field in the folder ID structure, as specified in section 2.2.1.1.
        public Guid FolderDatabaseGuid;

        // An unsigned integer identifying the folder in which the message resides. 6 bytes
        public byte[] FolderGlobalCounter;

        // This value MUST be set to zero.
        public ushort Pad_1;

        // A GUID associated with the Store object of the message and corresponding to the ReplicaId field of the Message ID structure, as specified in section 2.2.1.2.
        public Guid MessageDatabaseGuid;

        // An unsigned integer identifying the message. 6 bytes
        public byte[] MessageGlobalCounter;

        // This value MUST be set to zero.
        public ushort Pad_2;

        /// <summary>
        /// Parse the MessageEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the MessageEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            byte[] tempProviderUID = ReadBytes(16);
            if (tempProviderUID.ToString() == "%x1A.44.73.90.AA.66.11.CD.9B.C8.00.AA.00.2F.C4.5A")
            {
                this.ProviderUID = tempProviderUID;
            }
            else
            {
                this.ProviderUID = new Guid(tempProviderUID);
            }
            this.MessageType = (StoreObjectType)ReadUshort();
            this.FolderDatabaseGuid = ReadGuid();
            this.FolderGlobalCounter = ReadBytes(6);
            this.Pad_1 = ReadUshort();
            this.MessageDatabaseGuid = ReadGuid();
            this.MessageGlobalCounter = ReadBytes(6);
            this.Pad_2 = ReadUshort();
        }
    }

    /// <summary>
    /// 2.2.4.3	Store Object EntryID Structure
    /// </summary>
    public class StoreObjectEntryID : BaseStructure
    {
        // This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid. 
        public uint Flags;

        // The identifier for the provider that created the EntryID. 
        public byte[] ProviderUID;

        // This value MUST be set to zero.
        public byte Version;

        // This value MUST be set to zero.
        public byte Flag;

        // This field MUST be set to the following value, which represents "emsmdb.dll": %x45.4D.53.4D.44.42.2E.44.4C.4C.00.00.00.00.
        public byte[] DLLFileName;

        // This value MUST be set to 0x00000000.
        public uint WrappedFlags;

        // This Wrapped Provider UID.
        public byte[] WrappedProviderUID;

        // The value of this field is determined by where the folder is located. 
        public uint WrappedType;

        // A string of single-byte characters terminated by a single zero byte, indicating the short name or NetBIOS name of the server.
        public MAPIString ServerShortname;

        // A string of single-byte characters terminated by a single zero byte and representing the X500 DN of the mailbox, as specified in [MS-OXOAB]. 
        public MAPIString MailboxDN;

        /// <summary>
        /// Parse the StoreObjectEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the StoreObjectEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.ProviderUID = ReadBytes(16);
            this.Version = ReadByte();
            this.Flag = ReadByte();
            this.DLLFileName = ReadBytes(14);
            this.WrappedFlags = ReadUint();
            this.WrappedProviderUID = ReadBytes(16);
            this.WrappedType = ReadUint();
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
        // This value MUST be set to 0x00000000, indicating a long-term EntryID.
        public uint Flags;

        // The identifier for the provider that created the EntryID. 
        public byte[] ProviderUID;

        // This value MUST be set to %x01.00.00.00.
        public uint Version;

        // An integer representing the type of the object. 
        public AddressbookEntryIDtype Type;

        // The X500 DN of the Address Book object. 
        public MAPIString X500DN;

        /// <summary>
        /// Parse the AddressBookEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressBookEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.ProviderUID = ReadBytes(16);
            this.Version = ReadUint();
            this.Type = (AddressbookEntryIDtype)ReadUint();
            this.X500DN = new MAPIString(Encoding.ASCII);
            this.X500DN.Parse(s);
        }
    }

    ///  <summary>
    /// The enum of AddressbookEntryID type.
    /// </summary>
    public enum AddressbookEntryIDtype : uint
    {
        Localmailuser = 0x00000000,
        Distributionlist = 0x00000001,
        Bulletinboardorpublicfolder = 0x00000002,
        Automatedmailbox = 0x00000003,
        Organizationalmailbox = 0x00000004,
        Privatedistributionlist = 0x00000005,
        Remotemailuser = 0x00000006,
        Container = 0x00000100,
        Template = 0x00000101,
        Oneoffuser = 0x00000102,
        Search = 0x00000200
    }
    #endregion
    #endregion

    #region 2.4	Error Codes

    /// <summary>
    /// 2.4	Error Codes
    /// </summary>
    public enum ErrorCodes : uint
    {
        Success = 0x00000000,
        GeneralFailure = 0x80004005,
        OutOfMemory = 0x8007000E,
        InvalidParameter = 0x80070057,
        NoInterface = 0x80004002,
        AccessDenied = 0x80070005,
        StorageInvalidFunction = 0x80030001,
        StorageAccessDenied = 0x80030005,
        StorageInsufficientMemory = 0x80030008,
        StorageInvalidPointer = 0x80030009,
        StorageReadFault = 0x8003001E,
        StorageLockViolation = 0x80030021,
        StorageInvalidParameter = 0x80030057,
        StreamSizeError = 0x80030070,
        StorageInvalidFlag = 0x800300FF,
        StorageCannotSave = 0x80030103,
        NotSupported = 0x80040102,
        InvalidCharacterWidth = 0x80040103,
        StringTooLong = 0x80040105,
        InvalidFlag = 0x80040106,
        InvalidEntryID = 0x80040107,
        InvalidObject = 0x80040108,
        ObjectChanged = 0x80040109,
        ObjectDeleted = 0x8004010A,
        ServerBusy = 0x8004010B,
        OutOfDisk = 0x8004010D,
        OutOfResources = 0x8004010E,
        NotFound = 0x8004010F,
        VersionMismatch = 0x80040110,
        LogonFailed = 0x80040111,
        TooManySessions = 0x80040112,
        UserCanceled = 0x80040113,
        AbortFailed = 0x80040114,
        NetworkError = 0x80040115,
        DiskError = 0x80040116,
        TooComplex = 0x80040117,
        InvalidColumn = 0x80040118,
        ComputedValue = 0x8004011A,
        CorruptData = 0x8004011B,
        InvalidCodepage = 0x8004011E,
        InvalidLocale = 0x8004011F,
        TimeSkew = 0x80040123,
        EndOfSession = 0x80040200,
        UnknownEntryId = 0x80040201,
        NotCompleted = 0x80040400,
        Timeout = 0x80040401,
        EmptyTable = 0x80040402,
        TableTooBig = 0x80040403,
        InvalidBookmark = 0x80040405,
        ErrorWait = 0x80040500,
        ErrorCancel = 0x80040501,
        NoSuppress = 0x80040602,
        CollidingNames = 0x80040604,
        NotInitialized = 0x80040605,
        NoRecipients = 0x80040607,
        AlreadySent = 0x80040608,
        HasFolders = 0x80040609,
        HasMessages = 0x8004060A,
        FolderCycle = 0x8004060B,
        TooManyLocks = 0x8004060D,
        AmbiguousRecipient = 0x80040700,
        SyncObjectDeleted = 0x80040800,
        IgnoreFailure = 0x80040801,
        SyncConflict = 0x80040802,
        NoParentFolder = 0x80040803,
        CycleDetected = 0x80040804,
        NotSynchronized = 0x80040805,
        NamedPropertyQuota = 0x80040900,
        NotImplemented = 0x80040FFF
    }

    ///  <summary>
    /// 2.4.1	Additional Error Codes
    /// </summary>
    public enum AdditionalErrorCodes : uint
    {
        IsamError = 0x000003EA,
        UnknownUser = 0x000003EB,
        Exiting = 0x000003ED,
        BadConfiguration = 0x000003EE,
        UnknownCodePage = 0x000003EF,
        ServerMemory = 0x000003F0,
        LoginPermission = 0x000003F2,
        DatabaseRolledBack = 0x000003F3,
        DatabaseCopiedError = 0x000003F4,
        AuditNotAllowed = 0x000003F5,
        ZombieUser = 0x000003F6,
        UnconvertableACL = 0x000003F7,
        NoFreeJetSessions = 0x0000044C,
        DifferentJetSession = 0x0000044D,
        FileRemove = 0x0000044F,
        ParameterOverflow = 0x00000450,
        BadVersion = 0x00000451,
        TooManyColumns = 0x00000452,
        HaveMore = 0x00000453,
        DatabaseError = 0x00000454,
        IndexNameTooBig = 0x00000455,
        UnsupportedProperty = 0x00000456,
        MessageNotSaved = 0x00000457,
        UnpublishedNotification = 0x00000459,
        DifferentRoot = 0x0000045B,
        BadFolderName = 0x0000045C,
        AttachmentOpen = 0x0000045D,
        InvalidCollapseState = 0x0000045E,
        SkipMyChildren = 0x0000045F,
        SearchFolder = 0x00000460,
        NotSearchFolder = 0x00000461,
        FolderSetReceive = 0x00000462,
        NoReceiveFolder = 0x00000463,
        DeleteSubmittedMessage = 0x00000465,
        InvalidRecipients = 0x00000467,
        NoReplicaHere = 0x00000468,
        NoReplicaAvailable = 0x00000469,
        PublicDatabase = 0x0000046A,
        NotPublicDatabase = 0x0000046B,
        RecordNotFound = 0x0000046C,
        ReplicationConflict = 0x0000046D,
        FXBufferOverrun = 0x00000470,
        FXBufferEmpty = 0x00000471,
        FXPartialValue = 0x00000472,
        FxNoRoom = 0x00000473,
        TimeExpired = 0x00000474,
        DestinationError = 0x00000475,
        DatabaseNotInitialized = 0x00000476,
        WrongServer = 0x00000478,
        BufferTooSmall = 0x0000047D,
        AttachmentResolutionRequired = 0x0000047E,
        ServerPaused = 0x0000047F,
        ServerBusy = 0x00000480,
        NoSuchLogon = 0x00000481,
        LoadLibraryFailed = 0x00000482,
        AlreadyConfigured = 0x00000483,
        NotConfigured = 0x00000484,
        DataLoss = 0x00000485,
        MaximumSendThreadExceeded = 0x00000488,
        FxErrorMarker = 0x00000489,
        NoFreeJtabs = 0x0000048A,
        NotPrivateDatabase = 0x0000048B,
        IsintegMDB = 0x0000048C,
        RecoveryMismatch = 0x0000048D,
        TableMayNotBeDeleted = 0x0000048E,
        SearchFolderScopeViolation = 0x00000490,
        RpcRegisterIf = 0x000004B1,
        RpcListen = 0x000004B2,
        RpcFormat = 0x000004B6,
        NoCopyTo = 0x000004B7,
        NullObject = 0x000004B9,
        RpcAuthentication = 0x000004BC,
        RpcBadAuthenticationLevel = 0x000004BD,
        NullCommentRestriction = 0x000004BE,
        RulesLoadError = 0x000004CC,
        RulesDeliverErr = 0x000004CD,
        RulesParsingErr = 0x000004CE,
        RulesCreateDAE = 0x000004CF,
        RulesCreateDAM = 0x000004D0,
        RulesNoMoveCopyFolder = 0x000004D1,
        RulesNoFolderRights = 0x000004D2,
        MessageTooBig = 0x000004D4,
        FormNotValid = 0x000004D5,
        NotAuthorized = 0x000004D6,
        DeleteMessage = 0x000004D7,
        BounceMessage = 0x000004D8,
        QuotaExceeded = 0x000004D9,
        MaxSubmissionExceeded = 0x000004DA,
        MaxAttachmentExceeded = 0x000004DB,
        SendAsDenied = 0x000004DC,
        ShutoffQuotaExceeded = 0x000004DD,
        TooManyOpenObjects = 0x000004DE,
        ClientVersionBlocked = 0x000004DF,
        RpcHttpDisallowed = 0x000004E0,
        CachedModeRequired = 0x000004E1,
        FolderNotCleanedUp = 0x000004E3,
        FormatError = 0x000004ED,
        NotExpanded = 0x000004F7,
        NotCollapsed = 0x000004F8,
        NoExpandLeafRow = 0x000004F9,
        UnregisteredNameProp = 0x000004FA,
        FolderDisabled = 0x000004FB,
        DomainError = 0x000004FC,
        NoCreateRight = 0x000004FF,
        PublicRoot = 0x00000500,
        NoReadRight = 0x00000501,
        NoCreateSubfolderRight = 0x00000502,
        MessageCycle = 0x00000504,
        NullDestinationObject = 0x00000503,
        TooManyRecips = 0x00000505,
        VirusScanInProgress = 0x0000050A,
        VirusDetected = 0x0000050B,
        MailboxInTransit = 0x0000050C,
        BackupInProgress = 0x0000050D,
        VirusMessageDeleted = 0x0000050E,
        InvalidBackupSequence = 0x0000050F,
        InvalidBackupType = 0x00000510,
        TooManyBackups = 0x00000511,
        RestoreInProgress = 0x00000512,
        DuplicateObject = 0x00000579,
        ObjectNotFound = 0x0000057A,
        FixupReplyRule = 0x0000057B,
        TemplateNotFound = 0x0000057C,
        RuleExecution = 0x0000057D,
        DSNoSuchObject = 0x0000057E,
        AlreadyTombstoned = 0x0000057F,
        ReadOnlyTransaction = 0x00000596,
        Paused = 0x0000060E,
        NotPaused = 0x0000060F,
        WrongMailbox = 0x00000648,
        ChangePassword = 0x0000064C,
        PasswordExpired = 0x0000064D,
        InvalidWorkstation = 0x0000064E,
        InvalidLogonHours = 0x0000064F,
        AccountDisabled = 0x00000650,
        RuleVersion = 0x000006A4,
        RuleFormat = 0x000006A5,
        RuleSendAsDenied = 0x000006A6,
        NoServerSupport = 0x000006B9,
        LockTimedOut = 0x000006BA,
        ObjectLocked = 0x000006BB,
        InvalidLockNamespace = 0x000006BD,
        MessageDeleted = 0x000007D6,
        ProtocolDisabled = 0x000007D8,
        CleartextLogonDisabled = 0x000007D9,
        Rejected = 0x000007EE,
        AmbiguousAlias = 0x0000089A,
        UnknownMailbox = 0x0000089B,
        ExpressionReserved = 0x000008FC,
        ExpressionParseDepth = 0x000008FD,
        ExpressionArgumentType = 0x000008FE,
        ExpressionSyntax = 0x000008FF,
        ExpressionBadStringToken = 0x00000900,
        ExpressionBadColToken = 0x00000901,
        ExpressionTypeMismatch = 0x00000902,
        ExpressionOperatorNotSupported = 0x00000903,
        ExpressionDivideByZero = 0x00000904,
        ExpressionUnaryArgument = 0x00000905,
        NotLocked = 0x00000960,
        ClientEvent = 0x00000961,
        CorruptEvent = 0x00000965,
        CorruptWatermark = 0x00000966,
        EventError = 0x00000967,
        WatermarkError = 0x00000968,
        NonCanonicalACL = 0x00000969,
        MailboxDisabled = 0x0000096C,
        RulesFolderOverQuota = 0x0000096D,
        AddressBookUnavailable = 0x0000096E,
        AddressBookError = 0x0000096F,
        AddressBookObjectNotFound = 0x00000971,
        AddressBookPropertyError = 0x00000972,
        NotEncrypted = 0x00000970,
        RpcServerTooBusy = 0x00000973,
        RpcOutOfMemory = 0x00000974,
        RpcServerOutOfMemory = 0x00000975,
        RpcOutOfResources = 0x00000976,
        RpcServerUnavailable = 0x00000977,
        SecureSubmitError = 0x0000097A,
        EventsDeleted = 0x0000097C,
        SubsystemStopping = 0x0000097D,
        AttendantUnavailable = 0x0000097E,
        CIStopping = 0x00000A28,
        FxInvalidState = 0x00000A29,
        FxUnexpectedMarker = 0x00000A2A,
        DuplicateDelivery = 0x00000A2B,
        ConditionViolation = 0x00000A2C,
        MaximumConnectionPoolsExceeded = 0x00000A2D,
        InvalidRpcHandle = 0x00000A2E,
        EventNotFound = 0x00000A2F,
        PropertyNotPromoted = 0x00000A30,
        LowFreeSpaceForDatabase = 0x00000A31,
        LowFreeSpaceForLogs = 0x00000A32,
        MailboxIsQuarantined = 0x00000A33,
        DatabaseMountInProgress = 0x00000A34,
        DatabaseDismountInProgress = 0x00000A35,
        ConnectionsOverBudget = 0x00000A36,
        NotFoundInContainer = 0x00000A37,
        CannotRemove = 0x00000A38,
        InvalidConnectionPool = 0x00000A39,
        VirusScanGeneralFailure = 0x00000A3A,
        IsamErrorRfsFailure = 0xFFFFFF9C,
        IsamErrorRfsNotArmed = 0xFFFFFF9B,
        IsamErrorFileClose = 0xFFFFFF9A,
        IsamErrorOutOfThreads = 0xFFFFFF99,
        IsamErrorTooManyIO = 0xFFFFFF97,
        IsamErrorTaskDropped = 0xFFFFFF96,
        IsamErrorInternalError = 0xFFFFFF95,
        IsamErrorDatabaseBufferDependenciesCorrupted = 0xFFFFFF01,
        IsamErrorPreviousVersion = 0xFFFFFEBE,
        IsamErrorPageBoundary = 0xFFFFFEBD,
        IsamErrorKeyBoundary = 0xFFFFFEBC,
        IsamErrorBadPageLink = 0xFFFFFEB9,
        IsamErrorBadBookmark = 0xFFFFFEB8,
        IsamErrorNTSystemCallFailed = 0xFFFFFEB2,
        IsamErrorBadParentPageLink = 0xFFFFFEAE,
        IsamErrorSPAvailExtCacheOutOfSync = 0xFFFFFEAC,
        IsamErrorSPAvailExtCorrupted = 0xFFFFFEAB,
        IsamErrorSPAvailExtCacheOutOfMemory = 0xFFFFFEAA,
        IsamErrorSPOwnExtCorrupted = 0xFFFFFEA9,
        IsamErrorDbTimeCorrupted = 0xFFFFFEA8,
        IsamErrorKeyTruncated = 0xFFFFFEA6,
        IsamErrorKeyTooBig = 0xFFFFFE68,
        IsamErrorInvalidLoggedOperation = 0xFFFFFE0C,
        IsamErrorLogFileCorrupt = 0xFFFFFE0B,
        IsamErrorNoBackupDirectory = 0xFFFFFE09,
        IsamErrorBackupDirectoryNotEmpty = 0xFFFFFE08,
        IsamErrorBackupInProgress = 0xFFFFFE07,
        IsamErrorRestoreInProgress = 0xFFFFFE06,
        IsamErrorMissingPreviousLogFile = 0xFFFFFE03,
        IsamErrorLogWriteFail = 0xFFFFFE02,
        IsamErrorLogDisabledDueToRecoveryFailure = 0xFFFFFE01,
        IsamErrorCannotLogDuringRecoveryRedo = 0xFFFFFE00,
        IsamErrorLogGenerationMismatch = 0xFFFFFDFF,
        IsamErrorBadLogVersion = 0xFFFFFDFE,
        IsamErrorInvalidLogSequence = 0xFFFFFDFD,
        IsamErrorLoggingDisabled = 0xFFFFFDFC,
        IsamErrorLogBufferTooSmall = 0xFFFFFDFB,
        IsamErrorLogSequenceEnd = 0xFFFFFDF9,
        IsamErrorNoBackup = 0xFFFFFDF8,
        IsamErrorInvalidBackupSequence = 0xFFFFFDF7,
        IsamErrorBackupNotAllowedYet = 0xFFFFFDF5,
        IsamErrorDeleteBackupFileFail = 0xFFFFFDF4,
        IsamErrorMakeBackupDirectoryFail = 0xFFFFFDF3,
        IsamErrorInvalidBackup = 0xFFFFFDF2,
        IsamErrorRecoveredWithErrors = 0xFFFFFDF1,
        IsamErrorMissingLogFile = 0xFFFFFDF0,
        IsamErrorLogDiskFull = 0xFFFFFDEF,
        IsamErrorBadLogSignature = 0xFFFFFDEE,
        IsamErrorBadDbSignature = 0xFFFFFDED,
        IsamErrorBadCheckpointSignature = 0xFFFFFDEC,
        IsamErrorCheckpointCorrupt = 0xFFFFFDEB,
        IsamErrorMissingPatchPage = 0xFFFFFDEA,
        IsamErrorBadPatchPage = 0xFFFFFDE9,
        IsamErrorRedoAbruptEnded = 0xFFFFFDE8,
        IsamErrorBadSLVSignature = 0xFFFFFDE7,
        IsamErrorPatchFileMissing = 0xFFFFFDE6,
        IsamErrorDatabaseLogSetMismatch = 0xFFFFFDE5,
        IsamErrorDatabaseStreamingFileMismatch = 0xFFFFFDE4,
        IsamErrorLogFileSizeMismatch = 0xFFFFFDE3,
        IsamErrorCheckpointFileNotFound = 0xFFFFFDE2,
        IsamErrorRequiredLogFilesMissing = 0xFFFFFDE1,
        IsamErrorSoftRecoveryOnBackupDatabase = 0xFFFFFDE0,
        IsamErrorLogFileSizeMismatchDatabasesConsistent = 0xFFFFFDDF,
        IsamErrorLogSectorSizeMismatch = 0xFFFFFDDE,
        IsamErrorLogSectorSizeMismatchDatabasesConsistent = 0xFFFFFDDD,
        IsamErrorLogSequenceEndDatabasesConsistent = 0xFFFFFDDC,
        IsamErrorStreamingDataNotLogged = 0xFFFFFDDB,
        IsamErrorDatabaseDirtyShutdown = 0xFFFFFDDA,
        IsamErrorConsistentTimeMismatch = 0xFFFFFDD9,
        IsamErrorDatabasePatchFileMismatch = 0xFFFFFDD8,
        IsamErrorEndingRestoreLogTooLow = 0xFFFFFDD7,
        IsamErrorStartingRestoreLogTooHigh = 0xFFFFFDD6,
        IsamErrorGivenLogFileHasBadSignature = 0xFFFFFDD5,
        IsamErrorGivenLogFileIsNotContiguous = 0xFFFFFDD4,
        IsamErrorMissingRestoreLogFiles = 0xFFFFFDD3,
        IsamErrorMissingFullBackup = 0xFFFFFDD0,
        IsamErrorBadBackupDatabaseSize = 0xFFFFFDCF,
        IsamErrorDatabaseAlreadyUpgraded = 0xFFFFFDCE,
        IsamErrorDatabaseIncompleteUpgrade = 0xFFFFFDCD,
        IsamErrorMissingCurrentLogFiles = 0xFFFFFDCB,
        IsamErrorDbTimeTooOld = 0xFFFFFDCA,
        IsamErrorDbTimeTooNew = 0xFFFFFDC9,
        IsamErrorMissingFileToBackup = 0xFFFFFDC7,
        IsamErrorLogTornWriteDuringHardRestore = 0xFFFFFDC6,
        IsamErrorLogTornWriteDuringHardRecovery = 0xFFFFFDC5,
        IsamErrorLogCorruptDuringHardRestore = 0xFFFFFDC3,
        IsamErrorLogCorruptDuringHardRecovery = 0xFFFFFDC2,
        IsamErrorMustDisableLoggingForDbUpgrade = 0xFFFFFDC1,
        IsamErrorBadRestoreTargetInstance = 0xFFFFFDBF,
        IsamErrorRecoveredWithoutUndo = 0xFFFFFDBD,
        IsamErrorDatabasesNotFromSameSnapshot = 0xFFFFFDBC,
        IsamErrorSoftRecoveryOnSnapshot = 0xFFFFFDBB,
        IsamErrorCommittedLogFilesMissing = 0xFFFFFDBA,
        IsamErrorCommittedLogFilesCorrupt = 0xFFFFFDB6,
        IsamErrorUnicodeTranslationBufferTooSmall = 0xFFFFFDA7,
        IsamErrorUnicodeTranslationFail = 0xFFFFFDA6,
        IsamErrorUnicodeNormalizationNotSupported = 0xFFFFFDA5,
        IsamErrorExistingLogFileHasBadSignature = 0xFFFFFD9E,
        IsamErrorExistingLogFileIsNotContiguous = 0xFFFFFD9D,
        IsamErrorLogReadVerifyFailure = 0xFFFFFD9C,
        IsamErrorSLVReadVerifyFailure = 0xFFFFFD9B,
        IsamErrorCheckpointDepthTooDeep = 0xFFFFFD9A,
        IsamErrorRestoreOfNonBackupDatabase = 0xFFFFFD99,
        IsamErrorInvalidGrbit = 0xFFFFFC7C,
        IsamErrorTermInProgress = 0xFFFFFC18,
        IsamErrorFeatureNotAvailable = 0xFFFFFC17,
        IsamErrorInvalidName = 0xFFFFFC16,
        IsamErrorInvalidParameter = 0xFFFFFC15,
        IsamErrorDatabaseFileReadOnly = 0xFFFFFC10,
        IsamErrorInvalidDatabaseId = 0xFFFFFC0E,
        IsamErrorOutOfMemory = 0xFFFFFC0D,
        IsamErrorOutOfDatabaseSpace = 0xFFFFFC0C,
        IsamErrorOutOfCursors = 0xFFFFFC0B,
        IsamErrorOutOfBuffers = 0xFFFFFC0A,
        IsamErrorTooManyIndexes = 0xFFFFFC09,
        IsamErrorTooManyKeys = 0xFFFFFC08,
        IsamErrorRecordDeleted = 0xFFFFFC07,
        IsamErrorReadVerifyFailure = 0xFFFFFC06,
        IsamErrorPageNotInitialized = 0xFFFFFC05,
        IsamErrorOutOfFileHandles = 0xFFFFFC04,
        IsamErrorDiskIO = 0xFFFFFC02,
        IsamErrorInvalidPath = 0xFFFFFC01,
        IsamErrorInvalidSystemPath = 0xFFFFFC00,
        IsamErrorInvalidLogDirectory = 0xFFFFFBFF,
        IsamErrorRecordTooBig = 0xFFFFFBFE,
        IsamErrorTooManyOpenDatabases = 0xFFFFFBFD,
        IsamErrorInvalidDatabase = 0xFFFFFBFC,
        IsamErrorNotInitialized = 0xFFFFFBFB,
        IsamErrorAlreadyInitialized = 0xFFFFFBFA,
        IsamErrorInitInProgress = 0xFFFFFBF9,
        IsamErrorFileAccessDenied = 0xFFFFFBF8,
        IsamErrorBufferTooSmall = 0xFFFFFBF2,
        IsamErrorTooManyColumns = 0xFFFFFBF0,
        IsamErrorContainerNotEmpty = 0xFFFFFBED,
        IsamErrorInvalidFilename = 0xFFFFFBEC,
        IsamErrorInvalidBookmark = 0xFFFFFBEB,
        IsamErrorColumnInUse = 0xFFFFFBEA,
        IsamErrorInvalidBufferSize = 0xFFFFFBE9,
        IsamErrorColumnNotUpdatable = 0xFFFFFBE8,
        IsamErrorIndexInUse = 0xFFFFFBE5,
        IsamErrorLinkNotSupported = 0xFFFFFBE4,
        IsamErrorNullKeyDisallowed = 0xFFFFFBE3,
        IsamErrorNotInTransaction = 0xFFFFFBE2,
        IsamErrorTooManyActiveUsers = 0xFFFFFBDD,
        IsamErrorInvalidCountry = 0xFFFFFBDB,
        IsamErrorInvalidLanguageId = 0xFFFFFBDA,
        IsamErrorInvalidCodePage = 0xFFFFFBD9,
        IsamErrorInvalidLCMapStringFlags = 0xFFFFFBD8,
        IsamErrorVersionStoreEntryTooBig = 0xFFFFFBD7,
        IsamErrorVersionStoreOutOfMemoryAndCleanupTimedOut = 0xFFFFFBD6,
        IsamErrorVersionStoreOutOfMemory = 0xFFFFFBD3,
        IsamErrorCannotIndex = 0xFFFFFBD1,
        IsamErrorRecordNotDeleted = 0xFFFFFBD0,
        IsamErrorTooManyMempoolEntries = 0xFFFFFBCF,
        IsamErrorOutOfObjectIDs = 0xFFFFFBCE,
        IsamErrorOutOfLongValueIDs = 0xFFFFFBCD,
        IsamErrorOutOfAutoincrementValues = 0xFFFFFBCC,
        IsamErrorOutOfDbtimeValues = 0xFFFFFBCB,
        IsamErrorOutOfSequentialIndexValues = 0xFFFFFBCA,
        IsamErrorRunningInOneInstanceMode = 0xFFFFFBC8,
        IsamErrorRunningInMultiInstanceMode = 0xFFFFFBC7,
        IsamErrorSystemParamsAlreadySet = 0xFFFFFBC6,
        IsamErrorSystemPathInUse = 0xFFFFFBC5,
        IsamErrorLogFilePathInUse = 0xFFFFFBC4,
        IsamErrorTempPathInUse = 0xFFFFFBC3,
        IsamErrorInstanceNameInUse = 0xFFFFFBC2,
        IsamErrorInstanceUnavailable = 0xFFFFFBBE,
        IsamErrorDatabaseUnavailable = 0xFFFFFBBD,
        IsamErrorInstanceUnavailableDueToFatalLogDiskFull = 0xFFFFFBBC,
        IsamErrorOutOfSessions = 0xFFFFFBB3,
        IsamErrorWriteConflict = 0xFFFFFBB2,
        IsamErrorTransTooDeep = 0xFFFFFBB1,
        IsamErrorInvalidSesid = 0xFFFFFBB0,
        IsamErrorWriteConflictPrimaryIndex = 0xFFFFFBAF,
        IsamErrorInTransaction = 0xFFFFFBAC,
        IsamErrorRollbackRequired = 0xFFFFFBAB,
        IsamErrorTransReadOnly = 0xFFFFFBAA,
        IsamErrorSessionWriteConflict = 0xFFFFFBA9,
        IsamErrorRecordTooBigForBackwardCompatibility = 0xFFFFFBA8,
        IsamErrorCannotMaterializeForwardOnlySort = 0xFFFFFBA7,
        IsamErrorSesidTableIdMismatch = 0xFFFFFBA6,
        IsamErrorInvalidInstance = 0xFFFFFBA5,
        IsamErrorDatabaseDuplicate = 0xFFFFFB4F,
        IsamErrorDatabaseInUse = 0xFFFFFB4E,
        IsamErrorDatabaseNotFound = 0xFFFFFB4D,
        IsamErrorDatabaseInvalidName = 0xFFFFFB4C,
        IsamErrorDatabaseInvalidPages = 0xFFFFFB4B,
        IsamErrorDatabaseCorrupted = 0xFFFFFB4A,
        IsamErrorDatabaseLocked = 0xFFFFFB49,
        IsamErrorCannotDisableVersioning = 0xFFFFFB48,
        IsamErrorInvalidDatabaseVersion = 0xFFFFFB47,
        IsamErrorDatabase200Format = 0xFFFFFB46,
        IsamErrorDatabase400Format = 0xFFFFFB45,
        IsamErrorDatabase500Format = 0xFFFFFB44,
        IsamErrorPageSizeMismatch = 0xFFFFFB43,
        IsamErrorTooManyInstances = 0xFFFFFB42,
        IsamErrorDatabaseSharingViolation = 0xFFFFFB41,
        IsamErrorAttachedDatabaseMismatch = 0xFFFFFB40,
        IsamErrorDatabaseInvalidPath = 0xFFFFFB3F,
        IsamErrorDatabaseIdInUse = 0xFFFFFB3E,
        IsamErrorForceDetachNotAllowed = 0xFFFFFB3D,
        IsamErrorCatalogCorrupted = 0xFFFFFB3C,
        IsamErrorPartiallyAttachedDB = 0xFFFFFB3B,
        IsamErrorDatabaseSignInUse = 0xFFFFFB3A,
        IsamErrorDatabaseCorruptedNoRepair = 0xFFFFFB38,
        IsamErrorInvalidCreateDbVersion = 0xFFFFFB37,
        IsamErrorTableLocked = 0xFFFFFAEA,
        IsamErrorTableDuplicate = 0xFFFFFAE9,
        IsamErrorTableInUse = 0xFFFFFAE8,
        IsamErrorObjectNotFound = 0xFFFFFAE7,
        IsamErrorDensityInvalid = 0xFFFFFAE5,
        IsamErrorTableNotEmpty = 0xFFFFFAE4,
        IsamErrorInvalidTableId = 0xFFFFFAE2,
        IsamErrorTooManyOpenTables = 0xFFFFFAE1,
        IsamErrorIllegalOperation = 0xFFFFFAE0,
        IsamErrorTooManyOpenTablesAndCleanupTimedOut = 0xFFFFFADF,
        IsamErrorObjectDuplicate = 0xFFFFFADE,
        IsamErrorInvalidObject = 0xFFFFFADC,
        IsamErrorCannotDeleteTempTable = 0xFFFFFADB,
        IsamErrorCannotDeleteSystemTable = 0xFFFFFADA,
        IsamErrorCannotDeleteTemplateTable = 0xFFFFFAD9,
        IsamErrorExclusiveTableLockRequired = 0xFFFFFAD6,
        IsamErrorFixedDDL = 0xFFFFFAD5,
        IsamErrorFixedInheritedDDL = 0xFFFFFAD4,
        IsamErrorCannotNestDDL = 0xFFFFFAD3,
        IsamErrorDDLNotInheritable = 0xFFFFFAD2,
        IsamErrorInvalidSettings = 0xFFFFFAD0,
        IsamErrorClientRequestToStopJetService = 0xFFFFFACF,
        IsamErrorCannotAddFixedVarColumnToDerivedTable = 0xFFFFFACE,
        IsamErrorIndexCantBuild = 0xFFFFFA87,
        IsamErrorIndexHasPrimary = 0xFFFFFA86,
        IsamErrorIndexDuplicate = 0xFFFFFA85,
        IsamErrorIndexNotFound = 0xFFFFFA84,
        IsamErrorIndexMustStay = 0xFFFFFA83,
        IsamErrorIndexInvalidDef = 0xFFFFFA82,
        IsamErrorInvalidCreateIndex = 0xFFFFFA7F,
        IsamErrorTooManyOpenIndexes = 0xFFFFFA7E,
        IsamErrorMultiValuedIndexViolation = 0xFFFFFA7D,
        IsamErrorIndexBuildCorrupted = 0xFFFFFA7C,
        IsamErrorPrimaryIndexCorrupted = 0xFFFFFA7B,
        IsamErrorSecondaryIndexCorrupted = 0xFFFFFA7A,
        IsamErrorInvalidIndexId = 0xFFFFFA78,
        IsamErrorIndexTuplesSecondaryIndexOnly = 0xFFFFFA6A,
        IsamErrorIndexTuplesTooManyColumns = 0xFFFFFA69,
        IsamErrorIndexTuplesNonUniqueOnly = 0xFFFFFA68,
        IsamErrorIndexTuplesTextBinaryColumnsOnly = 0xFFFFFA67,
        IsamErrorIndexTuplesVarSegMacNotAllowed = 0xFFFFFA66,
        IsamErrorIndexTuplesInvalidLimits = 0xFFFFFA65,
        IsamErrorIndexTuplesCannotRetrieveFromIndex = 0xFFFFFA64,
        IsamErrorIndexTuplesKeyTooSmall = 0xFFFFFA63,
        IsamErrorColumnLong = 0xFFFFFA23,
        IsamErrorColumnNoChunk = 0xFFFFFA22,
        IsamErrorColumnDoesNotFit = 0xFFFFFA21,
        IsamErrorNullInvalid = 0xFFFFFA20,
        IsamErrorColumnIndexed = 0xFFFFFA1F,
        IsamErrorColumnTooBig = 0xFFFFFA1E,
        IsamErrorColumnNotFound = 0xFFFFFA1D,
        IsamErrorColumnDuplicate = 0xFFFFFA1C,
        IsamErrorMultiValuedColumnMustBeTagged = 0xFFFFFA1B,
        IsamErrorColumnRedundant = 0xFFFFFA1A,
        IsamErrorInvalidColumnType = 0xFFFFFA19,
        IsamErrorTaggedNotNULL = 0xFFFFFA16,
        IsamErrorNoCurrentIndex = 0xFFFFFA15,
        IsamErrorKeyIsMade = 0xFFFFFA14,
        IsamErrorBadColumnId = 0xFFFFFA13,
        IsamErrorBadItagSequence = 0xFFFFFA12,
        IsamErrorColumnInRelationship = 0xFFFFFA11,
        IsamErrorCannotBeTagged = 0xFFFFFA0F,
        IsamErrorDefaultValueTooBig = 0xFFFFFA0C,
        IsamErrorMultiValuedDuplicate = 0xFFFFFA0B,
        IsamErrorLVCorrupted = 0xFFFFFA0A,
        IsamErrorMultiValuedDuplicateAfterTruncation = 0xFFFFFA08,
        IsamErrorDerivedColumnCorruption = 0xFFFFFA07,
        IsamErrorInvalidPlaceholderColumn = 0xFFFFFA06,
        IsamErrorRecordNotFound = 0xFFFFF9BF,
        IsamErrorRecordNoCopy = 0xFFFFF9BE,
        IsamErrorNoCurrentRecord = 0xFFFFF9BD,
        IsamErrorRecordPrimaryChanged = 0xFFFFF9BC,
        IsamErrorKeyDuplicate = 0xFFFFF9BB,
        IsamErrorAlreadyPrepared = 0xFFFFF9B9,
        IsamErrorKeyNotMade = 0xFFFFF9B8,
        IsamErrorUpdateNotPrepared = 0xFFFFF9B7,
        IsamErrorDataHasChanged = 0xFFFFF9B5,
        IsamErrorLanguageNotSupported = 0xFFFFF9AD,
        IsamErrorTooManySorts = 0xFFFFF95B,
        IsamErrorInvalidOnSort = 0xFFFFF95A,
        IsamErrorTempFileOpenError = 0xFFFFF8F5,
        IsamErrorTooManyAttachedDatabases = 0xFFFFF8F3,
        IsamErrorDiskFull = 0xFFFFF8F0,
        IsamErrorPermissionDenied = 0xFFFFF8EF,
        IsamErrorFileNotFound = 0xFFFFF8ED,
        IsamErrorFileInvalidType = 0xFFFFF8EC,
        IsamErrorAfterInitialization = 0xFFFFF8C6,
        IsamErrorLogCorrupted = 0xFFFFF8C4,
        IsamErrorInvalidOperation = 0xFFFFF88E,
        IsamErrorAccessDenied = 0xFFFFF88D,
        IsamErrorTooManySplits = 0xFFFFF88B,
        IsamErrorSessionSharingViolation = 0xFFFFF88A,
        IsamErrorEntryPointNotFound = 0xFFFFF889,
        IsamErrorSessionContextAlreadySet = 0xFFFFF888,
        IsamErrorSessionContextNotSetByThisThread = 0xFFFFF887,
        IsamErrorSessionInUse = 0xFFFFF886,
        IsamErrorRecordFormatConversionFailed = 0xFFFFF885,
        IsamErrorOneDatabasePerSession = 0xFFFFF884,
        IsamErrorRollbackError = 0xFFFFF883,
        IsamErrorCallbackFailed = 0xFFFFF7CB,
        IsamErrorCallbackNotResolved = 0xFFFFF7CA,
        IsamErrorOSSnapshotInvalidSequence = 0xFFFFF69F,
        IsamErrorOSSnapshotTimeOut = 0xFFFFF69E,
        IsamErrorOSSnapshotNotAllowed = 0xFFFFF69D,
        IsamErrorOSSnapshotInvalidSnapId = 0xFFFFF69C,
        IsamErrorLSCallbackNotSpecified = 0xFFFFF448,
        IsamErrorLSAlreadySet = 0xFFFFF447,
        IsamErrorLSNotSet = 0xFFFFF446,
        IsamErrorFileIOSparse = 0xFFFFF060,
        IsamErrorFileIOBeyondEOF = 0xFFFFF05F,
        IsamErrorFileCompressed = 0xFFFFF05B
    }

    ///  <summary>
    /// 2.4.2	Property Error Codes
    /// </summary>
    public enum PropertyErrorCodes : uint
    {
        NotEnoughMemory = 0x8007000E,
        NotFound = 0x8004010F,
        BadValue = 0x80040301,
        InvalidType = 0x80040302,
        UnsupportedType = 0x80040303,
        UnexpectedType = 0x80040304,
        TooBig = 0x80040305,
        DeclineCopy = 0x80040306,
        UnexpectedId = 0x80040307
    }

    ///  <summary>
    /// 2.4.3	Warning Codes
    /// </summary>
    public enum WarningCodes : uint
    {
        ErrorsReturned = 0x00040380,
        PositionChanged = 0x00040481,
        ApproximateCount = 0x00040482,
        PartiallyComplete = 0x00040680,
        SyncProgress = 0x00040820,
        NewerClientChange = 0x00040821,
        IsamWarningRemainingVersions = 0x00000141,
        IsamWarningUniqueKey = 0x00000159,
        IsamWarningSeparateLongValue = 0x00000196,
        IsamWarningExistingLogFileHasBadSignature = 0x0000022E,
        IsamWarningExistingLogFileIsNotContiguous = 0x0000022F,
        IsamWarningSkipThisRecord = 0x00000234,
        IsamWarningTargetInstanceRunning = 0x00000242,
        IsamWarningDatabaseRepaired = 0x00000253,
        IsamWarningColumnNull = 0x000003EC,
        IsamWarningBufferTruncated = 0x000003EE,
        IsamWarningDatabaseAttached = 0x000003EF,
        IsamWarningSortOverflow = 0x000003F1,
        IsamWarningSeekNotEqual = 0x0000040F,
        IsamWarningNoErrorInfo = 0x0000041F,
        IsamWarningNoIdleActivity = 0x00000422,
        IsamWarningNoWriteLock = 0x0000042B,
        IsamWarningColumnSetNull = 0x0000042C,
        IsamWarningTableEmpty = 0x00000515,
        IsamWarningTableInUseBySystem = 0x0000052F,
        IsamWarningCorruptIndexDeleted = 0x00000587,
        IsamWarningColumnMaxTruncated = 0x000005E8,
        IsamWarningCopyLongValue = 0x000005F0,
        IsamWarningColumnSkipped = 0x000005FB,
        IsamWarningColumnNotLocal = 0x000005FC,
        IsamWarningColumnMoreTags = 0x000005FD,
        IsamWarningColumnTruncated = 0x000005FE,
        IsamWarningColumnPresent = 0x000005FF,
        IsamWarningColumnSingleValue = 0x00000600,
        IsamWarningColumnDefault = 0x00000601,
        IsamWarningDataHasChanged = 0x0000064A,
        IsamWarningKeyChanged = 0x00000652,
        IsamWarningFileOpenReadOnly = 0x00000715,
        IsamWarningIdleFull = 0x00000774,
        IsamWarningDefragAlreadyRunning = 0x000007D0,
        IsamWarningDefragNotRunning = 0x000007D1,
        IsamWarningCallbackNotRegistered = 0x00000834,
        IsamWarningNotYetImplemented = 0xFFFFFFFF,
        UnbindSuccess = 0x000000001,
        UnbindFailure = 0x00000002,

    }
    #endregion

    #region 2.6	Property Name Structures

    /// <summary>
    /// The enum of Kind.
    /// </summary>
    public enum KindEnum : byte
    {
        LID = 0x00,
        Name = 0x01,
        NoPropertyName = 0xFF
    }

    ///  <summary>
    /// 2.6.1	PropertyName Structure
    /// </summary>
    public class PropertyName : BaseStructure
    {
        // The Kind field. 
        public KindEnum Kind;

        // The GUID that identifies the property set for the named property.
        public Guid GUID;

        // This field is present only if the value of the Kind field is equal to 0x00.
        public uint? LID;

        // The value of this field is equal to the number of bytes in the Name string that follows it. 
        public byte? NameSize;

        // This field is present only if Kind is equal to 0x01.
        public MAPIString Name;

        /// <summary>
        /// Parse the PropertyName structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyName structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Kind = (KindEnum)ReadByte();
            this.GUID = ReadGuid();
            switch (this.Kind)
            {
                case KindEnum.LID:
                    {
                        this.LID = ReadUint();
                        break;
                    }
                case KindEnum.Name:
                    {
                        this.NameSize = ReadByte();
                        this.Name = new MAPIString(Encoding.Unicode, "", (int)NameSize / 2);
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
    /// 2.6.2	PropertyName_r Structure
    /// </summary>
    public class PropertyName_r : BaseStructure
    {
        // Encodes the GUID field of the PropertyName structure, as specified in section 2.6.1.
        public Guid GUID;

        // All clients and servers MUST set this value to 0x00000000.
        public uint Reserved;

        // This value encodes the LID field in the PropertyName structure, as specified in section 2.6.1. 
        public uint LID;

        /// <summary>
        /// Parse the PropertyName_r structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyName_r structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.GUID = ReadGuid();
            this.Reserved = ReadUint();
            this.LID = ReadUint();
        }
    }
    #endregion

    #region 2.7	PropertyProblem Structure
    /// <summary>
    /// 2.7	PropertyProblem Structure
    /// </summary>
    public class PropertyProblem : BaseStructure
    {
        // An unsigned integer. This value specifies an index into an array of property tags.
        public ushort Index;

        // A PropertyTag structure, as specified in section 2.9. 
        public PropertyTag PropertyTag;

        // An unsigned integer. This value specifies the error that occurred when processing this property.
        public PropertyErrorCodes ErrorCode;

        /// <summary>
        /// Parse the PropertyProblem structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyProblem structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Index = ReadUshort();
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            this.ErrorCode = (PropertyErrorCodes)ReadUint();
        }
    }
    #endregion

    #region 2.8	Property Row Structures
    /// <summary>
    /// 2.8.1	PropertyRow Structures
    /// </summary>
    public class PropertyRow : BaseStructure
    {
        // An unsigned integer. This value indicate if all property values are present and without error.
        public byte Flag;

        // An array of variable-sized structures.
        public object[] ValueArray;

        // The array of property tag.
        private PropertyTag[] PropTags;

        /// <summary>
        /// The Constructor to set property tag.
        /// </summary>
        /// <param name="propTags">The array of property tag.</param>
        public PropertyRow(PropertyTag[] propTags)
        {
            this.PropTags = propTags;
        }
        /// <summary>
        /// Parse the PropertyRow structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flag = ReadByte();
            List<object> tempPropArray = new List<object>();
            foreach (PropertyTag tempPropTag in PropTags)
            {
                object rowPropValue = null;
                tempPropTag.PropertyType = ConvertToPropType((ushort)tempPropTag.PropertyType);

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
                else if (Flag == 0x01)
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
            this.ValueArray = tempPropArray.ToArray();
        }
    }

    ///  <summary>
    /// The enumeration specifies the type of address. 
    /// </summary>
    public enum AddressTypeEnum : int
    {
        NoType = 0x0,
        X500DN = 0x1,
        MsMail = 0x2,
        SMTP = 0x3,
        Fax = 0x4,
        ProfessionalOfficeSystem = 0x5,
        PersonalDistributionList1 = 0x6,
        PersonalDistributionList2 = 0x7
    }

    /// <summary>
    /// 2.8.3.1	RecipientFlags Field
    /// </summary>
    public class RecipientFlags : BaseStructure
    {
        // If this flag is b'1', a different transport is responsible for delivery to this recipient (1).
        [BitAttribute(1)]
        public int R;

        // If this flag is b'1', the value of the TransmittableDisplayName field is the same as the value of the DisplayName field.
        [BitAttribute(1)]
        public int S;

        // If this flag is b'1', the TransmittableDisplayName (section 2.8.3.2) field is included.
        [BitAttribute(1)]
        public int T;

        // If this flag is b'1', the DisplayName (section 2.8.3.2) field is included.
        [BitAttribute(1)]
        public int D;

        // If this flag is b'1', the EmailAddress (section 2.8.3.2) field is included.
        [BitAttribute(1)]
        public int E;

        // This enumeration specifies the type of address. 
        [BitAttribute(3)]
        public AddressTypeEnum Type;

        // If this flag is b'1', this recipient (1) has a non-standard address type and the AddressType field is included.
        [BitAttribute(1)]
        public int O;

        // The server MUST set this to b'0000'.
        [BitAttribute(4)]
        public int Reserved;

        // If this flag is b'1', the SimpleDisplayName field is included.
        [BitAttribute(1)]
        public int I;
        //If this flag is b'1', the associated string properties are in Unicode with a 2-byte terminating null character; if this flag is b'0', string properties are MBCS with a single terminating null character.
        [BitAttribute(1)]
        public int U;

        // If b'1', this flag specifies that the recipient (1) does not support receiving rich text messages.
        [BitAttribute(1)]
        public int N;

        /// <summary>
        /// Parse the RecipientFlags structure.
        /// </summary>
        /// <param name="s">A stream containing the RecipientFlags structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Byte tempByte = ReadByte();
            int index = 0;
            this.R = GetBits(tempByte, index, 1);
            index = index + 1;
            this.S = GetBits(tempByte, index, 1);
            index = index + 1;
            this.T = GetBits(tempByte, index, 1);
            index = index + 1;
            this.D = GetBits(tempByte, index, 1);
            index = index + 1;
            this.E = GetBits(tempByte, index, 1);
            index = index + 1;
            this.Type = (AddressTypeEnum)GetBits(tempByte, index, 3);

            tempByte = ReadByte();
            index = 0;
            this.O = GetBits(tempByte, index, 1);
            index = index + 1;
            this.Reserved = GetBits(tempByte, index, 4);
            index = index + 4;
            this.I = GetBits(tempByte, index, 1);
            index = index + 1;
            this.U = GetBits(tempByte, index, 1);
            index = index + 1;
            this.N = GetBits(tempByte, index, 1);
        }
    }

    /// <summary>
    /// The enum value of DisplayType.
    /// </summary>
    public enum DisplayType : byte
    {
        MessagingUser = 0x00,
        DistributionList = 0x01,
        Forum = 0x02,
        AutomatedAgent = 0x03,
        AddressBookforLargeGroup = 0x04,
        Private = 0x05,
        AddressBookfromMessagingSystem = 0x06
    }

    ///  <summary>
    /// 2.8.3.2	RecipientRow Structure
    /// </summary>
    public class RecipientRow : BaseStructure
    {
        // A RecipientFlags structure, as specified in section 2.8.3.1. 
        public RecipientFlags RecipientFlags;

        // Unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise. 
        public byte? AddressPrefixUsed;

        // An enumeration. This field MUST be present when the Type field of the RecipientFlags field is set to X500DN (0x1) and MUST NOT be present otherwise. 
        public DisplayType? DisplayType;

        // A null-terminated ASCII string. 
        public MAPIString X500DN;

        // An unsigned integer. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7). 
        public ushort? EntryIdSize;

        // An array of bytes. This field MUST be present when the Type field of the RecipientFlags field is set to PersonalDistributionList1 (0x6) or PersonalDistributionList2 (0x7). 
        public AddressBookEntryID EntryID;

        // This value specifies the size of the SearchKey field.
        public ushort? SearchKeySize;

        // This array specifies the search key of the distribution list.
        public byte?[] SearchKey;

        // This string specifies the address type of the recipient (1).
        public MAPIString AddressType;

        // This string specifies the email address of the recipient (1).
        public MAPIString EmailAddress;

        // This string specifies the email address of the recipient (1).
        public MAPIString DisplayName;

        // This string specifies the email address of the recipient (1).
        public MAPIString SimpleDisplayName;

        // This string specifies the email address of the recipient (1).
        public MAPIString TransmittableDisplayName;

        // This value specifies the number of columns from the RecipientColumns field that are included in the RecipientProperties field. 
        public ushort? RecipientColumnCount;

        // The columns used for this row are those specified in RecipientProperties.
        public PropertyRow RecipientProperties;

        // The array of property tag.
        private PropertyTag[] PropTags;

        /// <summary>
        /// The Constructor to set the  property tag.
        /// </summary>
        /// <param name="ptypMultiCountSize"> The array of property tag.</param>
        public RecipientRow(PropertyTag[] propTags)
        {
            this.PropTags = propTags;
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
                this.AddressPrefixUsed = ReadByte();
                this.DisplayType = (DisplayType)ReadByte();
                this.X500DN = new MAPIString(Encoding.ASCII);
                this.X500DN.Parse(s);

            }
            else if (this.RecipientFlags.Type == AddressTypeEnum.PersonalDistributionList1 || this.RecipientFlags.Type == AddressTypeEnum.PersonalDistributionList2)
            {
                this.EntryIdSize = ReadUshort();
                this.EntryID = new AddressBookEntryID();
                this.EntryID.Parse(s);
                this.SearchKeySize = ReadUshort();
                this.SearchKey = ConvertArray(ReadBytes((int)this.SearchKeySize));
            }
            else if (this.RecipientFlags.Type == AddressTypeEnum.NoType && this.RecipientFlags.O == 0x1)
            {
                this.AddressType = new MAPIString(Encoding.ASCII);
                this.AddressType.Parse(s);

            }

            if (RecipientFlags.E == 0x1)
            {
                this.EmailAddress = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.EmailAddress.Parse(s);

            }

            if (RecipientFlags.D == 0x1)
            {
                this.DisplayName = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.DisplayName.Parse(s);

            }

            if (RecipientFlags.I == 0x1)
            {
                this.SimpleDisplayName = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.SimpleDisplayName.Parse(s);
            }

            if (RecipientFlags.T == 0x1)
            {
                this.TransmittableDisplayName = new MAPIString((RecipientFlags.U == 0x1) ? Encoding.Unicode : Encoding.ASCII);
                this.TransmittableDisplayName.Parse(s);
            }
            this.RecipientColumnCount = ReadUshort();
            List<PropertyTag> PropTagsActually = new List<PropertyTag>();
            if(this.PropTags.Length >= this.RecipientColumnCount)
            {
                for (int i = 0; i < this.RecipientColumnCount; i++)
                {
                    PropTagsActually.Add(this.PropTags[i]);
                }
            }
            else
            {
                throw new Exception(String.Format("Request format error: the RecipientColumnCount {0} should be less than RecipientColumns count {1}", this.RecipientColumnCount, this.PropTags.Length));
            }
            PropertyRow tempPropertyRow = new PropertyRow(PropTagsActually.ToArray());
            this.RecipientProperties = tempPropertyRow;
            this.RecipientProperties.Parse(s);
        }
    }
    #endregion

    #region 2.9	PropertyTag Structure

    /// <summary>
    /// 2.9	PropertyTag Structure
    /// </summary>
    public class PropertyTag : BaseStructure
    {
        // An unsigned integer that identifies the data type of the property value, as specified by the table in section 2.11.1.
        public PropertyDataType PropertyType;

        // An unsigned integer that identifies the property.
        public PidTagPropertyEnum PropertyId;

        /// <summary>
        /// Initializes a new instance of the PropertyTag class with parameters.
        /// </summary>
        /// <param name="PType">The Type of the PropertyTag.</param>
        /// /// <param name="PId">The Id of the PropertyTag.</param>
        public PropertyTag(PropertyDataType PType, PidTagPropertyEnum PId)
        {
            this.PropertyType = PType;
            this.PropertyId = PId;
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
    /// // Section 2.11.1   Property Data Types
    /// </summary>
    public enum PropertyDataType : ushort
    {
        PtypInteger16 = 0x0002,
        PtypInteger32 = 0x0003,
        PtypFloating32 = 0x0004,
        PtypFloating64 = 0x0005,
        PtypCurrency = 0x0006,
        PtypFloatingTime = 0x0007,
        PtypErrorCode = 0x000A,
        PtypBoolean = 0x000B,
        PtypInteger64 = 0x0014,
        PtypString = 0x001F,
        PtypString8 = 0x001E,
        PtypTime = 0x0040,
        PtypGuid = 0x0048,
        PtypServerId = 0x00FB,
        PtypRestriction = 0x00FD,
        PtypRuleAction = 0x00FE,
        PtypBinary = 0x0102,
        PtypMultipleInteger16 = 0x1002,
        PtypMultipleInteger32 = 0x1003,
        PtypMultipleFloating32 = 0x1004,
        PtypMultipleFloating64 = 0x1005,
        PtypMultipleCurrency = 0x1006,
        PtypMultipleFloatingTime = 0x1007,
        PtypMultipleInteger64 = 0x1014,
        PtypMultipleString = 0x101F,
        PtypMultipleString8 = 0x101E,
        PtypMultipleTime = 0x1040,
        PtypMultipleGuid = 0x1048,
        PtypMultipleBinary = 0x1102,
        PtypUnspecified = 0x0000,
        PtypNull = 0x0001,

        // IN FUTURE: How to distinguish PtypObject from PtypEmbeddedTable since they share the same value
        PtypObject_Or_PtypEmbeddedTable = 0x000D,
    }

    // Section 2.11.1.3   Multivalue Property Value Instances
    public enum PropertyDataTypeFlag : ushort
    {
        MutltiValue = 0x1000,
        MultivalueInstance = 0x2000,
    }

    /// <summary>
    /// 2 bytes; a 16-bit integer. [MS-DTYP]: INT16
    /// </summary>
    public class PtypInteger16 : BaseStructure
    {
        // 16-bit integer. 
        public Int16 Value;

        /// <summary>
        /// Parse the PtypInteger16 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypInteger16 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = ReadINT16();
        }

    }

    ///  <summary>
    /// 4 bytes; a 32-bit integer. [MS-DTYP]: INT32
    /// </summary>
    public class PtypInteger32 : BaseStructure
    {
        // 32-bit integer. 
        public Int32 Value;

        /// <summary>
        /// Parse the PtypInteger32 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypInteger32 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = ReadINT32();
        }
    }

    ///  <summary>
    /// 4 bytes; a 32-bit floating point number. [MS-DTYP]: FLOAT
    /// </summary>
    public class PtypFloating32 : BaseStructure
    {
        // 32-bit floating point number.
        public float Value;

        /// <summary>
        /// Parse the PtypFloating32 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloating32 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (float)ReadINT32();
        }
    }

    ///  <summary>
    /// 8 bytes; a 64-bit floating point number. [MS-DTYP]: DOUBLE
    /// </summary>
    public class PtypFloating64 : BaseStructure
    {
        // 64-bit floating point number. 
        public double Value;

        /// <summary>
        /// Parse the PtypFloating64 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloating64 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (double)ReadINT64();
        }
    }

    ///  <summary>
    /// 8 bytes; a 64-bit signed, scaled integer representation of a decimal currency value, with four places to the right of the decimal point. [MS-DTYP]: LONGLONG, [MS-OAUT]: CURRENCY
    /// </summary>
    public class PtypCurrency : BaseStructure
    {
        // 64-bit signed, scaled integer representation of a decimal currency value
        public Int64 Value;

        /// <summary>
        /// Parse the PtypCurrency structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypCurrency structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = ReadINT64();
        }
    }

    ///  <summary>
    /// 8 bytes; a 64-bit floating point number. 
    /// </summary>
    public class PtypFloatingTime : BaseStructure
    {
        // 64-bit floating point number. 
        public double Value;

        /// <summary>
        /// Parse the PtypFloatingTime structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloatingTime structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (double)ReadINT64();
        }
    }

    /// <summary>
    /// 4 bytes; a 32-bit integer encoding error information as specified in section 2.4.1.
    /// </summary>
    public class PtypErrorCode : BaseStructure
    {
        // 32-bit integer encoding error information.
        public AdditionalErrorCodes Value;

        /// <summary>
        /// Parse the PtypErrorCode structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypErrorCode structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = (AdditionalErrorCodes)ReadUint();
        }
    }

    ///  <summary>
    /// 1 byte; restricted to 1 or 0.
    /// </summary>
    public class PtypBoolean : BaseStructure
    {
        // 1 byte; restricted to 1 or 0.
        public Boolean Value;

        /// <summary>
        /// Parse the PtypBoolean structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypBoolean structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = ReadBoolean();
        }
    }

    /// <summary>
    /// 8 bytes; a 64-bit integer.[MS-DTYP]: LONGLONG.
    /// </summary>
    public class PtypInteger64 : BaseStructure
    {
        // 64-bit integer.
        public Int64 Value;

        /// <summary>
        /// Parse the PtypInteger64 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypInteger64 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = ReadINT64();
        }
    }

    ///  <summary>
    /// Variable size; a string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
    /// </summary>
    public class PtypString : BaseStructure
    {
        // A string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
        public MAPIString Value;

        /// <summary>
        /// Parse the PtypString structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypString structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = new MAPIString(Encoding.Unicode);
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// Variable size; a string of multibyte characters in externally specified encoding with terminating null character (single 0 byte).
    /// </summary>
    public class PtypString8 : BaseStructure
    {
        // A string of multibyte characters in externally specified encoding with terminating null character (single 0 byte).
        public MAPIString Value;

        /// <summary>
        /// Parse the PtypString8 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypString8 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = new MAPIString(Encoding.ASCII);
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// 8 bytes; a 64-bit integer representing the number of 100-nanosecond intervals since January 1, 1601.[MS-DTYP]: FILETIME.
    /// </summary>
    public class PtypTime : BaseStructure
    {
        // 64-bit integer representing the number of 100-nanosecond intervals since January 1, 1601.[MS-DTYP]: FILETIME.
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
                ulong temp = ReadUlong();
                DateTime startdate = new DateTime(1601, 1, 1).AddMilliseconds(temp / 10000);
                this.Value = startdate.ToLocalTime();
            }
            catch (ArgumentOutOfRangeException ex)
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
        // A GUID value.
        public Guid Value;

        /// <summary>
        /// Parse the PtypGuid structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypGuid structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = ReadGuid();
        }
    }

    ///  <summary>
    /// Variable size; a 16-bit COUNT field followed by a structure as specified in section 2.11.1.4.
    /// </summary>
    public class PtypServerId : BaseStructure
    {
        // The COUNT values are typically used to specify the size of an associated field.
        public ushort Count;

        //  The value 0x01 indicates the remaining bytes conform to this structure; 
        public byte Ours;

        // A Folder ID structure, as specified in section 2.2.1.1.
        public FolderID FolderID;

        // A Message ID structure, as specified in section 2.2.1.2, identifying a message in a folder identified by an associated folder ID. 
        public MessageID MessageID;

        // An unsigned instance number within an array of ServerIds to compare against. 
        public uint? Instance;

        // The Ours value 0x00 indicates this is a client-defined value and has whatever size and structure the client has defined.
        public byte?[] ClientData;

        /// <summary>
        /// Parse the PtypServerId structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypServerId structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Count = ReadUshort();
            this.Ours = ReadByte();
            if (this.Ours == 0x01)
            {
                this.FolderID = new FolderID();
                this.FolderID.Parse(s);
                this.MessageID = new MessageID();
                this.MessageID.Parse(s);
                this.Instance = ReadUint();
            }
            else
            {
                this.ClientData = ConvertArray(ReadBytes(this.Count - 1));
            }

        }
    }

    ///  <summary>
    /// Variable size; a byte array representing one or more Restriction structures as specified in section 2.12.
    /// </summary>
    public class PtypRestriction : RestrictionType
    {
        //None, class PtypRestriction is same as RestrictionType.
    }

    ///  <summary>
    /// Variable size; a 16-bit COUNT field followed by that many rule action structures, as specified in [MS-OXORULE] section 2.2.5.
    /// </summary>
    public class PtypRuleAction : RuleAction
    {
        //None, class PtypRuleAction is same as RuleAction.
    }

    ///  <summary>
    /// The enum of the ptyp type Count wide : 16 bits wide or 32 bits wide.
    /// </summary>
    public enum CountWideEnum : uint
    {
        twoBytes = 2,
        fourBytes = 4
    }

    ///  <summary>
    /// The help method to read the Count of ptyp type.
    /// </summary>
    public class HelpMethod : BaseStructure
    {
        /// <summary>
        /// The method to read the Count of ptyp type.
        /// </summary>
        /// <param name="countWide">The count wide.</param>
        /// <param name="s">The stream contain the COUNT</param>
        /// <returns>The COUNT value.</returns>
        public object ReadCount(CountWideEnum countWide, Stream s)
        {
            base.Parse(s);
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    {
                        return ReadUshort();
                    }
                case CountWideEnum.fourBytes:
                    {
                        return ReadUint();
                    }
                default:
                    return ReadUshort(); ;
            }
        }

        /// <summary>
        /// Format the error codes.
        /// </summary>
        /// <param name="errorCodeUint">The uint error code</param>
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
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many bytes.
    /// </summary>
    public class PtypBinary : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The binary value.
        public byte[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypBinary type.</param>
        public PtypBinary(CountWideEnum wide)
        {
            countWide = wide;
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
            this.Value = ReadBytes(this.Count.GetHashCode());
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypInteger16 values.
    /// </summary>
    public class PtypMultipleInteger16 : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // Workaround, need to update once the COUNT wide of PtypMultipleBinary is confirmed.
        public ushort undefinedCount;

        // The Int16 value.
        public Int16[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleInteger16 type.</param>
        public PtypMultipleInteger16(CountWideEnum wide)
        {
            countWide = wide;
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
            this.undefinedCount = ReadUshort();
            List<Int16> tempvalue = new List<Int16>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(ReadINT16());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypInteger32 values.
    /// </summary>
    public class PtypMultipleInteger32 : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // Workaround, need to update once the COUNT wide of PtypMultipleBinary is confirmed.
        public ushort undefinedCount;

        // The Int32 value.
        public Int32[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleInteger32 type.</param>
        public PtypMultipleInteger32(CountWideEnum wide)
        {
            countWide = wide;
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
            this.undefinedCount = ReadUshort();
            List<Int32> tempvalue = new List<Int32>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(ReadINT32());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypFloating32 values.
    /// </summary>
    public class PtypMultipleFloating32 : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The float value.
        public float[] Value;

        // The Count wide size.
        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleFloating32 type.</param>
        public PtypMultipleFloating32(CountWideEnum wide)
        {
            countWide = wide;
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
                tempvalue.Add(ReadINT32());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    /// <summary>
    /// Variable size; a COUNT field followed by that many PtypFloating64 values.
    /// </summary>
    public class PtypMultipleFloating64 : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The arrary of double value.
        public double[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleFloating64 type.</param>
        public PtypMultipleFloating64(CountWideEnum wide)
        {
            countWide = wide;
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
                tempvalue.Add(ReadINT64());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypCurrency values.
    /// </summary>
    public class PtypMultipleCurrency : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The arrary of Int64 value.
        public Int64[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleCurrency type.</param>
        public PtypMultipleCurrency(CountWideEnum wide)
        {
            countWide = wide;
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
            List<Int64> tempvalue = new List<Int64>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(ReadINT64());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypFloatingTime values.
    /// </summary>
    public class PtypMultipleFloatingTime : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The arrary of double value.
        public double[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleFloatingTime type.</param>
        public PtypMultipleFloatingTime(CountWideEnum wide)
        {
            countWide = wide;
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
                tempvalue.Add(ReadINT64());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypInteger64 values.
    /// </summary>
    public class PtypMultipleInteger64 : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The arrary of Int64 value.
        public Int64[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleInteger64 type.</param>
        public PtypMultipleInteger64(CountWideEnum wide)
        {
            countWide = wide;
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
            List<Int64> tempvalue = new List<Int64>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                tempvalue.Add(ReadINT64());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypString values.
    /// </summary>
    public class PtypMultipleString : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // TDI #99147
        public ushort undefinedCount;

        // The arrary of string value.
        public MAPIString[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleString type.</param>
        public PtypMultipleString(CountWideEnum wide)
        {
            countWide = wide;
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
            this.undefinedCount = ReadUshort();
            List<MAPIString> tempvalue = new List<MAPIString>();
            MAPIString str;
            for (int i = 0; i < this.Count.GetHashCode();i++ )
            {
                str = new MAPIString(Encoding.Unicode);
                str.Parse(s);
                tempvalue.Add(str);
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypString8 values.
    /// </summary>
    public class PtypMultipleString8 : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The arrary of string value.
        public MAPIString[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleString8 type.</param>
        public PtypMultipleString8(CountWideEnum wide)
        {
            countWide = wide;
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

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypTime values.
    /// </summary>
    public class PtypMultipleTime : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The arrary of time value.
        public PtypTime[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleTime type.</param>
        public PtypMultipleTime(CountWideEnum wide)
        {
            countWide = wide;
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

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypGuid values.
    /// </summary>
    public class PtypMultipleGuid : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // The arrary of GUID value.
        public Guid[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleGuid type.</param>
        public PtypMultipleGuid(CountWideEnum wide)
        {
            countWide = wide;
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
                tempvalue.Add(ReadGuid());
            }
            this.Value = tempvalue.ToArray();
        }
    }

    ///  <summary>
    /// Variable size; a COUNT field followed by that many PtypBinary values.
    /// </summary>
    public class PtypMultipleBinary : BaseStructure
    {
        // COUNT values are typically used to specify the size of an associated field.
        public object Count;

        // Workaround, need to update once the COUNT wide of PtypMultipleBinary is confirmed.
        ushort undefinedCount;

        // The arrary of binary value.
        public PtypBinary[] Value;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleBinary type.</param>
        public PtypMultipleBinary(CountWideEnum wide)
        {
            countWide = wide;
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
            this.undefinedCount = ReadUshort();
            List<PtypBinary> tempvalue = new List<PtypBinary>();
            for (int i = 0; i < this.Count.GetHashCode(); i++)
            {
                PtypBinary binary = new PtypBinary(CountWideEnum.twoBytes);
                binary.Parse(s);
                tempvalue.Add(binary);
            }
            this.Value = tempvalue.ToArray();

        }
    }

    ///  <summary>
    /// Any: this property type value matches any type; 
    /// </summary>
    public class PtypUnspecified : BaseStructure
    {
        /// <summary>
        /// The constructor method.
        /// </summary>
        public PtypUnspecified()
        {
            throw new Exception("MSOXCDATA: Not implemented type definition - PtypUnspecified");
        }
    }

    ///  <summary>
    /// None: This property is a placeholder.
    /// </summary>
    public class PtypNull : BaseStructure
    {
        // The null value.
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
        /// The Constructor method.
        /// </summary>
        public PtypObject_Or_PtypEmbeddedTable()
        {
            throw new Exception("MSOXCDATA: Not implemented type definition - PtypObject_Or_PtypEmbeddedTable");
        }
    }

    #endregion

    #region 2.11.2	Property Value Structures

    /// <summary>
    /// 2.11.2	Property Value Structures
    /// </summary>
    public class PropertyValue : BaseStructure
    {
        // A PropertyValue structure, as specified in section 2.11.2. The value MUST be compatible with the value of the PropertyType field.
        public object Value;

        // The Count wide size of ptypMutiple type.
        private CountWideEnum countWide;

        // An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        private PropertyDataType PropertyType;

        /// <summary>
        /// The Constructor to set the property type and Count wide size.
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public PropertyValue(PropertyDataType ProType, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
            PropertyType = ProType;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyValue class without parameters.
        /// </summary>
        public PropertyValue()
        {
        }

        /// <summary>
        /// Parse the PropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyValue structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = this.ReadPropertyValue(this.PropertyType, s, countWide);
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
                        PtypServerId tempPropertyValue = new PtypServerId();
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
                        PtypMultipleString tempPropertyValue = new PtypMultipleString(ptypMultiCountSize);
                        tempPropertyValue.Parse(s);
                        propertyValue = tempPropertyValue;
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
    /// 2.11.3	TypedPropertyValue Structure 
    /// </summary>
    public class TypedPropertyValue : BaseStructure
    {
        // An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        public PropertyDataType PropertyType;

        // A PropertyValue structure, as specified in section 2.11.2. The value MUST be compatible with the value of the PropertyType field.
        public object PropertyValue;

        // The Count wide size of ptypMutiple type.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public TypedPropertyValue(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
        }
        /// <summary>
        /// Parse the TypedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing the TypedPropertyValue structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = ConvertToPropType(ReadUshort());
            PropertyValue propertyValue = new PropertyValue();
            this.PropertyValue = propertyValue.ReadPropertyValue(this.PropertyType, s, countWide);

        }
    }
    #endregion

    #region 2.11.4	TaggedPropertyValue Structure
    /// <summary>
    /// 2.11.4	TaggedPropertyValue Structure
    /// </summary>
    public class TaggedPropertyValue : BaseStructure
    {
        // A PropertyTag structure, as specified in section 2.9, giving the values of the PropertyId and PropertyType fields for the property.
        public PropertyTag PropertyTag;

        // A PropertyValue structure, as specified in section 2.11.2.1. specifying the value of the property. 
        public object PropertyValue;

        // The Constructor to set the Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="ptypMultiCountSize">The Constructor to set the Count wide siz.</param>
        public TaggedPropertyValue(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
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
            this.PropertyValue = propertyValue.ReadPropertyValue(this.PropertyTag.PropertyType, s, countWide);
        }
    }
    #endregion

    #region 2.11.5	FlaggedPropertyValue Structure
    /// <summary>
    /// 2.11.5	FlaggedPropertyValue Structure
    /// </summary>
    public class FlaggedPropertyValue : BaseStructure
    {
        // An unsigned integer. This value of this flag determines what is conveyed in the PropertyValue field. 
        public byte Flag;

        // A PropertyValue structure, as specified in section 2.11.2.1, unless the Flag field is set to 0x1.
        public object PropertyValue;

        // The Property data type.
        private PropertyDataType PropertyType;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Property data type and the Count wide size.
        /// </summary>
        /// <param name="propertyType">The Property data type.</param>
        /// <param name="ptypMultiCountSize">The Count wide size.</param>
        public FlaggedPropertyValue(PropertyDataType propertyType, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            PropertyType = propertyType;
            countWide = ptypMultiCountSize;
        }
        /// <summary>
        /// Parse the FlaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing the FlaggedPropertyValue structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flag = ReadByte();
            if (this.Flag == 0x00)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(this.PropertyType, s, countWide);
            }
            else if (this.Flag == 0x0A)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(PropertyDataType.PtypErrorCode, s, countWide);
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
    /// 2.11.6	FlaggedPropertyValueWithType Structure
    /// </summary>
    public class FlaggedPropertyValueWithType : BaseStructure
    {
        // An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        public PropertyDataType PropertyType;

        // An unsigned integer. This flag MUST be set one of three possible values: 0x0, 0x1, or 0xA, which determines what is conveyed in the PropertyValue field. 
        public byte Flag;

        // A PropertyValue structure, as specified in section 2.11.2.1, unless the Flag field is set to 0x1. 
        public object PropertyValue;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        ///  The Constructor to set the Count wide size.
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size.</param>
        public FlaggedPropertyValueWithType(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
        }
        /// <summary>
        /// Parse the FlaggedPropertyValueWithType structure.
        /// </summary>
        /// <param name="s">A stream containing the FlaggedPropertyValueWithType structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = ConvertToPropType(ReadUshort());
            this.Flag = ReadByte();
            if (this.Flag == 0x00)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(this.PropertyType, s, countWide);
            }
            else if (this.Flag == 0x0A)
            {
                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(PropertyDataType.PtypErrorCode, s, countWide);
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
    /// The enum value of StringType
    /// </summary>
    public enum StringTypeEnum : byte
    {
        NoPresent = 0x00,
        Empty = 0x01,
        CharacterString = 0x02,
        ReducedUnicodeCharacterString = 0x03,
        UnicodeCharacterString = 0x04
    }

    ///  <summary>
    /// 2.11.7	TypedString Structure
    /// </summary>
    public class TypedString : BaseStructure
    {
        // An enum value of StringType
        public StringTypeEnum StringType;

        // If the StringType field is set to 0x02, 0x03, or 0x04, then this field MUST be present and in the format specified by the Type field. Otherwise, this field MUST NOT be present.
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
    /// The enum vlaue of restriction value.
    /// </summary>
    public enum RestrictTypeEnum : byte
    {
        AndRestriction = 0x00,
        OrRestriction = 0x01,
        NotRestriction = 0x02,
        ContentRestriction = 0x03,
        PropertyRestriction = 0x04,
        ComparePropertiesRestriction = 0x05,
        BitMaskRestriction = 0x06,
        SizeRestriction = 0x07,
        ExistRestriction = 0x08,
        SubObjectRestriction = 0x09,
        CommentRestriction = 0x0A,
        CountRestriction = 0x0B
    }

    /// <summary>
    ///  2.12   Restrictions
    /// </summary>
    public class RestrictionType : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction.
        public object Restriction;

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
                        AndRestriction restriction = new AndRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.OrRestriction:
                    {
                        OrRestriction restriction = new OrRestriction();
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
                        ContentRestriction restriction = new ContentRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }
                case RestrictTypeEnum.PropertyRestriction:
                    {
                        PropertyRestriction restriction = new PropertyRestriction();
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
                        SubObjectRestriction restriction = new SubObjectRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }
                case RestrictTypeEnum.CommentRestriction:
                    {
                        CommentRestriction restriction = new CommentRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }
                case RestrictTypeEnum.CountRestriction:
                    {
                        CountRestriction restriction = new CountRestriction();
                        restriction.Parse(s);
                        this.Restriction = restriction;
                        break;
                    }
                default:
                    break;
            }
        }
    }

    ///  <summary>
    /// 2.12.1	And Restriction Structures
    /// </summary>
    public class AndRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x00.
        public RestrictTypeEnum RestrictType;

        // This value specifies how many restriction structures are present in the Restricts field. The width of this field is 16 bits in the context of ROPs and 32 bits in the context of extended rules.
        public object RestrictCount;

        // An array of restriction structures. 
        public RestrictionType[] Restricts;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size and restrict type.
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public AndRestriction(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
        }
        /// <summary>
        /// Parse the AndRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the AndRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            if (countWide == CountWideEnum.twoBytes)
            {
                this.RestrictCount = ReadUshort();
            }
            else
            {
                this.RestrictCount = ReadUint();
            }
            List<RestrictionType> tempRestricts = new List<RestrictionType>();
            for (int length = 0; length < RestrictCount.GetHashCode(); length++)
            {
                RestrictionType tempRestriction = new RestrictionType();
                tempRestriction.Parse(s);
                tempRestricts.Add(tempRestriction);

            }
            this.Restricts = tempRestricts.ToArray();
        }
    }

    ///  <summary>
    /// 2.12.2.1	OrRestriction Structure
    /// </summary>
    public class OrRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x01.
        public RestrictTypeEnum RestrictType;

        // This value specifies how many restriction structures are present in the Restricts field. The width of this field is 16 bits in the context of ROPs and 32 bits in the context of extended rules.
        public object RestrictCount;

        // An array of restriction structures. This field MUST contain the number of structures indicated by the RestrictCount field.
        public RestrictionType[] Restricts;

        // The Count wide size.
        private CountWideEnum countWide;

        /// <summary>
        /// The Constructor to set the Count wide size and restrict type.
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public OrRestriction(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
        }
        /// <summary>
        /// Parse the OrRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the OrRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            if (countWide == CountWideEnum.twoBytes)
            {
                this.RestrictCount = ReadUshort();
            }
            else
            {
                this.RestrictCount = ReadUint();
            }
            List<RestrictionType> tempRestricts = new List<RestrictionType>();
            for (int length = 0; length < RestrictCount.GetHashCode(); length++)
            {
                RestrictionType tempRestriction = new RestrictionType();
                tempRestriction.Parse(s);
                tempRestricts.Add(tempRestriction);

            }
            this.Restricts = tempRestricts.ToArray();
        }
    }

    ///  <summary>
    /// 2.12.3	Not Restriction Structures
    /// </summary>
    public class NotRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x02.
        public RestrictTypeEnum RestrictType;

        // A restriction structure. This value specifies the restriction (2) that the logical NOT operation applies to.
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
    /// The enum of FuzzyLevelLow.
    /// </summary>
    public enum FuzzyLevelLowEnum : ushort
    {
        FL_FULLSTRING = 0x0000,
        FL_SUBSTRING = 0x0001,
        FL_PREFIX = 0x0002
    }

    ///  <summary>
    /// The enum of FuzzyLevelHighEnum.
    /// </summary>
    public enum FuzzyLevelHighEnum : ushort
    {

        FL_IGNORECASE = 0x00001,
        FL_IGNORENONSPACE = 0x0002,
        FL_LOOSE = 0x0004

    }

    ///  <summary>
    /// 2.12.4	Content Restriction Structures
    /// </summary>
    public class ContentRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x03.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This field specifies the level of precision that the server enforces when checking for a match against a ContentRestriction structure.
        public FuzzyLevelLowEnum FuzzyLevelLow;

        // This field applies only to string-value properties. 
        public FuzzyLevelHighEnum FuzzyLevelHigh;

        // This value indicates the property tag of the column whose value MUST be matched against the value specified in the TaggedValue field.
        public PropertyTag PropertyTag;

        // A TaggedPropertyValue structure, as specified in section 2.11.4. 
        public TaggedPropertyValue TaggedValue;

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
            this.TaggedValue = new TaggedPropertyValue();
            this.TaggedValue.Parse(s);
        }
    }

    ///  <summary>
    /// The enum type of RelOp.
    /// </summary>
    public enum RelOpType : byte
    {
        RelationalOperatorLessThan = 0x00,
        RelationalOperatorLessThanOrEqual = 0x01,
        RelationalOperatorGreaterThan = 0x02,
        RelationalOperatorGreaterThanOrEqual = 0x03,
        RelationalOperatorEqual = 0x04,
        RelationalOperatorNotEqual = 0x5,
        RelationalOperatorMemberOfDL = 0x64
    }

    ///  <summary>
    /// 2.12.5	Property Restriction Structures
    /// </summary>
    public class PropertyRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x4.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This value indicates the relational operator that is used to compare the property on the object with the value of the TaggedValue field. 
        public RelOpType RelOp;

        // An unsigned integer. This value indicates the property tag of the property that MUST be compared.
        public uint PropTag;

        // A TaggedValue structure, as specified in section 2.11.4. 
        public TaggedPropertyValue TaggedValue;

        /// <summary>
        /// Parse the PropertyRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.RelOp = (RelOpType)ReadByte();
            this.PropTag = ReadUint();
            this.TaggedValue = new TaggedPropertyValue();
            this.TaggedValue.Parse(s);
        }
    }

    ///  <summary>
    /// 2.12.6	Compare Properties Restriction Structures
    /// </summary>
    public class ComparePropertiesRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x05.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This value indicates the relational operator used to compare the two properties. 
        public RelOpType RelOp;

        // An unsigned integer. This value is the property tag of the first property that MUST be compared.
        public uint PropTag1;

        // An unsigned integer. This value is the property tag of the second property that MUST be compared.
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
            this.PropTag1 = ReadUint();
            this.PropTag2 = ReadUint();
        }
    }

    ///  <summary>
    /// The enum type of BitmapRelOp.
    /// </summary>
    public enum BitmapRelOpType : byte
    {
        BMR_EQZ = 0x00,
        BMR_NEZ = 0x01
    }

    ///  <summary>
    /// 2.12.7	Bitmask Restriction Structures
    /// </summary>
    public class BitMaskRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x06.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This value specifies how the server MUST perform the masking operation. 
        public BitmapRelOpType BitmapRelOp;

        // An unsigned integer. This value is the property tag of the property to be tested. 
        public PtypInteger32 PropTag;

        // An unsigned integer. The bitmask to be used for the AND operation.
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
            this.Mask = ReadUint();
        }
    }

    ///  <summary>
    /// 2.12.8	Size Restriction Structures
    /// </summary>
    public class SizeRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x07.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This value indicates the relational operator used in the size comparison.
        public RelOpType RelOp;

        // An unsigned integer. This value indicates the property tag of the property whose value size is being tested.
        public uint PropTag;

        // An unsigned integer. This value indicates the size, in bytes, that is to be used in the comparison.
        public uint Size;

        /// <summary>
        /// Parse the SizeRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the SizeRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.RelOp = (RelOpType)ReadByte();
            this.PropTag = ReadUint();
            this.Size = ReadUint();
        }
    }

    ///  <summary>
    /// 2.12.9	Exist Restriction Structures
    /// </summary>
    public class ExistRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x08.
        public RestrictTypeEnum RestrictType;

        // This value encodes the PropTag field of the SizeRestriction structure. 
        public uint PropTag;

        /// <summary>
        /// Parse the ExistRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the ExistRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.PropTag = ReadUint();
        }
    }

    ///  <summary>
    /// 2.12.10	Subobject Restriction Structures
    /// </summary>
    public class SubObjectRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x09.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This value is a property tag that designates the target of the subrestriction. 
        public uint Subobject;

        // A Restriction structure. 
        public RestrictionType Restriction;

        /// <summary>
        /// Parse the SubObjectRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the SubObjectRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.Subobject = ReadUint();
            this.Restriction = new RestrictionType();
            this.Restriction.Parse(s);
        }
    }

    ///  <summary>
    /// 2.12.11	CommentRestriction Structure
    /// </summary>
    public class CommentRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x0A.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This value specifies how many TaggedValue structures are present in the TaggedValues field.
        public byte TaggedValuesCount;

        //  An array of TaggedPropertyValue structures, as specified in section 2.11.4. 
        public TaggedPropertyValue[] TaggedValues;

        // An unsigned integer. This field MUST contain either TRUE (0x01) or FALSE (0x00). 
        public bool RestrictionPresent;

        // A Restriction structure. This field is present only if RestrictionPresent is TRUE.
        public RestrictionType Restriction;

        /// <summary>
        /// Parse the CommentRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the CommentRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.TaggedValuesCount = ReadByte();
            List<TaggedPropertyValue> tempTaggedValue = new List<TaggedPropertyValue>();
            for (int i = 0; i < this.TaggedValuesCount; i++)
            {
                TaggedPropertyValue tempproperty = new TaggedPropertyValue();
                tempproperty.Parse(s);
                tempTaggedValue.Add(tempproperty);
            }
            this.TaggedValues = tempTaggedValue.ToArray();
            this.RestrictionPresent = ReadBoolean();
            if (this.RestrictionPresent == true)
            {
                this.Restriction = new RestrictionType();
                this.Restriction.Parse(s);
            }
        }
    }

    ///  <summary>
    /// 2.12.12	CountRestriction Structure
    /// </summary>
    public class CountRestriction : BaseStructure
    {
        // An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x0B.
        public RestrictTypeEnum RestrictType;

        // An unsigned integer. This value specifies the limit on the number of matches to be returned when the value of the SubRestriction field is evaluated.
        public uint Count;

        // A restriction structure. This field specifies the restriction (2) to be limited.
        public RestrictionType SubRestriction;

        /// <summary>
        /// Parse the CountRestriction structure.
        /// </summary>
        /// <param name="s">A stream containing the CountRestriction structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RestrictType = (RestrictTypeEnum)ReadByte();
            this.Count = ReadUint();
            this.SubRestriction = new RestrictionType();
            this.SubRestriction.Parse(s);
        }
    }
    #endregion

    #region 2.13	Table Sorting Structures
    /// <summary>
    /// The enum value of Order type.
    /// </summary>
    public enum OrderType : byte
    {
        Ascending = 0x00,
        Descending = 0x01,
        MaximumCategory = 0x04
    }

    ///  <summary>
    /// 2.13.1	SortOrder Structure
    /// </summary>
    public class SortOrder : BaseStructure
    {
        // This value identifies the data type of the column to be used for sorting. 
        public PropertyDataType PropertyType;

        // This value identifies the column to be used for sorting.
        public PidTagPropertyEnum PropertyId;

        // The order type.
        public OrderType Order;

        /// <summary>
        /// Parse the SortOrder structure.
        /// </summary>
        /// <param name="s">A stream containing the SortOrder structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)ReadUshort();
            this.PropertyId = (PidTagPropertyEnum)ReadUshort();
            this.Order = (OrderType)ReadByte();
        }
    }

    ///  <summary>
    /// 2.13.2	SortOrderSet Structure
    /// </summary>
    public class SortOrderSet : BaseStructure
    {
        // An unsigned integer. This value specifies how many SortOrder structures are present in the SortOrders field.
        public ushort SortOrderCount;

        // An unsigned integer. This value specifies that the first CategorizedCount columns are categorized. 
        public ushort CategorizedCount;

        // An unsigned integer. This value specifies that the first ExpandedCount field in the categorized columns starts in an expanded state in which all of the rows that apply to the category are visible in the table view. 
        public ushort ExpandedCount;

        // An array of SortOrder structures. This field MUST contain the number of structures indicated by the value of the SortOrderCount field. 
        public SortOrder[] SortOrders;

        /// <summary>
        /// Parse the SortOrderSet structure.
        /// </summary>
        /// <param name="s">A stream containing the SortOrderSet structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SortOrderCount = ReadUshort();
            this.CategorizedCount = ReadUshort();
            this.ExpandedCount = ReadUshort();
            List<SortOrder> tempSortOrders = new List<SortOrder>();
            for (int i = 0; i < this.SortOrderCount; i++)
            {
                SortOrder SortOrder = new SortOrder();
                SortOrder.Parse(s);
                tempSortOrders.Add(SortOrder);
            }
            this.SortOrders = tempSortOrders.ToArray();
        }
    }
    #endregion

}
