using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.18 RopCollapseRow ROP
    /// A class indicates the RopCollapseRow ROP Response Buffer.
    /// </summary>
    public class RopCollapseRowResponse : Block
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
        /// An unsigned integer that specifies the total number of rows in the collapsed category.
        /// </summary>
        BlockT<uint> CollapsedRowCount;

        /// <summary>
        /// Parse the RopCollapseRowResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                CollapsedRowCount = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopCollapseRowResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(CollapsedRowCount, "CollapsedRowCount");
        }
    }
}
