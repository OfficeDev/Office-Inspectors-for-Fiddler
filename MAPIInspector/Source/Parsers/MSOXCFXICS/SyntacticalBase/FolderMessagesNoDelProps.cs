namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// The FolderMessagesNoDelProps element contains the messages contained in a folder.
    /// </summary>
    public class FolderMessagesNoDelProps : SyntacticalBase
    {
        /// <summary>
        /// A list of MessageList.
        /// </summary>
        public MessageList[] MessageLists;

        /// <summary>
        /// Initializes a new instance of the FolderMessagesNoDelProps class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderMessagesNoDelProps(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized FolderMessagesNoDelProps
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized FolderMessagesNoDelProps, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream
                && MessageList.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            int count = 0;
            List<MessageList> interMessageLists = new List<MessageList>();

            while (!stream.IsEndOfStream && count < 2)
            {
                if (MessageList.Verify(stream))
                {
                    interMessageLists.Add(new MessageList(stream));
                }
                else
                {
                    break;
                }

                count++;
            }

            this.MessageLists = interMessageLists.ToArray();
        }
    }
}
