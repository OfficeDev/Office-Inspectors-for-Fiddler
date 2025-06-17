using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The readStateChanges element contains information of Message objects that had their read state changed
    /// </summary>
    public class ReadStateChanges : Block
    {
        /// <summary>
        /// The start marker of ReadStateChange.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Verify that a stream's current position contains a serialized ReadStateChange.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized ReadStateChange, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.IncrSyncRead);
        }

        protected override void Parse()
        {
            StartMarker = BlockT<Markers>(parser);
            if (StartMarker.Data == Markers.IncrSyncRead)
            {
                PropList = Parse<PropList>(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("ReadStateChanges");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(PropList, "PropList");
        }
    }
}
