namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.5 RopModifyRecipients ROP
    /// A class indicates the ModifyRecipientRow structure.
    /// </summary>
    public class ModifyRecipientRow : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the ID of the recipient (1).
        /// </summary>
        public uint RowId;

        /// <summary>
        /// An enumeration that specifies the type of recipient (1).
        /// </summary>
        public byte RecipientType;

        /// <summary>
        /// An unsigned integer that specifies the size of the RecipientRow field.
        /// </summary>
        public ushort RecipientRowSize;

        /// <summary>
        /// A RecipientRow structure.
        /// </summary>
        public RecipientRow RecipientRow;

        /// <summary>
        /// A parameter for construct function
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Initializes a new instance of the ModifyRecipientRow class.
        /// </summary>
        /// <param name="propTags">The initialized parameter</param>
        public ModifyRecipientRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the ModifyRecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing ModifyRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RowId = this.ReadUint();
            this.RecipientType = this.ReadByte();
            this.RecipientRowSize = this.ReadUshort();

            if (this.RecipientRowSize > 0)
            {
                this.RecipientRow = new RecipientRow(this.propTags);
                this.RecipientRow.Parse(s);
            }
        }
    }
}
