using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.17 RopWriteStreamExtended
    /// A class indicates the RopWriteStreamExtended ROP Response Buffer.
    /// </summary>
    public class RopWriteStreamExtendedResponse : Block
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
        /// An unsigned integer that specifies the number of bytes actually written.
        /// </summary>
        public BlockT<uint> WrittenSize;

        /// <summary>
        /// Parse the RopWriteStreamExtendedResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            WrittenSize = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopWriteStreamExtendedResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(WrittenSize, "WrittenSize");
        }
    }
}
