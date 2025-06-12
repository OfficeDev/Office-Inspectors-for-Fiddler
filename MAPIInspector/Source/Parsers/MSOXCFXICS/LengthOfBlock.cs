namespace MAPIInspector.Parsers
{
    /// <summary>
    /// This class contains int value and byte array block.
    /// </summary>
    public class LengthOfBlock
    {
        /// <summary>
        /// Specifies the number of blocks
        /// </summary>
        public int TotalSize;

        /// <summary>
        /// Specifies block length
        /// </summary>
        public byte[] BlockSize;

        /// <summary>
        /// Initializes a new instance of the LengthOfBlock class
        /// </summary>
        /// <param name="totalSize">The total size</param>
        /// <param name="blockSize">The block size</param>
        public LengthOfBlock(int totalSize, byte[] blockSize)
        {
            this.TotalSize = totalSize;
            this.BlockSize = blockSize;
        }
    }
}
