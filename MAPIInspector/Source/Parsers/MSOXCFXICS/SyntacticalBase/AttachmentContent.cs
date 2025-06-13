namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The attachmentContent element contains the properties and the embedded message of an Attachment object. If present,
    /// </summary>

    public class AttachmentContent : SyntacticalBase
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
        /// An EmbeddedMessage value.
        /// </summary>
        public EmbeddedMessage EmbeddedMessage;

        /// <summary>
        /// Initializes a new instance of the AttachmentContent class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public AttachmentContent(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized attachmentContent.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized attachmentContent, return true, else false.</returns>
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

            if (EmbeddedMessage.Verify(stream))
            {
                this.EmbeddedMessage = new EmbeddedMessage(stream);
            }
        }
    }
}
