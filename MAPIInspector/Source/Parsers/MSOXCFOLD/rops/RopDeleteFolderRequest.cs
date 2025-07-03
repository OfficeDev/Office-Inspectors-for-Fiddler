using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.3 RopDeleteFolder ROP
    /// The RopDeleteFolder ROP ([MS-OXCROPS] section 2.2.4.3) removes a folder.
    /// </summary>
    public class RopDeleteFolderRequest : Block
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
        /// A flags structure that contains flags that control how to delete the folder.
        /// </summary>
        public BlockT<DeleteFolderFlags> DeleteFolderFlags;

        /// <summary>
        /// An identifier that specifies the folder to be deleted.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopDeleteFolderRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            DeleteFolderFlags = ParseT<DeleteFolderFlags>();
            FolderId = Parse<FolderID>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopDeleteFolderRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(DeleteFolderFlags, "DeleteFolderFlags");
            AddChild(FolderId, "FolderId");
        }
    }
}