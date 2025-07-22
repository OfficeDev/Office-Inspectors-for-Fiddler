using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFXICS] 2.2.4.3.16 messageContent Element
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
                MetaTagDnPrefix = Parse<MetaPropValue>();
            }

            PropList = Parse<PropList>();
            MessageChildren = Parse<MessageChildren>();
        }

        protected override void ParseBlocks()
        {
            Text = "MessageContent";
            AddLabeledChild(MetaTagDnPrefix, "MetaTagDnPrefix");
            AddLabeledChild(PropList, "PropList");
            AddLabeledChild(MessageChildren, "MessageChildren");
        }
    }
}
