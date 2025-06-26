using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the AUX_PERF_GC_SUCCESS_V2 Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.12   AUX_PERF_GC_SUCCESS_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_GC_SUCCESS_V2 : BaseStructure
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
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// The client-defined operation that was successful.
        /// </summary>
        public byte RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_GC_SUCCESS_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_GC_SUCCESS_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ProcessID = ReadUshort();
            ClientID = ReadUshort();
            ServerID = ReadUshort();
            SessionID = ReadUshort();
            TimeSinceRequest = ReadUint();
            TimeToCompleteRequest = ReadUint();
            RequestOperation = ReadByte();
            Reserved = ReadBytes(3);
        }
    }
}