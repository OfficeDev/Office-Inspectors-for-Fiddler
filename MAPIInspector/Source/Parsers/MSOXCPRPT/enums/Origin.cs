namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that specifies the origin location for the seek operation.
    /// </summary>
    public enum Origin : byte
    {
        /// <summary>
        /// The point of origin is the beginning of the stream.
        /// </summary>
        Beginning = 0x00,

        /// <summary>
        /// The point of origin is the location of the current seek pointer.
        /// </summary>
        Current = 0x01,

        /// <summary>
        /// The point of origin is the end of the stream.
        /// </summary>
        End = 0x02
    }
}
