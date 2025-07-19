using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.19 RopGetCollapseState ROP
    /// A class indicates the RopGetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopGetCollapseStateResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the size of the CollapseState field.
        /// </summary>
        public BlockT<ushort> CollapseStateSize;

        /// <summary>
        /// An array of bytes that specifies a collapse state for a categorized table.
        /// </summary>
        public BlockBytes CollapseState;

        /// <summary>
        /// Parse the RopGetCollapseStateResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                CollapseStateSize = ParseT<ushort>();
                CollapseState = ParseBytes((int)CollapseStateSize);
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetCollapseStateResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(CollapseStateSize, "CollapseStateSize");
            AddChildBytes(CollapseState, "CollapseState");
        }
    }
}
