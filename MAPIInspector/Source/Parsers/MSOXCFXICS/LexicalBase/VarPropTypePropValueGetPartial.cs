namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValueGetPartial : PropValue
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
        /// Initializes a new instance of the VarPropTypePropValueGetPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public VarPropTypePropValueGetPartial(FastTransferStream stream)
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
                MapiInspector.MAPIParser.PartialGetType = (ushort)this.PropType;
                MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialGetType != 0 && MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIParser.PartialGetType;

                    if (MapiInspector.MAPIParser.PartialGetRemainSize != -1)
                    {
                        this.plength = MapiInspector.MAPIParser.PartialGetRemainSize;
                        if (this.plength % 2 != 0 && (this.ptype == (ushort)PropertyDataType.PtypString || this.ptype == (ushort)CodePageType.PtypCodePageUnicode || this.ptype == (ushort)CodePageType.ptypCodePageUnicode52))
                        {
                            MapiInspector.MAPIParser.IsOneMoreByteToRead = true;
                        }

                        MapiInspector.MAPIParser.PartialGetRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

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
                    MapiInspector.MAPIParser.PartialGetType = typeValue;
                    MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            PtypString pstring = new PtypString();

                            if (stream.Length - stream.Position < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (MapiInspector.MAPIParser.IsOneMoreByteToRead)
                                    {
                                        stream.Position += 1;
                                        MapiInspector.MAPIParser.IsOneMoreByteToRead = false;
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
                                if (MapiInspector.MAPIParser.IsOneMoreByteToRead)
                                {
                                    stream.Position += 1;
                                    MapiInspector.MAPIParser.IsOneMoreByteToRead = false;
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
                                    MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                    this.plength = (int)(stream.Length - stream.Position);
                                    lengthValue = this.plength;

                                    if (lengthValue != 0)
                                    {
                                        if (MapiInspector.MAPIParser.IsOneMoreByteToRead)
                                        {
                                            stream.Position += 1;
                                            MapiInspector.MAPIParser.IsOneMoreByteToRead = false;
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
                                    if (MapiInspector.MAPIParser.IsOneMoreByteToRead)
                                    {
                                        stream.Position += 1;
                                        MapiInspector.MAPIParser.IsOneMoreByteToRead = false;
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
                                MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
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
                                MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
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

                            if (stream.Length - stream.Position < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (MapiInspector.MAPIParser.IsOneMoreByteToRead)
                                    {
                                        stream.Position += 1;
                                        MapiInspector.MAPIParser.IsOneMoreByteToRead = false;
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
                                if (MapiInspector.MAPIParser.IsOneMoreByteToRead)
                                {
                                    stream.Position += 1;
                                    MapiInspector.MAPIParser.IsOneMoreByteToRead = false;
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
                                MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
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
                                MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIParser.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
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
