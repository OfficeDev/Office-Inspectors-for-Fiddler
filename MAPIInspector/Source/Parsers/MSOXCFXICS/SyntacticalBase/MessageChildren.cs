namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// The MessageChildren element represents children of the Message objects: Recipient and Attachment objects.
    /// </summary>
    public class MessageChildren : SyntacticalBase
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

        /// <summary>
        /// Initializes a new instance of the MessageChildren class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MessageChildren(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<Attachment> interAttachments = new List<Attachment>();
            List<Recipient> interRecipients = new List<Recipient>();

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
            {
                this.FxdelPropsBeforeRecipient = new MetaPropValue(stream);
            }

            if (Recipient.Verify(stream))
            {
                interRecipients = new List<Recipient>();

                while (Recipient.Verify(stream))
                {
                    interRecipients.Add(new Recipient(stream));
                }
            }

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
            {
                this.FxdelPropsBeforeAttachment = new MetaPropValue(stream);
            }

            while (Attachment.Verify(stream))
            {
                interAttachments.Add(new Attachment(stream));
            }

            this.Attachments = interAttachments.ToArray();
            this.Recipients = interRecipients.ToArray();
        }
    }
}
