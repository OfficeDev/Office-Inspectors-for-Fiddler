using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the LargePropertyTagArray structure.
    /// [MS-OXCMAPIHTTP] 2.2.1 Common Data Types
    /// [MS-OXCMAPIHTTP] 2.2.1.8 LargePropertyTagArray Structure
    /// </summary>
    public class LargePropertyTagArray : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the PropertyTags field.
        /// </summary>
        public BlockT<uint> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures, each of which contains a property tag that specifies a property.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the LargePropertyTagArray structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyTagCount = ParseT<uint>();
            var tempPT = new List<PropertyTag>();
            for (int i = 0; i < PropertyTagCount; i++)
            {
                tempPT.Add(Parse<PropertyTag>());
            }

            PropertyTags = tempPT.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "LargePropertyTagArray";
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}
