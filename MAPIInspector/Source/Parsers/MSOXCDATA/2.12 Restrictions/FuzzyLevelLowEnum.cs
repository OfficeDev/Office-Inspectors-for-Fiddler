namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum of FuzzyLevelLow.
    /// [MS-OXCDATA] 2.12.4 Content Restriction Structures
    /// </summary>
    public enum FuzzyLevelLowEnum : ushort
    {
        /// <summary>
        /// The value stored in the TaggedValue field and the value of the column property tag match one another in their entirety.
        /// </summary>
        FL_FULLSTRING = 0x0000,

        /// <summary>
        /// The value stored in the TaggedValue field matches some portion of the value of the column property tag.
        /// </summary>
        FL_SUBSTRING = 0x0001,

        /// <summary>
        /// The value stored in the TaggedValue field matches a starting portion of the value of the column property tag.
        /// </summary>
        FL_PREFIX = 0x0002
    }
}
