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
        /// 2.2.1.1 AddressBookPropertyValue Structure
        /// </summary>
        public BlockT<bool> HasValue;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide = 0; // Default to no count field

        private readonly bool isAddressBook = false;

        /// <summary>
        /// Initializes a new instance of the PtypString class
        /// </summary>
        public PtypString() { }

        /// <summary>
        /// Initializes a new instance of the PtypString class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypString type.</param>
        public PtypString(CountWideEnum wide, bool isAddressBook)
        {
            countWide = wide;
            this.isAddressBook = isAddressBook;
        }

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
            if (isAddressBook)
            {
                // If this is an AddressBookPropertyValue, we need to check if HasValue is present
                HasValue = ParseAs<byte, bool>();
                if (HasValue) Value = ParseStringW(-1);
                return;
            }

            // If we have a countWide enum, we read a count field and use it.
            // Otherwise, if we were given a count, we use that directly.
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    Count = ParseAs<ushort, int>();
                    Value = ParseStringW(Count/2);
                    break;
                case CountWideEnum.fourBytes:
                    Count = ParseT<int>();
                    Value = ParseStringW(Count/2);
                    break;
                default:
                    Value = ParseStringW(-1);
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            Text = $"\"{Value.Text}\"";
            AddChildBlockT(HasValue, "HasValue");
            AddChildBlockT(Count, "Count");
            AddHeader($"cch:{Value.Data.Length} = 0x{Value.Data.Length:X}");
        }
    }
}
