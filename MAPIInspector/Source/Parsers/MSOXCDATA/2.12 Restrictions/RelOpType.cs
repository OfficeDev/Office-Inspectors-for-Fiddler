namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum type of RelOp.
    /// 2.12.6 Compare Properties Restriction Structures
    /// </summary>
    public enum RelOpType : byte
    {
        /// <summary>
        /// TRUE if the value of the object's property is less than the specified value.
        /// </summary>
        RelationalOperatorLessThan = 0x00,

        /// <summary>
        /// TRUE if the value of the object's property is less than or equal to the specified value.
        /// </summary>
        RelationalOperatorLessThanOrEqual = 0x01,

        /// <summary>
        /// TRUE if the value of the object's property value is greater than the specified value.
        /// </summary>
        RelationalOperatorGreaterThan = 0x02,

        /// <summary>
        /// TRUE if the value of the object's property value is greater than or equal to the specified value.
        /// </summary>
        RelationalOperatorGreaterThanOrEqual = 0x03,

        /// <summary>
        /// TRUE if the object's property value equals the specified value.
        /// </summary>
        RelationalOperatorEqual = 0x04,

        /// <summary>
        /// TRUE if the object's property value does not equal the specified value.
        /// </summary>
        RelationalOperatorNotEqual = 0x5,

        /// <summary>
        /// TRUE if the value of the object's property is in the DL membership of the specified property value.
        /// </summary>
        RelationalOperatorMemberOfDL = 0x64
    }
}
