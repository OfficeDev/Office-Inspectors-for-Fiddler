namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCRPC] 2.2.2.2 AUX_HEADER Structure
    /// The enum type corresponding auxiliary block structure that follows the AUX_HEADER structure when the Version field is AUX_VERSION_2. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum AuxiliaryBlockType_2 : byte
    {
        /// <summary>
        /// AUX_TYPE_PERF_SESSIONINFO type
        /// </summary>
        AUX_TYPE_PERF_SESSIONINFO = 0x04,

        /// <summary>
        /// AUX_TYPE_PERF_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_MDB_SUCCESS = 0x07,

        /// <summary>
        /// AUX_TYPE_PERF_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_GC_SUCCESS = 0x08,

        /// <summary>
        /// AUX_TYPE_PERF_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_FAILURE = 0x09,

        /// <summary>
        /// AUX_TYPE_PERF_PROCESSINFO type
        /// </summary>
        AUX_TYPE_PERF_PROCESSINFO = 0x0B,

        /// <summary>
        /// AUX_TYPE_PERF_BG_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_MDB_SUCCESS = 0x0E,

        /// <summary>
        /// AUX_TYPE_PERF_BG_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_GC_SUCCESS = 0x0F,

        /// <summary>
        /// AUX_TYPE_PERF_BG_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_BG_FAILURE = 0x10,

        /// <summary>
        /// AUX_TYPE_PERF_FG_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_MDB_SUCCESS = 0x13,

        /// <summary>
        /// AUX_TYPE_PERF_FG_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_GC_SUCCESS = 0x14,

        /// <summary>
        /// AUX_TYPE_PERF_FG_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_FG_FAILURE = 0x15
    }
}
