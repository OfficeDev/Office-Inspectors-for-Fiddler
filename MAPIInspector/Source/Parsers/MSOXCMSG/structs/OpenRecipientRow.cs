namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.1.2.1 OpenRecipientRow Structure
    /// A class indicates the OpenRecipientRow structure.
    /// </summary>
    public class OpenRecipientRow : BaseStructure
    {
        /// <summary>
        /// An enumeration that specifies the type of recipient (2). 
        /// </summary>
        public RecipientType RecipientType;

        /// <summary>
        /// An identifier that specifies the code page for the recipient (2).
        /// </summary>
        public ushort CodePageId;

        /// <summary>
        /// Reserved. The server MUST set this field to 0x0000.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the RecipientRow field.
        /// </summary>
        public ushort RecipientRowSize;

        /// <summary>
        /// A RecipientRow structure. 
        /// </summary>
        public RecipientRow RecipientRow;
        
        /// <summary>
        /// Array of PropertyTag used to initialize the class.
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Initializes a new instance of the OpenRecipientRow class.
        /// </summary>
        /// <param name="propTags">Array of PropertyTag used to initialize the class.</param>
        public OpenRecipientRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the OpenRecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing OpenRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RecipientType = new RecipientType();
            RecipientType.Parse(s);
            CodePageId = ReadUshort();
            Reserved = ReadUshort();
            RecipientRowSize = ReadUshort();
            RecipientRow = new RecipientRow(propTags);
            RecipientRow.Parse(s);
        }
    }
}
