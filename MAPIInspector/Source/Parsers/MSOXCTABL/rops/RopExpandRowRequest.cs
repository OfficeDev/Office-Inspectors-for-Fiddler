using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.17 RopExpandRow ROP
    /// The RopExpandRow ROP ([MS-OXCROPS] section 2.2.5.16) expands a collapsed category of a table and returns the rows that belong in the newly expanded category.
    /// </summary>
    public class RopExpandRowRequest : Block
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
        /// An unsigned integer that specifies the maximum number of expanded rows to return data for.
        /// </summary>
        BlockT<ushort> MaxRowCount;

        /// <summary>
        /// An identifier that specifies the category to be expanded.
        /// </summary>
        BlockT<long> CategoryId;

        /// <summary>
        /// Parse the RopExpandRowRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            MaxRowCount = ParseT<ushort>();
            CategoryId = ParseT<long>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopExpandRowRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(MaxRowCount, "MaxRowCount");
            AddChildBlockT(CategoryId, "CategoryId");
        }
    }
}
