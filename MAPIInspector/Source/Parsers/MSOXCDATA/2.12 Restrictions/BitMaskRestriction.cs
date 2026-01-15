using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.7 Bitmask Restriction Structures
    /// </summary>
    public class BitMaskRestriction : Block
    {
        /// <summary>
        /// The parsing context that determines count field widths.
        /// </summary>
        private PropertyCountContext context;

        /// <summary>
        /// Initializes a new instance of the BitMaskRestriction class
        /// </summary>
        /// <param name="countContext">The parsing context that determines count field widths.</param>
        public BitMaskRestriction(PropertyCountContext countContext)
        {
            context = countContext;
        }

        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x06.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// An unsigned integer. This value specifies how the server MUST perform the masking operation.
        /// </summary>
        public BlockT<BitmapRelOpType> BitmapRelOp;

        /// <summary>
        /// An unsigned integer. This value is the property tag of the property to be tested.
        /// </summary>
        public PropertyTag PropTag;

        /// <summary>
        /// An unsigned integer. The bitmask to be used for the AND operation.
        /// </summary>
        public BlockT<uint> Mask;

        /// <summary>
        /// Parse the BitMaskRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            BitmapRelOp = ParseT<BitmapRelOpType>();
            PropTag = Parse<PropertyTag>();
            Mask = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "BitMaskRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(BitmapRelOp, "BitmapRelOp");
            AddChild(PropTag, "PropTag");
            AddChildBlockT(Mask, "Mask");
        }
    }
}
