using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.7 RopGetStatus ROP
    /// A class indicates the RopGetStatus ROP Response Buffer.
    /// </summary>
    public class RopGetStatusResponse : Block
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
        /// An enumeration that specifies the status of the table.
        /// </summary>
        public BlockT<TableStatus> TableStatus;

        /// <summary>
        /// Parse the RopGetStatusResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                TableStatus = ParseT<TableStatus>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetStatusResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(TableStatus, "TableStatus");
        }
    }
}
