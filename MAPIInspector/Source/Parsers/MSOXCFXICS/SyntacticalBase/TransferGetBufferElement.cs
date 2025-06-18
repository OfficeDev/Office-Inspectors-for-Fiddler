using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
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
            if (MapiInspector.MAPIParser.PartialGetType != 0 &&
                MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                var tmpMarker = BlockParser.BlockT<Markers>.TestParse(parser);
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = ParseT<Markers>(parser);
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialGetId))
                {
                    MetaValue = Parse<MetaPropValueGetPartial>(parser);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType(MapiInspector.MAPIParser.PartialGetType) &&
                        MapiInspector.MAPIParser.PartialGetRemainSize == -1)
                    {
                        if (MapiInspector.MAPIParser.PartialGetType == PropertyDataType.PtypInteger32 &&
                            MapiInspector.MAPIParser.PartialGetId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValueGetPartial>(parser);
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValueGetPartial>(parser);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType(MapiInspector.MAPIParser.PartialGetType) ||
                    LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialGetType) ||
                    (LexicalTypeHelper.IsFixedType(MapiInspector.MAPIParser.PartialGetType) &&
                    MapiInspector.MAPIParser.PartialGetRemainSize != -1))
                    {
                        PropValue = Parse<VarPropTypePropValueGetPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsMVType(MapiInspector.MAPIParser.PartialGetType))
                    {
                        PropValue = Parse<MvPropTypePropValueGetPartial>(parser);
                    }
                }
            }
            else
            {
                var tmpMarker = BlockParser.BlockT<Markers>.TestParse(parser);
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = ParseT<Markers>(parser);
                }
                else if (MarkersHelper.IsMetaTag((MetaProperties)tmpMarker.Data))
                {
                    MetaValue = Parse<MetaPropValueGetPartial>(parser);
                }
                else
                {
                    var offset = parser.Offset;
                    PropValue propValue = Parse<PropValue>(parser);
                    parser.Offset = offset;

                    if (LexicalTypeHelper.IsFixedType(propValue.PropType.Data) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<FixedPropTypePropValueGetPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsVarType(propValue.PropType.Data) ||
                        PropValue.IsMetaTagIdsetGiven(parser) ||
                        LexicalTypeHelper.IsCodePageType(propValue.PropType.Data))
                    {
                        PropValue = Parse<VarPropTypePropValueGetPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsMVType(propValue.PropType.Data))
                    {
                        PropValue = Parse<MvPropTypePropValueGetPartial>(parser);
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TransferGetBufferElement");
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            if (Marker != null) AddChild(Marker, $"Marker:{Marker.Data}");
        }
    }
}
