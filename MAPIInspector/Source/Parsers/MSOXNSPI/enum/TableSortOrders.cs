namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.10 Table Sort Order
    /// The TableSortOrders enum type
    /// </summary>
    public enum TableSortOrders : uint
    {
        /// <summary>
        /// The table is sorted ascending on the PidTagDisplayName property
        /// </summary>
        SortTypeDisplayName = 0x00000000,

        /// <summary>
        /// The table is sorted ascending on the PidTagAddressBookPhoneticDisplayName property
        /// </summary>
        SortTypePhoneticDisplayName = 0x00000003,

        /// <summary>
        /// The table is sorted ascending on the PidTagDisplayName property
        /// </summary>
        SortTypeDisplayName_RO = 0x000003E8,

        /// <summary>
        /// The table is sorted ascending on the PidTagDisplayName property
        /// </summary>
        SortTypeDisplayName_W = 0x000003E9
    }
}
