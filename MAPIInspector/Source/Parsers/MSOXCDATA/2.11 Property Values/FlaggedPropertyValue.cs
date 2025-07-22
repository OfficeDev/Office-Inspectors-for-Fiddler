using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.5 FlaggedPropertyValue Structure
    /// </summary>
    public class FlaggedPropertyValue : Block
    {
        /// <summary>
        /// An unsigned integer. This value of this flag determines what is conveyed in the PropertyValue field.
        /// </summary>
        public BlockT<byte> Flag;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2.1, unless the Flag field is set to 0x1.
        /// </summary>
        public Block _PropertyValue;

        /// <summary>
        /// The Property data type.
        /// </summary>
        private PropertyDataType propertyType;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the FlaggedPropertyValue class
        /// </summary>
        /// <param name="propertyType">The Property data type.</param>
        /// <param name="ptypMultiCountSize">The Count wide size.</param>
        public FlaggedPropertyValue(PropertyDataType _propertyType, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            propertyType = _propertyType;
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the FlaggedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            Flag = ParseT<byte>();
            if (Flag == 0x00)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(propertyType, parser, countWide);
            }
            else if (Flag == 0x0A)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(PropertyDataType.PtypErrorCode, parser, countWide);
            }
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Flag, "Flag");
            if (_PropertyValue != null)
            {
                AddChild(_PropertyValue, $"PropertyValue:{_PropertyValue}");
            }
            else
            {
                AddHeader("PropertyValue is null");
            }
        }
    }
}
