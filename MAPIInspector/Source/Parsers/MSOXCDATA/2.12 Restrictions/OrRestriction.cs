using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.2.1 OrRestriction Structure
    /// </summary>
    public class OrRestriction : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicates the type of restriction (2) and MUST be set to 0x00.
        /// </summary>
        public BlockT<RestrictTypeEnum> RestrictType;

        /// <summary>
        /// This value specifies how many restriction structures are present in the Restricts field. The width of this field is 16 bits in the context of ROPs and 32 bits in the context of extended rules.
        /// </summary>
        public BlockT<uint> RestrictCount;

        /// <summary>
        /// An array of restriction structures. This field MUST contain the number of structures indicated by the RestrictCount field.
        /// </summary>
        public RestrictionType[] Restricts;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the OrRestriction class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public OrRestriction(CountWideEnum ptypMultiCountSize)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the OrRestriction structure.
        /// </summary>
        protected override void Parse()
        {
            RestrictType = ParseT<RestrictTypeEnum>();
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    RestrictCount = ParseAs<ushort, uint>();
                    break;
                default:
                case CountWideEnum.fourBytes:
                    RestrictCount = ParseT<uint>();
                    break;
            }

            var tempRestricts = new List<RestrictionType>();
            for (int length = 0; length < RestrictCount; length++)
            {
                var tempRestriction = new RestrictionType();
                tempRestriction.Parse(parser);
                tempRestricts.Add(tempRestriction);
            }

            Restricts = tempRestricts.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "OrRestriction";
            AddChildBlockT(RestrictType, "RestrictType");
            AddChildBlockT(RestrictCount, "RestrictCount");
            AddLabeledChildren(Restricts, "Restricts");
        }
    }
}
