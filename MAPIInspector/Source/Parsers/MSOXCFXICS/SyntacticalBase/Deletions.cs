using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The Deletions element contains information of messages that have been deleted expired or moved out of the sync scope.
    /// </summary>
    public class Deletions : Block
    {
        /// <summary>
        /// The start marker of Deletions.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Verify that a stream's current position contains a serialized Deletions.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized Deletions, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.IncrSyncDel);
        }

        protected override void Parse()
        {
            StartMarker = ParseT<Markers>(parser);
            if (StartMarker.Data == Markers.IncrSyncDel)
            {
                PropList = Parse<PropList>(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("Deletions");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(PropList, "PropList");
        }
    }
}
