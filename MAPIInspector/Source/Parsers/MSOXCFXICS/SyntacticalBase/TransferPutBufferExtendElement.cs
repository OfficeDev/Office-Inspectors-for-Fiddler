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
                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker))
                {
                    Marker = ParseT<Markers>();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialPutExtendId))
                {
                    MetaValue = Parse<MetaPropValuePutExtendPartial>();
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        if (MapiInspector.MAPIParser.PartialPutExtendType == PropertyDataType.PtypInteger32 &&
                            MapiInspector.MAPIParser.PartialPutExtendId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValuePutExtendPartial>();
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValuePutExtendPartial>();
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType) ||
                    LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutExtendPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        PropValue = Parse<MvPropTypePropValuePutExtendPartial>();
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
                    MetaValue = Parse<MetaPropValuePutExtendPartial>();
                }
                else
                {
                    var offset = parser.Offset;
                    PropValue propValue = Parse<PropValue>();
                    parser.Offset = offset;

                    if (LexicalTypeHelper.IsFixedType(propValue.PropType) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<FixedPropTypePropValuePutExtendPartial>();
                    }
                    else if (LexicalTypeHelper.IsVarType(propValue.PropType) ||
                        PropValue.IsMetaTagIdsetGiven(parser) ||
                        LexicalTypeHelper.IsCodePageType(propValue.PropType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutExtendPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(propValue.PropType) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<MvPropTypePropValuePutExtendPartial>();
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TransferPutBufferExtendElement");
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            AddChildBlockT(Marker, "Marker");
        }
    }
}
