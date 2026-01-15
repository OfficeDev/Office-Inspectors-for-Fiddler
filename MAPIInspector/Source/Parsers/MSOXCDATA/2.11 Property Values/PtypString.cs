using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
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
        /// [MS-OXCMAPIHTTP] 2.2.1.1 AddressBookPropertyValue Structure
        /// </summary>
        public BlockT<bool> HasValue;

        /// <summary>
        /// Bool value indicates if this property value is for address book.
        /// </summary>
        private readonly bool isAddressBook = false;

        /// <summary>
        /// Initializes a new instance of the PtypString class (parameterless constructor)
        /// </summary>
        public PtypString()
        {
            isAddressBook = false;
        }

        /// <summary>
        /// Initializes a new instance of the PtypString class
        /// </summary>
        /// <param name="isAddressBook">Whether this is for address book parsing.</param>
        public PtypString(bool isAddressBook = false)
        {
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

            // PtypString doesn't use count fields - it's null-terminated
            Value = ParseStringW();
        }

        protected override void ParseBlocks()
        {
            Text = $"\"{Value.Text}\"";
            AddChildBlockT(HasValue, "HasValue");
            AddChildBlockT(Count, "Count");
            AddHeader($"cch: {Value.Data.Length} = 0x{Value.Data.Length:X}");
        }
    }
}
