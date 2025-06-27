using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the LongTermIdRange structure.
    /// 2.2.13.12.1.1 LongTermIdRange Structure
    /// </summary>
    public class LongTermIdRange : Block
    {
        /// <summary>
        /// A LongTermId structure that specifies the beginning of a range. 
        /// </summary>
        public LongTermID MinLongTermId;

        /// <summary>
        /// A LongTermId structure that specifies the end of a range.
        /// </summary>
        public LongTermID MaxLongTermId;

        /// <summary>
        /// Parse the LongTermIdRange structure.
        /// </summary>
        protected override void Parse()
        {
            MinLongTermId = Parse<LongTermID>();
            MaxLongTermId = Parse<LongTermID>();
        }

        protected override void ParseBlocks()
        {
            AddChild(MinLongTermId, "MinLongTermId");
            AddChild(MaxLongTermId, "MaxLongTermId");
        }
    }
}
