using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.3 TypedPropertyValue Structure
    /// </summary>
    public class TypedPropertyValue : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        /// </summary>
        public BlockT<PropertyDataType> PropertyType;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2. The value MUST be compatible with the value of the propertyType field.
        /// </summary>
        public Block _PropertyValue;

        /// <summary>
        /// The parsing context that determines count field widths.
        /// </summary>
        private PropertyCountContext context = PropertyCountContext.RopBuffers;

        /// <summary>
        /// Initializes a new instance of the TypedPropertyValue class (parameterless constructor)
        /// </summary>
        public TypedPropertyValue()
        {
            context = PropertyCountContext.RopBuffers;
        }

        /// <summary>
        /// Initializes a new instance of the TypedPropertyValue class
        /// </summary>
        /// <param name="countContext">The parsing context that determines count field widths</param>
        public TypedPropertyValue(PropertyCountContext countContext = PropertyCountContext.RopBuffers)
        {
            context = countContext;
        }

        /// <summary>
        /// Parse the TypedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            _PropertyValue = PropertyValue.ReadPropertyValue(PropertyType, parser, context);
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(PropertyType, "PropertyType");
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
