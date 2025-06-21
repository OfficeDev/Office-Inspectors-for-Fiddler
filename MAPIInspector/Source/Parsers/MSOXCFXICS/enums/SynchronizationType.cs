namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that defines the type of synchronization requested.
    /// 2.2.3.2.1.1.1 RopSynchronizationConfigure ROP Request Buffer
    /// </summary>
    public enum SynchronizationType : byte
    {
        /// <summary>
        /// Indicates a content synchronization operation.
        /// </summary>
        Contents = 0x01,

        /// <summary>
        /// Indicates a hierarchy synchronization operation
        /// </summary>
        Hierarchy = 0x02,
    }
}
