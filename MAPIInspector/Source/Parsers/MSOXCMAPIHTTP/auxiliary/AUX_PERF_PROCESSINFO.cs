using System;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_PROCESSINFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.6   AUX_PERF_PROCESSINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_PROCESSINFO : BaseStructure
    {
        /// <summary>
        /// The client-assigned process identification number.
        /// </summary>
        public ushort ProcessID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The GUID representing the client process to associate with the process identification number in the ProcessID field.
        /// </summary>
        public Guid ProcessGuid;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ProcessName field. 
        /// </summary>
        public ushort ProcessNameOffset;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// A null-terminated Unicode string that contains the client process name. 
        /// </summary>
        public MAPIString ProcessName;

        /// <summary>
        /// Parse the AUX_PERF_PROCESSINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_PROCESSINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ProcessID = ReadUshort();
            Reserved1 = ReadUshort();
            ProcessGuid = ReadGuid();
            ProcessNameOffset = ReadUshort();
            Reserved2 = ReadUshort();

            if (ProcessNameOffset != 0)
            {
                ProcessName = new MAPIString(Encoding.Unicode);
                ProcessName.Parse(s);
            }
        }
    }
}