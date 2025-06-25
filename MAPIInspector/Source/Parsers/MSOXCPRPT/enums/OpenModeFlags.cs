namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A flags structure that contains flags that control how the stream is opened.
    /// </summary>
    public enum OpenModeFlags : byte
    {
        /// <summary>
        /// Open the stream for read-only access.
        /// </summary>
        ReadOnly = 0x00,

        /// <summary>
        /// Open the stream for read/write access.
        /// </summary>
        ReadWrite = 0x01,

        /// <summary>
        /// Open a new stream. This mode will delete the current property value and open the stream for read/write access
        /// </summary>
        Create = 0x02
    }
}
