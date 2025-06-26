using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.5.1.1 ModifyRecipientRow Structure
    /// A class indicates the ModifyRecipientRow structure.
    /// </summary>
    public class ModifyRecipientRow : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the ID of the recipient (1).
        /// </summary>
        public BlockT<uint> RowId;

        /// <summary>
        /// An enumeration that specifies the type of recipient (1).
        /// </summary>
        public RecipientType RecipientType;

        /// <summary>
        /// An unsigned integer that specifies the size of the RecipientRow field.
        /// </summary>
        public BlockT<ushort> RecipientRowSize;

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
        protected override void Parse()
        {
            RowId = ParseT<uint>();
            RecipientType = Parse<RecipientType>();
            RecipientRowSize = ParseT<ushort>();

            if (RecipientRowSize.Data > 0)
            {
                RecipientRow = new RecipientRow(propTags);
                RecipientRow.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("ModifyRecipientRow");
            AddChildBlockT(RowId, "RowId");
            AddChild(RecipientType, "RecipientType");
            AddChildBlockT(RecipientRowSize, "RecipientRowSize");
            AddChild(RecipientRow, "RecipientRow");
        }
    }
}
