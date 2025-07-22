using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.26 RopWriteAndCommitStream
    /// A class indicates the RopWriteAndCommitStream ROP Request Buffer.
    /// </summary>
    public class RopWriteAndCommitStreamRequest : Block
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
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public BlockT<ushort> DataSize;

        /// <summary>
        /// An array of bytes that specifies the bytes to be written to the stream. The size of this field, in bytes, is specified by the DataSize field.
        /// </summary>
        public BlockBytes Data;

        /// <summary>
        /// Parse the RopWriteAndCommitStreamRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            DataSize = ParseT<ushort>();
            Data = ParseBytes(DataSize);
        }

        protected override void ParseBlocks()
        {
            Text = "RopWriteAndCommitStreamRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(DataSize, "DataSize");
            AddChildBytes(Data, "Data");
        }
    }
}
