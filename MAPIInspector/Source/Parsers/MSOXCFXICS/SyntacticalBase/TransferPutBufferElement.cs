using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TransferPutBufferElement : Block
    {
        /// <summary>
        /// MetaTagDnPrefix field
        /// </summary>
        public MetaPropValuePutPartial MetaValue;

        /// <summary>
        /// PropValue  field
        /// </summary>
        public PropValue PropValue;

        /// <summary>
        /// Marker field
        /// </summary>
        public BlockT<Markers> Marker;

        protected override void Parse()
        {
            if (MapiInspector.MAPIParser.PartialPutType != 0 &&
                MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                var tmpMarker = BlockParser.BlockT<Markers>.TestParse(parser);
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = BlockT<Markers>(parser);
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialPutId))
                {
                    MetaValue = Parse<MetaPropValuePutPartial>(parser);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType(MapiInspector.MAPIParser.PartialPutType))
                    {
                        if (MapiInspector.MAPIParser.PartialPutType == PropertyDataType.PtypInteger32 &&
                            MapiInspector.MAPIParser.PartialPutId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValuePutPartial>(parser);
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValuePutPartial>(parser);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType(MapiInspector.MAPIParser.PartialPutType) ||
                    LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialPutType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsMVType(MapiInspector.MAPIParser.PartialPutType))
                    {
                        PropValue = Parse<MvPropTypePropValuePutPartial>(parser);
                    }
                }
            }
            else
            {
                var tmpMarker = BlockParser.BlockT<Markers>.TestParse(parser);
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = BlockT<Markers>(parser);
                }
                else if (MarkersHelper.IsMetaTag((MetaProperties)tmpMarker.Data))
                {
                    MetaValue = Parse<MetaPropValuePutPartial>(parser);
                }
                else
                {
                    var offset = parser.Offset;
                    PropValue propValue = Parse<PropValue>(parser);
                    parser.Offset = offset;

                    if (LexicalTypeHelper.IsFixedType(propValue.PropType.Data) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<FixedPropTypePropValuePutPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsVarType(propValue.PropType.Data) ||
                        PropValue.IsMetaTagIdsetGiven(parser) ||
                    LexicalTypeHelper.IsCodePageType(propValue.PropType.Data))
                    {
                        PropValue = Parse<VarPropTypePropValuePutPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsMVType(propValue.PropType.Data) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<MvPropTypePropValuePutPartial>(parser);
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TransferPutBufferElement");
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            if (Marker != null) AddChild(Marker, $"Marker:{Marker.Data}");
        }
    }
}
