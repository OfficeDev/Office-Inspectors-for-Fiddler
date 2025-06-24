namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.6.5 RopModifyRecipients ROP
    /// A class indicates the RopModifyRecipients ROP request Buffer.
    /// </summary>
    public class RopModifyRecipientsRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. 
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public ushort ColumnCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that can be included for each recipient (1).
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of rows in the RecipientRows field.
        /// </summary>
        public ushort RowCount;

        /// <summary>
        /// A list of ModifyRecipientRow structures.
        /// </summary>
        public ModifyRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopModifyRecipientsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopModifyRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ColumnCount = this.ReadUshort();
            List<PropertyTag> propertyTags = new List<PropertyTag>();

            for (int i = 0; i < this.ColumnCount; i++)
            {
                PropertyTag propertyTag = Block.Parse<PropertyTag>(s);
                propertyTags.Add(propertyTag);
            }

            this.RecipientColumns = propertyTags.ToArray();
            this.RowCount = this.ReadUshort();
            List<ModifyRecipientRow> modifyRecipientRows = new List<ModifyRecipientRow>();

            for (int i = 0; i < this.RowCount; i++)
            {
                ModifyRecipientRow modifyRecipientRow = new ModifyRecipientRow(this.RecipientColumns);
                modifyRecipientRow.Parse(s);
                modifyRecipientRows.Add(modifyRecipientRow);
            }

            this.RecipientRows = modifyRecipientRows.ToArray();
        }
    }
}
