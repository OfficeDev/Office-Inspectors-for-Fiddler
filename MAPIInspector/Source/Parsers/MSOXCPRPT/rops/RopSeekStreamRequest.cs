using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.21 RopSeekStream
    /// A class indicates the RopSeekStream ROP Request Buffer.
    /// </summary>
    public class RopSeekStreamRequest : Block
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
        /// An enumeration that specifies the origin location for the seek operation.
        /// </summary>
        public BlockT<Origin> Origin;

        /// <summary>
        /// An unsigned integer that specifies the seek offset.
        /// </summary>
        public BlockT<ulong> _Offset;

        /// <summary>
        /// Parse the RopSeekStreamRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            Origin = ParseT<Origin>();
            _Offset = ParseT<ulong>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSeekStreamRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(Origin, "Origin");
            AddChildBlockT(_Offset, "Offset");
        }
    }
}
