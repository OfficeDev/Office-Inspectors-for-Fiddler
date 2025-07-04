using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.12 RopCreateBookmark ROP
    /// A class indicates the RopCreateBookmark ROP Response Buffer.
    /// </summary>
    public class RopCreateBookmarkResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public BlockT<ushort> BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the bookmark created. The size of this field, in bytes, is specified by the BookmarkSize field.
        /// </summary>
        public BlockBytes Bookmark;

        /// <summary>
        /// Parse the RopCreateBookmarkResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                BookmarkSize = ParseT<ushort>();
                Bookmark = ParseBytes((int)BookmarkSize);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopCreateBookmarkResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(BookmarkSize, "BookmarkSize");
            AddChildBytes(Bookmark, "Bookmark");
        }
    }
}
