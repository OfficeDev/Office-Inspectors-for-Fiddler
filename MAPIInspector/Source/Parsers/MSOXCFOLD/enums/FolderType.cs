namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Section 2.2.1.2.1   RopCreateFolder ROP Request Buffer
    /// </summary>
    public enum FolderType : byte
    {
        /// <summary>
        /// Generic folder
        /// </summary>
        GenericFolder = 1,

        /// <summary>
        /// Search folder
        /// </summary>
        SearchFolder = 2
    }
}