using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
    /// 8 bytes; a 64-bit floating point number.
    /// </summary>
    public class PtypFloatingTime : Block
    {
        /// <summary>
        /// 64-bit floating point number.
        /// </summary>
        public BlockT<double> Value;

        /// <summary>
        /// Parse the PtypFloatingTime structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloatingTime structure</param>
        protected override void Parse()
        {
            Value = ParseT<double>();
        }

        protected override void ParseBlocks()
        {
            // TODO: Display as time
            Text = $"{Value.Data}";
        }
    }
}
