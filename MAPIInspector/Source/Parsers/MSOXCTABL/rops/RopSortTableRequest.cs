using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.2.1 RopSortTable ROP Request Buffer
    /// The RopSortTable ROP ([MS-OXCROPS] section 2.2.5.2) orders the rows of a contents table based on sort criteria.
    /// </summary>
    public class RopSortTableRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control this operation.
        /// </summary>
        public BlockT<AsynchronousFlags> SortTableFlags;

        /// <summary>
        /// An unsigned integer that specifies how many SortOrder structures are present in the SortOrders field.
        /// </summary>
        public BlockT<ushort> SortOrderCount;

        /// <summary>
        /// An unsigned integer that specifies the number of category SortOrder structures in the SortOrders field.
        /// </summary>
        public BlockT<ushort> CategoryCount;

        /// <summary>
        /// An unsigned integer that specifies the number of expanded categories in the SortOrders field.
        /// </summary>
        public BlockT<ushort> ExpandedCount;

        /// <summary>
        /// An array of SortOrder structures that specifies the sort order for the rows in the table.
        /// </summary>
        public SortOrder[] SortOrders;

        /// <summary>
        /// Parse the RopSortTableRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            SortTableFlags = ParseT<AsynchronousFlags>();
            SortOrderCount = ParseT<ushort>();
            CategoryCount = ParseT<ushort>();
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
            Text = "RopSortTableRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(SortTableFlags, "SortTableFlags");
            AddChildBlockT(SortOrderCount, "SortOrderCount");
            AddChildBlockT(CategoryCount, "CategoryCount");
            AddChildBlockT(ExpandedCount, "ExpandedCount");
            AddLabeledChildren(SortOrders, "SortOrders");
        }
    }
}
