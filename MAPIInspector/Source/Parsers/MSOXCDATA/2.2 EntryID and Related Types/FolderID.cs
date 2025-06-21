namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2 EntryID and Related Types
    /// 2.2.1 Folder ID, Message ID, and Global Identifier Structures
    /// 2.2.1.1 Folder ID Structure
    /// </summary>
    public class FolderID : Block
    {
        /// <summary>
        /// An unsigned integer identifying a Store object.
        /// </summary>
        public BlockT<ushort> ReplicaId;

        /// <summary>
        /// An unsigned integer identifying the folder within its Store object. 6 bytes
        /// </summary>
        public BlockBytes GlobalCounter;

        /// <summary>
        /// Parse the FolderID structure.
        /// </summary>
        protected override void Parse()
        {
            ReplicaId = ParseT<ushort>();
            GlobalCounter = ParseBytes(6);
        }

        protected override void ParseBlocks()
        {
            SetText("FolderID");
            AddChildBlockT(ReplicaId, "ReplicaId");
            if (GlobalCounter != null) AddChild(GlobalCounter, $"GlobalCounter:{GlobalCounter.ToHexString(false)}");
        }
    }
}
