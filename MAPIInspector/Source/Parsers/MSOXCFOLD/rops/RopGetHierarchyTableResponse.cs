using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.13 RopGetHierarchyTable ROP
    /// A class indicates the RopGetHierarchyTable ROP Response Buffer.
    /// </summary>
    public class RopGetHierarchyTableResponse : Block
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
        /// An unsigned integer that represents the number of rows in the hierarchy table. 
        /// </summary>
        public BlockT<uint> RowCount;

        /// <summary>
        /// Parse the RopGetHierarchyTableResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue.Data == ErrorCodes.Success)
            {
                RowCount = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetHierarchyTableResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(RowCount, "RowCount");
        }
    }
}