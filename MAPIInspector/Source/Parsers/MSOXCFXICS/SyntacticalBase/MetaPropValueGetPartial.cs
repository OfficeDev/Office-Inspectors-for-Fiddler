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

        private Block Comment;

        protected override void Parse()
        {
            if (Partial.PartialGetType == 0 ||
                (Partial.PartialGetType != 0 &&
                !(Partial.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                Partial.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                Partial.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                PropType = ParseT<PropertyDataType>();
                PropID = ParseT<PidTagPropertyEnum>();
            }

            if (parser.Empty)
            {
                Partial.PartialGetType = PropType;
                Partial.PartialGetId = PropID;
                Partial.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialGetType != 0 &&
                    Partial.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    Comment = Partial.CreatePartialComment();

                    propertyType = Partial.PartialGetType;
                    propertyID = Partial.PartialGetId;

                    // clear
                    Partial.PartialGetType = 0;
                    Partial.PartialGetId = 0;

                    if (Partial.PartialGetRemainSize == -1)
                    {
                        Partial.PartialGetServerUrl = string.Empty;
                        Partial.PartialGetProcessName = string.Empty;
                        Partial.PartialGetClientInfo = string.Empty;
                    }
                }

                PropertyDataType typeValue = PropType != null ? PropType : propertyType;
                PidTagPropertyEnum identifyValue = PropID != null ? PropID : propertyID;
                if (identifyValue != PidTagPropertyEnum.MetaTagNewFXFolder && identifyValue != PidTagPropertyEnum.MetaTagDnPrefix)
                {
                    PropValue = Parse<PtypInteger32>();
                }
                else if (identifyValue == PidTagPropertyEnum.MetaTagNewFXFolder)
                {
                    if (!parser.Empty)
                    {
                        if (Partial.PartialGetRemainSize != -1 &&
                            Partial.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                            Partial.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                            Partial.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            length = CreateBlock(Partial.PartialGetRemainSize, 0, 0);

                            // clear
                            Partial.PartialGetRemainSize = -1;
                            Partial.PartialGetServerUrl = string.Empty;
                            Partial.PartialGetProcessName = string.Empty;
                            Partial.PartialGetClientInfo = string.Empty;
                        }
                        else
                        {
                            length = ParseT<int>();
                        }

                        if (parser.RemainingBytes < length)
                        {
                            Partial.PartialGetType = typeValue;
                            Partial.PartialGetId = identifyValue;
                            Partial.PartialGetRemainSize = length - parser.RemainingBytes;
                            Partial.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            Partial.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            Partial.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];

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
            Text = "MetaPropValueGetPartial";
            AddChild(Comment);
            AddChildBlockT(PropType, "PropType");
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            if (PropValue != null)
            {
                if (PropValue is PtypInteger32 int32)
                {
                    AddChild(PropValue, $"PropValue:0x{int32.Value.Data:X}");
                }
                else
                {
                    AddChild(PropValue, $"PropValue:{PropValue}");
                }
            }
            AddChildBlockT(length, "length");
        }
    }
}