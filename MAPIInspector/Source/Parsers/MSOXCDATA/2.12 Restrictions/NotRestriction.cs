using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.3 Not Restriction Structures
    /// </summary>
    public class NotRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x02.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// A restriction structure. This value specifies the restriction (2) that the logical NOT operation applies to.
        /// </summary>
        public RestrictionType Restriction;

        /// <summary>
        /// The parsing context that determines count field widths.
        /// </summary>
        private readonly PropertyCountContext context;

        /// <summary>
        /// Initializes a new instance of the NotRestriction class
        /// </summary>
        /// <param name="countContext">The parsing context that determines count field widths.</param>
        public NotRestriction(PropertyCountContext countContext)
        {
            context = countContext;
        }

        /// <summary>
        /// Parse the NotRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            Restriction = new RestrictionType(context);
            Restriction.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "NotRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddLabeledChild(Restriction, "Restriction");
        }
    }
}
