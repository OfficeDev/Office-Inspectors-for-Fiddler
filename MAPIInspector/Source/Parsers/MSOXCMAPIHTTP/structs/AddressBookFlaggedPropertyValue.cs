using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookFlaggedPropertyValue structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.5 AddressBookFlaggedPropertyValue Structure
    /// </summary>
    public class AddressBookFlaggedPropertyValue : Block
    {
        /// <summary>
        /// An unsigned integer. This value of this flag determines what is conveyed in the PropertyValue field.
        /// </summary>
        public BlockT<byte> Flag;

        /// <summary>
        /// An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless the Flag field is set to 0x1.
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// A PropertyDataType used to initialize the constructed function
        /// </summary>
        private PropertyDataType propertyDataType;

        /// <summary>
        /// Initializes a new instance of the AddressBookFlaggedPropertyValue class.
        /// </summary>
        /// <param name="propertyDataType">The PropertyDataType parameter for AddressBookFlaggedPropertyValue</param>
        public AddressBookFlaggedPropertyValue(PropertyDataType propertyDataType)
        {
            this.propertyDataType = propertyDataType;
        }

        /// <summary>
        /// Parse the AddressBookFlaggedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            Flag = ParseT<byte>();
            if (Flag != 0x01)
            {
                if (Flag == 0x00)
                {
                    var addressPropValue = new AddressBookPropertyValue(propertyDataType);
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
            Text = "AddressBookFlaggedPropertyValue";
            AddChildBlockT(Flag, "Flag");
            AddChild(PropertyValue, "PropertyValue");
        }
    }
}