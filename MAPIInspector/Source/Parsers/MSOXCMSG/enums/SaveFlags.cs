namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3.3.1 RopSaveChangesMessage ROP Request Buffer
    /// The enum value of SaveFlags that contains flags that specify how the save operation behaves.
    /// </summary>
    public enum SaveFlags : byte
    {
        /// <summary>
        /// Keeps the Message object open with read-only access
        /// </summary>
        KeepOpenReadOnly = 0x01,

        /// <summary>
        /// Keeps the Message object open with read/write access
        /// </summary>
        KeepOpenReadWrite = 0x02,

        /// <summary>
        /// Keeps the Message object open with read/write access. The ecObjectModified error code is not valid when this flag is set; the server overwrites any changes instead
        /// </summary>
        ForceSave = 0x04
    }
}
