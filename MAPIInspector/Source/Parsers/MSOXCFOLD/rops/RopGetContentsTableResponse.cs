namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.14 RopGetContentsTable ROP
    /// A class indicates the RopGetContentsTable ROP Response Buffer.
    /// </summary>
    public class RopGetContentsTableResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that represents the number of rows in the hierarchy table. 
        /// </summary>
        public uint? RowCount;

        /// <summary>
        /// Parse the RopGetContentsTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetContentsTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                RowCount = ReadUint();
            }
        }
    }
}