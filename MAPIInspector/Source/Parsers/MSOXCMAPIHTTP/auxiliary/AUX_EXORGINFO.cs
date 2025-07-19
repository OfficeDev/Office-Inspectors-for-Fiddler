using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_EXORGINFO Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.17 AUX_EXORGINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_EXORGINFO : Block
    {
        /// <summary>
        /// The OrgFlags
        /// </summary>
        public BlockT<OrgFlags> OrgFlags;

        /// <summary>
        /// Parse the AUX_EXORGINFO structure.
        /// </summary>
        protected override void Parse()
        {
            OrgFlags = ParseT<OrgFlags>();
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_EXORGINFO";
            AddChildBlockT(OrgFlags, "OrgFlags");
        }
    }
}