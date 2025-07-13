using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
    /// </summary>
    public class PtypString : Block
    {
        // When used, this is a count of bytes. BlockStringW accepts a count of characters, so we need to convert it.
        public BlockT<int> Count;

        /// <summary>
        /// A string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
        /// </summary>
        public BlockString Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide = 0; // Default to no count field

        /// <summary>
        /// Initializes a new instance of the PtypString class
        /// </summary>
        public PtypString() { }

        /// <summary>
        /// Initializes a new instance of the PtypString class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypString type.</param>
        public PtypString(CountWideEnum wide) => countWide = wide;

        /// <summary>
        /// Initializes a new instance of the PtypString class
        /// </summary>
        /// <param name="count">The count of bytes to be read.</param>
        public PtypString(int count) => Count = CreateBlock(count, 0, 0);

        /// <summary>
        /// Parse the PtypString structure.
        /// </summary>
        protected override void Parse()
        {
            // If we have a countWide enum, we read a count field and use it.
            // Otherwise, if we were given a count, we use that directly.
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    Count = ParseAs<ushort, int>();
                    Value = ParseStringW(Count);
                    break;
                case CountWideEnum.fourBytes:
                    Count = ParseT<int>();
                    Value = ParseStringW(Count);
                    break;
                default:
                    Value = ParseStringW(-1);
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            Text = $"\"{Value.Text}\"";
            AddChild(Count, $"Count:{Count.Data} = 0x{Count.Data:X}");
            AddHeader($"cch:{Value.Data.Length} = 0x{Value.Data.Length:X}");
        }
    }
}
