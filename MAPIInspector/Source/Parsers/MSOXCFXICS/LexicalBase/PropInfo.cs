namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropInfo class.
    /// </summary>
    public class PropInfo : LexicalBase
    {
        /// <summary>
        /// The property id.
        /// </summary>
        public PidTagPropertyEnum PropID;

        /// <summary>
        /// The namedPropInfo in lexical definition.
        /// </summary>
        public NamedPropInfo NamedPropInfo;

        /// <summary>
        /// Initializes a new instance of the PropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public PropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized PropInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized PropInfo, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream;
        }

        /// <summary>
        /// Parse a PropInfo instance from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A PropInfo instance.</returns>
        public static LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new PropInfo(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.PropID = (PidTagPropertyEnum)stream.ReadUInt16();

            if ((ushort)this.PropID >= 0x8000)
            {
                this.NamedPropInfo = NamedPropInfo.ParseFrom(stream) as NamedPropInfo;
            }
        }
    }
}
