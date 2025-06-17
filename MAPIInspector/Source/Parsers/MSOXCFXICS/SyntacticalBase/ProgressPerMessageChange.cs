using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The ProgressPerMessageChange is used to parse ContentSync class.
    /// </summary>
    public class ProgressPerMessageChange : Block
    {
        /// <summary>
        /// A ProgressPerMessage value.
        /// </summary>
        public ProgressPerMessage ProgressPerMessage;

        /// <summary>
        /// A MessageChange value.
        /// </summary>
        public MessageChange MessageChange;

        /// <summary>
        /// Verify that a stream's current position contains a serialized ProgressPerMessageChange.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized ProgressPerMessageChange, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return ProgressPerMessage.Verify(parser) || MessageChange.Verify(parser);
        }

        protected override void Parse()
        {
            if (ProgressPerMessage.Verify(parser))
            {
                ProgressPerMessage = Parse<ProgressPerMessage>(parser);
            }

            MessageChange = Parse<MessageChange>(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("ProgressPerMessageChange");
            AddLabeledChild(ProgressPerMessage, "ProgressPerMessage");
            AddLabeledChild(MessageChange, "MessageChange");
        }
    }
}
