using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSetLocalReplicaMidsetDeleted ROP Response Buffer.
    /// [MS-OXCROPS] 2.2.13.12.2 RopSetLocalReplicaMidsetDeleted ROP Response Buffer
    /// </summary>
    public class RopSetLocalReplicaMidsetDeletedResponse : Block
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
        /// Parse the RopSetLocalReplicaMidsetDeletedResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSetLocalReplicaMidsetDeletedResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
        }
    }
}
