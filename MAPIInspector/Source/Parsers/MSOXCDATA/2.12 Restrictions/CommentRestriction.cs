using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.12.11 CommentRestriction Structure
    /// </summary>
    public class CommentRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x0A.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// An unsigned integer. This value specifies how many TaggedValue structures are present in the TaggedValues field.
        /// </summary>
        public BlockT<byte> TaggedValuesCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures, as specified in section 2.11.4.
        /// </summary>
        public TaggedPropertyValue[] TaggedValues;

        /// <summary>
        /// An unsigned integer. This field MUST contain either TRUE (0x01) or FALSE (0x00).
        /// </summary>
        public BlockT<bool> RestrictionPresent;

        /// <summary>
        /// A Restriction structure. This field is present only if RestrictionPresent is TRUE.
        /// </summary>
        public RestrictionType Restriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the CommentRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public CommentRestriction(CountWideEnum ptypMultiCountSize)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the CommentRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            TaggedValuesCount = ParseT<byte>();
            var tempTaggedValue = new List<TaggedPropertyValue>();
            for (int i = 0; i < TaggedValuesCount.Data; i++)
            {
                var tempproperty = new TaggedPropertyValue(countWide);
                tempproperty.Parse(parser);
                tempTaggedValue.Add(tempproperty);
            }

            TaggedValues = tempTaggedValue.ToArray();
            RestrictionPresent = ParseAs<byte, bool>();
            if (RestrictionPresent.Data == true)
            {
                Restriction = new RestrictionType(countWide);
                Restriction.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("CommentRestriction");
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(TaggedValuesCount, "TaggedValuesCount");
            AddLabeledChildren(TaggedValues, "TaggedValues");
            AddChildBlockT(RestrictionPresent, "RestrictionPresent");
            if (Restriction != null)
            {
                AddChild(Restriction);
            }
        }
    }
}
