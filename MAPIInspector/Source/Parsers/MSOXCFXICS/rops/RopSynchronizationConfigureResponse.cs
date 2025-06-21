using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopSynchronizationConfigure ROP Response Buffer.
    ///  2.2.3.2.1.1.2 RopSynchronizationConfigure ROP Response Buffer
    /// </summary>
    public class RopSynchronizationConfigureResponse : Block
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
        /// Parse the RopSynchronizationConfigureResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationConfigureResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(ReturnValue, "ReturnValue");
        }
    }
}
