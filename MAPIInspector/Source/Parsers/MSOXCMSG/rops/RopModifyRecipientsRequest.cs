namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// 2.2.6.5 RopModifyRecipients ROP
    /// A class indicates the RopModifyRecipients ROP request Buffer.
    /// </summary>
    public class RopModifyRecipientsRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public BlockT<ushort> ColumnCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that can be included for each recipient (1).
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of rows in the RecipientRows field.
        /// </summary>
        public BlockT<ushort> RowCount;

        /// <summary>
        /// A list of ModifyRecipientRow structures.
        /// </summary>
        public ModifyRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopModifyRecipientsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ColumnCount = ParseT<ushort>();
            var propertyTags = new List<PropertyTag>();

            for (int i = 0; i < ColumnCount.Data; i++)
            {
                propertyTags.Add(Parse<PropertyTag>());
            }

            RecipientColumns = propertyTags.ToArray();
            RowCount = ParseT<ushort>();
            var modifyRecipientRows = new List<ModifyRecipientRow>();

            for (int i = 0; i < RowCount.Data; i++)
            {
                ModifyRecipientRow modifyRecipientRow = new ModifyRecipientRow(RecipientColumns);
                modifyRecipientRow.Parse(parser);
                modifyRecipientRows.Add(modifyRecipientRow);
            }

            RecipientRows = modifyRecipientRows.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopModifyRecipientsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ColumnCount, "ColumnCount");
            AddLabeledChildren(RecipientColumns, "AddLabeledChildren");
            AddChildBlockT(RowCount, "RowCount");
        }
    }
}
