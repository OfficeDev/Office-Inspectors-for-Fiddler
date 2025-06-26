using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3.1 RopOpenMessage
    ///  A class indicates the RopOpenMessage ROP Request Buffer.
    /// </summary>
    public class RopOpenMessageRequest : Block
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
        /// An identifier that specifies which code page will be used for string values associated with the message.
        /// </summary>
        public BlockT<short> CodePageId;

        /// <summary>
        /// An identifier that identifies the parent folder of the message to be opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A flags structure that contains flags that control the access to the message.
        /// </summary>
        public BlockT<OpenMessageModeFlags> OpenModeFlags;

        /// <summary>
        /// An identifier that identifies the message to be opened.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopOpenMessageRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            CodePageId = ParseT<short>();
            FolderId = Parse<FolderID>();
            OpenModeFlags = ParseT<OpenMessageModeFlags>();
            MessageId = Parse<MessageID>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopOpenMessageRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChild(FolderId, "FolderId");
            AddChildBlockT(OpenModeFlags, "OpenModeFlags");
            AddChild(MessageId, "MessageId");
        }
    }
}
