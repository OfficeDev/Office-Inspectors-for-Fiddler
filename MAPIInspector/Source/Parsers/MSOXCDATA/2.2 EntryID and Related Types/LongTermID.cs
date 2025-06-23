namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2.1.3.1 LongTermID Structure
    /// </summary>
    public class LongTermID : Block
    {
        /// <summary>
        /// An unsigned integer identifying a Store object.
        /// </summary>
        public BlockGuid DatabaseGuid;

        /// <summary>
        /// An unsigned integer identifying the folder or message within its Store object. 6 bytes
        /// </summary>
        public BlockBytes GlobalCounter;

        /// <summary>
        /// A 2-byte Pad field.
        /// </summary>
        public BlockT<ushort> Pad;

        /// <summary>
        /// Parse the LongTermID structure.
        /// </summary>
        protected override void Parse()
        {
            DatabaseGuid = Parse<BlockGuid>();
            GlobalCounter = ParseBytes(6);
            Pad = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            this.AddChildGuid(DatabaseGuid, "DatabaseGuid");
            if (GlobalCounter != null) AddChild(GlobalCounter, $"GlobalCounter :{GlobalCounter.ToHexString(false)}");
            AddChildBlockT(Pad, "Pad");
        }
    }
}
