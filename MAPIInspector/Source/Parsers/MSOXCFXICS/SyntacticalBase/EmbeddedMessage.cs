namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// Contain a MessageContent.
    /// </summary>
    public class EmbeddedMessage : SyntacticalBase
    {
        /// <summary>
        /// The start marker of the EmbeddedMessage.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A MessageContent value represents the content of a message: its properties, the recipients, and the attachments.
        /// </summary>
        public MessageContent MessageContent;

        /// <summary>
        /// The end marker of the EmbeddedMessage.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the EmbeddedMessage class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public EmbeddedMessage(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized EmbeddedMessage.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized EmbeddedMessage, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartEmbed);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartEmbed)
            {
                this.StartMarker = Markers.NewAttach;
                this.MessageContent = new MessageContent(stream);

                if (stream.ReadMarker() == Markers.EndEmbed)
                {
                    this.EndMarker = Markers.EndEmbed;
                }
                else
                {
                    throw new Exception("The EmbeddedMessage cannot be parsed successfully. The EndEmbed Marker is missed.");
                }
            }
        }
    }
}
