using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookPropertyValueList structure.
    /// [MS-OXCMAPIHTTP] 2.2.1 Common Data Types
    /// [MS-OXCMAPIHTTP] 2.2.1.3 AddressBookPropertyValueList Structure
    /// </summary>
    public class AddressBookPropertyValueList : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the PropertyValues field.
        /// </summary>
        public BlockT<uint> PropertyValueCount;

        /// <summary>
        /// An array of AddressBookTaggedPropertyValue structures
        /// </summary>
        public AddressBookTaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the AddressBookPropertyValueList structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyValueCount = ParseT<uint>();
            var tempABTP = new List<AddressBookTaggedPropertyValue>();
            for (int i = 0; i < PropertyValueCount; i++)
            {
                tempABTP.Add(Parse<AddressBookTaggedPropertyValue>());
            }

            PropertyValues = tempABTP.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "AddressBookPropertyValueList";
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
