using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookTypedPropertyValue structure.
    /// [MS-OXCMAPIHTTP] 2.2.1 Common Data Types
    /// [MS-OXCMAPIHTTP] 2.2.1.4 AddressBookTypedPropertyValue Structure
    /// </summary>
    public class AddressBookTypedPropertyValue : Block
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An AddressBookPropertyValue structure
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookTypedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            PropertyValue = new AddressBookPropertyValue(PropertyType);
            PropertyValue.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "AddressBookTypedPropertyValue";
            AddChild(PropertyValue, "PropertyValue");
        }
    }
}
