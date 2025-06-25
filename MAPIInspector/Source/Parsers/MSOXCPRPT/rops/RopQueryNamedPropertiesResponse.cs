namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.9 RopQueryNamedProperties
    ///  A class indicates the RopQueryNamedProperties ROP Response Buffer.
    /// </summary>
    public class RopQueryNamedPropertiesResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of elements contained in the PropertyIds and PropertyNames fields.
        /// </summary>
        public ushort? IdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name.
        /// </summary>
        public ushort?[] PropertyIds;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names for the property IDs specified in the PropertyIds field. 
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryNamedPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                IdCount = ReadUshort();
                PropertyIds = ConvertArray(new ushort[(int)IdCount]);
                PropertyNames = new PropertyName[(int)IdCount];

                for (int i = 0; i < IdCount; i++)
                {
                    PropertyIds[i] = ReadUshort();
                }

                for (int i = 0; i < IdCount; i++)
                {
                    PropertyNames[i] = new PropertyName();
                    PropertyNames[i].Parse(s);
                }
            }
        }
    }
}
