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
                MapiInspector.MAPIParser.PartialPutType = PropType.Data;
                MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutType != 0 && MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = CreateBlock(MapiInspector.MAPIParser.PartialPutType, 0, 0);

                    if (MapiInspector.MAPIParser.PartialPutRemainSize != -1)
                    {
                        plength = MapiInspector.MAPIParser.PartialPutRemainSize;

                        if (plength % 2 != 0 &&
                            (ptype.Data == PropertyDataType.PtypString ||
                            ptype.Data == (PropertyDataType)CodePageType.PtypCodePageUnicode ||
                            ptype.Data == (PropertyDataType)CodePageType.PtypCodePageUnicode52))
                        {
                            splitpreviousOne = true;
                        }

                        MapiInspector.MAPIParser.PartialPutRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>(parser);
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;

                    if (MapiInspector.MAPIParser.PartialPutRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = ParseT<int>(parser);
                }

                int blockLength = Length != null ? Length.Data : plength;
                PropertyDataType typeValue = PropertyDataType.PtypUnspecified;
                if (PropType != null)
                {
                    typeValue = PropType.Data;
                }
                else if (ptype != null)
                {
                    typeValue = ptype.Data;
                }

                if (parser.RemainingBytes < blockLength)
                {
                    MapiInspector.MAPIParser.PartialPutType = typeValue;
                    MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
                                Length = ParseT<int>(parser);
                                blockLength = Length.Data;
                            }

                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(parser, blockLength);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(parser, blockLength);
                            break;
                    }
                }
            }
        }
        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            SetText("VarPropTypePropValuePutPartial");
            if (Length != null) AddChild(Length, $"Length:{Length.Data}");
            AddChild(ValueArray, $"ValueArray: {ValueArray}");
        }
    }
}
