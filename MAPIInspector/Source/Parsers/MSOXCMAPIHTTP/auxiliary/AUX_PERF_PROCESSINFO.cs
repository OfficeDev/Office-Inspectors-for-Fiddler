using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_PROCESSINFO Auxiliary Block Structure
    /// 2.2.2.2 AUX_HEADER Structure
    /// 2.2.2.2.6 AUX_PERF_PROCESSINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_PROCESSINFO : Block
    {
        /// <summary>
        /// The client-assigned process identification number.
        /// </summary>
        public BlockT<ushort> ProcessID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved1;

        /// <summary>
        /// The GUID representing the client process to associate with the process identification number in the ProcessID field.
        /// </summary>
        public BlockGuid ProcessGuid;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ProcessName field.
        /// </summary>
        public BlockT<ushort> ProcessNameOffset;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved2;

        /// <summary>
        /// A null-terminated Unicode string that contains the client process name.
        /// </summary>
        public BlockString ProcessName;

        /// <summary>
        /// Parse the AUX_PERF_PROCESSINFO structure.
        /// </summary>
        protected override void Parse()
        {
            ProcessID = ParseT<ushort>();
            Reserved1 = ParseT<ushort>();
            ProcessGuid = Parse<BlockGuid>();
            ProcessNameOffset = ParseT<ushort>();
            Reserved2 = ParseT<ushort>();

            if (ProcessNameOffset != 0)
            {
                // TODO: Use the actual offset to parse string
                ProcessName = ParseStringW();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_PROCESSINFO";
            AddChildBlockT(ProcessID, "ProcessID");
            AddChildBlockT(Reserved1, "Reserved1");
            this.AddChildGuid(ProcessGuid, "ProcessGuid");
            AddChildBlockT(ProcessNameOffset, "ProcessNameOffset");
            AddChildBlockT(Reserved2, "Reserved2");
            AddChildString(ProcessName, "ProcessName");
        }
    }
}