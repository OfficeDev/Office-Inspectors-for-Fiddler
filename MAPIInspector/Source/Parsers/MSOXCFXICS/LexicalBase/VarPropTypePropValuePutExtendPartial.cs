using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValuePutExtendPartial : PropValue
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
                MapiInspector.MAPIParser.PartialPutExtendType = PropType.Data;
                MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutExtendType != 0 && MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    ptype = BlockT<PropertyDataType>.Create(MapiInspector.MAPIParser.PartialPutExtendType, 0, 0);

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize != -1)
                    {
                        plength = MapiInspector.MAPIParser.PartialPutExtendRemainSize;

                        if (plength % 2 != 0 &&
                            (ptype.Data == PropertyDataType.PtypString ||
                            ptype.Data == (PropertyDataType)CodePageType.PtypCodePageUnicode ||
                            ptype.Data == (PropertyDataType)CodePageType.PtypCodePageUnicode52))
                        {
                            splitpreviousOne = true;
                        }

                        MapiInspector.MAPIParser.PartialPutExtendRemainSize = -1;
                    }
                    else
                    {
                        Length = BlockT<int>.Parse(parser);
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialPutExtendType = 0;

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = BlockT<int>.Parse(parser);
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
                    MapiInspector.MAPIParser.PartialPutExtendType = typeValue;
                    MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
                                Length = BlockT<int>.Parse(parser);
                                blockLength = Length.Data;
                            }

                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = BlockBytes.Parse(parser, blockLength);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                MapiInspector.MAPIParser.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = BlockBytes.Parse(parser, blockLength);
                            break;
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            SetText("VarPropTypePropValuePutExtendPartial");
            if (Length != null) AddChild(Length, $"Length:{Length.Data}");
            AddChild(ValueArray, $"ValueArray: {ValueArray}");
        }
    }
}
