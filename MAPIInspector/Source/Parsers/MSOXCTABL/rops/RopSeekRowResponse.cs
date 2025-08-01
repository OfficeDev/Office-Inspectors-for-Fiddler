using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.10.2 RopSeekRowFractional ROP Response Buffer
    /// A class indicates the RopSeekRow ROP Response Buffer.
    /// </summary>
    public class RopSeekRowResponse : Block
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
        /// A Boolean that specifies whether the full number of rows sought past was less than the number that was requested.
        /// </summary>
        public BlockT<bool> HasSoughtLess;

        /// <summary>
        /// A signed integer that specifies the direction and number of rows sought.
        /// </summary>
        public BlockT<int> RowsSought;

        /// <summary>
        /// Parse the RopSeekRowResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                HasSoughtLess = ParseAs<byte, bool>();
                RowsSought = ParseT<int>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopSeekRowResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(HasSoughtLess, "HasSoughtLess");
            AddChildBlockT(RowsSought, "RowsSought");
        }
    }
}
