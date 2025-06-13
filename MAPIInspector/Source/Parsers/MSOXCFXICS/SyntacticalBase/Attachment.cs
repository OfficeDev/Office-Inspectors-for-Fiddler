namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// Contains an attachmentContent.
    /// </summary>
    public class Attachment : SyntacticalBase
    {
        /// <summary>
        /// The  start marker of an attachment object.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PidTagAttachNumber property.
        /// </summary>
        public FixedPropTypePropValue PidTagAttachNumber;

        /// <summary>
        /// Attachment content.
        /// </summary>
        public AttachmentContent AttachmentContent;

        /// <summary>
        /// The end marker of an attachment object.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the Attachment class.
        /// </summary>
        /// <param name="stream">a FastTransferStream</param>
        public Attachment(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized attachment.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized attachment, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.NewAttach);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.NewAttach)
            {
                this.StartMarker = Markers.NewAttach;
                this.PidTagAttachNumber = new FixedPropTypePropValue(stream);
                this.AttachmentContent = new AttachmentContent(stream);

                if (stream.ReadMarker() == Markers.EndAttach)
                {
                    this.EndMarker = Markers.EndAttach;
                }
                else
                {
                    throw new Exception("The Attachment cannot be parsed successfully. The EndAttach Marker is missed.");
                }
            }
        }
    }
}
