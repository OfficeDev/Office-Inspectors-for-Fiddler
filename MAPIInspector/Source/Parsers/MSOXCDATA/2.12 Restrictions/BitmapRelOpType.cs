namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12.7.1 BitMaskRestriction Structure
    /// The enum type of BitmapRelOp.
    /// </summary>
    public enum BitmapRelOpType : byte
    {
        /// <summary>
        /// Perform a bitwise AND operation on the value of the Mask field with the value of the property PropTag field, and test for being equal to 0 (zero).
        /// </summary>
        BMR_EQZ = 0x00,

        /// <summary>
        /// Perform a bitwise AND operation on the value of the Mask field with the value of the property PropTag field, and test for not being equal to 0 (zero).
        /// </summary>
        BMR_NEZ = 0x01
    }
}
