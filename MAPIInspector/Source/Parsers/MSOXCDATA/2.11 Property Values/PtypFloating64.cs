using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// 8 bytes; a 64-bit floating point number. [MS-DTYP]: DOUBLE
    /// </summary>
    public class PtypFloating64 : Block
    {
        /// <summary>
        /// 64-bit floating point number.
        /// </summary>
        public BlockT<double> Value;

        /// <summary>
        /// Parse the PtypFloating64 structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseT<double>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data}";
        }

    }
}
