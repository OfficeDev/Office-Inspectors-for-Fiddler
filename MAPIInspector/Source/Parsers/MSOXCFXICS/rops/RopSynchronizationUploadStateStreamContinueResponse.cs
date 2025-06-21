using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamContinue ROP Response Buffer.
    ///  2.2.3.2.2.2.2 RopSynchronizationUploadStateStreamContinue ROP Response Buffer
    /// </summary>
    public class RopSynchronizationUploadStateStreamContinueResponse : Block
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
        /// Parse the RopSynchronizationUploadStateStreamContinueResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationUploadStateStreamContinueResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ReturnValue, "ReturnValue");
        }
    }
}
