using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.14 RopFindRow ROP
    /// A class indicates the RopFindRow ROP Response Buffer.
    /// </summary>
    public class RopFindRowResponse : BaseStructure
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
        /// A Boolean that specifies whether the bookmark target is no longer visible.
        /// </summary>
        public bool? RowNoLongerVisible;

        /// <summary>
        /// A Boolean that indicates whether the RowData field is present.
        /// </summary>
        public bool? HasRowData;

        /// <summary>
        /// A Boolean that indicates whether the RowData field is present.
        /// </summary>
        public PropertyRow RowData;

        /// <summary>
        /// Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        /// </summary>
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// Initializes a new instance of the RopFindRowResponse class.
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopFindRowResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopFindRowResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFindRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                RowNoLongerVisible = ReadBoolean();
                HasRowData = ReadBoolean();
                if ((bool)HasRowData)
                {
                    PropertyRow tempPropertyRow = new PropertyRow(propertiesBySetColum);
                    RowData = tempPropertyRow;
                    RowData.Parse(s);
                }
            }
        }
    }
}
