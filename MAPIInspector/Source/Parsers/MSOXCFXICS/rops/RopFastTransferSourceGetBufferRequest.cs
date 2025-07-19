using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopFastTransferSourceGetBuffer ROP Request Buffer.
    /// 2.2.3.1.1.5.1 RopFastTransferSourceGetBuffer ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceGetBufferRequest : Block
    {
        /// <summary>
        /// A byte that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// A byte that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// A byte that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An UShort that specifies the buffer size requested.
        /// </summary>
        public BlockT<ushort> BufferSize;

        /// <summary>
        /// An UShort that is present when the BufferSize field is set to 0xBABE.
        /// </summary>
        public BlockT<ushort> MaximumBufferSize;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            BufferSize = ParseT<ushort>();
            if (BufferSize == 0xBABE)
            {
                MaximumBufferSize = ParseT<ushort>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopFastTransferSourceGetBufferRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(BufferSize, "BufferSize");
            AddChildBlockT(MaximumBufferSize, "MaximumBufferSize ");
        }
    }
}
