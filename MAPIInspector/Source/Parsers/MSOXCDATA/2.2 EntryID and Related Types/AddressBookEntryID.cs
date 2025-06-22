namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;

    /// <summary>
    /// 2.2.5.2  Address Book EntryID Structure
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
        public BlockT<Guid> ProviderUID;

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
        public BlockStringA X500DN;

        /// <summary>
        /// Parse the AddressBookEntryID structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            ProviderUID = ParseT<Guid>();
            Version = ParseT<uint>();
            Type = ParseT<AddressbookEntryIDtype>();
            X500DN = ParseStringA();
        }

        protected override void ParseBlocks()
        {
            SetText("AddressBookEntryID");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(ProviderUID, "ProviderUID");
            AddChildBlockT(Version, "Version");
            AddChildBlockT(Type, "Type");
            if (X500DN != null)
            {
                AddChild(X500DN, $"X500DN:{X500DN}");
            }
        }
    }
}
