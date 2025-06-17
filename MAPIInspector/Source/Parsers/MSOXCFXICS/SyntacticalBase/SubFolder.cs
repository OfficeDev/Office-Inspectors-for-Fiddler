namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class SubFolder : Block
    {
        /// <summary>
        /// The start marker of SubFolder.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A folderContent value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContent FolderContent;

        /// <summary>
        /// The end marker of SubFolder.
        /// </summary>
        public BlockT<Markers> EndMarker;

        /// <summary>
        /// Verify that a stream's current position contains a serialized SubFolder.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized SubFolder, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.StartSubFld);
        }

        protected override void Parse()
        {
            StartMarker = BlockT<Markers>.Parse(parser);
            if (StartMarker.Data == Markers.StartSubFld)
            {
                FolderContent = Parse<FolderContent>(parser);
                EndMarker = BlockT<Markers>.Parse(parser);
                if (EndMarker.Data != Markers.EndFolder)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("SubFolder");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(FolderContent, "FolderContent");
            if (EndMarker != null) AddChild(EndMarker, $"EndMarker:{EndMarker.Data}");
        }
    }
}
