using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXNSPI] 2.2.9 EntryIDs
    /// [MS-OXCMAPIHTTP] 2.2.5.10 ModLinkAtt
    /// [MS-OXNSPI] 2.2.9.3 PermanentEntryID
    /// [MS-OXNSPI] 2.2.9.2 EphemeralEntryID
    /// A class indicates the EphemeralEntryID structure.
    /// </summary>
    public class EntryID : Block
    {
        /// <summary>
        /// The size of the ID. This field is missing from the ModLinkAtt documentation.
        /// </summary>
        public BlockT<uint> EntryIDSize;

        /// <summary>
        /// A PermanentEntryID structure that specifies a Permanent Entry ID.
        /// </summary>
        public PermanentEntryID PermanentEntryID;

        /// <summary>
        /// A EphemeralEntryID structure that specifies a Ephemeral Entry ID.
        /// </summary>
        public EphemeralEntryID EphemeralEntryID;

        /// <summary>
        /// Parse the EntryID payload of session.
        /// </summary>
        protected override void Parse()
        {
            EntryIDSize = ParseT<uint>();
            var currentByte = TestParse<byte>();
            if (currentByte == 0x87)
            {
                EphemeralEntryID = Parse<EphemeralEntryID>();
            }
            else if (currentByte == 0x00)
            {
                PermanentEntryID = Parse<PermanentEntryID>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "EntryID";
            AddChildBlockT(EntryIDSize, "EntryIDSize");
            AddChild(EphemeralEntryID, "EphemeralEntryID");
            AddChild(PermanentEntryID, "PermanentEntryID");
        }
    }
}
