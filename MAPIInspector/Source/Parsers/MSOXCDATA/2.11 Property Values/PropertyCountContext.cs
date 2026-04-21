namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Defines the parsing context that determines count field width for property values.
    /// Based on [MS-OXCDATA] 2.11.1.1 COUNT Data Type Values specification.
    /// </summary>
    public enum PropertyCountContext
    {
        /// <summary>
        /// ROP buffers context - PtypBinary uses 16-bit count, PtypMultiple uses 32-bit count.
        /// Used in RopGetPropertiesSpecific ROP ([MS-OXCROPS] section 2.2.8.3).
        /// </summary>
        RopBuffers,

        /// <summary>
        /// Extended rules context - Both PtypBinary and PtypMultiple use 32-bit count.
        /// Used in [MS-OXORULE] section 2.2.4.
        /// </summary>
        ExtendedRules,

        /// <summary>
        /// MAPI extensions for HTTP context - Both PtypBinary and PtypMultiple use 32-bit count.
        /// Used in [MS-OXCMAPIHTTP] section 2.2.5.
        /// </summary>
        MapiHttp,

        /// <summary>
        /// Address book context - Uses same rules as MAPI HTTP context.
        /// </summary>
        AddressBook
    }
}