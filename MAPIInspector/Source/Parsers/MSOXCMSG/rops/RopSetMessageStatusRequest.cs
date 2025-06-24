namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.8 RopSetMessageStatus ROP
    /// A class indicates the RopSetMessageStatus ROP request Buffer.
    /// </summary>
    public class RopSetMessageStatusRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the message for which the status will be changed.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// A flags structure that contains status flags to set on the message.
        /// </summary>
        public MessageStatusFlag MessageStatusFlags;

        /// <summary>
        /// A bitmask that specifies which bits in the MessageStatusFlags field are to be changed.
        /// </summary>
        public uint MessageStatusMask;

        /// <summary>
        /// Parse the RopSetMessageStatusRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageStatusRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            MessageId = new MessageID();
            MessageId.Parse(s);
            MessageStatusFlags = (MessageStatusFlag)ReadUint();
            MessageStatusMask = ReadUint();
        }
    }
}
