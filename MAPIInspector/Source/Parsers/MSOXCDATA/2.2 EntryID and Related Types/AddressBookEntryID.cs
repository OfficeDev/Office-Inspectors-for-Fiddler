using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.2 Address Book EntryID Structure
    /// </summary>
    public class AddressBookEntryID : Block
    {
        /// <summary>
        /// This value MUST be set to 0x00000000, indicating a long-term EntryID.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// The identifier for the provider that created the EntryID.
        /// </summary>
        public BlockGuid ProviderUID;

        /// <summary>
        /// This value MUST be set to %x01.00.00.00.
        /// </summary>
        public BlockT<uint> Version;

        /// <summary>
        /// An integer representing the type of the object.
        /// </summary>
        public BlockT<AddressbookEntryIDtype> Type;

        /// <summary>
        /// The X500 DN of the Address Book object.
        /// </summary>
        public BlockString X500DN; // Ascii

        /// <summary>
        /// Parse the AddressBookEntryID structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            ProviderUID = Parse<BlockGuid>();
            Version = ParseT<uint>();
            Type = ParseT<AddressbookEntryIDtype>();
            X500DN = ParseStringA();
        }

        protected override void ParseBlocks()
        {
            Text = "AddressBookEntryID";
            AddChildBlockT(Flags, "Flags");
            this.AddChildGuid(ProviderUID, "ProviderUID");
            AddChildBlockT(Version, "Version");
            AddChildBlockT(Type, "Type");
            AddChildString(X500DN, "X500DN");
        }
    }
}
