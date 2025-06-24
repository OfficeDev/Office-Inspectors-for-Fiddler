namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The version information of the payload data. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum PayloadDataVersion : byte
    {
        /// <summary>
        /// AUX_VERSION_1 flag
        /// </summary>
        AUX_VERSION_1 = 0x01,

        /// <summary>
        /// AUX_VERSION_2 flag
        /// </summary>
        AUX_VERSION_2 = 0x02
    }
}