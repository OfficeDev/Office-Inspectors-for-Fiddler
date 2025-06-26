using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum type for flags that control the behavior of the RopLogon.
    /// </summary>
    [Flags]
    public enum LogonFlags : byte
    {
        /// <summary>
        /// This flag is set for logon to a private mailbox and is not set for logon to public folders.
        /// </summary>
        Private = 0x01,

        /// <summary>
        /// Undercover flag
        /// </summary>
        Undercover = 0x02,

        /// <summary>
        /// This flag is ignored by the server
        /// </summary>
        Ghosted = 0x04,

        /// <summary>
        /// This flag is ignored by the server.
        /// </summary>
        SpoolerProcess = 0x08
    }
}
