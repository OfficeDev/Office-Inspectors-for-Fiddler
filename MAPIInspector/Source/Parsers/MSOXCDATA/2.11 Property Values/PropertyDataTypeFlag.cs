namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Section 2.11.1.3   Multi-value Property Value Instances
    /// </summary>
    public enum PropertyDataTypeFlag : ushort
    {
        /// <summary>
        /// MutltiValue flag
        /// </summary>
        MutltiValue = 0x1000,

        /// <summary>
        /// MultivalueInstance flag
        /// </summary>
        MultivalueInstance = 0x2000,
    }
}
