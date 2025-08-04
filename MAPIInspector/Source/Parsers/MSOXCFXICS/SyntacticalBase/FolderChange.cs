using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFXICS] 2.2.4.3.5 folderChange Element
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
            if (StartMarker == Markers.IncrSyncChg)
            {
                PropList = Parse<PropList>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "FolderChange";
            AddChildBlockT(StartMarker, "StartMarker");
            AddLabeledChild(PropList, "PropList");
        }
    }
}
