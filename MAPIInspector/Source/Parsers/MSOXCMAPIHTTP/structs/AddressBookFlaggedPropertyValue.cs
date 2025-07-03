using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookFlaggedPropertyValue structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.5 AddressBookFlaggedPropertyValue Structure
    /// </summary>
    public class AddressBookFlaggedPropertyValue : BaseStructure
    {
        /// <summary>
        /// An unsigned integer. This value of this flag determines what is conveyed in the PropertyValue field.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless the Flag field is set to 0x1.
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// A PropertyDataType used to initialize the constructed function
        /// </summary>
        private PropertyDataType propertyDataType;

        /// <summary>
        /// Source property tag information
        /// </summary>
        public AnnotatedComment PropertyTag;

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
        /// <param name="s">A stream containing AddressBookFlaggedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            Flag = ReadByte();

            if (Flag != 0x01)
            {
                if (Flag == 0x00)
                {
                    AddressBookPropertyValue addressPropValue = new AddressBookPropertyValue(propertyDataType);
                    addressPropValue.Parse(s);
                    PropertyValue = addressPropValue;
                }
                else if (Flag == 0x0A)
                {
                    AddressBookPropertyValue addressPropValueForErrorCode = new AddressBookPropertyValue(PropertyDataType.PtypErrorCode);
                    addressPropValueForErrorCode.Parse(s);
                    PropertyValue = addressPropValueForErrorCode;
                }
            }
        }
    }
}