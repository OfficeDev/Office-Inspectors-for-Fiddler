namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.6.7 RopReloadCachedInformation ROP
    /// A class indicates the RopReloadCachedInformation ROP response Buffer.
    /// </summary>
    public class RopReloadCachedInformationResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex specified field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

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
        /// An array of PropertyTag structures that specifies the property values that can be included for each recipient (2).
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
        /// Parse the RopReloadCachedInformationResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReloadCachedInformationResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
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
