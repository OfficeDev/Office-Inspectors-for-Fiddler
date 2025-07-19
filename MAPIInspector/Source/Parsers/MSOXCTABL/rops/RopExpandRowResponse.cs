using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.17 RopExpandRow ROP
    /// A class indicates the RopExpandRow ROP Response Buffer.
    /// </summary>
    public class RopExpandRowResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the total number of rows that are in the expanded category.
        /// </summary>
        BlockT<uint> ExpandedRowCount;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyRow structures.
        /// </summary>
        public BlockT<ushort> RowCount;

        /// <summary>
        /// A list of PropertyRow structures. The number of structures contained in this field is specified by the RowCount field.
        /// </summary>
        public PropertyRow[] RowData;

        /// <summary>
        /// Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1).
        /// </summary>
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// Initializes a new instance of the RopExpandRowResponse class.
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopExpandRowResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopExpandRowResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                ExpandedRowCount = ParseT<uint>();
                RowCount = ParseT<ushort>();
                var tempPropertyRows = new List<PropertyRow>();
                for (int i = 0; i < RowCount; i++)
                {
                    var tempPropertyRow = new PropertyRow(propertiesBySetColum);
                    tempPropertyRow.Parse(parser);
                    tempPropertyRows.Add(tempPropertyRow);
                }

                RowData = tempPropertyRows.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopExpandRowResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(ExpandedRowCount, "ExpandedRowCount");
            AddChildBlockT(RowCount, "RowCount");
            AddLabeledChildren(RowData, "RowData");
        }
    }
}
