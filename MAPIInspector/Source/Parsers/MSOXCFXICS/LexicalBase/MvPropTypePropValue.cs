namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValue : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public BlockT<int> Length;

        Block[] ValueArray;

        /// <summary>
        /// Verify that a stream's current position contains a serialized MvPropTypePropValue.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized MvPropTypePropValue, return true, else false</returns>
        public static new bool Verify(BinaryParser parser)
        {
            var tmp = TestParse<PropertyDataType>(parser);
            if (tmp == null || !tmp.Parsed) return false;
            return LexicalTypeHelper.IsMVType(tmp.Data) && !IsMetaTagIdsetGiven(parser);
        }

        protected override void Parse()
        {
            base.Parse();
            Length = ParseT<int>();
            long blocksLength = Length.Data;

            ValueArray = ParseArray(parser, PropType.Data, blocksLength);
        }

        public static Block[] ParseArray(BinaryParser parser, PropertyDataType dataType, long dataLength)
        {
            var blocks = new List<Block>();
            while (dataLength > 0)
            {
                Block tmpBlock = null;
                switch (dataType)
                {
                    case PropertyDataType.PtypMultipleInteger16:
                        tmpBlock = Parse<PtypInteger16>(parser);
                        break;
                    case PropertyDataType.PtypMultipleInteger32:
                        tmpBlock = Parse<PtypInteger32>(parser);
                        break;
                    case PropertyDataType.PtypMultipleFloating32:
                        tmpBlock = Parse<PtypFloating32>(parser);
                        break;
                    case PropertyDataType.PtypMultipleFloating64:
                        tmpBlock = Parse<PtypFloating64>(parser);
                        break;
                    case PropertyDataType.PtypMultipleCurrency:
                        tmpBlock = Parse<PtypCurrency>(parser);
                        break;
                    case PropertyDataType.PtypMultipleFloatingTime:
                        tmpBlock = Parse<PtypFloatingTime>(parser);
                        break;
                    case PropertyDataType.PtypMultipleInteger64:
                        tmpBlock = Parse<PtypInteger64>(parser);
                        break;
                    case PropertyDataType.PtypMultipleTime:
                        tmpBlock = Parse<PtypTime>(parser);
                        break;
                    case PropertyDataType.PtypMultipleGuid:
                        tmpBlock = Parse<PtypGuid>(parser);
                        break;
                    case PropertyDataType.PtypMultipleBinary:
                        tmpBlock = new PtypBinaryBlock(CountWideEnum.fourBytes);
                        tmpBlock.Parse(parser);
                        break;
                    case PropertyDataType.PtypMultipleString:
                        tmpBlock = new PtypString(CountWideEnum.fourBytes);
                        tmpBlock.Parse(parser);
                        break;
                    case PropertyDataType.PtypMultipleString8:
                        tmpBlock = new PtypString8(CountWideEnum.fourBytes);
                        tmpBlock.Parse(parser);
                        break;
                }

                if (tmpBlock != null)
                {
                    blocks.Add(tmpBlock);
                    dataLength -= tmpBlock.Size;
                }
            }

            return blocks.ToArray();
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            AddChildBlockT(Length, "Length");
            AddLabeledChildren(ValueArray, "ValueArray");
        }
    }
}