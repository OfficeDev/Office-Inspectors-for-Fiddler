using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXNSPI] 2.2.2 Property Values
    /// [MS-OXNSPI] 2.2.2.12 PropertyValue_r Structure
    /// [MS-OXCDATA] 2.11.2.2 PropertyValue_r Structure
    /// A class indicates the PropertyValue_r structure.
    /// </summary>
    public class PropertyValue_r : Block
    {
        /// <summary>
        /// Encodes the PropTag of the property whose value is represented by the PropertyValue_r data structure.
        /// </summary>
        public PropertyTag UlPropTag;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00000000.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// Encodes the actual value of the property represented by the PropertyValue_r data structure.
        /// </summary>
        public PROP_VAL_UNION Value;

        /// <summary>
        /// Parse the PropertyValue_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            UlPropTag = Parse<PropertyTag>();
            Value = new PROP_VAL_UNION(UlPropTag.PropertyType);
            Value.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "PropertyValue_r";
            AddChild(UlPropTag, "UlPropTag");
            AddChild(Value, "Value");
        }
    }
}
