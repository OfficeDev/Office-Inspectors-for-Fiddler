namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCMSG] 2.2.3.1.1 RopOpenMessage ROP Request Buffer
    /// The enum value of OpenModeFlags that contains flags that control the access to the message.
    /// </summary>
    public enum OpenMessageModeFlags : byte
    {
        /// <summary>
        /// Message will be opened as read-only
        /// </summary>
        ReadOnly = 0x00,

        /// <summary>
        /// Message will be opened for both reading and writing
        /// </summary>
        ReadWrite = 0x01,

        /// <summary>
        /// Open for read/write if the user has write permissions for the folder, read-only if not.
        /// </summary>
        BestAccess = 0x03,

        /// <summary>
        /// Open a soft deleted Message object if available
        /// </summary>
        OpenSoftDeleted = 0x04
    }
}
