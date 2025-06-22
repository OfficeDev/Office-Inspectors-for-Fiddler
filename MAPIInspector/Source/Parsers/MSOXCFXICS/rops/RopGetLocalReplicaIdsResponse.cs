namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;

    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Response Buffer.
    ///  2.2.13.13.2 RopGetLocalReplicaIds ROP Success Response Buffer
    /// </summary>
    public class RopGetLocalReplicaIdsResponse : Block
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
        /// This field contains the replica GUID that is shared by the IDs.
        /// </summary>
        public BlockGuid ReplGuid;

        /// <summary>
        /// An array of bytes that specifies the first value in the reserved range.
        /// </summary>
        public BlockBytes GlobalCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue.Data == ErrorCodes.Success)
            {
                ReplGuid = Parse<BlockGuid>();
                GlobalCount = ParseBytes(6);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetLocalReplicaIdsResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(ReturnValue, "ReturnValue");
            AddChild(ReplGuid, $"ReplGuid:{ReplGuid}");
            AddChild(GlobalCount, "GlobalCount");
        }
    }
}
