namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.9 RopEmptyFolder ROP
    /// A class indicates the RopEmptyFolder ROP Response Buffer.
    /// </summary>
    public class RopEmptyFolderResponse : BaseStructure
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
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopEmptyFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopEmptyFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            PartialCompletion = ReadBoolean();
        }
    }
}