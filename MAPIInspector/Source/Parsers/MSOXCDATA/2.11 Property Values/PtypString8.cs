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
        private Block _count;
        public int Count = -1;

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
        public PtypString8() => countWide = 0; // Default to no count

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypString8 type.</param>
        public PtypString8(CountWideEnum wide) => countWide = wide;

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        /// <param name="count">The count of bytes to be read.</param>
        public PtypString8(int count) => Count = count;

        /// Parse the PtypString8 structure.
        /// </summary>
        protected override void Parse()
        {
            // If we have a countWide enum, we read a count field and use it.
            // Otherwise, if we were given a count, we use that directly.
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    _count = ParseT<ushort>();
                    Count = (_count as BlockT<ushort>).Data;
                    break;
                case CountWideEnum.fourBytes:
                    _count = ParseT<int>();
                    Count = (_count as BlockT<int>).Data;
                    break;
                default:
                    break;
            }

            // If Count is -1, we don't know the size, so we parse until the null terminator.
            // If Count is >= 0, we parse that many bytes, which is the same as the number of characters.
            Value = ParseStringA(Count == -1 ? Count : Count);
        }

        protected override void ParseBlocks()
        {
            Text = $"\"{Value.Text}\"";
            if (_count != null) AddChild(_count, $"Count:{Count}");
        }
    }
}
