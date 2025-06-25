namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.2 RopGetPropertiesSpecific
    ///  A class indicates the RopGetPropertiesSpecific ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesSpecificResponse : BaseStructure
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
        /// A PropertyRow structure. 
        /// </summary>
        public PropertyRow RowData;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesSpecificResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                PropertyTag[] proTags = new PropertyTag[0];
                if (!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
                {
                    proTags = DecodingContext.GetPropertiesSpec_propertyTags[MapiInspector.MAPIParser.ParsingSession.id][InputHandleIndex].Dequeue();
                }
                else
                {
                    proTags = DecodingContext.GetPropertiesSpec_propertyTags[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][InputHandleIndex].Dequeue();
                }
                RowData = new PropertyRow(proTags);
                RowData.Parse(s);
            }
        }
    }
}
