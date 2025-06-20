namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyMessages ROP Request Buffer.
    ///  2.2.3.1.1.3.1 RopFastTransferSourceCopyMessages ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceCopyMessagesRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public BlockT<ushort> MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies the messages to copy. 
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation. 
        /// </summary>
        public BlockT<CopyFlags_CopyMessages> CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public BlockT<SendOptions> SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyMessagesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            MessageIdCount = ParseT<ushort>();

            var messageIdList = new List<MessageID>();
            for (int i = 0; i < MessageIdCount.Data; i++)
            {
                messageIdList.Add(Parse<MessageID>());
            }

            MessageIds = messageIdList.ToArray();
            CopyFlags = ParseT<CopyFlags_CopyMessages>();
            SendOptions = ParseT<SendOptions>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferSourceCopyMessagesRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(MessageIdCount, "MessageIdCount");
            foreach (var messageId in MessageIds)
            {
                AddChild(messageId);
            }

            AddChildBlockT(CopyFlags, "CopyFlags");
            AddChildBlockT(SendOptions, "SendOptions");
        }
    }
}
