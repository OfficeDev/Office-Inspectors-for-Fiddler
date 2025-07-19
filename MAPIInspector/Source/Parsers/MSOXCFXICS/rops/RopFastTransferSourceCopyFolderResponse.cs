using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopFastTransferSourceCopyFolder ROP Response Buffer.
    /// 2.2.3.1.1.4.2 RopFastTransferSourceCopyFolder ROP Response Buffer
    /// </summary>
    public class RopFastTransferSourceCopyFolderResponse : Block
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
        /// Parse the RopFastTransferSourceCopyFolderResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferSourceCopyFolderResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
        }
    }
}
