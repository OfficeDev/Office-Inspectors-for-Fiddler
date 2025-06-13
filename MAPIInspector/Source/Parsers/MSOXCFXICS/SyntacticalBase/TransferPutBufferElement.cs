namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TransferPutBufferElement : SyntacticalBase
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
        public object Marker;

        /// <summary>
        /// Initializes a new instance of the TransferPutBufferElement class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TransferPutBufferElement(FastTransferStream stream)
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
            if (MapiInspector.MAPIParser.PartialPutType != 0 && MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialPutId))
                {
                    this.MetaValue = new MetaPropValuePutPartial(stream);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIParser.PartialPutType))
                    {
                        if (MapiInspector.MAPIParser.PartialPutType == (ushort)PropertyDataType.PtypInteger32 && MapiInspector.MAPIParser.PartialPutId == 0x4017)
                        {
                            this.PropValue = new VarPropTypePropValuePutPartial(stream);
                        }
                        else
                        {
                            this.PropValue = new FixedPropTypePropValuePutPartial(stream);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIParser.PartialPutType)
                    || LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialPutType))
                    {
                        this.PropValue = new VarPropTypePropValuePutPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIParser.PartialPutType))
                    {
                        this.PropValue = new MvPropTypePropValuePutPartial(stream);
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
                    this.MetaValue = new MetaPropValuePutPartial(stream);
                }
                else
                {
                    long streamPosition = stream.Position;
                    PropValue propValue = new PropValue(stream);
                    stream.Position = streamPosition;

                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new FixedPropTypePropValuePutPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)propValue.PropType) || PropValue.IsMetaTagIdsetGiven(stream)
                    || LexicalTypeHelper.IsCodePageType((ushort)propValue.PropType))
                    {
                        this.PropValue = new VarPropTypePropValuePutPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new MvPropTypePropValuePutPartial(stream);
                    }
                }
            }
        }
    }
}
