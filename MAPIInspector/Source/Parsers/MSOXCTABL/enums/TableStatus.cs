namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.1.3 TableStatus
    /// </summary>
    public enum TableStatus : byte
    {
        /// <summary>
        /// No operations are in progress.
        /// </summary>
        TBLSTAT_COMPLETE = 0x00,

        /// <summary>
        /// A RopSortTable ROP is in progress.
        /// </summary>
        TBLSTAT_SORTING = 0x09,

        /// <summary>
        /// An error occurred during a RopSortTable ROP
        /// </summary>
        TBLSTAT_SORT_ERROR = 0x0A,

        /// <summary>
        /// A RopSetColumns ROP is in progress.
        /// </summary>
        TBLSTAT_SETTING_COLS = 0x0B,

        /// <summary>
        /// An error occurred during a RopSetColumns ROP
        /// </summary>
        TBLSTAT_SETCOL_ERROR = 0x0D,

        /// <summary>
        /// A RopRestrict ROP is in progress.
        /// </summary>
        TBLSTAT_RESTRICTING = 0x0E,

        /// <summary>
        /// An error occurred during a RopRestrict ROP.
        /// </summary>
        TBLSTAT_RESTRICT_ERROR = 0x0F
    }
}
