using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.7 RopMoveFolder ROP
    /// A class indicates the RopMoveFolder ROP Response Buffer.
    /// </summary>
    public class RopMoveFolderResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request. 
        /// </summary>
        public BlockT<byte> SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public BlockT<uint> DestHandleIndex;

        /// <summary>
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public BlockT<bool> PartialCompletion;

        /// <summary>
        /// Parse the RopMoveFolderResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            SourceHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if ((AdditionalErrorCodes)ReturnValue.Data == AdditionalErrorCodes.NullDestinationObject)
            {
                DestHandleIndex = ParseT<uint>();
                PartialCompletion = ParseAs<byte, bool>();
            }
            else
            {
                PartialCompletion = ParseAs<byte, bool>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopMoveFolderResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
            AddChildBlockT(PartialCompletion, "PartialCompletion");
        }
    }
}