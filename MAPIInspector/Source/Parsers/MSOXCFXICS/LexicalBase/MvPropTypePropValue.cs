using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValue : PropValue
    {
        /// <summary>
        /// This represent the length variable, the count of the number of elements.
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
            return LexicalTypeHelper.IsMVType(tmp) && !IsMetaTagIdsetGiven(parser);
        }

        protected override void Parse()
        {
            base.Parse();
            Length = ParseT<int>();
            long dataCount = Length;

            ValueArray = ParseArray(parser, PropType, dataCount);
        }

        public static Block[] ParseArray(BinaryParser parser, PropertyDataType dataType, long dataCount)
        {
            var blocks = new List<Block>();
            for (int i = 0; i < dataCount; i++)
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
                        tmpBlock = new PtypBinary(CountWideEnum.fourBytes);
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