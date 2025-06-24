namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2.6.11 RopSetMessageReadFlag ROP
    /// A class indicates the RopSetMessageReadFlag ROP response Buffer.
    /// </summary>
    public class RopSetMessageReadFlagResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the read status of a public folder's message has changed.
        /// </summary>
        public BlockT<bool> ReadStatusChanged;

        /// <summary>
        /// An unsigned integer index that is present when the value in the ReadStatusChanged field is nonzero and is not present
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An array of bytes that is present when the value in the ReadStatusChanged field is nonzero and is not present
        /// </summary>
        public BlockBytes ClientData;

        /// <summary>
        /// Parse the RopSetMessageReadFlagResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            ResponseHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue.Data == ErrorCodes.Success)
            {
                ReadStatusChanged = ParseAs<byte, bool>();

                if (ReadStatusChanged.Data)
                {
                    LogonId = ParseT<byte>();
                    ClientData = ParseBytes(24);
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetMessageReadFlagResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(ResponseHandleIndex, "ResponseHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(ReadStatusChanged, "ReadStatusChanged");
            AddChildBlockT(LogonId, "LogonId");
            AddChild(ClientData, "ClientData");
        }
    }
}
