using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TopFolder : Block
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// The start marker of TopFolder.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A FolderContentNoDelProps value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContentNoDelProps FolderContentNoDelProps;

        /// <summary>
        /// The end marker of TopFolder.
        /// </summary>
        public BlockT<Markers> EndMarker;

        protected override void Parse()
        {
            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagDnPrefix))
            {
                MetaTagDnPrefix = Parse<MetaPropValue>(parser);
            }

            StartMarker = BlockT<Markers>.Parse(parser);
            if (StartMarker.Data == Markers.StartTopFld)
            {
                FolderContentNoDelProps = Parse<FolderContentNoDelProps>(parser);

                EndMarker = BlockT<Markers>.Parse(parser);
                if (EndMarker.Data != Markers.EndFolder)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TopFolder");
            AddChild(MetaTagDnPrefix, "MetaTagDnPrefix");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddChild(FolderContentNoDelProps, "FolderContentNoDelProps");
            if (EndMarker != null) AddChild(EndMarker, $"EndMarker:{EndMarker.Data}");
        }
    }
}
