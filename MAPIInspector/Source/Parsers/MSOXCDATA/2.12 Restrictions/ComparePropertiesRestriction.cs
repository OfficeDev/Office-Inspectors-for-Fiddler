using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.12.6 Compare Properties Restriction Structures
    /// </summary>
    public class ComparePropertiesRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x05.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// An unsigned integer. This value indicates the relational operator used to compare the two properties.
        /// </summary>
        public BlockT<RelOpType> RelOp;

        /// <summary>
        /// An unsigned integer. This value is the property tag of the first property that MUST be compared.
        /// </summary>
        public PropertyTag PropTag1;

        /// <summary>
        /// An unsigned integer. This value is the property tag of the second property that MUST be compared.
        /// </summary>
        public PropertyTag PropTag2;

        /// <summary>
        /// Parse the ComparePropertiesRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            RelOp = ParseT<RelOpType>();
            PropTag1 = Parse<PropertyTag>();
            PropTag2 = Parse<PropertyTag>();
        }

        protected override void ParseBlocks()
        {
            Text = "ComparePropertiesRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(RelOp, "RelOp");
            AddChild(PropTag1, "PropTag1");
            AddChild(PropTag2, "PropTag2");
        }
    }
}
