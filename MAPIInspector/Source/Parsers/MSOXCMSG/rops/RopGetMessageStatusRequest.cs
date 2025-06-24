namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.9 RopGetMessageStatus ROP
    /// A class indicates the RopGetMessageStatus ROP request Buffer.
    /// </summary>
    public class RopGetMessageStatusRequest : BaseStructure
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
        /// An identifier that specifies the message for which the status will be returned.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopGetMessageStatusRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetMessageStatusRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
        }
    }
}
