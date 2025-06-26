using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// 2 bytes; restricted to 1 or 0.
    /// </summary>
    public class PtypBooleanShort : Block
    {
        /// <summary>
        /// 1 byte; restricted to 1 or 0.
        /// </summary>
        public BlockT<bool> Value;

        /// <summary>
        /// Parse the PtypBoolean structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseAs<short, bool>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data}";
        }
    }
}
