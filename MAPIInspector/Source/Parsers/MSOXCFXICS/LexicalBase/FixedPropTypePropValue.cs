using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValue : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public Block FixedValue;

        /// <summary>
        /// Verify that a stream's current position contains a serialized FixedPropTypePropValue.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized FixedPropTypePropValue, return true, else false</returns>
        public static new bool Verify(BinaryParser parser)
        {
            var tmp = TestParse<PropertyDataType>(parser);
            if (tmp == null || !tmp.Parsed) return false;
            return LexicalTypeHelper.IsFixedType(tmp) && !IsMetaTagIdsetGiven(parser);
        }

        protected override void Parse()
        {
            base.Parse();

            FixedValue = ParseFixedProp(parser, PropType, PropInfo.PropID);
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            AddChild(FixedValue, $"FixedValue:{FixedValue}");
        }

        public static Block ParseFixedProp(BinaryParser parser, PropertyDataType dataType, PidTagPropertyEnum id)
        {
            switch (dataType)
            {
                case PropertyDataType.PtypInteger16:
                    return Parse<PtypInteger16>(parser);
                case PropertyDataType.PtypInteger32:
                    if (id == PidTagPropertyEnum.PidTagChangeNumber)
                    {
                        return Parse<CN>(parser);
                    }
                    else
                    {
                        return Parse<PtypInteger32>(parser);
                    }

                case PropertyDataType.PtypFloating32:
                    return Parse<PtypFloating32>(parser);
                case PropertyDataType.PtypFloating64:
                    return Parse<PtypFloating64>(parser);
                case PropertyDataType.PtypCurrency:
                    return Parse<PtypCurrency>(parser);
                case PropertyDataType.PtypFloatingTime:
                    return Parse<PtypFloatingTime>(parser);
                case PropertyDataType.PtypBoolean:
                    return Parse<PtypBooleanShort>(parser);
                case PropertyDataType.PtypInteger64:
                    if (id == (PidTagPropertyEnum)0x6714)
                    {
                        return Parse<CN>(parser);
                    }
                    else if (id == PidTagPropertyEnum.PidTagMid)
                    {
                        return Parse<MessageID>(parser);
                    }
                    else if (id == PidTagPropertyEnum.PidTagFolderId)
                    {
                        return Parse<FolderID>(parser);
                    }
                    else
                    {
                        return Parse<PtypInteger64>(parser);
                    }

                case PropertyDataType.PtypTime:
                    return Parse<PtypTime>(parser);
                case PropertyDataType.PtypGuid:
                    return Parse<PtypGuid>(parser);
            }

            return null;
        }
    }
}
