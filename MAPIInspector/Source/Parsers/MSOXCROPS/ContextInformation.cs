namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The ContextInformation is used to save the related parameters during parsing.
    /// </summary>
    public class ContextInformation
    {
        /// <summary>
        /// Gets or sets RopId indicates the target ROP searched
        /// </summary>
        public RopIdType RopID { get; set; }

        /// <summary>
        /// Gets or sets handle indicates the target handle searched
        /// </summary>
        public uint Handle { get; set; }

        /// <summary>
        /// Gets or sets result searched for the target context information
        /// </summary>
        public object RelatedInformation { get; set; }
    }
}
