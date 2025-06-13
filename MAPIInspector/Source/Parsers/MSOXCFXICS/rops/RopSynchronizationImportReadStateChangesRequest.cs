namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationImportReadStateChanges ROP Request Buffer.
    ///  2.2.3.2.4.6.1 RopSynchronizationImportReadStateChanges ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportReadStateChangesRequest : BaseStructure
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
        /// An unsigned integer that specifies the size, in bytes, of the MessageReadStates field.
        /// </summary>
        public ushort MessageReadStatesSize;

        /// <summary>
        /// A list of MessageReadState structures that specify the messages and associated read states to be changed.
        /// </summary>
        public MessageReadState[] MessageReadStates;

        /// <summary>
        /// Parse the RopSynchronizationImportReadStateChangesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportReadStateChangesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageReadStatesSize = this.ReadUshort();
            List<MessageReadState> interValue = new List<MessageReadState>();
            int size = this.MessageReadStatesSize;

            while (size > 0)
            {
                MessageReadState interValueI = new MessageReadState();
                interValueI.Parse(s);
                interValue.Add(interValueI);
                size -= interValueI.MessageId.Length + 1 + 2;
            }

            this.MessageReadStates = interValue.ToArray();
        }
    }
}
