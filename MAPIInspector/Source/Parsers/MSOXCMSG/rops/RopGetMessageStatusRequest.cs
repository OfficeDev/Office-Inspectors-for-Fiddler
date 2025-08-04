using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.6.9 RopGetMessageStatus ROP
    /// A class indicates the RopGetMessageStatus ROP request Buffer.
    /// </summary>
    public class RopGetMessageStatusRequest : Block
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
        /// An identifier that specifies the message for which the status will be returned.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopGetMessageStatusRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            MessageId = Parse<MessageID>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetMessageStatusRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(MessageId, "MessageId");
        }
    }
}
