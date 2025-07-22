using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.22 RopCopyToStream
    /// A class indicates the RopCopyToStream ROP Response Buffer.
    /// </summary>
    public class RopCopyToStreamResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public BlockT<uint> DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes read from the source object.
        /// </summary>
        public BlockT<ulong> ReadByteCount;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes written to the destination object.
        /// </summary>
        public BlockT<ulong> WrittenByteCount;

        /// <summary>
        /// Parse the RopCopyToStreamResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            SourceHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if ((AdditionalErrorCodes)ReturnValue.Data == AdditionalErrorCodes.NullDestinationObject)
            {
                DestHandleIndex = ParseT<uint>();
            }

            ReadByteCount = ParseT<ulong>();
            WrittenByteCount = ParseT<ulong>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopCopyToStreamResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
            AddChildBlockT(ReadByteCount, "ReadByteCount");
            AddChildBlockT(WrittenByteCount, "WrittenByteCount");
        }
    }
}
