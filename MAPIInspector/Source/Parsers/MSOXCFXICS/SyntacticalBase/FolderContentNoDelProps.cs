namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// The folderContentNoDelProps element contains the content of a folder: its properties, messages, and subFolders.
    /// </summary>
    public class FolderContentNoDelProps : SyntacticalBase
    {
        /// <summary>
        /// Contains the properties of the Folder object, which are possibly affected by property filters.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// A MetaTagNewFXFolder property.
        /// </summary>
        public MetaPropValue MetaTagNewFXFolder;

        /// <summary>
        /// The FolderMessagesNoDelProps element contains the messages contained in a folder.
        /// </summary>
        public FolderMessagesNoDelProps FolderMessagesNoDelProps;

        /// <summary>
        /// A MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue MetaTagFXDelProp;

        /// <summary>
        /// The subFolders element contains subFolders of a folder.
        /// </summary>
        public SubFolderNoDelProps[] SubFolderNoDelPropList;

        /// <summary>
        /// Initializes a new instance of the FolderContentNoDelProps class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderContentNoDelProps(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized folderContentNoDelProps.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized folderContentNoDelProps, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream && PropList.Verify(stream);
        }

        /// <summary>
        ///  Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.PropList = new PropList(stream);

            if (!stream.IsEndOfStream)
            {
                List<SubFolderNoDelProps> interSubFolders = new List<SubFolderNoDelProps>();

                if (stream.VerifyMetaProperty(MetaProperties.MetaTagNewFXFolder))
                {
                    this.MetaTagNewFXFolder = new MetaPropValue(stream);
                }
                else
                {
                    this.FolderMessagesNoDelProps = new FolderMessagesNoDelProps(stream);
                }

                if (!stream.IsEndOfStream)
                {
                    while (SubFolderNoDelProps.Verify(stream))
                    {
                        interSubFolders.Add(new SubFolderNoDelProps(stream));
                    }

                    this.SubFolderNoDelPropList = interSubFolders.ToArray();
                }
            }
        }
    }
}
