using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// [MS-OXCRPC] 2.2.2.2 AUX_HEADER Structure
    /// [MS-OXCRPC] 2.2.2.2.14 AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_FAILURE_V2 : Block
    {
        /// <summary>
        /// The process identification number.
        /// </summary>
        public BlockT<ushort> ProcessID;

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
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved1;

        /// <summary>
        /// The number of milliseconds since a request failure occurred.
        /// </summary>
        public BlockT<uint> TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the request failure took to complete.
        /// </summary>
        public BlockT<uint> TimeToFailRequest;

        /// <summary>
        /// The error code returned for the failed request.
        /// </summary>
        public BlockT<ErrorCodes> ResultCode;

        /// <summary>
        /// The client-defined operation that failed.
        /// </summary>
        public BlockT<byte> RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockBytes Reserved2;

        /// <summary>
        /// Parse the AUX_PERF_FAILURE_V2 structure.
        /// </summary>
        protected override void Parse()
        {
            ProcessID = ParseT<ushort>();
            ClientID = ParseT<ushort>();
            ServerID = ParseT<ushort>();
            SessionID = ParseT<ushort>();
            RequestID = ParseT<ushort>();
            Reserved1 = ParseT<ushort>();
            TimeSinceRequest = ParseT<uint>();
            TimeToFailRequest = ParseT<uint>();
            ResultCode = ParseT<ErrorCodes>();
            RequestOperation = ParseT<byte>();
            Reserved2 = ParseBytes(3);
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_FAILURE_V2";
            AddChildBlockT(ProcessID, "ProcessID");
            AddChildBlockT(ClientID, "ClientID");
            AddChildBlockT(ServerID, "ServerID");
            AddChildBlockT(SessionID, "SessionID");
            AddChildBlockT(RequestID, "RequestID");
            AddChildBlockT(Reserved1, "Reserved1");
            AddChildBlockT(TimeSinceRequest, "TimeSinceRequest");
            AddChildBlockT(TimeToFailRequest, "TimeToFailRequest");
            this.AddError(ResultCode, "ResultCode ");
            AddChildBlockT(RequestOperation, "RequestOperation");
            AddChildBytes(Reserved2, "Reserved2");
        }
    }
}
