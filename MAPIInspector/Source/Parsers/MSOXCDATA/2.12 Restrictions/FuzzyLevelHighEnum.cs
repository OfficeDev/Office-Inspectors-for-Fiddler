namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum of FuzzyLevelHighEnum.
    /// [MS-OXCDATA] 2.12.4 Content Restriction Structures
    /// </summary>
    public enum FuzzyLevelHighEnum : ushort
    {
        /// <summary>
        /// The comparison does not consider case.
        /// </summary>
        FL_IGNORECASE = 0x00001,

        /// <summary>
        /// The comparison ignores Unicode-defined nonspacing characters such as diacritical marks.
        /// </summary>
        FL_IGNORENONSPACE = 0x0002,

        /// <summary>
        /// The comparison results in a match whenever possible, ignoring case and nonspacing characters.
        /// </summary>
        FL_LOOSE = 0x0004
    }
}
