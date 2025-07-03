using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that specifies flags control the behavior of RopFastTransferSourceCopy operations.
    /// 2.2.3.1.1.1.1 RopFastTransferSourceCopyTo ROP Request Buffer
    /// </summary>
    [Flags]
    public enum SendOptions : byte
    {
        /// <summary>
        /// This flag indicates whether string properties are output in Unicode or in the code page set on the current connection
        /// </summary>
        Unicode = 0x01,

        /// <summary>
        /// This flag indicates support for code page property types
        /// </summary>
        UseCpid = 0x02,

        /// <summary>
        /// This flag is the combination of the Unicode and UseCpid flags.
        /// </summary>
        ForUpload = 0x03,

        /// <summary>
        /// This flag indicates that the client supports recovery mode
        /// </summary>
        RecoverMode = 0x04,

        /// <summary>
        /// This flag indicates whether string properties are output in Unicode.
        /// </summary>
        ForceUnicode = 0x08,

        /// <summary>
        /// This flag MUST only be set for content synchronization download operations.
        /// </summary>
        PartialItem = 0x10,

        /// <summary>
        /// Reserved flag
        /// </summary>
        Reserved1 = 0x20,

        /// <summary>
        /// Reserved flag
        /// </summary>
        Reserved2 = 0x40,
    }
}
