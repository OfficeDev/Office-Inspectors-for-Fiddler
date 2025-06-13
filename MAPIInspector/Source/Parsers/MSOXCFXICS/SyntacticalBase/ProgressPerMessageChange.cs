namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The ProgressPerMessageChange is used to parse ContentSync class.
    /// </summary>
    public class ProgressPerMessageChange : SyntacticalBase
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
        /// Initializes a new instance of the ProgressPerMessageChange class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ProgressPerMessageChange(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized ProgressPerMessageChange.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized ProgressPerMessageChange, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return ProgressPerMessage.Verify(stream) || MessageChange.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (ProgressPerMessage.Verify(stream))
            {
                this.ProgressPerMessage = new ProgressPerMessage(stream);
            }

            this.MessageChange = new MessageChange(stream);
        }
    }
}
