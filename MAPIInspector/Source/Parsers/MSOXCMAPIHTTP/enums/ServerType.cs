namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The server type assigned by client.
    /// </summary>
    public enum ServerType : ushort
    {
        /// <summary>
        /// SERVERTYPE_UNKNOWN flag
        /// </summary>
        SERVERTYPE_UNKNOWN = 0x00,

        /// <summary>
        /// SERVERTYPE_PRIVATE flag
        /// </summary>
        SERVERTYPE_PRIVATE = 0x01,

        /// <summary>
        /// SERVERTYPE_PUBLIC flag
        /// </summary>
        SERVERTYPE_PUBLIC = 0x02,

        /// <summary>
        /// SERVERTYPE_DIRECTORY flag
        /// </summary>
        SERVERTYPE_DIRECTORY = 0x03,

        /// <summary>
        /// SERVERTYPE_REFERRAL flag
        /// </summary>
        SERVERTYPE_REFERRAL = 0x04
    }
}
