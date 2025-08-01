using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The NamedPropInfo class.
    /// [MS-OXCFXICS] 2.2.4.1 Lexical structure
    /// See PropertyName for more information.
    /// </summary>
    public class NamedPropInfo : Block
    {
        /// <summary>
        /// The PropertySet item in lexical definition.
        /// </summary>
        public BlockGuid PropertySet;

        /// <summary>
        /// The flag variable.
        /// </summary>
        public BlockT<KindEnum> Flag;

        /// <summary>
        /// The Dispid in lexical definition.
        /// </summary>
        public BlockT<uint> Dispid;

        /// <summary>
        /// The name of the NamedPropInfo.
        /// </summary>
        public BlockString Name; // Unicode;

        protected override void Parse()
        {
            PropertySet = Parse<BlockGuid>();
            Flag = ParseT<KindEnum>();

            if (Flag == KindEnum.LID)
            {
                Dispid = ParseT<uint>();
            }
            else if (Flag == KindEnum.Name)
            {
                Name = ParseStringW();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "NamedPropInfo";

            this.AddChildGuid(PropertySet, "PropertySet");
            AddChildBlockT(Flag, "Flag");

            NamedProperty namedProp = null;
            if (PropertySet != null && Dispid != null)
            {
                namedProp = NamedProperty.Lookup(PropertySet.value, Dispid);
            }

            if (Dispid != null)
            {
                if (namedProp != null)
                    AddChild(Dispid, $"Dispid: {namedProp.Name} = 0x{Dispid.Data:X4}");
                else
                    AddChild(Dispid, $"Dispid: 0x{Dispid.Data:X4}");
            }

            AddChildString(Name, "Name");
        }
    }
}
