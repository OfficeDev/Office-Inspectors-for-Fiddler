using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.6.1.2.1 OpenRecipientRow Structure
    /// A class indicates the OpenRecipientRow structure.
    /// </summary>
    public class OpenRecipientRow : Block
    {
        /// <summary>
        /// An enumeration that specifies the type of recipient (2).
        /// </summary>
        public RecipientType RecipientType;

        /// <summary>
        /// An identifier that specifies the code page for the recipient (2).
        /// </summary>
        public BlockT<ushort> CodePageId;

        /// <summary>
        /// Reserved. The server MUST set this field to 0x0000.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the RecipientRow field.
        /// </summary>
        public BlockT<ushort> RecipientRowSize;

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
        protected override void Parse()
        {
            RecipientType = Parse<RecipientType>();
            CodePageId = ParseT<ushort>();
            Reserved = ParseT<ushort>();
            RecipientRowSize = ParseT<ushort>();
            RecipientRow = new RecipientRow(propTags);
            RecipientRow.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            Text = "OpenRecipientRow";
            AddChild(RecipientType, "RecipientType");
            AddChildBlockT(CodePageId, "CodePageId");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(RecipientRowSize, "RecipientRowSize");
            AddChild(RecipientRow, $"RecipientRow");
        }
    }
}
