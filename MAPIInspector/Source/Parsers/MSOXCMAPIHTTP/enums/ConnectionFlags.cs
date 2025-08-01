namespace MAPIInspector.Parsers
{
    /// <summary>
    /// ConnectionFlags designating the mode of operation.
    /// </summary>
    public enum ConnectionFlags : uint
    {
        /// <summary>
        /// Client running cached mode
        /// </summary>
        Clientisrunningincachedmode = 0x0001,

        /// <summary>
        /// Client is not designating mode of operation
        /// </summary>
        Clientisnotdesignatingamodeofoperation = 0x0000,
    }
}
