using System.Collections.Generic;
using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.3.4.2 RopGetReceiveFolderTable ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.3.4.3 RopGetReceiveFolderTable ROP Failure Response Buffer
    /// A class indicates the RopGetReceiveFolderTable ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderTableResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of row structures contained in the Rows field.
        /// </summary>
        public BlockT<uint> RowCount;

        /// <summary>
        /// An array of row structures. This field contains the rows of the Receive folder table. Each row is returned in either a StandardPropertyRow or a FlaggedPropertyRow structure.
        /// </summary>
        public PropertyRow[] Rows;

        /// <summary>
        /// Parse the RopGetReceiveFolderTableResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                // PidTagMessageClass is defined as PtypString8 due to Open Specification said all characters in this property MUST be from the ASCII characters 0x20 through 0x7F.
                var properties_GetReceiveFolderTable = new PropertyTag[3]
                {
                      new PropertyTag(PropertyDataType.PtypInteger64, PidTagPropertyEnum.PidTagFolderId),
                      new PropertyTag(PropertyDataType.PtypString8, PidTagPropertyEnum.PidTagMessageClass),
                      new PropertyTag(PropertyDataType.PtypTime, PidTagPropertyEnum.PidTagLastModificationTime)
                };

                RowCount = ParseT<uint>();
                var tmpRows = new List<PropertyRow>();
                for (int i = 0; i < RowCount; i++)
                {
                    var proRow = new PropertyRow(properties_GetReceiveFolderTable);
                    proRow.Parse(parser);
                    tmpRows.Add(proRow);
                }

                Rows = tmpRows.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetReceiveFolderTableResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChild(RowCount, "RowCount");
            AddLabeledChildren(Rows, "Rows");
        }
    }
}
