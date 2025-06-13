namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValue : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public object FixedValue;

        /// <summary>
        /// Initializes a new instance of the FixedPropTypePropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public FixedPropTypePropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized FixedPropTypePropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized FixedPropTypePropValue, return true, else false</returns>
        public static new bool Verify(FastTransferStream stream)
        {
            ushort tmp = stream.VerifyUInt16();
            return LexicalTypeHelper.IsFixedType((PropertyDataType)tmp)
                && !PropValue.IsMetaTagIdsetGiven(stream);
        }

        /// <summary>
        /// Parse a DispidNamedPropInfo instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A DispidNamedPropInfo instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new FixedPropTypePropValue(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            PropertyDataType type = (PropertyDataType)this.PropType;

            switch (type)
            {
                case PropertyDataType.PtypInteger16:
                    this.FixedValue = stream.ReadInt16();
                    break;
                case PropertyDataType.PtypInteger32:
                    if ((ushort)this.PropInfo.PropID == 0x67A4)
                    {
                        CN tmpCN = new CN();
                        tmpCN.Parse(stream);
                        this.FixedValue = tmpCN;
                    }
                    else
                    {
                        this.FixedValue = stream.ReadInt32();
                    }

                    break;
                case PropertyDataType.PtypFloating32:
                    this.FixedValue = stream.ReadFloating32();
                    break;
                case PropertyDataType.PtypFloating64:
                    this.FixedValue = stream.ReadFloating64();
                    break;
                case PropertyDataType.PtypCurrency:
                    this.FixedValue = stream.ReadCurrency();
                    break;
                case PropertyDataType.PtypFloatingTime:
                    this.FixedValue = stream.ReadFloatingTime();
                    break;
                case PropertyDataType.PtypBoolean:
                    this.FixedValue = stream.ReadBoolean();
                    break;
                case PropertyDataType.PtypInteger64:
                    if ((ushort)this.PropInfo.PropID == 0x6714)
                    {
                        CN tmpCN = new CN();
                        tmpCN.Parse(stream);
                        this.FixedValue = tmpCN;
                    }
                    else if ((ushort)base.PropInfo.PropID == 0x674A)
                    {
                        MessageID tmpMID = new MessageID();
                        tmpMID.Parse(stream);
                        this.FixedValue = tmpMID;
                    }
                    else if ((ushort)base.PropInfo.PropID == 0x6748)
                    {
                        FolderID tmpFID = new FolderID();
                        tmpFID.Parse(stream);
                        this.FixedValue = tmpFID;
                    }
                    else
                    {
                        this.FixedValue = stream.ReadInt64();
                    }

                    break;
                case PropertyDataType.PtypTime:
                    PtypTime tempPropertyValue = new PtypTime();
                    tempPropertyValue.Parse(stream);
                    this.FixedValue = tempPropertyValue;
                    break;
                case PropertyDataType.PtypGuid:
                    this.FixedValue = stream.ReadGuid();
                    break;
            }
        }
    }
}
