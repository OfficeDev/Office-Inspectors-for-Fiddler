using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.9 EntryIDs
    /// 2.2.9.1 MinimalEntryID
    /// A class indicates the MinimalEntryID structure.
    /// </summary>
    public class MinimalEntryID : BaseStructure
    {
        /// <summary>
        /// A Minimal Entry ID is a single DWORD value that identifies a specific object in the address book.
        /// </summary>
        public uint MinEntryID;

        /// <summary>
        /// Parse the MinimalEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            MinEntryID = ReadUint();
        }
    }
}
