using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.9.9.1 RopCopyToStream ROP Request Buffer
    /// A class indicates the RopCopyToStream ROP Request Buffer.
    /// </summary>
    public class RopCopyToStreamRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public BlockT<byte> SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public BlockT<byte> DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes to be copied.
        /// </summary>
        public BlockT<ulong> ByteCount;

        /// <summary>
        /// Parse the RopCopyToStreamRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            SourceHandleIndex = ParseT<byte>();
            DestHandleIndex = ParseT<byte>();
            ByteCount = ParseT<ulong>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopCopyToStreamRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
            AddChildBlockT(ByteCount, "ByteCount");
        }
    }
}
