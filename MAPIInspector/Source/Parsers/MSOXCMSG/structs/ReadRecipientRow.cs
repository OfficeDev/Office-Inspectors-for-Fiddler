namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.6.2.1 ReadRecipientRow Structure
    /// A class indicates the ReadRecipientRow structure.
    /// </summary>
    public class ReadRecipientRow : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the row ID of the recipient (2).
        /// </summary>
        public uint RowId;

        /// <summary>
        /// An enumeration that specifies the type of recipient (2).
        /// </summary>
        public byte RecipientType;

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
        /// A RecipientRow structure. //TODO: put the raw bytes here temporarily and we need to refine it later once we get the key which is required by RecipientRow.
        /// </summary>
        public byte[] RecipientRow;

        /// <summary>
        /// Parse the ReadRecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing ReadRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RowId = ReadUint();
            RecipientType = ReadByte();
            CodePageId = ReadUshort();
            Reserved = ReadUshort();
            RecipientRowSize = ReadUshort();
            RecipientRow = ReadBytes(RecipientRowSize);
        }
    }
}
