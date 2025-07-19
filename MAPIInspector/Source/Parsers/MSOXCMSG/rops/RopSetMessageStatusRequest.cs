using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.8 RopSetMessageStatus ROP
    /// A class indicates the RopSetMessageStatus ROP request Buffer.
    /// </summary>
    public class RopSetMessageStatusRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the message for which the status will be changed.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// A flags structure that contains status flags to set on the message.
        /// </summary>
        public BlockT<MessageStatusFlag> MessageStatusFlags;

        /// <summary>
        /// A bitmask that specifies which bits in the MessageStatusFlags field are to be changed.
        /// </summary>
        public BlockT<uint> MessageStatusMask;

        /// <summary>
        /// Parse the RopSetMessageStatusRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            MessageId = Parse<MessageID>();
            MessageStatusFlags = ParseT<MessageStatusFlag>();
            MessageStatusMask = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSetMessageStatusRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(MessageId, "MessageId");
            AddChildBlockT(MessageStatusFlags, "MessageStatusFlags");
            AddChildBlockT(MessageStatusMask, "MessageStatusMask");
        }
    }
}
