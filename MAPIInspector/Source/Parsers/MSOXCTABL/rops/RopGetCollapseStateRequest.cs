using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.18.1 RopGetCollapseState ROP Request Buffer
    /// The RopGetCollapseState ROP ([MS-OXCROPS] section 2.2.5.18) returns the data necessary to rebuild the current expanded/collapsed state of the table.
    /// </summary>
    public class RopGetCollapseStateRequest : Block
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
        /// An identifier that specifies the row to be preserved as the cursor.
        /// </summary>
        BlockT<long> RowId;

        /// <summary>
        /// An unsigned integer that specifies the instance number of the row that is to be preserved as the cursor.
        /// </summary>
        BlockT<uint> RowInstanceNumber;

        /// <summary>
        /// Parse the RopGetCollapseStateRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            RowId = ParseT<long>();
            RowInstanceNumber = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetCollapseStateRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(RowId, "RowId");
            AddChildBlockT(RowInstanceNumber, "RowInstanceNumber");
        }
    }
}
