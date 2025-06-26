using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3.1 RopOpenMessage
    ///  A class indicates the RopOpenMessage ROP response Buffer.
    /// </summary>
    public class RopOpenMessageResponse : Block
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
        /// An unsigned integer that specifies the number of recipients (1) on the message.
        /// </summary>
        public BlockT<ushort> RecipientCount;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public BlockT<ushort> ColumnCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that can be included in each row that is specified in the RecipientRows field.
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientRows field.
        /// </summary>
        public BlockT<byte> RowCount;

        /// <summary>
        /// A list of OpenRecipientRow structures.
        /// </summary>
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopOpenMessageResponse structure.
        /// </summary>
        protected override void Parse()
        {

            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue.Data == ErrorCodes.Success)
            {
                HasNamedProperties = ParseAs<byte, bool>();
                SubjectPrefix = Parse<TypedString>();
                NormalizedSubject = Parse<TypedString>();
                RecipientCount = ParseT<ushort>();
                ColumnCount = ParseT<ushort>();
                var propertyTags = new List<PropertyTag>();

                for (int i = 0; i < ColumnCount.Data; i++)
                {
                    propertyTags.Add(Parse<PropertyTag>());
                }

                RecipientColumns = propertyTags.ToArray();
                RowCount = ParseT<byte>();
                var openRecipientRows = new List<OpenRecipientRow>();

                for (int i = 0; i < RowCount.Data; i++)
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
            SetText("RopOpenMessageResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
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
