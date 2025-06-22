namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.1.11 RopDeleteMessages ROP
    /// The RopDeleteMessages ROP ([MS-OXCROPS] section 2.2.4.11) is used to soft delete one or more messages from a folder. 
    /// </summary>
    public class RopDeleteMessagesRequest : BaseStructure
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the server sends a non-read receipt to the message sender when a message is deleted.
        /// </summary>
        public bool NotifyNonRead;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies the messages to be deleted. T
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopDeleteMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            WantAsynchronous = ReadBoolean();
            NotifyNonRead = ReadBoolean();
            MessageIdCount = ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }

            MessageIds = tempMessageIDs.ToArray();
        }
    }
}