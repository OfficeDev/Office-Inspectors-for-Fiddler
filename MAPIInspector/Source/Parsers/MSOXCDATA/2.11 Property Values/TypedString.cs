using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.7 TypedString Structure
    /// </summary>
    public class TypedString : Block
    {
        /// <summary>
        /// An enum value of StringType
        /// </summary>
        public BlockT<StringTypeEnum> StringType;

        /// <summary>
        /// If the StringType field is set to 0x02, 0x03, or 0x04, then this field MUST be present and in the format specified by the Type field. Otherwise, this field MUST NOT be present.
        /// </summary>
        public BlockString String;

        /// <summary>
        /// Parse the TypedString structure.
        /// </summary>
        protected override void Parse()
        {
            StringType = ParseT<StringTypeEnum>();
            switch (StringType.Data)
            {
                case StringTypeEnum.NoPresent:
                case StringTypeEnum.Empty:
                    {
                        String = null;
                        break;
                    }
                case StringTypeEnum.CharacterString:
                case StringTypeEnum.ReducedUnicodeCharacterString:
                    {
                        String = ParseStringA();
                        break;
                    }
                case StringTypeEnum.UnicodeCharacterString:
                    {
                        String = ParseStringW();
                        break;
                    }
                default:
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TypedString");
            AddChildBlockT(StringType, "StringType");
            AddChildString(String, "String");
        }
    }
}
