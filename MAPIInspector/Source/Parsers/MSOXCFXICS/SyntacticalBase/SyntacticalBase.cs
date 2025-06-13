namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Base class for all syntactical object.
    /// 2.2.4.2 FastTransfer stream syntactical structure
    /// </summary>
    public abstract class SyntacticalBase
    {
        /// <summary>
        /// The size of an MetaTag value.
        /// </summary>
        protected const int MetaLength = 4;

        /// <summary>
        /// Previous position.
        /// </summary>
        private long previousPosition;

        /// <summary>
        /// Initializes a new instance of the SyntacticalBase class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        protected SyntacticalBase(FastTransferStream stream)
        {
            this.previousPosition = stream.Position;

            if (stream != null && stream.Length > 0)
            {
                this.Parse(stream);
            }
        }

        /// <summary>
        /// Parse object from memory stream,
        /// </summary>
        /// <param name="stream">Stream contains the serialized object</param>
        public abstract void Parse(FastTransferStream stream);

        public override string ToString() => string.Empty;
    }
}
