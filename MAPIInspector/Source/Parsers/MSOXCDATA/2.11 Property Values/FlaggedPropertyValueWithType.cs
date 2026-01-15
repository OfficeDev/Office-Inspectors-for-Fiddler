using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.6 FlaggedPropertyValueWithType Structure
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
        /// The parsing context that determines count field widths.
        /// </summary>
        private PropertyCountContext context;

        /// <summary>
        /// Initializes a new instance of the FlaggedPropertyValueWithType class
        /// </summary>
        /// <param name="countContext">The parsing context that determines count field widths</param>
        public FlaggedPropertyValueWithType(PropertyCountContext countContext = PropertyCountContext.RopBuffers)
        {
            context = countContext;
        }

        /// <summary>
        /// Parse the FlaggedPropertyValueWithType structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            Flag = ParseT<byte>();
            if (Flag == 0x00)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(PropertyType, parser, context);
            }
            else if (Flag == 0x0A)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(PropertyDataType.PtypErrorCode, parser, context);
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
                AddHeader("PropertyValue is null");
            }
        }
    }
}
