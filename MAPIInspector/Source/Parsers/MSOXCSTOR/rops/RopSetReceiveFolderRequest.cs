using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.3.3.1 RopSetReceiveFolder ROP Request Buffer
    /// A class indicates the RopSetReceiveFolder ROP Request Buffer.
    /// </summary>
    public class RopSetReceiveFolderRequest : Block
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
        /// An identifier that specifies the Receive folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies which message class to set the Receive folder for.
        /// </summary>
        public BlockString MessageClass;

        /// <summary>
        /// Parse the RopSetReceiveFolderRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            FolderId = Parse<FolderID>();
            MessageClass = ParseStringA();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSetReceiveFolderRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(FolderId, "FolderId");
            AddChildString(MessageClass, "MessageClass");
        }
    }
}
