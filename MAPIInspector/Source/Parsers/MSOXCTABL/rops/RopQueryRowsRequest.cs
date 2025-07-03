using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.5 RopQueryRows ROP
    /// The RopQueryRows ROP ([MS-OXCROPS] section 2.2.5.4) returns zero or more rows from a table, beginning from the current table cursor position.
    /// </summary>
    public class RopQueryRowsRequest : Block
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
        public BlockT<QueryRowsFlags> QueryRowsFlags;

        /// <summary>
        /// A Boolean that specifies the direction to read rows.
        /// </summary>
        public BlockT<bool> ForwardRead;

        /// <summary>
        /// An unsigned integer that specifies the number of requested rows.
        /// </summary>
        public BlockT<ushort> RowCount;

        /// <summary>
        /// Parse the RopQueryRowsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryRowsRequest structure.</param>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            QueryRowsFlags = ParseT<QueryRowsFlags>();
            ForwardRead = ParseAs<byte, bool>();
            RowCount = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopQueryRowsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(QueryRowsFlags, "QueryRowsFlags");
            AddChildBlockT(ForwardRead, "ForwardRead");
            AddChildBlockT(RowCount, "RowCount");
        }
    }
}
