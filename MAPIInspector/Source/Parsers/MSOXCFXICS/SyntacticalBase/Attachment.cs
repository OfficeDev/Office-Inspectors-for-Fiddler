using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains an attachmentContent.
    /// </summary>
    public class Attachment : Block
    {
        /// <summary>
        /// The start marker of an attachment object.
        /// </summary>
        public BlockT<Markers> StartMarker;

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
        public BlockT<Markers> EndMarker;

        /// <summary>
        /// Verify that a stream's current position contains a serialized attachment.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized attachment, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.NewAttach);
        }

        protected override void Parse()
        {
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.NewAttach)
            {
                PidTagAttachNumber = Parse<FixedPropTypePropValue>();
                AttachmentContent = Parse<AttachmentContent>();

                EndMarker = ParseT<Markers>();
                if (EndMarker == Markers.EndAttach)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "Attachment";
            AddChildBlockT(StartMarker, "StartMarker");
            AddChild(PidTagAttachNumber, "PidTagAttachNumber");
            AddChild(AttachmentContent, "AttachmentContent");
            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
