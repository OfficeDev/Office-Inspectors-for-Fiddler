using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure for Standard Rules
    /// </summary>
    public class OP_MOVE_and_OP_COPY_ActionData_forStandard : Block
    {
        /// <summary>
        /// A Boolean value that indicates whether the folder is in the user's mailbox or a different mailbox.
        /// </summary>
        public BlockT<bool> FolderInThisStore;

        /// <summary>
        /// An integer that specifies the size, in bytes, of the StoreEID field.
        /// </summary>
        public BlockT<ushort> StoreEIDSize;

        /// <summary>
        /// A Store Object EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.3, that identifies the message store.
        /// </summary>
        public BlockBytes StoreEID;

        /// <summary>
        /// An integer that specifies the size, in bytes, of the FolderEID field.
        /// </summary>
        public BlockT<ushort> FolderEIDSize;

        /// <summary>
        /// A structure that identifies the destination folder.
        /// </summary>
        public Block FolderEID;

        /// <summary>
        /// Parse the OP_MOVE_and_OP_COPY_ActionData_forStandard structure.
        /// </summary>
        protected override void Parse()
        {
            FolderInThisStore = ParseAs<byte, bool>();
            StoreEIDSize = ParseT<ushort>();

            // No matter the value of FolderInThisStore, the server tends to set StoreEIDSize to 0x0001.
            // So instead of parsing it, we'll just read StoreEIDSize bytes.
            StoreEID = ParseBytes(StoreEIDSize);

            FolderEIDSize = ParseT<ushort>();
            if (FolderInThisStore)
            {
                FolderEID = new ServerEid();
                FolderEID.Parse(parser);
            }
            else
            {
                FolderEID = ParseBytes(FolderEIDSize);
            }
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(FolderInThisStore, "FolderInThisStore");
            AddChildBlockT(StoreEIDSize, "StoreEIDSize");
            AddChildBytes(StoreEID, "StoreEID");
            AddChildBlockT(FolderEIDSize, "FolderEIDSize");
            AddChild(FolderEID, "FolderEID");
        }
    }
}
