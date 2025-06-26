using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the Connect request type.
    ///  2.2.4 Request Types for Mailbox Server Endpoint
    ///  2.2.4.1 Connect
    /// </summary>
    public class ConnectRequestBody : BaseStructure
    {
        /// <summary>
        /// A null-terminated ASCII string that specifies the DN of the user who is requesting the connection. 
        /// </summary>
        public MAPIString UserDn;

        /// <summary>
        /// A set of flags that designate the type of connection being requested. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the code page that the server is being requested to use for string values of properties. 
        /// </summary>
        public uint DefaultCodePage;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for sorting. 
        /// </summary>
        public uint LcidSort;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for everything other than sorting. 
        /// </summary>
        public uint LcidString;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            UserDn = new MAPIString(Encoding.ASCII);
            UserDn.Parse(s);
            Flags = ReadUint();
            DefaultCodePage = ReadUint();
            LcidSort = ReadUint();
            LcidString = ReadUint();
            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
            else
            {
                AuxiliaryBuffer = null;
            }
        }
    }
}