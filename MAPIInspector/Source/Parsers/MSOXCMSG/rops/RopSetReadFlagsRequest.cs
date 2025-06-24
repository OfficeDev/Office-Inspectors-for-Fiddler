namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.6.10 RopSetReadFlags ROP
    /// A class indicates the RopSetReadFlags ROP request Buffer.
    /// </summary>
    public class RopSetReadFlagsRequest : BaseStructure
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A flags structure that contains flags that specify the flags to set.
        /// </summary>
        public ReadFlags ReadFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specify the messages that are to have their read flags changed.
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopSetReadFlagsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetReadFlagsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            WantAsynchronous = ReadBoolean();
            ReadFlags = (ReadFlags)ReadByte();
            MessageIdCount = ReadUshort();
            List<MessageID> messageIDs = new List<MessageID>();

            for (int i = 0; i < MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                messageIDs.Add(messageID);
            }

            MessageIds = messageIDs.ToArray();
        }
    }
}
