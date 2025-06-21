namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.2.5.2  Address Book EntryID Structure
    /// </summary>
    public class AddressBookEntryID : BaseStructure
    {
        /// <summary>
        /// This value MUST be set to 0x00000000, indicating a long-term EntryID.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// The identifier for the provider that created the EntryID.
        /// </summary>
        public byte[] ProviderUID;

        /// <summary>
        /// This value MUST be set to %x01.00.00.00.
        /// </summary>
        public uint Version;

        /// <summary>
        /// An integer representing the type of the object.
        /// </summary>
        public AddressbookEntryIDtype Type;

        /// <summary>
        /// The X500 DN of the Address Book object.
        /// </summary>
        public MAPIString X500DN;

        /// <summary>
        /// Parse the AddressBookEntryID structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressBookEntryID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            ProviderUID = ReadBytes(16);
            Version = ReadUint();
            Type = (AddressbookEntryIDtype)ReadUint();
            X500DN = new MAPIString(Encoding.ASCII);
            X500DN.Parse(s);
        }
    }
}
