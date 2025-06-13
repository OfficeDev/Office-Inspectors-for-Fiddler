namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// Represents a NamedPropInfo has a Dispid.
    /// </summary>
    public class DispidNamedPropInfo : NamedPropInfo
    {
        /// <summary>
        /// The Dispid in lexical definition.
        /// </summary>
        public AnnotatedUint Dispid;

        /// <summary>
        /// Initializes a new instance of the DispidNamedPropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public DispidNamedPropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized DispidNamedPropInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>If the stream's current position contains a serialized DispidNamedPropInfo, return true, else false</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.Verify(0x00, Guid.Empty.ToByteArray().Length);
        }

        /// <summary>
        /// Parse a DispidNamedPropInfo instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A DispidNamedPropInfo instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new DispidNamedPropInfo(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.Dispid = new AnnotatedUint(stream);
            var namedProp = NamedProperty.Lookup(this.PropertySet.value, Dispid.value);
            if (namedProp != null)
                Dispid.ParsedValue = $"{namedProp.Name} = 0x{Dispid.value:X4}";
            else
                Dispid.ParsedValue = $"0x{Dispid.value:X4}";
        }
    }
}
