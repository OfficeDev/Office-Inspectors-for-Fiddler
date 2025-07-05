using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_SESSIONINFO_V2 Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.3 AUX_PERF_SESSIONINFO_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SESSIONINFO_V2 : Block
    {
        /// <summary>
        /// The session identification number.
        /// </summary>
        public BlockT<ushort> SessionID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// The GUID representing the client session to associate with the session identification number in the SessionID field.
        /// </summary>
        public BlockGuid SessionGuid;

        /// <summary>
        /// The connection identification number.
        /// </summary>
        public BlockT<uint> ConnectionID;

        /// <summary>
        /// Parse the AUX_PERF_SESSIONINFO_V2 structure.
        /// </summary>
        protected override void Parse()
        {
            SessionID = ParseT<ushort>();
            Reserved = ParseT<ushort>();
            SessionGuid = Parse<BlockGuid>();
            ConnectionID = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            SetText("AUX_PERF_SESSIONINFO_V2");
            AddChildBlockT(SessionID, "SessionID");
            AddChildBlockT(Reserved, "Reserved");
            this.AddChildGuid(SessionGuid, "SessionGuid");
            AddChildBlockT(ConnectionID, "ConnectionID");
        }
    }
}