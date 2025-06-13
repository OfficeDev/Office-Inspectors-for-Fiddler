namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropValue represents identification information and the value of the property.
    /// </summary>
    public class PropValue : LexicalBase
    {
        /// <summary>
        /// The propType.
        /// </summary>
        public PropertyDataType? PropType;

        /// <summary>
        /// The PropInfo.
        /// </summary>
        public PropInfo PropInfo;

        /// <summary>
        /// The propType for partial split
        /// </summary>
        protected ushort ptype;

        /// <summary>
        /// The PropId for partial split
        /// </summary>
        protected PidTagPropertyEnum pid;

        /// <summary>
        /// Initializes a new instance of the PropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public PropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Indicate whether the stream's position is IsMetaTagIdsetGiven.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>True if the stream's position is IsMetaTagIdsetGiven,else false.</returns>
        public static bool IsMetaTagIdsetGiven(FastTransferStream stream)
        {
            ushort type = stream.VerifyUInt16();
            ushort id = stream.VerifyUInt16(2);
            return type == (ushort)PropertyDataType.PtypInteger32 && id == 0x4017;
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized PropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized PropValue, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream
                && (FixedPropTypePropValue.Verify(stream) || VarPropTypePropValue.Verify(stream) || MvPropTypePropValue.Verify(stream))
                && !MarkersHelper.IsMarker(stream.VerifyUInt32())
                && !MarkersHelper.IsMetaTag(stream.VerifyUInt32());
        }

        /// <summary>
        /// Parse a PropValue instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A PropValue instance.</returns>
        public static LexicalBase ParseFrom(FastTransferStream stream)
        {
            if (FixedPropTypePropValue.Verify(stream))
            {
                return FixedPropTypePropValue.ParseFrom(stream);
            }
            else if (VarPropTypePropValue.Verify(stream))
            {
                return VarPropTypePropValue.ParseFrom(stream);
            }
            else if (MvPropTypePropValue.Verify(stream))
            {
                return MvPropTypePropValue.ParseFrom(stream);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            if ((MapiInspector.MAPIParser.IsPut == true && (MapiInspector.MAPIParser.PartialPutType == 0 || (MapiInspector.MAPIParser.PartialPutType != 0 && !(MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))) ||
                (MapiInspector.MAPIParser.IsGet == true && (MapiInspector.MAPIParser.PartialGetType == 0 || (MapiInspector.MAPIParser.PartialGetType != 0 && !(MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))) ||
                (MapiInspector.MAPIParser.IsPutExtend == true && (MapiInspector.MAPIParser.PartialPutExtendType == 0 || (MapiInspector.MAPIParser.PartialPutType != 0 && !(MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))))
            {
                this.PropType = (PropertyDataType)stream.ReadUInt16();
                this.PropInfo = PropInfo.ParseFrom(stream) as PropInfo;
            }
        }
    }
}
