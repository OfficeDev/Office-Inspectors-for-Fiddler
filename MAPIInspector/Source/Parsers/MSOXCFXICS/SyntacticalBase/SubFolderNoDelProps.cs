namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// Contains a folderContentNoDelProps.
    /// </summary>
    public class SubFolderNoDelProps : SyntacticalBase
    {
        /// <summary>
        /// The start marker of SubFolder.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A folderContentNoDelProps value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContentNoDelProps FolderContentNoDelProps;

        /// <summary>
        /// The end marker of SubFolder.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the SubFolderNoDelProps class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public SubFolderNoDelProps(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized SubFolderNoDelProps.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized SubFolderNoDelProps, return true, else false.</returns>
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
                this.FolderContentNoDelProps = new FolderContentNoDelProps(stream);

                if (stream.ReadMarker() == Markers.EndFolder)
                {
                    this.EndMarker = Markers.EndFolder;
                }
                else
                {
                    throw new Exception("The SubFolderNoDelProps cannot be parsed successfully. The EndFolder Marker is missed.");
                }
            }
        }
    }
}
