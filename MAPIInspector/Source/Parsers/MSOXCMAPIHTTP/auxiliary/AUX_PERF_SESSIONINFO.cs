using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_SESSIONINFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.2 AUX_PERF_SESSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SESSIONINFO : Block
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
        /// Parse the AUX_PERF_SESSIONINFO structure.
        /// </summary>
        protected override void Parse()
        {
            SessionID = ParseT<ushort>();
            Reserved = ParseT<ushort>();
            SessionGuid = Parse<BlockGuid>();
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_SESSIONINFO";
            AddChildBlockT(SessionID, "SessionID");
            AddChildBlockT(Reserved, "Reserved");
            this.AddChildGuid(SessionGuid, "SessionGuid");
        }
    }
}