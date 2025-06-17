using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropInfo class.
    /// .2.4.1 Lexical structure propInfo
    /// </summary>
    public class PropInfo : Block
    {
        /// <summary>
        /// The property id.
        /// </summary>
        public BlockT<PidTagPropertyEnum> PropID;

        /// <summary>
        /// The namedPropInfo in lexical definition.
        /// </summary>
        public NamedPropInfo NamedPropInfo;

        protected override void Parse()
        {
            PropID = BlockT<PidTagPropertyEnum>(parser);

            if ((ushort)PropID.Data >= 0x8000)
            {
                NamedPropInfo = Parse<NamedPropInfo>(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("PropInfo");
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            AddChild(NamedPropInfo);
        }
    }
}
