namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The NamedPropInfo class.
    /// </summary>
    public class NamedPropInfo : LexicalBase
    {
        /// <summary>
        /// The PropertySet item in lexical definition.
        /// </summary>
        public AnnotatedGuid PropertySet;

        /// <summary>
        /// The flag variable.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// Initializes a new instance of the NamedPropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public NamedPropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse a NamedPropInfo instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A NamedPropInfo instance.</returns>
        public static LexicalBase ParseFrom(FastTransferStream stream)
        {
            if (DispidNamedPropInfo.Verify(stream))
            {
                return new DispidNamedPropInfo(stream);
            }
            else if (NameNamedPropInfo.Verify(stream))
            {
                return new NameNamedPropInfo(stream);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.PropertySet = new AnnotatedGuid(stream);
            int tmp = stream.ReadByte();
            if (tmp > 0)
            {
                this.Flag = (byte)tmp;
            }
        }
    }
}
