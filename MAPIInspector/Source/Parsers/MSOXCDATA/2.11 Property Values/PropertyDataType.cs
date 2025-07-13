using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Section 2.11.1   Property Data Types
    /// Section 2.11.1.3   Multi-value Property Value Instances
    /// </summary>
    [Flags]
    public enum PropertyDataType : ushort
    {
        /// <summary>
        /// PtypUnspecified type
        /// </summary>
        PtypUnspecified = 0x0000,

        /// <summary>
        /// PtypNull type
        /// </summary>
        PtypNull = 0x0001,

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
        /// IN FUTURE: How to distinguish PtypObject from PtypEmbeddedTable since they share the same value
        /// </summary>
        PtypObject_Or_PtypEmbeddedTable = 0x000D,

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
        /// MultiValue flag
        /// </summary>
        MultiValue = 0x1000,

        /// <summary>
        /// MultivalueInstance flag
        /// </summary>
        MultivalueInstance = 0x2000,

        /// <summary>
        /// PtypMultipleInteger16 type
        /// </summary>
        PtypMultipleInteger16 = PtypInteger16 | MultiValue,

        /// <summary>
        /// PtypMultipleInteger32 type
        /// </summary>
        PtypMultipleInteger32 = PtypInteger32 | MultiValue,

        /// <summary>
        /// PtypMultipleFloating32 type
        /// </summary>
        PtypMultipleFloating32 = PtypFloating32 | MultiValue,

        /// <summary>
        /// PtypMultipleFloating64 type
        /// </summary>
        PtypMultipleFloating64 = PtypFloating64 | MultiValue,

        /// <summary>
        /// PtypMultipleCurrency type
        /// </summary>
        PtypMultipleCurrency = PtypCurrency | MultiValue,

        /// <summary>
        /// PtypMultipleFloatingTime type
        /// </summary>
        PtypMultipleFloatingTime = PtypFloatingTime | MultiValue,

        /// <summary>
        /// PtypMultipleInteger64 type
        /// </summary>
        PtypMultipleInteger64 = PtypInteger64 | MultiValue,

        /// <summary>
        /// PtypMultipleString type
        /// </summary>
        PtypMultipleString = PtypString | MultiValue,

        /// <summary>
        /// PtypMultipleString8 type
        /// </summary>
        PtypMultipleString8 = PtypString8 | MultiValue,

        /// <summary>
        /// PtypMultipleTime type
        /// </summary>
        PtypMultipleTime = PtypTime | MultiValue,

        /// <summary>
        /// PtypMultipleGuid type
        /// </summary>
        PtypMultipleGuid = PtypGuid | MultiValue,

        /// <summary>
        /// PtypMultipleBinary type
        /// </summary>
        PtypMultipleBinary = PtypBinary | MultiValue
    }
}