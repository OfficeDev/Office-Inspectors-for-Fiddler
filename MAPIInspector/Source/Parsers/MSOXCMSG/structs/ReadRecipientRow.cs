using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.6.2.1 ReadRecipientRow Structure
    /// A class indicates the ReadRecipientRow structure.
    /// </summary>
    public class ReadRecipientRow : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the row ID of the recipient (2).
        /// </summary>
        public BlockT<uint> RowId;

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
        /// A RecipientRow structure. //TODO: put the raw bytes here temporarily and we need to refine it later once we get the key which is required by RecipientRow.
        /// </summary>
        public BlockBytes RecipientRow;

        /// <summary>
        /// Parse the ReadRecipientRow structure.
        /// </summary>
        protected override void Parse()
        {
            RowId = ParseT<uint>();
            RecipientType = Parse<RecipientType>();
            CodePageId = ParseT<ushort>();
            Reserved = ParseT<ushort>();
            RecipientRowSize = ParseT<ushort>();
            RecipientRow = ParseBytes(RecipientRowSize);
        }

        protected override void ParseBlocks()
        {
            Text = "ReadRecipientRow";
            AddChildBlockT(RowId, "RowId");
            AddChild(RecipientType, "RecipientType");
            AddChildBlockT(CodePageId, "CodePageId");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(RecipientRowSize, "RecipientRowSize");
            AddChildBytes(RecipientRow, "RecipientRow");
        }
    }
}
