using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFOLD] 2.2.1.12 RopHardDeleteMessages ROP
    /// The RopHardDeleteMessages ROP ([MS-OXCROPS] section 2.2.4.12) is used to hard delete one or more messages from a folder.
    /// </summary>
    public class RopHardDeleteMessagesRequest : Block
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public BlockT<bool> WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the server sends a non-read receipt to the message sender when a message is deleted.
        /// </summary>
        public BlockT<bool> NotifyNonRead;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public BlockT<ushort> MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies the messages to be deleted.
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopHardDeleteMessagesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            WantAsynchronous = ParseAs<byte, bool>();
            NotifyNonRead = ParseAs<byte, bool>();
            MessageIdCount = ParseT<ushort>();
            var tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < MessageIdCount; i++)
            {
                tempMessageIDs.Add(Parse<MessageID>());
            }

            MessageIds = tempMessageIDs.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RopHardDeleteMessagesRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(WantAsynchronous, "WantAsynchronous");
            AddChildBlockT(NotifyNonRead, "NotifyNonRead");
            AddChildBlockT(MessageIdCount, "MessageIdCount");
            AddLabeledChildren(MessageIds, "MessageIds");
        }
    }
}
