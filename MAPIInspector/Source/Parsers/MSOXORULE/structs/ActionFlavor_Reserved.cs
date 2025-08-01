using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.5.1.1 Action Flavors
    /// This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is not related to REPLY or FORWARD
    /// </summary>
    public class ActionFlavor_Reserved : Block
    {
        /// <summary>
        /// The reserved bits.
        /// </summary>
        public BlockT<int> Reservedbits;

        /// <summary>
        /// Parse the ActionFlavor_Reserved structure.
        /// </summary>
        protected override void Parse()
        {
            Reservedbits = ParseT<int>();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Reservedbits, "Reservedbits");
        }
    }
}
