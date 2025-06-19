namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;
    /// <summary>
    /// The structure of LongTermId
    /// 2.2.1.3.1 LongTermID Structure
    /// </summary>
    public class LongTermId : Block
    {
        /// <summary>
        /// A 128-bit unsigned integer identifying a Store object.
        /// </summary>
        public BlockT<Guid> DatabaseGuid;

        /// <summary>
        /// An unsigned 48-bit integer identifying the folder within its Store object.
        /// </summary>
        public BlockBytes GlobalCounter;

        /// <summary>
        /// An UShort.
        /// </summary>
        public BlockT<ushort> Pad;

        /// <summary>
        /// Parse the LongTermId structure
        /// </summary>
        protected override void Parse()
        {
            DatabaseGuid = ParseT<Guid>();
            GlobalCounter = ParseBytes(6, 6);
            Pad = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            SetText("LongTermId");
            AddChildBlockT(DatabaseGuid, "DatabaseGuid");
            if (GlobalCounter != null) AddChild(GlobalCounter, $"GlobalCounter:{GlobalCounter.ToHexString(false)}");
            AddChildBlockT(Pad, "Pad");
        }
    }
}
