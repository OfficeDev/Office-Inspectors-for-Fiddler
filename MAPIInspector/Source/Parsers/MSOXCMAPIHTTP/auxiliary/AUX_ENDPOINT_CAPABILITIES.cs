using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_ENDPOINT_CAPABILITIES Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.19  AUX_ENDPOINT_CAPABILITIES
    /// </summary>
    public class AUX_ENDPOINT_CAPABILITIES : Block
    {
        /// <summary>
        /// A flag that indicates that the server combines capabilities on a single endpoint.
        /// </summary>
        public BlockT<EndpointCapabilityFlag> EndpointCapabilityFlag;

        /// <summary>
        /// Parse the AUX_ENDPOINT_CAPABILITIES structure.
        /// </summary>
        protected override void Parse()
        {
            EndpointCapabilityFlag = ParseT<EndpointCapabilityFlag>();
        }

        protected override void ParseBlocks()
        {
            SetText("AUX_ENDPOINT_CAPABILITIES");
            AddChildBlockT(EndpointCapabilityFlag, "EndpointCapabilityFlag");
        }
    }
}