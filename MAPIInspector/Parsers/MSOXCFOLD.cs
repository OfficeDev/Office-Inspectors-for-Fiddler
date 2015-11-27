using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    #region The enum value that used by Rops.

    /// <summary>
    /// Section 2.2.1.1.1   RopOpenFolder ROP Request Buffer
    /// </summary>
    [Flags]
    public enum OpenModeFlagsMSOXCFOLD : byte
    {
        OpenSoftDeleted = 0x04
    }
    /// <summary>
    /// Section 2.2.1.2.1   RopCreateFolder ROP Request Buffer
    /// </summary>
    public enum FolderType : byte
    {
        GenericFolder = 1,
        SearchFolder = 2
    }

    /// <summary>
    /// Section 2.2.1.3.1   RopDeleteFolder ROP Request Buffer
    /// </summary>
    [Flags]
    public enum DeleteFolderFlags : byte
    {
        DEL_MESSAGES = 0x01,
        DEL_FOLDERS = 0x04,
        DELETE_HARD_DELETE = 0x10
    }

    /// <summary>
    /// Section 2.2.1.4.1   RopSetSearchCriteria ROP Request Buffer
    /// </summary>
    [Flags]
    public enum SearchRequestFlags : uint
    {
        STOP_SEARCH = 0x00000001,
        RESTART_SEARCH = 0x00000002,
        RECURSIVE_SEARCH = 0x00000004,
        SHALLOW_SEARCH = 0x00000008,
        CONTENT_INDEXED_SEARCH = 0x00010000,
        NON_CONTENT_INDEXED_SEARCH = 0x00020000,
        STATIC_SEARCH = 0x00040000
    }

    /// <summary>
    /// Section 2.2.1.5.2   RopGetSearchCriteria ROP Response Buffer
    /// </summary>
    [Flags]
    public enum SearchResponseFlags : uint
    {
        SEARCH_RUNNING = 0x00000001,
        SEARCH_REBUILD = 0x00000002,
        SEARCH_RECURSIVE = 0x00000004,
        SEARCH_COMPLETE = 0x00001000,
        SEARCH_PARTIAL = 0x00002000,
        SEARCH_STATIC = 0x00010000,
        SEARCH_MAYBE_STATIC = 0x00020000,
        CI_TOTALLY = 0x01000000,
        TWIR_TOTALLY = 0x08000000
    }

    /// <summary>
    /// Section 2.2.1.13.1   RopGetHierarchyTable ROP Request Buffer
    /// </summary>
    [Flags]
    public enum HierarchyTableFlags : byte
    {
        Depth = 0x04,
        DeferredErrors = 0x08,
        NoNotifications = 0x10,
        SoftDeletes = 0x20,
        UseUnicode = 0x40,
        SuppressesNotifications = 0x80
    }
    /// <summary>
    /// Section 2.2.1.14.1   RopGetContentsTable ROP Request Buffer
    /// </summary>
    [Flags]
    public enum ContentsTableFlags : byte
    {
        Associated = 0x02,
        DeferredErrors = 0x08,
        NoNotifications = 0x10,
        SoftDeletes = 0x20,
        UseUnicode = 0x40,
        ConversationMembers = 0x80
    }

    #endregion

    #region 2.2.1.1	RopOpenFolder ROP
    /// <summary>
    /// The RopOpenFolder ROP ([MS-OXCROPS] section 2.2.4.1) opens an existing folder. 
    /// </summary>
    public class RopOpenFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // A 64-bit identifier that specifies the folder to be opened.
        public FolderID FolderId;

        // An 8-bit flags structure that contains flags that are used to control how the folder is opened.
        public OpenModeFlagsMSOXCFOLD OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.OpenModeFlags = (OpenModeFlagsMSOXCFOLD)ReadByte();
        }
    }

    ///  <summary>
    /// A class indicates the RopOpenFolder ROP Response Buffer.
    /// </summary>
    public class RopOpenFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request. 
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that indicates whether the folder has rules associated with it.
        public bool? HasRules;

        // A Boolean that specifies whether the folder is a ghosted folder.
        public bool? IsGhosted;

        // This value specifies the number of strings in the Servers field.
        public ushort? ServerCount;

        // This value specifies the number of values in the Servers field that refer to lowest-cost servers.
        public ushort? CheapServerCount;

        // A list of null-terminated ASCII strings that specify which servers have replicas (2) of this folder. 
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopOpenFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.HasRules = ReadBoolean();
                this.IsGhosted = ReadBoolean();
                if ((bool)IsGhosted)
                {
                    this.ServerCount = ReadUshort();
                    this.CheapServerCount = ReadUshort();
                    List<MAPIString> tempServers = new List<MAPIString>();
                    for (int i = 0; i < ServerCount; i++)
                    {
                        MAPIString tempString = new MAPIString(Encoding.ASCII);
                        tempString.Parse(s);
                        tempServers.Add(tempString);
                    }
                    this.Servers = tempServers.ToArray();
                }
            }
        }
    }

    #endregion

    #region 2.2.1.2	RopCreateFolder ROP
    /// <summary>
    /// The RopCreateFolder ROP ([MS-OXCROPS] section 2.2.4.2) creates a new folder
    /// </summary>
    public class RopCreateFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // An enumeration that specifies what type of folder to create. 
        public FolderType FolderType;

        // A Boolean that specifies whether the DisplayName field and the Comment field contain Unicode characters or multibyte characters.
        public bool UseUnicodeStrings;

        // Boolean that specifies whether this operation opens a Folder object or fails when the Folder object already exists.
        public bool OpenExisting;

        // Reserved. This field MUST be set to 0x00.
        public byte Reserved;

        // A null-terminated multibyte string that specifies the name of the created folder. 
        public MAPIString DisplayName;

        // A null-terminated multibyte string that specifies the folder comment that is associated with the created folder. 
        public MAPIString Comment;

        /// <summary>
        /// Parse the RopCreateFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.FolderType = (FolderType)ReadByte();
            this.UseUnicodeStrings = ReadBoolean();
            this.OpenExisting = ReadBoolean();
            this.Reserved = ReadByte();
            if (UseUnicodeStrings)
            {
                this.DisplayName = new MAPIString(Encoding.Unicode);
                this.DisplayName.Parse(s);
                this.Comment = new MAPIString(Encoding.Unicode);
                this.Comment.Parse(s);
            }
            else
            {
                this.DisplayName = new MAPIString(Encoding.ASCII);
                this.DisplayName.Parse(s);
                this.Comment = new MAPIString(Encoding.ASCII);
                this.Comment.Parse(s);
            }

        }
    }

    ///  <summary>
    /// A class indicates the RopCreateFolder ROP Response Buffer.
    /// </summary>
    public class RopCreateFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request. 
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An identifier that specifies the folder created or opened.
        public FolderID FolderId;

        // A Boolean that indicates whether an existing folder was opened or a new folder was created.
        public bool? IsExistingFolder;

        // A Boolean that indicates whether the folder has rules associated with it.
        public bool? HasRules;

        // A Boolean that indicates whether the server is an active replica of this folder. 
        public bool? IsGhosted;

        // This value specifies the number of strings in the Servers field.
        public ushort? ServerCount;

        // This value specifies the number of values in the Servers field that refer to lowest-cost servers.
        public ushort? CheapServerCount;

        // These strings specify which servers have replicas (2) of this folder.
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopCreateFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
                this.IsExistingFolder = ReadBoolean();
                if ((bool)IsExistingFolder)
                {
                    this.HasRules = ReadBoolean();
                    this.IsGhosted = ReadBoolean();
                    if ((bool)IsGhosted)
                    {
                        this.ServerCount = ReadUshort();
                        this.CheapServerCount = ReadUshort();
                        List<MAPIString> tempServers = new List<MAPIString>();
                        for (int i = 0; i < ServerCount; i++)
                        {
                            MAPIString tempString = new MAPIString(Encoding.ASCII);
                            tempString.Parse(s);
                            tempServers.Add(tempString);
                        }
                        this.Servers = tempServers.ToArray();
                    }
                }
            }
        }
    }

    #endregion

    #region 2.2.1.3	RopDeleteFolder ROP
    /// <summary>
    /// The RopDeleteFolder ROP ([MS-OXCROPS] section 2.2.4.3) removes a folder. 
    /// </summary>
    public class RopDeleteFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A flags structure that contains flags that control how to delete the folder. 
        public DeleteFolderFlags DeleteFolderFlags;

        // An identifier that specifies the folder to be deleted.
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopDeleteFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeleteFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.DeleteFolderFlags = (DeleteFolderFlags)ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
        }
    }

    ///  <summary>
    /// A class indicates the RopDeleteFolder ROP Response Buffer.
    /// </summary>
    public class RopDeleteFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether the operation was partially completed.
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopDeleteFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeleteFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.PartialCompletion = ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.4	RopSetSearchCriteria ROP
    /// <summary>
    /// The RopSetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.4) establishes search criteria for a search folder.
    /// </summary>
    public class RopSetSearchCriteriaRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the length of the RestrictionData field.
        public ushort RestrictionDataSize;

        // A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder. 
        public RestrictionType RestrictionData;

        // An unsigned integer that specifies the number of identifiers in the FolderIds field.
        public ushort FolderIdCount;

        // An array of 64-bit identifiers that specifies which folders are searched. 
        public FolderID[] FolderIds;

        // A flags structure that contains flags that control the search for a search folder.
        public SearchRequestFlags SearchFlags;

        /// <summary>
        /// Parse the RopSetSearchCriteriaRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetSearchCriteriaRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.RestrictionDataSize = ReadUshort();
            if (RestrictionDataSize > 0)
            {
                this.RestrictionData = new RestrictionType();
                this.RestrictionData.Parse(s);
            }
            this.FolderIdCount = ReadUshort();
            List<FolderID> tempFolderIDs = new List<FolderID>();
            for (int i = 0; i < FolderIdCount; i++)
            {
                FolderID folderID = new FolderID();
                folderID.Parse(s);
                tempFolderIDs.Add(folderID);
            }
            this.FolderIds = tempFolderIDs.ToArray();
            this.SearchFlags = (SearchRequestFlags)ReadUint();
        }
    }

    ///  <summary>
    /// A class indicates the RopSetSearchCriteria ROP Response Buffer.
    /// </summary>
    public class RopSetSearchCriteriaResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetSearchCriteriaResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetSearchCriteriaResponse structure.</param>
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

    #region 2.2.1.5	RopGetSearchCriteria ROP
    /// <summary>
    /// The RopGetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.5) obtains the search criteria and the status of a search for a search folder. 
    /// </summary>
    public class RopGetSearchCriteriaRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A Boolean that specifies whether the client requests the restriction data (returned in the RestrictionData field of the response) to be specified with Unicode strings or with ASCII strings. 
        public bool UseUnicode;

        // A Boolean that specifies whether the server includes the restriction information in the response.
        public bool IncludeRestriction;

        // A Boolean that specifies whether the server includes the folders list in the response.
        public bool IncludeFolders;

        /// <summary>
        /// Parse the RopGetSearchCriteriaRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetSearchCriteriaRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.UseUnicode = ReadBoolean();
            this.IncludeRestriction = ReadBoolean();
            this.IncludeFolders = ReadBoolean();
        }
    }

    ///  <summary>
    /// A class indicates the RopGetSearchCriteria ROP Response Buffer.
    /// </summary>
    public class RopGetSearchCriteriaResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the length of the RestrictionData field.
        public ushort? RestrictionDataSize;

        // A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder. 
        public RestrictionType RestrictionData;

        // An unsigned integer that MUST be set to the value of the LogonId field in the request.
        public byte? LogonId;

        // An unsigned integer that specifies the number of identifiers in the FolderIds field.
        public ushort? FolderIdCount;

        // An array of 64-bit identifiers that specifies which folders are searched. 
        public FolderID[] FolderIds;

        // A flags structure that contains flags that control the search for a search folder. 
        public SearchResponseFlags SearchFlags;

        /// <summary>
        /// Parse the RopGetSearchCriteriaResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetSearchCriteriaResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RestrictionDataSize = ReadUshort();
                if (RestrictionDataSize > 0)
                {
                    this.RestrictionData = new RestrictionType();
                    this.RestrictionData.Parse(s);
                }
                this.LogonId = ReadByte();
                this.FolderIdCount = ReadUshort();
                List<FolderID> tempFolderIDs = new List<FolderID>();
                for (int i = 0; i < FolderIdCount; i++)
                {
                    FolderID folderID = new FolderID();
                    folderID.Parse(s);
                    tempFolderIDs.Add(folderID);
                }
                this.FolderIds = tempFolderIDs.ToArray();
                this.SearchFlags = (SearchResponseFlags)ReadUint();
            }
        }
    }

    #endregion

    #region 2.2.1.6	RopMoveCopyMessages ROP
    /// <summary>
    /// The RopMoveCopyMessages ROP ([MS-OXCROPS] section 2.2.4.6) moves or copies messages from a source folder to a destination folder. 
    /// </summary>
    public class RopMoveCopyMessagesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        public byte SourceHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        public byte DestHandleIndex;

        // An unsigned integer that specifies the size of the MessageIds field.
        public ushort MessageIdCount;

        // An array of 64-bit identifiers that specifies which messages to move or copy. 
        public MessageID[] MessageIds;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A Boolean that specifies whether the operation is a copy or a move.
        public bool WantCopy;

        /// <summary>
        /// Parse the RopMoveCopyMessagesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopMoveCopyMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.SourceHandleIndex = ReadByte();
            this.DestHandleIndex = ReadByte();
            this.MessageIdCount = ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }
            this.MessageIds = tempMessageIDs.ToArray();
            this.WantAsynchronous = ReadBoolean();
            this.WantCopy = ReadBoolean();
        }
    }

    ///  <summary>
    /// A class indicates the RopMoveCopyMessages ROP Response Buffer.
    /// </summary>
    public class RopMoveCopyMessagesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request.
        public byte SourceHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request. 
        public uint? DestHandleIndex;

        // A Boolean that indicates whether the operation was only partially completed.
        public bool PartialCompletion;
        /// <summary>
        /// Parse the RopMoveCopyMessagesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopMoveCopyMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SourceHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = ReadUint();
                this.PartialCompletion = ReadBoolean();
            }
            else
            {
                this.PartialCompletion = ReadBoolean();
            }
        }
    }

    #endregion

    #region 2.2.1.7	RopMoveFolder ROP
    /// <summary>
    /// The RopMoveFolder ROP ([MS-OXCROPS] section 2.2.4.7) moves a folder from one parent folder to another parent folder.
    /// </summary>
    public class RopMoveFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        public byte SourceHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        public byte DestHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A Boolean that specifies whether the NewFolderName field contains Unicode characters or multibyte characters.
        public bool UseUnicode;

        // An identifier that specifies the folder to be moved.
        public FolderID FolderId;

        // A null-terminated multibyte string that specifies the name for the new moved folder. 
        public MAPIString NewFolderName;

        /// <summary>
        /// Parse the RopMoveFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopMoveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.SourceHandleIndex = ReadByte();
            this.DestHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.UseUnicode = ReadBoolean();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            if (UseUnicode)
            {
                this.NewFolderName = new MAPIString(Encoding.Unicode);
                this.NewFolderName.Parse(s);
            }
            else
            {
                this.NewFolderName = new MAPIString(Encoding.ASCII);
                this.NewFolderName.Parse(s);
            }
        }
    }

    ///  <summary>
    /// A class indicates the RopMoveFolder ROP Response Buffer.
    /// </summary>
    public class RopMoveFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request. 
        public byte SourceHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        //  An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        public uint? DestHandleIndex;

        //A Boolean that indicates whether the operation was only partially completed.
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopMoveFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopMoveFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SourceHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = ReadUint();
                this.PartialCompletion = ReadBoolean();
            }
            else
            {
                this.PartialCompletion = ReadBoolean();
            }
        }
    }

    #endregion

    #region 2.2.1.8	RopCopyFolder ROP
    /// <summary>
    /// The RopCopyFolder ROP ([MS-OXCROPS] section 2.2.4.8) copies a folder from one parent folder to another parent folder. 
    /// </summary>
    public class RopCopyFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        public byte SourceHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        public byte DestHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A Boolean that specifies that the copy is recursive.
        public bool WantRecursive;

        // A Boolean that specifies whether the NewFolderName field contains Unicode characters or multibyte characters.
        public bool UseUnicode;

        // An identifier that specifies the folder to be moved.
        public FolderID FolderId;

        // A null-terminated multibyte string that specifies the name for the new moved folder. 
        public MAPIString NewFolderName;

        /// <summary>
        /// Parse the RopCopyFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.SourceHandleIndex = ReadByte();
            this.DestHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.WantRecursive = ReadBoolean();
            this.UseUnicode = ReadBoolean();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            if (UseUnicode)
            {
                this.NewFolderName = new MAPIString(Encoding.Unicode);
                this.NewFolderName.Parse(s);
            }
            else
            {
                this.NewFolderName = new MAPIString(Encoding.ASCII);
                this.NewFolderName.Parse(s);
            }
        }
    }

    ///  <summary>
    /// A class indicates the RopCopyFolder ROP Response Buffer.
    /// </summary>
    public class RopCopyFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request. 
        public byte SourceHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        //  An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        public uint? DestHandleIndex;

        //A Boolean that indicates whether the operation was only partially completed.
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopCopyFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCopyFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SourceHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = ReadUint();
                this.PartialCompletion = ReadBoolean();
            }
            else
            {
                this.PartialCompletion = ReadBoolean();
            }
        }
    }

    #endregion

    #region 2.2.1.9	RopEmptyFolder ROP
    /// <summary>
    /// The RopEmptyFolder ROP ([MS-OXCROPS] section 2.2.4.9) is used to soft delete messages and subfolders from a folder without deleting the folder itself. 
    /// </summary>
    public class RopEmptyFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A Boolean that specifies whether the operation also deletes folder associated information (FAI) messages.
        public bool WantDeleteAssociated;

        /// <summary>
        /// Parse the RopEmptyFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopEmptyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.WantDeleteAssociated = ReadBoolean();
        }
    }

    ///  <summary>
    /// A class indicates the RopEmptyFolder ROP Response Buffer.
    /// </summary>
    public class RopEmptyFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that indicates whether the operation was only partially completed.
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopEmptyFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopEmptyFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.PartialCompletion = ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.10	RopHardDeleteMessagesAndSubfolders ROP
    /// <summary>
    /// The RopHardDeleteMessagesAndSubfolders ROP ([MS-OXCROPS] section 2.2.4.10) is used to hard delete all messages and subfolders from a folder without deleting the folder itself.
    /// </summary>
    public class RopHardDeleteMessagesAndSubfoldersRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A Boolean that specifies whether the operation also deletes folder associated information (FAI) messages.
        public bool WantDeleteAssociated;

        /// <summary>
        /// Parse the RopHardDeleteMessagesAndSubfoldersRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopHardDeleteMessagesAndSubfoldersRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.WantDeleteAssociated = ReadBoolean();
        }
    }

    ///  <summary>
    /// A class indicates the RopHardDeleteMessagesAndSubfolders ROP Response Buffer.
    /// </summary>
    public class RopHardDeleteMessagesAndSubfoldersResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that indicates whether the operation was only partially completed.
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopHardDeleteMessagesAndSubfoldersResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopHardDeleteMessagesAndSubfoldersResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.PartialCompletion = ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.11	RopDeleteMessages ROP
    /// <summary>
    /// The RopDeleteMessages ROP ([MS-OXCROPS] section 2.2.4.11) is used to soft delete one or more messages from a folder. 
    /// </summary>
    public class RopDeleteMessagesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A Boolean that specifies whether the server sends a non-read receipt to the message sender when a message is deleted.
        public bool NotifyNonRead;

        // An unsigned integer that specifies the number of identifiers in the MessageIds field.
        public ushort MessageIdCount;

        // An array of 64-bit identifiers that specifies the messages to be deleted. T
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopDeleteMessagesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeleteMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.NotifyNonRead = ReadBoolean();
            this.MessageIdCount = ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }
            this.MessageIds = tempMessageIDs.ToArray();
        }
    }

    ///  <summary>
    /// A class indicates the RopDeleteMessages ROP Response Buffer.
    /// </summary>
    public class RopDeleteMessagesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that indicates whether the operation was only partially completed.
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopDeleteMessagesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeleteMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.PartialCompletion = ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.12	RopHardDeleteMessages ROP
    /// <summary>
    /// The RopHardDeleteMessages ROP ([MS-OXCROPS] section 2.2.4.12) is used to hard delete one or more messages from a folder.
    /// </summary>
    public class RopHardDeleteMessagesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A Boolean that specifies whether the server sends a non-read receipt to the message sender when a message is deleted.
        public bool NotifyNonRead;

        // An unsigned integer that specifies the number of identifiers in the MessageIds field.
        public ushort MessageIdCount;

        // An array of 64-bit identifiers that specifies the messages to be deleted. T
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopHardDeleteMessagesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopHardDeleteMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.NotifyNonRead = ReadBoolean();
            this.MessageIdCount = ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }
            this.MessageIds = tempMessageIDs.ToArray();
        }
    }

    ///  <summary>
    /// A class indicates the RopHardDeleteMessages ROP Response Buffer.
    /// </summary>
    public class RopHardDeleteMessagesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that indicates whether the operation was only partially completed.
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopHardDeleteMessagesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopHardDeleteMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.PartialCompletion = ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.13	RopGetHierarchyTable ROP
    /// <summary>
    /// The RopGetHierarchyTable ROP ([MS-OXCROPS] section 2.2.4.13) is used to retrieve the hierarchy table for a folder. 
    /// </summary>
    public class RopGetHierarchyTableRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // These flags control the type of table.
        public HierarchyTableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetHierarchyTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetHierarchyTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.TableFlags = (HierarchyTableFlags)ReadByte();
        }
    }

    ///  <summary>
    /// A class indicates the RopGetHierarchyTable ROP Response Buffer.
    /// </summary>
    public class RopGetHierarchyTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that represents the number of rows in the hierarchy table. 
        public uint? RowCount;

        /// <summary>
        /// Parse the RopGetHierarchyTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetHierarchyTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = ReadUint();
            }
        }
    }

    #endregion

    #region 2.2.1.14	RopGetContentsTable ROP
    /// <summary>
    /// The RopGetContentsTable ROP ([MS-OXCROPS] section 2.2.4.14) is used to retrieve the contents table for a folder. 
    /// </summary>
    public class RopGetContentsTableRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // These flags control the type of table.
        public HierarchyTableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetContentsTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetContentsTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.TableFlags = (HierarchyTableFlags)ReadByte();
        }
    }

    ///  <summary>
    /// A class indicates the RopGetContentsTable ROP Response Buffer.
    /// </summary>
    public class RopGetContentsTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that represents the number of rows in the hierarchy table. 
        public uint? RowCount;

        /// <summary>
        /// Parse the RopGetContentsTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetContentsTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = ReadUint();
            }
        }
    }

    #endregion

}
