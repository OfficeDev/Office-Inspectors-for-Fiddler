using System;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Supply help functions for lexical enumerations.
    /// </summary>
    public class LexicalTypeHelper
    {
        /// <summary>
        /// Contains fixedPropTypes.
        /// </summary>
        private static List<PropertyDataType> fixedTypes;

        /// <summary>
        /// Contains varPropTypes.
        /// </summary>
        private static List<PropertyDataType> varTypes;

        /// <summary>
        /// Contains mvPropTypes.
        /// </summary>
        private static List<PropertyDataType> mVTypes;

        /// <summary>
        /// Contains CodePageTypes.
        /// </summary>
        private static List<CodePageType> codePageTypes;

        /// <summary>
        /// Contains MetaProperty Ids.
        /// </summary>
        private static List<PidTagPropertyEnum> metaPropIds;

        /// <summary>
        /// Initializes static members of the LexicalTypeHelper class.
        /// </summary>
        static LexicalTypeHelper()
        {
            fixedTypes = new List<PropertyDataType>
            {
                PropertyDataType.PtypInteger16,
                PropertyDataType.PtypInteger32,
                PropertyDataType.PtypFloating32,
                PropertyDataType.PtypFloating64,
                PropertyDataType.PtypCurrency,
                PropertyDataType.PtypFloatingTime,
                PropertyDataType.PtypErrorCode,
                PropertyDataType.PtypBoolean,
                PropertyDataType.PtypInteger64,
                PropertyDataType.PtypTime,
                PropertyDataType.PtypGuid
            };

            varTypes = new List<PropertyDataType>
            {
                PropertyDataType.PtypString,
                PropertyDataType.PtypString8,
                PropertyDataType.PtypBinary,
                PropertyDataType.PtypServerId,
                PropertyDataType.PtypObject_Or_PtypEmbeddedTable
            };

            mVTypes = new List<PropertyDataType>
            {
                PropertyDataType.PtypMultipleInteger16,
                PropertyDataType.PtypMultipleInteger32,
                PropertyDataType.PtypMultipleFloating32,
                PropertyDataType.PtypMultipleFloating64,
                PropertyDataType.PtypMultipleCurrency,
                PropertyDataType.PtypMultipleFloatingTime,
                PropertyDataType.PtypMultipleInteger64,
                PropertyDataType.PtypMultipleString,
                PropertyDataType.PtypMultipleString8,
                PropertyDataType.PtypMultipleTime,
                PropertyDataType.PtypMultipleGuid,
                PropertyDataType.PtypMultipleBinary
            };

            codePageTypes = new List<CodePageType>
            {
                CodePageType.PtypCodePageUnicode,
                CodePageType.PtypCodePageUnicodeBigendian,
                CodePageType.PtypCodePageWesternEuropean
            };

            metaPropIds = new List<PidTagPropertyEnum>
            {
                PidTagPropertyEnum.MetaTagFXDelProp,
                PidTagPropertyEnum.MetaTagEcWarning,
                PidTagPropertyEnum.MetaTagNewFXFolder,
                PidTagPropertyEnum.MetaTagIncrSyncGroupId,
                PidTagPropertyEnum.MetaTagIncrementalSyncMessagePartial,
                PidTagPropertyEnum.MetaTagDnPrefix
            };
        }

        /// <summary>
        /// Indicate whether a PropertyDataType is a multi-valued property type.
        /// </summary>
        /// <param name="type">A PropertyDataType.</param>
        /// <returns>If the PropertyDataType is a multi-value type return true, else false.</returns>
        public static bool IsMVType(PropertyDataType type)
        {
            return mVTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a PropertyDataType is either PtypString, PtypString8 or PtypBinary, PtypServerId, or PtypObject. 
        /// </summary>
        /// <param name="type">A PropertyDataType.</param>
        /// <returns>If the PropertyDataType is a either PtypString, PtypString8 or PtypBinary, PtypServerId, or PtypObject return true, else false.</returns>
        public static bool IsVarType(PropertyDataType type)
        {
            return varTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a property type value of any type that has a fixed length.
        /// </summary>
        /// <param name="type">A property type.</param>
        /// <returns>If a property type value of any type that has a fixed length, return true , else return false.</returns>
        public static bool IsFixedType(PropertyDataType type)
        {
            return fixedTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a PropertyID is a Meta property ID.
        /// </summary>
        /// <param name="id">A PidTagPropertyEnum value.</param>
        /// <returns>If a PropertyID is a Meta property ID, return true, else return false.</returns>
        public static bool IsMetaPropertyID(PidTagPropertyEnum id)
        {
            return metaPropIds.Contains(id);
        }

        /// <summary>
        /// Indicate whether a PropertyDataType value is a codePage property type. 
        /// </summary>
        /// <param name="type">A PropertyDataType value.</param>
        /// <returns>If the PropertyDataType is a either PtypCodePageUnicode, PtypCodePageUnicodeBigendian or PtypCodePageWesternEuropean return true, else false.</returns>
        public static bool IsCodePageType(PropertyDataType type)
        {
            foreach (var t in Enum.GetValues(typeof(CodePageType)))
            {
                if (type == (PropertyDataType)t)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
