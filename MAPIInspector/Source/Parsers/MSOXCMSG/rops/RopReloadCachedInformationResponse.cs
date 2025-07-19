using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.7 RopReloadCachedInformation ROP
    /// A class indicates the RopReloadCachedInformation ROP response Buffer.
    /// </summary>
    public class RopReloadCachedInformationResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex specified field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

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
        /// An unsigned integer that specifies the number of recipients (2) on the message.
        /// </summary>
        public BlockT<ushort> RecipientCount;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public BlockT<ushort> ColumnCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that can be included for each recipient (2).
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
        /// Parse the RopReloadCachedInformationResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
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
                    OpenRecipientRow openRecipientRow = new OpenRecipientRow(RecipientColumns);
                    openRecipientRow.Parse(parser);
                    openRecipientRows.Add(openRecipientRow);
                }

                RecipientRows = openRecipientRows.ToArray();
            }
        }
        protected override void ParseBlocks()
        {
            SetText("RopReloadCachedInformationResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
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
