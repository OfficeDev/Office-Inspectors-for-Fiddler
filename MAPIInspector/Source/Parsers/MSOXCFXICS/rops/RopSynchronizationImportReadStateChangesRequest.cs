namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    ///  A class indicates the RopSynchronizationImportReadStateChanges ROP Request Buffer.
    ///  2.2.3.2.4.6.1 RopSynchronizationImportReadStateChanges ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportReadStateChangesRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the MessageReadStates field.
        /// </summary>
        public BlockT<ushort> MessageReadStatesSize;

        /// <summary>
        /// A list of MessageReadState structures that specify the messages and associated read states to be changed.
        /// </summary>
        public MessageReadState[] MessageReadStates;

        /// <summary>
        /// Parse the RopSynchronizationImportReadStateChangesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            MessageReadStatesSize = ParseT<ushort>();

            var interValue = new List<MessageReadState>();
            parser.PushCap(MessageReadStatesSize.Data);
            while (!parser.Empty)
            {
                interValue.Add(Parse<MessageReadState>());
            }
            parser.PopCap();
            MessageReadStates = interValue.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationImportReadStateChangesRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(MessageReadStatesSize, "MessageReadStatesSize");
            AddLabeledChildren(MessageReadStates, "MessageReadStates");
        }
    }
}
