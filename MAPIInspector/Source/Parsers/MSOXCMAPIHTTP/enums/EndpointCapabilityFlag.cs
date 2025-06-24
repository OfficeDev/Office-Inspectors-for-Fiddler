namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A flag that indicates that the server combines capabilities on a single endpoint. It is defined in section 2.2.2.2.19 of MS-OXCRPC.
    /// </summary>
    public enum EndpointCapabilityFlag : uint
    {
        /// <summary>
        /// Endpoint capabilities single endpoint
        /// </summary>
        ENDPOINT_CAPABILITIES_SINGLE_ENDPOINT = 0x00000001
    }
}