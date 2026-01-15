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
        /// The parsing context that determines count field widths.
        /// </summary>
        private readonly PropertyCountContext context;

        /// <summary>
        /// An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        /// </summary>
        private PropertyDataType propertyType;

        /// <summary>
        /// Initializes a new instance of the FlaggedPropertyValue class
        /// </summary>
        /// <param name="_propertyType">The property type</param>
        /// <param name="countContext">The parsing context that determines count field widths</param>
        public FlaggedPropertyValue(PropertyDataType _propertyType, PropertyCountContext countContext = PropertyCountContext.RopBuffers)
        {
            context = countContext;
            propertyType = _propertyType;
        }

        /// <summary>
        /// Parse the FlaggedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            Flag = ParseT<byte>();
            if (Flag == 0x00)
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(propertyType, parser, context);
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
                AddChild(_PropertyValue, $"PropertyValue:{_PropertyValue}");
            }
            else
            {
                AddHeader("PropertyValue is null");
            }
        }
    }
}
