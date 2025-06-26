using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the GetAddressBookUrlRequest structure.
    ///  2.2.5.19 GetAddressBookUrl
    /// </summary>
    public class GetAddressBookUrlRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A null-terminated Unicode string that specifies the distinguished name (DN) of the user's mailbox. 
        /// </summary>
        public MAPIString UserDn;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetAddressBookUrlRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetAddressBookUrlRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            Flags = ReadUint();
            UserDn = new MAPIString(Encoding.Unicode);
            UserDn.Parse(s);
            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}