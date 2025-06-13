namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationImportDeletes ROP Request Buffer.
    ///  2.2.3.2.4.5.1 RopSynchronizationImportDeletes ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportDeletesRequest : BaseStructure
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
        /// A flags structure that contains flags that specify options for the imported deletions.
        /// </summary>
        public ImportDeleteFlags ImportDeleteFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportDeletesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportDeletesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ImportDeleteFlags = (ImportDeleteFlags)this.ReadByte();
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
