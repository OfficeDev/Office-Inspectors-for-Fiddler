namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// The folderContent element contains the content of a folder: its properties, messages, and subFolders.
    /// </summary>
    public class FolderContent : SyntacticalBase
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// Contains the properties of the Folder object, which are possibly affected by property filters.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// A MetaTagNewFXFolder property.
        /// </summary>
        public MetaPropValue MetaTagNewFXFolder;

        /// <summary>
        /// The folderMessages element contains the messages contained in a folder.
        /// </summary>
        public FolderMessages FolderMessages;

        /// <summary>
        /// A MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue MetaTagFXDelProp;

        /// <summary>
        /// The subFolders element contains subFolders of a folder.
        /// </summary>
        public SubFolder[] SubFolders;

        /// <summary>
        /// Initializes a new instance of the FolderContent class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderContent(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized folderContent.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized folderContent, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix || PropList.Verify(stream));
        }

        /// <summary>
        ///  Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

            this.PropList = new PropList(stream);

            if (!stream.IsEndOfStream)
            {
                List<SubFolder> interSubFolders = new List<SubFolder>();

                if (stream.VerifyMetaProperty(MetaProperties.MetaTagNewFXFolder))
                {
                    this.MetaTagNewFXFolder = new MetaPropValue(stream);
                }
                else
                {
                    this.FolderMessages = new FolderMessages(stream);
                }

                if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
                {
                    this.MetaTagFXDelProp = new MetaPropValue(stream);
                }

                if (!stream.IsEndOfStream)
                {
                    while (SubFolder.Verify(stream))
                    {
                        interSubFolders.Add(new SubFolder(stream));
                    }

                    this.SubFolders = interSubFolders.ToArray();
                }
            }
        }
    }
}
