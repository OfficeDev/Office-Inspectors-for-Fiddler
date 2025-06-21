namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.11.7 TypedString Structure
    /// </summary>
    public class TypedString : BaseStructure
    {
        /// <summary>
        /// An enum value of StringType
        /// </summary>
        public StringTypeEnum StringType;

        /// <summary>
        /// If the StringType field is set to 0x02, 0x03, or 0x04, then this field MUST be present and in the format specified by the Type field. Otherwise, this field MUST NOT be present.
        /// </summary>
        public MAPIString String;

        /// <summary>
        /// Parse the TypedString structure.
        /// </summary>
        /// <param name="s">A stream containing the TypedString structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            StringType = (StringTypeEnum)ReadByte();
            switch (StringType)
            {
                case StringTypeEnum.NoPresent:
                case StringTypeEnum.Empty:
                    {
                        String = null;
                        break;
                    }

                case StringTypeEnum.CharacterString:
                    {
                        String = new MAPIString(Encoding.ASCII);
                        String.Parse(s);
                        break;
                    }

                case StringTypeEnum.ReducedUnicodeCharacterString:
                    {
                        String = new MAPIString(Encoding.ASCII);
                        String.Parse(s);
                        break;
                    }

                case StringTypeEnum.UnicodeCharacterString:
                    {
                        String = new MAPIString(Encoding.Unicode);
                        String.Parse(s);
                        break;
                    }

                default:
                    break;
            }
        }
    }
}
