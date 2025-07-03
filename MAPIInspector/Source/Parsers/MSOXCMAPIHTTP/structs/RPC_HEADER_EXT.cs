using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The RPC_HEADER_EXT structure provides information about the payload. It is defined in section 2.2.2.1 of MS-OXCRPC.
    /// </summary>
    public class RPC_HEADER_EXT : BaseStructure
    {
        /// <summary>
        /// The version of the structure. This value MUST be set to 0x0000.
        /// </summary>
        public ushort Version;

        /// <summary>
        /// The flags that specify how data that follows this header MUST be interpreted.
        /// </summary>
        public RpcHeaderFlags Flags;

        /// <summary>
        /// The total length of the payload data that follows the RPC_HEADER_EXT structure.
        /// </summary>
        public ushort Size;

        /// <summary>
        /// The length of the payload data after it has been uncompressed.
        /// </summary>
        public ushort SizeActual;

        /// <summary>
        /// Parse the RPC_HEADER_EXT.
        /// </summary>
        /// <param name="s">A stream related to the RPC_HEADER_EXT.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Version = ReadUshort();
            Flags = (RpcHeaderFlags)ReadUshort();
            Size = ReadUshort();
            SizeActual = ReadUshort();
        }
    }
}