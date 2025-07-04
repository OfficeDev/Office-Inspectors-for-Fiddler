using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.8 RopQueryPosition ROP
    /// A class indicates the  RopQueryPosition ROP Response Buffer.
    /// </summary>
    public class RopQueryPositionResponse : Block
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
        /// An unsigned integer that indicates the index (0-based) of the current row.
        /// </summary>
        BlockT<uint> Numerator;

        /// <summary>
        /// An unsigned integer that indicates the total number of rows in the table.
        /// </summary>
        BlockT<uint> Denominator;

        /// <summary>
        /// Parse the RopQueryPositionResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                Numerator = ParseT<uint>();
                Denominator = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopQueryPositionResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(Numerator, "Numerator");
            AddChildBlockT(Denominator, "Denominator");
        }
    }
}
