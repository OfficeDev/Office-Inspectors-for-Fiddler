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
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        private readonly bool isAddressBook = false;

        /// <summary>
        /// Initializes a new instance of the PtypBinary class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypBinary type.</param>
        public PtypBinary(CountWideEnum wide, bool isAddressBook)
        {
            countWide = wide;
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

            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    Count= ParseAs<ushort,uint>();
                    break;
                default:
                case CountWideEnum.fourBytes:
                    Count = ParseT<uint>();
                    break;
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
