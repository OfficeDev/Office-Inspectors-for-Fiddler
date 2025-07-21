using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.3.1 RopDeleteFolder ROP Request Buffer
    /// </summary>
    [Flags]
    public enum DeleteFolderFlags : byte
    {
        /// <summary>
        /// The folder and all of the Message objects in the folder are deleted.
        /// </summary>
        DEL_MESSAGES = 0x01,

        /// <summary>
        /// The folder and all of its subfolders are deleted
        /// </summary>
        DEL_FOLDERS = 0x04,

        /// <summary>
        /// The folder is hard deleted
        /// </summary>
        DELETE_HARD_DELETE = 0x10
    }
}