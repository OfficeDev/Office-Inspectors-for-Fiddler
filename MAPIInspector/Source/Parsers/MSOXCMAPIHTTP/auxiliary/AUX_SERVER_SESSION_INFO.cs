using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.21   AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_SERVER_SESSION_INFO : BaseStructure
    {
        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerSessionContextInfo field.
        /// </summary>
        public ushort OffsetServerSessionContextInfo;

        /// <summary>
        /// A null-terminated Unicode string that contains opaque server session context information to be logged by the client.
        /// </summary>
        public MAPIString ServerSessionContextInfo;

        /// <summary>
        /// Parse the AUX_SERVER_SESSION_INFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_SERVER_SESSION_INFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            OffsetServerSessionContextInfo = ReadUshort();

            if (OffsetServerSessionContextInfo != 0)
            {
                ServerSessionContextInfo = new MAPIString(Encoding.Unicode);
                ServerSessionContextInfo.Parse(s);
            }
        }
    }
}