using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// 4 bytes; a 32-bit integer. [MS-DTYP]: INT32
    /// </summary>
    public class PtypInteger32 : Block
    {
        /// <summary>
        /// 32-bit integer.
        /// </summary>
        public BlockT<int> Value;

        /// <summary>
        /// Parse the PtypInteger32 structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseT<int>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data}";
        }
    }
}
