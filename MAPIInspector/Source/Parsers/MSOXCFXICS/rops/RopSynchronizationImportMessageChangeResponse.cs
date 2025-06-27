using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSynchronizationImportMessageChange ROP Response Buffer.
    /// 2.2.3.2.4.2.2 RopSynchronizationImportMessageChange ROP Response Buffer
    /// </summary>
    public class RopSynchronizationImportMessageChangeResponse : Block
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
        /// An identifier.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageChangeResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                MessageId = Parse<MessageID>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationImportMessageChangeResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChild(MessageId, "MessageId");
        }
    }
}
