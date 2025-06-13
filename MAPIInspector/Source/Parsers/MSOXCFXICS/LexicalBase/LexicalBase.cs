namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Base class for lexical objects
    /// 2.2.4.1 FastTransfer stream lexical structure
    /// </summary>
    public abstract class LexicalBase
    {
        /// <summary>
        /// Initializes a new instance of the LexicalBase class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        protected LexicalBase(FastTransferStream stream)
        {
            this.Parse(stream);
        }

        /// <summary>
        /// Parse from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public virtual void Parse(FastTransferStream stream)
        {
        }

        public override string ToString() => string.Empty;
    }
}
