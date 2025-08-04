using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.9.8.2 RopSeekStream ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.9.8.3 RopSeekStream ROP Failure Response Buffer
    /// A class indicates the RopSeekStream ROP Response Buffer.
    /// </summary>
    public class RopSeekStreamResponse : Block
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
        /// An unsigned integer that represents the new position in the stream after the operation.
        /// </summary>
        public BlockT<ulong> NewPosition;

        /// <summary>
        /// Parse the RopSeekStreamResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                NewPosition = ParseT<ulong>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopSeekStreamResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(NewPosition, "NewPosition");
        }
    }
}
