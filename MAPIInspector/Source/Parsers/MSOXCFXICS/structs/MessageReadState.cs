namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    ///  A class indicates the MessageReadState structure.
    ///  2.2.13.3.1.1 MessageReadState Structure
    /// </summary>
    public class MessageReadState : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the size of the MessageId field.
        /// </summary>
        public BlockT<ushort> MessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the message to be marked as read or unread.
        /// </summary>
        public BlockBytes MessageId;

        /// <summary>
        /// A Boolean that specifies whether to mark the message as read or not.
        /// </summary>
        public BlockT<bool> MarkAsRead;

        /// <summary>
        /// Parse the MessageReadState structure.
        /// </summary>
        protected override void Parse()
        {
            MessageIdSize = ParseT<ushort>();
            MessageId = ParseBytes(MessageIdSize.Data);
            MarkAsRead = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            SetText("MessageReadState");
            AddChild(MessageIdSize, "MessageIdSize");
            AddChild(MessageId, "MessageId");
            AddChild(MarkAsRead, "MarkAsRead");
        }
    }
}