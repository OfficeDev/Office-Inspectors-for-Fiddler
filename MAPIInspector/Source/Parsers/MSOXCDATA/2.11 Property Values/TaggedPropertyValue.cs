using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.4 TaggedPropertyValue Structure
    /// </summary>
    public class TaggedPropertyValue : Block
    {
        /// <summary>
        /// A PropertyTag structure, as specified in section 2.9, giving the values of the PropertyId and propertyType fields for the property.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2.1. specifying the value of the property.
        /// </summary>
        public Block _PropertyValue;

        /// <summary>
        /// The Constructor to set the Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// A propertyTag structure, used for PropertyRestriction
        /// </summary>
        private PropertyTag tagInRestriction;

        /// <summary>
        /// Initializes a new instance of the TaggedPropertyValue class
        /// </summary>
        /// <param name="ptypMultiCountSize">The count size of multiple property</param>
        /// <param name="propertyTag">The PropertyTag structure</param>
        public TaggedPropertyValue(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes, PropertyTag propertyTag = null)
        {
            countWide = ptypMultiCountSize;
            tagInRestriction = propertyTag;
        }

        /// <summary>
        /// Parse the TaggedPropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyTag = Parse<PropertyTag>();
            if (tagInRestriction != null)
            {
                if (((ushort)tagInRestriction.PropertyType.Data & 0x1000) == 0x1000)
                {
                    tagInRestriction.PropertyType.Data = (PropertyDataType)((ushort)tagInRestriction.PropertyType.Data & 0xfff);
                }

                _PropertyValue = PropertyValue.ReadPropertyValue(tagInRestriction.PropertyType.Data, parser, countWide);
            }
            else
            {
                _PropertyValue = PropertyValue.ReadPropertyValue(PropertyTag.PropertyType.Data, parser, countWide);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("TaggedPropertyValue");
            AddChild(PropertyTag, "PropertyTag");
            if (_PropertyValue != null)
            {
                AddChild(_PropertyValue, $"PropertyValue:{_PropertyValue.Text}");
            }
            else
            {
                SetText("PropertyValue is null");
            }
        }
    }
}
