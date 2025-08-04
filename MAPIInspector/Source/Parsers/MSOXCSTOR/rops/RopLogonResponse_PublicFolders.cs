using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCSTOR] 2.2.1.1.4 RopLogon ROP Success Response Buffer for Public Folders
    /// [MS-OXCROPS] 2.2.3.1.3 RopLogon ROP Success Response Buffer for Public Folders
    /// A class indicates the RopLogon ROP Response Buffer for public folders.
    /// </summary>
    public class RopLogonResponse_PublicFolders : Block
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
        /// An identifier that specifies a replica ID for the RopLogon.
        /// </summary>
        public BlockT<ushort> ReplId;

        /// <summary>
        /// A GUID that specifies the replica GUID associated with the replica ID that is specified in the ReplId field.
        /// </summary>
        public BlockGuid ReplGuid;

        /// <summary>
        /// This field is not used and is ignored by the client.
        /// </summary>
        public BlockGuid PerUserGuid;

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
        /// Parse the RopLogonResponse_PublicFolders structure.
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

                ReplId = ParseT<ushort>();
                ReplGuid = Parse<BlockGuid>();
                PerUserGuid = Parse<BlockGuid>();
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
            Text = "RopLogonResponse_PublicFolders";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(LogonFlags, "LogonFlags");
            AddLabeledChildren(FolderIds, "FolderIds");
            AddChildBlockT(LogonFlags, "ResponseFlags");
            AddChildBlockT(ReplId, "ReplId");
            this.AddChildGuid(ReplGuid, "ReplGuid");
            this.AddChildGuid(PerUserGuid, "PerUserGuid");
            AddChildBlockT(ServerNameSize, "ServerNameSize");
            AddChildString(ServerName, "ServerName");
        }
    }
}
