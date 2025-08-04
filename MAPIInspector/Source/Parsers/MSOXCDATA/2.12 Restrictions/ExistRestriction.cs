using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.9 Exist Restriction Structures
    /// </summary>
    public class ExistRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x08.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// This value encodes the PropTag field of the SizeRestriction structure.
        /// </summary>
        public PropertyTag PropTag;

        /// <summary>
        /// Parse the ExistRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            PropTag = Parse<PropertyTag>();
        }

        protected override void ParseBlocks()
        {
            Text = "ExistRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChild(PropTag, "PropTag");
        }
    }
}
