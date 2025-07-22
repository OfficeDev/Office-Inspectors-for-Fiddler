namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.6.1 PropertyName Structure
    /// The enum of Kind.
    /// </summary>
    public enum KindEnum : byte
    {
        /// <summary>
        /// The property is identified by the LID field.
        /// </summary>
        LID = 0x00,

        /// <summary>
        /// The property is identified by the Name field.
        /// </summary>
        Name = 0x01,

        /// <summary>
        /// The property does not have an associated PropertyName field.
        /// </summary>
        NoPropertyName = 0xFF
    }
}
