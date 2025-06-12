namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An flag structure that defines the parameters of the import operation.
    /// </summary>
    public enum ImportDeleteFlags : byte
    {
        /// <summary>
        /// If this flag is set, folder deletions are being imported.
        /// </summary>
        Hierarchy = 0x01,

        /// <summary>
        /// If this flag is set, hard deletions are being imported
        /// </summary>
        HardDelete = 0x02,
    }
}
