using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    /// [MS-OXCRPC] 2.2.2.2 AUX_HEADER Structure
    /// [MS-OXCRPC] 2.2.2.2.20 AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONNECTION_INFO : Block
    {
        /// <summary>
        /// The GUID of the connection to the server.
        /// </summary>
        public BlockGuid ConnectionGUID;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ConnectionContextInfo field.
        /// </summary>
        public BlockT<ushort> OffsetConnectionContextInfo;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// The number of connection attempts.
        /// </summary>
        public BlockT<uint> ConnectionAttempts;

        /// <summary>
        /// A flag designating the mode of operation.
        /// </summary>
        public BlockT<ConnectionFlags> ConnectionFlags;

        /// <summary>
        /// A null-terminated Unicode string that contains opaque connection context information to be logged by the server.
        /// </summary>
        public BlockString ConnectionContextInfo;

        /// <summary>
        /// Parse the AUX_CLIENT_CONNECTION_INFO structure.
        /// </summary>
        protected override void Parse()
        {
            ConnectionGUID = Parse<BlockGuid>();
            OffsetConnectionContextInfo = ParseT<ushort>();
            Reserved = ParseT<ushort>();
            ConnectionAttempts = ParseT<uint>();
            ConnectionFlags = ParseT<ConnectionFlags>();

            if (OffsetConnectionContextInfo != 0)
            {
                ConnectionContextInfo = ParseStringW();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_CLIENT_CONNECTION_INFO";
            this.AddChildGuid(ConnectionGUID, "ConnectionGUID");
            AddChildBlockT(OffsetConnectionContextInfo, "OffsetConnectionContextInfo");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(ConnectionAttempts, "ConnectionAttempts");
            AddChildBlockT(ConnectionFlags, "ConnectionFlags");
            AddChildString(ConnectionContextInfo, "ConnectionContextInfo");
        }
    }
}
