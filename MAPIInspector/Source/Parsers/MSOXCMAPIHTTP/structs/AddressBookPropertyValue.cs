using BlockParser;

namespace MAPIInspector.Parsers
{
    // There is one variation in the width of count fields.In the context of ROP buffers,
    // such as the RopGetPropertiesSpecific ROP([MS-OXCROPS] section 2.2.8.3), byte counts
    // for PtypBinary property values are 16 bits wide and value counts for all PtypMultiple
    // property values are 32 bits wide.However, in the context of extended rules, as
    // specified in [MS - OXORULE] section 2.2.4, and in the context of the MAPI extensions
    // for HTTP, as specified in [MS - OXCMAPIHTTP] section 2.2.5, byte counts for PtypBinary
    // property values and value counts for PtypMultiple property values are 32 bits wide.
    // Such count fields have a width designation of COUNT, as specified in section 2.11.1.1,
    // rather than an explicit width, as throughout section 2.11.

    /// <summary>
    /// A class indicates the AddressBookPropertyValue structure.
    /// [MS-OXCMAPIHTTP] 2.2.1 Common Data Types
    /// [MS-OXCMAPIHTTP] 2.2.1.1 AddressBookPropertyValue Structure
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
        /// Initializes a new instance of the AddressBookPropertyValue class.
        /// </summary>
        /// <param name="_propertyDataType">The PropertyDataType value</param>
        public AddressBookPropertyValue(PropertyDataType _propertyDataType)
        {
            propertyDataType = _propertyDataType;
        }

        /// <summary>
        /// Parse the AddressBookPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            bool hasHasValue = (propertyDataType == PropertyDataType.PtypString) || (propertyDataType == PropertyDataType.PtypString8) ||
                            (propertyDataType == PropertyDataType.PtypBinary) || (propertyDataType == PropertyDataType.PtypMultipleInteger16) ||
                            (propertyDataType == PropertyDataType.PtypMultipleInteger32) || (propertyDataType == PropertyDataType.PtypMultipleFloating32) ||
                            (propertyDataType == PropertyDataType.PtypMultipleFloating64) || (propertyDataType == PropertyDataType.PtypMultipleCurrency) ||
                            (propertyDataType == PropertyDataType.PtypMultipleFloatingTime) || (propertyDataType == PropertyDataType.PtypMultipleInteger64) ||
                            (propertyDataType == PropertyDataType.PtypMultipleString) || (propertyDataType == PropertyDataType.PtypMultipleString8) ||
                            (propertyDataType == PropertyDataType.PtypMultipleTime) || (propertyDataType == PropertyDataType.PtypMultipleGuid) ||
                            (propertyDataType == PropertyDataType.PtypMultipleBinary);

            if (hasHasValue)
            {
                HasValue = ParseAs<byte, bool>();
            }

            if (HasValue == null || HasValue)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(propertyDataType, parser, PropertyCountContext.MapiHttp, true);
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AddressBookPropertyValue";
            AddChildBlockT(HasValue, "HasValue");
            if (_PropertyValue != null)
            {
                AddChild(_PropertyValue, $"PropertyValue:{_PropertyValue.Text}");
            }
            else
            {
                AddHeader("PropertyValue is null");
            }
        }
    }
}
