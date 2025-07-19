namespace BlockParser
{
    public partial class Block
    {
        /// <summary>
        /// CreateBlock an empty non parsing block
        /// </summary>
        /// <returns>New instance of ScratchBlock</returns>
        public static Block Create() => new ScratchBlock();

        /// <summary>
        /// CreateBlock a block with specified size, offset, and formatted text
        /// </summary>
        /// <param name="size">Size of the block</param>
        /// <param name="offset">Offset of the block</param>
        /// <param name="text">The label text for the new node.</param>
        /// <returns>Newly created block with specified parameters</returns>
        public static Block Create(long size, long offset, string text)
        {
            var ret = Create();
            ret.Size = size;
            ret.Offset = offset;
            ret.Text = text;
            return ret;
        }

        /// <summary>
        /// CreateBlock a block with formatted text, such as a header or label
        /// </summary>
        /// <param name="text">The label text for the new node.</param>
        /// <returns>Newly created block with formatted text</returns>
        public static Block Create(string text)
        {
            var ret = Create();
            ret.Text = text;
            return ret;
        }

        /// <summary>
        /// Creates a block containing the specified data, size, and offset.
        /// </summary>
        /// <typeparam name="T">The value type to store in the block.</typeparam>
        /// <param name="data">The data to store in the block.</param>
        /// <param name="size">The size of the block in bytes.</param>
        /// <param name="offset">The offset of the block within the parent structure or stream.</param>
        /// <returns>A new <see cref="BlockT{T}"/> instance containing the provided data, size, and offset.</returns>
        public static BlockT<T> CreateBlock<T>(T data, long size, long offset) where T : struct
        {
            var ret = new BlockT<T>(data, size, offset)
            {
                Parsed = true
            };
            return ret;
        }
    }
}
