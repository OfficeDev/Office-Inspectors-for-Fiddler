using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamBegin ROP Request Buffer.
    ///  2.2.3.2.2.1.1 RopSynchronizationUploadStateStreamBegin ROP Request Buffer
    /// </summary>
    public class RopSynchronizationUploadStateStreamBeginRequest : Block
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
        /// A PropertyTag structure.
        /// </summary>
        public PropertyTag StateProperty;

        /// <summary>
        /// An unsigned integer that specifies the size of the stream to be uploaded.
        /// </summary>
        public BlockT<uint> TransferBufferSize;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamBeginRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            StateProperty = Parse<PropertyTag>();
            TransferBufferSize = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationUploadStateStreamBeginRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(StateProperty, "StateProperty");
            AddChildBlockT(TransferBufferSize, "TransferBufferSize");
        }
    }
}
