using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.9 RopSeekRow ROP
    /// The RopSeekRow ROP ([MS-OXCROPS] section 2.2.5.8) moves the table cursor to a specific location in the table. 
    /// </summary>
    public class RopSeekRowRequest : Block
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
        /// An enumeration that specifies the origin of this seek operation. 
        /// </summary>
        public BlockT<Bookmarks> Origin;

        /// <summary>
        /// A signed integer that specifies the direction and the number of rows to seek.
        /// </summary>
        public BlockT<int> RowCount;

        /// <summary>
        /// A Boolean that specifies whether the server returns the actual number of rows moved in the response.
        /// </summary>
        public BlockT<bool> WantRowMovedCount;

        /// <summary>
        /// Parse the RopSeekRowRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowRequest structure.</param>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            Origin = ParseT<Bookmarks>();
            RowCount = ParseT<int>();
            WantRowMovedCount = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSeekRowRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(Origin, "Origin");
            AddChildBlockT(RowCount, "RowCount");
            AddChildBlockT(WantRowMovedCount, "WantRowMovedCount");
        }
    }
}
