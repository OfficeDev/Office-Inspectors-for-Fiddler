namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the AUX_ENDPOINT_CAPABILITIES Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.19  AUX_ENDPOINT_CAPABILITIES
    /// </summary>
    public class AUX_ENDPOINT_CAPABILITIES : BaseStructure
    {
        /// <summary>
        /// A flag that indicates that the server combines capabilities on a single endpoint.
        /// </summary>
        public EndpointCapabilityFlag EndpointCapabilityFlag;

        /// <summary>
        /// Parse the AUX_ENDPOINT_CAPABILITIES structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_ENDPOINT_CAPABILITIES structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            EndpointCapabilityFlag = (EndpointCapabilityFlag)ReadUint();
        }
    }
}