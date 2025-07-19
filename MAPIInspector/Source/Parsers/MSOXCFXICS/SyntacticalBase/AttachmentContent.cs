using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4.3.1 attachmentContent Element
    /// The attachmentContent element contains the properties and the embedded message of an Attachment object. If present,
    /// </summary>

    public class AttachmentContent : Block
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
        /// An EmbeddedMessage value.
        /// </summary>
        public EmbeddedMessage EmbeddedMessage;

        protected override void Parse()
        {
            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagDnPrefix))
            {
                MetaTagDnPrefix = Parse<MetaPropValue>();
            }

            PropList = Parse<PropList>();

            if (EmbeddedMessage.Verify(parser))
            {
                EmbeddedMessage = Parse<EmbeddedMessage>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AttachmentContent";
            AddLabeledChild(MetaTagDnPrefix, "MetaTagDnPrefix");
            AddLabeledChild(PropList, "PropList");
            AddLabeledChild(EmbeddedMessage, "EmbeddedMessage");
        }
    }
}
