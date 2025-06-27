using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.9   AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_MDB_SUCCESS : BaseStructure
    {
        /// <summary>
        /// The client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// Parse the AUX_PERF_MDB_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_MDB_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ClientID = ReadUshort();
            ServerID = ReadUshort();
            SessionID = ReadUshort();
            RequestID = ReadUshort();
            TimeSinceRequest = ReadUint();
            TimeToCompleteRequest = ReadUint();
        }
    }
}