using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1.4 PtypServerId Type
    /// </summary>
    public class PtypServerIdStruct : Block
    {
        /// <summary>
        /// The value 0x01 indicates the remaining bytes conform to this structure;
        /// </summary>
        public BlockT<byte> Ours;

        /// <summary>
        /// A Folder ID structure, as specified in section 2.2.1.1.
        /// </summary>
        public FolderID FolderID;

        /// <summary>
        /// A Message ID structure, as specified in section 2.2.1.2, identifying a message in a folder identified by an associated folder ID.
        /// </summary>
        public MessageID MessageID;

        /// <summary>
        /// An unsigned instance number within an array of server IDs to compare against.
        /// </summary>
        public BlockT<uint> Instance;

        /// <summary>
        /// The Ours value 0x00 indicates this is a client-defined value and has whatever size and structure the client has defined.
        /// </summary>
        public BlockBytes ClientData;

        /// <summary>
        /// Parse the PtypServerId structure.
        /// </summary>
        protected override void Parse()
        {
            Ours = ParseT<byte>();
            if (Ours.Data == 0x01)
            {
                FolderID = Parse<FolderID>();
                MessageID = Parse<MessageID>();
                Instance = ParseT<uint>();
            }
            else
            {
                ClientData = ParseBytes(parser.RemainingBytes);
            }
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Ours, "Ours");
            if (Ours.Data == 0x01)
            {
                AddLabeledChild(FolderID, "FolderID");
                AddLabeledChild(MessageID, "MessageID");
                AddChildBlockT(Instance, "Instance");
            }
            else
            {
                AddLabeledChild(ClientData, "ClientData");
            }
        }
    }
}
