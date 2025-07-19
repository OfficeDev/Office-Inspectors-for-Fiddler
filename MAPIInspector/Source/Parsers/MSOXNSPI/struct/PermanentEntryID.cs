using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.9 EntryIDs
    /// 2.2.9.3 PermanentEntryID
    /// A class indicates the PermanentEntryID structure.
    /// </summary>
    public class PermanentEntryID : Block
    {
        /// <summary>
        /// The type of ID.
        /// </summary>
        public BlockT<byte> IDType;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00.
        /// </summary>
        public BlockT<byte> R1;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00.
        /// </summary>
        public BlockT<byte> R2;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00.
        /// </summary>
        public BlockT<byte> R3;

        /// <summary>
        /// A FlatUID_r value that contains the constant GUID specified in Permanent Entry ID GUID,
        /// </summary>
        public BlockGuid ProviderUID;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00000001.
        /// </summary>
        public BlockT<uint> R4;

        /// <summary>
        /// The display type of the object specified by Permanent Entry ID.
        /// </summary>
        public BlockT<DisplayTypeValues> DisplayTypeString;

        /// <summary>
        /// The DN (1) of the object specified by Permanent Entry ID.
        /// </summary>
        public BlockString DistinguishedName; // Ascii

        /// <summary>
        /// Parse the PermanentEntryID payload of session.
        /// </summary>
        protected override void Parse()
        {
            IDType = ParseT<byte>();
            R1 = ParseT<byte>();
            R2 = ParseT<byte>();
            R3 = ParseT<byte>();
            ProviderUID = Parse<BlockGuid>();
            R4 = ParseT<uint>();
            DisplayTypeString = ParseT<DisplayTypeValues>();
            DistinguishedName = ParseStringA();
        }

        protected override void ParseBlocks()
        {
            Text = "PermanentEntryID";
            AddChildBlockT(IDType, "IDType ");
            AddChildBlockT(R1, "R1");
            AddChildBlockT(R2, "R2");
            AddChildBlockT(R3, "R3");
            this.AddChildGuid(ProviderUID, "ProviderUID");
            AddChildBlockT(R4, "R4");
            AddChildBlockT(DisplayTypeString, "DisplayTypeString");
            AddChildString(DistinguishedName, "DistinguishedName");
        }
    }
}
