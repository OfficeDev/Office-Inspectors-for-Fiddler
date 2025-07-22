namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCMSG] 2.2.3.12.1 RopOpenAttachment ROP Request Buffer
    /// The enum specifies the flags for opening attachments.
    /// </summary>
    public enum OpenAttachmentFlags : byte
    {
        /// <summary>
        /// Attachment will be opened as read-only
        /// </summary>
        ReadOnly = 0x00,

        /// <summary>
        /// Attachment will be opened for both reading and writing
        /// </summary>
        ReadWrite = 0x01,

        /// <summary>
        /// Attachment will be opened for read/write if the user has write permissions for the attachment; opened for read-only if not
        /// </summary>
        BestAccess = 0x03
    }
}
