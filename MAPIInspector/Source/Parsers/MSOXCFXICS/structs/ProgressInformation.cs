using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The ProgressInformation.
    /// 2.2.2.7 ProgressInformation Structure
    /// </summary>
    public class ProgressInformation : Block
    {
        /// <summary>
        /// An unsigned 16-bit value that contains a number that identifies the binary structure of the data that follows.
        /// </summary>
        public BlockT<ushort> Version;

        /// <summary>
        ///  The padding.
        /// </summary>
        public BlockT<ushort> Padding1;

        /// <summary>
        /// An unsigned 32-bit integer value that contains the total number of changes to FAI messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public BlockT<uint> FAIMessageCount;

        /// <summary>
        /// An unsigned 64-bit integer value that contains the size in bytes of all changes to FAI messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public BlockT<ulong> FAIMessageTotalSize;

        /// <summary>
        /// An unsigned 32-bit integer value that contains the total number of changes to normal messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public BlockT<uint> NormalMessageCount;

        /// <summary>
        /// The padding.
        /// </summary>
        public BlockT<uint> Padding2;

        /// <summary>
        /// An unsigned 64-bit integer value that contains the size in bytes of all changes to normal messages  that are scheduled for download during the current synchronization operation.
        /// </summary>
        public BlockT<ulong> NormalMessageTotalSize;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            Version = ParseT<ushort>(parser);
            Padding1 = ParseT<ushort>(parser);
            FAIMessageCount = ParseT<uint>(parser);
            FAIMessageTotalSize = ParseT<ulong>(parser);
            NormalMessageCount = ParseT<uint>(parser);
            Padding2 = ParseT<uint>(parser);
            NormalMessageTotalSize = ParseT<ulong>(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("ProgressInformation");
            if (Version != null) AddChild(Version, $"Version:{Version.Data}");
            if (Padding1 != null) AddChild(Padding1, $"Padding1:{Padding1.Data}");
            if (FAIMessageCount != null) AddChild(FAIMessageCount, $"FAIMessageCount:{FAIMessageCount.Data}");
            if (FAIMessageTotalSize != null) AddChild(FAIMessageTotalSize, $"FAIMessageTotalSize:{FAIMessageTotalSize.Data}");
            if (NormalMessageCount != null) AddChild(NormalMessageCount, $"NormalMessageCount:{NormalMessageCount.Data}");
            if (Padding2 != null) AddChild(Padding2, $"Padding2:{Padding2.Data}");
            if (NormalMessageTotalSize != null) AddChild(NormalMessageTotalSize, $"NormalMessageTotalSize:{NormalMessageTotalSize.Data}");
        }
    }
}
