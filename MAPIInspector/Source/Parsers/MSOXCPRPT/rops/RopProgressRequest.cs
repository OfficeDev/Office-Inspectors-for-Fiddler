namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    ///  2.2.2.23 RopProgress
    ///  A class indicates the RopProgress ROP Request Buffer.
    /// </summary>
    public class RopProgressRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether to cancel the operation.
        /// </summary>
        public BlockT<bool> WantCancel;

        /// <summary>
        /// Parse the RopProgressRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            WantCancel = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopProgressRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(WantCancel, "WantCancel");
        }
    }
}
