namespace MAPIInspector.Parsers
{
    /// <summary>
    /// This enumeration is used to specify CopyFlags for destination configure.
    /// </summary>
    public enum CopyFlags_DestinationConfigure : byte
    {
        /// <summary>
        /// If this flag is set, the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x01,
    }
}
