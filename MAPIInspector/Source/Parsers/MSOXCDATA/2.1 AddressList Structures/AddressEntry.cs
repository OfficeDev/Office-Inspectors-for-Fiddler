using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.1 AddressList Structures
    /// 2.1.1 AddressEntry Structure
    /// </summary>
    public class AddressEntry : Block
    {
        /// <summary>
        /// An unsigned integer whose value is equal to the number of associated TaggedPropertyValue structures, as specified in section 2.11.4.
        /// </summary>
        public BlockT<uint> PropertyCount;

        /// <summary>
        /// A set of TaggedPropertyValue structures representing one addressee.
        /// </summary>
        public TaggedPropertyValue[] Values;

        /// <summary>
        /// Parse the AddressEntry structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyCount = ParseT<uint>();
            var tempArray = new List<TaggedPropertyValue>();
            for (int i = 0; i < PropertyCount; i++)
            {
                var tempproperty = new TaggedPropertyValue();
                tempproperty.Parse(parser);
                tempArray.Add(tempproperty);
            }

            Values = tempArray.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("AddressEntry");
            AddChildBlockT(PropertyCount, "PropertyCount");
            AddLabeledChildren(Values, "Values");
        }
    }
}
