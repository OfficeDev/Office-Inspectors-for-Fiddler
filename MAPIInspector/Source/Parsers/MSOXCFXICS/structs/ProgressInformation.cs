namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The ProgressInformation.
    /// 2.2.2.7 ProgressInformation Structure
    /// </summary>
    public class ProgressInformation : BaseStructure
    {
        /// <summary>
        /// An unsigned 16-bit value that contains a number that identifies the binary structure of the data that follows.
        /// </summary>
        public ushort Version;

        /// <summary>
        ///  The padding.
        /// </summary>
        public ushort Padding1;

        /// <summary>
        /// An unsigned 32-bit integer value that contains the total number of changes to FAI messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public uint FAIMessageCount;

        /// <summary>
        /// An unsigned 64-bit integer value that contains the size in bytes of all changes to FAI messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public ulong FAIMessageTotalSize;

        /// <summary>
        /// An unsigned 32-bit integer value that contains the total number of changes to normal messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public uint NormalMessageCount;

        /// <summary>
        /// The padding.
        /// </summary>
        public uint Padding2;

        /// <summary>
        /// An unsigned 64-bit integer value that contains the size in bytes of all changes to normal messages  that are scheduled for download during the current synchronization operation.
        /// </summary>
        public ulong NormalMessageTotalSize;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains ProgressInformation.</param>
        public void Parse(FastTransferStream stream)
        {
            this.Version = stream.ReadUInt16();
            this.Padding1 = stream.ReadUInt16();
            this.FAIMessageCount = stream.ReadUInt32();
            this.FAIMessageTotalSize = stream.ReadUInt64();
            this.NormalMessageCount = stream.ReadUInt32();
            this.Padding2 = stream.ReadUInt32();
            this.NormalMessageTotalSize = stream.ReadUInt64();
        }
    }
}
