using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  2.2.2.14 RopOpenStream
    ///  A class indicates the RopOpenStream ROP Response Buffer.
    /// </summary>
    public class RopOpenStreamResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that indicates the size of the stream opened.
        /// </summary>
        public BlockT<uint> StreamSize;

        /// <summary>
        /// Parse the RopOpenStreamResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                StreamSize = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopOpenStreamResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(StreamSize, "StreamSize");
        }
    }
}
