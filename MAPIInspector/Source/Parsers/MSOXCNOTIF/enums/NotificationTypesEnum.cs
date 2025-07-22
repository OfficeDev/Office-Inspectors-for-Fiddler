using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum value of Notification type.
    /// </summary>
    [Flags]
    public enum NotificationTypesEnum : ushort
    {
        /// <summary>
        /// A new email message has been received by the server
        /// </summary>
        NewMail = 0x0002,

        /// <summary>
        /// A new object has been created on the server.
        /// </summary>
        ObjectCreated = 0x0004,

        /// <summary>
        /// An existing object has been deleted from the server
        /// </summary>
        ObjectDeleted = 0x0008,

        /// <summary>
        /// An existing object has been modified on the server
        /// </summary>
        ObjectModified = 0x0010,

        /// <summary>
        /// An existing object has been moved to another location on the server
        /// </summary>
        ObjectMoved = 0x0020,

        /// <summary>
        /// An existing object has been copied on the server.
        /// </summary>
        ObjectCopied = 0x0040,

        /// <summary>
        /// A search operation has been completed on the server
        /// </summary>
        SearchCompleted = 0x0080,

        /// <summary>
        /// A table has been modified on the server
        /// </summary>
        TableModified = 0x0100,

        /// <summary>
        /// Extended one
        /// </summary>
        Extended = 0x0400,

        /// <summary>
        /// The notification contains information about a change in the total number of messages in a folder triggering the event
        /// </summary>
        T = 0x1000,

        /// <summary>
        /// The notification contains information about a change in the number of unread messages in a folder triggering the event
        /// </summary>
        U = 0x2000,

        /// <summary>
        /// The notification is caused by an event in a search folder
        /// </summary>
        S = 0x4000,

        /// <summary>
        /// The notification is caused by an event on a message
        /// </summary>
        M = 0x8000,
    }
}
