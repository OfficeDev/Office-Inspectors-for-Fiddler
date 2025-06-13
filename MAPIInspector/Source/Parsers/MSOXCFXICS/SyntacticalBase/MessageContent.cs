namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MessageContent element represents the content of a message: its properties, the recipients, and the attachments.
    /// </summary>
    public class MessageContent : SyntacticalBase
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Represents children of the Message objects: Recipient and Attachment objects.
        /// </summary>
        public MessageChildren MessageChildren;

        /// <summary>
        /// Initializes a new instance of the MessageContent class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MessageContent(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageContent.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageContent, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix || PropList.Verify(stream));
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

            this.PropList = new PropList(stream);
            this.MessageChildren = new MessageChildren(stream);
        }
    }
}
