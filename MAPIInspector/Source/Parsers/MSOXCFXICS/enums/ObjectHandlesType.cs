namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Object handles type.
    /// TODO: Unused?
    /// </summary>
    public enum ObjectHandlesType : byte
    {
        /// <summary>
        /// Handles for handle
        /// </summary>
        FolderHandles = 0x01,

        /// <summary>
        /// Message for handle
        /// </summary>
        MessageHandles = 0x02,

        /// <summary>
        /// Attachment for handle
        /// </summary>
        AttachmentHandles = 0x03,
    }
}
