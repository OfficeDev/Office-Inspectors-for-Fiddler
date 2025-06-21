namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.13 Table Sorting Structures
    /// 2.13.1 SortOrder Structure
    /// </summary>
    public class SortOrder : Block
    {
        /// <summary>
        /// This value identifies the data type of the column to be used for sorting.
        /// </summary>
        public BlockT<PropertyDataType> PropertyType;

        /// <summary>
        /// This value identifies the column to be used for sorting.
        /// </summary>
        public BlockT<PidTagPropertyEnum> PropertyId;

        /// <summary>
        /// The order type.
        /// </summary>
        public BlockT<OrderType> Order;

        /// <summary>
        /// Parse the sortOrder structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            PropertyId = ParseT<PidTagPropertyEnum>();
            Order = ParseT<OrderType>();
        }

        protected override void ParseBlocks()
        {
            SetText("SortOrder");
            AddChildBlockT(PropertyType, "PropertyType");
            AddChildBlockT(PropertyId, "PropertyId");
            AddChildBlockT(Order, "Order");
        }
    }
}
