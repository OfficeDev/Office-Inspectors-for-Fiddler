namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The folderContent element contains the content of a folder: its properties, messages, and subFolders.
    /// </summary>
    public class FolderContent : Block
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

        protected override void Parse()
        {
            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagDnPrefix))
            {
                MetaTagDnPrefix = Parse<MetaPropValue>(parser);
            }

            PropList = Parse<PropList>(parser);

            if (!parser.Empty)
            {
                var interSubFolders = new List<SubFolder>();

                if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagNewFXFolder))
                {
                    MetaTagNewFXFolder = Parse<MetaPropValue>(parser);
                }
                else
                {
                    FolderMessages = Parse<FolderMessages>(parser);
                }

                if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagFXDelProp))
                {
                    MetaTagFXDelProp = Parse<MetaPropValue>(parser);
                }

                if (!parser.Empty)
                {
                    while (SubFolder.Verify(parser))
                    {
                        interSubFolders.Add(Parse<SubFolder>(parser));
                    }

                    SubFolders = interSubFolders.ToArray();
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("FolderContent");
            AddLabeledChild("MetaTagDnPrefix", MetaTagDnPrefix);
            AddLabeledChild("PropList", PropList);
            AddLabeledChild("MetaTagNewFXFolder", MetaTagNewFXFolder);
            AddLabeledChild("FolderMessages", FolderMessages);
            AddLabeledChild("MetaTagFXDelProp", MetaTagFXDelProp);
            if (SubFolders != null && SubFolders.Length > 0)
            {
                foreach (var subFolder in SubFolders)
                {
                    AddLabeledChild("SubFolder", subFolder);
                }
            }
        }
    }
}
