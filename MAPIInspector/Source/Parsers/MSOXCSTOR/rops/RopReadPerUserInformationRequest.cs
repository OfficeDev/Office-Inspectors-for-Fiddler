namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.12 RopReadPerUserInformation
    ///  A class indicates the RopReadPerUserInformation ROP Request Buffer.
    /// </summary>
    public class RopReadPerUserInformationRequest : BaseStructure
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
        /// A LongTermID structure that specifies the folder for which to get per-user information.
        /// </summary>
        public LongTermID FolderId;

        /// <summary>
        /// Reserved field.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// An unsigned integer that specifies the location at which to start reading within the per-user information to be retrieved.
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes of per-user information to be retrieved.
        /// </summary>
        public ushort MaxDataSize;

        /// <summary>
        /// Parse the RopReadPerUserInformationRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadPerUserInformationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            FolderId = new LongTermID();
            FolderId.Parse(s);
            Reserved = ReadByte();
            DataOffset = ReadUint();
            MaxDataSize = ReadUshort();
        }
    }
}
