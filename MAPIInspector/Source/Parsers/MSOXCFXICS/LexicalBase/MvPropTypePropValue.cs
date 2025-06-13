namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValue : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public int Length;

        /// <summary>
        /// A list of fixed size values.
        /// </summary>
        public byte[][] FixedSizeValueList;

        /// <summary>
        /// A list of LengthOfBlock.
        /// </summary>
        public LengthOfBlock[] VarSizeValueList;

        /// <summary>
        /// Initializes a new instance of the MvPropTypePropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MvPropTypePropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>I the stream's current position contains a serialized MvPropTypePropValue, return true, else false</returns>
        public static new bool Verify(FastTransferStream stream)
        {
            ushort tmp = stream.VerifyUInt16();
            return LexicalTypeHelper.IsMVType((PropertyDataType)tmp) && !PropValue.IsMetaTagIdsetGiven(stream);
        }

        /// <summary>
        /// Parse a MvPropTypePropValue instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>A MvPropTypePropValue instance </returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new MvPropTypePropValue(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            PropertyDataType type = (PropertyDataType)this.PropType;
            this.Length = stream.ReadInt32();

            switch (type)
            {
                case PropertyDataType.PtypMultipleInteger16:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 2);
                    break;
                case PropertyDataType.PtypMultipleInteger32:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 4);
                    break;
                case PropertyDataType.PtypMultipleFloating32:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 4);
                    break;
                case PropertyDataType.PtypMultipleFloating64:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleCurrency:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleFloatingTime:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleInteger64:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleTime:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleGuid:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, Guid.Empty.ToByteArray().Length);
                    break;
                case PropertyDataType.PtypMultipleBinary:
                    this.VarSizeValueList = stream.ReadLengthBlocks(this.Length);
                    break;
                case PropertyDataType.PtypMultipleString:
                    this.VarSizeValueList = stream.ReadLengthBlocks(this.Length);
                    break;
                case PropertyDataType.PtypMultipleString8:
                    this.VarSizeValueList = stream.ReadLengthBlocks(this.Length);
                    break;
            }
        }
    }
}
