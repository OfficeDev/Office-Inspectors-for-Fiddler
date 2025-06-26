using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  2.2.2.20 RopSetStreamSize
    ///  A class indicates the RopSetStreamSize ROP Request Buffer.
    /// </summary>
    public class RopSetStreamSizeRequest : Block
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
        /// An unsigned integer that specifies the size of the stream.
        /// </summary>
        public BlockT<ulong> StreamSize;

        /// <summary>
        /// Parse the RopSetStreamSizeRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            StreamSize = ParseT<ulong>();
        }
        protected override void ParseBlocks()
        {
            SetText("RopSetStreamSizeRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(StreamSize, "StreamSize");
        }
    }
}
