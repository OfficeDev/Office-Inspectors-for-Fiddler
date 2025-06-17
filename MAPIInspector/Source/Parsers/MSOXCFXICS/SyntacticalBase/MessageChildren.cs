namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The MessageChildren element represents children of the Message objects: Recipient and Attachment objects.
    /// </summary>
    public class MessageChildren : Block
    {
        /// <summary>
        /// A MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue FxdelPropsBeforeRecipient;

        /// <summary>
        /// A list of recipients.
        /// </summary>
        public Recipient[] Recipients;

        /// <summary>
        /// Another MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue FxdelPropsBeforeAttachment;

        /// <summary>
        /// A list of attachments.
        /// </summary>
        public Attachment[] Attachments;

        protected override void Parse()
        {
            var interAttachments = new List<Attachment>();
            var interRecipients = new List<Recipient>();

            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagFXDelProp))
            {
                FxdelPropsBeforeRecipient = Parse<MetaPropValue>(parser);
            }

            if (Recipient.Verify(parser))
            {
                interRecipients = new List<Recipient>();

                while (Recipient.Verify(parser))
                {
                    interRecipients.Add(Parse<Recipient>(parser));
                }
            }

            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagFXDelProp))
            {
                FxdelPropsBeforeAttachment = Parse<MetaPropValue>(parser);
            }

            while (Attachment.Verify(parser))
            {
                interAttachments.Add(Parse<Attachment>(parser));
            }

            Attachments = interAttachments.ToArray();
            Recipients = interRecipients.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("MessageChildren");
            AddLabeledChild("FxdelPropsBeforeRecipient", FxdelPropsBeforeRecipient);
            AddLabeledChildren("Recipients", Recipients);
            AddLabeledChild("FxdelPropsBeforeAttachment", FxdelPropsBeforeAttachment);
            AddLabeledChildren("Attachments", Attachments);
        }
    }
}
