namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    ///  2.2.2.4 RopGetPropertiesList
    ///  A class indicates the RopGetPropertiesList ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesListResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of property tags in the PropertyTags field.
        /// </summary>
        public ushort? PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that lists the property tags on the object.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesListResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesListResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                PropertyTagCount = ReadUshort();
                PropertyTag[] interTag = new PropertyTag[(int)PropertyTagCount];

                for (int i = 0; i < PropertyTagCount; i++)
                {
                    interTag[i] = Block.Parse<PropertyTag>(s);
                }

                PropertyTags = interTag;
            }
        }
    }
}
