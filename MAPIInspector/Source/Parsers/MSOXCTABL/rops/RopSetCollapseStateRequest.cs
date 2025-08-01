using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.19.1 RopSetCollapseState ROP Request Buffer
    /// The following descriptions define valid fields for the RopSetCollapseState ROP request buffer ([MS-OXCROPS] section 2.2.5.19.1).
    /// </summary>
    public class RopSetCollapseStateRequest : Block
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
        /// An unsigned integer that specifies the size of the CollapseState field.
        /// </summary>
        BlockT<ushort> CollapseStateSize;

        /// <summary>
        /// An array of bytes that specifies a collapse state for a categorized table. The size of this field, in bytes, is specified by the CollapseStateSize field.
        /// </summary>
        public BlockBytes CollapseState;

        /// <summary>
        /// Parse the RopSetCollapseStateRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            CollapseStateSize = ParseT<ushort>();
            CollapseState = ParseBytes(CollapseStateSize);
        }

        protected override void ParseBlocks()
        {
            Text = "RopSetCollapseStateRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(CollapseStateSize, "CollapseStateSize");
            AddChildBytes(CollapseState, "CollapseState");
        }
    }
}
