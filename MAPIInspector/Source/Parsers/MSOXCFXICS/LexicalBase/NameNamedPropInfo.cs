namespace MAPIInspector.Parsers
{
    using System;
    using System.Text;

    /// <summary>
    /// The NameNamedPropInfo class.
    /// </summary>
    public class NameNamedPropInfo : NamedPropInfo
    {
        /// <summary>
        /// The name of the NamedPropInfo.
        /// </summary>
        public MAPIString Name;

        /// <summary>
        /// Initializes a new instance of the NameNamedPropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public NameNamedPropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized NameNamedPropInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>If the stream's current position contains a serialized NameNamedPropInfo, return true, else false</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.Verify(0x01, Guid.Empty.ToByteArray().Length);
        }

        /// <summary>
        /// Parse a NameNamedPropInfo instance from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A NameNamedPropInfo instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new NameNamedPropInfo(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.Name = new MAPIString(Encoding.Unicode);
            this.Name.Parse(stream);
        }
    }
}
