namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The FolderMessagesNoDelProps element contains the messages contained in a folder.
    /// </summary>
    public class FolderMessagesNoDelProps : Block
    {
        /// <summary>
        /// A list of MessageList.
        /// </summary>
        public MessageList[] MessageLists;

        protected override void Parse()
        {
            int count = 0;
            var interMessageLists = new List<MessageList>();

            while (!parser.Empty && count < 2)
            {
                if (MessageList.Verify(parser))
                {
                    interMessageLists.Add(Parse<MessageList>());
                }
                else
                {
                    break;
                }

                count++;
            }

            MessageLists = interMessageLists.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("FolderMessagesNoDelProps");
            AddLabeledChildren(MessageLists, "MessageLists");
        }
    }
}
