using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.15   AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONTROL : BaseStructure
    {
        /// <summary>
        /// The flags that instruct the client to either enable or disable behavior. 
        /// </summary>
        public EnableFlags EnableFlags;

        /// <summary>
        /// The number of milliseconds the client keeps unsent performance data before the data is expired. 
        /// </summary>
        public uint ExpiryTime;

        /// <summary>
        /// Parse the AUX_CLIENT_CONTROL structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_CLIENT_CONTROL structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            EnableFlags = (EnableFlags)ReadUint();
            ExpiryTime = ReadUint();
        }
    }
}