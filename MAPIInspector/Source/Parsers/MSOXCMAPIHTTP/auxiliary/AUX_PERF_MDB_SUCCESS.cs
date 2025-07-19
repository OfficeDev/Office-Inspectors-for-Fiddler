using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.9 AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_MDB_SUCCESS : Block
    {
        /// <summary>
        /// The client identification number.
        /// </summary>
        public BlockT<ushort> ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public BlockT<ushort> ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public BlockT<ushort> SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public BlockT<ushort> RequestID;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public BlockT<uint> TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public BlockT<uint> TimeToCompleteRequest;

        /// <summary>
        /// Parse the AUX_PERF_MDB_SUCCESS structure.
        /// </summary>
        protected override void Parse()
        {
            ClientID = ParseT<ushort>();
            ServerID = ParseT<ushort>();
            SessionID = ParseT<ushort>();
            RequestID = ParseT<ushort>();
            TimeSinceRequest = ParseT<uint>();
            TimeToCompleteRequest = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_MDB_SUCCESS";
            AddChildBlockT(ClientID, "ClientID");
            AddChildBlockT(ServerID, "ServerID");
            AddChildBlockT(SessionID, "SessionID");
            AddChildBlockT(RequestID, "RequestID");
            AddChildBlockT(TimeSinceRequest, "TimeSinceRequest");
            AddChildBlockT(TimeToCompleteRequest, "TimeToCompleteRequest");
        }
    }
}