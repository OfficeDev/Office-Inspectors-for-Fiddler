namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the MessageReadState structure.
    ///  2.2.13.3.1.1 MessageReadState Structure
    /// </summary>
    public class MessageReadState : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the size of the MessageId field.
        /// </summary>
        public ushort MessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the message to be marked as read or unread.
        /// </summary>
        public byte[] MessageId;

        /// <summary>
        /// A Boolean that specifies whether to mark the message as read or not.
        /// </summary>
        public bool MarkAsRead;

        /// <summary>
        /// Parse the MessageReadState structure.
        /// </summary>
        /// <param name="s">A stream containing MessageReadState structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.MessageIdSize = this.ReadUshort();
            this.MessageId = this.ReadBytes(this.MessageIdSize);
            this.MarkAsRead = this.ReadBoolean();
        }
    }
}
