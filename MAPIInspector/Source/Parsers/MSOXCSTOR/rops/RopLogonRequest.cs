using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.1 RopLogon
    /// A class indicates the RopLogon ROP Request Buffer.
    /// </summary>
    public class RopLogonRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the RopLogon.
        /// </summary>
        public BlockT<LogonFlags> LogonFlags;

        /// <summary>
        /// A flags structure that contains more flags that control the behavior of the RopLogon.
        /// </summary>
        public BlockT<OpenFlags> OpenFlags;

        /// <summary>
        /// A flags structure. This field is not used and is ignored by the server.
        /// </summary>
        public BlockT<uint> StoreState;

        /// <summary>
        /// An unsigned integer that specifies the size of the ESSDN field.
        /// </summary>
        public BlockT<ushort> EssdnSize;

        /// <summary>
        /// A null-terminated ASCII string that specifies which mailbox to log on to. 
        /// </summary>
        public BlockString Essdn;

        /// <summary>
        /// Parse the RopLogonRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            LogonFlags = ParseT<LogonFlags>();
            OpenFlags = ParseT<OpenFlags>();
            StoreState = ParseT<uint>();
            EssdnSize = ParseT<ushort>();
            if (EssdnSize > 0)
            {
                Essdn = ParseStringA();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopLogonRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(LogonFlags, "LogonFlags");
            AddChildBlockT(OpenFlags, "OpenFlags");
            AddChildBlockT(StoreState, "StoreState");
            AddChildBlockT(EssdnSize, "EssdnSize");
            AddChildString(Essdn, "Essdn");
        }
    }
}
