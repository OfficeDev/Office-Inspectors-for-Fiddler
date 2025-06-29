using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4.3.23 recipient Element
    /// The Recipient element represents a Recipient object, which is a subobject of the Message object.
    /// </summary>
    public class Recipient : Block
    {
        /// <summary>
        /// The start marker of Recipient.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// The end marker of Recipient.
        /// </summary>
        public BlockT<Markers> EndMarker;

        /// <summary>
        /// Verify that a stream's current position contains a serialized Recipient.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized Recipient, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.StartRecip);
        }

        protected override void Parse()
        {
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.StartRecip)
            {
                PropList = Parse<PropList>();

                EndMarker = ParseT<Markers>();
                if (EndMarker != Markers.EndToRecip)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("Recipient");
            AddChildBlockT(StartMarker, "StartMarker");
            AddLabeledChild(PropList, "PropList");
            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
