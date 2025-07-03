using MapiInspector;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookTaggedPropertyValue structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.2 AddressBookTaggedPropertyValue Structure
    /// </summary>
    public class AddressBookTaggedPropertyValue : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public PidTagPropertyEnum PropertyId;

        /// <summary>
        /// An AddressBookPropertyValue structure
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Source property tag information
        /// </summary>
        public AnnotatedComment PropertyTag;

        /// <summary>
        /// Parse the AddressBookTaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookTaggedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            PropertyType = (PropertyDataType)ReadUshort();
            PropertyId = (PidTagPropertyEnum)ReadUshort();
            AddressBookPropertyValue addressBookValue = new AddressBookPropertyValue(PropertyType);
            addressBookValue.Parse(s);
            PropertyValue = addressBookValue;
            PropertyTag = $"{PropertyType}:{Utilities.EnumToString(PropertyId)}";
        }
    }
}