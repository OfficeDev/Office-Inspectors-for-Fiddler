using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_SERVERINFO Auxiliary Block Structure
    /// 2.2.2.2 AUX_HEADER Structure
    /// 2.2.2.2.5 AUX_PERF_SERVERINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SERVERINFO : Block
    {
        /// <summary>
        /// The client-assigned server identification number.
        /// </summary>
        public BlockT<ushort> ServerID;

        /// <summary>
        /// The server type assigned by client.
        /// </summary>
        public BlockT<ServerType> ServerType;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerDN field.
        /// </summary>
        public BlockT<ushort> ServerDNOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerName field.
        /// </summary>
        public BlockT<ushort> ServerNameOffset;

        /// <summary>
        /// A null-terminated Unicode string that contains the DN of the server.
        /// </summary>
        public BlockString ServerDN;

        /// <summary>
        /// A null-terminated Unicode string that contains the server name.
        /// </summary>
        public BlockString ServerName;

        /// <summary>
        /// Parse the AUX_PERF_SERVERINFO structure.
        /// </summary>
        protected override void Parse()
        {
            ServerID = ParseT<ushort>();
            ServerType = ParseT<ServerType>();
            ServerDNOffset = ParseT<ushort>();
            ServerNameOffset = ParseT<ushort>();

            if (ServerDNOffset != 0)
            {
                // TODO: Use the actual offset to parse string
                ServerDN = ParseStringW();
            }

            if (ServerNameOffset != 0)
            {
                // TODO: Use the actual offset to parse string
                ServerName = ParseStringW();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_SERVERINFO";
            AddChildBlockT(ServerID, "ServerID");
            AddChildBlockT(ServerType, "ServerType");
            AddChildBlockT(ServerDNOffset, "ServerDNOffset");
            AddChildBlockT(ServerNameOffset, "ServerNameOffset");
            AddChildString(ServerDN, "ServerDN");
            AddChildString(ServerName, "ServerName");
        }
    }
}