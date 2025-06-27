using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.8 RopLongTermIdFromId
    /// A class indicates the RopLongTermIdFromId ROP Response Buffer.
    /// </summary>
    public class RopLongTermIdFromIdResponse : Block
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
        /// A LongTermID structure that specifies the long-term ID that was converted from the short-term ID, which is specified in the ObjectId field of the request.
        /// </summary>
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopLongTermIdFromIdResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                LongTermId = Parse<LongTermID>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopLongTermIdFromIdResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChild(LongTermId, "LongTermId");
        }
    }
}
