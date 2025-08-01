namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCRPC] 2.2.2.2 AUX_HEADER Structure
    /// The enum type corresponding auxiliary block structure that follows the AUX_HEADER structure when the Version field is AUX_VERSION_1. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum AuxiliaryBlockType_1 : byte
    {
        /// <summary>
        /// AUX_TYPE_PERF_REQUESTID type
        /// </summary>
        AUX_TYPE_PERF_REQUESTID = 0x01,

        /// <summary>
        /// AUX_TYPE_PERF_CLIENTINFO type
        /// </summary>
        AUX_TYPE_PERF_CLIENTINFO = 0x02,

        /// <summary>
        /// AUX_TYPE_PERF_SERVERINFO type
        /// </summary>
        AUX_TYPE_PERF_SERVERINFO = 0x03,

        /// <summary>
        /// AUX_TYPE_PERF_SESSIONINFO type
        /// </summary>
        AUX_TYPE_PERF_SESSIONINFO = 0x04,

        /// <summary>
        /// AUX_TYPE_PERF_DEFMDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_DEFMDB_SUCCESS = 0x05,

        /// <summary>
        /// AUX_TYPE_PERF_DEFGC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_DEFGC_SUCCESS = 0x06,

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
        /// AUX_TYPE_CLIENT_CONTROL type
        /// </summary>
        AUX_TYPE_CLIENT_CONTROL = 0x0A,

        /// <summary>
        /// AUX_TYPE_PERF_PROCESSINFO type
        /// </summary>
        AUX_TYPE_PERF_PROCESSINFO = 0x0B,

        /// <summary>
        /// AUX_TYPE_PERF_BG_DEFMDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_DEFMDB_SUCCESS = 0x0C,

        /// <summary>
        /// AUX_TYPE_PERF_BG_DEFGC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_DEFGC_SUCCESS = 0x0D,

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
        /// AUX_TYPE_PERF_FG_DEFMDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_DEFMDB_SUCCESS = 0x11,

        /// <summary>
        /// AUX_TYPE_PERF_FG_DEFGC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_DEFGC_SUCCESS = 0x12,

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
        AUX_TYPE_PERF_FG_FAILURE = 0x15,

        /// <summary>
        /// AUX_TYPE_OSVERSIONINFO type
        /// </summary>
        AUX_TYPE_OSVERSIONINFO = 0x16,

        /// <summary>
        /// AUX_TYPE_EXORGINFO type
        /// </summary>
        AUX_TYPE_EXORGINFO = 0x17,

        /// <summary>
        /// AUX_TYPE_PERF_ACCOUNTINFO type
        /// </summary>
        AUX_TYPE_PERF_ACCOUNTINFO = 0x18,

        /// <summary>
        /// AUX_TYPE_ENDPOINT_CAPABILITIES type
        /// </summary>
        AUX_TYPE_ENDPOINT_CAPABILITIES = 0x48,

        /// <summary>
        /// AUX_TYPE_EXCEPTION_TRACE type
        /// </summary>
        AUX_TYPE_EXCEPTION_TRACE = 0x49,

        /// <summary>
        /// AUX_CLIENT_CONNECTION_INFO type
        /// </summary>
        AUX_CLIENT_CONNECTION_INFO = 0x4A,

        /// <summary>
        /// AUX_SERVER_SESSION_INFO type
        /// </summary>
        AUX_SERVER_SESSION_INFO = 0x4B,

        /// <summary>
        /// AUX_PROTOCOL_DEVICE_IDENTIFICATION type
        /// </summary>
        AUX_PROTOCOL_DEVICE_IDENTIFICATION = 0x4E
    }
}
