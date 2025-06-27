namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum structure that contains an OR'ed combination. 
    /// </summary>
    public enum FindRowFlags : byte
    {
        /// <summary>
        /// Perform the find forwards.
        /// </summary>
        Forwards = 0x00,

        /// <summary>
        /// Perform the find backwards
        /// </summary>
        Backwards = 0x01
    }
}
