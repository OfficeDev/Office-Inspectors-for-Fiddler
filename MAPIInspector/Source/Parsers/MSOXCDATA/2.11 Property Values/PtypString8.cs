using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
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
        /// [MS-OXCMAPIHTTP] 2.2.1.1 AddressBookPropertyValue Structure
        /// </summary>
        public BlockT<bool> HasValue;

        /// <summary>
        /// Bool value indicates if this property value is for address book.
        /// </summary>
        private readonly bool isAddressBook = false;

        /// <summary>
        /// Initializes a new instance of the PtypString8 class (parameterless constructor)
        /// </summary>
        public PtypString8()
        {
            isAddressBook = false;
        }

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        /// <param name="isAddressBook">Whether this is for address book parsing.</param>
        public PtypString8(bool isAddressBook = false)
        {
            this.isAddressBook = isAddressBook;
        }

        /// <summary>
        /// Initializes a new instance of the PtypString8 class
        /// </summary>
        /// <param name="count">The count of bytes to be read.</param>
        public PtypString8(int count) => Count = CreateBlock(count, 0, 0);

        /// <summary>
        /// Parse the PtypString8 structure.
        /// </summary>
        protected override void Parse()
        {
            if (isAddressBook)
            {
                // If this is an AddressBookPropertyValue, we need to check if HasValue is present
                HasValue = ParseAs<byte, bool>();
                if (HasValue) Value = ParseStringA(-1);
                return;
            }

            // PtypString8 doesn't use count fields - it's null-terminated
            Value = ParseStringA();
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
