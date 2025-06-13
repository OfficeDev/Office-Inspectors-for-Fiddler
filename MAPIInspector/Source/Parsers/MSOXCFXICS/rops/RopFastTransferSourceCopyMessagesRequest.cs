namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyMessages ROP Request Buffer.
    ///  2.2.3.1.1.3.1 RopFastTransferSourceCopyMessages ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceCopyMessagesRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies the messages to copy. 
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation. 
        /// </summary>
        public CopyFlags_CopyMessages CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.MessageIdCount = this.ReadUshort();

            List<MessageID> messageIdList = new List<MessageID>();
            for (int i = 0; i < this.MessageIdCount; i++)
            {
                MessageID messageId = new MessageID();
                messageId.Parse(s);
                messageIdList.Add(messageId);
            }

            this.MessageIds = messageIdList.ToArray();
            this.CopyFlags = (CopyFlags_CopyMessages)ReadByte();
            this.SendOptions = (SendOptions)ReadByte();
        }
    }
}
