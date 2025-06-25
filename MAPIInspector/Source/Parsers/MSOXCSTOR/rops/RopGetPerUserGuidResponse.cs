namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    ///  2.2.1.11 RopGetPerUserGuid
    ///  A class indicates the RopGetPerUserGuid ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserGuidResponse : BaseStructure
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
        /// A GUID that specifies the database for which per-user information was obtained.
        /// </summary>
        public Guid? DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserGuidResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPerUserGuidResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                DatabaseGuid = ReadGuid();
            }
        }
    }
}
