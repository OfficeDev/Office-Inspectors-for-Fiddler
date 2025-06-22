namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.12 RopHardDeleteMessages ROP
    /// A class indicates the RopHardDeleteMessages ROP Response Buffer.
    /// </summary>
    public class RopHardDeleteMessagesResponse : BaseStructure
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
        /// Parse the RopHardDeleteMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopHardDeleteMessagesResponse structure.</param>
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