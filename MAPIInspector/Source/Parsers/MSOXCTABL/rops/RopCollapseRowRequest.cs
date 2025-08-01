using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.17.1 RopCollapseRow ROP Request Buffer
    /// The RopCollapseRow ROP ([MS-OXCROPS] section 2.2.5.17) collapses an expanded category.
    /// </summary>
    public class RopCollapseRowRequest : Block
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
        /// An identifier that specifies the category to be collapsed.
        /// </summary>
        BlockT<long> CategoryId;

        /// <summary>
        /// Parse the RopCollapseRowRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            CategoryId = ParseT<long>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopCollapseRowRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(CategoryId, "CategoryId");
        }
    }
}
