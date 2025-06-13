namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationImportHierarchyChange ROP Request Buffer.
    ///  2.2.3.2.4.3.1 RopSynchronizationImportHierarchyChange ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportHierarchyChangeRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the HierarchyValues field.
        /// </summary>
        public ushort HierarchyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify hierarchy-related properties of the folder.
        /// </summary>
        public TaggedPropertyValue[] HierarchyValues;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportHierarchyChangeRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportHierarchyChangeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.HierarchyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interHierarchyValues = new TaggedPropertyValue[(int)this.HierarchyValueCount];

            for (int i = 0; i < this.HierarchyValueCount; i++)
            {
                interHierarchyValues[i] = new TaggedPropertyValue();
                interHierarchyValues[i].Parse(s);
            }

            this.HierarchyValues = interHierarchyValues;
            this.PropertyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)this.PropertyValueCount];

            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                interValue[i] = new TaggedPropertyValue();
                interValue[i].Parse(s);
            }

            this.PropertyValues = interValue;
        }
    }
}
