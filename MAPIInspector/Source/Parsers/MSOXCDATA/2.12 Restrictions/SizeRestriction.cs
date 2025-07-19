using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.12.8 Size Restriction Structures
    /// </summary>
    public class SizeRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x07.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        ///  An unsigned integer. This value indicates the relational operator used in the size comparison.
        /// </summary>
        public BlockT<RelOpType> RelOp;

        /// <summary>
        /// An unsigned integer. This value indicates the property tag of the property whose value size is being tested.
        /// </summary>
        public PropertyTag PropTag;

        /// <summary>
        /// An unsigned integer. This value indicates the size, in bytes, that is to be used in the comparison.
        /// </summary>
        public BlockT<uint> _Size;

        /// <summary>
        /// Parse the SizeRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            RelOp = ParseT<RelOpType>();
            PropTag = Parse<PropertyTag>();
            _Size = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "SizeRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(RelOp, "RelOp");
            AddChild(PropTag, "PropTag");
            AddChildBlockT(_Size, "_Size");
        }
    }
}
