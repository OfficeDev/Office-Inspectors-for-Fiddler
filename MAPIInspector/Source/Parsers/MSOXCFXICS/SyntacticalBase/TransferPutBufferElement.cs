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
            if (Partial.PartialPutType != 0 &&
                Partial.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                Partial.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                Partial.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                var tmpMarker = TestParse<Markers>();
                if (MarkersHelper.IsMarker(tmpMarker))
                {
                    Marker = ParseT<Markers>();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(Partial.PartialPutId))
                {
                    MetaValue = Parse<MetaPropValuePutPartial>();
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType(Partial.PartialPutType))
                    {
                        if (Partial.PartialPutType == PropertyDataType.PtypInteger32 &&
                            Partial.PartialPutId == PidTagPropertyEnum.MetaTagIdsetGiven)
                        {
                            PropValue = Parse<VarPropTypePropValuePutPartial>();
                        }
                        else
                        {
                            PropValue = Parse<FixedPropTypePropValuePutPartial>();
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType(Partial.PartialPutType) ||
                    LexicalTypeHelper.IsCodePageType(Partial.PartialPutType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(Partial.PartialPutType))
                    {
                        PropValue = Parse<MvPropTypePropValuePutPartial>();
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
                    MetaValue = Parse<MetaPropValuePutPartial>();
                }
                else
                {
                    var offset = parser.Offset;
                    PropValue propValue = Parse<PropValue>();
                    parser.Offset = offset;

                    if (LexicalTypeHelper.IsFixedType(propValue.PropType) &&
                        !PropValue.IsMetaTagIdsetGiven(parser))
                    {
                        PropValue = Parse<FixedPropTypePropValuePutPartial>();
                    }
                    else if (LexicalTypeHelper.IsVarType(propValue.PropType) ||
                        PropValue.IsMetaTagIdsetGiven(parser) ||
                    LexicalTypeHelper.IsCodePageType(propValue.PropType))
                    {
                        PropValue = Parse<VarPropTypePropValuePutPartial>();
                    }
                    else if (LexicalTypeHelper.IsMVType(propValue.PropType) &&
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
