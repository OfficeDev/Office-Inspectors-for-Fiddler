namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.4.3 Warning Codes
    /// </summary>
    public enum WarningCodes : uint
    {
        /// <summary>
        /// A request involving multiple properties failed for one or more individual properties, while succeeding overall.
        /// </summary>
        ErrorsReturned = 0x00040380,

        /// <summary>
        /// A table operation succeeded, but the bookmark specified is no longer set at the same row as when it was last used.
        /// </summary>
        PositionChanged = 0x00040481,

        /// <summary>
        /// The row count returned by a table operation is approximate, not exact.
        /// </summary>
        ApproximateCount = 0x00040482,

        /// <summary>
        /// A move, copy, or delete operation succeeded for some messages but not for others.
        /// </summary>
        PartiallyComplete = 0x00040680,

        /// <summary>
        /// The operation succeeded, but there is more to do.
        /// </summary>
        SyncProgress = 0x00040820,

        /// <summary>
        /// In a change conflict, the client has the more recent change.
        /// </summary>
        NewerClientChange = 0x00040821,

        /// <summary>
        /// The version store is still active.
        /// </summary>
        IsamWarningRemainingVersions = 0x00000141,

        /// <summary>
        /// A seek on an index that is not unique yielded a unique key.
        /// </summary>
        IsamWarningUniqueKey = 0x00000159,

        /// <summary>
        /// A database column is a separated long value.
        /// </summary>
        IsamWarningSeparateLongValue = 0x00000196,

        /// <summary>
        /// The existing log file has a bad signature.
        /// </summary>
        IsamWarningExistingLogFileHasBadSignature = 0x0000022E,

        /// <summary>
        /// The existing log file is not contiguous.
        /// </summary>
        IsamWarningExistingLogFileIsNotContiguous = 0x0000022F,

        /// <summary>
        /// This error is for internal use only.
        /// </summary>
        IsamWarningSkipThisRecord = 0x00000234,

        /// <summary>
        /// The TargetInstance specified for the restore is running.
        /// </summary>
        IsamWarningTargetInstanceRunning = 0x00000242,

        /// <summary>
        /// The database corruption has been repaired.
        /// </summary>
        IsamWarningDatabaseRepaired = 0x00000253,

        /// <summary>
        /// The column has a null value.
        /// </summary>
        IsamWarningColumnNull = 0x000003EC,

        /// <summary>
        /// The buffer is too small for the data.
        /// </summary>
        IsamWarningBufferTruncated = 0x000003EE,

        /// <summary>
        /// The database is already attached.
        /// </summary>
        IsamWarningDatabaseAttached = 0x000003EF,

        /// <summary>
        /// The sort that is being attempted does not have enough memory to complete.
        /// </summary>
        IsamWarningSortOverflow = 0x000003F1,

        /// <summary>
        /// An exact match was not found during a seek.
        /// </summary>
        IsamWarningSeekNotEqual = 0x0000040F,

        /// <summary>
        /// There is no extended error information.
        /// </summary>
        IsamWarningNoErrorInfo = 0x0000041F,

        /// <summary>
        /// No idle activity occurred.
        /// </summary>
        IsamWarningNoIdleActivity = 0x00000422,

        /// <summary>
        /// There is a no write lock at transaction level 0.
        /// </summary>
        IsamWarningNoWriteLock = 0x0000042B,

        /// <summary>
        /// The column is set to a null value.
        /// </summary>
        IsamWarningColumnSetNull = 0x0000042C,

        /// <summary>
        /// An empty table was opened.
        /// </summary>
        IsamWarningTableEmpty = 0x00000515,

        /// <summary>
        /// The system cleanup has a cursor open on the table.
        /// </summary>
        IsamWarningTableInUseBySystem = 0x0000052F,

        /// <summary>
        /// The out-of-date index is required to be removed.
        /// </summary>
        IsamWarningCorruptIndexDeleted = 0x00000587,

        /// <summary>
        /// The maximum length is too large and has been truncated.
        /// </summary>
        IsamWarningColumnMaxTruncated = 0x000005E8,

        /// <summary>
        /// A binary large object (BLOB) value has been moved from the record into a separate storage of BLOBs.
        /// </summary>
        IsamWarningCopyLongValue = 0x000005F0,

        /// <summary>
        /// The column values were not returned because the corresponding column ID or itagSequence member from the JET_ENUMCOLUMNVALUE structure that was requested for enumeration was null.
        /// </summary>
        IsamWarningColumnSkipped = 0x000005FB,

        /// <summary>
        /// The column values were not returned because they could not be reconstructed from the existing data.
        /// </summary>
        IsamWarningColumnNotLocal = 0x000005FC,

        /// <summary>
        /// The existing column values were not requested for enumeration.
        /// </summary>
        IsamWarningColumnMoreTags = 0x000005FD,

        /// <summary>
        /// The column value was truncated at the requested size limit during enumeration.
        /// </summary>
        IsamWarningColumnTruncated = 0x000005FE,

        /// <summary>
        /// The column values exist but were not returned by the request.
        /// </summary>
        IsamWarningColumnPresent = 0x000005FF,

        /// <summary>
        /// The column value was returned in JET_COLUMNENUM as a result of the JET_bitEnumerateCompressOutput being set.
        /// </summary>
        IsamWarningColumnSingleValue = 0x00000600,

        /// <summary>
        /// The column value is set to the default value of the column.
        /// </summary>
        IsamWarningColumnDefault = 0x00000601,

        /// <summary>
        /// The data has changed.
        /// </summary>
        IsamWarningDataHasChanged = 0x0000064A,

        /// <summary>
        /// A new key is being used.
        /// </summary>
        IsamWarningKeyChanged = 0x00000652,

        /// <summary>
        /// The database file is read-only.
        /// </summary>
        IsamWarningFileOpenReadOnly = 0x00000715,

        /// <summary>
        /// The idle registry is full.
        /// </summary>
        IsamWarningIdleFull = 0x00000774,

        /// <summary>
        /// An online defragmentation was already running on the specified database.
        /// </summary>
        IsamWarningDefragAlreadyRunning = 0x000007D0,

        /// <summary>
        /// An online defragmentation is not running on the specified database.
        /// </summary>
        IsamWarningDefragNotRunning = 0x000007D1,

        /// <summary>
        /// A nonexistent callback function was unregistered.
        /// </summary>
        IsamWarningCallbackNotRegistered = 0x00000834,

        /// <summary>
        /// The function is not yet implemented.
        /// </summary>
        IsamWarningNotYetImplemented = 0xFFFFFFFF,

        /// <summary>
        /// Warning code returned by the NSPI server to indicate that the unbind call was successful.
        /// </summary>
        UnbindSuccess = 0x000000001,

        /// <summary>
        /// Warning code returned by the NSPI server to indicate that the NSPI bind call failed.
        /// </summary>
        UnbindFailure = 0x00000002,
    }
}
