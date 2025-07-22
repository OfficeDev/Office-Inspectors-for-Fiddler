using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookFlaggedPropertyValueWithType structure.
    /// [MS-OXCMAPIHTTP] 2.2.1 Common Data Types
    /// [MS-OXCMAPIHTTP] 2.2.1.6 AddressBookFlaggedPropertyValueWithType Structure
    /// </summary>
    public class AddressBookFlaggedPropertyValueWithType : Block
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        /// </summary>
        public BlockT<PropertyDataType> PropertyType;

        /// <summary>
        /// An unsigned integer. This flag MUST be set one of three possible values: 0x0, 0x1, or 0xA, which determines what is conveyed in the PropertyValue field.
        /// </summary>
        public BlockT<byte> Flag;

        /// <summary>
        /// An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless Flag field is set to 0x01
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookFlaggedPropertyValueWithType structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            Flag = ParseT<byte>();

            if (Flag != 0x01)
            {
                if (Flag == 0x00)
                {
                    var addressPropValue = new AddressBookPropertyValue(PropertyType);
                    addressPropValue.Parse(parser);
                    PropertyValue = addressPropValue;
                }
                else if (Flag == 0x0A)
                {
                    var addressPropValueForErrorCode = new AddressBookPropertyValue(PropertyDataType.PtypErrorCode);
                    addressPropValueForErrorCode.Parse(parser);
                    PropertyValue = addressPropValueForErrorCode;
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AddressBookFlaggedPropertyValueWithType";
            AddChildBlockT(PropertyType, "PropertyType");
            AddChildBlockT(Flag, "Flag");
            if (PropertyValue != null)
            {
                AddLabeledChild(PropertyValue, "PropertyValue");
            }
            else
            {
                AddHeader("PropertyValue is null");
            }
        }
    }
}
