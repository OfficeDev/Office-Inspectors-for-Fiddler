namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1.1 COUNT Data Type Values
    /// The enum of the Ptyp data type Count wide : 16 bits wide or 32 bits wide.
    /// </summary>
    public enum CountWideEnum : uint
    {
        /// <summary>
        /// The count width is two bytes
        /// </summary>
        twoBytes = 2,

        /// <summary>
        /// The count width is four bytes
        /// </summary>
        fourBytes = 4
    }
}
