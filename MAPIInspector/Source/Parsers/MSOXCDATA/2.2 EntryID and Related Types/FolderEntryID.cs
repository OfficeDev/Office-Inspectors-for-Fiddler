namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2.4 Messaging Object EntryIDs Structures
    /// 2.2.4.1 Folder EntryID Structure
    /// </summary>
    public class FolderEntryID : Block
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
        public BlockT<StoreObjectType> FolderType;

        /// <summary>
        /// A GUID associated with the Store object and corresponding to the ReplicaId field of the Folder ID structure.
        /// </summary>
        public BlockGuid DatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the folder. 6 bytes
        /// </summary>
        public BlockBytes GlobalCounter;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public BlockT<ushort> Pad;

        /// <summary>
        /// Parse the FolderEntryID structure.
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
            // byte[] verifyProviderUID = { 0x1A, 0x44, 0x73, 0x90, 0xAA, 0x66, 0x11, 0xCD, 0x9B, 0xC8, 0x00, 0xAA, 0x00, 0x2F, 0xC4, 0x5A };

            FolderType = ParseT<StoreObjectType>();
            DatabaseGuid = Parse<BlockGuid>();
            GlobalCounter = ParseBytes(6);
            Pad = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Flags, "Flags");
            this.AddChildGuid(ProviderUID, "ProviderUID}");
            AddChildBlockT(FolderType, "FolderType");
            this.AddChildGuid(DatabaseGuid, "DatabaseGuid");
            if (GlobalCounter != null) AddChild(GlobalCounter, $"GlobalCounter :{GlobalCounter.ToHexString(false)}");
            AddChildBlockT(Pad, "Pad");
        }
    }
}
