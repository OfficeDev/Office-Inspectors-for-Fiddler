using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Section 2.2.1.5.2   RopGetSearchCriteria ROP Response Buffer
    /// </summary>
    [Flags]
    public enum SearchResponseFlags : uint
    {
        /// <summary>
        /// The search is running
        /// </summary>
        SEARCH_RUNNING = 0x00000001,

        /// <summary>
        /// The search is in the CPU-intensive part of the search
        /// </summary>
        SEARCH_REBUILD = 0x00000002,

        /// <summary>
        /// the specified search folder containers and all their child search folder containers are searched for matching entries
        /// </summary>
        SEARCH_RECURSIVE = 0x00000004,

        /// <summary>
        /// The search results are complete
        /// </summary>
        SEARCH_COMPLETE = 0x00001000,

        /// <summary>
        /// Only some parts of messages were included
        /// </summary>
        SEARCH_PARTIAL = 0x00002000,

        /// <summary>
        /// The search is static
        /// </summary>
        SEARCH_STATIC = 0x00010000,

        /// <summary>
        /// The search is still being evaluated
        /// </summary>
        SEARCH_MAYBE_STATIC = 0x00020000,

        /// <summary>
        /// The search is done using content indexing.
        /// </summary>
        CI_TOTALLY = 0x01000000,

        /// <summary>
        /// The search is done without using content indexing
        /// </summary>
        TWIR_TOTALLY = 0x08000000
    }
}