using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.4.1 RopSetSearchCriteria ROP Request Buffer
    /// </summary>
    [Flags]
    public enum SearchRequestFlags : uint
    {
        /// <summary>
        /// The search is aborted
        /// </summary>
        STOP_SEARCH = 0x00000001,

        /// <summary>
        /// The search is initiated
        /// </summary>
        RESTART_SEARCH = 0x00000002,

        /// <summary>
        /// The search includes the search folder containers and all of their child folders.
        /// </summary>
        RECURSIVE_SEARCH = 0x00000004,

        /// <summary>
        /// The search includes only the search folder containers that are specified in the FolderIds field
        /// </summary>
        SHALLOW_SEARCH = 0x00000008,

        /// <summary>
        /// The search uses a content-indexed search
        /// </summary>
        CONTENT_INDEXED_SEARCH = 0x00010000,

        /// <summary>
        /// The search does not use a content-indexed search
        /// </summary>
        NON_CONTENT_INDEXED_SEARCH = 0x00020000,

        /// <summary>
        /// The search is static
        /// </summary>
        STATIC_SEARCH = 0x00040000
    }
}