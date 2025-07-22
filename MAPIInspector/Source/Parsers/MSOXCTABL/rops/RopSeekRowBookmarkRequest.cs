using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.10 RopSeekRowBookmark ROP
    /// The RopSeekRowBookmark ROP ([MS-OXCROPS] section 2.2.5.9) moves the table cursor to a specific location in the table.
    /// </summary>
    public class RopSeekRowBookmarkRequest : Block
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
        /// A signed integer that specifies the direction and the number of rows to seek.
        /// </summary>
        BlockT<int> RowCount;

        /// <summary>
        /// A Boolean that specifies whether the server returns the actual number of rows sought in the response.
        /// </summary>
        BlockT<bool> WantRowMovedCount;

        /// <summary>
        /// Parse the RopSeekRowBookmarkRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            BookmarkSize = ParseT<ushort>();
            Bookmark = ParseBytes(BookmarkSize);
            RowCount = ParseT<int>();
            WantRowMovedCount = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSeekRowBookmarkRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(BookmarkSize, "BookmarkSize");
            AddChildBytes(Bookmark, "Bookmark");
            AddChildBlockT(RowCount, "RowCount");
            AddChildBlockT(WantRowMovedCount, "WantRowMovedCount");
        }
    }
}
