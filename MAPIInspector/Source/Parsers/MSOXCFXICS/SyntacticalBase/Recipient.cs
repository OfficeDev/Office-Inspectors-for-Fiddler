namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
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
            StartMarker = BlockT<Markers>(parser);
            if (StartMarker.Data == Markers.StartRecip)
            {
                PropList = Parse<PropList>(parser);

                EndMarker = BlockT<Markers>(parser);
                if (EndMarker.Data != Markers.EndToRecip)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("Recipient");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(PropList, "PropList");
            if (EndMarker != null) AddChild(EndMarker, $"EndMarker:{EndMarker.Data}");
        }
    }
}
