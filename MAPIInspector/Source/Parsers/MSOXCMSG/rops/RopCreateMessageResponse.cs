using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.2 RopCreateMessage
    /// A class indicates the RopCreateMessage ROP response Buffer.
    /// </summary>
    public class RopCreateMessageResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex specified in field the request.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the MessageId field is present.
        /// </summary>
        public BlockT<bool> HasMessageId;

        /// <summary>
        /// An identifier that is present if HasMessageId is nonzero and is not present if it is zero.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopCreateMessageResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                HasMessageId = ParseAs<byte, bool>();
                if (HasMessageId)
                {
                    MessageId = Parse<MessageID>();
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopCreateMessageResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(HasMessageId, "HasMessageId");
            AddChild(MessageId, "MessageId");
        }
    }
}
