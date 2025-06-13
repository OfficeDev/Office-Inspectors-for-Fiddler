namespace MAPIInspector.Parsers
{
    using System;
    /// <summary>
    /// The structure of LongTermId
    /// 2.2.1.3.1 LongTermID Structure
    /// </summary>
    public class LongTermId : SyntacticalBase
    {
        /// <summary>
        /// A 128-bit unsigned integer identifying a Store object.
        /// </summary>
        public Guid DatabaseGuid;

        /// <summary>
        /// An unsigned 48-bit integer identifying the folder within its Store object.
        /// </summary>
        [BytesAttribute(6)]
        public ulong GlobalCounter;

        /// <summary>
        /// An UShort.
        /// </summary>
        public ushort Pad;

        /// <summary>
        /// Initializes a new instance of the LongTermId class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public LongTermId(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse the LongTermId structure
        /// </summary>
        /// <param name="stream">The stream to parse</param>
        public override void Parse(FastTransferStream stream)
        {
            this.DatabaseGuid = stream.ReadGuid();
            this.GlobalCounter = BitConverter.ToUInt64(stream.ReadBlock(6), 0);
            this.Pad = stream.ReadUInt16();
        }
    }
}
