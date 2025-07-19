using BlockParser;
using System.Net.Mail;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.16 RopOpenEmbeddedMessage ROP
    /// A class indicates the RopOpenEmbeddedMessage ROP request Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An identifier that specifies which code page is used for string values associated with the message.
        /// </summary>
        public BlockT<ushort> CodePageId;

        /// <summary>
        /// A flags structure that contains flags that control the access to the message.
        /// </summary>
        public BlockT<OpenMessageModeFlags> OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            CodePageId = ParseT<ushort>();
            OpenModeFlags = ParseT<OpenMessageModeFlags>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopOpenEmbeddedMessageRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(CodePageId, "CodePageId");
            AddChildBlockT(OpenModeFlags, "OpenModeFlags");
        }
    }
}
