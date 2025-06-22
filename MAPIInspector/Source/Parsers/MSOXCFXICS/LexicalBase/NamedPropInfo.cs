using BlockParser;
using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The NamedPropInfo class.
    /// 2.2.4.1 Lexical structure namedPropInfo
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
        public BlockT<byte> Flag;

        /// <summary>
        /// The Dispid in lexical definition.
        /// </summary>
        public BlockT<uint> Dispid;

        /// <summary>
        /// The name of the NamedPropInfo.
        /// </summary>
        public BlockStringW Name;

        protected override void Parse()
        {
            PropertySet = Parse<BlockGuid>();
            Flag = ParseT<byte>();

            if (Flag.Data == 0x00)
            {
                Dispid = ParseT<uint>();
            }
            else if (Flag.Data == 0x01)
            {
                Name = ParseStringW();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("NamedPropInfo");

            if (PropertySet != null) AddChild(PropertySet, $"PropertySet: {PropertySet}");
            if (Flag != null) AddChild(Flag, $"Flag:{Flag.Data:X}");

            NamedProperty namedProp = null;
            if (PropertySet != null && Dispid != null)
            {
                namedProp = NamedProperty.Lookup(PropertySet.value.Data, Dispid.Data);
            }

            if (Dispid != null)
            {
                if (namedProp != null)
                    AddChild(Dispid, $"Dispid: {namedProp.Name} = 0x{Dispid.Data:X4}");
                else
                    AddChild(Dispid, $"Dispid: 0x{Dispid.Data:X4}");
            }

            if (Name != null) AddChild(Name, $"Name: {Name.Data}");
        }
    }
}
