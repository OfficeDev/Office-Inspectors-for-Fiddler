using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_SERVERINFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.5   AUX_PERF_SERVERINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SERVERINFO : BaseStructure
    {
        /// <summary>
        /// The client-assigned server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The server type assigned by client.
        /// </summary>
        public ServerType ServerType;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerDN field.
        /// </summary>
        public ushort ServerDNOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerName field.
        /// </summary>
        public ushort ServerNameOffset;

        /// <summary>
        /// A null-terminated Unicode string that contains the DN of the server.
        /// </summary>
        public MAPIString ServerDN;

        /// <summary>
        /// A null-terminated Unicode string that contains the server name.
        /// </summary>
        public MAPIString ServerName;

        /// <summary>
        /// Parse the AUX_PERF_SERVERINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SERVERINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ServerID = ReadUshort();
            ServerType = (ServerType)ReadUshort();
            ServerDNOffset = ReadUshort();
            ServerNameOffset = ReadUshort();

            if (ServerDNOffset != 0)
            {
                ServerDN = new MAPIString(Encoding.Unicode);
                ServerDN.Parse(s);
            }

            if (ServerNameOffset != 0)
            {
                ServerName = new MAPIString(Encoding.Unicode);
                ServerName.Parse(s);
            }
        }
    }
}