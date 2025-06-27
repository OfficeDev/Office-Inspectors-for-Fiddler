using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookTypedPropertyValue structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.4 AddressBookTypedPropertyValue Structure
    /// </summary>
    public class AddressBookTypedPropertyValue : BaseStructure
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
        /// Source property tag information
        /// </summary>
        public AnnotatedComment PropertyTag;

        /// <summary>
        /// Parse the AddressBookTypedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookTypedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            PropertyType = (PropertyDataType)ReadUshort();
            AddressBookPropertyValue addressBookPropValue = new AddressBookPropertyValue(PropertyType);
            addressBookPropValue.Parse(s);
            PropertyValue = addressBookPropValue;
        }
    }
}