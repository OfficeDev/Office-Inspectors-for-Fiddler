namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2.1.2.1 RopRegisterNotification ROP
    /// The RopRegisterNotification ROP ([MS-OXCROPS] section 2.2.14.1) creates a subscription for specified notifications on the server and returns a handle of the subscription to the client.
    /// </summary>
    public class RopRegisterNotificationRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x29.
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
        /// A flags structure that contains flags that specify the types of events to register for.
        /// </summary>
        public BlockT<NotificationTypesEnum> NotificationTypes;

        /// <summary>
        /// A flags structure.
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// A Boolean that specifies whether the notification is scoped to the mailbox instead of a specific folder or message.
        /// </summary>
        public BlockT<bool> WantWholeStore;

        /// <summary>
        /// This value specifies the folder to register notifications for
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// This value specifies the message to register notifications for.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopRegisterNotificationRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            NotificationTypes = ParseT<NotificationTypesEnum>();

            if (NotificationTypes.Data == NotificationTypesEnum.Extended)
            {
                Reserved = ParseT<byte>();
            }

            WantWholeStore = ParseAs<byte, bool>();

            if (!WantWholeStore.Data)
            {
                FolderId = Parse<FolderID>();
                MessageId = Parse<MessageID>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopPendingResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(NotificationTypes, "NotificationTypes");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(WantWholeStore, "WantWholeStore");
            AddChild(FolderId, "FolderId");
            AddChild(MessageId, "MessageId");
        }
    }
}
