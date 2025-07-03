using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1.2 ActionData Structure
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure for Extended Rules
    /// </summary>
    public class OP_MOVE_and_OP_COPY_ActionData_forExtended : Block
    {
        /// <summary>
        /// An integer that specifies the size, in bytes, of the StoreEID field.
        /// </summary>
        public BlockT<uint> StoreEIDSize;

        /// <summary>
        /// This field is not used and can be set to any non-null value by the client and the server. 
        /// </summary>
        public BlockBytes StoreEID;

        /// <summary>
        /// An integer that specifies the size, in bytes, of the FolderEID field.
        /// </summary>
        public BlockT<uint> FolderEIDSize;

        /// <summary>
        /// A Folder EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.1, that identifies the destination folder. 
        /// </summary>
        public FolderEntryID FolderEID;

        /// <summary>
        /// Parse the OP_MOVE_and_OP_COPY_ActionData_forExtended structure.
        /// </summary>
        protected override void Parse()
        {
            StoreEIDSize = ParseAs<byte, uint>();
            StoreEID = ParseBytes(StoreEIDSize);
            FolderEIDSize = ParseAs<byte, uint>();
            FolderEID = Parse<FolderEntryID>();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(StoreEIDSize, "StoreEIDSize");
            AddChildBytes(StoreEID, "StoreEID");
            AddChildBlockT(FolderEIDSize, "FolderEIDSize");
            AddChild(FolderEID, "FolderEID");
        }
    }
}
