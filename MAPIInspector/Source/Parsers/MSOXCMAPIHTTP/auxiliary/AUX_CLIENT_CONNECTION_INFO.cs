using System;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.20   AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONNECTION_INFO : BaseStructure
    {
        /// <summary>
        /// The GUID of the connection to the server.
        /// </summary>
        public Guid ConnectionGUID;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ConnectionContextInfo field.
        /// </summary>
        public ushort OffsetConnectionContextInfo;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The number of connection attempts.
        /// </summary>
        public uint ConnectionAttempts;

        /// <summary>
        /// A flag designating the mode of operation.
        /// </summary>
        public ConnectionFlags ConnectionFlags;

        /// <summary>
        /// A null-terminated Unicode string that contains opaque connection context information to be logged by the server.
        /// </summary>
        public MAPIString ConnectionContextInfo;

        /// <summary>
        /// Parse the AUX_CLIENT_CONNECTION_INFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_ENDPOINT_CAPABILITIES structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ConnectionGUID = ReadGuid();
            OffsetConnectionContextInfo = ReadUshort();
            Reserved = ReadUshort();
            ConnectionAttempts = ReadUint();
            ConnectionFlags = (ConnectionFlags)ReadUint();

            if (OffsetConnectionContextInfo != 0)
            {
                ConnectionContextInfo = new MAPIString(Encoding.Unicode);
                ConnectionContextInfo.Parse(s);
            }
        }
    }
}