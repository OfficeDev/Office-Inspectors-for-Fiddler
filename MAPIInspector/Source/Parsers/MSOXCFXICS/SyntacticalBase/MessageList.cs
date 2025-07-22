using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFXICS] 2.2.4.3.17 messageList Element
    /// The MessageList element contains a list of messages, which is determined by the scope of the operation.
    /// </summary>
    public class MessageList : Block
    {
        /// <summary>
        /// A list of MetaTagMessage objects.
        /// </summary>
        public MetaTagMessage[] MetaTagMessages;


        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageList.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized MessageList, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MetaTagMessage.Verify(parser);
        }

        protected override void Parse()
        {
            var interMessageList = new List<MetaTagMessage>();

            while (Verify(parser))
            {
                interMessageList.Add(Parse<MetaTagMessage>());
            }

            MetaTagMessages = interMessageList.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "MessageList";
            if (MetaTagMessages != null)
            {
                foreach (var message in MetaTagMessages)
                {
                    AddChild(message);
                }
            }
        }
    }
}
