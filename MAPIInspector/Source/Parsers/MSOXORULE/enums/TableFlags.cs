namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.1.3 RuleData Structure
    /// The enum value of TableFlags.
    /// </summary>
    public enum TableFlags : byte
    {
        /// <summary>
        /// This bit is set if the client is requesting that string values in the table be returned as Unicode strings
        /// </summary>
        U_0x40 = 0x40,

        /// <summary>
        /// These unused bits MUST be set to zero by the client
        /// </summary>
        U_0x00 = 0x00
    }
}
