namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaTagEcWaringMessage is used to parse MessageList class.
    /// </summary>
    public class MetaTagMessage : SyntacticalBase
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// MetaTagEcWaring indicates a MetaTagEcWaring property.
        /// </summary>
        public MetaPropValue MetaTagEcWaring;

        /// <summary>
        /// Message indicates a Message object.
        /// </summary>
        public Message Message;

        /// <summary>
        /// Initializes a new instance of the MetaTagMessage class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MetaTagMessage(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MetaTagEcWaringMessage.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MetaTagEcWaringMessage, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream
                && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix
                || stream.VerifyUInt32() == (uint)MetaProperties.MetaTagEcWarning
                || Message.Verify(stream));
        }

        /// <summary>
        /// Parse MetaTagEcWaringMessage from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagEcWarning))
            {
                this.MetaTagEcWaring = new MetaPropValue(stream);
            }

            if (Message.Verify(stream))
            {
                this.Message = new Message(stream);
            }
        }
    }
}
