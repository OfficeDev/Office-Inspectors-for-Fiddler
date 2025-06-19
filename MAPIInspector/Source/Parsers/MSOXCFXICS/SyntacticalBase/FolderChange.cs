using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The FolderChange element contains a new or changed folder in the hierarchy sync.
    /// </summary>
    public class FolderChange : Block
    {
        /// <summary>
        /// The start marker of FolderChange.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Verify that a stream's current position contains a serialized FolderChange.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized FolderChange, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.IncrSyncChg);
        }

        protected override void Parse()
        {
            StartMarker = ParseT<Markers>();
            if (StartMarker.Data == Markers.IncrSyncChg)
            {
                PropList = Parse<PropList>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("FolderChange");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(PropList, "PropList");
        }
    }
}
