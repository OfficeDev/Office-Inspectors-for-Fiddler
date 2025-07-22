using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.4 Content Restriction Structures
    /// </summary>
    public class ContentRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x03.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// An unsigned integer. This field specifies the level of precision that the server enforces when checking for a match against a ContentRestriction structure.
        /// </summary>
        public BlockT<FuzzyLevelLowEnum> FuzzyLevelLow;

        /// <summary>
        /// This field applies only to string-value properties.
        /// </summary>
        public BlockT<FuzzyLevelHighEnum> FuzzyLevelHigh;

        /// <summary>
        /// This value indicates the property tag of the column whose value MUST be matched against the value specified in the TaggedValue field.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A TaggedPropertyValue structure, as specified in section 2.11.4.
        /// </summary>
        public TaggedPropertyValue TaggedValue;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the ContentRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public ContentRestriction(CountWideEnum ptypMultiCountSize)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the ContentRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            FuzzyLevelLow = ParseT<FuzzyLevelLowEnum>();
            FuzzyLevelHigh = ParseT<FuzzyLevelHighEnum>();
            PropertyTag = Parse<PropertyTag>();
            TaggedValue = new TaggedPropertyValue(countWide, PropertyTag);
            TaggedValue.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "ContentRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(FuzzyLevelLow, "FuzzyLevelLow");
            AddChildBlockT(FuzzyLevelHigh, "FuzzyLevelHigh");
            AddChild(PropertyTag);
            AddChild(TaggedValue, "TaggedValue");
        }
    }
}
