using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1.2.4.1 RecipientBlockData Structure
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.4.1 RecipientBlockData Structure
    /// </summary>
    public class RecipientBlockData : Block
    {
        /// <summary>
        /// This value is implementation-specific and not required for interoperability
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// An integer that specifies the number of structures present in the PropertyValues field. This number MUST be greater than zero.
        /// </summary>
        public BlockT<ushort> NoOfProperties;

        /// <summary>
        /// An array of TaggedPropertyValue structures, each of which contains a property that provides some information about the recipient (2).
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RecipientBlockData structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<byte>();
            NoOfProperties = ParseT<ushort>();
            var propertyValues = new List<TaggedPropertyValue>();
            for (int i = 0; i < NoOfProperties; i++)
            {
                TaggedPropertyValue propertyValue = new TaggedPropertyValue();
                propertyValue.Parse(parser);
                propertyValues.Add(propertyValue);
            }

            PropertyValues = propertyValues.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RecipientBlockData";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(NoOfProperties, "NoOfProperties");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
