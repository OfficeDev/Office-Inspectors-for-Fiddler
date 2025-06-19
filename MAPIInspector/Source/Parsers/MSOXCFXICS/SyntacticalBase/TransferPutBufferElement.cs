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
                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = ParseT<Markers>();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialPutId))
                {
                    MetaValue = Parse<MetaPropValuePutPartial>();
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType(MapiInspector.MAPIParser.PartialPutType))
                    {
                        if (MapiInspector.MAPIParser.PartialPutType == PropertyDataType.PtypInteger32 &&
                            MapiInspector.MAPIParser.PartialPutId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValuePutPartial>();
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValuePutPartial>();
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType(MapiInspector.MAPIParser.PartialPutType) ||
                    LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialPutType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(MapiInspector.MAPIParser.PartialPutType))
                    {
                        PropValue = Parse<MvPropTypePropValuePutPartial>();
                    }
                }
            }
            else
            {
                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker.Data))
                {
                    Marker = ParseT<Markers>();
                }
                else if (MarkersHelper.IsMetaTag((MetaProperties)tmpMarker.Data))
                {
                    MetaValue = Parse<MetaPropValuePutPartial>();
                }
                else
                {
                    var offset = parser.Offset;
                    PropValue propValue = Parse<PropValue>();
                    parser.Offset = offset;

                    if (LexicalTypeHelper.IsFixedType(propValue.PropType.Data) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<FixedPropTypePropValuePutPartial>();
                    }
                    else if (LexicalTypeHelper.IsVarType(propValue.PropType.Data) ||
                        PropValue.IsMetaTagIdsetGiven(parser) ||
                    LexicalTypeHelper.IsCodePageType(propValue.PropType.Data))
                    {
                        PropValue = Parse<VarPropTypePropValuePutPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(propValue.PropType.Data) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<MvPropTypePropValuePutPartial>();
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TransferPutBufferElement");
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            AddChildBlockT(Marker, "Marker");
        }
    }
}
