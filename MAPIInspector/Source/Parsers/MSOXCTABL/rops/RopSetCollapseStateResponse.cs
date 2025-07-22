using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.20 RopSetCollapseState ROP
    /// A class indicates the RopSetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopSetCollapseStateResponse : Block
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
        /// An array of bytes that specifies the origin for the seek operation.
        /// </summary>
        public BlockBytes Bookmark;

        /// <summary>
        /// Parse the RopSetCollapseStateResponse structure.
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
            Text = "RopSetCollapseStateResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(BookmarkSize, "BookmarkSize");
            AddChildBytes(Bookmark, "Bookmark");
        }
    }
}
