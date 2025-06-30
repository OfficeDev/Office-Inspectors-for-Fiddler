using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4 FastTransfer Stream
    /// </summary>
    public class TransferGetBufferElement : Block
    {
        /// <summary>
        /// MetaTagDnPrefix field
        /// </summary>
        public MetaPropValueGetPartial MetaValue;

        /// <summary>
        /// PropValue field
        /// </summary>
        public PropValue PropValue;

        /// <summary>
        /// Marker field
        /// </summary>
        public BlockT<Markers> Marker;

        protected override void Parse()
        {
            if (Partial.PartialGetType != 0 &&
                Partial.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                Partial.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                Partial.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker))
                {
                    Marker = ParseT<Markers>();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(Partial.PartialGetId))
                {
                    MetaValue = Parse<MetaPropValueGetPartial>();
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType(Partial.PartialGetType) &&
                        Partial.PartialGetRemainSize == -1)
                    {
                        if (Partial.PartialGetType == PropertyDataType.PtypInteger32 &&
                            Partial.PartialGetId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValueGetPartial>();
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValueGetPartial>();
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType(Partial.PartialGetType) ||
                    LexicalTypeHelper.IsCodePageType(Partial.PartialGetType) ||
                    (LexicalTypeHelper.IsFixedType(Partial.PartialGetType) &&
                    Partial.PartialGetRemainSize != -1))
                    {
                        PropValue = Parse<VarPropTypePropValueGetPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(Partial.PartialGetType))
                    {
                        PropValue = Parse<MvPropTypePropValueGetPartial>();
                    }
                }
            }
            else
            {
                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker))
                {
                    Marker = ParseT<Markers>();
                }
                else if (MarkersHelper.IsMetaTag((MetaProperties)tmpMarker.Data))
                {
                    MetaValue = Parse<MetaPropValueGetPartial>();
                }
                else
                {
                    var offset = parser.Offset;
                    PropValue propValue = Parse<PropValue>();
                    parser.Offset = offset;

                    if (LexicalTypeHelper.IsFixedType(propValue.PropType) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<FixedPropTypePropValueGetPartial>();
                    }
                    else if (LexicalTypeHelper.IsVarType(propValue.PropType) ||
                        PropValue.IsMetaTagIdsetGiven(parser) ||
                        LexicalTypeHelper.IsCodePageType(propValue.PropType))
                    {
                        PropValue = Parse<VarPropTypePropValueGetPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(propValue.PropType))
                    {
                        PropValue = Parse<MvPropTypePropValueGetPartial>();
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TransferGetBufferElement");
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            AddChildBlockT(Marker, "Marker");
        }
    }
}
