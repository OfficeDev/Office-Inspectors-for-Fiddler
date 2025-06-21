namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.6 Property Name Structures
    /// 2.6.1 PropertyName Structure
    /// </summary>
    public class PropertyName : BaseStructure
    {
        /// <summary>
        /// The Kind field.
        /// </summary>
        public KindEnum Kind;

        /// <summary>
        /// The GUID that identifies the property set for the named property.
        /// </summary>
        public AnnotatedGuid GUID;

        /// <summary>
        /// This field is present only if the value of the Kind field is equal to 0x00.
        /// </summary>
        public AnnotatedUint LID;

        /// <summary>
        /// The value of this field is equal to the number of bytes in the Name string that follows it.
        /// </summary>
        public byte? NameSize;

        /// <summary>
        /// This field is present only if Kind is equal to 0x01.
        /// </summary>
        public MAPIString Name;

        /// <summary>
        /// Parse the PropertyName structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyName structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Kind = (KindEnum)ReadByte();
            GUID = new AnnotatedGuid(s);
            switch (Kind)
            {
                case KindEnum.LID:
                    {
                        LID = new AnnotatedUint(s);
                        var namedProp = NamedProperty.Lookup(GUID.value, LID.value);
                        if (namedProp != null)
                            LID.ParsedValue = $"{namedProp.Name} = 0x{LID.value:X4}";
                        else
                            LID.ParsedValue = $"0x{LID.value:X4}";

                        break;
                    }

                case KindEnum.Name:
                    {
                        NameSize = ReadByte();
                        Name = new MAPIString(Encoding.Unicode, string.Empty, (int)NameSize / 2);
                        Name.Parse(s);

                        break;
                    }

                case KindEnum.NoPropertyName:
                default:
                    {
                        break;
                    }
            }
        }
    }
}
