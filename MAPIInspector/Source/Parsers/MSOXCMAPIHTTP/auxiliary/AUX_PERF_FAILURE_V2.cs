using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.14   AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_FAILURE_V2 : BaseStructure
    {
        /// <summary>
        /// The process identification number.
        /// </summary>
        public ushort ProcessID;

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
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The number of milliseconds since a request failure occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the request failure took to complete.
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
        public byte[] Reserved2;

        /// <summary>
        /// Parse the AUX_PERF_FAILURE_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_FAILURE_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ProcessID = ReadUshort();
            ClientID = ReadUshort();
            ServerID = ReadUshort();
            SessionID = ReadUshort();
            RequestID = ReadUshort();
            Reserved1 = ReadUshort();
            TimeSinceRequest = ReadUint();
            TimeToFailRequest = ReadUint();
            ResultCode = ReadUint();
            RequestOperation = ReadByte();
            Reserved2 = ReadBytes(3);
        }
    }
}