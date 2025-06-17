namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// The message element represents a Message object.
    /// </summary>
    public class Message : Block
    {
        /// <summary>
        /// The start marker of message.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A MessageContent value.Represents the content of a message: its properties, the recipients, and the attachments.
        /// </summary>
        public MessageContent Content;

        /// <summary>
        /// The end marker of message.
        /// </summary>
        public BlockT<Markers> EndMarker;

        /// <summary>
        /// Verify that a stream's current position contains a serialized message.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized message, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.StartMessage) ||
                MarkersHelper.VerifyMarker(parser, Markers.StartFAIMsg);
        }

        protected override void Parse()
        {
            StartMarker = BlockT<Markers>(parser);

            Content = Parse<MessageContent>(parser);

            EndMarker = BlockT<Markers>(parser);
            if (EndMarker.Data != Markers.EndMessage)
            {
                Parsed = false;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("Message");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(Content, "Content");
            if (EndMarker != null) AddChild(EndMarker, $"EndMarker:{EndMarker.Data}");
        }
    }
}
