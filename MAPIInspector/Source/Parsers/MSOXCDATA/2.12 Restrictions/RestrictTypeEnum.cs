namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.12 Restrictions
    /// The enum value of restriction value.
    /// </summary>
    public enum RestrictTypeEnum : byte
    {
        /// <summary>
        /// Logical AND operation applied to a list of subrestrictions.
        /// </summary>
        AndRestriction = 0x00,

        /// <summary>
        /// Logical OR operation applied to a list of subrestrictions.
        /// </summary>
        OrRestriction = 0x01,

        /// <summary>
        /// Logical NOT operation applied to a subrestriction.
        /// </summary>
        NotRestriction = 0x02,

        /// <summary>
        /// Search a property value for specific content.
        /// </summary>
        ContentRestriction = 0x03,

        /// <summary>
        /// Compare a property value with a particular value.
        /// </summary>
        PropertyRestriction = 0x04,

        /// <summary>
        /// Compare the values of two properties.
        /// </summary>
        ComparePropertiesRestriction = 0x05,

        /// <summary>
        /// Perform a bitwise AND operation on a property value with a mask and compare that with 0 (zero).
        /// </summary>
        BitMaskRestriction = 0x06,

        /// <summary>
        /// Compare the size of a property value to a particular figure.
        /// </summary>
        SizeRestriction = 0x07,

        /// <summary>
        /// Test whether a property has a value.
        /// </summary>
        ExistRestriction = 0x08,

        /// <summary>
        /// Test whether any row of a message's attachment or recipient table satisfies a subrestriction.
        /// </summary>
        SubObjectRestriction = 0x09,

        /// <summary>
        /// Associates a comment with a subrestriction.
        /// </summary>
        CommentRestriction = 0x0A,

        /// <summary>
        /// Limits the number of matches returned from a subrestriction.
        /// </summary>
        CountRestriction = 0x0B
    }
}
