using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4.3.11 messageChange Element
    /// The MessageChange element contains information for the changed messages.
    /// </summary>
    public class MessageChange : Block
    {
        /// <summary>
        /// A MessageChangeFull value.
        /// </summary>
        public MessageChangeFull MessageChangeFull;

        /// <summary>
        /// A MessageChangePartial value.
        /// </summary>
        public MessageChangePartial MesageChangePartial;

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageChange.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized MessageChange, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MessageChangeFull.Verify(parser) || MessageChangePartial.Verify(parser);
        }

        protected override void Parse()
        {
            if (MessageChangeFull.Verify(parser))
            {
                MessageChangeFull = Parse<MessageChangeFull>();
            }
            else
            {
                MesageChangePartial = Parse<MessageChangePartial>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("MessageChange");
            AddLabeledChild(MessageChangeFull, "MessageChangeFull");
            AddLabeledChild(MesageChangePartial, "MesageChangePartial");
        }
    }
}
