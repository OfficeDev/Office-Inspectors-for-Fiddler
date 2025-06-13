namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class SubFolder : SyntacticalBase
    {
        /// <summary>
        /// The start marker of SubFolder.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A folderContent value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContent FolderContent;

        /// <summary>
        /// The end marker of SubFolder.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the SubFolder class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public SubFolder(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized SubFolder.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized SubFolder, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartSubFld);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartSubFld)
            {
                this.StartMarker = Markers.StartSubFld;
                this.FolderContent = new FolderContent(stream);
                if (stream.ReadMarker() == Markers.EndFolder)
                {
                    this.EndMarker = Markers.EndFolder;
                }
                else
                {
                    throw new Exception("The SubFolder cannot be parsed successfully. The EndFolder Marker is missed.");
                }
            }
        }
    }
}
