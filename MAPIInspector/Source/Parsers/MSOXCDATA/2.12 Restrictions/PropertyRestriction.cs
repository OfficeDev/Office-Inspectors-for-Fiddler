using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.5 Property Restriction Structures
    /// </summary>
    public class PropertyRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x4.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// An unsigned integer. This value indicates the relational operator that is used to compare the property on the object with the value of the TaggedValue field.
        /// </summary>
        public BlockT<RelOpType> RelOp;

        /// <summary>
        /// An unsigned integer. This value indicates the property tag of the property that MUST be compared.
        /// </summary>
        public PropertyTag PropTag;

        /// <summary>
        /// A TaggedValue structure, as specified in section 2.11.4.
        /// </summary>
        public TaggedPropertyValue TaggedValue;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PropertyRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public PropertyRestriction(CountWideEnum ptypMultiCountSize)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the PropertyRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            RelOp = ParseT<RelOpType>();
            PropTag = Parse<PropertyTag>();
            TaggedValue = new TaggedPropertyValue(countWide, PropTag);
            TaggedValue.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "PropertyRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(RelOp, "RelOp");
            AddChild(PropTag, "PropTag");
            AddLabeledChild(TaggedValue, "TaggedValue");
        }
    }
}
