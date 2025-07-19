using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.3 RuleData
    /// The RuleData structure contains properties and flags that provide details about a standard rule.
    /// </summary>
    public class RuleData : Block
    {
        /// <summary>
        /// A value that contains flags specifying whether the rule (2) is to be added, modified, or deleted.
        /// </summary>
        public BlockT<RuleDataFlags> RuleDataFlags;

        /// <summary>
        /// An integer that specifies the number of properties that are specified in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures, as specified in [MS-OXCDATA] section 2.11.4, each of which contains one property of a standard rule.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RuleData structure.
        /// </summary>
        protected override void Parse()
        {
            RuleDataFlags = ParseT<RuleDataFlags>();
            PropertyValueCount = ParseT<ushort>();
            var tempPropertyValues = new List<TaggedPropertyValue>();
            for (int i = 0; i < PropertyValueCount; i++)
            {
                var temptaggedPropertyValue = new TaggedPropertyValue(CountWideEnum.twoBytes);
                temptaggedPropertyValue.Parse(parser);
                tempPropertyValues.Add(temptaggedPropertyValue);
            }

            PropertyValues = tempPropertyValues.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RuleData";
            AddChildBlockT(RuleDataFlags, "RuleDataFlags");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
