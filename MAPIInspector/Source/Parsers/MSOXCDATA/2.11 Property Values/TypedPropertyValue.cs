namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.3 TypedPropertyValue Structure
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
        /// Source property tag information
        /// </summary>
        public AnnotatedComment PropertyTag;

        /// <summary>
        /// The Count wide size of ptypMutiple type.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the TypedPropertyValue class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type</param>
        public TypedPropertyValue(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the TypedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            _PropertyValue = PropertyValue.ReadPropertyValue(PropertyType.Data, parser, countWide);
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
                SetText("PropertyValue is null");
            }
        }
    }
}
