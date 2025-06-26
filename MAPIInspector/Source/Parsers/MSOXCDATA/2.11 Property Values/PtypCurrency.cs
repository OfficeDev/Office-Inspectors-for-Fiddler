using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// 8 bytes; a 64-bit signed, scaled integer representation of a decimal currency value, with four places to the right of the decimal point. [MS-DTYP]: LONGLONG, [MS-OAUT]: CURRENCY
    /// </summary>
    public class PtypCurrency : Block
    {
        /// <summary>
        /// 64-bit signed, scaled integer representation of a decimal currency value
        /// </summary>
        public BlockT<long> Value;

        /// <summary>
        /// Parse the PtypCurrency structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypCurrency structure</param>
        protected override void Parse()
        {
            Value = ParseT<long>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data}";
        }
    }
}
