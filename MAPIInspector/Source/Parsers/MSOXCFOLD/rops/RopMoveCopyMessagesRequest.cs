namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.1.6 RopMoveCopyMessages ROP
    /// The RopMoveCopyMessages ROP ([MS-OXCROPS] section 2.2.4.6) moves or copies messages from a source folder to a destination folder. 
    /// </summary>
    public class RopMoveCopyMessagesRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which messages to move or copy. 
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the operation is a copy or a move.
        /// </summary>
        public bool WantCopy;

        /// <summary>
        /// Parse the RopMoveCopyMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopMoveCopyMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            SourceHandleIndex = ReadByte();
            DestHandleIndex = ReadByte();
            MessageIdCount = ReadUshort();
            List<MessageID> tempMessageIDs = new List<MessageID>();
            for (int i = 0; i < MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                tempMessageIDs.Add(messageID);
            }

            MessageIds = tempMessageIDs.ToArray();
            WantAsynchronous = ReadBoolean();
            WantCopy = ReadBoolean();
        }
    }
}