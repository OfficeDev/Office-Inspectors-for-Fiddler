namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBufferExtended ROP Response Buffer.
    ///  2.2.3.1.2.3.2 RopFastTransferDestinationPutBufferExtended ROP Response Buffer
    /// </summary>
    public class RopFastTransferDestinationPutBufferExtendedResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// The current status of the transfer.
        /// </summary>
        public BlockT<ushort> TransferStatus;

        /// <summary>
        /// An unsigned integer that specifies the number of steps that have been completed in the current operation.
        /// </summary>
        public BlockT<uint> InProgressCount;

        /// <summary>
        /// An unsigned integer that specifies the approximate total number of steps to be completed in the current operation.
        /// </summary>
        public BlockT<uint> TotalStepCount;

        /// <summary>
        /// A reserved field
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// An unsigned integer that specifies the buffer size that was used.
        /// </summary>
        public BlockT<ushort> BufferUsedSize;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferExtendedResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            TransferStatus = ParseT<ushort>();
            InProgressCount = ParseT<uint>();
            TotalStepCount = ParseT<uint>();
            Reserved = ParseT<byte>();
            BufferUsedSize = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferSourceGetBufferResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ReturnValue, "ReturnValue");
            AddChildBlockT(TransferStatus, "TransferStatus");
            AddChildBlockT(InProgressCount, "InProgressCount");
            AddChildBlockT(TotalStepCount, "TotalStepCount");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(BufferUsedSize, "BufferUsedSize");
        }
    }
}
