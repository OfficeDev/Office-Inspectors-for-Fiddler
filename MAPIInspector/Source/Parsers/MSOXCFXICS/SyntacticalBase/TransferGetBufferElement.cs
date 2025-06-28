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

        private Block Comment;

        protected override void Parse()
        {
            if (MapiInspector.MAPIParser.PartialGetType != 0 &&
                MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                Comment = Create("Partial Details");
                Comment.AddHeader($"PartialGetType:{MapiInspector.MAPIParser.PartialGetType:X}");
                Comment.AddHeader($"PartialGetId:{MapiInspector.MAPIParser.PartialGetId}");
                Comment.AddHeader($"PartialGetRemainSize:{MapiInspector.MAPIParser.PartialGetRemainSize}");

                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker))
                {
                    Marker = ParseT<Markers>();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialGetId))
                {
                    MetaValue = Parse<MetaPropValueGetPartial>();
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType(MapiInspector.MAPIParser.PartialGetType) &&
                        MapiInspector.MAPIParser.PartialGetRemainSize == -1)
                    {
                        if (MapiInspector.MAPIParser.PartialGetType == PropertyDataType.PtypInteger32 &&
                            MapiInspector.MAPIParser.PartialGetId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValueGetPartial>();
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValueGetPartial>();
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType(MapiInspector.MAPIParser.PartialGetType) ||
                    LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialGetType) ||
                    (LexicalTypeHelper.IsFixedType(MapiInspector.MAPIParser.PartialGetType) &&
                    MapiInspector.MAPIParser.PartialGetRemainSize != -1))
                    {
                        PropValue = Parse<VarPropTypePropValueGetPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(MapiInspector.MAPIParser.PartialGetType))
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
            AddChild(Comment);
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            AddChildBlockT(Marker, "Marker");
        }
    }
}
