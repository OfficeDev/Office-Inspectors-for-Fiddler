namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropertyGroup.
    /// 2.2.2.8.1 PropertyGroup
    /// </summary>
    public class PropertyGroup : BaseStructure
    {
        /// <summary>
        /// An unsigned 32-bit integer value that specifies how many PropertyTag structures are present in the PropertyTags field. 
        /// </summary>
        public uint PropertyTagCount;

        /// <summary>
        /// An array of PropertyTagWithGroupPropertyName structures.
        /// </summary>
        public PropertyTagWithGroupPropertyName[] PropertyTags;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PropertyGroup.</param>
        public void Parse(FastTransferStream stream)
        {
            this.PropertyTagCount = stream.ReadUInt32();
            this.PropertyTags = new PropertyTagWithGroupPropertyName[this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                PropertyTagWithGroupPropertyName tmpName = new PropertyTagWithGroupPropertyName();
                tmpName.Parse(stream);
                this.PropertyTags[i] = tmpName;
            }
        }
    }
}
