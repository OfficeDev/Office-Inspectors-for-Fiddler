using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.6 RopMoveCopyMessages ROP
    /// The RopMoveCopyMessages ROP ([MS-OXCROPS] section 2.2.4.6) moves or copies messages from a source folder to a destination folder.
    /// </summary>
    public class RopMoveCopyMessagesRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public BlockT<byte> SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public BlockT<byte> DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the MessageIds field.
        /// </summary>
        public BlockT<ushort> MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which messages to move or copy.
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public BlockT<bool> WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the operation is a copy or a move.
        /// </summary>
        public BlockT<bool> WantCopy;

        /// <summary>
        /// Parse the RopMoveCopyMessagesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            SourceHandleIndex = ParseT<byte>();
            DestHandleIndex = ParseT<byte>();
            MessageIdCount = ParseT<ushort>();
            var tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < MessageIdCount; i++)
            {
                tempMessageIDs.Add(Parse<MessageID>());
            }

            MessageIds = tempMessageIDs.ToArray();
            WantAsynchronous = ParseAs<byte, bool>();
            WantCopy = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopMoveCopyMessagesRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
            AddChildBlockT(MessageIdCount, "MessageIdCount");
            AddLabeledChildren(MessageIds, "MessageIds");
            AddChildBlockT(WantAsynchronous, "WantAsynchronous");
            AddChildBlockT(WantCopy, "WantCopy");
        }
    }
}