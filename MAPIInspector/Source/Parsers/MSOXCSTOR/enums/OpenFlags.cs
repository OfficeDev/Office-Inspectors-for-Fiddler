using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum type for additional flags that control the behavior of the RopLogon.
    /// </summary>
    [Flags]
    public enum OpenFlags : uint
    {
        /// <summary>
        /// A request for administrative access to the mailbox. 
        /// </summary>
        USE_ADMIN_PRIVILEGE = 0x00000001,

        /// <summary>
        /// A request to open a public folders message store. This flag MUST be set for public logons.
        /// </summary>
        PUBLIC = 0x00000002,

        /// <summary>
        /// This flag is ignored
        /// </summary>
        HOME_LOGON = 0x00000004,

        /// <summary>
        /// This flag is ignored
        /// </summary>
        TAKE_OWNERSHIP = 0x00000008,

        /// <summary>
        /// Requests a private server to provide an alternate public server.
        /// </summary>
        ALTERNATE_SERVER = 0x00000100,

        /// <summary>
        /// This flag allows the client to log on to a public message store that is not the user's default public message store
        /// </summary>
        IGNORE_HOME_MDB = 0x00000200,

        /// <summary>
        /// A request for a nonmessaging logon session
        /// </summary>
        NO_MAIL = 0x00000400,

        /// <summary>
        /// For a private-mailbox logon this flag SHOULD be set
        /// </summary>
        USE_PER_MDB_REPLID_MAPPING = 0x01000000,

        /// <summary>
        /// Indicates that the client supports asynchronous processing of RopSetReadFlags
        /// </summary>
        SUPPORT_PROGRESS = 0x20000000
    }
}
