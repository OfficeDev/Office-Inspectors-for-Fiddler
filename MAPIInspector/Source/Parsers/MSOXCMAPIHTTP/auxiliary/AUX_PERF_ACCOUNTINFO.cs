using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.18 AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_ACCOUNTINFO : Block
    {
        /// <summary>
        /// The client-assigned identification number.
        /// </summary>
        public BlockT<ushort> ClientID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// A GUID representing the client account information that relates to the client identification number in the ClientID field.
        /// </summary>
        public BlockGuid Account;

        /// <summary>
        /// Parse the AUX_PERF_ACCOUNTINFO structure.
        /// </summary>
        protected override void Parse()
        {
            ClientID = ParseT<ushort>();
            Reserved = ParseT<ushort>();
            Account = Parse<BlockGuid>();
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_ACCOUNTINFO";
            AddChildBlockT(ClientID, "ClientID");
            AddChildBlockT(Reserved, "Reserved");
            this.AddChildGuid(Account, "Account");
        }
    }
}