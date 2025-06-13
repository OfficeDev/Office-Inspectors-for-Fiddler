namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MessageChange element contains information for the changed messages.
    /// </summary>
    public class MessageChange : SyntacticalBase
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
        /// Initializes a new instance of the MessageChange class.
        /// </summary>
        /// <param name="stream">A FastTransferStream object.</param>
        public MessageChange(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageChange.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageChange, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return MessageChangeFull.Verify(stream) || MessageChangePartial.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MessageChangeFull.Verify(stream))
            {
                this.MessageChangeFull = new MessageChangeFull(stream);
            }
            else
            {
                this.MesageChangePartial = new MessageChangePartial(stream);
            }
        }
    }
}
