using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
    /// 8 bytes; a 64-bit integer.[MS-DTYP]: LONGLONG.
    /// </summary>
    public class PtypInteger64 : Block
    {
        /// <summary>
        /// 64-bit integer.
        /// </summary>
        public BlockT<long> Value;

        /// <summary>
        /// Parse the PtypInteger64 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypInteger64 structure</param>
        protected override void Parse()
        {
            Value = ParseT<long>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data} = 0x{Value.Data:X}";
        }
    }
}
