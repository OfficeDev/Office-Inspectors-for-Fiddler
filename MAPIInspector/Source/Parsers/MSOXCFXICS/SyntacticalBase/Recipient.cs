namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// The Recipient element represents a Recipient object, which is a subobject of the Message object.
    /// </summary>
    public class Recipient : SyntacticalBase
    {
        /// <summary>
        /// The start marker of Recipient.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// The end marker of Recipient.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the Recipient class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public Recipient(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized Recipient.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized Recipient, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartRecip);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartRecip)
            {
                this.StartMarker = Markers.StartRecip;
                this.PropList = new PropList(stream);

                if (stream.ReadMarker() == Markers.EndToRecip)
                {
                    this.EndMarker = Markers.EndToRecip;
                }
                else
                {
                    throw new Exception("The Recipient cannot be parsed successfully. The EndToRecip Marker is missed.");
                }
            }
        }
    }
}
