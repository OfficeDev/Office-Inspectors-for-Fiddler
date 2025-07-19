using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.9 EntryIDs
    /// 2.2.9.1 MinimalEntryID
    /// A class indicates the MinimalEntryID structure.
    /// </summary>
    public class MinimalEntryID : Block
    {
        /// <summary>
        /// A Minimal Entry ID is a single DWORD value that identifies a specific object in the address book.
        /// </summary>
        public BlockT<uint> MinEntryID;

        /// <summary>
        /// Parse the MinimalEntryID payload of session.
        /// </summary>
        protected override void Parse()
        {
            MinEntryID = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "MinimalEntryID";
            AddChildBlockT(MinEntryID, "MinEntryID");
        }
    }
}
