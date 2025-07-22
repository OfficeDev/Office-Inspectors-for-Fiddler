namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum value of TableEvent type.
    /// </summary>
    public enum TableEventTypeEnum : ushort
    {
        /// <summary>
        /// The notification is for TableChanged events
        /// </summary>
        TableChanged = 0x0001,

        /// <summary>
        /// The notification is for TableRowAdded events.
        /// </summary>
        TableRowAdded = 0x0003,

        /// <summary>
        /// The notification is for TableRowDeleted events.
        /// </summary>
        TableRowDeleted = 0x0004,

        /// <summary>
        /// The notification is for TableRowModified events.
        /// </summary>
        TableRowModified = 0x0005,

        /// <summary>
        /// The notification is for TableRestrictionChanged events
        /// </summary>
        TableRestrictionChanged = 0x0007
    }
}
