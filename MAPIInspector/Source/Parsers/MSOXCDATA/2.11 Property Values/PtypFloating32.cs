namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.1 Property Data Types
    /// 4 bytes; a 32-bit floating point number. [MS-DTYP]: FLOAT
    /// </summary>
    public class PtypFloating32 : Block
    {
        /// <summary>
        /// 32-bit floating point number.
        /// </summary>
        public BlockT<float> Value;

        /// <summary>
        /// Parse the PtypFloating32 structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypFloating32 structure</param>
        protected override void Parse()
        {
            Value = ParseT<float>();
        }

        protected override void ParseBlocks()
        {
            Text = $"{Value.Data}";
        }

    }
}
