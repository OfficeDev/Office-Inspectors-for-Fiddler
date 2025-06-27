using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3.1.2 RopOpenMessage ROP Response Buffer
    /// An enumeration that specifies the type of recipient (2).
    /// </summary>
    public class RecipientType : Block
    {
        private BlockT<byte> Byte0;

        /// <summary>
        /// RecipientType flag
        /// </summary>
        public BlockT<RecipientTypeFlag> Flag;

        /// <summary>
        /// RecipientType type
        /// </summary>
        public BlockT<RecipientTypeType> Type;

        /// <summary>
        /// Parse RecipientType structure
        /// </summary>
        protected override void Parse()
        {
            Byte0 = ParseT<byte>();
            int index = 0;
            Flag = CreateBlock((RecipientTypeFlag)(BaseStructure.GetBits(Byte0, index, 4) & 0xF0), Byte0.Size, Byte0.Offset);
            index = index + 4;
            Type = CreateBlock((RecipientTypeType)(BaseStructure.GetBits(Byte0, index, 4) & 0x0F), Byte0.Size, Byte0.Offset);
            index = index + 4;
        }

        protected override void ParseBlocks()
        {
            SetText("RecipientType");
            AddChildBlockT(Flag, "Flag");
            AddChildBlockT(Type, "Type");
        }
    }
}
