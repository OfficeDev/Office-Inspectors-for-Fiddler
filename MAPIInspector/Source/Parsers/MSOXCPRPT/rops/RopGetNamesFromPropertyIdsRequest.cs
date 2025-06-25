namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.13 RopGetNamesFromPropertyIds
    ///  A class indicates the RopGetNamesFromPropertyIds ROP Request Buffer.
    /// </summary>
    public class RopGetNamesFromPropertyIdsRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        /// </summary>
        public ushort PropertyIdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers.
        /// </summary>
        public ushort[] PropertyIds;

        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetNamesFromPropertyIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            PropertyIdCount = ReadUshort();
            PropertyIds = new ushort[(int)PropertyIdCount];

            for (int i = 0; i < PropertyIdCount; i++)
            {
                PropertyIds[i] = ReadUshort();
            }
        }
    }
}
