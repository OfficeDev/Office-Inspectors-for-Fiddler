namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum type for flags specifies a status to set on a message.
    /// </summary>
    public enum LockState : byte
    {
        /// <summary>
        /// Mark the message as locked.
        /// </summary>
        IstLock = 0x00,

        /// <summary>
        /// Mark the message as unlocked.
        /// </summary>
        IstUnlock = 0x01,

        /// <summary>
        /// Mark the message as ready for processing by the server.
        /// </summary>
        IstFininshed = 0x02
    }
}
