using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum type for flags that provide details about the state of the mailbox.
    /// </summary>
    [Flags]
    public enum ResponseFlags : byte
    {
        /// <summary>
        /// This bit MUST be set and MUST be ignored by the client
        /// </summary>
        Reserved = 0x01,

        /// <summary>
        /// The user has owner permission on the mailbox.
        /// </summary>
        OwnerRight = 0x02,

        /// <summary>
        /// The user has the right to send mail from the mailbox.
        /// </summary>
        SendAsRight = 0x04,

        /// <summary>
        /// The Out of Office (OOF) state is set on the mailbox
        /// </summary>
        OOF = 0x10
    }
}
