namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.13.1 SortOrder Structure
    /// The enum value of Order type.
    /// </summary>
    public enum OrderType : byte
    {
        /// <summary>
        /// Sort by this column in ascending order.
        /// </summary>
        Ascending = 0x00,

        /// <summary>
        /// Sort by this column in descending order.
        /// </summary>
        Descending = 0x01,

        /// <summary>
        /// This is an aggregated column in a categorized sort, whose maximum value (within the group of items with the same value as that of the previous category) is to be used as the sort key for the entire group.
        /// </summary>
        MaximumCategory = 0x04
    }
}
