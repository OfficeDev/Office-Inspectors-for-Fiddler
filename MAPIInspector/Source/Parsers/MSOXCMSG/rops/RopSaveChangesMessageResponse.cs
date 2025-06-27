using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.3 RopSaveChangesMessage ROP
    /// A class indicates the RopSaveChangesMessage ROP response Buffer.
    /// </summary>
    public class RopSaveChangesMessageResponse : Block
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
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the ID of the message saved.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSaveChangesMessageResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            ResponseHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                InputHandleIndex = ParseT<byte>();
                MessageId = Parse<MessageID>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSaveChangesMessageResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(ResponseHandleIndex, "ResponseHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(MessageId, "MessageId");
        }
    }
}
