namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValuePutPartial : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public object FixedValue;

        /// <summary>
        /// Initializes a new instance of the FixedPropTypePropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public FixedPropTypePropValuePutPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIParser.PartialPutType = (ushort)this.PropType;
                MapiInspector.MAPIParser.PartialPutId = (ushort)this.PropInfo.PropID;
                MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutType != 0 && MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIParser.PartialPutType;
                    this.pid = (PidTagPropertyEnum)MapiInspector.MAPIParser.PartialPutId;

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;
                    MapiInspector.MAPIParser.PartialPutId = 0;
                    MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                    MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                    MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
                }

                ushort typeValue;
                ushort identifyValue;

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if (this.PropInfo != null)
                {
                    identifyValue = (ushort)this.PropInfo.PropID;
                }
                else
                {
                    identifyValue = (ushort)this.pid;
                }

                switch ((PropertyDataType)typeValue)
                {
                    case PropertyDataType.PtypInteger16:
                        this.FixedValue = stream.ReadInt16();
                        break;
                    case PropertyDataType.PtypInteger32:
                        if (identifyValue == 0x67A4)
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
                        if (identifyValue == 0x6714)
                        {
                            CN tmpCN = new CN();
                            tmpCN.Parse(stream);
                            this.FixedValue = tmpCN;
                        }
                        else if (identifyValue == 0x674A)
                        {
                            MessageID tmpMID = new MessageID();
                            tmpMID.Parse(stream);
                            this.FixedValue = tmpMID;
                        }
                        else if (identifyValue == 0x6748)
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
}
