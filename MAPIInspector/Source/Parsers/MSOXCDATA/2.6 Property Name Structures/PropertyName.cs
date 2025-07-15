using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.6 Property Name Structures
    /// 2.6.1 PropertyName Structure
    /// See NamedPropInfo for more information.
    /// </summary>
    public class PropertyName : Block
    {
        /// <summary>
        /// The Kind field.
        /// </summary>
        public BlockT<KindEnum> Kind;

        /// <summary>
        /// The GUID that identifies the property set for the named property.
        /// </summary>
        public BlockGuid GUID;

        /// <summary>
        /// This field is present only if the value of the Kind field is equal to 0x00.
        /// </summary>
        public BlockT<uint> LID;

        /// <summary>
        /// The value of this field is equal to the number of bytes in the Name string that follows it.
        /// </summary>
        public BlockT<byte> NameSize;

        /// <summary>
        /// This field is present only if Kind is equal to 0x01.
        /// </summary>
        public BlockString Name; // Unicode

        /// <summary>
        /// Parse the PropertyName structure.
        /// </summary>
        protected override void Parse()
        {
            Kind = ParseT<KindEnum>();
            GUID = Parse<BlockGuid>();

            switch (Kind.Data)
            {
                case KindEnum.LID:
                    LID = ParseT<uint>();
                    break;
                case KindEnum.Name:
                    NameSize = ParseT<byte>();
                    Name = ParseStringW(NameSize/2);
                    break;
                case KindEnum.NoPropertyName:
                default:
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("PropertyName");

            AddChildBlockT(Kind, "Kind");
            this.AddChildGuid(GUID, "GUID");

            NamedProperty namedProp = null;
            if (GUID != null && LID != null)
            {
                namedProp = NamedProperty.Lookup(GUID.value, LID);
            }

            if (LID != null)
            {
                if (namedProp != null)
                    AddChild(LID, $"Dispid: {namedProp.Name} = 0x{LID:X4}");
                else
                    AddChild(LID, $"Dispid: 0x{LID:X4}");
            }

            AddChildBlockT(NameSize, "NameSize");
            AddChildString(Name, "Name");
        }
    }
}
