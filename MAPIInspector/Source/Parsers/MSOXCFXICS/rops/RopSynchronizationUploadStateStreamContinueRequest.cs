using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSynchronizationUploadStateStreamContinue ROP Request Buffer.
    /// 2.2.3.2.2.2.1 RopSynchronizationUploadStateStreamContinue ROP Request Buffer
    /// </summary>
    public class RopSynchronizationUploadStateStreamContinueRequest : Block
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
        /// An unsigned integer that specifies the size, in bytes, of the StreamData field.
        /// </summary>
        public BlockT<uint> StreamDataSize;

        /// <summary>
        /// An array of bytes that contains the state stream data to be uploaded.
        /// </summary>
        public BlockBytes StreamData;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamContinueRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            StreamDataSize = ParseT<uint>();
            StreamData = ParseBytes(StreamDataSize);
        }

        protected override void ParseBlocks()
        {
            Text = "RopSynchronizationUploadStateStreamContinueRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(StreamDataSize, "StreamDataSize");
            AddChildBytes(StreamData, "StreamData");
        }
    }
}
