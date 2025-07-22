using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// This structure is a PropertyTag Structure (MS-OXCDATA section 2.9) which is special for named properties
    /// [MS-OXCFXICS] 2.2.2.8.1.1 GroupPropertyName Structure
    /// </summary>
    public class PropertyTagWithGroupPropertyName : Block
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value, as specified by the table in section 2.11.1.
        /// </summary>
        public BlockT<PropertyDataType> PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public BlockT<ushort> PropertyId;

        /// <summary>
        /// A GroupPropertyName structure.
        /// </summary>
        public GroupPropertyName GroupPropertyName;

        /// <summary>
        /// Parse the PropertyTagWithGroupPropertyName structure.
        /// </summary>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            PropertyId = ParseT<ushort>();
            if (PropertyId >= 0x8000)
            {
                GroupPropertyName = Parse<GroupPropertyName>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "PropertyTagWithGroupPropertyName";
            AddChildBlockT(PropertyType, "PropertyType");
            AddChildBlockT(PropertyId, "PropertyId");
            if (PropertyId >= 0x8000)
            {
                AddChild(GroupPropertyName);
            }
        }
    }
}
