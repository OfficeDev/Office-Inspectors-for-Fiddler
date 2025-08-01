namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The OrgFlags enum
    /// </summary>
    public enum OrgFlags : uint
    {
        /// <summary>
        /// Public folder enable flag
        /// </summary>
        PUBLIC_FOLDERS_ENABLED = 0x00000001,

        /// <summary>
        /// Use auto-discover for public folder configuration
        /// </summary>
        USE_AUTODISCOVER_FOR_PUBLIC_FOLDER_CONFIGURATION = 0x0000002
    }
}
