using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.2 Message ID Structure
    /// </summary>
    public class MessageID : Block
    {
        /// <summary>
        /// An unsigned integer identifying a Store object.
        /// </summary>
        public BlockT<ushort> ReplicaId;

        /// <summary>
        /// An unsigned integer identifying the message within its Store object. 6 bytes
        /// </summary>
        public BlockBytes GlobalCounter;

        /// <summary>
        /// Parse the MessageID structure.
        /// </summary>
        protected override void Parse()
        {
            ReplicaId = ParseT<ushort>();
            GlobalCounter = ParseBytes(6);
        }

        protected override void ParseBlocks()
        {
            Text = "MessageID";
            AddChildBlockT(ReplicaId, "ReplicaId");
            AddChildBytes(GlobalCounter, "GlobalCounter");
        }
    }
}
