namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// Represents CN structure contains a change number that identifies a version of a messaging object. 
    /// 2.2.2.1 CN Structure
    /// </summary>
    public class CN : Block
    {
        /// <summary>
        /// A 16-bit unsigned integer identifying the server replica in which the messaging object was last changed.
        /// </summary>
        public BlockT<ushort> ReplicaId;

        /// <summary>
        /// An unsigned 48-bit integer identifying the change to the messaging object.
        /// </summary>
        public BlockBytes GlobalCounter; // 6 bytes

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            ReplicaId = ParseT<ushort>();
            GlobalCounter = ParseBytes(6);
        }

        protected override void ParseBlocks()
        {
            SetText("CN");
            if (ReplicaId != null) AddChild(ReplicaId, $"ReplicaId:{ReplicaId.Data} ({ReplicaId.Data:X4})");
            AddLabeledChild(GlobalCounter, "GlobalCounter");
        }
    }
}

