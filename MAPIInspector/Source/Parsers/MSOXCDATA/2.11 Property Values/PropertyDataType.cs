namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Section 2.11.1   Property Data Types
    /// </summary>
    public enum PropertyDataType : ushort
    {
        /// <summary>
        /// PtypInteger16 type
        /// </summary>
        PtypInteger16 = 0x0002,

        /// <summary>
        /// PtypInteger32 type
        /// </summary>
        PtypInteger32 = 0x0003,

        /// <summary>
        /// PtypFloating32 type
        /// </summary>
        PtypFloating32 = 0x0004,

        /// <summary>
        /// PtypFloating64 type
        /// </summary>
        PtypFloating64 = 0x0005,

        /// <summary>
        /// PtypCurrency type
        /// </summary>
        PtypCurrency = 0x0006,

        /// <summary>
        /// PtypFloatingTime type
        /// </summary>
        PtypFloatingTime = 0x0007,

        /// <summary>
        /// PtypErrorCode type
        /// </summary>
        PtypErrorCode = 0x000A,

        /// <summary>
        /// PtypBoolean type
        /// </summary>
        PtypBoolean = 0x000B,

        /// <summary>
        /// PtypInteger64 type
        /// </summary>
        PtypInteger64 = 0x0014,

        /// <summary>
        /// PtypString type
        /// </summary>
        PtypString = 0x001F,

        /// <summary>
        /// PtypString8 type
        /// </summary>
        PtypString8 = 0x001E,

        /// <summary>
        /// PtypTime type
        /// </summary>
        PtypTime = 0x0040,

        /// <summary>
        /// PtypGuid type
        /// </summary>
        PtypGuid = 0x0048,

        /// <summary>
        /// PtypServerId type
        /// </summary>
        PtypServerId = 0x00FB,

        /// <summary>
        /// PtypRestriction type
        /// </summary>
        PtypRestriction = 0x00FD,

        /// <summary>
        /// PtypRuleAction type
        /// </summary>
        PtypRuleAction = 0x00FE,

        /// <summary>
        /// PtypBinary type
        /// </summary>
        PtypBinary = 0x0102,

        /// <summary>
        /// PtypMultipleInteger16 type
        /// </summary>
        PtypMultipleInteger16 = 0x1002,

        /// <summary>
        /// PtypMultipleInteger32 type
        /// </summary>
        PtypMultipleInteger32 = 0x1003,

        /// <summary>
        /// PtypMultipleFloating32 type
        /// </summary>
        PtypMultipleFloating32 = 0x1004,

        /// <summary>
        /// PtypMultipleFloating64 type
        /// </summary>
        PtypMultipleFloating64 = 0x1005,

        /// <summary>
        /// PtypMultipleCurrency type
        /// </summary>
        PtypMultipleCurrency = 0x1006,

        /// <summary>
        /// PtypMultipleFloatingTime type
        /// </summary>
        PtypMultipleFloatingTime = 0x1007,

        /// <summary>
        /// PtypMultipleInteger64 type
        /// </summary>
        PtypMultipleInteger64 = 0x1014,

        /// <summary>
        /// PtypMultipleString type
        /// </summary>
        PtypMultipleString = 0x101F,

        /// <summary>
        /// PtypMultipleString8 type
        /// </summary>
        PtypMultipleString8 = 0x101E,

        /// <summary>
        /// PtypMultipleTime type
        /// </summary>
        PtypMultipleTime = 0x1040,

        /// <summary>
        /// PtypMultipleGuid type
        /// </summary>
        PtypMultipleGuid = 0x1048,

        /// <summary>
        /// PtypMultipleBinary type
        /// </summary>
        PtypMultipleBinary = 0x1102,

        /// <summary>
        /// PtypUnspecified type
        /// </summary>
        PtypUnspecified = 0x0000,

        /// <summary>
        /// PtypNull type
        /// </summary>
        PtypNull = 0x0001,

        /// <summary>
        /// IN FUTURE: How to distinguish PtypObject from PtypEmbeddedTable since they share the same value
        /// </summary>
        PtypObject_Or_PtypEmbeddedTable = 0x000D,
    }
}
