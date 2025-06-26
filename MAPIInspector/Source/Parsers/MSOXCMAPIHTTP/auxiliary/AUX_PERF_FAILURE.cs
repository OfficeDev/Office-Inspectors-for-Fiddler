using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the AUX_PERF_FAILURE Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.13   AUX_PERF_FAILURE Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_FAILURE : BaseStructure
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
        /// The number of milliseconds since a request failure occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the failed request took to complete.
        /// </summary>
        public uint TimeToFailRequest;

        /// <summary>
        /// The error code returned for the failed request. 
        /// </summary>
        public uint ResultCode;

        /// <summary>
        /// The client-defined operation that failed.
        /// </summary>
        public byte RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_FAILURE structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_FAILURE structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ClientID = ReadUshort();
            ServerID = ReadUshort();
            SessionID = ReadUshort();
            RequestID = ReadUshort();
            TimeSinceRequest = ReadUint();
            TimeToFailRequest = ReadUint();
            ResultCode = ReadUint();
            RequestOperation = ReadByte();
            Reserved = ReadBytes(3);
        }
    }
}