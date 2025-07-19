using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookTaggedPropertyValue structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.2 AddressBookTaggedPropertyValue Structure
    /// </summary>
    public class AddressBookTaggedPropertyValue : Block
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        /// </summary>
        public BlockT<PropertyDataType> PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public BlockT<PidTagPropertyEnum> PropertyId;

        /// <summary>
        /// An AddressBookPropertyValue structure
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookTaggedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            PropertyId = ParseT<PidTagPropertyEnum>();
            PropertyValue = new AddressBookPropertyValue(PropertyType);
            PropertyValue.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "AddressBookTaggedPropertyValue";
            AddChildBlockT(PropertyType, "PropertyType");
            AddChildBlockT(PropertyId, "PropertyId");
            AddChild(PropertyValue, "PropertyValue");
        }
    }
}