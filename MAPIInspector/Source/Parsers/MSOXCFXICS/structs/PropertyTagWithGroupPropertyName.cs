namespace MAPIInspector.Parsers
{
    /// <summary>
    /// This structure is a PropertyTag Structure (MS-OXCDATA section 2.9) which is special for named properties 
    /// 2.2.2.8.1.1 GroupPropertyName Structure
    /// </summary>
    public class PropertyTagWithGroupPropertyName : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value, as specified by the table in section 2.11.1.
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public ushort PropertyId;

        /// <summary>
        /// A GroupPropertyName structure.
        /// </summary>
        public GroupPropertyName GroupPropertyName;

        /// <summary>
        /// Parse the PropertyTagWithGroupPropertyName structure.
        /// </summary>
        /// <param name="stream">A stream containing the PropertyTagWithGroupPropertyName structure</param>
        public void Parse(FastTransferStream stream)
        {
            this.PropertyType = (PropertyDataType)stream.ReadUInt16();
            this.PropertyId = stream.ReadUInt16();
            if (this.PropertyId >= 0x8000)
            {
                this.GroupPropertyName = new GroupPropertyName();
                this.GroupPropertyName.Parse(stream);
            }
        }
    }
}
