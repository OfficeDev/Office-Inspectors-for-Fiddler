namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.5 RopModifyRecipients ROP
    /// A class indicates the RopModifyRecipients ROP response Buffer.
    /// </summary>
    public class RopModifyRecipientsResponse : BaseStructure
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
        /// Parse the RopModifyRecipientsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopModifyRecipientsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
        }
    }
}
