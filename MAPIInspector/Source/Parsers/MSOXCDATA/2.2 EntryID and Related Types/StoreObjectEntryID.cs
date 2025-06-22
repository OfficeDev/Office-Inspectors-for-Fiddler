namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.2.4.3 Store Object EntryID Structure
    /// </summary>
    public class StoreObjectEntryID : Block
    {
        /// <summary>
        /// This value MUST be set to 0x00000000. Bits in this field indicate under what circumstances a short-term EntryID is valid.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// The identifier for the provider that created the EntryID.
        /// </summary>
        public BlockGuid ProviderUID;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public BlockT<byte> Version;

        /// <summary>
        /// This value MUST be set to zero.
        /// </summary>
        public BlockT<byte> Flag;

        /// <summary>
        /// This field MUST be set to the following value, which represents "emsmdb.dll": %x45.4D.53.4D.44.42.2E.44.4C.4C.00.00.00.00.
        /// </summary>
        public BlockBytes DLLFileName; // 14 bytes

        /// <summary>
        /// This value MUST be set to 0x00000000
        /// </summary>
        public BlockT<uint> WrappedFlags;

        /// <summary>
        /// This Wrapped Provider UID.
        /// </summary>
        public BlockGuid WrappedProviderUID;

        /// <summary>
        /// The value of this field is determined by where the folder is located.
        /// </summary>
        public BlockT<uint> WrappedType;

        /// <summary>
        /// A string of single-byte characters terminated by a single zero byte, indicating the short name or NetBIOS name of the server.
        /// </summary>
        public BlockString ServerShortname;

        /// <summary>
        /// A string of single-byte characters terminated by a single zero byte and representing the X500 DN of the mailbox, as specified in [MS-OXOAB].
        /// </summary>
        public BlockString MailboxDN;

        /// <summary>
        /// Parse the StoreObjectEntryID structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            ProviderUID = Parse<BlockGuid>();
            Version = ParseT<byte>();
            Flag = ParseT<byte>();
            DLLFileName = ParseBytes(14);
            WrappedFlags = ParseT<uint>();
            WrappedProviderUID = Parse<BlockGuid>();
            WrappedType = ParseT<uint>();
            ServerShortname = ParseStringA();
            MailboxDN = ParseStringA();
        }

        protected override void ParseBlocks()
        {
            SetText("StoreObjectEntryID");
            AddChildBlockT(Flags, "Flags");
            AddChild(ProviderUID, $"ProviderUID:{ProviderUID}");
            AddChildBlockT(Version, "Version");
            AddChildBlockT(Flag, "Flag");
            if (DLLFileName != null) AddChild(DLLFileName, $"DLLFileName:{DLLFileName.ToHexString(false)}");
            AddChildBlockT(WrappedFlags, "WrappedFlags");
            AddChild(WrappedProviderUID, $"WrappedProviderUID:{WrappedProviderUID}");
            AddChildBlockT(WrappedType, "WrappedType");
            AddChild(ServerShortname, $"ServerShortname:{ServerShortname}");
            AddChild(MailboxDN, $"MailboxDN:{MailboxDN}");
        }
    }
}
