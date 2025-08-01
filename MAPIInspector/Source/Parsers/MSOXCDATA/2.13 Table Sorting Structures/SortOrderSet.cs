using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.13.2 SortOrderSet Structure
    /// </summary>
    public class SortOrderSet : Block
    {
        /// <summary>
        /// An unsigned integer. This value specifies how many sortOrder structures are present in the SortOrders field.
        /// </summary>
        public BlockT<ushort> SortOrderCount;

        /// <summary>
        /// An unsigned integer. This value specifies that the first CategorizedCount columns are categorized.
        /// </summary>
        public BlockT<ushort> CategorizedCount;

        /// <summary>
        /// An unsigned integer. This value specifies that the first ExpandedCount field in the categorized columns starts in an expanded state in which all of the rows that apply to the category are visible in the table view.
        /// </summary>
        public BlockT<ushort> ExpandedCount;

        /// <summary>
        /// An array of sortOrder structures. This field MUST contain the number of structures indicated by the value of the SortOrderCount field.
        /// </summary>
        public SortOrder[] SortOrders;

        /// <summary>
        /// Parse the SortOrderSet structure.
        /// </summary>
        /// <param name="s">A stream containing the SortOrderSet structure</param>
        protected override void Parse()
        {
            SortOrderCount = ParseT<ushort>();
            CategorizedCount = ParseT<ushort>();
            ExpandedCount = ParseT<ushort>();
            var tempSortOrders = new List<SortOrder>();
            for (int i = 0; i < SortOrderCount; i++)
            {
                tempSortOrders.Add(Parse<SortOrder>());
            }

            SortOrders = tempSortOrders.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "SortOrderSet";
            AddChildBlockT(SortOrderCount, "SortOrderCount");
            AddChildBlockT(CategorizedCount, "CategorizedCount");
            AddChildBlockT(ExpandedCount, "ExpandedCount");
            AddLabeledChildren(SortOrders, "SortOrders");
        }
    }
}
