namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValuePutPartial : PropValue
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
        /// Length for partial
        /// </summary>
        private int Plength;

        /// <summary>
        /// Initializes a new instance of the MvPropTypePropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValuePutPartial(FastTransferStream stream)
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
                        this.Plength = MapiInspector.MAPIParser.PartialPutRemainSize;
                        MapiInspector.MAPIParser.PartialPutRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialPutType = 0;
                    MapiInspector.MAPIParser.PartialPutId = 0;

                    if (MapiInspector.MAPIParser.PartialPutRemainSize == -1 && MapiInspector.MAPIParser.PartialPutSubRemainSize == -1)
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

                switch ((PropertyDataType)this.PropType)
                {
                    case PropertyDataType.PtypMultipleInteger16:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 2, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleInteger32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleFloating32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleFloating64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleCurrency:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleFloatingTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleInteger64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleGuid:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, Guid.Empty.ToByteArray().Length, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleBinary:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleString:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleString8:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, true);
                        break;
                }
            }
        }
    }
}
