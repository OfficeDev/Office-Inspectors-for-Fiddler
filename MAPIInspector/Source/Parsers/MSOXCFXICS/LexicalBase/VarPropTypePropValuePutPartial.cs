namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValuePutPartial : PropValue
    {
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public int? Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public object ValueArray;

        /// <summary>
        /// The length value used for partial split
        /// </summary>
        protected int plength;

        /// <summary>
        /// Boolean value used to record whether ptypString value is split to two bytes which parse in different buffer
        /// </summary>
        protected bool splitpreviousOne = false;

        /// <summary>
        /// Initializes a new instance of the VarPropTypePropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public VarPropTypePropValuePutPartial(FastTransferStream stream)
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

                    if (MapiInspector.MAPIParser.PartialPutRemainSize != -1)
                    {
                        this.plength = MapiInspector.MAPIParser.PartialPutRemainSize;
                        MapiInspector.MAPIParser.PartialPutRemainSize = -1;

                        if (this.plength % 2 != 0 && (this.ptype == (ushort)PropertyDataType.PtypString || this.ptype == (ushort)CodePageType.PtypCodePageUnicode || this.ptype == (ushort)CodePageType.ptypCodePageUnicode52))
                        {
                            this.splitpreviousOne = true;
                        }
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;

                    if (MapiInspector.MAPIParser.PartialPutRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
                    }
                }
                else
                {
                    this.Length = stream.ReadInt32();
                }

                int lengthValue;
                ushort typeValue;

                if (this.Length != null)
                {
                    lengthValue = (int)this.Length;
                }
                else
                {
                    lengthValue = this.plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if ((stream.Length - stream.Position) < lengthValue)
                {
                    MapiInspector.MAPIParser.PartialPutType = typeValue;
                    MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            PtypString pstring = new PtypString();

                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    stream.Position += 1;
                                    splitpreviousOne = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case CodePageType.ptypCodePageUnicode52:
                            {
                                PtypString pstringII = new PtypString();

                                if (this.Length != null)
                                {
                                    this.Length = stream.ReadInt32();
                                    lengthValue = (int)this.Length;
                                }

                                if (stream.Length - stream.Position < lengthValue)
                                {
                                    MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                    this.plength = (int)(stream.Length - stream.Position);
                                    lengthValue = this.plength;

                                    if (lengthValue != 0)
                                    {
                                        if (this.splitpreviousOne)
                                        {
                                            stream.Position += 1;
                                            this.splitpreviousOne = false;
                                        }

                                        if ((lengthValue / 2) != 0)
                                        {
                                            pstringII = new PtypString(lengthValue / 2);
                                            pstringII.Parse(stream);
                                        }
                                        else
                                        {
                                            pstringII = null;
                                        }

                                        if (lengthValue % 2 != 0)
                                        {
                                            stream.Position += 1;
                                        }
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }
                                else
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstringII = new PtypString(lengthValue / 2);
                                        pstringII.Parse(stream);
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }

                                this.ValueArray = pstringII;

                                break;
                            }

                        case CodePageType.PtypCodePageUnicodeBigendian:
                        case CodePageType.PtypCodePageWesternEuropean:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pdstring8 = new PtypString8(lengthValue);
                            pdstring8.Parse(stream);
                            this.ValueArray = pdstring8;
                            break;
                    }
                }
                else
                {
                    switch ((PropertyDataType)typeValue)
                    {
                        case PropertyDataType.PtypString:
                            PtypString pstring = new PtypString();
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                                if (lengthValue != 0)
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    stream.Position += 1;
                                    splitpreviousOne = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case PropertyDataType.PtypString8:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        case PropertyDataType.PtypBinary:
                        case PropertyDataType.PtypServerId:
                        case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                    }
                }
            }
        }
    }
}
