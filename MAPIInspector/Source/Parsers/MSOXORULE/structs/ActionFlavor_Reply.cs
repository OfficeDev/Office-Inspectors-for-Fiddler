using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.5.1.1 Action Flavors
    /// This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is relate to REPLY
    /// </summary>
    public class ActionFlavor_Reply : Block
    {
        /// <summary>
        /// The reserved bit.
        /// </summary>
        private BlockT<byte> Byte0;

        public BlockT<byte> Reservedbits0;

        /// <summary>
        /// Server will use fixed, server-defined text in the reply message and ignore the text in the reply template.
        /// </summary>
        public BlockT<byte> ST;

        /// <summary>
        /// The server SHOULD not send the message to the message sender (the reply template MUST contain recipients (2) in this case).
        /// </summary>
        public BlockT<byte> NS;

        /// <summary>
        /// The reserved bit.3 bytes
        /// </summary>
        public BlockBytes Reservedbits1;

        /// <summary>
        /// Parse the ActionFlavor_Reply structure.
        /// </summary>
        protected override void Parse()
        {
            Byte0 = ParseT<byte>();
            int index = 0;
            Reservedbits0 = CreateBlock(MapiInspector.Utilities.GetBits(Byte0, index, 6), Byte0.Size, Byte0.Offset);
            index += 6;
            ST = CreateBlock(MapiInspector.Utilities.GetBits(Byte0, index, 1), Byte0.Size, Byte0.Offset);
            index += 1;
            NS = CreateBlock(MapiInspector.Utilities.GetBits(Byte0, index, 1), Byte0.Size, Byte0.Offset);
            Reservedbits1 = ParseBytes(3);
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Reservedbits0, "Reservedbits0");
            AddChildBlockT(ST, "ST");
            AddChildBlockT(NS, "NS");
            AddChildBytes(Reservedbits1, "Reservedbits1");
        }
    }
}
