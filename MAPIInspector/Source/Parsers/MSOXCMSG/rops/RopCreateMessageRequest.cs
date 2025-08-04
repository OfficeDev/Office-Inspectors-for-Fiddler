using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.6.2 RopCreateMessage
    /// A class indicates the RopCreateMessage ROP request Buffer.
    /// </summary>
    public class RopCreateMessageRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
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
        /// An identifier that specifies the code page for the message.
        /// </summary>
        public BlockT<ushort> CodePageId;

        /// <summary>
        /// An identifier that specifies the parent folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A Boolean that specifies whether the message is an FAI message.
        /// </summary>
        public BlockT<bool> AssociatedFlag;

        /// <summary>
        /// Parse the RopCreateMessageRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            CodePageId = ParseT<ushort>();
            FolderId = Parse<FolderID>();
            AssociatedFlag = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopCreateMessageRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(CodePageId, "CodePageId");
            AddChild(FolderId, "FolderId");
            AddChildBlockT(AssociatedFlag, "AssociatedFlag");
        }
    }
}
