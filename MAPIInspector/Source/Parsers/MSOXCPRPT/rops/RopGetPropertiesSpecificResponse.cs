using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.8.3.2 RopGetPropertiesSpecific ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.8.3.3 RopGetPropertiesSpecific ROP Failure Response Buffer
    /// A class indicates the RopGetPropertiesSpecific ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesSpecificResponse : Block
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
        /// A PropertyRow structure.
        /// </summary>
        public PropertyRow RowData;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                PropertyTag[] propTags = new PropertyTag[0];
                if (!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
                {
                    propTags = DecodingContext.GetPropertiesSpec_propertyTags[MapiInspector.MAPIParser.ParsingSession.id][InputHandleIndex].Dequeue();
                }
                else
                {
                    propTags = DecodingContext.GetPropertiesSpec_propertyTags[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][InputHandleIndex].Dequeue();
                }

                RowData = new PropertyRow(propTags, PropertyCountContext.RopBuffers);
                RowData.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPropertiesSpecificResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChild(RowData, "RowData");
        }
    }
}
