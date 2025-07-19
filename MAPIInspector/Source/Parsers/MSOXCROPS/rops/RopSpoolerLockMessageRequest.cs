using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.5 RopSpoolerLockMessage
    /// A class indicates the RopSpoolerLockMessage ROP Request Buffer.
    /// </summary>
    public class RopSpoolerLockMessageRequest : Block
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
        /// An integer flag specifies a status to set on the message.
        /// </summary>
        public BlockT<LockState> LockState;

        /// <summary>
        /// Parse the RopSpoolerLockMessageRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            MessageId = Parse<MessageID>();
            LockState = ParseT<LockState>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSpoolerLockMessageRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(MessageId, "MessageId");
            AddChildBlockT(LockState, "LockState");
        }
    }
}
