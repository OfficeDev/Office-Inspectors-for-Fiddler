using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.23 RopProgress
    /// A class indicates the RopProgress ROP Response Buffer.
    /// </summary>
    public class RopProgressResponse : Block
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
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer that specifies the number of tasks completed.
        /// </summary>
        public BlockT<uint> CompletedTaskCount;

        /// <summary>
        /// An unsigned integer that specifies the total number of tasks.
        /// </summary>
        public BlockT<uint> TotalTaskCount;

        /// <summary>
        /// Parse the RopProgressResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                LogonId = ParseT<byte>();
                CompletedTaskCount = ParseT<uint>();
                TotalTaskCount = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopProgressResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(CompletedTaskCount, "CompletedTaskCount");
            AddChildBlockT(TotalTaskCount, "TotalTaskCount");
        }
    }
}
