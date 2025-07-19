using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.12 RopOpenAttachment ROP
    /// A class indicates the RopOpenAttachment ROP response Buffer.
    /// </summary>
    public class RopOpenAttachmentResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// Parse the RopOpenAttachmentResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            ResponseHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopOpenAttachmentResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(ResponseHandleIndex, "ResponseHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
        }
    }
}
