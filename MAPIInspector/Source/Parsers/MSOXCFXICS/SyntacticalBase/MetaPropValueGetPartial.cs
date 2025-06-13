namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValueGetPartial : SyntacticalBase
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public PropertyDataType? PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public PidTagPropertyEnum? PropID;

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
        /// Initializes a new instance of the MetaPropValueGetPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValueGetPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIParser.PartialGetType == 0 || (MapiInspector.MAPIParser.PartialGetType != 0 && !(MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                this.PropType = (PropertyDataType)stream.ReadUInt16();
                this.PropID = (PidTagPropertyEnum)stream.ReadUInt16();
            }

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIParser.PartialGetType = (ushort)this.PropType;
                MapiInspector.MAPIParser.PartialGetId = (ushort)this.PropID;
                MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialGetType != 0 && MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.propertyType = MapiInspector.MAPIParser.PartialGetType;
                    this.propertyID = MapiInspector.MAPIParser.PartialGetId;

                    // clear
                    MapiInspector.MAPIParser.PartialGetType = 0;
                    MapiInspector.MAPIParser.PartialGetId = 0;

                    if (MapiInspector.MAPIParser.PartialGetRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialGetClientInfo = string.Empty;
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

                        if (MapiInspector.MAPIParser.PartialGetRemainSize != -1 && MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                            && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            this.length = MapiInspector.MAPIParser.PartialGetRemainSize;

                            // clear
                            MapiInspector.MAPIParser.PartialGetRemainSize = -1;
                            MapiInspector.MAPIParser.PartialGetServerUrl = string.Empty;
                            MapiInspector.MAPIParser.PartialGetProcessName = string.Empty;
                            MapiInspector.MAPIParser.PartialGetClientInfo = string.Empty;
                        }
                        else
                        {
                            this.length = stream.ReadInt32();
                        }

                        if ((stream.Length - stream.Position) < this.length)
                        {
                            MapiInspector.MAPIParser.PartialGetType = typeValue;
                            MapiInspector.MAPIParser.PartialGetId = identifyValue;
                            MapiInspector.MAPIParser.PartialGetRemainSize = this.length - (int)(stream.Length - stream.Position);
                            MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                            MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];

                            if (spositon != stream.Position)
                            {
                                // the length value is from the previous RopBuffer
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
