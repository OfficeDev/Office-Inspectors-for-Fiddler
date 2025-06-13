namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// The message element represents a Message object.
    /// </summary>
    public class Message : SyntacticalBase
    {
        /// <summary>
        /// The start marker of message.
        /// </summary>
        public Markers? StartMarker1;

        /// <summary>
        /// The start marker of message.
        /// </summary>
        public Markers? StartMarker2;

        /// <summary>
        /// A MessageContent value.Represents the content of a message: its properties, the recipients, and the attachments.
        /// </summary>
        public MessageContent Content;

        /// <summary>
        /// The end marker of message.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the Message class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public Message(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized message.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized message, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartMessage) ||
                stream.VerifyMarker(Markers.StartFAIMsg);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            Markers marker = stream.ReadMarker();

            if (marker == Markers.StartMessage || marker == Markers.StartFAIMsg)
            {
                if (marker == Markers.StartMessage)
                {
                    this.StartMarker1 = Markers.StartMessage;
                }
                else
                {
                    this.StartMarker2 = Markers.StartFAIMsg;
                }

                this.Content = new MessageContent(stream);

                if (stream.ReadMarker() == Markers.EndMessage)
                {
                    this.EndMarker = Markers.EndMessage;
                }
                else
                {
                    throw new Exception("The Message cannot be parsed successfully. The EndMessage Marker is missed.");
                }
            }
        }
    }
}
