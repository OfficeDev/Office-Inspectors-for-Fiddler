using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.15.1 RopBufferTooSmall
    /// A class indicates the RopBufferTooSmall ROP Response Buffer.
    /// </summary>
    public class RopBufferTooSmallResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the size required for the ROP output buffer.
        /// </summary>
        public BlockT<ushort> SizeNeeded;

        /// <summary>
        /// An array of bytes that contains the section of the ROP input buffer that was not executed because of the insufficient size of the ROP output buffer.
        /// </summary>
        public BlockBytes RequestBuffers;

        /// <summary>
        /// An unsigned integer that specifies the size of RequestBuffers.
        /// </summary>
        private uint requestBuffersSize;

        /// <summary>
        /// Initializes a new instance of the RopBufferTooSmallResponse class.
        /// </summary>
        /// <param name="requestBuffersSize"> The size of RequestBuffers.</param>
        public RopBufferTooSmallResponse(uint requestBuffersSize)
        {
            this.requestBuffersSize = requestBuffersSize;
        }

        /// <summary>
        /// Parse the RopBufferTooSmallResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            SizeNeeded = ParseT<ushort>();
            RequestBuffers = ParseBytes((int)requestBuffersSize);
        }

        protected override void ParseBlocks()
        {
            SetText("RopBufferTooSmallResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(SizeNeeded, "SizeNeeded");
            AddChildBytes(RequestBuffers, "RequestBuffers");
        }
    }
}
