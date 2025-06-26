using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopFastTransferDestinationConfigure ROP Request Buffer.
    ///  2.2.3.1.2.1.1 RopFastTransferDestinationConfigure ROP Request Buffer
    /// </summary>
    public class RopFastTransferDestinationConfigureRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An enumeration that indicates how the data stream was created on the source.
        /// </summary>
        public BlockT<SourceOperation> SourceOperation;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the transfer operation.
        /// </summary>
        public BlockT<CopyFlags_DestinationConfigure> CopyFlags;

        /// <summary>
        /// Parse the RopFastTransferDestinationConfigureRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            SourceOperation = ParseT<SourceOperation>();
            CopyFlags = ParseT<CopyFlags_DestinationConfigure>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferDestinationConfigureRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(SourceOperation, "SourceOperation");
            AddChildBlockT(CopyFlags, "CopyFlags");
        }
    }
}
