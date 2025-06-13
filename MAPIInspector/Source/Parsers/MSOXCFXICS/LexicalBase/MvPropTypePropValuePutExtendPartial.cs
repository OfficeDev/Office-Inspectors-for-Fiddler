namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValuePutExtendPartial : PropValue
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
        /// Initializes a new instance of the MvPropTypePropValuePutExtendPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValuePutExtendPartial(FastTransferStream stream)
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
                MapiInspector.MAPIParser.PartialPutExtendType = (ushort)this.PropType;
                MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIParser.PartialPutExtendType != 0 && MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                    && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIParser.PartialPutExtendType;

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize != -1)
                    {
                        this.Plength = MapiInspector.MAPIParser.PartialPutExtendRemainSize;
                        MapiInspector.MAPIParser.PartialPutExtendRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIParser.PartialPutExtendType = 0;
                    MapiInspector.MAPIParser.PartialPutExtendId = 0;

                    if (MapiInspector.MAPIParser.PartialPutExtendRemainSize == -1 && MapiInspector.MAPIParser.PartialPutExtendSubRemainSize == -1)
                    {
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
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
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 2, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleCurrency:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleFloatingTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleGuid:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, Guid.Empty.ToByteArray().Length, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleBinary:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleString:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleString8:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, false);
                        break;
                }
            }
        }
    }
}
