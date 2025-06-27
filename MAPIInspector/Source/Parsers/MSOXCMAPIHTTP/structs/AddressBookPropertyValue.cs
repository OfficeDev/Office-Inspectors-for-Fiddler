using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the AddressBookPropertyValue structure.
    ///  2.2.1 Common Data Types
    ///  2.2.1.1 AddressBookPropertyValue Structure
    /// </summary>
    public class AddressBookPropertyValue : Block
    {
        /// <summary>
        /// An unsigned integer when the PropertyType is known to be either PtypString, PtypString8, PtypBinary or PtypMultiple ([MS-OXCDATA] section 2.11.1). 
        /// </summary>
        public BlockT<bool> HasValue;

        /// <summary>
        /// A PropertyValue structure, unless HasValue is present with a value of FALSE (0x00).
        /// </summary>
        public Block _PropertyValue;

        /// <summary>
        /// A propertyDataType is used to initialized the AddressBookPropertyValue structure
        /// </summary>
        private PropertyDataType propertyDataType;

        /// <summary>
        /// A CountWideEnum is used to initialized the AddressBookPropertyValue structure
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Source property tag information
        /// </summary>
        public AnnotatedComment PropertyTag;

        /// <summary>
        /// Initializes a new instance of the AddressBookPropertyValue class.
        /// </summary>
        /// <param name="propertyDataType">The PropertyDataType for this structure</param>
        /// <param name="ptypMultiCountSize">The CountWideEnum for this structure</param>
        public AddressBookPropertyValue(PropertyDataType _propertyDataType, CountWideEnum ptypMultiCountSize = CountWideEnum.fourBytes)
        {
            propertyDataType = _propertyDataType;
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the AddressBookPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            bool hasValue = (propertyDataType == PropertyDataType.PtypString) || (propertyDataType == PropertyDataType.PtypString8) ||
                            (propertyDataType == PropertyDataType.PtypBinary) || (propertyDataType == PropertyDataType.PtypMultipleInteger16) ||
                            (propertyDataType == PropertyDataType.PtypMultipleInteger32) || (propertyDataType == PropertyDataType.PtypMultipleFloating32) ||
                            (propertyDataType == PropertyDataType.PtypMultipleFloating64) || (propertyDataType == PropertyDataType.PtypMultipleCurrency) ||
                            (propertyDataType == PropertyDataType.PtypMultipleFloatingTime) || (propertyDataType == PropertyDataType.PtypMultipleInteger64) ||
                            (propertyDataType == PropertyDataType.PtypMultipleString) || (propertyDataType == PropertyDataType.PtypMultipleString8) ||
                            (propertyDataType == PropertyDataType.PtypMultipleTime) || (propertyDataType == PropertyDataType.PtypMultipleGuid) ||
                            (propertyDataType == PropertyDataType.PtypMultipleBinary);

            if (hasValue)
            {
                HasValue = ParseAs<byte, bool>();
            }

            if (HasValue == null || HasValue)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(propertyDataType, parser, countWide);
            }
        }

        protected override void ParseBlocks()
        {
            if (HasValue != null && HasValue)
            {
                AddChildBlockT(HasValue, "HasValue");
                if (_PropertyValue != null)
                {
                    AddChild(_PropertyValue, $"PropertyValue:{_PropertyValue.Text}");
                }
                else
                {
                    SetText("PropertyValue is null");
                }
            }
        }
    }
}