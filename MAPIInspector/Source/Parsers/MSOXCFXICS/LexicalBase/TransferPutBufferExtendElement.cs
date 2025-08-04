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
            if (Partial.PartialPutExtendType != 0 &&
                Partial.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                Partial.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                Partial.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker))
                {
                    Marker = ParseT<Markers>();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(Partial.PartialPutExtendId))
                {
                    MetaValue = Parse<MetaPropValuePutExtendPartial>();
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType(Partial.PartialPutExtendType))
                    {
                        if (Partial.PartialPutExtendType == PropertyDataType.PtypInteger32 &&
                            Partial.PartialPutExtendId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValuePutExtendPartial>();
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValuePutExtendPartial>();
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType(Partial.PartialPutExtendType) ||
                    LexicalTypeHelper.IsCodePageType(Partial.PartialPutExtendType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutExtendPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(Partial.PartialPutExtendType))
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
            Text = "TransferPutBufferExtendElement";
            AddChild(MetaValue, "MetaValue");
            AddChild(PropValue, "PropValue");
            AddChildBlockT(Marker, "Marker");
        }
    }
}
