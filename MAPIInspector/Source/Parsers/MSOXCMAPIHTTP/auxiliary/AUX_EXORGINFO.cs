namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// A class indicates the AUX_EXORGINFO Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.17   AUX_EXORGINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_EXORGINFO : BaseStructure
    {
        /// <summary>
        /// The OrgFlags
        /// </summary>
        public OrgFlags OrgFlags;

        /// <summary>
        /// Parse the AUX_EXORGINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_EXORGINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            OrgFlags = (OrgFlags)ReadUint();
        }
    }
}