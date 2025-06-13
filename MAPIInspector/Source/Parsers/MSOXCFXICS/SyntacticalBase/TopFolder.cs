namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TopFolder : SyntacticalBase
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// The start marker of TopFolder.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A FolderContentNoDelProps value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContentNoDelProps FolderContentNoDelProps;

        /// <summary>
        /// The end marker of TopFolder.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the TopFolder class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TopFolder(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify a stream's current position contains a serialized TopFolder.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized TopFolder, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix || stream.VerifyMarker(Markers.StartTopFld);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

            if (stream.ReadMarker() == Markers.StartTopFld)
            {
                this.StartMarker = Markers.StartTopFld;
                this.FolderContentNoDelProps = new FolderContentNoDelProps(stream);

                if (stream.ReadMarker() == Markers.EndFolder)
                {
                    this.EndMarker = Markers.EndFolder;
                }
            }
        }
    }
}
