namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.2.4.3 Store Object EntryID Structure
    /// </summary>
    public class StoreObjectEntryID : BaseStructure
    {
        /// <summary>
        /// This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// The identifier for the provider that created the EntryID.
        /// </summary>
        public byte[] ProviderUID;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public byte Version;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// This field MUST be set to the following value, which represents "emsmdb.dll": %x45.4D.53.4D.44.42.2E.44.4C.4C.00.00.00.00.
        /// </summary>
        public byte[] DLLFileName;

        /// <summary>
        /// This value MUST be set to 0x00000000
        /// </summary>
        public uint WrappedFlags;

        /// <summary>
        /// This Wrapped Provider UID.
        /// </summary>
        public byte[] WrappedProviderUID;

        /// <summary>
        /// The value of this field is determined by where the folder is located.
        /// </summary>
        public uint WrappedType;

        /// <summary>
        /// A string of single-byte characters terminated by a single zero byte, indicating the short name or NetBIOS name of the server.
        /// </summary>
        public MAPIString ServerShortname;

        /// <summary>
        /// A string of single-byte characters terminated by a single zero byte and representing the X500 DN of the mailbox, as specified in [MS-OXOAB].
        /// </summary>
        public MAPIString MailboxDN;

        /// <summary>
        /// Parse the StoreObjectEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the StoreObjectEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            ProviderUID = ReadBytes(16);
            Version = ReadByte();
            Flag = ReadByte();
            DLLFileName = ReadBytes(14);
            WrappedFlags = ReadUint();
            WrappedProviderUID = ReadBytes(16);
            WrappedType = ReadUint();
            ServerShortname = new MAPIString(Encoding.ASCII);
            ServerShortname.Parse(s);
            MailboxDN = new MAPIString(Encoding.ASCII);
            MailboxDN.Parse(s);
        }
    }
}
