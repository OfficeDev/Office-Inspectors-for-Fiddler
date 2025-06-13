namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValuePutPartial : SyntacticalBase
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
        /// Initializes a new instance of the MetaPropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValuePutPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIParser.PartialPutType == 0 || (MapiInspector.MAPIParser.PartialPutType != 0 && !(MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                this.PropType = stream.ReadUInt16();
                this.PropID = stream.ReadUInt16();
            }

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIParser.PartialPutType = (ushort)this.PropType;
                MapiInspector.MAPIParser.PartialPutId = (ushort)this.PropID;
                MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutType != 0 && MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.propertyType = MapiInspector.MAPIParser.PartialPutType;
                    this.propertyID = MapiInspector.MAPIParser.PartialPutId;

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;
                    MapiInspector.MAPIParser.PartialPutId = 0;

                    if (MapiInspector.MAPIParser.PartialPutRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
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

                        if (MapiInspector.MAPIParser.PartialPutRemainSize != -1 && MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                            && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            this.length = MapiInspector.MAPIParser.PartialPutRemainSize;

                            // clear
                            MapiInspector.MAPIParser.PartialPutRemainSize = -1;
                            MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                            MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                            MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
                        }
                        else
                        {
                            this.length = stream.ReadInt32();
                        }

                        if ((stream.Length - stream.Position) < this.length)
                        {
                            MapiInspector.MAPIParser.PartialPutType = typeValue;
                            MapiInspector.MAPIParser.PartialPutId = identifyValue;
                            MapiInspector.MAPIParser.PartialPutRemainSize = this.length - (int)(stream.Length - stream.Position);
                            MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];

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
