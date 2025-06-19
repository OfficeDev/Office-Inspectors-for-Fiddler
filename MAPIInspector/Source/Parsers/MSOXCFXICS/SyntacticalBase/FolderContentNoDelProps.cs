namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The folderContentNoDelProps element contains the content of a folder: its properties, messages, and subFolders.
    /// </summary>
    public class FolderContentNoDelProps : Block
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

        protected override void Parse()
        {
            PropList = Parse<PropList>();

            if (!parser.Empty)
            {
                var interSubFolders = new List<SubFolderNoDelProps>();

                if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagNewFXFolder))
                {
                    MetaTagNewFXFolder = Parse<MetaPropValue>();
                }
                else
                {
                    FolderMessagesNoDelProps = Parse<FolderMessagesNoDelProps>();
                }

                if (!parser.Empty)
                {
                    while (SubFolderNoDelProps.Verify(parser))
                    {
                        interSubFolders.Add(Parse<SubFolderNoDelProps>());
                    }

                    SubFolderNoDelPropList = interSubFolders.ToArray();
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("FolderContentNoDelProps");
            AddLabeledChild(PropList, "PropList");
            AddLabeledChild(MetaTagNewFXFolder, "MetaTagNewFXFolder");
            AddLabeledChild(FolderMessagesNoDelProps, "FolderMessagesNoDelProps");
            AddLabeledChild(MetaTagFXDelProp, "MetaTagFXDelProp");
            AddLabeledChildren(SubFolderNoDelPropList, "SubFoldersNoDelProps");
        }
    }
}
