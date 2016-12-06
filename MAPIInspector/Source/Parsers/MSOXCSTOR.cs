using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text;

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
        public MAPIString Essdn;

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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        //  A flags structure that contains flags that specify the type of logon.
        public LogonFlags? LogonFlags;

        // 13 64-bit identifiers that specify a set of special folders for a mailbox.
        public FolderID[] FolderIds;

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
        public MAPIString ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PrivateMailboxes structure.
        /// </summary>
        /// <param name="s">An stream containing RopLogonResponse_PrivateMailboxes structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.FolderIds = new FolderID[13];
                for (int i = 0; i < 13; i++)
                {
                    FolderIds[i] = new FolderID();
                    FolderIds[i].Parse(s);
                }
                this.ResponseFlags = (ResponseFlags)ReadByte();
                this.MailboxGuid = ReadGuid();
                this.ReplId = ReadUshort();
                this.ReplGuid = ReadGuid();
                this.LogonTime = new LogonTime();
                this.LogonTime.Parse(s);
                this.GwartTime = ReadUlong();
                this.StoreState = ReadUint();
            }
            else if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.WrongServer)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.ServerNameSize = ReadByte();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        //  A flags structure that contains flags that specify the type of logon.
        public LogonFlags? LogonFlags;

        // 13 64-bit identifiers that specify a set of special folders for a mailbox.
        public FolderID[] FolderIds;

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
        public MAPIString ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PublicFolders structure.
        /// </summary>
        /// <param name="s">An stream containing RopLogonResponse_PublicFolders structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.FolderIds = new FolderID[13];
                for (int i = 0; i < 13; i++)
                {
                    FolderIds[i] = new FolderID();
                    FolderIds[i].Parse(s);
                }
                this.ReplId = ReadUshort();
                this.ReplGuid = ReadGuid();
                this.PerUserGuid = ReadGuid();
            }
            else if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.WrongServer)
            {
                this.LogonFlags = (LogonFlags)ReadByte();
                this.ServerNameSize = ReadByte();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A null-terminated ASCII string that specifies the message class to find the Receive folder for.
        public MAPIString MessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetReceiveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.MessageClass = new MAPIString(Encoding.ASCII);
            this.MessageClass.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopGetReceiveFolder ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An identifier that specifies the Receive folder.
        public FolderID FolderId;

        // A null-terminated ASCII string that specifies the message class that is actually configured for delivery to the folder.
        public MAPIString ExplicitMessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetReceiveFolderResponse structure.</param>
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An identifier that specifies the Receive folder.
        public FolderID FolderId;

        // A null-terminated ASCII string that specifies which message class to set the Receive folder for.
        public MAPIString MessageClass;

        /// <summary>
        /// Parse the RopSetReceiveFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetReceiveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetReceiveFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetReceiveFolderResponse structure.</param>
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

    #region 2.2.1.4 RopGetReceiveFolderTable
    /// <summary>
    ///  A class indicates the RopGetReceiveFolderTable  ROP Request Buffer.
    /// </summary>
    public class RopGetReceiveFolderTableRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetReceiveFolderTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetReceiveFolderTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetReceiveFolderTable ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of row structures contained in the Rows field.
        public uint? RowCount;

        // An array of row structures. This field contains the rows of the Receive folder table. Each row is returned in either a StandardPropertyRow or a FlaggedPropertyRow structure.
        public PropertyRow[] Rows;

        /// <summary>
        /// Parse the RopGetReceiveFolderTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetReceiveFolderTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            List<PropertyRow> TmpRows = new List<PropertyRow>();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = ReadUint();

                for (int i = 0; i < this.RowCount; i++)
                {
                    // PidTagMessageClass is defined as PtypString8 due to Open Specification said all characters in this property MUST be from the 
                    // ASCII characters 0x20 through 0x7F. 
                    PropertyTag[] Properties_GetReceiveFolderTable = new PropertyTag[3] 
                    { new PropertyTag(PropertyDataType.PtypInteger64, PidTagPropertyEnum.PidTagFolderId),
                      new PropertyTag(PropertyDataType.PtypString8, PidTagPropertyEnum.PidTagMessageClass),
                      new PropertyTag(PropertyDataType.PtypTime, PidTagPropertyEnum.PidTagLastModificationTime)
                    };
                    PropertyRow ProRow = new PropertyRow(Properties_GetReceiveFolderTable);
                    ProRow.Parse(s);
                    TmpRows.Add(ProRow);
                }
                this.Rows = TmpRows.ToArray();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetStoreStateRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetStoreStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetStoreState ROP Response Buffer.
    /// </summary>
    public class RopGetStoreStateResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A unsigned integer that indicates the state of the mailbox for the logged on user. 
        public uint? StoreState;

        /// <summary>
        /// Parse the RopGetStoreStateResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetStoreStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            List<PropertyRow> TmpRows = new List<PropertyRow>();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.StoreState = ReadUint();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An identifier that specifies the folder for which to get owning servers.
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopGetOwningServersRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetOwningServersRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopGetOwningServers ROP Response Buffer.
    /// </summary>
    public class RopGetOwningServersResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of strings in the OwningServers field.
        public ushort? OwningServersCount;

        // An unsigned integer that specifies the number of strings in the OwningServers field that refer to lowest-cost servers.
        public ushort? CheapServersCount;

        // A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        public MAPIString[] OwningServers;

        /// <summary>
        /// Parse the RopGetOwningServersResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetOwningServersResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.OwningServersCount = ReadUshort();
                this.CheapServersCount = ReadUshort();

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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An identifier that specifies the folder to check.
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopPublicFolderIsGhostedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopPublicFolderIsGhosted ROP Response Buffer.
    /// </summary>
    public class RopPublicFolderIsGhostedResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether the folder is a ghosted folder.
        public bool? IsGhosted;

        // An unsigned integer that is present if IsGhosted is nonzero and is not present if IsGhosted is zero.
        public ushort? ServersCount;

        // An unsigned integer that is present if the value of the IsGhosted field is nonzero and is not present if the value of the IsGhosted field is zero.
        public ushort? CheapServersCount;

        // A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopPublicFolderIsGhostedResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.IsGhosted = ReadBoolean();
                if (this.IsGhosted == true)
                {
                    this.ServersCount = ReadUshort();
                    this.CheapServersCount = ReadUshort();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An identifier that specifies the short-term ID to be converted to a long-term ID.
        public byte[] ObjectId;

        /// <summary>
        /// Parse the RopLongTermIdFromIdRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopLongTermIdFromIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ObjectId = ReadBytes(8);
        }
    }

    /// <summary>
    ///  A class indicates the RopLongTermIdFromId ROP Response Buffer.
    /// </summary>
    public class RopLongTermIdFromIdResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A LongTermID structure that specifies the long-term ID that was converted from the short-term ID, which is specified in the ObjectId field of the request.
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopLongTermIdFromIdResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopLongTermIdFromIdResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A LongTermID structure that specifies the long-term ID to be converted to a short-term ID.
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopIdFromLongTermIdRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopIdFromLongTermIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.LongTermId = new LongTermID();
            this.LongTermId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopIdFromLongTermId ROP Response Buffer.
    /// </summary>
    public class RopIdFromLongTermIdResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An identifier that specifies the short-term ID that was converted from the long-term ID, which is specified in the LongTermId field of the request.
        public byte?[] ObjectId;

        /// <summary>
        /// Parse the RopIdFromLongTermIdResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopIdFromLongTermIdResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.ObjectId = ConvertArray(ReadBytes(8));
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A GUID that specifies which database the client is querying data for
        public Guid DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPerUserLongTermIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.DatabaseGuid = ReadGuid();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPerUserLongTermIds ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserLongTermIdsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of structures in the LongTermIds field.
        public ushort? LongTermIdCount;

        // An array of LongTermID structures that specifies which folders the user has per-user information about. 
        public LongTermID[] LongTermIds;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPerUserLongTermIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.LongTermIdCount = ReadUshort();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A LongTermID structure that specifies the public folder. 
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopGetPerUserGuidRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPerUserGuidRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.LongTermId = new LongTermID();
            this.LongTermId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopGetPerUserGuid ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserGuidResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A GUID that specifies the database for which per-user information was obtained.
        public Guid? DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserGuidResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetPerUserGuidResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.DatabaseGuid = ReadGuid();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A LongTermID structure that specifies the folder for which to get per-user information.
        public LongTermID FolderId;

        // Reserved.
        public byte Reserved;

        // An unsigned integer that specifies the location at which to start reading within the per-user information to be retrieved.
        public uint DataOffset;

        // An unsigned integer that specifies the maximum number of bytes of per-user information to be retrieved.
        public ushort MaxDataSize;

        /// <summary>
        /// Parse the RopReadPerUserInformationRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopReadPerUserInformationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.FolderId = new LongTermID();
            this.FolderId.Parse(s);
            this.Reserved = ReadByte();
            this.DataOffset = ReadUint();
            this.MaxDataSize = ReadUshort();
        }
    }

    /// <summary>
    ///  A class indicates the RopReadPerUserInformation ROP Response Buffer.
    /// </summary>
    public class RopReadPerUserInformationResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether this operation reached the end of the per-user information stream.
        public bool? HasFinished;

        // An unsigned integer that specifies the size of the Data field.
        public ushort? DataSize;

        // An array of bytes. This field contains the per-user data that is returned.
        public byte?[] Data;

        /// <summary>
        /// Parse the RopReadPerUserInformationResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopReadPerUserInformationResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.HasFinished = ReadBoolean();
                this.DataSize = ReadUshort();
                this.Data = ConvertArray(ReadBytes((int)this.DataSize));
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A LongTermID structure that specifies the folder to set per-user information for.
        public LongTermID FolderId;

        // A Boolean that specifies whether this operation specifies the end of the per-user information stream.
        public bool HasFinished;

        // An unsigned integer that specifies the location in the per-user information stream to start writing
        public uint DataOffset;

        // An unsigned integer that specifies the size of the Data field.
        public ushort DataSize;

        // An array of bytesthat is the per-user data to write.
        public byte[] Data;

        // An GUID that is present when the DataOffset is 0 and the logon associated with the LogonId field was created with the Private flag set in the RopLogon ROP request buffer
        public Guid? ReplGuid;

        /// <summary>
        /// Parse the RopWritePerUserInformationRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopWritePerUserInformationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.FolderId = new LongTermID();
            this.FolderId.Parse(s);
            this.HasFinished = ReadBoolean();
            this.DataOffset = ReadUint();
            this.DataSize = ReadUshort();
            this.Data = ReadBytes((int)this.DataSize);
            if (this.DataOffset == 0 && (((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIInspector.currentParsingSessionID][LogonId] & (byte)LogonFlags.Private) == (byte)LogonFlags.Private))
            {
                this.ReplGuid = ReadGuid();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopWritePerUserInformation ROP Response Buffer.
    /// </summary>
    public class RopWritePerUserInformationResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopWritePerUserInformationResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopWritePerUserInformationResponse structure.</param>
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

    #region Enum
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
    [Flags]
    public enum OpenFlags : uint
    {
        USE_ADMIN_PRIVILEGE = 0x00000001,
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
        Reserved = 0x01,
        OwnerRight = 0x02,
        SendAsRight = 0x04,
        OOF = 0x10
    }

    /// <summary>
    /// The enum type for days of the week.
    /// </summary>
    public enum DayOfWeek : byte
    {
        Sunday = 0x00,
        Monday = 0x01,
        Tuesday = 0x02,
        Wednesday = 0x03,
        Thursday = 0x04,
        Friday = 0x05,
        Saturday = 0x06
    }

    /// <summary>
    /// The enum type for months of a year.
    /// </summary>
    public enum Month : byte
    {
        January = 0x01,
        February = 0x02,
        March = 0x03,
        April = 0x04,
        May = 0x05,
        June = 0x06,
        July = 0x07,
        August = 0x08,
        September = 0x09,
        October = 0x0A,
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
    #endregion
}
