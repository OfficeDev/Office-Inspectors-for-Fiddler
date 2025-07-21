using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.14.1 RopGetContentsTable ROP Request Buffer
    /// </summary>
    [Flags]
    public enum ContentsTableFlags : byte
    {
        /// <summary>
        /// The contents table lists only the FAI messages.
        /// </summary>
        Associated = 0x02,

        /// <summary>
        /// Deferred Errors
        /// </summary>
        DeferredErrors = 0x08,

        /// <summary>
        /// The contents table notifications to the client are disabled
        /// </summary>
        NoNotifications = 0x10,

        /// <summary>
        /// The contents table lists only the messages that are soft deleted
        /// </summary>
        SoftDeletes = 0x20,

        /// <summary>
        /// The columns that contain string data are returned in Unicode format
        /// </summary>
        UseUnicode = 0x40,

        /// <summary>
        /// The contents table lists messages pertaining to a single conversation (one result row represents a single message)
        /// </summary>
        ConversationMembers = 0x80
    }
}