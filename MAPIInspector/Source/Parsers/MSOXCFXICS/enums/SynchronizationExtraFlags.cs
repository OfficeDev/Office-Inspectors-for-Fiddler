namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A flags structure that contains flags control the additional behavior of the synchronization.
    /// [MS-OXCFXICS] 2.2.3.2.1.1.1 RopSynchronizationConfigure ROP Request Buffer
    /// </summary>
    public enum SynchronizationExtraFlags : uint
    {
        /// <summary>
        /// Indicates whether the server includes the PidTagFolderId or PidTagMid properties in the folder change or message change header
        /// </summary>
        Eid = 0x00000001,

        /// <summary>
        /// Indicates whether the server includes the PidTagMessageSize property in the message change header.
        /// </summary>
        MessageSize = 0x00000002,

        /// <summary>
        /// Indicates whether the server includes the PidTagChangeNumber property in the message change header
        /// </summary>
        CN = 0x00000004,

        /// <summary>
        /// Indicates whether the server sorts messages by their delivery time
        /// </summary>
        OrderByDeliveryTime = 0x00000008,
    }
}
