namespace MAPIInspector.Parsers
{
    using System;

    #region 2.2.2.1 CN
    /// <summary>
    /// Represents CN structure contains a change number that identifies a version of a messaging object. 
    /// </summary>
    public class CN : BaseStructure
    {
        /// <summary>
        /// A 16-bit unsigned integer identifying the server replica in which the messaging object was last changed.
        /// </summary>
        public ushort ReplicaId;

        /// <summary>
        /// An unsigned 48-bit integer identifying the change to the messaging object.
        /// </summary>
        [BytesAttribute(6)]
        public ulong GlobalCounter;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains CN.</param>
        public void Parse(FastTransferStream stream)
        {
            this.ReplicaId = stream.ReadUInt16();
            this.GlobalCounter = BitConverter.ToUInt64(stream.ReadBlock(6), 0);
        }
    }

    #endregion
}
