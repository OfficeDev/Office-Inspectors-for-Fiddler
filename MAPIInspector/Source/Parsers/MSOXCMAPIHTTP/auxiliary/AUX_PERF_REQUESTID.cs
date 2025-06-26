using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the AUX_PERF_REQUESTID Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.1   AUX_PERF_REQUESTID Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_REQUESTID : BaseStructure
    {
        /// <summary>
        /// The session identification number. 
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// Parse the AUX_PERF_REQUESTID structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_REQUESTID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            SessionID = ReadUshort();
            RequestID = ReadUshort();
        }
    }
}