using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.6.2 PropertyName_r Structure
    /// </summary>
    public class PropertyName_r : Block
    {
        /// <summary>
        /// Encodes the GUID field of the PropertyName structure, as specified in section 2.6.1.
        /// </summary>
        public BlockGuid GUID;

        /// <summary>
        /// All clients and servers MUST set this value to 0x00000000.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// This value encodes the LID field in the PropertyName structure, as specified in section 2.6.1.
        /// </summary>
        public BlockT<uint> LID;

        /// <summary>
        /// Parse the PropertyName_r structure.
        /// </summary>
        protected override void Parse()
        {
            GUID = Parse<BlockGuid>();
            Reserved = ParseT<uint>();
            LID = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "PropertyName_r";
            this.AddChildGuid(GUID, "GUID");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(LID, "LID");
        }
    }
}
