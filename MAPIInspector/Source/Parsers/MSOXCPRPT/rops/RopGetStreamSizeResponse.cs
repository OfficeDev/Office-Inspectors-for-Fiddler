using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.9.6.2 RopGetStreamSize ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.9.6.3 RopGetStreamSize ROP Failure Response Buffer
    /// A class indicates the RopGetStreamSize ROP Response Buffer.
    /// </summary>
    public class RopGetStreamSizeResponse : Block
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
        /// An unsigned integer that is the current size of the stream.
        /// </summary>
        public BlockT<uint> StreamSize;

        /// <summary>
        /// Parse the RopGetStreamSizeResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                StreamSize = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetStreamSizeResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(StreamSize, "StreamSize");
        }
    }
}
