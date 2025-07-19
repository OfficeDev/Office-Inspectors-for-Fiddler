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
                MetaTagDnPrefix = Parse<MetaPropValue>();
            }

            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.StartTopFld)
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
            Text = "TopFolder";
            AddChild(MetaTagDnPrefix, "MetaTagDnPrefix");
            AddChildBlockT(StartMarker, "StartMarker");
            AddChild(FolderContentNoDelProps, "FolderContentNoDelProps");
            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
