using BlockParser;

namespace MAPIInspector.Parsers
{
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
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.StartEmbed)
            {
                MessageContent = Parse<MessageContent>();

                EndMarker = ParseT<Markers>();
                if (EndMarker != Markers.EndEmbed)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("EmbeddedMessage");
            AddChildBlockT(StartMarker, "StartMarker");
            AddLabeledChild(MessageContent, "MessageContent");
            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
