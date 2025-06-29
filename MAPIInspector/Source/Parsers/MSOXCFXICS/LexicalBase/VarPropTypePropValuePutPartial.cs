using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValuePutPartial : PropValue
    {
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public BlockT<int> Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public Block ValueArray;

        /// <summary>
        /// The length value used for partial split
        /// </summary>
        protected int plength;

        /// <summary>
        /// Boolean value used to record whether ptypString value is split to two bytes which parse in different buffer
        /// </summary>
        protected bool splitpreviousOne = false;

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                Partial.PartialPutType = PropType;
                Partial.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialPutType != 0 && Partial.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && Partial.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && Partial.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = CreateBlock(Partial.PartialPutType, 0, 0);

                    if (Partial.PartialPutRemainSize != -1)
                    {
                        plength = Partial.PartialPutRemainSize;

                        if (plength % 2 != 0 &&
                            (ptype == PropertyDataType.PtypString ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode52))
                        {
                            splitpreviousOne = true;
                        }

                        Partial.PartialPutRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    Partial.PartialPutType = 0;

                    if (Partial.PartialPutRemainSize == -1)
                    {
                        Partial.PartialPutServerUrl = string.Empty;
                        Partial.PartialPutProcessName = string.Empty;
                        Partial.PartialPutClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = ParseT<int>();
                }

                int blockLength = Length != null ? Length : plength;
                PropertyDataType typeValue = PropertyDataType.PtypUnspecified;
                if (PropType != null)
                {
                    typeValue = PropType;
                }
                else if (ptype != null)
                {
                    typeValue = ptype;
                }

                if (parser.RemainingBytes < blockLength)
                {
                    Partial.PartialPutType = typeValue;
                    Partial.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    Partial.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    Partial.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    if (splitpreviousOne)
                                    {
                                        parser.Advance(1);
                                        splitpreviousOne = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    if (blockLength % 2 != 0)
                                    {
                                        parser.Advance(1);
                                    }
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    parser.Advance(1);
                                    splitpreviousOne = false;
                                }

                                if ((blockLength / 2) != 0)
                                {
                                    ValueArray = new PtypString(blockLength);
                                    ValueArray.Parse(parser);
                                }
                            }

                            break;
                        case CodePageType.PtypCodePageUnicode52:
                            if (Length != null)
                            {
                                Length = ParseT<int>();
                                blockLength = Length;
                            }

                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    if (splitpreviousOne)
                                    {
                                        parser.Advance(1);
                                        splitpreviousOne = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    if (blockLength % 2 != 0)
                                    {
                                        parser.Advance(1);
                                    }
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    parser.Advance(1);
                                    splitpreviousOne = false;
                                }

                                if ((blockLength / 2) != 0)
                                {
                                    ValueArray = new PtypString(blockLength);
                                    ValueArray.Parse(parser);
                                }
                            }

                            break;
                        case CodePageType.PtypCodePageUnicodeBigendian:
                        case CodePageType.PtypCodePageWesternEuropean:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                    }
                }
                else
                {
                    switch (typeValue)
                    {
                        case PropertyDataType.PtypString:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    if (splitpreviousOne)
                                    {
                                        parser.Advance(1);
                                        splitpreviousOne = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    if (blockLength % 2 != 0)
                                    {
                                        parser.Advance(1);
                                    }
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    parser.Advance(1);
                                    splitpreviousOne = false;
                                }

                                if ((blockLength / 2) != 0)
                                {
                                    ValueArray = new PtypString(blockLength);
                                    ValueArray.Parse(parser);
                                }
                            }

                            break;
                        case PropertyDataType.PtypString8:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        case PropertyDataType.PtypBinary:
                        case PropertyDataType.PtypServerId:
                        case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(blockLength);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(blockLength);
                            break;
                    }
                }
            }
        }
        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            SetText("VarPropTypePropValuePutPartial");
            AddChildBlockT(Length, "Length");
            AddChild(ValueArray, $"ValueArray: {ValueArray}");
        }
    }
}
