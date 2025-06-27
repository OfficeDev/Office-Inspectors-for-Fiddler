using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetMailboxUrlRequest structure.
    /// 2.2.5.18 GetMailboxUrl
    /// </summary>
    public class GetMailboxUrlRequest : BaseStructure
    {
        /// <summary>
        /// Not used. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A null-terminated Unicode string that specifies the distinguished name (DN) of the mailbox server for which to look up the URL.
        /// </summary>
        public MAPIString ServerDn;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMailboxUrlRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetMailboxUrlRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            ServerDn = new MAPIString(Encoding.Unicode);
            ServerDn.Parse(s);
            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}