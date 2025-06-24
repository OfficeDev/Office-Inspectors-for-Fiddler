namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// 2.2.1.8 PidTagMessageStatus Property
    /// The enum specifies the status of a message in a contents table.
    /// </summary>
    [Flags]
    public enum MessageStatusFlag : uint
    {
        /// <summary>
        /// The message has been marked for downloading from the remote message store to the local client
        /// </summary>
        msRemoteDownload = 0x00001000,

        /// <summary>
        /// This is a conflict resolve message
        /// </summary>
        msInConflict = 0x00000800,

        /// <summary>
        /// The message has been marked for deletion at the remote message store without downloading to the local client
        /// </summary>
        msRemoteDelete = 0x00002000
    }
}
