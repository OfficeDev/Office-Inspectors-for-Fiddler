using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.9.3.2 RopWriteStream ROP Response Buffer
    /// A class indicates the RopWriteStream ROP Response Buffer.
    /// </summary>
    public class RopWriteStreamResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes actually written.
        /// </summary>
        public BlockT<ushort> WrittenSize;

        /// <summary>
        /// Parse the RopWriteStreamResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            WrittenSize = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopWriteStreamResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(WrittenSize, "WrittenSize");
        }
    }
}
