using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_DEFGC_SUCCESS Auxiliary Block Structure
    /// [MS-OXCRPC] 2.2.2.2 AUX_HEADER Structure
    /// [MS-OXCRPC] 2.2.2.2.8 AUX_PERF_DEFGC_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_DEFGC_SUCCESS : Block
    {
        /// <summary>
        /// The server identification number.
        /// </summary>
        public BlockT<ushort> ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public BlockT<ushort> SessionID;

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
        public BlockBytes Reserved; // 3 bytes

        /// <summary>
        /// Parse the AUX_PERF_DEFGC_SUCCESS structure.
        /// </summary>
        protected override void Parse()
        {
            ServerID = ParseT<ushort>();
            SessionID = ParseT<ushort>();
            TimeSinceRequest = ParseT<uint>();
            TimeToCompleteRequest = ParseT<uint>();
            RequestOperation = ParseT<byte>();
            Reserved = ParseBytes(3);
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_DEFGC_SUCCESS";
            AddChildBlockT(ServerID, "ServerID");
            AddChildBlockT(SessionID, "SessionID");
            AddChildBlockT(TimeSinceRequest, "TimeSinceRequest");
            AddChildBlockT(TimeToCompleteRequest, "TimeToCompleteRequest");
            AddChildBlockT(RequestOperation, "RequestOperation");
            AddChildBytes(Reserved, "Reserved");
        }
    }
}
