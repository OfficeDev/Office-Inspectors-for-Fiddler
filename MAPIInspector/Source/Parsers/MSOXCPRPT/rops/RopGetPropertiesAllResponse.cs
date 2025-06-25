namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.3 RopGetPropertiesAll
    ///  A class indicates the RopGetPropertiesAll ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesAllResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public ushort? PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that are the properties defined on the object.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopGetPropertiesAllResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesAllResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                PropertyValueCount = ReadUshort();
                TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)PropertyValueCount];

                for (int i = 0; i < PropertyValueCount; i++)
                {
                    interValue[i] = new TaggedPropertyValue();
                    interValue[i].Parse(s);
                }

                PropertyValues = interValue;
            }
        }
    }
}
