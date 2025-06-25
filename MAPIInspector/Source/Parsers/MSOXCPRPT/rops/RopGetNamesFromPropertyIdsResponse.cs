namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.13 RopGetNamesFromPropertyIds
    ///  A class indicates the RopGetNamesFromPropertyIds ROP Response Buffer.
    /// </summary>
    public class RopGetNamesFromPropertyIdsResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of structures in the PropertyNames field.
        /// </summary>
        public ushort? PropertyNameCount;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names requested.
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetNamesFromPropertyIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                PropertyNameCount = ReadUshort();
                PropertyNames = new PropertyName[(int)PropertyNameCount];

                for (int i = 0; i < PropertyNameCount; i++)
                {
                    PropertyNames[i] = new PropertyName();
                    PropertyNames[i].Parse(s);
                }
            }
        }
    }
}
