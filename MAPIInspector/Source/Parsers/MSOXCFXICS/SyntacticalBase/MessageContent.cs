using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MessageContent element represents the content of a message: its properties, the recipients, and the attachments.
    /// </summary>
    public class MessageContent : Block
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Represents children of the Message objects: Recipient and Attachment objects.
        /// </summary>
        public MessageChildren MessageChildren;

        protected override void Parse()
        {
            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagDnPrefix))
            {
                MetaTagDnPrefix = Parse<MetaPropValue>(parser);
            }

            PropList = Parse<PropList>(parser);
            MessageChildren = Parse<MessageChildren>(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("MessageContent");
            AddLabeledChild("MetaTagDnPrefix", MetaTagDnPrefix);
            AddLabeledChild("PropList", PropList);
            AddLabeledChild("MessageChildren", MessageChildren);
        }
    }
}
