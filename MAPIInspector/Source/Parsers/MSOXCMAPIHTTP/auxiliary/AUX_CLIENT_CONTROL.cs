using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.15 AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONTROL : Block
    {
        /// <summary>
        /// The flags that instruct the client to either enable or disable behavior.
        /// </summary>
        public BlockT<EnableFlags> EnableFlags;

        /// <summary>
        /// The number of milliseconds the client keeps unsent performance data before the data is expired.
        /// </summary>
        public BlockT<uint> ExpiryTime;

        /// <summary>
        /// Parse the AUX_CLIENT_CONTROL structure.
        /// </summary>
        protected override void Parse()
        {
            EnableFlags = ParseT<EnableFlags>();
            ExpiryTime = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            SetText("AUX_CLIENT_CONTROL");
            AddChildBlockT(EnableFlags, "EnableFlags");
            AddChildBlockT(ExpiryTime, "ExpiryTime");
        }
    }
}