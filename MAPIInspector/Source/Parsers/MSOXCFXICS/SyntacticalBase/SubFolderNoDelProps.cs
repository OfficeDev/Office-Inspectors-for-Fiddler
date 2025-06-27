using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContentNoDelProps.
    /// </summary>
    public class SubFolderNoDelProps : Block
    {
        /// <summary>
        /// The start marker of SubFolder.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A folderContentNoDelProps value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContentNoDelProps FolderContentNoDelProps;

        /// <summary>
        /// The end marker of SubFolder.
        /// </summary>
        public BlockT<Markers> EndMarker;

        /// <summary>
        /// Verify that a stream's current position contains a serialized SubFolderNoDelProps.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized SubFolderNoDelProps, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.StartSubFld);
        }

        protected override void Parse()
        {
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.StartSubFld)
            {
                FolderContentNoDelProps = Parse<FolderContentNoDelProps>();

                EndMarker = ParseT<Markers>();
                if (EndMarker != Markers.EndFolder)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("SubFolderNoDelProps");
            AddChildBlockT(StartMarker, "StartMarker");
            if (FolderContentNoDelProps != null) AddChild(FolderContentNoDelProps, "FolderContentNoDelProps");
            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
