using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.14 RopFindRow ROP
    /// A class indicates the RopFindRow ROP Response Buffer.
    /// </summary>
    public class RopFindRowResponse : Block
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
        /// A Boolean that specifies whether the bookmark target is no longer visible.
        /// </summary>
        public BlockT<bool> RowNoLongerVisible;

        /// <summary>
        /// A Boolean that indicates whether the RowData field is present.
        /// </summary>
        public BlockT<bool> HasRowData;

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
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                RowNoLongerVisible = ParseAs<byte, bool>();
                HasRowData = ParseAs<byte, bool>();
                if (HasRowData)
                {
                    RowData = new PropertyRow(propertiesBySetColum);
                    RowData.Parse(parser);
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopFindRowResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ReturnValue, "ReturnValue");
            AddChildBlockT(RowNoLongerVisible, "RowNoLongerVisible");
            AddChildBlockT(HasRowData, "HasRowData");
            AddChild(RowData);
        }
    }
}
