using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.11.1 RopCreateBookmark ROP Request Buffer
    /// The RopCreateBookmark ROP ([MS-OXCROPS] section 2.2.5.11) creates a new bookmark at the current cursor position in the table.
    /// </summary>
    public class RopCreateBookmarkRequest : Block
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
        /// Parse the RopCreateBookmarkRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopCreateBookmarkRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
        }
    }
}
