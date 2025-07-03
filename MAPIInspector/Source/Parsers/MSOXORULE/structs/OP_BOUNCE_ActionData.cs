using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1.2.5 OP_BOUNCE ActionData Structure
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.5 OP_BOUNCE ActionData Structure
    /// </summary>
    public class OP_BOUNCE_ActionData : Block
    {
        /// <summary>
        /// An integer that specifies a bounce code.
        /// </summary>
        public BlockT<BounceCodeEnum> BounceCode;

        /// <summary>
        /// Parse the OP_BOUNCE_ActionData structure.
        /// </summary>
        protected override void Parse()
        {
            BounceCode = ParseT<BounceCodeEnum>();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(BounceCode, "BounceCode");
        }
    }
}
