using System;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_SESSIONINFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.2   AUX_PERF_SESSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SESSIONINFO : BaseStructure
    {
        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The GUID representing the client session to associate with the session identification number in the SessionID field.
        /// </summary>
        public Guid SessionGuid;

        /// <summary>
        /// Parse the AUX_PERF_SESSIONINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SESSIONINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            SessionID = ReadUshort();
            Reserved = ReadUshort();
            SessionGuid = ReadGuid();
        }
    }
}