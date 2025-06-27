using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.13 RopCreateAttachment ROP
    /// A class indicates the RopCreateAttachment ROP response Buffer.
    /// </summary>
    public class RopCreateAttachmentResponse : Block
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
        /// An unsigned integer identifier that refers to the attachment created.
        /// </summary>
        public BlockT<uint> AttachmentID;

        /// <summary>
        /// Parse the RopCreateAttachmentResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                AttachmentID = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopCreateAttachmentResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(AttachmentID, "AttachmentID");
        }
    }
}
