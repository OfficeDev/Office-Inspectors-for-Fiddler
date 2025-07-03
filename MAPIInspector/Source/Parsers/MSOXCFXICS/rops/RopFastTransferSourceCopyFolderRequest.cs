using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopFastTransferSourceCopyFolder ROP Request Buffer.
    /// 2.2.3.1.1.4.1 RopFastTransferSourceCopyFolder ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceCopyFolderRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation.
        /// </summary>
        public BlockT<CopyFlags_CopyFolder> CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation.
        /// </summary>
        public BlockT<SendOptions> SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyFolderRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            CopyFlags = ParseT<CopyFlags_CopyFolder>();
            SendOptions = ParseT<SendOptions>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferSourceCopyFolderRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(CopyFlags, "CopyFlags");
            AddChildBlockT(SendOptions, "SendOptions");
        }
    }
}
