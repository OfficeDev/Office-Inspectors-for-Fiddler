namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// 2.2.6.10 RopSetReadFlags ROP
    /// A class indicates the RopSetReadFlags ROP request Buffer.
    /// </summary>
    public class RopSetReadFlagsRequest : Block
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public BlockT<bool> WantAsynchronous;

        /// <summary>
        /// A flags structure that contains flags that specify the flags to set.
        /// </summary>
        public BlockT<ReadFlags> ReadFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public BlockT<ushort> MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specify the messages that are to have their read flags changed.
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopSetReadFlagsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            WantAsynchronous = ParseAs<byte, bool>();
            ReadFlags = ParseT<ReadFlags>();
            MessageIdCount = ParseT<ushort>();
            var messageIDs = new List<MessageID>();

            for (int i = 0; i < MessageIdCount.Data; i++)
            {
                messageIDs.Add(Parse<MessageID>());
            }

            MessageIds = messageIDs.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetReadFlagsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(WantAsynchronous, "WantAsynchronous");
            AddChildBlockT(ReadFlags, "ReadFlags");
            AddChildBlockT(MessageIdCount, "MessageIdCount");
            AddLabeledChildren(MessageIds, "MessageIds");
        }
    }
}
