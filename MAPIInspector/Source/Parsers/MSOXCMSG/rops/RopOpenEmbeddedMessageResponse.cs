using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.16 RopOpenEmbeddedMessage ROP
    /// A class indicates the RopOpenEmbeddedMessage ROP response Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// Reserved. This field MUST be set to 0x00.
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// An identifier that specifies the ID of the Embedded Message object.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// A Boolean that specifies whether the message has named properties.
        /// </summary>
        public BlockT<bool> HasNamedProperties;

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
        public BlockT<ushort> RecipientCount;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public BlockT<ushort> ColumnCount;

        /// <summary>
        /// An unsigned integer that specifies the number of recipients (2) on the message.
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of rows in the RecipientRows field.
        /// </summary>
        public BlockT<byte> RowCount;

        /// <summary>
        /// A list of OpenRecipientRow structures.
        /// </summary>
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                Reserved = ParseT<byte>();
                MessageId = Parse<MessageID>();
                HasNamedProperties = ParseAs<byte, bool>();
                SubjectPrefix = Parse<TypedString>();
                NormalizedSubject = Parse<TypedString>();
                RecipientCount = ParseT<ushort>();
                ColumnCount = ParseT<ushort>();
                var propertyTags = new List<PropertyTag>();

                for (int i = 0; i < ColumnCount; i++)
                {
                    propertyTags.Add(Parse<PropertyTag>());
                }

                RecipientColumns = propertyTags.ToArray();
                RowCount = ParseT<byte>();
                var openRecipientRows = new List<OpenRecipientRow>();

                for (int i = 0; i < RowCount; i++)
                {
                    var openRecipientRow = new OpenRecipientRow(RecipientColumns);
                    openRecipientRow.Parse(parser);
                    openRecipientRows.Add(openRecipientRow);
                }

                RecipientRows = openRecipientRows.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopOpenEmbeddedMessageResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(Reserved, "Reserved");
            AddChild(MessageId, "MessageId");
            AddChildBlockT(HasNamedProperties, "HasNamedProperties");
            AddChild(SubjectPrefix, "SubjectPrefix");
            AddChild(NormalizedSubject, "NormalizedSubject");
            AddChildBlockT(RecipientCount, "RecipientCount");
            AddChildBlockT(ColumnCount, "ColumnCount");
            AddLabeledChildren(RecipientColumns, "RecipientColumns");
            AddChildBlockT(RowCount, "RowCount");
            AddLabeledChildren(RecipientRows, "RecipientRows");
        }
    }
}
