namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.7 TypedString Structure
    /// The enum value of StringType
    /// </summary>
    public enum StringTypeEnum : byte
    {
        /// <summary>
        /// No string is present.
        /// </summary>
        NoPresent = 0x00,

        /// <summary>
        /// The string is empty.
        /// </summary>
        Empty = 0x01,

        /// <summary>
        /// Null-terminated 8-bit character string.
        /// </summary>
        CharacterString = 0x02,

        /// <summary>
        /// Null-terminated reduced Unicode character string.
        /// </summary>
        ReducedUnicodeCharacterString = 0x03,

        /// <summary>
        /// Null-terminated Unicode character string.
        /// </summary>
        UnicodeCharacterString = 0x04
    }
}
