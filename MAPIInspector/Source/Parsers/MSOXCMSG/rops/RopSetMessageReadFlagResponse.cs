namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.11 RopSetMessageReadFlag ROP
    /// A class indicates the RopSetMessageReadFlag ROP response Buffer.
    /// </summary>
    public class RopSetMessageReadFlagResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the read status of a public folder's message has changed.
        /// </summary>
        public bool? ReadStatusChanged;

        /// <summary>
        /// An unsigned integer index that is present when the value in the ReadStatusChanged field is nonzero and is not present
        /// </summary>
        public byte? LogonId;

        /// <summary>
        /// An array of bytes that is present when the value in the ReadStatusChanged field is nonzero and is not present 
        /// </summary>
        public byte?[] ClientData;

        /// <summary>
        /// Parse the RopSetMessageReadFlagResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageReadFlagResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            ResponseHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                ReadStatusChanged = ReadBoolean();

                if ((bool)ReadStatusChanged)
                {
                    LogonId = ReadByte();
                    ClientData = ConvertArray(ReadBytes(24));
                }
            }
        }
    }
}
