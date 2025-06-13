namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TransferGetBufferElement : SyntacticalBase
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
        public object Marker;

        /// <summary>
        /// Initializes a new instance of the TransferGetBufferElement class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TransferGetBufferElement(FastTransferStream stream)
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
            if (MapiInspector.MAPIParser.PartialGetType != 0 && MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIParser.PartialGetId))
                {
                    this.MetaValue = new MetaPropValueGetPartial(stream);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIParser.PartialGetType) && MapiInspector.MAPIParser.PartialGetRemainSize == -1)
                    {
                        if (MapiInspector.MAPIParser.PartialGetType == (ushort)PropertyDataType.PtypInteger32 && MapiInspector.MAPIParser.PartialGetId == 0x4017)
                        {
                            this.PropValue = new VarPropTypePropValueGetPartial(stream);
                        }
                        else
                        {
                            this.PropValue = new FixedPropTypePropValueGetPartial(stream);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIParser.PartialGetType)
                    || LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIParser.PartialGetType) ||
                    (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIParser.PartialGetType) && MapiInspector.MAPIParser.PartialGetRemainSize != -1))
                    {
                        this.PropValue = new VarPropTypePropValueGetPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIParser.PartialGetType))
                    {
                        this.PropValue = new MvPropTypePropValueGetPartial(stream);
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
                    this.MetaValue = new MetaPropValueGetPartial(stream);
                }
                else
                {
                    long streamPosition = stream.Position;
                    PropValue propertyValue = new PropValue(stream);
                    stream.Position = streamPosition;

                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)propertyValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new FixedPropTypePropValueGetPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)propertyValue.PropType) || PropValue.IsMetaTagIdsetGiven(stream)
                    || LexicalTypeHelper.IsCodePageType((ushort)propertyValue.PropType))
                    {
                        this.PropValue = new VarPropTypePropValueGetPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)propertyValue.PropType))
                    {
                        this.PropValue = new MvPropTypePropValueGetPartial(stream);
                    }
                }
            }
        }
    }
}
