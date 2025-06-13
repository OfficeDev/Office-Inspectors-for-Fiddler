namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// The MessageList element contains a list of messages, which is determined by the scope of the operation.
    /// </summary>
    public class MessageList : SyntacticalBase
    {
        /// <summary>
        /// A list of MetaTagMessage objects.
        /// </summary>
        public MetaTagMessage[] MetaTagMessages;

        /// <summary>
        /// Initializes a new instance of the MessageList class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MessageList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageList.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return MetaTagMessage.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<MetaTagMessage> interMessageList = new List<MetaTagMessage>();

            while (Verify(stream))
            {
                interMessageList.Add(new MetaTagMessage(stream));
            }

            this.MetaTagMessages = interMessageList.ToArray();
        }
    }
}
