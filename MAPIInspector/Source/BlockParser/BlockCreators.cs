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
        /// <param name="format">Format string for the block text</param>
        /// <param name="args">Arguments to format the block text</param>
        /// <returns>Newly created block with specified parameters</returns>
        public static Block Create(long size, long offset, string format, params object[] args)
        {
            var ret = Create();
            ret.Size = size;
            ret.Offset = offset;
            ret.SetText(format, args);
            return ret;
        }

        /// <summary>
        /// CreateBlock a block with formatted text, such as a header or label
        /// </summary>
        /// <param name="format">Format string for the block text</param>
        /// <param name="args">Arguments to format the block text</param>
        /// <returns>Newly created block with formatted text</returns>
        public static Block Create(string format, params object[] args)
        {
            var ret = Create();
            ret.SetText(format, args);
            return ret;
        }

        public static BlockT<T> CreateBlock<T>(T data, long size, long offset) where T : struct
        {
            var ret = new BlockT<T>(data, size, offset)
            {
                Parsed = true
            };
            return ret;
        }

        public static BlockStringW CreateStringW(string data, int size, int offset)
        {
            var ret = new BlockStringW
            {
                Parsed = true,
                EnableJunk = false,
                data = data
            };
            ret.SetText(data);
            ret.Size = size;
            ret.Offset = offset;
            return ret;
        }
    }
}
