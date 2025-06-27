using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.15 RopReadStream
    /// A class indicates the RopReadStream ROP Request Buffer.
    /// </summary>
    public class RopReadStreamRequest : Block
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
        /// An unsigned integer that specifies the maximum number of bytes to read if the value is not equal to 0xBABE.
        /// </summary>
        public BlockT<ushort> ByteCount;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes to read if the value of the ByteCount field is equal to 0xBABE.
        /// </summary>
        public BlockT<uint> MaximumByteCount;

        /// <summary>
        /// Parse the RopReadStreamRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ByteCount = ParseT<ushort>();

            if (ByteCount == 0xBABE)
            {
                MaximumByteCount = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopReadStreamRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ByteCount, "ByteCount");
            AddChildBlockT(MaximumByteCount, "MaximumByteCount");
        }
    }
}
