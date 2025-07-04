using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.15 RopFreeBookmark ROP
    /// The RopFreeBookmark ROP ([MS-OXCROPS] section 2.2.5.14) frees the memory associated with a bookmark that was returned by a previous RopCreateBookmark ROP request ([MS-OXCROPS] section 2.2.5.11).
    /// </summary>
    public class RopFreeBookmarkRequest : Block
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
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        BlockT<ushort> BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the origin for the seek operation.
        /// </summary>
        public BlockBytes Bookmark;

        /// <summary>
        /// Parse the RopFreeBookmarkRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            BookmarkSize = ParseT<ushort>();
            Bookmark = ParseBytes(BookmarkSize);
        }

        protected override void ParseBlocks()
        {
            SetText("RopFreeBookmarkRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(BookmarkSize, "BookmarkSize");
            AddChildBytes(Bookmark, "Bookmark");
        }
    }
}
