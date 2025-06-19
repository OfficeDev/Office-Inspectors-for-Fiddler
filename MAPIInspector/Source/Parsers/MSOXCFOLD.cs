namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    #region The enum value that used by ROPs.

    /// <summary>
    /// Section 2.2.1.1.1   RopOpenFolder ROP Request Buffer
    /// </summary>
    [Flags]
    public enum OpenModeFlagsMSOXCFOLD : byte
    {
        /// <summary>
        /// The operation opens either an existing folder or a soft-deleted folder
        /// </summary>
        OpenSoftDeleted = 0x04
    }

    /// <summary>
    /// Section 2.2.1.2.1   RopCreateFolder ROP Request Buffer
    /// </summary>
    public enum FolderType : byte
    {
        /// <summary>
        /// Generic folder
        /// </summary>
        GenericFolder = 1,

        /// <summary>
        /// Search folder
        /// </summary>
        SearchFolder = 2
    }

    /// <summary>
    /// Section 2.2.1.3.1   RopDeleteFolder ROP Request Buffer
    /// </summary>
    [Flags]
    public enum DeleteFolderFlags : byte
    {
        /// <summary>
        /// The folder and all of the Message objects in the folder are deleted.
        /// </summary>
        DEL_MESSAGES = 0x01,

        /// <summary>
        /// The folder and all of its subfolders are deleted
        /// </summary>
        DEL_FOLDERS = 0x04,

        /// <summary>
        /// The folder is hard deleted
        /// </summary>
        DELETE_HARD_DELETE = 0x10
    }

    /// <summary>
    /// Section 2.2.1.4.1   RopSetSearchCriteria ROP Request Buffer
    /// </summary>
    [Flags]
    public enum SearchRequestFlags : uint
    {
        /// <summary>
        /// The search is aborted
        /// </summary>
        STOP_SEARCH = 0x00000001,

        /// <summary>
        /// The search is initiated
        /// </summary>
        RESTART_SEARCH = 0x00000002,

        /// <summary>
        /// The search includes the search folder containers and all of their child folders.
        /// </summary>
        RECURSIVE_SEARCH = 0x00000004,

        /// <summary>
        /// The search includes only the search folder containers that are specified in the FolderIds field
        /// </summary>
        SHALLOW_SEARCH = 0x00000008,

        /// <summary>
        /// The search uses a content-indexed search
        /// </summary>
        CONTENT_INDEXED_SEARCH = 0x00010000,

        /// <summary>
        /// The search does not use a content-indexed search
        /// </summary>
        NON_CONTENT_INDEXED_SEARCH = 0x00020000,

        /// <summary>
        /// The search is static
        /// </summary>
        STATIC_SEARCH = 0x00040000
    }

    /// <summary>
    /// Section 2.2.1.5.2   RopGetSearchCriteria ROP Response Buffer
    /// </summary>
    [Flags]
    public enum SearchResponseFlags : uint
    {
        /// <summary>
        /// The search is running
        /// </summary>
        SEARCH_RUNNING = 0x00000001,

        /// <summary>
        /// The search is in the CPU-intensive part of the search
        /// </summary>
        SEARCH_REBUILD = 0x00000002,

        /// <summary>
        /// the specified search folder containers and all their child search folder containers are searched for matching entries
        /// </summary>
        SEARCH_RECURSIVE = 0x00000004,

        /// <summary>
        /// The search results are complete
        /// </summary>
        SEARCH_COMPLETE = 0x00001000,

        /// <summary>
        /// Only some parts of messages were included
        /// </summary>
        SEARCH_PARTIAL = 0x00002000,

        /// <summary>
        /// The search is static
        /// </summary>
        SEARCH_STATIC = 0x00010000,

        /// <summary>
        /// The search is still being evaluated
        /// </summary>
        SEARCH_MAYBE_STATIC = 0x00020000,

        /// <summary>
        /// The search is done using content indexing.
        /// </summary>
        CI_TOTALLY = 0x01000000,

        /// <summary>
        /// The search is done without using content indexing
        /// </summary>
        TWIR_TOTALLY = 0x08000000
    }

    /// <summary>
    /// Section 2.2.1.13.1   RopGetHierarchyTable ROP Request Buffer
    /// </summary>
    [Flags]
    public enum HierarchyTableFlags : byte
    {
        /// <summary>
        /// the hierarchy table lists folders from all levels under the folder
        /// </summary>
        Depth = 0x04,

        /// <summary>
        /// Deferred Errors
        /// </summary>
        DeferredErrors = 0x08,

        /// <summary>
        /// The hierarchy table notifications to the client are disabled
        /// </summary>
        NoNotifications = 0x10,

        /// <summary>
        /// The hierarchy table lists only the folders that are soft deleted
        /// </summary>
        SoftDeletes = 0x20,

        /// <summary>
        /// The columns that contain string data are returned in Unicode format
        /// </summary>
        UseUnicode = 0x40,

        /// <summary>
        /// The notifications generated by the client's actions on the hierarchy table are suppressed
        /// </summary>
        SuppressesNotifications = 0x80
    }

    /// <summary>
    /// Section 2.2.1.14.1   RopGetContentsTable ROP Request Buffer
    /// </summary>
    [Flags]
    public enum ContentsTableFlags : byte
    {
        /// <summary>
        /// The contents table lists only the FAI messages.
        /// </summary>
        Associated = 0x02,

        /// <summary>
        /// Deferred Errors
        /// </summary>
        DeferredErrors = 0x08,

        /// <summary>
        /// The contents table notifications to the client are disabled
        /// </summary>
        NoNotifications = 0x10,

        /// <summary>
        /// The contents table lists only the messages that are soft deleted
        /// </summary>
        SoftDeletes = 0x20,

        /// <summary>
        /// The columns that contain string data are returned in Unicode format
        /// </summary>
        UseUnicode = 0x40,

        /// <summary>
        /// The contents table lists messages pertaining to a single conversation (one result row represents a single message)
        /// </summary>
        ConversationMembers = 0x80
    }

    #endregion

    #region 2.2.1.1	RopOpenFolder ROP
    /// <summary>
    /// The RopOpenFolder ROP ([MS-OXCROPS] section 2.2.4.1) opens an existing folder. 
    /// </summary>
    public class RopOpenFolderRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A 64-bit identifier that specifies the folder to be opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// An 8-bit flags structure that contains flags that are used to control how the folder is opened.
        /// </summary>
        public OpenModeFlagsMSOXCFOLD OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.OpenModeFlags = (OpenModeFlagsMSOXCFOLD)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopOpenFolder ROP Response Buffer.
    /// </summary>
    public class RopOpenFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that indicates whether the folder has rules associated with it.
        /// </summary>
        public bool? HasRules;

        /// <summary>
        /// A Boolean that specifies whether the folder is a ghosted folder.
        /// </summary>
        public bool? IsGhosted;

        /// <summary>
        /// This value specifies the number of strings in the Servers field.
        /// </summary>
        public ushort? ServerCount;

        /// <summary>
        /// This value specifies the number of values in the Servers field that refer to lowest-cost servers.
        /// </summary>
        public ushort? CheapServerCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (2) of this folder. 
        /// </summary>
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopOpenFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.HasRules = this.ReadBoolean();
                this.IsGhosted = this.ReadBoolean();
                if ((bool)this.IsGhosted)
                {
                    this.ServerCount = this.ReadUshort();
                    this.CheapServerCount = this.ReadUshort();
                    List<MAPIString> tempServers = new List<MAPIString>();
                    for (int i = 0; i < this.ServerCount; i++)
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An enumeration that specifies what type of folder to create. 
        /// </summary>
        public FolderType FolderType;

        /// <summary>
        /// A Boolean that specifies whether DisplayName and Comment fields are formated in Unicode.
        /// </summary>
        public bool UseUnicodeStrings;

        /// <summary>
        /// Boolean that specifies whether this operation opens a Folder object or fails when the Folder object already exists.
        /// </summary>
        public bool OpenExisting;

        /// <summary>
        /// Reserved. This field MUST be set to 0x00.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// A null-terminated string that specifies the name of the created folder. 
        /// </summary>
        public MAPIString DisplayName;

        /// <summary>
        /// A null-terminated folder string that specifies the folder comment that is associated with the created folder. 
        /// </summary>
        public MAPIString Comment;

        /// <summary>
        /// Parse the RopCreateFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.FolderType = (FolderType)this.ReadByte();
            this.UseUnicodeStrings = this.ReadBoolean();
            this.OpenExisting = this.ReadBoolean();
            this.Reserved = this.ReadByte();
            if (this.UseUnicodeStrings)
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

    /// <summary>
    /// A class indicates the RopCreateFolder ROP Response Buffer.
    /// </summary>
    public class RopCreateFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An identifier that specifies the folder created or opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A Boolean that indicates whether an existing folder was opened or a new folder was created.
        /// </summary>
        public bool? IsExistingFolder;

        /// <summary>
        /// A Boolean that indicates whether the folder has rules associated with it.
        /// </summary>
        public bool? HasRules;

        /// <summary>
        /// A Boolean that indicates whether the server is an active replica of this folder. 
        /// </summary>
        public bool? IsGhosted;

        /// <summary>
        /// This value specifies the number of strings in the Servers field.
        /// </summary>
        public ushort? ServerCount;

        /// <summary>
        /// This value specifies the number of values in the Servers field that refer to lowest-cost servers.
        /// </summary>
        public ushort? CheapServerCount;

        /// <summary>
        /// These strings specify which servers have replicas (2) of this folder.
        /// </summary>
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopCreateFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
                this.IsExistingFolder = this.ReadBoolean();
                if ((bool)this.IsExistingFolder)
                {
                    this.HasRules = this.ReadBoolean();
                    this.IsGhosted = this.ReadBoolean();
                    if ((bool)this.IsGhosted)
                    {
                        this.ServerCount = this.ReadUshort();
                        this.CheapServerCount = this.ReadUshort();
                        List<MAPIString> tempServers = new List<MAPIString>();
                        for (int i = 0; i < this.ServerCount; i++)
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
        /// A flags structure that contains flags that control how to delete the folder. 
        /// </summary>
        public DeleteFolderFlags DeleteFolderFlags;

        /// <summary>
        /// An identifier that specifies the folder to be deleted.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopDeleteFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.DeleteFolderFlags = (DeleteFolderFlags)this.ReadByte();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
        }
    }

    /// <summary>
    /// A class indicates the RopDeleteFolder ROP Response Buffer.
    /// </summary>
    public class RopDeleteFolderResponse : BaseStructure
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
        /// A Boolean that specifies whether the operation was partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopDeleteFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            this.PartialCompletion = this.ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.4	RopSetSearchCriteria ROP
    /// <summary>
    /// The RopSetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.4) establishes search criteria for a search folder.
    /// </summary>
    public class RopSetSearchCriteriaRequest : BaseStructure
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
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder. 
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the FolderIds field.
        /// </summary>
        public ushort FolderIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which folders are searched. 
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// A flags structure that contains flags that control the search for a search folder.
        /// </summary>
        public SearchRequestFlags SearchFlags;

        /// <summary>
        /// Parse the RopSetSearchCriteriaRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetSearchCriteriaRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.RestrictionDataSize = this.ReadUshort();
            if (this.RestrictionDataSize > 0)
            {
                this.RestrictionData = new RestrictionType();
                this.RestrictionData.Parse(s);
            }

            this.FolderIdCount = this.ReadUshort();
            List<FolderID> tempFolderIDs = new List<FolderID>();
            for (int i = 0; i < this.FolderIdCount; i++)
            {
                FolderID folderID = new FolderID();
                folderID.Parse(s);
                tempFolderIDs.Add(folderID);
            }

            this.FolderIds = tempFolderIDs.ToArray();
            this.SearchFlags = (SearchRequestFlags)ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopSetSearchCriteria ROP Response Buffer.
    /// </summary>
    public class RopSetSearchCriteriaResponse : BaseStructure
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
        /// Parse the RopSetSearchCriteriaResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetSearchCriteriaResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }

    #endregion

    #region 2.2.1.5	RopGetSearchCriteria ROP
    /// <summary>
    /// The RopGetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.5) obtains the search criteria and the status of a search for a search folder. 
    /// </summary>
    public class RopGetSearchCriteriaRequest : BaseStructure
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
        /// A Boolean that specifies whether the client requests the restriction data (returned in the RestrictionData field of the response) to be specified with Unicode strings or with ASCII strings. 
        /// </summary>
        public bool UseUnicode;

        /// <summary>
        /// A Boolean that specifies whether the server includes the restriction information in the response.
        /// </summary>
        public bool IncludeRestriction;

        /// <summary>
        /// A Boolean that specifies whether the server includes the folders list in the response.
        /// </summary>
        public bool IncludeFolders;

        /// <summary>
        /// Parse the RopGetSearchCriteriaRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetSearchCriteriaRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.UseUnicode = this.ReadBoolean();
            this.IncludeRestriction = this.ReadBoolean();
            this.IncludeFolders = this.ReadBoolean();
        }
    }

    /// <summary>
    /// A class indicates the RopGetSearchCriteria ROP Response Buffer.
    /// </summary>
    public class RopGetSearchCriteriaResponse : BaseStructure
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
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort? RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder. 
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An unsigned integer that MUST be set to the value of the LogonId field in the request.
        /// </summary>
        public byte? LogonId;

        /// <summary>
        ///  An unsigned integer that specifies the number of identifiers in the FolderIds field.
        /// </summary>
        public ushort? FolderIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which folders are searched. 
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        ///  A flags structure that contains flags that control the search for a search folder. 
        /// </summary>
        public SearchResponseFlags SearchFlags;

        /// <summary>
        /// Parse the RopGetSearchCriteriaResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetSearchCriteriaResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RestrictionDataSize = this.ReadUshort();
                if (this.RestrictionDataSize > 0)
                {
                    this.RestrictionData = new RestrictionType();
                    this.RestrictionData.Parse(s);
                }

                this.LogonId = this.ReadByte();
                this.FolderIdCount = this.ReadUshort();
                List<FolderID> tempFolderIDs = new List<FolderID>();
                for (int i = 0; i < this.FolderIdCount; i++)
                {
                    FolderID folderID = new FolderID();
                    folderID.Parse(s);
                    tempFolderIDs.Add(folderID);
                }

                this.FolderIds = tempFolderIDs.ToArray();
                this.SearchFlags = (SearchResponseFlags)this.ReadUint();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which messages to move or copy. 
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the operation is a copy or a move.
        /// </summary>
        public bool WantCopy;

        /// <summary>
        /// Parse the RopMoveCopyMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopMoveCopyMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.DestHandleIndex = this.ReadByte();
            this.MessageIdCount = this.ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < this.MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }

            this.MessageIds = tempMessageIDs.ToArray();
            this.WantAsynchronous = this.ReadBoolean();
            this.WantCopy = this.ReadBoolean();
        }
    }

    /// <summary>
    /// A class indicates the RopMoveCopyMessages ROP Response Buffer.
    /// </summary>
    public class RopMoveCopyMessagesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request. 
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopMoveCopyMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopMoveCopyMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = this.ReadUint();
                this.PartialCompletion = this.ReadBoolean();
            }
            else
            {
                this.PartialCompletion = this.ReadBoolean();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the NewFolderName field contains Unicode characters.
        /// </summary>
        public bool UseUnicode;

        /// <summary>
        /// An identifier that specifies the folder to be moved.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated string that specifies the name for the new moved folder. 
        /// </summary>
        public MAPIString NewFolderName;

        /// <summary>
        /// Parse the RopMoveFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopMoveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.DestHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.UseUnicode = this.ReadBoolean();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            if (this.UseUnicode)
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

    /// <summary>
    /// A class indicates the RopMoveFolder ROP Response Buffer.
    /// </summary>
    public class RopMoveFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request. 
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopMoveFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopMoveFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = this.ReadUint();
                this.PartialCompletion = this.ReadBoolean();
            }
            else
            {
                this.PartialCompletion = this.ReadBoolean();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies that the copy is recursive.
        /// </summary>
        public bool WantRecursive;

        /// <summary>
        /// A Boolean that specifies whether the NewFolderName field contains Unicode characters.
        /// </summary>
        public bool UseUnicode;

        /// <summary>
        /// An identifier that specifies the folder to be moved.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated string that specifies the name for the new moved folder. 
        /// </summary>
        public MAPIString NewFolderName;

        /// <summary>
        /// Parse the RopCopyFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.DestHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.WantRecursive = this.ReadBoolean();
            this.UseUnicode = this.ReadBoolean();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            if (this.UseUnicode)
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

    /// <summary>
    /// A class indicates the RopCopyFolder ROP Response Buffer.
    /// </summary>
    public class RopCopyFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request. 
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopCopyFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.SourceHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                this.DestHandleIndex = this.ReadUint();
                this.PartialCompletion = this.ReadBoolean();
            }
            else
            {
                this.PartialCompletion = this.ReadBoolean();
            }
        }
    }

    #endregion

    #region 2.2.1.9	RopEmptyFolder ROP
    /// <summary>
    /// The RopEmptyFolder ROP ([MS-OXCROPS] section 2.2.4.9) is used to soft delete messages and sub-folders from a folder without deleting the folder itself. 
    /// </summary>
    public class RopEmptyFolderRequest : BaseStructure
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the operation also deletes folder associated information (FAI) messages.
        /// </summary>
        public bool WantDeleteAssociated;

        /// <summary>
        /// Parse the RopEmptyFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopEmptyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.WantDeleteAssociated = this.ReadBoolean();
        }
    }

    /// <summary>
    /// A class indicates the RopEmptyFolder ROP Response Buffer.
    /// </summary>
    public class RopEmptyFolderResponse : BaseStructure
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
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopEmptyFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopEmptyFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            this.PartialCompletion = this.ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.10	RopHardDeleteMessagesAndSubfolders ROP
    /// <summary>
    /// The RopHardDeleteMessagesAndSubfolders ROP ([MS-OXCROPS] section 2.2.4.10) is used to hard delete all messages and sub-folders from a folder without deleting the folder itself.
    /// </summary>
    public class RopHardDeleteMessagesAndSubfoldersRequest : BaseStructure
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
        ///  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the operation also deletes folder associated information (FAI) messages.
        /// </summary>
        public bool WantDeleteAssociated;

        /// <summary>
        /// Parse the RopHardDeleteMessagesAndSubfoldersRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopHardDeleteMessagesAndSubfoldersRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.WantDeleteAssociated = this.ReadBoolean();
        }
    }

    /// <summary>
    /// A class indicates the RopHardDeleteMessagesAndSubfolders ROP Response Buffer.
    /// </summary>
    public class RopHardDeleteMessagesAndSubfoldersResponse : BaseStructure
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
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopHardDeleteMessagesAndSubfoldersResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopHardDeleteMessagesAndSubfoldersResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            this.PartialCompletion = this.ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.11	RopDeleteMessages ROP
    /// <summary>
    /// The RopDeleteMessages ROP ([MS-OXCROPS] section 2.2.4.11) is used to soft delete one or more messages from a folder. 
    /// </summary>
    public class RopDeleteMessagesRequest : BaseStructure
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the server sends a non-read receipt to the message sender when a message is deleted.
        /// </summary>
        public bool NotifyNonRead;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies the messages to be deleted. T
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopDeleteMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.NotifyNonRead = this.ReadBoolean();
            this.MessageIdCount = this.ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < this.MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }

            this.MessageIds = tempMessageIDs.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the RopDeleteMessages ROP Response Buffer.
    /// </summary>
    public class RopDeleteMessagesResponse : BaseStructure
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
        ///  An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopDeleteMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            this.PartialCompletion = this.ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.12	RopHardDeleteMessages ROP
    /// <summary>
    /// The RopHardDeleteMessages ROP ([MS-OXCROPS] section 2.2.4.12) is used to hard delete one or more messages from a folder.
    /// </summary>
    public class RopHardDeleteMessagesRequest : BaseStructure
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the server sends a non-read receipt to the message sender when a message is deleted.
        /// </summary>
        public bool NotifyNonRead;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies the messages to be deleted.
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopHardDeleteMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopHardDeleteMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.NotifyNonRead = this.ReadBoolean();
            this.MessageIdCount = this.ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < this.MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }

            this.MessageIds = tempMessageIDs.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the RopHardDeleteMessages ROP Response Buffer.
    /// </summary>
    public class RopHardDeleteMessagesResponse : BaseStructure
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
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopHardDeleteMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopHardDeleteMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            this.PartialCompletion = this.ReadBoolean();
        }
    }

    #endregion

    #region 2.2.1.13	RopGetHierarchyTable ROP
    /// <summary>
    /// The RopGetHierarchyTable ROP ([MS-OXCROPS] section 2.2.4.13) is used to retrieve the hierarchy table for a folder. 
    /// </summary>
    public class RopGetHierarchyTableRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// These flags control the type of table.
        /// </summary>
        public HierarchyTableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetHierarchyTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetHierarchyTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.TableFlags = (HierarchyTableFlags)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopGetHierarchyTable ROP Response Buffer.
    /// </summary>
    public class RopGetHierarchyTableResponse : BaseStructure
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
        /// An unsigned integer that represents the number of rows in the hierarchy table. 
        /// </summary>
        public uint? RowCount;

        /// <summary>
        /// Parse the RopGetHierarchyTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetHierarchyTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = this.ReadUint();
            }
        }
    }

    #endregion

    #region 2.2.1.14	RopGetContentsTable ROP
    /// <summary>
    /// The RopGetContentsTable ROP ([MS-OXCROPS] section 2.2.4.14) is used to retrieve the contents table for a folder. 
    /// </summary>
    public class RopGetContentsTableRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// These flags control the type of table.
        /// </summary>
        public BlockT<HierarchyTableFlags> TableFlags;

        /// <summary>
        /// Parse the RopGetContentsTableRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            TableFlags = ParseT<HierarchyTableFlags>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetContentsTableRequest");
            AddChild(RopId, "RopId:{0}", RopId.Data);
            AddChild(LogonId, "LogonId:0x{0:X2}", LogonId.Data);
            AddChild(InputHandleIndex, "InputHandleIndex:{0}", InputHandleIndex.Data);
            AddChild(OutputHandleIndex, "OutputHandleIndex:{0}", OutputHandleIndex.Data);
            AddChild(TableFlags, "TableFlags:{0}", TableFlags.Data);
        }
    }

    /// <summary>
    /// A class indicates the RopGetContentsTable ROP Response Buffer.
    /// </summary>
    public class RopGetContentsTableResponse : BaseStructure
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
        /// An unsigned integer that represents the number of rows in the hierarchy table. 
        /// </summary>
        public uint? RowCount;

        /// <summary>
        /// Parse the RopGetContentsTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetContentsTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = this.ReadUint();
            }
        }
    }
    #endregion
}