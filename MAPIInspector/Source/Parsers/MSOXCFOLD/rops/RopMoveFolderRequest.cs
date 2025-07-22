using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFOLD] 2.2.1.7 RopMoveFolder ROP
    /// The RopMoveFolder ROP ([MS-OXCROPS] section 2.2.4.7) moves a folder from one parent folder to another parent folder.
    /// </summary>
    public class RopMoveFolderRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public BlockT<byte> SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public BlockT<byte> DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public BlockT<bool> WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the NewFolderName field contains Unicode characters.
        /// </summary>
        public BlockT<bool> UseUnicode;

        /// <summary>
        /// An identifier that specifies the folder to be moved.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated string that specifies the name for the new moved folder.
        /// </summary>
        public BlockString NewFolderName;

        /// <summary>
        /// Parse the RopMoveFolderRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            SourceHandleIndex = ParseT<byte>();
            DestHandleIndex = ParseT<byte>();
            WantAsynchronous = ParseAs<byte, bool>();
            UseUnicode = ParseAs<byte, bool>();
            FolderId = Parse<FolderID>();
            if (UseUnicode)
            {
                NewFolderName = ParseStringW();
            }
            else
            {
                NewFolderName = ParseStringA();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopMoveFolderRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
            AddChildBlockT(WantAsynchronous, "WantAsynchronous");
            AddChildBlockT(UseUnicode, "UseUnicode");
            AddChild(FolderId);
            AddChildString(NewFolderName, "NewFolderName");
        }
    }
}
