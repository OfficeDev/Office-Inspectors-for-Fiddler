using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.14 RopFindRow ROP
    /// The RopFindRow ROP ([MS-OXCROPS] section 2.2.5.13) returns the next row in a table that matches the search criteria and moves the cursor to that row.
    /// </summary>
    public class RopFindRowRequest : Block
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
        public BlockT<FindRowFlags> FindRowFlags;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        BlockT<ushort> RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this operation.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An enumeration that specifies where this operation begins its search.
        /// </summary>
        public BlockT<Bookmarks> Origin;

        /// <summary>
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        BlockT<ushort> BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the bookmark to use as the origin.
        /// </summary>
        public BlockBytes Bookmark;

        /// <summary>
        /// Parse the RopFindRow structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            FindRowFlags = ParseT<FindRowFlags>();
            RestrictionDataSize = ParseT<ushort>();
            if (RestrictionDataSize > 0)
            {
                RestrictionData = new RestrictionType();
                RestrictionData.Parse(parser);
            }

            Origin = ParseT<Bookmarks>();
            BookmarkSize = ParseT<ushort>();
            Bookmark = ParseBytes(BookmarkSize);
        }

        protected override void ParseBlocks()
        {
            SetText("RopFindRowRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(FindRowFlags, "FindRowFlags");
            AddChildBlockT(RestrictionDataSize, "RestrictionDataSize");
            AddChild(RestrictionData, "RestrictionData");
            AddChildBlockT(Origin, "Origin");
            AddChildBlockT(BookmarkSize, "BookmarkSize");
            AddChildBytes(Bookmark, "Bookmark");
        }
    }
}
