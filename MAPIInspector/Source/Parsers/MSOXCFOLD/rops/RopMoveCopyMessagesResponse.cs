namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.6 RopMoveCopyMessages ROP
    /// A class indicates the RopMoveCopyMessages ROP Response Buffer.
    /// </summary>
    public class RopMoveCopyMessagesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field in the request.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request. 
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// A Boolean that indicates whether the operation was only partially completed.
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopMoveCopyMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopMoveCopyMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            SourceHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                DestHandleIndex = ReadUint();
                PartialCompletion = ReadBoolean();
            }
            else
            {
                PartialCompletion = ReadBoolean();
            }
        }
    }
}