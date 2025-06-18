using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValueGetPartial : Block
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
            if (MapiInspector.MAPIParser.PartialGetType == 0 ||
                (MapiInspector.MAPIParser.PartialGetType != 0 &&
                !(MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                PropType = ParseT<PropertyDataType>(parser);
                PropID = ParseT<PidTagPropertyEnum>(parser);
            }

            if (parser.Empty)
            {
                MapiInspector.MAPIParser.PartialGetType = PropType.Data;
                MapiInspector.MAPIParser.PartialGetId = PropID.Data;
                MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialGetType != 0 &&
                    MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    propertyType = MapiInspector.MAPIParser.PartialGetType;
                    propertyID = MapiInspector.MAPIParser.PartialGetId;

                    // clear
                    MapiInspector.MAPIParser.PartialGetType = 0;
                    MapiInspector.MAPIParser.PartialGetId = 0;

                    if (MapiInspector.MAPIParser.PartialGetRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialGetClientInfo = string.Empty;
                    }
                }

                PropertyDataType typeValue = PropType != null ? PropType.Data : propertyType;
                PidTagPropertyEnum identifyValue = PropID != null ? PropID.Data : propertyID;
                if (identifyValue != PidTagPropertyEnum.MetaTagNewFXFolder && identifyValue != PidTagPropertyEnum.MetaTagDnPrefix)
                {
                    PropValue = Parse<PtypInteger32>(parser);
                }
                else if (identifyValue == PidTagPropertyEnum.MetaTagNewFXFolder)
                {
                    if (!parser.Empty)
                    {
                        if (MapiInspector.MAPIParser.PartialGetRemainSize != -1 &&
                            MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                            MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                            MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            length = CreateBlock(MapiInspector.MAPIParser.PartialGetRemainSize, 0, 0);

                            // clear
                            MapiInspector.MAPIParser.PartialGetRemainSize = -1;
                            MapiInspector.MAPIParser.PartialGetServerUrl = string.Empty;
                            MapiInspector.MAPIParser.PartialGetProcessName = string.Empty;
                            MapiInspector.MAPIParser.PartialGetClientInfo = string.Empty;
                        }
                        else
                        {
                            length = ParseT<int>(parser);
                        }

                        if (parser.RemainingBytes < length.Data)
                        {
                            MapiInspector.MAPIParser.PartialGetType = typeValue;
                            MapiInspector.MAPIParser.PartialGetId = identifyValue;
                            MapiInspector.MAPIParser.PartialGetRemainSize = length.Data - parser.RemainingBytes;
                            MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];

                            PropValue = ParseBytes(parser.RemainingBytes);
                        }
                        else
                        {
                            PropValue = ParseBytes(length.Data);
                        }
                    }
                }
                else
                {
                    PropValue = Parse< PtypString8>(parser);
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("MetaPropValueGetPartial");
            if (PropType != null) AddChild(PropType, $"PropType:{PropType.Data}");
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            if (PropValue != null) AddChild(PropValue, $"PropValue:{PropValue}");
            if (length != null) AddChild(length, $"Length:{length.Data}");
        }
    }
}