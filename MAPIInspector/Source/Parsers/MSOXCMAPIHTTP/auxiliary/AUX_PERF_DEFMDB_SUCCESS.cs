using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_DEFMDB_SUCCESS Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.7 AUX_PERF_DEFMDB_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_DEFMDB_SUCCESS : Block
    {
        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public BlockT<uint> TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public BlockT<uint> TimeToCompleteRequest;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public BlockT<ushort> RequestID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// Parse the AUX_PERF_DEFMDB_SUCCESS structure.
        /// </summary>
        protected override void Parse()
        {
            TimeSinceRequest = ParseT<uint>();
            TimeToCompleteRequest = ParseT<uint>();
            RequestID = ParseT<ushort>();
            Reserved = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            SetText("AUX_PERF_DEFMDB_SUCCESS");
            AddChildBlockT(TimeSinceRequest, "TimeSinceRequest");
            AddChildBlockT(TimeToCompleteRequest, "TimeToCompleteRequest");
            AddChildBlockT(RequestID, "RequestID");
            AddChildBlockT(Reserved, "Reserved");
        }
    }
}