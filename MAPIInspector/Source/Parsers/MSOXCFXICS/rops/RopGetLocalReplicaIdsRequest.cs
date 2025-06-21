using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Request Buffer.
    ///  2.2.13.13.1 RopGetLocalReplicaIds ROP Request Buffer
    /// </summary>
    public class RopGetLocalReplicaIdsRequest : Block
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
        /// An unsigned integer that specifies the number of IDs to reserve.
        /// </summary>
        public BlockT<uint> IdCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            IdCount = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetLocalReplicaIdsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(IdCount, "IdCount");
        }
    }
}
