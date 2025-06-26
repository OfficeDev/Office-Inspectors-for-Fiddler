using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  2.2.1.13 RopWritePerUserInformation
    ///  A class indicates the RopWritePerUserInformation ROP Request Buffer.
    /// </summary>
    public class RopWritePerUserInformationRequest : Block
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
        /// A LongTermID structure that specifies the folder to set per-user information for.
        /// </summary>
        public LongTermID FolderId;

        /// <summary>
        /// A Boolean that specifies whether this operation specifies the end of the per-user information stream.
        /// </summary>
        public BlockT<bool> HasFinished;

        /// <summary>
        /// An unsigned integer that specifies the location in the per-user information stream to start writing
        /// </summary>
        public BlockT<uint> DataOffset;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public BlockT<ushort> DataSize;

        /// <summary>
        /// An array of bytes that is the per-user data to write.
        /// </summary>
        public BlockBytes Data;

        /// <summary>
        /// An GUID that is present when the DataOffset is 0 and the RopLogon associated with the LogonId field was created with the Private flag set in the RopLogon ROP request buffer
        /// </summary>
        public BlockGuid ReplGuid;

        /// <summary>
        /// Parse the RopWritePerUserInformationRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            FolderId = Parse<LongTermID>();
            HasFinished = ParseAs<byte, bool>();
            DataOffset = ParseT<uint>();
            DataSize = ParseT<ushort>();
            Data = ParseBytes(DataSize.Data);
            if (!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                if (DataOffset.Data == 0 &&
                    (((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIParser.ParsingSession.id][LogonId.Data] & (byte)LogonFlags.Private) == (byte)LogonFlags.Private))
                {
                    ReplGuid = Parse<BlockGuid>();
                }
            }
            else
            {
                if (DataOffset.Data == 0 &&
                    (((byte)DecodingContext.SessionLogonFlagMapLogId[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][LogonId.Data] & (byte)LogonFlags.Private) == (byte)LogonFlags.Private))
                {
                    ReplGuid = Parse<BlockGuid>();
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopWritePerUserInformationRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(FolderId, "FolderId");
            AddChildBlockT(HasFinished, "HasFinished");
            AddChildBlockT(DataOffset, "DataOffset");
            AddChildBlockT(DataSize, "DataSize");
            AddChildBytes(Data, "Data");
            this.AddChildGuid(ReplGuid, "ReplGuid");
        }
    }
}
