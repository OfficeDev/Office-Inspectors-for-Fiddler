namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.6.16 RopOpenEmbeddedMessage ROP
    /// A class indicates the RopOpenEmbeddedMessage ROP response Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Reserved. This field MUST be set to 0x00.
        /// </summary>
        public byte? Reserved;

        /// <summary>
        /// An identifier that specifies the ID of the Embedded Message object.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// A Boolean that specifies whether the message has named properties.
        /// </summary>
        public bool? HasNamedProperties;

        /// <summary>
        /// A TypedString structure that specifies the subject prefix of the message.
        /// </summary>
        public TypedString SubjectPrefix;

        /// <summary>
        /// A TypedString structure that specifies the normalized subject of the message.
        /// </summary>
        public TypedString NormalizedSubject;

        /// <summary>
        /// An unsigned integer that specifies the number of recipients (2) on the message.
        /// </summary>
        public ushort? RecipientCount;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public ushort? ColumnCount;

        /// <summary>
        /// An unsigned integer that specifies the number of recipients (2) on the message.
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of rows in the RecipientRows field.
        /// </summary>
        public byte? RowCount;

        /// <summary>
        /// A list of OpenRecipientRow structures.
        /// </summary>
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenEmbeddedMessageResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            OutputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                Reserved = ReadByte();
                MessageId = new MessageID();
                MessageId.Parse(s);
                HasNamedProperties = ReadBoolean();
                SubjectPrefix = new TypedString();
                SubjectPrefix.Parse(s);
                NormalizedSubject = new TypedString();
                NormalizedSubject.Parse(s);
                RecipientCount = ReadUshort();
                ColumnCount = ReadUshort();
                List<PropertyTag> propertyTags = new List<PropertyTag>();

                for (int i = 0; i < ColumnCount; i++)
                {
                    PropertyTag propertyTag = Block.Parse<PropertyTag>(s);
                    propertyTags.Add(propertyTag);
                }

                RecipientColumns = propertyTags.ToArray();
                RowCount = ReadByte();
                List<OpenRecipientRow> openRecipientRows = new List<OpenRecipientRow>();

                for (int i = 0; i < RowCount; i++)
                {
                    OpenRecipientRow openRecipientRow = new OpenRecipientRow(RecipientColumns);
                    openRecipientRow.Parse(s);
                    openRecipientRows.Add(openRecipientRow);
                }

                RecipientRows = openRecipientRows.ToArray();
            }
        }
    }
}
