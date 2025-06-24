namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2.6.15 RopSaveChangesAttachment ROP
    /// A class indicates the RopSaveChangesAttachment ROP response Buffer.
    /// </summary>
    public class RopSaveChangesAttachmentResponse : Block
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
        /// Parse the RopSaveChangesAttachmentResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            ResponseHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSaveChangesAttachmentResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(ResponseHandleIndex, "ResponseHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
        }
    }
}
