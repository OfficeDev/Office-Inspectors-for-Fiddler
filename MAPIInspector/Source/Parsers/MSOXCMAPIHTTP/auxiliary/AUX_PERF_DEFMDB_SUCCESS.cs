namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the AUX_PERF_DEFMDB_SUCCESS Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.7   AUX_PERF_DEFMDB_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_DEFMDB_SUCCESS : BaseStructure
    {
        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// Parse the AUX_PERF_DEFMDB_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_DEFMDB_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            TimeSinceRequest = ReadUint();
            TimeToCompleteRequest = ReadUint();
            RequestID = ReadUshort();
            Reserved = ReadUshort();
        }
    }
}