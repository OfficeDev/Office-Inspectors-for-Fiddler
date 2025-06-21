namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationConfigure ROP Response Buffer.
    ///  2.2.3.1.2.1.2 RopFastTransferDestinationConfigure ROP Response Buffer
    /// </summary>
    public class RopFastTransferDestinationConfigureResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferDestinationConfigureResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferDestinationConfigureResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ReturnValue, "ReturnValue");
        }
    }
}
