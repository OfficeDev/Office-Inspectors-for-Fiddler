namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// Contain a MessageContent.
    /// </summary>
    public class EmbeddedMessage : Block
    {
        /// <summary>
        /// The start marker of the EmbeddedMessage.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A MessageContent value represents the content of a message: its properties, the recipients, and the attachments.
        /// </summary>
        public MessageContent MessageContent;

        /// <summary>
        /// The end marker of the EmbeddedMessage.
        /// </summary>
        public BlockT<Markers> EndMarker;

        /// <summary>
        /// Verify that a stream's current position contains a serialized EmbeddedMessage.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized EmbeddedMessage, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.StartEmbed);
        }

        protected override void Parse()
        {
            StartMarker = BlockT<Markers>.Parse(parser);
            if (StartMarker.Data == Markers.StartEmbed)
            {
                MessageContent = Parse<MessageContent>(parser);

                EndMarker = BlockT<Markers>.Parse(parser);
                if (EndMarker.Data != Markers.EndEmbed)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("EmbeddedMessage");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(MessageContent, "MessageContent");
            if (EndMarker != null) AddChild(EndMarker, $"EndMarker:{EndMarker.Data}");
        }
    }
}
