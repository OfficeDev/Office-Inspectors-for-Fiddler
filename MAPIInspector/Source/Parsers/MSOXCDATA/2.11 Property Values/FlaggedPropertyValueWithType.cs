namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.6 FlaggedPropertyValueWithType Structure
    /// </summary>
    public class FlaggedPropertyValueWithType : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        /// </summary>
        public BlockT<PropertyDataType> PropertyType;

        /// <summary>
        /// An unsigned integer. This flag MUST be set one of three possible values: 0x0, 0x1, or 0xA, which determines what is conveyed in the PropertyValue field.
        /// </summary>
        public BlockT<byte> Flag;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2.1, unless the Flag field is set to 0x1.
        /// </summary>
        public Block _PropertyValue;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        ///  Initializes a new instance of the FlaggedPropertyValueWithType class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size.</param>
        public FlaggedPropertyValueWithType(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the FlaggedPropertyValueWithType structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            Flag = ParseT<byte>();
            if (Flag.Data == 0x00)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(PropertyType.Data, parser, countWide);
            }
            else if (Flag.Data == 0x0A)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(PropertyDataType.PtypErrorCode, parser, countWide);
            }
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Flag, "Flag");
            if (_PropertyValue != null)
            {
                AddLabeledChild(_PropertyValue, "PropertyValue");
            }
            else
            {
                SetText("PropertyValue is null");
            }
        }
    }
}
