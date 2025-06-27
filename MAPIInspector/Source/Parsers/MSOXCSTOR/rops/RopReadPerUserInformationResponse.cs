using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.12 RopReadPerUserInformation
    /// A class indicates the RopReadPerUserInformation ROP Response Buffer.
    /// </summary>
    public class RopReadPerUserInformationResponse : Block
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
        /// A Boolean that specifies whether this operation reached the end of the per-user information stream.
        /// </summary>
        public BlockT<bool> HasFinished;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public BlockT<ushort> DataSize;

        /// <summary>
        /// An array of bytes. This field contains the per-user data that is returned.
        /// </summary>
        public BlockBytes Data;

        /// <summary>
        /// Parse the RopReadPerUserInformationResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                HasFinished = ParseAs<byte, bool>();
                DataSize = ParseT<ushort>();
                Data = ParseBytes(DataSize);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopReadPerUserInformationResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(HasFinished, "HasFinished");
            AddChildBlockT(DataSize, "DataSize");
            AddChildBytes(Data, "Data");
        }
    }
}
