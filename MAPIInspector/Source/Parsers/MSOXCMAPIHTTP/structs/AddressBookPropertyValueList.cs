using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the AddressBookPropertyValueList structure.
    ///  2.2.1 Common Data Types
    ///  2.2.1.3 AddressBookPropertyValueList Structure
    /// </summary>
    public class AddressBookPropertyValueList : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the PropertyValues field.
        /// </summary>
        public uint PropertyValueCount;

        /// <summary>
        /// An array of AddressBookTaggedPropertyValue structures
        /// </summary>
        public AddressBookTaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the AddressBookPropertyValueList structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookPropertyValueList structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            PropertyValueCount = ReadUint();
            List<AddressBookTaggedPropertyValue> tempABTP = new List<AddressBookTaggedPropertyValue>();

            for (int i = 0; i < PropertyValueCount; i++)
            {
                AddressBookTaggedPropertyValue abtp = new AddressBookTaggedPropertyValue();
                abtp.Parse(s);
                tempABTP.Add(abtp);
            }

            PropertyValues = tempABTP.ToArray();
        }
    }
}