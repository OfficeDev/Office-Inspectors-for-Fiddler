using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.9 RopGetMessageStatus ROP
    /// A class indicates the RopGetMessageStatus ROP response Buffer.
    /// </summary>
    public class RopGetMessageStatusResponse : Block
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
        /// A flags structure that contains the status flags that were set on the message before this operation.
        /// </summary>
        public BlockT<MessageStatusFlag> MessageStatusFlags;

        /// <summary>
        /// Parse the RopGetMessageStatusResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                MessageStatusFlags = ParseT<MessageStatusFlag>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetMessageStatusResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(MessageStatusFlags, "MessageStatusFlags");
        }
    }
}
