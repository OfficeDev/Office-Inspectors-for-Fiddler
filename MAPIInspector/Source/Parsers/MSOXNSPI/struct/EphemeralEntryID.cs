using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.9 EntryIDs
    /// 2.2.9.2 EphemeralEntryID
    /// A class indicates the EphemeralEntryID structure.
    /// </summary>
    public class EphemeralEntryID : Block
    {
        /// <summary>
        /// The type of ID.
        /// </summary>
        public BlockT<byte> Type;

        /// <summary>
        /// Reserved, generally value is a constant 0x00.
        /// </summary>
        public BlockT<byte> R1;

        /// <summary>
        /// Reserved, generally value is a constant 0x00.
        /// </summary>
        public BlockT<byte> R2;

        /// <summary>
        /// Reserved, generally value is a constant 0x00.
        /// </summary>
        public BlockT<byte> R3;

        /// <summary>
        /// A FlatUID_r value contains the GUID of the server that issued Ephemeral Entry ID.
        /// </summary>
        public BlockGuid ProviderUID;

        /// <summary>
        /// Reserved, generally value is a constant 0x00000001.
        /// </summary>
        public BlockT<uint> R4;

        /// <summary>
        /// The display type of the object specified by Ephemeral Entry ID.
        /// </summary>
        public BlockT<DisplayTypeValues> DisplayType;

        /// <summary>
        /// The Minimal Entry ID of object.
        /// </summary>
        public MinimalEntryID Mid;

        /// <summary>
        /// Parse the EphemeralEntryID payload of session.
        /// </summary>
        protected override void Parse()
        {
            Type = ParseT<byte>();
            R1 = ParseT<byte>();
            R2 = ParseT<byte>();
            R3 = ParseT<byte>();
            ProviderUID = Parse<BlockGuid>();
            R4 = ParseT<uint>();
            DisplayType = ParseT<DisplayTypeValues>();
            Mid = Parse<MinimalEntryID>();
        }

        protected override void ParseBlocks()
        {
            Text = "EphemeralEntryID";
            AddChildBlockT(Type, "Type");
            AddChildBlockT(R1, "R1");
            AddChildBlockT(R2, "R2");
            AddChildBlockT(R3, "R3");
            this.AddChildGuid(ProviderUID, "ProviderUID");
            AddChildBlockT(R4, "R4");
            AddChildBlockT(DisplayType, "DisplayType");
            AddChild(Mid, "Mid");
        }
    }
}
