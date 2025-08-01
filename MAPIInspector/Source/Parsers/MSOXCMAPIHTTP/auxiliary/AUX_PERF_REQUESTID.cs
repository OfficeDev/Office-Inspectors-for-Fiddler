using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_REQUESTID Auxiliary Block Structure
    /// [MS-OXCRPC] 2.2.2.2 AUX_HEADER Structure
    /// [MS-OXCRPC] 2.2.2.2.1 AUX_PERF_REQUESTID Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_REQUESTID : Block
    {
        /// <summary>
        /// The session identification number.
        /// </summary>
        public BlockT<ushort> SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public BlockT<ushort> RequestID;

        /// <summary>
        /// Parse the AUX_PERF_REQUESTID structure.
        /// </summary>
        protected override void Parse()
        {
            SessionID = ParseT<ushort>();
            RequestID = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_REQUESTID";
            AddChildBlockT(SessionID, "SessionID");
            AddChildBlockT(RequestID, "RequestID");
        }
    }
}
