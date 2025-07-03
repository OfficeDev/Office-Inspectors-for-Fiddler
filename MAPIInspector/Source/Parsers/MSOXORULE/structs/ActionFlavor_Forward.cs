using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1.1 Action Flavors
    /// This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is relate to FORWARD
    /// </summary>
    public class ActionFlavor_Forward : Block
    {
        public BlockT<byte> Byte0;

        /// <summary>
        /// The reserved bit.
        /// </summary>
        public BlockT<byte> Reservedbits0;

        /// <summary>
        /// Indicates that the message SHOULD be forwarded as a Short Message Service (SMS) text message. 
        /// </summary>
        public BlockT<byte> TM;

        /// <summary>
        /// Forwards the message as an attachment. This value MUST NOT be combined with other ActionFlavor flags.
        /// </summary>
        public BlockT<byte> AT;

        /// <summary>
        /// Forwards the message without making any changes to the message. 
        /// </summary>
        public BlockT<byte> NC;

        /// <summary>
        /// Preserves the sender information and indicates that the message was auto forwarded. 
        /// </summary>
        public BlockT<byte> PR;

        /// <summary>
        /// The reserved bit.3 bytes.
        /// </summary>
        public BlockBytes Reservedbits1;

        /// <summary>
        /// Parse the ActionFlavor_Forward structure.
        /// </summary>
        protected override void Parse()
        {
            Byte0 = ParseT<byte>();
            int index = 0;
            Reservedbits0 = CreateBlock(BaseStructure.GetBits(Byte0, index, 4), Byte0.Size, Byte0.Offset);
            index += 4;
            TM = CreateBlock(BaseStructure.GetBits(Byte0, index, 1), Byte0.Size, Byte0.Offset);
            index += 1;
            AT = CreateBlock(BaseStructure.GetBits(Byte0, index, 1), Byte0.Size, Byte0.Offset);
            index += 1;
            NC = CreateBlock(BaseStructure.GetBits(Byte0, index, 1), Byte0.Size, Byte0.Offset);
            index += 1;
            PR = CreateBlock(BaseStructure.GetBits(Byte0, index, 1), Byte0.Size, Byte0.Offset);

            Reservedbits1 = ParseBytes(3);
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Reservedbits0, "Reservedbits0");
            AddChildBlockT(TM, "TM");
            AddChildBlockT(AT, "AT");
            AddChildBlockT(NC, "NC");
            AddChildBlockT(PR, "PR");
            AddChildBytes(Reservedbits1, "Reservedbits1");
        }

    }
}
