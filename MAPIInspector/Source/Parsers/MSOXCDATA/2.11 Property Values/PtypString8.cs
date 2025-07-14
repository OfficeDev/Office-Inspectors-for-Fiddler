using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a string of multibyte characters in externally specified encoding with terminating null character (single 0 byte).
    /// </summary>
    public class PtypString8 : Block
    {
        // When used, this is a count of bytes. BlockStringA accepts a count of characters, which should be the same
        public BlockT<int> Count;

        /// <summary>
        /// A string of multibyte characters in externally specified encoding with terminating null character (single 0 byte).
        /// </summary>
        public BlockString Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide = 0; // Default to no count field

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        public PtypString8() { }

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypString8 type.</param>
        public PtypString8(CountWideEnum wide) => countWide = wide;

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        /// <param name="count">The count of bytes to be read.</param>
        public PtypString8(int count) => Count = CreateBlock(count, 0, 0);

        /// Parse the PtypString8 structure.
        /// </summary>
        protected override void Parse()
        {
            // If we have a countWide enum, we read a count field and use it.
            // Otherwise, if we were given a count, we use that directly.
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    Count = ParseAs<ushort, int>();
                    Value = ParseStringA(Count);
                    break;
                case CountWideEnum.fourBytes:
                    Count = ParseT<int>();
                    Value = ParseStringA(Count);
                    break;
                default:
                    Value = ParseStringA(-1);
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            Text = $"\"{Value.Text}\"";
            AddChildBlockT(Count, "Count");
            AddHeader($"cch:{Value.Data.Length} = 0x{Value.Data.Length:X}");
        }
    }
}
