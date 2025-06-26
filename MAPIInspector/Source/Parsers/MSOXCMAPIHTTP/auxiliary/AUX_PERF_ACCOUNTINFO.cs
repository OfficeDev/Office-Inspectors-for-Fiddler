using System;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.18   AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_ACCOUNTINFO : BaseStructure
    {
        /// <summary>
        /// The client-assigned identification number. 
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A GUID representing the client account information that relates to the client identification number in the ClientID field.
        /// </summary>
        public Guid Account;

        /// <summary>
        /// Parse the AUX_PERF_ACCOUNTINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_ACCOUNTINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ClientID = ReadUshort();
            Reserved = ReadUshort();
            Account = ReadGuid();
        }
    }
}