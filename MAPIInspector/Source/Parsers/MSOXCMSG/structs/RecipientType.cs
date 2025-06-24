namespace MAPIInspector.Parsers
{
    using System.IO;


    /// <summary>
    /// 2.2.3.1.2 RopOpenMessage ROP Response Buffer
    /// An enumeration that specifies the type of recipient (2).
    /// </summary>
    public class RecipientType : BaseStructure
    {
        /// <summary>
        /// RecipientType flag
        /// </summary>
        [BitAttribute(4)]
        public RecipientTypeFlag Flag;

        /// <summary>
        /// RecipientType type
        /// </summary>
        [BitAttribute(4)]
        public RecipientTypeType Type;

        /// <summary>
        /// Parse RecipientType structure
        /// </summary>
        /// <param name="s">A stream containing RecipientType structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte bitWise = ReadByte();
            Flag = (RecipientTypeFlag)(bitWise & 0xF0);
            Type = (RecipientTypeType)(bitWise & 0x0F);
        }
    }
}
