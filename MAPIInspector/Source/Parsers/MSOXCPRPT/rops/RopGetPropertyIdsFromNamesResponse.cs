namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.12 RopGetPropertyIdsFromNames
    ///  A class indicates the RopGetPropertyIdsFromNames ROP Response Buffer.
    /// </summary>
    public class RopGetPropertyIdsFromNamesResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        /// </summary>
        public ushort? PropertyIdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name
        /// </summary>
        public ushort?[] PropertyIds;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertyIdsFromNamesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                PropertyIdCount = ReadUshort();
                PropertyIds = ConvertArray(new ushort[(int)PropertyIdCount]);

                for (int i = 0; i < PropertyIdCount; i++)
                {
                    PropertyIds[i] = ReadUshort();
                }
            }
        }
    }
}
