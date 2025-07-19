using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_GC_SUCCESS Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.11 AUX_PERF_GC_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_GC_SUCCESS : Block
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
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved1;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public BlockT<uint> TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public BlockT<uint> TimeToCompleteRequest;

        /// <summary>
        /// The client-defined operation that was successful.
        /// </summary>
        public BlockT<byte> RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockBytes Reserved2; // 3 bytes

        /// <summary>
        /// Parse the AUX_PERF_GC_SUCCESS structure.
        /// </summary>
        protected override void Parse()
        {
            ClientID = ParseT<ushort>();
            ServerID = ParseT<ushort>();
            SessionID = ParseT<ushort>();
            Reserved1 = ParseT<ushort>();
            TimeSinceRequest = ParseT<uint>();
            TimeToCompleteRequest = ParseT<uint>();
            RequestOperation = ParseT<byte>();
            Reserved2 = ParseBytes(3);
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_GC_SUCCESS";
            AddChildBlockT(ClientID, "ClientID");
            AddChildBlockT(ServerID, "ServerID");
            AddChildBlockT(SessionID, "SessionID");
            AddChildBlockT(Reserved1, "Reserved1");
            AddChildBlockT(TimeSinceRequest, "TimeSinceRequest");
            AddChildBlockT(TimeToCompleteRequest, "TimeToCompleteRequest");
            AddChildBlockT(RequestOperation, "RequestOperation");
            AddChildBytes(Reserved2, "Reserved2");
        }
    }
}