namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.3 RopSaveChangesMessage ROP
    /// A class indicates the RopSaveChangesMessage ROP response Buffer.
    /// </summary>
    public class RopSaveChangesMessageResponse : BaseStructure
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
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte? InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the ID of the message saved.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSaveChangesMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesMessageResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            ResponseHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                InputHandleIndex = ReadByte();
                MessageId = new MessageID();
                MessageId.Parse(s);
            }
        }
    }
}
