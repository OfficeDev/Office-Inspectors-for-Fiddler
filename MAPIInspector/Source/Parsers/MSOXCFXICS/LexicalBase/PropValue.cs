using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropValue represents identification information and the value of the property.
    /// 2.2.4.1 Lexical structure propValue
    /// 2.2.4.1.2 propValue Lexical Element
    /// </summary>
    public class PropValue : Block
    {
        /// <summary>
        /// The propType.
        /// </summary>
        public BlockT<PropertyDataType> PropType;

        /// <summary>
        /// The PropInfo.
        /// </summary>
        public PropInfo PropInfo;

        /// <summary>
        /// The propType for partial split
        /// </summary>
        protected BlockT<PropertyDataType> ptype;

        /// <summary>
        /// The PropId for partial split
        /// </summary>
        protected BlockT<PidTagPropertyEnum> pid;

        /// <summary>
        /// Indicate whether the stream's position is IsMetaTagIdsetGiven.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>True if the stream's position is IsMetaTagIdsetGiven,else false.</returns>
        public static bool IsMetaTagIdsetGiven(BinaryParser parser)
        {
            var offset = parser.Offset;
            var type = ParseT<PropertyDataType>(parser);
            var id = ParseT<PidTagPropertyEnum>(parser);
            parser.Offset = offset;
            if (!type.Parsed || !id.Parsed) return false;
            return type.Data == PropertyDataType.PtypInteger32 && id.Data == PidTagPropertyEnum.MetaTagIdsetGiven;
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized PropValue.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized PropValue, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            var tag = TestParse<Markers>(parser);
            if (!tag.Parsed) return false;
            return !parser.Empty &&
                (FixedPropTypePropValue.Verify(parser) ||
                VarPropTypePropValue.Verify(parser) ||
                MvPropTypePropValue.Verify(parser)) &&
                !MarkersHelper.IsMarker(tag.Data) &&
                !MarkersHelper.IsMetaTag((MetaProperties)tag.Data);
        }

        /// <summary>
        /// Parse a PropValue instance from a BinaryParser.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>A PropValue instance.</returns>
        public static PropValue ParseFrom(BinaryParser parser)
        {
            if (FixedPropTypePropValue.Verify(parser))
            {
                return Parse<FixedPropTypePropValue>(parser);
            }
            else if (VarPropTypePropValue.Verify(parser))
            {
                return Parse<VarPropTypePropValue>(parser);
            }
            else if (MvPropTypePropValue.Verify(parser))
            {
                return Parse<MvPropTypePropValue>(parser);
            }
            else
            {
                return null;
            }
        }

        protected override void Parse()
        {
            if ((MapiInspector.MAPIParser.IsPut == true && (MapiInspector.MAPIParser.PartialPutType == 0 || (MapiInspector.MAPIParser.PartialPutType != 0 && !(MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))) ||
                (MapiInspector.MAPIParser.IsGet == true && (MapiInspector.MAPIParser.PartialGetType == 0 || (MapiInspector.MAPIParser.PartialGetType != 0 && !(MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))) ||
                (MapiInspector.MAPIParser.IsPutExtend == true && (MapiInspector.MAPIParser.PartialPutExtendType == 0 || (MapiInspector.MAPIParser.PartialPutType != 0 && !(MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))))
            {
                PropType = ParseT<PropertyDataType>();
                PropInfo = Parse<PropInfo>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("PropValue");
            AddChildBlockT(PropType, "PropType"); // Consider: ({(ushort)PropType.Data:X4})
            AddChild(PropInfo);
        }
    }
}
