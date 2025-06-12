namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represents a command in GLOBSET.
    /// 2.2.2.6 GLOBSET Structure
    /// </summary>
    public class Command : BaseStructure
    {
        /// <summary>
        /// Parse from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public virtual void Parse(FastTransferStream stream)
        {
        }
    }
}
