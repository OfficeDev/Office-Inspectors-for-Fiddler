using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaTagEcWaringMessage is used to parse MessageList class.
    /// </summary>
    public class MetaTagMessage : Block
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
        /// Verify that a stream's current position contains a serialized MetaTagEcWaringMessage.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized MetaTagEcWaringMessage, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            var offset = parser.Offset;
            var prefix = ParseT<MetaProperties>(parser);
            var warning = ParseT<MetaProperties>(parser);
            parser.Offset = offset;
            if (!prefix.Parsed || warning.Parsed) return false;

            return !parser.Empty
                && (prefix == MetaProperties.MetaTagDnPrefix
                || warning == MetaProperties.MetaTagEcWarning
                || Message.Verify(parser));
        }

        protected override void Parse()
        {
            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagDnPrefix))
            {
                MetaTagDnPrefix = Parse<MetaPropValue>();
            }

            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagEcWarning))
            {
                MetaTagEcWaring = Parse<MetaPropValue>();
            }

            if (Message.Verify(parser))
            {
                Message = Parse<Message>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("MetaTagMessage");
            if (MetaTagDnPrefix != null) AddLabeledChild(MetaTagDnPrefix, "MetaTagDnPrefix");
            if (MetaTagEcWaring != null) AddLabeledChild(MetaTagEcWaring, "MetaTagEcWaring");
            if (Message != null) AddLabeledChild(Message, "Message");
        }
    }
}
