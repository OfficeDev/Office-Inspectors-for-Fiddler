using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
    /// Variable size; a COUNT field followed by that many bytes.
    /// </summary>
    public class PtypBinary : Block
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public BlockT<uint> Count;

        /// <summary>
        /// The binary value.
        /// </summary>
        public BlockBytes Value;

        /// <summary>
        /// [MS-OXCMAPIHTTP] 2.2.1.1 AddressBookPropertyValue Structure
        /// </summary>
        public BlockT<bool> HasValue;

        /// <summary>
        /// Whether to use 32-bit count (true) or 16-bit count (false).
        /// </summary>
        private bool usesFourByteCount;

        private readonly bool isAddressBook = false;

        /// <summary>
        /// Initializes a new instance of the PtypBinary class
        /// </summary>
        /// <param name="context">The parsing context that determines count field width.</param>
        /// <param name="isAddressBook">Whether this is for address book parsing.</param>
        public PtypBinary(PropertyCountContext context, bool isAddressBook = false)
        {
            // Determine count width based on context
            switch (context)
            {
                case PropertyCountContext.RopBuffers:
                    usesFourByteCount = false; // 16 bits wide
                    break;
                case PropertyCountContext.ExtendedRules:
                case PropertyCountContext.MapiHttp:
                case PropertyCountContext.AddressBook:
                    usesFourByteCount = true; // 32 bits wide
                    break;
                default:
                    usesFourByteCount = false; // Default to ROP buffer behavior
                    break;
            }
            this.isAddressBook = isAddressBook;
        }

        /// <summary>
        /// Parse the PtypBinary structure.
        /// </summary>
        protected override void Parse()
        {
            if (isAddressBook)
            {
                // If this is an AddressBookPropertyValue, we need to check if HasValue is present
                HasValue = ParseAs<byte, bool>();
                if (!HasValue) return;
            }

            if (!usesFourByteCount)
            {
                Count = ParseAs<ushort, uint>();
            }
            else
            {
                Count = ParseT<uint>();
            }

            Value = ParseBytes(Count);
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(HasValue, "HasValue");
            AddChildBlockT(Count, "Count");
            AddChildBytes(Value, "Value");
        }
    }
}
