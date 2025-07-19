using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.21 AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_SERVER_SESSION_INFO : Block
    {
        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerSessionContextInfo field.
        /// </summary>
        public BlockT<ushort> OffsetServerSessionContextInfo;

        /// <summary>
        /// A null-terminated Unicode string that contains opaque server session context information to be logged by the client.
        /// </summary>
        public BlockString ServerSessionContextInfo;

        /// <summary>
        /// Parse the AUX_SERVER_SESSION_INFO structure.
        /// </summary>
        protected override void Parse()
        {
            OffsetServerSessionContextInfo = ParseT<ushort>();

            if (OffsetServerSessionContextInfo != 0)
            {
                ServerSessionContextInfo = ParseStringW();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_SERVER_SESSION_INFO";
            AddChildBlockT(OffsetServerSessionContextInfo, "OffsetServerSessionContextInfo");
            AddChildString(ServerSessionContextInfo, "ServerSessionContextInfo");
        }
    }
}