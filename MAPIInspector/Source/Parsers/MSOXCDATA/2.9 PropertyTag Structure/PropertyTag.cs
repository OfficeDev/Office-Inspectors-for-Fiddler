using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.9 PropertyTag Structure
    /// </summary>
    public class PropertyTag : Block
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value, as specified by the table in section 2.11.1.
        /// </summary>
        public BlockT<PropertyDataType> PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public BlockT<PidTagPropertyEnum> PropertyId;

        public PropertyTag() { }

        /// <summary>
        /// Initializes a new instance of the PropertyTag class with parameters.
        /// </summary>
        /// <param name="ptype">The Type of the PropertyTag.</param>
        /// <param name="pId">The Id of the PropertyTag.</param>
        public PropertyTag(PropertyDataType ptype, PidTagPropertyEnum pId)
        {
            // TODO: Vet these params
            PropertyType = CreateBlock(ptype, 0, 0);
            PropertyId = CreateBlock(pId, 0, 0);
        }

        /// <summary>
        /// Parse the PropertyTag structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyTag structure</param>
        protected override void Parse()
        {
            PropertyType = ParseT<PropertyDataType>();
            PropertyId = ParseT<PidTagPropertyEnum>();
        }

        protected override void ParseBlocks()
        {
            SetText("PropertyTag");
            AddChildBlockT(PropertyType, "PropertyType");
            AddChildBlockT(PropertyId, "PropertyId");
        }
    }
}
