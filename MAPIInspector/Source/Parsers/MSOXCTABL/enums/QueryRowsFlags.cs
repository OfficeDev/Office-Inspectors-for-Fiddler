using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum value of QueryRowsFlags
    /// </summary>
    [Flags]
    public enum QueryRowsFlags : byte
    {
        /// <summary>
        /// Advance the table cursor.
        /// </summary>
        Advance = 0x00,

        /// <summary>
        /// Do not advance the table cursor.
        /// </summary>
        NoAdvance = 0x01,

        /// <summary>
        /// Enable packed buffers for the response. 
        /// </summary>
        EnablePackedBuffers = 0x02
    }
}
