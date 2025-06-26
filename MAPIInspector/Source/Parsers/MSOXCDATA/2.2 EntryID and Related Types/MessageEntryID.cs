using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4.2 Message EntryID Structure
    /// </summary>
    public class MessageEntryID : Block
    {
        /// <summary>
        /// This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// The value of this field is determined by where the folder is located.
        /// </summary>
        public BlockGuid ProviderUID;

        /// <summary>
        /// One of several Store object types specified in the table in section 2.2.4.
        /// </summary>
        public BlockT<StoreObjectType> MessageType;

        /// <summary>
        /// A GUID associated with the Store object of the folder in which the message resides and corresponding to the ReplicaId field in the folder ID structure, as specified in section 2.2.1.1.
        /// </summary>
        public BlockGuid FolderDatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the folder in which the message resides. 6 bytes
        /// </summary>
        public BlockBytes FolderGlobalCounter;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public BlockT<ushort> Pad1;

        /// <summary>
        /// A GUID associated with the Store object of the message and corresponding to the ReplicaId field of the Message ID structure, as specified in section 2.2.1.2.
        /// </summary>
        public BlockGuid MessageDatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the message. 6 bytes
        /// </summary>
        public BlockBytes MessageGlobalCounter;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public BlockT<ushort> Pad2;

        /// <summary>
        /// Parse the MessageEntryID structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            ProviderUID = Parse<BlockGuid>();
            // Original implementation looked for this but didn't appear to do anything with it.
            // Provider UID (16 bytes): The value of this field is determined by where the folder is located. For a folder in a
            // private mailbox, this value MUST be set to value of the MailboxGuid field from the RopLogon ROP response buffer
            // ([MS-OXCROPS] section 2.2.3.1.2). For a folder in the public message store, this value MUST be set to
            // %x1A.44.73.90.AA.66.11.CD.9B.C8.00.AA.00.2F.C4.5A.
            // if (tempProviderUID.ToString() == "%x1A.44.73.90.AA.66.11.CD.9B.C8.00.AA.00.2F.C4.5A")

            MessageType = ParseT<StoreObjectType>();
            FolderDatabaseGuid = Parse<BlockGuid>();
            FolderGlobalCounter = ParseBytes(6);
            Pad1 = ParseT<ushort>();
            MessageDatabaseGuid = Parse<BlockGuid>();
            MessageGlobalCounter = ParseBytes(6);
            Pad2 = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Flags, "Flags");
            this.AddChildGuid(ProviderUID, "ProviderUID");
            AddChildBlockT(MessageType, "MessageType");
            this.AddChildGuid(FolderDatabaseGuid, "FolderDatabaseGuid");
            AddChildBytes(FolderGlobalCounter, "FolderGlobalCounter");
            AddChildBlockT(Pad1, "Pad1");
            this.AddChildGuid(MessageDatabaseGuid, "MessageDatabaseGuid");
            AddChildBytes(MessageGlobalCounter, "MessageGlobalCounter");
            AddChildBlockT(Pad2, "Pad2");
        }
    }
}
