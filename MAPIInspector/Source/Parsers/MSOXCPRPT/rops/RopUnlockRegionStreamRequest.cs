using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.9.11.1 RopUnlockRegionStream ROP Request Buffer
    /// A class indicates the RopUnlockRegionStream ROP Request Buffer.
    /// </summary>
    public class RopUnlockRegionStreamRequest : Block
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
        /// An unsigned integer that specifies the byte location in the stream where the region begins.
        /// </summary>
        public BlockT<ulong> RegionOffset;

        /// <summary>
        /// An unsigned integer that specifies the size of the region, in bytes.
        /// </summary>
        public BlockT<ulong> RegionSize;

        /// <summary>
        /// A flags structure that contains flags specifying the behavior of the lock operation.
        /// </summary>
        public BlockT<uint> LockFlags;

        /// <summary>
        /// Parse the RopUnlockRegionStreamRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            RegionOffset = ParseT<ulong>();
            RegionSize = ParseT<ulong>();
            LockFlags = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopUnlockRegionStreamRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(RegionOffset, "RegionOffset");
            AddChildBlockT(RegionSize, "RegionSize");
            AddChildBlockT(LockFlags, "LockFlags");
        }
    }
}
