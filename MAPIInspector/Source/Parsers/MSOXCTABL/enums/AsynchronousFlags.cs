namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.1.4 Asynchronous Flags
    /// </summary>
    public enum AsynchronousFlags : byte
    {
        /// <summary>
        /// The server will perform the ROP asynchronously.
        /// </summary>
        TBL_SYNC = 0x00,

        /// <summary>
        /// The server will perform the operation synchronously
        /// </summary>
        TBL_ASYNC = 0x01
    }
}
