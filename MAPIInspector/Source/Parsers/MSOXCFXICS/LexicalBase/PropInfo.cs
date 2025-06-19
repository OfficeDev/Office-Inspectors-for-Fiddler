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
            PropID = ParseT<PidTagPropertyEnum>();

            if ((ushort)PropID.Data >= 0x8000)
            {
                NamedPropInfo = Parse<NamedPropInfo>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("PropInfo");
            AddChildBlockT(PropID, "PropID");
            AddChild(NamedPropInfo);
        }
    }
}
