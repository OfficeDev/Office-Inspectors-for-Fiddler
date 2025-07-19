using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.10 RopSeekRowBookmark ROP
    /// A class indicates the RopSeekRowBookmark ROP Response Buffer.
    /// </summary>
    public class RopSeekRowBookmarkResponse : Block
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
        /// A Boolean that specifies whether the bookmark target is no longer visible.
        /// </summary>
        public BlockT<bool> RowNoLongerVisible;

        /// <summary>
        /// A Boolean that specifies whether the full number of rows sought past was less than the number that was requested.
        /// </summary>
        public BlockT<bool> HasSoughtLess;

        /// <summary>
        /// An unsigned integer that specifies the direction and number of rows sought.
        /// </summary>
        BlockT<uint> RowsSought;

        /// <summary>
        /// Parse the RopSeekRowBookmarkResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                RowNoLongerVisible = ParseAs<byte, bool>();
                HasSoughtLess = ParseAs<byte, bool>();
                RowsSought = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSeekRowBookmarkResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(RowNoLongerVisible, "RowNoLongerVisible");
            AddChildBlockT(HasSoughtLess, "HasSoughtLess");
            AddChildBlockT(RowsSought, "RowsSought");
        }
    }
}
