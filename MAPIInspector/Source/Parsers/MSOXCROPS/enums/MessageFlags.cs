using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum type for flags indicates the status of a message object.
    /// </summary>
    [Flags]
    public enum MessageFlags : uint
    {
        /// <summary>
        /// mfRead flag
        /// </summary>
        mfRead = 0x00000001,

        /// <summary>
        /// mfUnsent flag
        /// </summary>
        mfUnsent = 0x00000008,

        /// <summary>
        /// mfResend flag
        /// </summary>
        mfResend = 0x00000080
    }
}
