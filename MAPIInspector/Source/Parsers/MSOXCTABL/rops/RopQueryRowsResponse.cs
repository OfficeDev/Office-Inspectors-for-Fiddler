using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.5 RopQueryRows ROP
    /// A class indicates the RopQueryRows ROP Response Buffer.
    /// </summary>
    public class RopQueryRowsResponse : Block
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
        /// An enumeration that specifies current location of the cursor.
        /// </summary>
        public BlockT<Bookmarks> Origin;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RowData field.
        /// </summary>
        public BlockT<ushort> RowCount;

        /// <summary>
        /// A list of PropertyRow structures.
        /// </summary>
        public PropertyRow[] RowData;

        /// <summary>
        /// Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1).
        /// </summary>
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// Initializes a new instance of the RopQueryRowsResponse class.
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopQueryRowsResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopQueryRows structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                Origin = ParseT<Bookmarks>();
                RowCount = ParseT<ushort>();
                if (RowCount != 0)
                {
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
        }

        protected override void ParseBlocks()
        {
            SetText("RopQueryRowsResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(Origin, "Origin");
            AddChildBlockT(RowCount, "RowCount");
            AddLabeledChildren(RowData, "RowData");
        }
    }
}
