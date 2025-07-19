using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.9 RopIdFromLongTermId
    /// A class indicates the RopIdFromLongTermId ROP Response Buffer.
    /// </summary>
    public class RopIdFromLongTermIdResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An identifier that specifies the short-term ID that was converted from the long-term ID, which is specified in the LongTermId field of the request.
        /// </summary>
        public BlockBytes ObjectId;

        /// <summary>
        /// Parse the RopIdFromLongTermIdResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                ObjectId = ParseBytes(8);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopIdFromLongTermIdResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBytes(ObjectId, "ObjectId");
        }
    }
}
