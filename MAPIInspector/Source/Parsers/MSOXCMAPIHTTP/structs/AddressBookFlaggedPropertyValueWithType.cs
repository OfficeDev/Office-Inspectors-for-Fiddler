using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookFlaggedPropertyValueWithType structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.6 AddressBookFlaggedPropertyValueWithType Structure
    /// </summary>
    public class AddressBookFlaggedPropertyValueWithType : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer. This flag MUST be set one of three possible values: 0x0, 0x1, or 0xA, which determines what is conveyed in the PropertyValue field.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless Flag field is set to 0x01
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Source property tag information
        /// </summary>
        public AnnotatedComment PropertyTag;

        /// <summary>
        /// Parse the AddressBookFlaggedPropertyValueWithType structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookFlaggedPropertyValueWithType structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            PropertyType = (PropertyDataType)ReadUshort();
            Flag = ReadByte();

            if (Flag != 0x01)
            {
                if (Flag == 0x00)
                {
                    AddressBookPropertyValue addressPropValue = new AddressBookPropertyValue(PropertyType);
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