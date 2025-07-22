using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.12 CountRestriction Structure
    /// </summary>
    public class CountRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x0B.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// An unsigned integer. This value specifies the limit on the number of matches to be returned when the value of the SubRestriction field is evaluated.
        /// </summary>
        public BlockT<uint> Count;

        /// <summary>
        /// A restriction structure. This field specifies the restriction (2) to be limited.
        /// </summary>
        public RestrictionType SubRestriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the CountRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public CountRestriction(CountWideEnum ptypMultiCountSize)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the CountRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            Count = ParseT<uint>();
            SubRestriction = new RestrictionType(countWide);
            SubRestriction.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "CountRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(Count, "Count");
            AddLabeledChild(SubRestriction, "SubRestriction");
        }
    }
}
