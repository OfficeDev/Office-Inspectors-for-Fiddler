using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValuePutExtendPartial : Block
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public BlockT<PropertyDataType> PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public BlockT<PidTagPropertyEnum> PropID;

        /// <summary>
        /// The property value.
        /// </summary>
        public Block PropValue;

        /// <summary>
        /// The property type for partial split.
        /// </summary>
        private PropertyDataType propertyType;

        /// <summary>
        /// The property id for partial split.
        /// </summary>
        private PidTagPropertyEnum propertyID;

        /// <summary>
        /// The length value is for ptypBinary
        /// </summary>
        public BlockT<int> length;

        protected override void Parse()
        {
            if (MapiInspector.MAPIParser.PartialPutExtendType == 0 ||
                (MapiInspector.MAPIParser.PartialPutType != 0 &&
                !(MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                PropType = BlockT<PropertyDataType>.Parse(parser);
                PropID = BlockT<PidTagPropertyEnum>.Parse(parser);
            }

            if (parser.Empty)
            {
                MapiInspector.MAPIParser.PartialPutExtendType = PropType.Data;
                MapiInspector.MAPIParser.PartialPutExtendId = PropID.Data;
                MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutExtendType != 0 &&
                    MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    propertyType = MapiInspector.MAPIParser.PartialPutExtendType;
                    propertyID = MapiInspector.MAPIParser.PartialPutExtendId;

                    // clear
                    MapiInspector.MAPIParser.PartialPutExtendType = 0;
                    MapiInspector.MAPIParser.PartialPutExtendId = 0;

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                    }
                }

                PropertyDataType typeValue = PropType != null ? PropType.Data : propertyType;
                PidTagPropertyEnum identifyValue = PropID != null ? PropID.Data : propertyID;
                if (identifyValue != PidTagPropertyEnum.MetaTagNewFXFolder && identifyValue != PidTagPropertyEnum.MetaTagDnPrefix)
                {
                    PropValue = BlockT<uint>.Parse(parser);
                }
                else if (identifyValue == PidTagPropertyEnum.MetaTagNewFXFolder)
                {
                    if (!parser.Empty)
                    {
                        if (MapiInspector.MAPIParser.PartialPutExtendRemainSize != -1 &&
                            MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                            MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                            MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            length = CreateBlock(MapiInspector.MAPIParser.PartialPutExtendRemainSize, 0, 0);

                            // clear
                            MapiInspector.MAPIParser.PartialPutExtendRemainSize = -1;
                            MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                            MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                            MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                        }
                        else
                        {
                            length = BlockT<int>.Parse(parser);
                        }

                        if (parser.RemainingBytes < length.Data)
                        {
                            MapiInspector.MAPIParser.PartialGetType = typeValue;
                            MapiInspector.MAPIParser.PartialGetId = identifyValue;
                            MapiInspector.MAPIParser.PartialPutExtendRemainSize = length.Data - parser.RemainingBytes;
                            MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];


                            PropValue = BlockBytes.Parse(parser, parser.RemainingBytes);
                        }
                        else
                        {
                            PropValue = BlockBytes.Parse(parser, length.Data);
                        }
                    }
                }
                else
                {
                    PropValue = Parse<PtypString8>(parser);
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("MetaPropValuePutExtendPartial");
            if (PropType!= null) AddChild(PropType, $"PropType:{PropType.Data}");
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            if (PropValue != null) AddChild(PropValue, $"PropValue:{PropValue.GetType().Name}");
            if (length != null) AddChild(length, $"Length:{length.Data}");
        }
    }
}