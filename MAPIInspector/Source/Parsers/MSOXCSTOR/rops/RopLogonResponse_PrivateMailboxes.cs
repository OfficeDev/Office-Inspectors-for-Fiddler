using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCSTOR] 2.2.3.1.2 RopLogon ROP Success Response Buffer for Private Mailboxes
    /// A class indicates the RopLogon ROP Response Buffer for private mailbox.
    /// </summary>
    public class RopLogonResponse_PrivateMailboxes : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// A flags structure that contains flags that specify the type of RopLogon.
        /// </summary>
        public BlockT<LogonFlags> LogonFlags;

        /// <summary>
        /// 13 64-bit identifiers that specify a set of special folders for a mailbox.
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// A flags structure that contains flags that provide details about the state of the mailbox.
        /// </summary>
        public BlockT<ResponseFlags> ResponseFlags;

        /// <summary>
        /// A GUID that identifies the mailbox on which the RopLogon was performed.
        /// </summary>
        public BlockGuid MailboxGuid;

        /// <summary>
        /// An identifier that specifies a replica ID for the RopLogon.
        /// </summary>
        public BlockT<ushort> ReplId;

        /// <summary>
        /// A GUID that specifies the replica GUID that is associated with the replica ID.
        /// </summary>
        public BlockGuid ReplGuid;

        /// <summary>
        /// A LogonTime structure that specifies the time at which the RopLogon occurred.
        /// </summary>
        public LogonTime LogonTime;

        /// <summary>
        /// An unsigned integer that contains a numeric value that tracks the currency of the Gateway Address Routing Table (GWART).
        /// </summary>
        public BlockT<ulong> GwartTime;

        /// <summary>
        /// A flags structure.
        /// </summary>
        public BlockT<uint> StoreState;

        /// <summary>
        /// The below two fields is defined for RopLogon redirect response in section 2.2.3.1.4 in MS-OXCROPS.
        /// An unsigned integer that specifies the length of the ServerName field.
        /// </summary>
        public BlockT<byte> ServerNameSize;

        /// <summary>
        /// A null-terminated ASCII string that specifies a different server for the client to connect to.
        /// </summary>
        public BlockString ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PrivateMailboxes structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                LogonFlags = ParseT<LogonFlags>();
                FolderIds = new FolderID[13];
                for (int i = 0; i < 13; i++)
                {
                    FolderIds[i] = Parse<FolderID>();
                }

                ResponseFlags = ParseT<ResponseFlags>();
                MailboxGuid = Parse<BlockGuid>();
                ReplId = ParseT<ushort>();
                ReplGuid = Parse<BlockGuid>();
                LogonTime = Parse<LogonTime>();
                GwartTime = ParseT<ulong>();
                StoreState = ParseT<uint>();
            }
            else if ((AdditionalErrorCodes)ReturnValue.Data == AdditionalErrorCodes.WrongServer)
            {
                LogonFlags = ParseT<LogonFlags>();
                ServerNameSize = ParseT<byte>();
                ServerName = ParseStringA();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopLogonResponse_PrivateMailboxes";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(LogonFlags, "LogonFlags");
            AddLabeledChildren(FolderIds, "FolderIds"); // TODO Interpert which folder is which
            AddChildBlockT(LogonFlags, "ResponseFlags");
            this.AddChildGuid(MailboxGuid, "MailboxGuid");
            AddChildBlockT(ReplId, "ReplId");
            this.AddChildGuid(ReplGuid, "ReplGuid");
            AddChild(LogonTime, LogonTime.Text);
            AddChildBlockT(GwartTime, "GwartTime");
            AddChildBlockT(StoreState, "StoreState");
            AddChildBlockT(ServerNameSize, "ServerNameSize");
            AddChildString(ServerName, "ServerName");
        }
    }
}
