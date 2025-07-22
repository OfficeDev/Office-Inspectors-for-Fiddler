namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.8 Positioning Minimal Entry IDs
    /// The PositioningMinimalEntryIDs enum type
    /// </summary>
    public enum PositioningMinimalEntryIDs : uint
    {
        /// <summary>
        /// Specifies the position before the first row in the current address book container.
        /// </summary>
        MID_BEGINNING_OF_TABLE = 0x00000000,

        /// <summary>
        /// Specifies the position after the last row in the current address book container
        /// </summary>
        MID_END_OF_TABLE = 0x00000002,

        /// <summary>
        /// Specifies the current position in a table.
        /// </summary>
        MID_CURRENT = 0x00000001
    }
}
