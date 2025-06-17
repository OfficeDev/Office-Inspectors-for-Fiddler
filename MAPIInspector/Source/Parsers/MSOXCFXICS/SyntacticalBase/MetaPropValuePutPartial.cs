using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValuePutPartial : Block
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
            if (MapiInspector.MAPIParser.PartialPutType == 0 ||
                (MapiInspector.MAPIParser.PartialPutType != 0 &&
                !(MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                PropType = BlockT<PropertyDataType>.Parse(parser);
                PropID = BlockT<PidTagPropertyEnum>.Parse(parser);
            }

            if (parser.Empty)
            {
                MapiInspector.MAPIParser.PartialPutType = PropType.Data;
                MapiInspector.MAPIParser.PartialPutId = PropID.Data;
                MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutType != 0 &&
                    MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    propertyType = MapiInspector.MAPIParser.PartialPutType;
                    propertyID = MapiInspector.MAPIParser.PartialPutId;

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;
                    MapiInspector.MAPIParser.PartialPutId = 0;

                    if (MapiInspector.MAPIParser.PartialPutRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
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
                        if (MapiInspector.MAPIParser.PartialPutRemainSize != -1 &&
                            MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                            MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                            MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            length = BlockT<int>.Create(MapiInspector.MAPIParser.PartialPutRemainSize, 0, 0);

                            // clear
                            MapiInspector.MAPIParser.PartialPutRemainSize = -1;
                            MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                            MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                            MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
                        }
                        else
                        {
                            length = BlockT<int>.Parse(parser);
                        }

                        if (parser.RemainingBytes < length.Data)
                        {
                            MapiInspector.MAPIParser.PartialPutType = typeValue;
                            MapiInspector.MAPIParser.PartialPutId = identifyValue;
                            MapiInspector.MAPIParser.PartialPutRemainSize = length.Data - parser.RemainingBytes;
                            MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];

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
            SetText("MetaPropValuePutPartial");
            if (PropType != null) AddChild(PropType, $"PropType:{PropType.Data}");
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            if (PropValue != null) AddChild(PropValue, $"PropValue:{PropValue.GetType().Name}");
            if (length != null) AddChild(length, $"Length:{length.Data}");
        }
    }
}
