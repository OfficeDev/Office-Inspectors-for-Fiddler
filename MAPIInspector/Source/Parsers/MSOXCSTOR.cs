namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    #region Enum
    /// <summary>
    /// The enum type for flags that control the behavior of the RopLogon.
    /// </summary>
    [Flags]
    public enum LogonFlags : byte
    {
        /// <summary>
        /// This flag is set for logon to a private mailbox and is not set for logon to public folders.
        /// </summary>
        Private = 0x01,

        /// <summary>
        /// Undercover flag
        /// </summary>
        Undercover = 0x02,

        /// <summary>
        /// This flag is ignored by the server
        /// </summary>
        Ghosted = 0x04,

        /// <summary>
        /// This flag is ignored by the server.
        /// </summary>
        SpoolerProcess = 0x08
    }

    /// <summary>
    /// The enum type for additional flags that control the behavior of the RopLogon.
    /// </summary>
    [Flags]
    public enum OpenFlags : uint
    {
        /// <summary>
        /// A request for administrative access to the mailbox. 
        /// </summary>
        USE_ADMIN_PRIVILEGE = 0x00000001,

        /// <summary>
        /// A request to open a public folders message store. This flag MUST be set for public logons.
        /// </summary>
        PUBLIC = 0x00000002,

        /// <summary>
        /// This flag is ignored
        /// </summary>
        HOME_LOGON = 0x00000004,

        /// <summary>
        /// This flag is ignored
        /// </summary>
        TAKE_OWNERSHIP = 0x00000008,

        /// <summary>
        /// Requests a private server to provide an alternate public server.
        /// </summary>
        ALTERNATE_SERVER = 0x00000100,

        /// <summary>
        /// This flag allows the client to log on to a public message store that is not the user's default public message store
        /// </summary>
        IGNORE_HOME_MDB = 0x00000200,

        /// <summary>
        /// A request for a nonmessaging logon session
        /// </summary>
        NO_MAIL = 0x00000400,

        /// <summary>
        /// For a private-mailbox logon this flag SHOULD be set
        /// </summary>
        USE_PER_MDB_REPLID_MAPPING = 0x01000000,

        /// <summary>
        /// Indicates that the client supports asynchronous processing of RopSetReadFlags
        /// </summary>
        SUPPORT_PROGRESS = 0x20000000
    }

    /// <summary>
    /// The enum type for flags that provide details about the state of the mailbox.
    /// </summary>
    [Flags]
    public enum ResponseFlags : byte
    {
        /// <summary>
        /// This bit MUST be set and MUST be ignored by the client
        /// </summary>
        Reserved = 0x01,

        /// <summary>
        /// The user has owner permission on the mailbox.
        /// </summary>
        OwnerRight = 0x02,

        /// <summary>
        /// The user has the right to send mail from the mailbox.
        /// </summary>
        SendAsRight = 0x04,

        /// <summary>
        /// The Out of Office (OOF) state is set on the mailbox
        /// </summary>
        OOF = 0x10
    }

    /// <summary>
    /// The enum type for days of the week.
    /// </summary>
    public enum DayOfWeek : byte
    {
        /// <summary>
        /// Sunday flag
        /// </summary>
        Sunday = 0x00,

        /// <summary>
        /// Monday flag
        /// </summary>
        Monday = 0x01,

        /// <summary>
        /// Tuesday flag
        /// </summary>
        Tuesday = 0x02,

        /// <summary>
        /// Wednesday flag
        /// </summary>
        Wednesday = 0x03,

        /// <summary>
        /// Thursday flag
        /// </summary>
        Thursday = 0x04,

        /// <summary>
        /// Friday flag
        /// </summary>
        Friday = 0x05,

        /// <summary>
        /// Saturday flag
        /// </summary>
        Saturday = 0x06
    }

    /// <summary>
    /// The enum type for months of a year.
    /// </summary>
    public enum Month : byte
    {
        /// <summary>
        /// January flag
        /// </summary>
        January = 0x01,

        /// <summary>
        /// February flag
        /// </summary>
        February = 0x02,

        /// <summary>
        /// March flag
        /// </summary>
        March = 0x03,

        /// <summary>
        /// April flag
        /// </summary>
        April = 0x04,

        /// <summary>
        /// May flag
        /// </summary>
        May = 0x05,

        /// <summary>
        /// June flag
        /// </summary>
        June = 0x06,

        /// <summary>
        /// July flag
        /// </summary>
        July = 0x07,

        /// <summary>
        /// August flag
        /// </summary>
        August = 0x08,

        /// <summary>
        /// September flag
        /// </summary>
        September = 0x09,

        /// <summary>
        /// October flag
        /// </summary>
        October = 0x0A,

        /// <summary>
        /// November flag
        /// </summary>
        November = 0x0B,

        /// <summary>
        /// December flag
        /// </summary>
        December = 0x0C
    }

    /// <summary>
    ///  A class indicates the RopLogon time.
    /// </summary>
    public class LogonTime : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the current second.
        /// </summary>
        public byte Seconds;

        /// <summary>
        /// An unsigned integer that specifies the current Minutes.
        /// </summary>
        public byte Minutes;

        /// <summary>
        /// An unsigned integer that specifies the current Hour.
        /// </summary>
        public byte Hour;

        /// <summary>
        /// An enumeration that specifies the current day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek;

        /// <summary>
        /// An unsigned integer that specifies the current day of the month.
        /// </summary>
        public byte Day;

        /// <summary>
        /// An unsigned integer that specifies the current month 
        /// </summary>
        public Month Month;

        /// <summary>
        /// An unsigned integer that specifies the current year.
        /// </summary>
        public ushort Year;

        /// <summary>
        /// Parse the LogonTime structure.
        /// </summary>
        /// <param name="s">A stream containing LogonTime structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Seconds = this.ReadByte();
            this.Minutes = this.ReadByte();
            this.Hour = this.ReadByte();
            this.DayOfWeek = (DayOfWeek)this.ReadByte();
            this.Day = this.ReadByte();
            this.Month = (Month)this.ReadByte();
            this.Year = this.ReadUshort();
        }
    }
    #endregion

    #region 2.2.1.1 RopLogon
    /// <summary>
    ///  A class indicates the RopLogon ROP Request Buffer.
    /// </summary>
    public class RopLogonRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the RopLogon.
        /// </summary>
        public LogonFlags LogonFlags;

        /// <summary>
        /// A flags structure that contains more flags that control the behavior of the RopLogon.
        /// </summary>
        public OpenFlags OpenFlags;

        /// <summary>
        /// A flags structure. This field is not used and is ignored by the server.
        /// </summary>
        public uint StoreState;

        /// <summary>
        /// An unsigned integer that specifies the size of the ESSDN field.
        /// </summary>
        public ushort EssdnSize;

        /// <summary>
        /// A null-terminated ASCII string that specifies which mailbox to log on to. 
        /// </summary>
        public MAPIString Essdn;

        /// <summary>
        /// Parse the RopLogonRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopLogonRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.LogonFlags = (LogonFlags)this.ReadByte();
            this.OpenFlags = (OpenFlags)this.ReadUint();
            this.StoreState = this.ReadUint();
            this.EssdnSize = this.ReadUshort();
            if (this.EssdnSize > 0)
            {
                this.Essdn = new MAPIString(Encoding.ASCII);
                this.Essdn.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopLogon ROP Response Buffer for private mailbox.
    /// </summary>
    public class RopLogonResponse_PrivateMailboxes : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A flags structure that contains flags that specify the type of RopLogon.
        /// </summary>
        public LogonFlags? LogonFlags;

        /// <summary>
        /// 13 64-bit identifiers that specify a set of special folders for a mailbox.
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// A flags structure that contains flags that provide details about the state of the mailbox. 
        /// </summary>
        public ResponseFlags? ResponseFlags;

        /// <summary>
        /// A GUID that identifies the mailbox on which the RopLogon was performed.
        /// </summary>
        public Guid? MailboxGuid;

        /// <summary>
        /// An identifier that specifies a replica ID for the RopLogon.
        /// </summary>
        public ushort? ReplId;

        /// <summary>
        /// A GUID that specifies the replica GUID that is associated with the replica ID.
        /// </summary>
        public Guid? ReplGuid;

        /// <summary>
        /// A LogonTime structure that specifies the time at which the RopLogon occurred. 
        /// </summary>
        public LogonTime LogonTime;

        /// <summary>
        /// An unsigned integer that contains a numeric value that tracks the currency of the Gateway Address Routing Table (GWART).
        /// </summary>
        public ulong? GwartTime;

        /// <summary>
        /// A flags structure.
        /// </summary>
        public uint? StoreState;

        /// <summary>
        /// The below two fields is defined for RopLogon redirect response in section 2.2.3.1.4 in MS-OXCROPS.
        /// An unsigned integer that specifies the length of the ServerName field.
        /// </summary>
        public byte? ServerNameSize;

        /// <summary>
        /// A null-terminated ASCII string that specifies a different server for the client to connect to.
        /// </summary>
        public MAPIString ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PrivateMailboxes structure.
        /// </summary>
        /// <param name="s">A stream containing RopLogonResponse_PrivateMailboxes structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.LogonFlags = (LogonFlags)this.ReadByte();
                this.FolderIds = new FolderID[13];
                for (int i = 0; i < 13; i++)
                {
                    this.FolderIds[i] = new FolderID();
                    this.FolderIds[i].Parse(s);
                }

                this.ResponseFlags = (ResponseFlags)this.ReadByte();
                this.MailboxGuid = this.ReadGuid();
                this.ReplId = this.ReadUshort();
                this.ReplGuid = this.ReadGuid();
                this.LogonTime = new LogonTime();
                this.LogonTime.Parse(s);
                this.GwartTime = this.ReadUlong();
                this.StoreState = this.ReadUint();
            }
            else if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.WrongServer)
            {
                this.LogonFlags = (LogonFlags)this.ReadByte();
                this.ServerNameSize = this.ReadByte();
                this.ServerName = new MAPIString(Encoding.ASCII);
                this.ServerName.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopLogon ROP Response Buffer for public folders.
    /// </summary>
    public class RopLogonResponse_PublicFolders : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A flags structure that contains flags that specify the type of RopLogon.
        /// </summary>
        public LogonFlags? LogonFlags;

        /// <summary>
        /// 13 64-bit identifiers that specify a set of special folders for a mailbox.
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// An identifier that specifies a replica ID for the RopLogon.
        /// </summary>
        public ushort? ReplId;

        /// <summary>
        /// A GUID that specifies the replica GUID associated with the replica ID that is specified in the ReplId field.
        /// </summary>
        public Guid? ReplGuid;

        /// <summary>
        /// This field is not used and is ignored by the client.
        /// </summary>
        public Guid? PerUserGuid;

        /// <summary>
        /// The below two fields is defined for RopLogon redirect response in section 2.2.3.1.4 in MS-OXCROPS.
        /// An unsigned integer that specifies the length of the ServerName field.
        /// </summary>
        public byte? ServerNameSize;

        /// <summary>
        /// A null-terminated ASCII string that specifies a different server for the client to connect to.
        /// </summary>
        public MAPIString ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PublicFolders structure.
        /// </summary>
        /// <param name="s">A stream containing RopLogonResponse_PublicFolders structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.LogonFlags = (LogonFlags)this.ReadByte();
                this.FolderIds = new FolderID[13];
                for (int i = 0; i < 13; i++)
                {
                    this.FolderIds[i] = new FolderID();
                    this.FolderIds[i].Parse(s);
                }

                this.ReplId = this.ReadUshort();
                this.ReplGuid = this.ReadGuid();
                this.PerUserGuid = this.ReadGuid();
            }
            else if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.WrongServer)
            {
                this.LogonFlags = (LogonFlags)this.ReadByte();
                this.ServerNameSize = this.ReadByte();
                this.ServerName = new MAPIString(Encoding.ASCII);
                this.ServerName.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.1.2 RopGetReceiveFolder
    /// <summary>
    ///  A class indicates the RopGetReceiveFolder ROP Request Buffer.
    /// </summary>
    public class RopGetReceiveFolderRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A null-terminated ASCII string that specifies the message class to find the Receive folder for.
        /// </summary>
        public MAPIString MessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetReceiveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageClass = new MAPIString(Encoding.ASCII);
            this.MessageClass.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopGetReceiveFolder ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An identifier that specifies the Receive folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies the message class that is actually configured for delivery to the folder.
        /// </summary>
        public MAPIString ExplicitMessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetReceiveFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
                this.ExplicitMessageClass = new MAPIString(Encoding.ASCII);
                this.ExplicitMessageClass.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.1.3 RopSetReceiveFolder
    /// <summary>
    ///  A class indicates the RopSetReceiveFolder ROP Request Buffer.
    /// </summary>
    public class RopSetReceiveFolderRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the Receive folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies which message class to set the Receive folder for.
        /// </summary>
        public MAPIString MessageClass;

        /// <summary>
        /// Parse the RopSetReceiveFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetReceiveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.MessageClass = new MAPIString(Encoding.ASCII);
            this.MessageClass.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopSetReceiveFolder ROP Response Buffer.
    /// </summary>
    public class RopSetReceiveFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetReceiveFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetReceiveFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.1.4 RopGetReceiveFolderTable
    /// <summary>
    ///  A class indicates the RopGetReceiveFolderTable  ROP Request Buffer.
    /// </summary>
    public class RopGetReceiveFolderTableRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetReceiveFolderTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetReceiveFolderTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetReceiveFolderTable ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderTableResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of row structures contained in the Rows field.
        /// </summary>
        public uint? RowCount;

        /// <summary>
        /// An array of row structures. This field contains the rows of the Receive folder table. Each row is returned in either a StandardPropertyRow or a FlaggedPropertyRow structure.
        /// </summary>
        public PropertyRow[] Rows;

        /// <summary>
        /// Parse the RopGetReceiveFolderTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetReceiveFolderTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            List<PropertyRow> tmpRows = new List<PropertyRow>();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = this.ReadUint();

                for (int i = 0; i < this.RowCount; i++)
                {
                    // PidTagMessageClass is defined as PtypString8 due to Open Specification said all characters in this property MUST be from the ASCII characters 0x20 through 0x7F. 
                    PropertyTag[] properties_GetReceiveFolderTable = new PropertyTag[3]
                    {
                      new PropertyTag(PropertyDataType.PtypInteger64, PidTagPropertyEnum.PidTagFolderId),
                      new PropertyTag(PropertyDataType.PtypString8, PidTagPropertyEnum.PidTagMessageClass),
                      new PropertyTag(PropertyDataType.PtypTime, PidTagPropertyEnum.PidTagLastModificationTime)
                    };
                    PropertyRow proRow = new PropertyRow(properties_GetReceiveFolderTable);
                    proRow.Parse(s);
                    tmpRows.Add(proRow);
                }

                this.Rows = tmpRows.ToArray();
            }
        }
    }
    #endregion

    #region 2.2.1.5 RopGetStoreState
    /// <summary>
    ///  A class indicates the RopGetStoreState  ROP Request Buffer.
    /// </summary>
    public class RopGetStoreStateRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetStoreStateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStoreStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetStoreState ROP Response Buffer.
    /// </summary>
    public class RopGetStoreStateResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A unsigned integer that indicates the state of the mailbox for the logged on user. 
        /// </summary>
        public uint? StoreState;

        /// <summary>
        /// Parse the RopGetStoreStateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStoreStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            List<PropertyRow> tmpRows = new List<PropertyRow>();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.StoreState = this.ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.1.6 RopGetOwningServers
    /// <summary>
    ///  A class indicates the RopGetOwningServers ROP Request Buffer.
    /// </summary>
    public class RopGetOwningServersRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the folder for which to get owning servers.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopGetOwningServersRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetOwningServersRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopGetOwningServers ROP Response Buffer.
    /// </summary>
    public class RopGetOwningServersResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the OwningServers field.
        /// </summary>
        public ushort? OwningServersCount;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the OwningServers field that refer to lowest-cost servers.
        /// </summary>
        public ushort? CheapServersCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        /// </summary>
        public MAPIString[] OwningServers;

        /// <summary>
        /// Parse the RopGetOwningServersResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetOwningServersResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.OwningServersCount = this.ReadUshort();
                this.CheapServersCount = this.ReadUshort();

                List<MAPIString> tmpOwning = new List<MAPIString>();
                for (int i = 0; i < this.OwningServersCount; i++)
                {
                    MAPIString subOwing = new MAPIString(Encoding.ASCII);
                    subOwing.Parse(s);
                    tmpOwning.Add(subOwing);
                }

                this.OwningServers = tmpOwning.ToArray();
            }
        }
    }
    #endregion

    #region 2.2.1.7 RopPublicFolderIsGhosted
    /// <summary>
    ///  A class indicates the RopPublicFolderIsGhosted ROP Request Buffer.
    /// </summary>
    public class RopPublicFolderIsGhostedRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the folder to check.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopPublicFolderIsGhostedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopPublicFolderIsGhosted ROP Response Buffer.
    /// </summary>
    public class RopPublicFolderIsGhostedResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the folder is a ghosted folder.
        /// </summary>
        public bool? IsGhosted;

        /// <summary>
        /// An unsigned integer that is present if IsGhosted is nonzero and is not present if IsGhosted is zero.
        /// </summary>
        public ushort? ServersCount;

        /// <summary>
        /// An unsigned integer that is present if the value of the IsGhosted field is nonzero and is not present if the value of the IsGhosted field is zero.
        /// </summary>
        public ushort? CheapServersCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        /// </summary>
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopPublicFolderIsGhostedResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.IsGhosted = this.ReadBoolean();
                if (this.IsGhosted == true)
                {
                    this.ServersCount = this.ReadUshort();
                    this.CheapServersCount = this.ReadUshort();
                    List<MAPIString> tmpServers = new List<MAPIString>();
                    for (int i = 0; i < this.ServersCount; i++)
                    {
                        MAPIString subServer = new MAPIString(Encoding.ASCII);
                        subServer.Parse(s);
                        tmpServers.Add(subServer);
                    }

                    this.Servers = tmpServers.ToArray();
                }
            }
        }
    }
    #endregion

    #region 2.2.1.8 RopLongTermIdFromId
    /// <summary>
    ///  A class indicates the RopLongTermIdFromId ROP Request Buffer.
    /// </summary>
    public class RopLongTermIdFromIdRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the short-term ID to be converted to a long-term ID.
        /// </summary>
        public byte[] ObjectId;

        /// <summary>
        /// Parse the RopLongTermIdFromIdRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopLongTermIdFromIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ObjectId = this.ReadBytes(8);
        }
    }

    /// <summary>
    ///  A class indicates the RopLongTermIdFromId ROP Response Buffer.
    /// </summary>
    public class RopLongTermIdFromIdResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A LongTermID structure that specifies the long-term ID that was converted from the short-term ID, which is specified in the ObjectId field of the request.
        /// </summary>
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopLongTermIdFromIdResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopLongTermIdFromIdResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.LongTermId = new LongTermID();
                this.LongTermId.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.1.9 RopIdFromLongTermId
    /// <summary>
    ///  A class indicates the RopIdFromLongTermId ROP Request Buffer.
    /// </summary>
    public class RopIdFromLongTermIdRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A LongTermID structure that specifies the long-term ID to be converted to a short-term ID.
        /// </summary>
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopIdFromLongTermIdRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopIdFromLongTermIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.LongTermId = new LongTermID();
            this.LongTermId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopIdFromLongTermId ROP Response Buffer.
    /// </summary>
    public class RopIdFromLongTermIdResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An identifier that specifies the short-term ID that was converted from the long-term ID, which is specified in the LongTermId field of the request.
        /// </summary>
        public byte?[] ObjectId;

        /// <summary>
        /// Parse the RopIdFromLongTermIdResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopIdFromLongTermIdResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.ObjectId = this.ConvertArray(this.ReadBytes(8));
            }
        }
    }
    #endregion

    #region 2.2.1.10 RopGetPerUserLongTermIds
    /// <summary>
    ///  A class indicates the RopGetPerUserLongTermIds ROP Request Buffer.
    /// </summary>
    public class RopGetPerUserLongTermIdsRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A GUID that specifies which database the client is querying data for
        /// </summary>
        public Guid DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPerUserLongTermIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.DatabaseGuid = this.ReadGuid();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPerUserLongTermIds ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserLongTermIdsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the LongTermIds field.
        /// </summary>
        public ushort? LongTermIdCount;

        /// <summary>
        /// An array of LongTermID structures that specifies which folders the user has per-user information about. 
        /// </summary>
        public LongTermID[] LongTermIds;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPerUserLongTermIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.LongTermIdCount = this.ReadUshort();
                this.LongTermIds = new LongTermID[(int)this.LongTermIdCount];
                for (int i = 0; i < this.LongTermIdCount; i++)
                {
                    this.LongTermIds[i] = new LongTermID();
                    this.LongTermIds[i].Parse(s);
                }
            }
        }
    }
    #endregion

    #region 2.2.1.11 RopGetPerUserGuid
    /// <summary>
    ///  A class indicates the RopGetPerUserGuid ROP Request Buffer.
    /// </summary>
    public class RopGetPerUserGuidRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A LongTermID structure that specifies the public folder. 
        /// </summary>
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopGetPerUserGuidRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPerUserGuidRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.LongTermId = new LongTermID();
            this.LongTermId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPerUserGuid ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserGuidResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A GUID that specifies the database for which per-user information was obtained.
        /// </summary>
        public Guid? DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserGuidResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPerUserGuidResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.DatabaseGuid = this.ReadGuid();
            }
        }
    }
    #endregion

    #region 2.2.1.12 RopReadPerUserInformation
    /// <summary>
    ///  A class indicates the RopReadPerUserInformation ROP Request Buffer.
    /// </summary>
    public class RopReadPerUserInformationRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A LongTermID structure that specifies the folder for which to get per-user information.
        /// </summary>
        public LongTermID FolderId;

        /// <summary>
        /// Reserved field.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// An unsigned integer that specifies the location at which to start reading within the per-user information to be retrieved.
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes of per-user information to be retrieved.
        /// </summary>
        public ushort MaxDataSize;

        /// <summary>
        /// Parse the RopReadPerUserInformationRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadPerUserInformationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.FolderId = new LongTermID();
            this.FolderId.Parse(s);
            this.Reserved = this.ReadByte();
            this.DataOffset = this.ReadUint();
            this.MaxDataSize = this.ReadUshort();
        }
    }

    /// <summary>
    ///  A class indicates the RopReadPerUserInformation ROP Response Buffer.
    /// </summary>
    public class RopReadPerUserInformationResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether this operation reached the end of the per-user information stream.
        /// </summary>
        public bool? HasFinished;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort? DataSize;

        /// <summary>
        /// An array of bytes. This field contains the per-user data that is returned.
        /// </summary>
        public byte?[] Data;

        /// <summary>
        /// Parse the RopReadPerUserInformationResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadPerUserInformationResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.HasFinished = this.ReadBoolean();
                this.DataSize = this.ReadUshort();
                this.Data = this.ConvertArray(this.ReadBytes((int)this.DataSize));
            }
        }
    }
    #endregion

    #region 2.2.1.13 RopWritePerUserInformation
    /// <summary>
    ///  A class indicates the RopWritePerUserInformation ROP Request Buffer.
    /// </summary>
    public class RopWritePerUserInformationRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A LongTermID structure that specifies the folder to set per-user information for.
        /// </summary>
        public LongTermID FolderId;

        /// <summary>
        /// A Boolean that specifies whether this operation specifies the end of the per-user information stream.
        /// </summary>
        public bool HasFinished;

        /// <summary>
        /// An unsigned integer that specifies the location in the per-user information stream to start writing
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that is the per-user data to write.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// An GUID that is present when the DataOffset is 0 and the RopLogon associated with the LogonId field was created with the Private flag set in the RopLogon ROP request buffer
        /// </summary>
        public Guid? ReplGuid;

        /// <summary>
        /// Parse the RopWritePerUserInformationRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopWritePerUserInformationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.FolderId = new LongTermID();
            this.FolderId.Parse(s);
            this.HasFinished = this.ReadBoolean();
            this.DataOffset = this.ReadUint();
            this.DataSize = this.ReadUshort();
            this.Data = this.ReadBytes((int)this.DataSize);
            if (this.DataOffset == 0 && (((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIInspector.ParsingSession.id][this.LogonId] & (byte)LogonFlags.Private) == (byte)LogonFlags.Private))
            {
                this.ReplGuid = this.ReadGuid();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopWritePerUserInformation ROP Response Buffer.
    /// </summary>
    public class RopWritePerUserInformationResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopWritePerUserInformationResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopWritePerUserInformationResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion
}
