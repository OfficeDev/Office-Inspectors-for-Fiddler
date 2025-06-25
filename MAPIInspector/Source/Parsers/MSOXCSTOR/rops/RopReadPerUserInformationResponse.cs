namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.12 RopReadPerUserInformation
    ///  A class indicates the RopReadPerUserInformation ROP Response Buffer.
    /// </summary>
    public class RopReadPerUserInformationResponse : BaseStructure
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
        /// A Boolean that specifies whether this operation reached the end of the per-user information stream.
        /// </summary>
        public bool? HasFinished;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort? DataSize;

        /// <summary>
        /// An array of bytes. This field contains the per-user data that is returned.
        /// </summary>
        public byte?[] Data;

        /// <summary>
        /// Parse the RopReadPerUserInformationResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadPerUserInformationResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                HasFinished = ReadBoolean();
                DataSize = ReadUshort();
                Data = ConvertArray(ReadBytes((int)DataSize));
            }
        }
    }
}
