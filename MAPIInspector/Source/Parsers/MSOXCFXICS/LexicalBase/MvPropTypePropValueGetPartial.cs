namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValueGetPartial : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public int? Length;

        /// <summary>
        /// A list of fixed size values.
        /// </summary>
        public byte[][] FixedSizeValueList;

        /// <summary>
        /// A list of LengthOfBlock.
        /// </summary>
        public LengthOfBlock[] VarSizeValueList;

        /// <summary>
        /// Length value for partial split
        /// </summary>
        private int Plength;

        /// <summary>
        /// Initializes a new instance of the MvPropTypePropValueGetPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValueGetPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
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
                        this.Plength = MapiInspector.MAPIParser.PartialGetRemainSize;
                        MapiInspector.MAPIParser.PartialGetRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialGetType = 0;
                    if (MapiInspector.MAPIParser.PartialGetRemainSize == -1 && MapiInspector.MAPIParser.PartialGetSubRemainSize == -1)
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
                    lengthValue = this.Plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                switch ((PropertyDataType)typeValue)
                {
                    case PropertyDataType.PtypMultipleInteger16:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 2, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleCurrency:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleFloatingTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleGuid:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, Guid.Empty.ToByteArray().Length, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleBinary:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleString:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleString8:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, true, false);
                        break;
                }
            }
        }
    }
}
