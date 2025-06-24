namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.2.6.11 RopSetMessageReadFlag ROP
    /// A class indicates the RopSetMessageReadFlag ROP request Buffer.
    /// </summary>
    public class RopSetMessageReadFlagRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response.
        /// </summary>
        public BlockT<byte> ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A flags structure. The possible values for these flags are specified in [MS-OXCMSG] section 2.2.3.11.1.
        /// </summary>
        public BlockT<ReadFlags> ReadFlags;

        /// <summary>
        /// An array of bytes that is present when the RopLogon associated with LogonId was created with the Private flag
        /// </summary>
        public BlockBytes ClientData; // 24 bytes

        /// <summary>
        /// Parse the RopSetMessageReadFlagRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            ResponseHandleIndex = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ReadFlags = ParseT<ReadFlags>();
            if(!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                if (((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIParser.ParsingSession.id][LogonId.Data] & (byte)LogonFlags.Private) != (byte)LogonFlags.Private)
                {
                    ClientData = ParseBytes(24);
                }
            }
            else
            {
                if (((byte)DecodingContext.SessionLogonFlagMapLogId[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][LogonId.Data] & (byte)LogonFlags.Private) != (byte)LogonFlags.Private)
                {
                    ClientData = ParseBytes(24);
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetMessageReadFlagRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ReadFlags, "ReadFlags");
            if (ClientData != null) AddChild(ClientData, $"ClientData:{ClientData.ToHexString(false)}");
        }
    }
}
