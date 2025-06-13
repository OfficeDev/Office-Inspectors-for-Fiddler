namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TransferPutBufferExtendElement : SyntacticalBase
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
        public object Marker;

        /// <summary>
        /// Initializes a new instance of the TransferPutBufferExtendElement class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TransferPutBufferExtendElement(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify a stream's current position contains a serialized TopFolder.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized TopFolder, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream;
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIParser.PartialPutExtendType != 0 && MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialPutExtendId))
                {
                    this.MetaValue = new MetaPropValuePutExtendPartial(stream);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        if (MapiInspector.MAPIParser.PartialPutExtendType == (ushort)PropertyDataType.PtypInteger32 && MapiInspector.MAPIParser.PartialPutExtendId == 0x4017)
                        {
                            this.PropValue = new VarPropTypePropValuePutExtendPartial(stream);
                        }
                        else
                        {
                            this.PropValue = new FixedPropTypePropValuePutExtendPartial(stream);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType)
                    || LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        this.PropValue = new VarPropTypePropValuePutExtendPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIParser.PartialPutExtendType))
                    {
                        this.PropValue = new MvPropTypePropValuePutExtendPartial(stream);
                    }
                }
            }
            else
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (MarkersHelper.IsMetaTag(stream.VerifyUInt32()))
                {
                    this.MetaValue = new MetaPropValuePutExtendPartial(stream);
                }
                else
                {
                    long streamPosition = stream.Position;
                    PropValue propValue = new PropValue(stream);
                    stream.Position = streamPosition;

                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new FixedPropTypePropValuePutExtendPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)propValue.PropType) || PropValue.IsMetaTagIdsetGiven(stream)
                    || LexicalTypeHelper.IsCodePageType((ushort)propValue.PropType))
                    {
                        this.PropValue = new VarPropTypePropValuePutExtendPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new MvPropTypePropValuePutExtendPartial(stream);
                    }
                }
            }
        }
    }
}
