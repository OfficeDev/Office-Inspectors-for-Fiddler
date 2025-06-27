using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a 16-bit COUNT field followed by a structure as specified in section 2.11.1.4.
    /// </summary>
    public class PtypServerId : Block
    {
        /// <summary>
        /// The COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public BlockT<ushort> Count;

        /// <summary>
        /// A structure as specified in section 2.11.1.4.
        /// </summary>
        public PtypServerIdStruct ServerId;

        /// <summary>
        /// Parse the PtypServerId structure.
        /// </summary>
        protected override void Parse()
        {
            Count = ParseT<ushort>();
            ServerId = Parse<PtypServerIdStruct>(Count);
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Count, "Count");
            AddLabeledChild(ServerId, "ServerId");
        }
    }
}
