using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFOLD] 2.2.1.1.1 RopOpenFolder ROP Request Buffer
    /// The RopOpenFolder ROP ([MS-OXCROPS] section 2.2.4.1) opens an existing folder.
    /// [MS-OXCROPS] 2.2.4.1.1 RopOpenFolder ROP Request Buffer
    /// </summary>
    public class RopOpenFolderRequest : Block
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
        /// A 64-bit identifier that specifies the folder to be opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// An 8-bit flags structure that contains flags that are used to control how the folder is opened.
        /// </summary>
        public BlockT<OpenModeFlagsMSOXCFOLD> OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenFolderRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            FolderId = Parse<FolderID>();
            OpenModeFlags = ParseT<OpenModeFlagsMSOXCFOLD>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopOpenFolderRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChild(FolderId);
            AddChildBlockT(OpenModeFlags, "OpenModeFlags");
        }
    }
}
