namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The folderMessages element contains the messages contained in a folder.
    /// </summary>
    public class FolderMessages : Block
    {
        /// <summary>
        /// A list of MetaTagFxDelPropMessageList.
        /// </summary>
        public MetaTagFxDelPropMessageList[] MetaTagFxDelPropMessageLists;

        protected override void Parse()
        {
            int count = 0;
            var interMessageLists = new List<MetaTagFxDelPropMessageList>();

            while (!parser.Empty && count < 2)
            {
                if (MetaTagFxDelPropMessageList.Verify(parser))
                {
                    interMessageLists.Add(Parse<MetaTagFxDelPropMessageList>());
                }
                else
                {
                    break;
                }

                count++;
            }

            MetaTagFxDelPropMessageLists = interMessageLists.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("FolderMessages");
            if (MetaTagFxDelPropMessageLists != null)
            {
                foreach (var messageList in MetaTagFxDelPropMessageLists)
                {
                    AddChild(messageList);
                }
            }
        }
    }
}
