namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValuePutExtendPartial : SyntacticalBase
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public ushort? PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public ushort? PropID;

        /// <summary>
        /// The property value.
        /// </summary>
        public object PropValue;

        /// <summary>
        /// The property type for partial split.
        /// </summary>
        private ushort propertyType;

        /// <summary>
        /// The property id for partial split.
        /// </summary>
        private ushort propertyID;

        /// <summary>
        /// The length value is for ptypBinary
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the MetaPropValuePutExtendPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValuePutExtendPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIParser.PartialPutExtendType == 0 || (MapiInspector.MAPIParser.PartialPutType != 0 && !(MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                this.PropType = stream.ReadUInt16();
                this.PropID = stream.ReadUInt16();
            }

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIParser.PartialPutExtendType = (ushort)this.PropType;
                MapiInspector.MAPIParser.PartialPutExtendId = (ushort)this.PropID;
                MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutExtendType != 0 && MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.propertyType = MapiInspector.MAPIParser.PartialPutExtendType;
                    this.propertyID = MapiInspector.MAPIParser.PartialPutExtendId;

                    // clear
                    MapiInspector.MAPIParser.PartialPutExtendType = 0;
                    MapiInspector.MAPIParser.PartialPutExtendId = 0;

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                    }
                }

                ushort typeValue;
                ushort identifyValue;

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.propertyType;
                }

                if (this.PropID != null)
                {
                    identifyValue = (ushort)this.PropID;
                }
                else
                {
                    identifyValue = this.propertyID;
                }

                if (identifyValue != 0x4011 && identifyValue != 0x4008)
                {
                    this.PropValue = stream.ReadUInt32();
                }
                else if (identifyValue == 0x4011)
                {
                    PtypBinary ptypeBinary = new PtypBinary(CountWideEnum.fourBytes);

                    if (!stream.IsEndOfStream)
                    {
                        long spositon = stream.Position;

                        if (MapiInspector.MAPIParser.PartialPutExtendRemainSize != -1 && MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                            && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            this.length = MapiInspector.MAPIParser.PartialPutExtendRemainSize;

                            // clear
                            MapiInspector.MAPIParser.PartialPutExtendRemainSize = -1;
                            MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                            MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                            MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                        }
                        else
                        {
                            this.length = stream.ReadInt32();
                        }

                        if ((stream.Length - stream.Position) < this.length)
                        {
                            MapiInspector.MAPIParser.PartialGetType = typeValue;
                            MapiInspector.MAPIParser.PartialGetId = identifyValue;
                            MapiInspector.MAPIParser.PartialPutExtendRemainSize = this.length - (int)(stream.Length - stream.Position);
                            MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];

                            if (spositon != stream.Position)
                            {
                                ptypeBinary.Count = (int)(stream.Length - stream.Position);
                            }

                            ptypeBinary.Value = stream.ReadBlock(this.length);
                        }
                        else
                        {
                            stream.Position -= 4;
                            ptypeBinary.Parse(stream);
                        }

                        this.PropValue = ptypeBinary;
                    }
                }
                else
                {
                    PtypString8 pstring8 = new PtypString8();
                    pstring8.Parse(stream);
                    this.PropValue = pstring8;
                }
            }
        }
    }
}
