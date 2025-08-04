using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.3.7.1 RopPublicFolderIsGhosted ROP Request Buffer
    /// A class indicates the RopPublicFolderIsGhosted ROP Request Buffer.
    /// </summary>
    public class RopPublicFolderIsGhostedRequest : Block
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
        /// An identifier that specifies the folder to check.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            FolderId = Parse<FolderID>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopPublicFolderIsGhostedRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(FolderId, "FolderId");
        }
    }
}
