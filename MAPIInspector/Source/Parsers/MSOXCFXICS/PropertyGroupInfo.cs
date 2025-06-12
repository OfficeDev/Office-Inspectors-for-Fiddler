namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropertyGroupInfo class
    /// 2.2.2.8 PropertyGroupInfo
    /// </summary>
    public class PropertyGroupInfo : BaseStructure
    {
        /// <summary>
        /// An unsigned 32-bit integer value that identifies a property mapping within the current synchronization download context.
        /// </summary>
        public uint GroupId;

        /// <summary>
        /// A reserved field
        /// </summary>
        public uint Reserved;

        /// <summary>
        ///  An unsigned 32-bit integer value that specifies how many PropertyGroup structures are present in the Groups field. 
        /// </summary>
        public uint GroupCount;

        /// <summary>
        /// An array of PropertyGroup structures,
        /// </summary>
        public PropertyGroup[] Groups;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PropertyGroupInfo.</param>
        public void Parse(FastTransferStream stream)
        {
            this.GroupId = stream.ReadUInt32();
            this.Reserved = stream.ReadUInt32();
            this.GroupCount = stream.ReadUInt32();
            this.Groups = new PropertyGroup[this.GroupCount];
            for (int i = 0; i < this.GroupCount; i++)
            {
                PropertyGroup tmpPropertyGroup = new PropertyGroup();
                tmpPropertyGroup.Parse(stream);
                this.Groups[i] = tmpPropertyGroup;
            }
        }
    }
}
