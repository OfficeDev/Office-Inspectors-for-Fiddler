namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The EnableFlags values
    /// </summary>
    public enum EnableFlags : uint
    {
        /// <summary>
        /// ENABLE_PERF_SENDTOSERVER flag
        /// </summary>
        ENABLE_PERF_SENDTOSERVER = 0x00000001,

        /// <summary>
        /// ENABLE_COMPRESSION flag
        /// </summary>
        ENABLE_COMPRESSION = 0x00000004,

        /// <summary>
        /// ENABLE_HTTP_TUNNELING flag
        /// </summary>
        ENABLE_HTTP_TUNNELING = 0x00000008,

        /// <summary>
        /// ENABLE_PERF_SENDGCDATA flag
        /// </summary>
        ENABLE_PERF_SENDGCDATA = 0x00000010
    }
}