namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
    /// </summary>
    public class PtypString : Block
    {
        // When used, this is a count of bytes. BlockStringW accepts a count of characters, so we need to convert it.
        private Block _count;
        public int Count = -1;

        /// <summary>
        /// A string of Unicode characters in UTF-16LE format encoding with terminating null character (0x0000).
        /// </summary>
        public BlockStringW Value;

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
        public PtypString(int count) => Count = count;

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
            // If Count is >= 0, we parse that many bytes, so we divide by 2 to get the number of characters.
            Value = ParseStringW(Count == -1 ? Count : Count / 2);
        }

        protected override void ParseBlocks()
        {
            Text = $"\"{Value.Text}\"";
            if (_count != null) AddChild(_count, $"Count:{Count}");
        }
    }
}
