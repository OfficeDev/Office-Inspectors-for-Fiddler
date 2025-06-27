using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.17 RopExpandRow ROP
    ///  A class indicates the RopExpandRow ROP Response Buffer.
    /// </summary>
    public class RopExpandRowResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the total number of rows that are in the expanded category.
        /// </summary>
        public uint? ExpandedRowCount;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyRow structures.
        /// </summary>
        public ushort? RowCount;

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
        /// <param name="s">A stream containing RopExpandRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                ExpandedRowCount = ReadUint();
                RowCount = ReadUshort();
                List<PropertyRow> tempPropertyRows = new List<PropertyRow>();
                for (int i = 0; i < RowCount; i++)
                {
                    PropertyRow tempPropertyRow = new PropertyRow(propertiesBySetColum);
                    tempPropertyRow.Parse(s);
                    tempPropertyRows.Add(tempPropertyRow);
                }

                RowData = tempPropertyRows.ToArray();
            }
        }
    }
}
