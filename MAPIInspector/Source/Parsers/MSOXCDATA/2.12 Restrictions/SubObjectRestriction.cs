using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.12.10 subObject Restriction Structures
    /// </summary>
    public class SubObjectRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x09.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// An unsigned integer. This value is a property tag that designates the target of the subrestriction.
        /// </summary>
        public PropertyTag Subobject;

        /// <summary>
        /// A Restriction structure.
        /// </summary>
        public RestrictionType Restriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        ///  Initializes a new instance of the SubObjectRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public SubObjectRestriction(CountWideEnum ptypMultiCountSize)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the SubObjectRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            Subobject = Parse<PropertyTag>();
            Restriction = new RestrictionType(countWide);
            Restriction.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "SubObjectRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChild(Subobject, "Subobject");
            AddLabeledChild(Restriction, $"Restriction");
        }
    }
}
