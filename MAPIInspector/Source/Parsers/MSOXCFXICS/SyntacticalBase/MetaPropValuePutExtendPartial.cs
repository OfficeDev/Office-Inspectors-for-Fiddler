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
            if (Partial.PartialPutExtendType == 0 ||
                (Partial.PartialPutType != 0 &&
                !(Partial.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                Partial.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                Partial.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                PropType = ParseT<PropertyDataType>();
                PropID = ParseT<PidTagPropertyEnum>();
            }

            if (parser.Empty)
            {
                Partial.PartialPutExtendType = PropType;
                Partial.PartialPutExtendId = PropID;
                Partial.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialPutExtendType != 0 &&
                    Partial.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    propertyType = Partial.PartialPutExtendType;
                    propertyID = Partial.PartialPutExtendId;

                    // clear
                    Partial.PartialPutExtendType = 0;
                    Partial.PartialPutExtendId = 0;

                    if (Partial.PartialPutExtendRemainSize == -1)
                    {
                        Partial.PartialPutExtendServerUrl = string.Empty;
                        Partial.PartialPutExtendProcessName = string.Empty;
                        Partial.PartialPutExtendClientInfo = string.Empty;
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
                        if (Partial.PartialPutExtendRemainSize != -1 &&
                            Partial.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                            Partial.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                            Partial.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            length = CreateBlock(Partial.PartialPutExtendRemainSize, 0, 0);

                            // clear
                            Partial.PartialPutExtendRemainSize = -1;
                            Partial.PartialPutExtendServerUrl = string.Empty;
                            Partial.PartialPutExtendProcessName = string.Empty;
                            Partial.PartialPutExtendClientInfo = string.Empty;
                        }
                        else
                        {
                            length = ParseT<int>();
                        }

                        if (parser.RemainingBytes < length)
                        {
                            Partial.PartialGetType = typeValue;
                            Partial.PartialGetId = identifyValue;
                            Partial.PartialPutExtendRemainSize = length - parser.RemainingBytes;
                            Partial.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            Partial.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            Partial.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];


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
            Text = "MetaPropValuePutExtendPartial";
            if (PropType!= null) AddChildBlockT(PropType, "PropType");
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            if (PropValue != null) AddChild(PropValue, $"PropValue:{PropValue.GetType().Name}");
            AddChildBlockT(length, "length");
        }
    }
}
