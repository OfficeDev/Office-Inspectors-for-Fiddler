using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum flags that specify how data that follows this header MUST be interpreted. It is defined in section 2.2.2.1 of MS-OXCRPC.
    /// </summary>
    [Flags]
    public enum RpcHeaderFlags : ushort
    {
        /// <summary>
        /// The data that follows the RPC_HEADER_EXT structure is compressed.
        /// </summary>
        Compressed = 0x0001,

        /// <summary>
        /// The data following the RPC_HEADER_EXT structure has been obfuscated.
        /// </summary>
        XorMagic = 0x0002,

        /// <summary>
        /// No other RPC_HEADER_EXT structure follows the data of the current RPC_HEADER_EXT structure.
        /// </summary>
        Last = 0x0004
    }
}
