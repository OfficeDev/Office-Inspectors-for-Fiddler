namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2.1.2 RopCreateFolder ROP
    /// The RopCreateFolder ROP ([MS-OXCROPS] section 2.2.4.2) creates a new folder
    /// </summary>
    public class RopCreateFolderRequest : Block
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
        /// An enumeration that specifies what type of folder to create. 
        /// </summary>
        public BlockT<FolderType> FolderType;

        /// <summary>
        /// A Boolean that specifies whether DisplayName and Comment fields are formated in Unicode.
        /// </summary>
        public BlockT<bool> UseUnicodeStrings;

        /// <summary>
        /// Boolean that specifies whether this operation opens a Folder object or fails when the Folder object already exists.
        /// </summary>
        public BlockT<bool> OpenExisting;

        /// <summary>
        /// Reserved. This field MUST be set to 0x00.
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// A null-terminated string that specifies the name of the created folder. 
        /// </summary>
        public BlockString DisplayName;

        /// <summary>
        /// A null-terminated folder string that specifies the folder comment that is associated with the created folder. 
        /// </summary>
        public BlockString Comment;

        /// <summary>
        /// Parse the RopCreateFolderRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            FolderType = ParseT<FolderType>();
            UseUnicodeStrings = ParseAs<byte, bool>();
            OpenExisting = ParseAs<byte, bool>();
            Reserved = ParseT<byte>();
            if (UseUnicodeStrings.Data)
            {
                DisplayName = ParseStringW();
                Comment = ParseStringW();
            }
            else
            {
                DisplayName = ParseStringA();
                Comment = ParseStringA();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopCreateFolderRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(FolderType, "FolderType");
            AddChildBlockT(UseUnicodeStrings, "UseUnicodeStrings");
            AddChildBlockT(OpenExisting, "OpenExisting");
            AddChildBlockT(Reserved, "Reserved");
            AddChildString(DisplayName, "DisplayName");
            AddChildString(Comment, "Comment");
        }
    }
}