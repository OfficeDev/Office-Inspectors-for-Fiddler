namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum value of ROP response status.
    /// </summary>
    public enum RopResponseStatus : uint
    {
        /// <summary>
        /// Success response
        /// </summary>
        Success = 0x00000000,

        /// <summary>
        /// Log on redirect response
        /// </summary>
        LogonRedirect = 0x00000478,

        /// <summary>
        /// Null destination object
        /// </summary>
        NullDestinationObject = 0x00000503
    }
}
