using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TransferPutBufferExtendElement : Block
    {
        /// <summary>
        /// MetaTagDnPrefix field
        /// </summary>
        public MetaPropValuePutExtendPartial MetaValue;

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
            if (MapiInspector.MAPIParser.PartialPutExtendType != 0 &&
                MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                var tmpMarker = TestParse<Markers>(parser);
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = ParseT<Markers>(parser);
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialPutExtendId))
                {
                    MetaValue = Parse<MetaPropValuePutExtendPartial>(parser);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        if (MapiInspector.MAPIParser.PartialPutExtendType == PropertyDataType.PtypInteger32 &&
                            MapiInspector.MAPIParser.PartialPutExtendId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValuePutExtendPartial>(parser);
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValuePutExtendPartial>(parser);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType) ||
                    LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutExtendPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        PropValue = Parse<MvPropTypePropValuePutExtendPartial>(parser);
                    }
                }
            }
            else
            {
                var tmpMarker = TestParse<Markers>(parser);
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = ParseT<Markers>(parser);
                }
                else if (MarkersHelper.IsMetaTag((MetaProperties)tmpMarker.Data))
                {
                    MetaValue = Parse<MetaPropValuePutExtendPartial>(parser);
                }
                else
                {
                    var offset = parser.Offset;
                    PropValue propValue = Parse<PropValue>(parser);
                    parser.Offset = offset;

                    if (LexicalTypeHelper.IsFixedType(propValue.PropType.Data) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<FixedPropTypePropValuePutExtendPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsVarType(propValue.PropType.Data) ||
                        PropValue.IsMetaTagIdsetGiven(parser) ||
                        LexicalTypeHelper.IsCodePageType(propValue.PropType.Data))
                    {
                        PropValue = Parse<VarPropTypePropValuePutExtendPartial>(parser);
                    }
                    else if (LexicalTypeHelper.IsMVType(propValue.PropType.Data) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<MvPropTypePropValuePutExtendPartial>(parser);
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TransferPutBufferExtendElement");
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            if (Marker != null) AddChild(Marker, $"Marker:{Marker.Data}");
        }
    }
}
