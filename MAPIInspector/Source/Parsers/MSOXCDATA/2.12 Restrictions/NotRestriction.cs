using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.12.3 Not Restriction Structures
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
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the AndRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public NotRestriction(CountWideEnum ptypMultiCountSize)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the NotRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            Restriction = new RestrictionType(countWide);
            Restriction.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("NotRestriction");
            AddChildBlockT(RestrictType, "RestrictType");
            AddLabeledChild(Restriction, "Restriction");
        }
    }
}
