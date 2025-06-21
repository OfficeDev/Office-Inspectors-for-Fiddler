namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represents the type of FastTransfer stream.
    /// TODO: Unused?
    /// </summary>
    public enum FastTransferStreamType
    {
        /// <summary>
        /// contentsSync type
        /// </summary>
        contentsSync = 1,

        /// <summary>
        /// hierarchySync type
        /// </summary>
        hierarchySync = 2,

        /// <summary>
        /// state type
        /// </summary>
        state = 3,

        /// <summary>
        /// folderContent type
        /// </summary>
        folderContent = 4,

        /// <summary>
        /// Message Content
        /// </summary>
        MessageContent = 5,

        /// <summary>
        /// attachment Content
        /// </summary>
        attachmentContent = 6,

        /// <summary>
        /// The MessageList element
        /// </summary>
        MessageList = 7,

        /// <summary>
        /// The TopFolder element
        /// </summary>
        TopFolder = 8
    }
}
