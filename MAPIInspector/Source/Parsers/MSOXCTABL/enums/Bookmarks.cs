namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.1.1 Predefined Bookmarks
    /// </summary>
    public enum Bookmarks : byte
    {
        /// <summary>
        /// Points to the beginning position of the table, or the first row.
        /// </summary>
        BOOKMARK_BEGINNING = 0x00,

        /// <summary>
        /// Points to the current position of the table, or the current row.
        /// </summary>
        BOOKMARK_CURRENT = 0x01,

        /// <summary>
        /// Points to the ending position of the table, or the location after the last row
        /// </summary>
        BOOKMARK_END = 0x02,

        /// <summary>
        /// Points to the custom position in the table. Used with the BookmarkSize and Bookmark fields.
        /// </summary>
        BOOKMARK_CUSTOM = 0x03
    }
}
