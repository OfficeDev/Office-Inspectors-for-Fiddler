using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1.2.1.1 ServerEid Structure
    /// This type is specified in MS-OXORULE Section 2.2.5.1.2.1.1 ServerEid Structure
    /// </summary>
    public class ServerEid : Block
    {
        /// <summary>
        /// The value 0x01 indicates that the remaining bytes conform to this structure;
        /// </summary>
        public BlockT<bool> Ours;

        /// <summary>
        /// A Folder ID structure, as specified in [MS-OXCDATA] section 2.2.1.1, that identifies the destination folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// This field is not used and MUST be set to all zeros.
        /// </summary>
        public BlockT<ulong> MessageId;

        /// <summary>
        /// This field is not used and MUST be set to all zeros.
        /// </summary>
        public BlockT<int> Instance;

        /// <summary>
        /// Parse the ServerEid structure.
        /// </summary>
        protected override void Parse()
        {
            Ours = ParseAs<byte, bool>();
            FolderId = Parse<FolderID>();
            MessageId = ParseT<ulong>();
            Instance = ParseT<int>();
        }

        protected override void ParseBlocks()
        {
            SetText("ServerEid");
            AddChildBlockT(Ours, "Ours");
            AddChild(FolderId, "FolderId");
            AddChildBlockT(MessageId, "MessageId");
            AddChildBlockT(Instance, "Instance");
        }
    }
}
