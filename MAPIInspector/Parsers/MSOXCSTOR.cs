using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    #region 2.2.1.1 RopLogon
    /// <summary>
    ///  A class indicates the RopLogon ROP Request Buffer.
    /// </summary>
    public class RopLogonRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;
        
        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;
        
        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;
        
        // A flags structure that contains flags that control the behavior of the logon.
        public LogonFlags LogonFlags;
        
        // A flags structure that contains more flags that control the behavior of the logon.
        public OpenFlags OpenFlags;
        
        // A flags structure. This field is not used and is ignored by the server.
        public uint StoreState;
        
        //  An unsigned integer that specifies the size of the Essdn field.
        public ushort EssdnSize;
        
        // A null-terminated ASCII string that specifies which mailbox to log on to. 
        [HelpAttribute(StringEncoding.ASCII, true, 1)]
        public string Essdn;

        /// <summary>
        /// Parse the RopLogonRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopLogonRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.LogonFlags = (LogonFlags)ReadByte();
            this.OpenFlags = (OpenFlags)ReadUint();
            this.StoreState = ReadUint();
            this.EssdnSize = ReadUshort();
            this.Essdn = ReadString();

            DecodingContext.LogonFlags = this.LogonFlags;
        }
    }

    /// <summary>
    ///  A class indicates the RopLogon ROP Response Buffer for private mailbox.
    /// </summary>
    public class RopLogonResponse_PrivateMailboxes : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;
        
        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;
        
        // An unsigned integer that specifies the status of the ROP.
        public uint ReturnValue;
        
        //  A flags structure that contains flags that specify the type of logon.
        public LogonFlags? LogonFlags;
        
        // TODO: 13 64-bit identifiers that specify a set of special folders for a mailbox.
        public byte?[] FolderIds;
        
        // A flags structure that contains flags that provide details about the state of the mailbox. 
        public ResponseFlags? ResponseFlags;
        
        // A GUID that identifies the mailbox on which the logon was performed.
        public Guid? MailboxGuid;
        
        // An identifier that specifies a replica ID for the logon.
        public ushort? ReplId;
        
        // A GUID that specifies the replica GUID that is associated with the replica ID.
        public Guid? ReplGuid;

        // A LogonTime structure that specifies the time at which the logon occurred. 
        public LogonTime LogonTime;
   
        // An unsigned integer that contains a numeric value that tracks the currency of the Gateway Address Routing Table (GWART).
        public ulong? GwartTime;
        
        // A flags structure.
        public uint? StoreState;

        // The below two fields is defined for RopLogon redirect response in section 2.2.3.1.4 in MS-OXCROPS.
        // An unsigned integer that specifies the length of the ServerName field.
        public byte? ServerNameSize;
       
        //  A null-terminated ASCII string that specifies a different server for the client to connect to.
        [HelpAttribute(StringEncoding.ASCII, false, 1)]
        public string ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PrivateMailboxes structure.
        /// </summary>
        /// <param name="s">An stream containing RopLogonResponse_PrivateMailboxes structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();

            if (ReturnValue == 0)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.FolderIds = ConvertArray(ReadBytes(104));
                this.ResponseFlags = (ResponseFlags)ReadByte();
                this.MailboxGuid = ReadGuid();
                this.ReplId = ReadUshort();
                this.ReplGuid = ReadGuid();
                this.LogonTime = new LogonTime();
                this.LogonTime.Parse(s);
                this.GwartTime = ReadUlong();
                this.StoreState = ReadUint();
            }
            else if (ReturnValue == 0x00000478)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.ServerNameSize = ReadByte();
                this.ServerName = ReadString();
                ModifyIsExistAttribute(this, "ServerName");
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopLogon ROP Response Buffer for public folders.
    /// </summary>
    public class RopLogonResponse_PublicFolders : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;
        
        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;
        
        // An unsigned integer that specifies the status of the ROP.
        public uint ReturnValue;
        
        //  A flags structure that contains flags that specify the type of logon.
        public LogonFlags? LogonFlags;
        
        // TODO: 13 64-bit identifiers that specify a set of special folders for a mailbox.
        public byte?[] FolderIds;
        
        // An identifier that specifies a replica ID for the logon.
        public ushort? ReplId;
        
        // A GUID that specifies the replica GUID associated with the replica ID that is specified in the ReplId field.
        public Guid? ReplGuid;
        
        // This field is not used and is ignored by the client.
        public Guid? PerUserGuid;

        // The below two fields is defined for RopLogon redirect response in section 2.2.3.1.4 in MS-OXCROPS.
        // An unsigned integer that specifies the length of the ServerName field.
        public byte? ServerNameSize;
        
        //  A null-terminated ASCII string that specifies a different server for the client to connect to.
        [HelpAttribute(StringEncoding.ASCII, false, 1)]
        public string ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PublicFolders structure.
        /// </summary>
        /// <param name="s">An stream containing RopLogonResponse_PublicFolders structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.ReturnValue = ReadUint();

            if (ReturnValue == 0)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.FolderIds = ConvertArray(ReadBytes(104));
                this.ReplId = ReadUshort();
                this.ReplGuid = ReadGuid();
                this.PerUserGuid = ReadGuid();
            }
            else if (ReturnValue == 0x00000478)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.ServerNameSize = ReadByte();
                this.ServerName = ReadString();
                ModifyIsExistAttribute(this, "ServerName");
            }
        }
    }
    #endregion

    /// <summary>
    /// The enum type for flags that control the behavior of the logon.
    /// </summary>
    [Flags]
    public enum LogonFlags : byte
    {
	    Private = 0x01,
	    Undercover = 0x02,
	    Ghosted = 0x04,
	    SpoolerProcess = 0x08
    }

    /// <summary>
    /// The enum type for additional flags that control the behavior of the logon.
    /// </summary>
    public enum OpenFlags : uint
    {
	    USE_ADMIN_PRIVILEGE  = 0x00000001,
	    PUBLIC = 0x00000002,
	    HOME_LOGON = 0x00000004,
	    TAKE_OWNERSHIP = 0x00000008,
	    ALTERNATE_SERVER = 0x00000100,
	    IGNORE_HOME_MDB = 0x00000200,
	    NO_MAIL = 0x00000400,
	    USE_PER_MDB_REPLID_MAPPING = 0x01000000,
	    SUPPORT_PROGRESS = 0x20000000
    }

    /// <summary>
    /// The enum type for flags that provide details about the state of the mailbox.
    /// </summary>
    [Flags]
    public enum ResponseFlags : byte
    {
	    Reserved    = 0x01,
	    OwnerRight  = 0x02,
	    SendAsRight = 0x04,
	    OOF         = 0x10
    }

    /// <summary>
    /// The enum type for days of the week.
    /// </summary>
    public enum DayOfWeek : byte
    {
        Sunday    = 0x00,
        Monday    = 0x01,
        Tuesday   = 0x02,
        Wednesday = 0x03,
        Thursday  = 0x04,
	    Friday    = 0x05,
	    Saturday  = 0x06
    }

    /// <summary>
    /// The enum type for months of a year.
    /// </summary>
    public enum Month : byte
    {
        January  = 0x01,
        February = 0x02,
        March    = 0x03,
        April    = 0x04,
        May      = 0x05,
	    June     = 0x06,
	    July     = 0x07,
        August   = 0x08,
        September= 0x09,
        October  = 0x0A,
        November = 0x0B,
        December = 0x0C
    }

    /// <summary>
    ///  A class indicates the Logon time.
    /// </summary>
    public class LogonTime : BaseStructure
    {
        // An unsigned integer that specifies the current second.
        public byte Seconds;
        // An unsigned integer that specifies the current Minutes.
        public byte Minutes;
        // An unsigned integer that specifies the current Hour.
        public byte Hour;
        // An enumeration that specifies the current day of the week.
        public DayOfWeek DayOfWeek;
        // An unsigned integer that specifies the current day of the month.
        public byte Day;
        //  An unsigned integer that specifies the current month 
        public Month Month;
        // An unsigned integer that specifies the current year.
        public ushort Year;

        /// <summary>
        /// Parse the LogonTime structure.
        /// </summary>
        /// <param name="s">An stream containing LogonTime structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Seconds = ReadByte();
            this.Minutes = ReadByte();
            this.Hour = ReadByte();
            this.DayOfWeek = (DayOfWeek)ReadByte();
            this.Day = ReadByte();
            this.Month = (Month)ReadByte();
            this.Year = ReadUshort();
         }
    }
}
