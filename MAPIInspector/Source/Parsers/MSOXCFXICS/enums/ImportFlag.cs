namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An 8-bit flag structure that defines the parameters of the import operation.
    /// 2.2.3.2.4.2 RopSynchronizationImportMessageChange
    /// </summary>
    public enum ImportFlag : byte
    {
        /// <summary>
        /// If this flag is set, the message being imported is an FAI message
        /// </summary>
        Associated = 0x10,

        /// <summary>
        /// If this flag is set, the server accepts conflicting versions of a particular message
        /// </summary>
        FailOnConflict = 0x40,
    }
}
