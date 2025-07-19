using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.1 RPC_HEADER_EXT Structure
    /// The RPC_HEADER_EXT structure provides information about the payload. It is defined in section 2.2.2.1 of MS-OXCRPC.
    /// </summary>
    public class RPC_HEADER_EXT : Block
    {
        /// <summary>
        /// The version of the structure. This value MUST be set to 0x0000.
        /// </summary>
        public BlockT<ushort> Version;

        /// <summary>
        /// The flags that specify how data that follows this header MUST be interpreted.
        /// </summary>
        public BlockT<RpcHeaderFlags> Flags;

        /// <summary>
        /// The total length of the payload data that follows the RPC_HEADER_EXT structure.
        /// </summary>
        public BlockT<ushort> _Size;

        /// <summary>
        /// The length of the payload data after it has been uncompressed.
        /// </summary>
        public BlockT<ushort> SizeActual;

        /// <summary>
        /// Parse the RPC_HEADER_EXT.
        /// </summary>
        protected override void Parse()
        {
            Version = ParseT<ushort>();
            Flags = ParseT<RpcHeaderFlags>();
            _Size = ParseT<ushort>();
            SizeActual = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            Text = "RPC_HEADER_EXT";
            AddChildBlockT(Version, "Version");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(_Size, "Size");
            AddChildBlockT(SizeActual, "SizeActual");
        }
    }
}