using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.15 RopReadStream
    /// A class indicates the RopReadStream ROP Response Buffer.
    /// </summary>
    public class RopReadStreamResponse : Block
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
        /// An unsigned integer that specifies the size, in bytes, of the Data field.
        /// </summary>
        public BlockT<ushort> DataSize;

        /// <summary>
        /// An array of bytes that are the bytes read from the stream.
        /// </summary>
        public BlockBytes Data;

        /// <summary>
        /// Parse the RopReadStreamResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            DataSize = ParseT<ushort>();
            Data = ParseBytes(DataSize);
        }

        protected override void ParseBlocks()
        {
            SetText("RopReadStreamResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(DataSize, "DataSize");
            AddChildBytes(Data, "Data");
        }
    }
}
