using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.7 RopTransportNewMail
    /// A class indicates the RopTransportNewMail ROP Request Buffer.
    /// </summary>
    public class RopTransportNewMailRequest : Block
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
        /// An identifier that specifies the new message object.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// An identifier that identifies the folder of the new message object.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies the message class of the new message object;
        /// </summary>
        public BlockString MessageClass;

        /// <summary>
        /// A flags structure that contains the message flags of the new message object.
        /// </summary>
        public BlockT<MessageFlags> MessageFlags;

        /// <summary>
        /// Parse the RopTransportNewMailRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            MessageId = Parse<MessageID>();
            FolderId = Parse<FolderID>();
            MessageClass = ParseStringA();
            MessageFlags = ParseT<MessageFlags>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopTransportNewMailRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(MessageId, "MessageId");
            AddChild(FolderId, "FolderId");
            AddChildString(MessageClass, "MessageClass");
            AddChildBlockT(MessageFlags, "MessageFlags");
        }
    }
}
