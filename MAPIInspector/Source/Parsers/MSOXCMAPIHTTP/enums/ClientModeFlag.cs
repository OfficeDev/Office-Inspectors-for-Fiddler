namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A flag that shows the mode in which the client is running.
    /// </summary>
    public enum ClientModeFlag : ushort
    {
        /// <summary>
        /// CLIENTMODE_UNKNOWN flag
        /// </summary>
        CLIENTMODE_UNKNOWN = 0x00,

        /// <summary>
        /// CLIENTMODE_CLASSIC flag
        /// </summary>
        CLIENTMODE_CLASSIC = 0x01,

        /// <summary>
        /// CLIENTMODE_CACHED flag
        /// </summary>
        CLIENTMODE_CACHED = 0x02
    }
}