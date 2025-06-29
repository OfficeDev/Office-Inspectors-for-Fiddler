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
            if (Partial.PartialPutType == 0 ||
                (Partial.PartialPutType != 0 &&
                !(Partial.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                Partial.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                Partial.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                PropType = ParseT<PropertyDataType>();
                PropID = ParseT<PidTagPropertyEnum>();
            }

            if (parser.Empty)
            {
                Partial.PartialPutType = PropType;
                Partial.PartialPutId = PropID;
                Partial.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialPutType != 0 &&
                    Partial.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    propertyType = Partial.PartialPutType;
                    propertyID = Partial.PartialPutId;

                    // clear
                    Partial.PartialPutType = 0;
                    Partial.PartialPutId = 0;

                    if (Partial.PartialPutRemainSize == -1)
                    {
                        Partial.PartialPutServerUrl = string.Empty;
                        Partial.PartialPutProcessName = string.Empty;
                        Partial.PartialPutClientInfo = string.Empty;
                    }
                }

                PropertyDataType typeValue = PropType != null ? PropType : propertyType;
                PidTagPropertyEnum identifyValue = PropID != null ? PropID : propertyID;
                if (identifyValue != PidTagPropertyEnum.MetaTagNewFXFolder && identifyValue != PidTagPropertyEnum.MetaTagDnPrefix)
                {
                    PropValue = ParseT<uint>();
                }
                else if (identifyValue == PidTagPropertyEnum.MetaTagNewFXFolder)
                {
                    if (!parser.Empty)
                    {
                        if (Partial.PartialPutRemainSize != -1 &&
                            Partial.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                            Partial.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                            Partial.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            length = CreateBlock(Partial.PartialPutRemainSize, 0, 0);

                            // clear
                            Partial.PartialPutRemainSize = -1;
                            Partial.PartialPutServerUrl = string.Empty;
                            Partial.PartialPutProcessName = string.Empty;
                            Partial.PartialPutClientInfo = string.Empty;
                        }
                        else
                        {
                            length = ParseT<int>();
                        }

                        if (parser.RemainingBytes < length)
                        {
                            Partial.PartialPutType = typeValue;
                            Partial.PartialPutId = identifyValue;
                            Partial.PartialPutRemainSize = length - parser.RemainingBytes;
                            Partial.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            Partial.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            Partial.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];

                            PropValue = ParseBytes(parser.RemainingBytes);
                        }
                        else
                        {
                            PropValue = ParseBytes(length);
                        }
                    }
                }
                else
                {
                    PropValue = Parse<PtypString8>();
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("MetaPropValuePutPartial");
            AddChildBlockT(PropType, "PropType");
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            if (PropValue != null) AddChild(PropValue, $"PropValue:{PropValue.GetType().Name}");
            AddChildBlockT(length, "length");
        }
    }
}
