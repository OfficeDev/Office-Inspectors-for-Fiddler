using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A flags structure that contains flags that control the behavior of the synchronization.
    /// 2.2.3.2.1.1.1 RopSynchronizationConfigure ROP Request Buffer
    /// </summary>
    [Flags]
    public enum SynchronizationFlags : ushort
    {
        /// <summary>
        /// Indicates whether the client supports Unicode
        /// </summary>
        Unicode = 0x0001,

        /// <summary>
        /// Indicates how the server downloads information about item deletions
        /// </summary>
        NoDeletions = 0x0002,

        /// <summary>
        /// Indicates whether the server downloads information about messages that went out of scope
        /// </summary>
        IgnoreNoLongerInScope = 0x0004,

        /// <summary>
        /// Indicates whether the server downloads information about changes to the read state of messages
        /// </summary>
        ReadState = 0x0008,

        /// <summary>
        /// Indicates whether the server downloads information about changes to FAI messages
        /// </summary>
        FAI = 0x0010,

        /// <summary>
        /// Indicates whether the server downloads information about changes to normal messages
        /// </summary>
        Normal = 0x0020,

        /// <summary>
        /// Indicates whether the server limits or excludes properties and subobjects output to the properties listed in PropertyTags
        /// </summary>
        OnlySpecifiedProperties = 0x0080,

        /// <summary>
        /// Identifies whether the server ignores any persisted values for the PidTagSourceKey property (section 2.2.1.2.5) and PidTagParentSourceKey property (section 2.2.1.2.6) when producing output for folder and message changes.
        /// </summary>
        NoForeignIdentifies = 0x0100,

        /// <summary>
        /// This flag MUST be set to 0 when sending.
        /// </summary>
        Reserved = 0x1000,

        /// <summary>
        /// Identifies whether the server outputs message bodies in their original format or in RTF
        /// </summary>
        BesBody = 0x2000,

        /// <summary>
        /// Indicates whether the server outputs properties and subobjects of FAI messages
        /// </summary>
        IgnoreSpecifiedOnFAI = 0x4000,

        /// <summary>
        /// Indicates whether the server injects progress information into the output FastTransfer stream
        /// </summary>
        Progress = 0x8000,
    }
}
