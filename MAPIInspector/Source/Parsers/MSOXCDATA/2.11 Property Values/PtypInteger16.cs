using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
    /// 2 bytes; a 16-bit integer. [MS-DTYP]: INT16
    /// </summary>
    public class PtypInteger16 : Block
    {
        /// <summary>
        /// 16-bit integer.
        /// </summary>
        public BlockT<short> Value;

        /// <summary>
        /// Parse the PtypInteger16 structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseT<short>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data}";
        }
    }
}
